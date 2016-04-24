using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;
using EFWCoreLib.CoreFrame.Business;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseMessage", EntityType = EntityType.Table, IsGB = true)]
    public class BaseMessage : AbstractEntity
    {
        private int  _id;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "Id", DataKey = true,  Match = "", IsInsert = false)]
        public int Id
        {
            get { return  _id; }
            set {  _id = value; }
        }

        private string  _messagetype;
        /// <summary>
        /// 消息类型
        /// </summary>
        [Column(FieldName = "MessageType", DataKey = false,  Match = "", IsInsert = true)]
        public string MessageType
        {
            get { return  _messagetype; }
            set {  _messagetype = value; }
        }

        private int  _senduser;
        /// <summary>
        /// 发送用户
        /// </summary>
        [Column(FieldName = "SendUser", DataKey = false,  Match = "", IsInsert = true)]
        public int SendUser
        {
            get { return  _senduser; }
            set {  _senduser = value; }
        }

        private int  _senddept;
        /// <summary>
        /// 发送科室
        /// </summary>
        [Column(FieldName = "SendDept", DataKey = false,  Match = "", IsInsert = true)]
        public int SendDept
        {
            get { return  _senddept; }
            set {  _senddept = value; }
        }

        private int  _sendwork;
        /// <summary>
        /// 发送机构
        /// </summary>
        [Column(FieldName = "SendWork", DataKey = false,  Match = "", IsInsert = true)]
        public int SendWork
        {
            get { return  _sendwork; }
            set {  _sendwork = value; }
        }

        private DateTime  _sendtime;
        /// <summary>
        /// 消息发送时间
        /// </summary>
        [Column(FieldName = "SendTime", DataKey = false,  Match = "", IsInsert = true)]
        public DateTime SendTime
        {
            get { return  _sendtime; }
            set {  _sendtime = value; }
        }

        private int  _receiveuser;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "ReceiveUser", DataKey = false,  Match = "", IsInsert = true)]
        public int ReceiveUser
        {
            get { return  _receiveuser; }
            set {  _receiveuser = value; }
        }

        private int  _receivedept;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "ReceiveDept", DataKey = false,  Match = "", IsInsert = true)]
        public int ReceiveDept
        {
            get { return  _receivedept; }
            set {  _receivedept = value; }
        }

        private int  _receivework;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "ReceiveWork", DataKey = false,  Match = "", IsInsert = true)]
        public int ReceiveWork
        {
            get { return  _receivework; }
            set {  _receivework = value; }
        }

        private string  _messagetitle;
        /// <summary>
        /// 消息标题
        /// </summary>
        [Column(FieldName = "MessageTitle", DataKey = false,  Match = "", IsInsert = true)]
        public string MessageTitle
        {
            get { return  _messagetitle; }
            set {  _messagetitle = value; }
        }

        private string  _messagecontent;
        /// <summary>
        /// 消息内容
        /// </summary>
        [Column(FieldName = "MessageContent", DataKey = false,  Match = "", IsInsert = true)]
        public string MessageContent
        {
            get { return  _messagecontent; }
            set {  _messagecontent = value; }
        }

        private DateTime  _limittime;
        /// <summary>
        /// 消息有效期
        /// </summary>
        [Column(FieldName = "Limittime", DataKey = false,  Match = "", IsInsert = true)]
        public DateTime Limittime
        {
            get { return  _limittime; }
            set {  _limittime = value; }
        }

        private string  _relationurl;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "RelationURL", DataKey = false,  Match = "", IsInsert = true)]
        public string RelationURL
        {
            get { return  _relationurl; }
            set {  _relationurl = value; }
        }

        private string  _relationtext;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "RelationText", DataKey = false,  Match = "", IsInsert = true)]
        public string RelationText
        {
            get { return  _relationtext; }
            set {  _relationtext = value; }
        }

        private string  _relationid;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "RelationId", DataKey = false,  Match = "", IsInsert = true)]
        public string RelationId
        {
            get { return  _relationid; }
            set {  _relationid = value; }
        }

        private int  _answermessageid;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "AnswerMessageId", DataKey = false,  Match = "", IsInsert = true)]
        public int AnswerMessageId
        {
            get { return  _answermessageid; }
            set {  _answermessageid = value; }
        }

    }
}
