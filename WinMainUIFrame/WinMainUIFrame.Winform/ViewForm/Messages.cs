using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.IView;

namespace WinMainUIFrame.Winform.ViewForm
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Messages
    {
        public static ControllerEventHandler InvokeController = null;

        #region 变量
        private string _typeCode;
        private string _titleText;
        private string _contentText;
        #endregion

        #region 属性
        private int _messageId;
        public int MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        /// <summary>
        /// 消息类型代码
        /// </summary>
        public string TypeCode
        {
            get { return _typeCode; }
            set { _typeCode = value; }
        }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = value; }
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string ContentText
        {
            get { return _contentText; }
            set { _contentText = value; }
        }


        private WinMenu _showMenu;
        public WinMenu ShowMenu
        {
            get { return _showMenu; }
            set { _showMenu = value; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 消息
        /// </summary>
        public Messages()
        {
            this._typeCode = "";
            this._titleText = "";
            this._contentText = "";
            _showMenu = null;
        }

        #endregion

        //获取当前用户的所有消息内容
        public static List<Messages> GetMessages()
        {
            List<Messages> mslist = new List<Messages>();

            //SysLoginRight currLoginUser = (SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser");
            //Message msg = new Message();
            //List<BaseMessage> msglist = msg.GetNotReadMessages(currLoginUser.UserId, currLoginUser.DeptId, currLoginUser.WorkId);
            List<BaseMessage> msglist = InvokeController("GetNotReadMessages") as List<BaseMessage>;
            foreach (BaseMessage val in msglist)
            {
                Messages ms = new Messages();
                ms.TypeCode = val.MessageType;
                ms.MessageId = val.Id;
                ms.TitleText = val.MessageTitle;
                ms.ContentText = val.MessageContent;
                mslist.Add(ms);
            }
            return mslist;
        }

        public static void setMessageRead(int[] msId)
        {
            //SysLoginRight currLoginUser = (SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser");
            //Message msg = new Message();
            foreach (int id in msId)
            {
                //msg.MessageRead(id, currLoginUser.UserId);
                InvokeController("MessageRead", id);
            }
        }

    }
}
