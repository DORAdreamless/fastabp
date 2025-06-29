using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HB.AbpFundation.DataSeeds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class TypeScriptApiGenerator
    {
        private readonly HashSet<string> _importedTypes = new HashSet<string>();
        private readonly StringBuilder _importsSection = new StringBuilder();
        private readonly StringBuilder _apiClassesSection = new StringBuilder();

        public string GenerateApiClasses(Type[] controllerTypes)
        {
            _importsSection.AppendLine("// Auto-generated API client");
            _importsSection.AppendLine("import api,{  sseExport } from '@/api/index';");
            _importsSection.AppendLine("import qs from 'qs';");
            _importsSection.AppendLine();

            foreach (var controllerType in controllerTypes)
            {
                GenerateApiClass(controllerType);
            }

            return _importsSection.ToString() + _apiClassesSection.ToString();
        }

        private void GenerateApiClass(Type controllerType)
        {
            var controllerName = GetControllerName(controllerType);
            var routeAttribute = controllerType.GetCustomAttribute<RouteAttribute>();
            var baseRoute = routeAttribute?.Template ?? GetDefaultRoute(controllerType, controllerName);

            var sb = new StringBuilder();

            // 添加类注释
            sb.AppendLine($"/** {controllerType.FullName} */");
            sb.AppendLine($"export class {controllerName}Api {{");
            sb.AppendLine();

            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                     .Where(m => m.GetCustomAttribute<NonActionAttribute>() == null);

            foreach (var method in methods)
            {
                GenerateApiMethod(sb, method, baseRoute, controllerName);
                sb.AppendLine();
            }

            sb.AppendLine("}");
            _apiClassesSection.AppendLine(sb.ToString());
        }

        private string GetControllerName(Type controllerType)
        {
            var name = controllerType.Name;
            return name.EndsWith("Controller") ? name[..^10] : name;
        }

        private string GetDefaultRoute(Type controllerType, string controllerName)
        {
            var namespaceParts = controllerType.Namespace?.Split('.') ?? Array.Empty<string>();
            var moduleName = namespaceParts.Length > 2 ? namespaceParts[^2] : "api";
            return $"/api/{moduleName}/{controllerName}";
        }

        private void GenerateApiMethod(StringBuilder sb, MethodInfo method, string baseRoute, string controllerName)
        {
            var methodName = GetMethodName(method);
            var httpMethod = GetHttpMethod(method);
            var parameters = method.GetParameters();
            var returnType = GetTypeScriptReturnType(method.ReturnType);

            // 处理路由
            var routeAttribute = method.GetCustomAttribute<RouteAttribute>();
            var actionRoute = routeAttribute?.Template ?? methodName;
            var fullRoute = CombineRoutes(baseRoute, actionRoute);
            fullRoute = "/"+fullRoute.Replace("[controller]/[action]", controllerName);

            // 处理参数
            var (paramDefinition, requestParams) = ProcessMethodParameters(parameters, httpMethod, fullRoute);

            // 方法注释
            sb.AppendLine($"\t/** {method.Name} */");

            // 方法签名
            sb.AppendLine($"\tstatic {ToCamelCase(methodName)}({paramDefinition}): {returnType} {{");

            // 方法体
            if (method.ReturnType == typeof(void) || method.ReturnType.IsGenericType &&
                method.ReturnType.GetGenericTypeDefinition() == typeof(Task) &&
                method.ReturnType.GetGenericArguments().Length == 0)
            {
                sb.AppendLine($"\t\treturn api.{httpMethod}(`{fullRoute}`{requestParams});");
            }
            else if (method.Name.Contains("Export", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($"\t\treturn sseExport(`{fullRoute}`{requestParams});");
            }
            else if (httpMethod=="delete"&& requestParams.IndexOf("qs.stringify")>0)
            {
                sb.AppendLine($"\t\treturn api.{httpMethod}(`{fullRoute}?`{requestParams});");
            }
            else
            {
                sb.AppendLine($"\t\treturn api.{httpMethod}(`{fullRoute}`{requestParams});");
            }

            sb.Append("\t}");
        }

        private (string paramDefinition, string requestParams) ProcessMethodParameters(ParameterInfo[] parameters, string httpMethod, string fullRoute)
        {
            if (parameters.Length == 0) return ("", "");

            var paramDefinition = new StringBuilder();
            var requestParams = new StringBuilder();
            var hasRouteParams = fullRoute.Contains('{');

            // 处理路由参数
            if (hasRouteParams)
            {
                var routeParamNames = GetRouteParameterNames(fullRoute);
                foreach (var paramName in routeParamNames)
                {
                    var param = parameters.FirstOrDefault(p => p.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase));
                    if (param != null)
                    {
                        paramDefinition.Append($"{param.Name}: {GetTypeScriptTypeName(param.ParameterType)}, ");
                    }
                }
            }

            // 处理查询参数和请求体
            var remainingParams = parameters.Where(p => !fullRoute.Contains($"{{{p.Name}}}")).ToArray();
            if (remainingParams.Length > 0)
            {
                var firstParam = remainingParams[0];

                if (httpMethod == "get" || httpMethod == "delete")
                {
                    if (!MustExportType(firstParam.ParameterType))
                    {
                        // 简单类型直接作为查询参数
                        paramDefinition.Append($"{firstParam.Name}: {GetTypeScriptTypeName(firstParam.ParameterType)}");
                        requestParams.Append($" + qs.stringify({{{firstParam.Name}: {firstParam.Name}}})");
                    }
                    else
                    {
                        // 复杂类型使用Partial
                        AddTypeImport(firstParam.ParameterType);
                        paramDefinition.Append($"data: Partial<{GetTypeScriptTypeName(firstParam.ParameterType)}>");
                        requestParams.Append(" + (data ? `?${qs.stringify(data)}` : '')");
                    }
                }
                else
                {
                    // POST/PUT/PATCH 使用请求体
                    AddTypeImport(firstParam.ParameterType);
                    paramDefinition.Append($"data: Partial<{GetTypeScriptTypeName(firstParam.ParameterType)}>");
                    requestParams.Append(", data");
                }
            }

            return (paramDefinition.ToString().TrimEnd(' ', ','), requestParams.ToString());
        }

        private IEnumerable<string> GetRouteParameterNames(string route)
        {
            var start = route.IndexOf('{');
            while (start >= 0)
            {
                var end = route.IndexOf('}', start);
                if (end < 0) break;

                yield return route.Substring(start + 1, end - start - 1);
                start = route.IndexOf('{', end);
            }
        }

        private string CombineRoutes(string baseRoute, string actionRoute)
        {
            if (string.IsNullOrEmpty(actionRoute)) return baseRoute;
            if (actionRoute.StartsWith("/")) return actionRoute;

            return baseRoute.EndsWith("/") ? $"{baseRoute}{actionRoute}" : $"{baseRoute}/{actionRoute}";
        }

        private string GetMethodName(MethodInfo method)
        {
            var name = method.Name;
            return name.EndsWith("Async") ? name[..^5] : name;
        }

        private string GetTypeScriptReturnType(Type returnType)
        {
            if (returnType == typeof(Task))
            {
                return "Promise<void>";
            }

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = returnType.GetGenericArguments()[0];
                AddTypeImport(resultType);
                return $"Promise<{GetTypeScriptTypeName(resultType)}>";
            }

            AddTypeImport(returnType);
            return $"Promise<{GetTypeScriptTypeName(returnType)}>";
        }

        private string GetTypeScriptTypeName(Type type)
        {
            if (type == null) return "any";

            // 处理Nullable<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"{GetInternalTypeScriptTypeName(type.GetGenericArguments()[0])} | null";
            }

            return GetInternalTypeScriptTypeName(type);
        }

        private string GetInternalTypeScriptTypeName(Type type)
        {
            if (type == null) return "any";

            // 处理数组/集合类型
            if (type.IsArray)
            {
                return $"Array<{GetInternalTypeScriptTypeName(type.GetElementType())}>";
            }

            // 处理泛型类型
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                // 处理常见集合类型
                if (genericTypeDefinition == typeof(List<>) ||
                    genericTypeDefinition == typeof(IList<>) ||
                    genericTypeDefinition == typeof(IReadOnlyList<>))
                {
                    return $"Array<{GetInternalTypeScriptTypeName(type.GenericTypeArguments[0])}>";
                }

                // 处理字典类型
                if (genericTypeDefinition == typeof(Dictionary<,>) ||
                    genericTypeDefinition == typeof(IDictionary<,>))
                {
                    return $"Record<{GetInternalTypeScriptTypeName(type.GenericTypeArguments[0])}, " +
                           $"{GetInternalTypeScriptTypeName(type.GenericTypeArguments[1])}>";
                }

                // 处理其他泛型类型
                var typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
                var args = string.Join(", ", type.GenericTypeArguments.Select(GetInternalTypeScriptTypeName));
                return $"{typeName}<{args}>";
            }

            // 处理基础类型
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String:
                case TypeCode.Char:
                    return "string";

                case TypeCode.Boolean:
                    return "boolean";

                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "number";

                case TypeCode.DateTime:
                    return "string";

                case TypeCode.Object:
                    // 处理特殊对象类型
                    if (type == typeof(Guid))
                        return "string";
                    if (type == typeof(TimeSpan))
                        return "string";
                    if (type == typeof(DateTime))
                        return "string";
                    if (type == typeof(DateOnly))
                        return "string";
                    return type.Name;

                default:
                    return "any";
            }
        }

        private void AddTypeImport(Type type)
        {
            if (type == null || !MustExportType(type)) return;

            // 处理泛型类型
            if (type.IsGenericType)
            {
                foreach (var genericArg in type.GetGenericArguments())
                {
                    AddTypeImport(genericArg);
                }

                type = type.GetGenericTypeDefinition();
            }

            var typeName = GetInternalTypeScriptTypeName(type);
            typeName = typeName.Replace("<>", "");
            if (_importedTypes.Contains(typeName)) return;

           
            _importedTypes.Add(typeName);
            _importsSection.AppendLine($"import {{ {typeName} }} from './types';");
        }

        private bool MustExportType(Type type)
        {
            // 获取原始类型（对于泛型类型获取其泛型定义）
            Type originalType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            if (!originalType.IsClass)
            {
                return false;
            }
            if (originalType.Namespace == null)
            {
                return false;
            }
            if (originalType.Namespace.StartsWith("HB."))
            {
                return true;
            }
            if (originalType.Namespace.StartsWith("Volo."))
            {
                return true;
            }
            if(originalType.Namespace== "CodeSmithCore")
            {
                return true;
            }
            return false;
        }

        private string GetHttpMethod(MethodInfo method)
        {
            var httpMethodAttr = method.GetCustomAttributes()
                .FirstOrDefault(a => a.GetType().Name.StartsWith("Http")) as dynamic;

            if (httpMethodAttr != null)
            {
                return httpMethodAttr.HttpMethods[0].ToLower();
            }

            // 根据方法名猜测HTTP方法
            var methodName = method.Name.ToLower();
            if (methodName.StartsWith("get")) return "get";
            if (methodName.StartsWith("create") || methodName.StartsWith("add")) return "post";
            if (methodName.StartsWith("update") || methodName.StartsWith("modify")) return "put";
            if (methodName.StartsWith("delete") || methodName.StartsWith("remove")) return "delete";

            return "post";
        }

        private string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToLowerInvariant(str[0]) + str[1..];
        }
    }
}
