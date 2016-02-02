using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseDept", EntityType = EntityType.Table, IsGB = true)]
    public class BaseDept:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int  _deptid;
        /// <summary>
        /// ID
        /// </summary>
        [Column(FieldName = "DeptId", DataKey = true,  Match = "", IsInsert = false)]
        public int DeptId
        {
            get { return  _deptid; }
            set {  _deptid = value; }
        }

        private int  _layer;
        /// <summary>
        /// 级别
        /// </summary>
        [Column(FieldName = "Layer", DataKey = false,  Match = "", IsInsert = true)]
        public int Layer
        {
            get { return  _layer; }
            set {  _layer = value; }
        }

        private string  _name="";
        /// <summary>
        /// 名称
        /// </summary>
        [Column(FieldName = "Name", DataKey = false,  Match = "", IsInsert = true)]
        public string Name
        {
            get { return  _name; }
            set {  _name = value; }
        }

        private string  _pym="";
        /// <summary>
        /// 拼音码
        /// </summary>
        [Column(FieldName = "Pym", DataKey = false,  Match = "", IsInsert = true)]
        public string Pym
        {
            get { return  _pym; }
            set {  _pym = value; }
        }

        private string  _wbm="";
        /// <summary>
        /// 五笔码
        /// </summary>
        [Column(FieldName = "Wbm", DataKey = false,  Match = "", IsInsert = true)]
        public string Wbm
        {
            get { return  _wbm; }
            set {  _wbm = value; }
        }

        private string  _szm="";
        /// <summary>
        /// 数字码
        /// </summary>
        [Column(FieldName = "Szm", DataKey = false,  Match = "", IsInsert = true)]
        public string Szm
        {
            get { return  _szm; }
            set {  _szm = value; }
        }

        private string  _code="";
        /// <summary>
        /// 代码
        /// </summary>
        [Column(FieldName = "Code", DataKey = false,  Match = "", IsInsert = true)]
        public string Code
        {
            get { return  _code; }
            set {  _code = value; }
        }

        private int  _delflag;
        /// <summary>
        /// 删除标志
        /// </summary>
        [Column(FieldName = "DelFlag", DataKey = false,  Match = "", IsInsert = true)]
        public int DelFlag
        {
            get { return  _delflag; }
            set {  _delflag = value; }
        }

        private int  _sortorder;
        /// <summary>
        /// 排序号
        /// </summary>
        [Column(FieldName = "SortOrder", DataKey = false,  Match = "", IsInsert = true)]
        public int SortOrder
        {
            get { return  _sortorder; }
            set {  _sortorder = value; }
        }

        private string  _memo="";
        /// <summary>
        /// 备注
        /// </summary>
        [Column(FieldName = "Memo", DataKey = false, Match = "", IsInsert = true)]
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
        
    }
}
