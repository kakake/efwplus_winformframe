
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
    /// 实体类型
    /// </summary>
    public enum EntityType
    {
        /// <summary>表</summary>
        Table,
        /// <summary>视图</summary>
        View
    }

    /// <summary>
    /// 数据表自定义属性
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class TableAttribute : Attribute
    {
        private string alias;
        private string _tableName;    //保存表名的字段
        private EntityType _entityType;  //实体类型
        private bool _isgb;   //是否是国标（共享）
        /// <summary>
        /// 主键字段名
        /// </summary>
        public string KeyFieldName;

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        /// <summary>
        /// 映射的表名
        /// </summary>
        public string TableName
        {
            set
            {
                this._tableName = value;
            }
            get
            {
                return this._tableName;
            }
        }
        /// <summary>
        /// 实体类型
        /// </summary>
        public EntityType EntityType
        {
            get { return _entityType; }
            set { _entityType = value; }
        }
        /// <summary>
        /// 是否是国标（共享）
        /// </summary>
        public bool IsGB
        {
            get { return _isgb; }
            set { _isgb = value; }
        }
    }

}
