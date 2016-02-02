
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Orm
{
    /// <summary>
    /// 数据列自定义属性
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        private string alias;        //别名
        private string _fieldname;   //字段名称
        private bool _datakey;       //是否为主键
        private bool _issinglequote; //字段类型带不带单引号
        private string _match;       //值的匹配条件

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool DataKey
        {
            get { return _datakey; }
            set { _datakey = value; }
        }
        /// <summary>
        /// 值的匹配条件
        /// </summary>
        public new string Match
        {
            get { return _match; }
            set { _match = value; }
        }

        private bool _isInsert = true;
        /// <summary>
        /// Add是否插入到数据库
        /// </summary>
        public bool IsInsert
        {
            get { return _isInsert; }
            set { _isInsert = value; }
        }
    }

   
}
