using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseGroup", EntityType = EntityType.Table, IsGB = true)]
    public class BaseGroup:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int  _groupid;
        /// <summary>
        /// 编号
        /// </summary>
        [Column(FieldName = "GroupId", DataKey = true,  Match = "", IsInsert = false)]
        public int GroupId
        {
            get { return  _groupid; }
            set {  _groupid = value; }
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

        private int  _delflag;
        /// <summary>
        /// 删除标记
        /// </summary>
        [Column(FieldName = "DelFlag", DataKey = false,  Match = "", IsInsert = true)]
        public int DelFlag
        {
            get { return  _delflag; }
            set {  _delflag = value; }
        }

        private int  _admin;
        /// <summary>
        /// 是否高级管理员
        /// </summary>
        [Column(FieldName = "Admin", DataKey = false,  Match = "", IsInsert = true)]
        public int Admin
        {
            get { return  _admin; }
            set {  _admin = value; }
        }

        private int  _everyone;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "Everyone", DataKey = false,  Match = "", IsInsert = true)]
        public int Everyone
        {
            get { return  _everyone; }
            set {  _everyone = value; }
        }

        private string  _memo="";
        /// <summary>
        /// 备注
        /// </summary>
        [Column(FieldName = "Memo", DataKey = false,  Match = "", IsInsert = true)]
        public string Memo
        {
            get { return  _memo; }
            set {  _memo = value; }
        }

        private string  _property="";
        /// <summary>
        /// 属性
        /// </summary>
        [Column(FieldName = "Property", DataKey = false,  Match = "", IsInsert = true)]
        public string Property
        {
            get { return  _property; }
            set {  _property = value; }
        }

    }
}
