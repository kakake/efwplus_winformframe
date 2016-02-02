using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseDeptLayer", EntityType = EntityType.Table, IsGB = true)]
    public class BaseDeptLayer:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int  _layerid;
        /// <summary>
        /// 节点ID
        /// </summary>
        [Column(FieldName = "LayerId", DataKey = true,  Match = "", IsInsert = false)]
        public int LayerId
        {
            get { return  _layerid; }
            set {  _layerid = value; }
        }

        private int  _pid;
        /// <summary>
        /// 父节点ID
        /// </summary>
        [Column(FieldName = "PId", DataKey = false,  Match = "", IsInsert = true)]
        public int PId
        {
            get { return  _pid; }
            set {  _pid = value; }
        }

        private string  _name;
        /// <summary>
        /// 名称
        /// </summary>
        [Column(FieldName = "Name", DataKey = false,  Match = "", IsInsert = true)]
        public string Name
        {
            get { return  _name; }
            set {  _name = value; }
        }

    }
}
