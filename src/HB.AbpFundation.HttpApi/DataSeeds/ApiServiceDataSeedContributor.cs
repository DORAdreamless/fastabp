using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.DataSeeds
{
    public class ApiServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        /// <summary>
        /// 是否需要导出为type.d.ts
        /// </summary>
        /// <param name="type">要检查的类型</param>
        /// <returns>是否需要导出</returns>
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
            if (originalType.Namespace == "CodeSmithCore")
            {
                return true;
            }
            return false;
        }

        private List<Type> GetRelatedTypes(Type type)
        {
            var types = new List<Type>();
            if (type == null)
                return types;

            // 避免循环引用导致的无限递归，使用原始类型进行比较
            var visitedTypes = new HashSet<Type>();
            return GetRelatedTypesInternal(type, types, visitedTypes);
        }

        private List<Type> GetRelatedTypesInternal(Type type, List<Type> types, HashSet<Type> visitedTypes)
        {
            if (type == null)
                return types;

            // 获取原始类型用于比较
            Type originalType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            if (visitedTypes.Contains(originalType))
                return types;

            visitedTypes.Add(originalType);

            if (MustExportType(originalType))
            {
                // 存储原始类型而不是具体泛型实例
                types.Add(originalType);
            }

            // 处理基类
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                GetRelatedTypesInternal(type.BaseType, types, visitedTypes);
            }

            // 处理泛型参数
            if (type.IsGenericType)
            {
                foreach (var genericArg in type.GetGenericArguments())
                {
                    GetRelatedTypesInternal(genericArg, types, visitedTypes);
                }
            }

            // 处理属性类型
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                GetRelatedTypesInternal(property.PropertyType, types, visitedTypes);
            }

            return types;
        }

        private string GetInternalTypeScriptTypeName(Type type)
        {
            if (type == null)
                return "any";

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

        private string GetTypeScriptTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return "?" + GetInternalTypeScriptTypeName(type.GenericTypeArguments[0]);
                }
            }

            return GetInternalTypeScriptTypeName(type);
        }

        private string GetPropertyName(string name)
        {
            return char.ToLower(name[0]) + name.Substring(1);
        }


        public async Task SeedAsync(DataSeedContext context)
        {
            var assemblies = new[]
            {
                Assembly.Load("HB.AbpFundation.Domain.Shared"),
                Assembly.Load("HB.AbpFundation.HttpApi")
            };

            await Task.Delay(10);

            List<Type> controllers = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(x => !string.IsNullOrWhiteSpace(x.Namespace) && x.Namespace.StartsWith("HB.") && x.IsClass && !x.IsAbstract).ToList();
                controllers.AddRange(types.Where(x => typeof(AbpFundationController).IsAssignableFrom(x)));
            }

            List<Type> dtos = new List<Type>();

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    if (method.IsDefined(typeof(NonActionAttribute)))
                    {
                        continue;
                    }
                    //判断是否是异步方法
                    if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        dtos.Add(method.ReturnType.GetGenericArguments()[0]);
                    }
                    else
                    {
                        dtos.Add(method.ReturnType);
                    }

                    foreach (var parameter in method.GetParameters())
                    {
                        dtos.Add(parameter.ParameterType);
                    }
                }
            }


            List<Type> allDtoTypes = new List<Type>();
            foreach (var dto in dtos)
            {
                allDtoTypes.AddRange(GetRelatedTypes(dto));
            }

            allDtoTypes = allDtoTypes.Distinct().OrderBy(x => x.FullName).ToList();

            Console.WriteLine("==========================================================");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Auto-generated type defines");
            foreach (var dtoType in allDtoTypes)
            {
                Console.WriteLine(dtoType.FullName);
                sb.AppendLine();
                sb.AppendLine($"/***{dtoType.FullName}***/");
                sb.Append("export interface ");
                var idx = dtoType.Name.IndexOf("`");
                if (idx > 0)
                {
                    sb.Append(GetTypeScriptTypeName(dtoType).Replace("<>", "<T>"));
                }
                else
                {
                    sb.Append(dtoType.Name);
                }
                if (MustExportType(dtoType.BaseType))
                {
                    sb.Append(" extends ").Append(GetTypeScriptTypeName(dtoType.BaseType));
                }
                sb.Append("{").AppendLine();

                foreach (var property in dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    sb.Append("\t").Append(GetPropertyName(property.Name)).Append(":").Append(GetTypeScriptTypeName(property.PropertyType)).Append(";").AppendLine();
                }

                sb.Append("}").AppendLine();
            }

           var generator = new TypeScriptApiGenerator();
            StringBuilder api = new StringBuilder();
            api.AppendLine(generator.GenerateApiClasses(controllers.ToArray()));
            foreach (var controller in controllers)
            {
              
                  var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    if (method.IsDefined(typeof(NonActionAttribute)))
                    {
                        continue;
                    }
                }
            }

            //Console.WriteLine(sb.ToString());
            File.WriteAllText("D:\\WORKS\\01demo\\new-abp\\demo\\src\\api\\services\\types.d.ts", sb.ToString(), Encoding.UTF8);
            File.WriteAllText("D:\\WORKS\\01demo\\new-abp\\demo\\src\\api\\services\\index.ts", api.ToString(), Encoding.UTF8);
            Console.WriteLine("==========================================================");
        }
    }
}
