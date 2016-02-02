using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseModule", EntityType = EntityType.Table, IsGB = true)]
    public class BaseModule:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int  _moduleid;
        /// <summary>
        /// 编号
        /// </summary>
        [Column(FieldName = "ModuleId", DataKey = true,  Match = "", IsInsert = false)]
        public int ModuleId
        {
            get { return  _moduleid; }
            set {  _moduleid = value; }
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

        private int  _sortid;
        /// <summary>
        /// 排序
        /// </summary>
        [Column(FieldName = "SortId", DataKey = false,  Match = "", IsInsert = true)]
        public int SortId
        {
            get { return  _sortid; }
            set {  _sortid = value; }
        }

        private string  _serverip="";
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "ServerIP", DataKey = false,  Match = "", IsInsert = true)]
        public string ServerIP
        {
            get { return  _serverip; }
            set {  _serverip = value; }
        }

    }
}
