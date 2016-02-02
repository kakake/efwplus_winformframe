using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseWorkers", EntityType = EntityType.Table, IsGB = true)]
    public class BaseWorkers:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int _workId;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "WorkId", DataKey = true, Match = "", IsInsert = false)]
        public int WorkId
        {
            get { return _workId; }
            set { _workId = value; }
        }

        private string  _workno;
        /// <summary>
        /// 工作组编码
        /// </summary>
        [Column(FieldName = "WorkNo", DataKey = false,  Match = "", IsInsert = true)]
        public string WorkNo
        {
            get { return  _workno; }
            set {  _workno = value; }
        }

        private string  _workname;
        /// <summary>
        /// 工作组名称
        /// </summary>
        [Column(FieldName = "WorkName", DataKey = false,  Match = "", IsInsert = true)]
        public string WorkName
        {
            get { return  _workname; }
            set {  _workname = value; }
        }

        private string  _memo;
        /// <summary>
        /// 工作组备注
        /// </summary>
        [Column(FieldName = "Memo", DataKey = false,  Match = "", IsInsert = true)]
        public string Memo
        {
            get { return  _memo; }
            set {  _memo = value; }
        }

        private string  _regkey;
        /// <summary>
        /// 注册码
        /// </summary>
        [Column(FieldName = "RegKey", DataKey = false,  Match = "", IsInsert = true)]
        public string RegKey
        {
            get { return  _regkey; }
            set {  _regkey = value; }
        }

        private string  _editioncode;
        /// <summary>
        /// 版本号
        /// </summary>
        [Column(FieldName = "EditionCode", DataKey = false,  Match = "", IsInsert = true)]
        public string EditionCode
        {
            get { return  _editioncode; }
            set {  _editioncode = value; }
        }

        private int  _delflag;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "DelFlag", DataKey = false,  Match = "", IsInsert = true)]
        public int DelFlag
        {
            get { return  _delflag; }
            set {  _delflag = value; }
        }

    }
}
