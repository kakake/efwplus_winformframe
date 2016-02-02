using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.WcfService;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// WCF控制器服务端基类
    /// </summary>
    public class WcfServerController : AbstractController
    {
       
        protected override SysLoginRight GetUserInfo()
        {
            if (ClientInfo != null && ClientInfo.LoginRight != null)
                return ClientInfo.LoginRight;
            return base.GetUserInfo();
        }

        //public DataTable GetPageRight(int MenuId)
        //{
        //    DataTable data = (DataTable)ExecuteFun.invoke(oleDb, "getPageRight", MenuId, LoginUserInfo.UserId);
        //    return data;
        //}

         /// <summary>
        /// 创建BaseWCFController的实例
        /// </summary>
        public WcfServerController()
        {
            
        }

        /// <summary>
        /// 初始化全局web服务参数对象
        /// </summary>
        public virtual void Init() { }


        /// <summary>
        /// 客户端传递的参数
        /// </summary>
        public string ParamJsonData
        {
            get;
            set;
        }

        /// <summary>
        /// 当前客户端信息
        /// </summary>
        public WCFClientInfo ClientInfo
        {
            get;
            set;
        }
    }


}
