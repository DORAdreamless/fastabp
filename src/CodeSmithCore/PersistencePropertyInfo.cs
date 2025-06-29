using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeSmithCore
{
    public class PersistencePropertyInfo
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
        /// c#类型名称
        /// </summary>
        public string CSharpTypeName { get; set; }

        public string DataType { get; set; }

        public bool Mandatory { get; set; }

        public string DefaultValue { get; set; }

        public string NameVar
        {
            get
            {
                return StringUtils.GetCamelCaseVar(Name);
            }
        }

        public PersistencePropertyInfo() { }

        public PersistencePropertyInfo(PropertyInfo property)
        {
            this.Name = property.Name;
            this.Comment = property.Name;

            var clrType = property.PropertyType;
            this.CSharpTypeName = clrType.Name;

            if (clrType.Name.StartsWith("Nullable`"))
            {
                this.CSharpTypeName = clrType.GenericTypeArguments[0].Name + "?";
                GetDataType(property, clrType.GenericTypeArguments[0]);
            }
            else
            {
                GetDataType(property, clrType);
            }
            if (clrType != typeof(string) && !clrType.Name.StartsWith("Nullable`"))
            {
                this.Mandatory = true;
            }

            var xmlComment = DocsByReflection.DocsService.GetXmlFromMember(property, false);
            if (xmlComment != null)
            {
                var xmlNode = xmlComment.SelectSingleNode("summary");
                if (xmlNode != null)
                {
                    var comment = xmlNode.InnerText;
                    this.Comment = StringUtils.GetCommentFromText(comment);
                }
            }
        }

        private void GetDataType(PropertyInfo property, Type clrType)
        {
            switch (clrType.Name)
            {
                case "Guid":
                    this.DataType = "uniqueidentifier";
                    break;
                case "DateTime":
                    this.DataType = "datetime2";
                    break;
                case "Boolean":
                    this.DataType = "bit";
                    break;
                case "Decimal":
                    this.DataType = "decimal(18,5)";
                    break;
                case "Float":
                    this.DataType = "float";
                    break;
                case "Double":
                    this.DataType = "double";
                    break;
                case "Int32":
                    this.DataType = "int";
                    break;
                case "Int64":
                    this.DataType = "bigint";
                    break;
                default:
                    if (property.IsDefined(typeof(StringLengthAttribute), false))
                    {
                        var attribute = property.GetCustomAttribute<StringLengthAttribute>();
                        if (attribute.MaximumLength == -1)
                        {

                            this.DataType = "nvarchar(MAX)";
                        }
                        else
                        {
                            this.DataType = $"nvarchar({attribute.MaximumLength})";
                        }

                    }
                    else
                    {
                        this.DataType = "nvarchar(1000)";
                    }
                    break;
            }
        }
    }
}
