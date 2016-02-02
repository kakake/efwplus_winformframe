using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EFWCoreLib.CoreFrame.Orm
{
    /// <summary>
    /// 外键映射名称
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class RelationNameAttribute : Attribute
    {
        private string _typeName;//缓存集合的名称
        private string _propertyName;//外键Code的映射属性名
        /// <summary>
        /// 缓存集合的名称
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }
        /// <summary>
        /// 外键Code的映射属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }
    }

    /// <summary>
    /// 给实体属性赋值
    /// </summary>
    public class RelationFunction
    {
        /// <summary>
        /// 缓存集合中根据名称和外键字段值，获取名称值
        /// GetRelationName getName = delegate(string typeName,string codeValue){//?};
        /// </summary>
        /// <param name="typeName">缓存数据源（CommonData）中的名称</param>
        /// <param name="codeValue">外键Code的映射字段的值</param>
        /// <returns></returns>
        public delegate object GetRelationName(string typeName,string codeValue);

        /// <summary>
        /// 给定义的实体属性赋值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="relation"></param>
        public static void GetRelationInfo(object model,GetRelationName relation)
        {
            string _typeName;
            string _code;

            System.Reflection.PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {

                RelationNameAttribute[] columnAttributes = (RelationNameAttribute[])property.GetCustomAttributes(typeof(RelationNameAttribute), true);
                if (columnAttributes.Length > 0)
                {
                    _typeName = columnAttributes[0].TypeName;
                    _code = model.GetType().GetProperty(columnAttributes[0].PropertyName).GetValue(model, null).ToString();

                    object value = relation(_typeName, _code);

                    property.SetValue(model, value, null);
                }
            }
        }
    }
}
