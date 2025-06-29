using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;


namespace CodeSmithCore
{
    public class PersistenceObjectInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 实体名称（带命名空间）
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 聚合根名称
        /// </summary>
        public string AggregationName
        {
            get
            {
                return Name;
            }
        }

        public string AggregationVar
        {
            get
            {
                return StringUtils.GetCamelCaseVar(AggregationName);
            }
        }
        /// <summary>
        /// 仓储接口名称
        /// </summary>
        public string IRepositoryName
        {
            get
            {
                return "I" + Name + "Repository";
            }
        }

        public string IRepositoryMember
        {
            get
            {
                return StringUtils.GetCamelCaseMember(RepositoryName);
            }
        }
        public string IRepositoryVar
        {
            get
            {
                return StringUtils.GetCamelCaseVar(RepositoryName);
            }
        }
        public string IQueryServiceMember
        {
            get
            {
                return StringUtils.GetCamelCaseMember(QueryServiceName);
            }
        }
        public string IQueryServiceVar
        {
            get
            {
                return StringUtils.GetCamelCaseVar(QueryServiceName);
            }
        }
        public string IUseCaseServiceMember
        {
            get
            {
                return StringUtils.GetCamelCaseMember(UseCaseServiceName);
            }
        }
        public string IUseCaseServiceVar
        {
            get
            {
                return StringUtils.GetCamelCaseVar(UseCaseServiceName);
            }
        }
        /// <summary>
        /// 仓储实现名称
        /// </summary>
        public string RepositoryName
        {
            get
            {
                return Name + "Repository";
            }
        }
        /// <summary>
        /// 查询服务接口名称
        /// </summary>
        public string IQueryServiceName
        {
            get
            {
                return "I" + AggregationName + "QueryService";
            }
        }
    
        /// <summary>
        /// 查询服务实现名称
        /// </summary>
        public string QueryServiceName
        {
            get
            {
                return AggregationName + "QueryService";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UseCaseServiceName
        {
            get
            {
                return AggregationName + "Service";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IUseCaseServiceName
        {
            get
            {
                return "I" + AggregationName + "Service";
            }
        }

  
        public string CreateInput
        {
            get
            {
                return "Create" + AggregationName + "Input";
            }
        }

        public string UpdateInput
        {
            get
            {
                return "Update" + AggregationName + "Input";
            }
        }

        public string DeleteInput
        {
            get
            {
                return "Delete" + AggregationName + "Input";
            }
        }

        public string GetListInput
        {
            get
            {
                return "Get" + AggregationName + "ListInput";
            }
        }

        public string GetIdInput
        {
            get
            {
                return "Get" + AggregationName + "IdInput";
            }
        }

        public string Output
        {
            get
            {
                return AggregationName+"Dto";
            }
        }

        public string ControllerName
        {
            get
            {
                return AggregationName + "Controller";
            }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PersistencePropertyInfo> Properties { get; set; } = new List<PersistencePropertyInfo>();

        public PersistenceObjectInfo() { }

        public PersistenceObjectInfo(Type clrType)
        {
            this.Name = clrType.Name;
            this.Comment = clrType.FullName;
            this.TypeName = clrType.FullName;

            TableName = clrType.Name;
            if (clrType.IsDefined(typeof(TableAttribute), false))
            {
                TableAttribute attribute = clrType.GetCustomAttribute<TableAttribute>();
                TableName = attribute.Name;
            }
            XmlElement xmlComment = null;

            xmlComment = DocsByReflection.DocsService.GetXmlFromType(clrType, false);
            if (xmlComment != null)
            {
                var xmlNode = xmlComment.SelectSingleNode("summary");
                if (xmlNode != null)
                {
                    string comment = xmlNode.InnerText;
                    this.Comment = StringUtils.GetCommentFromText(comment);
                }
            }

            var properties = clrType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {

                Properties.Add(new PersistencePropertyInfo(property));
            }
        }
    }
}
