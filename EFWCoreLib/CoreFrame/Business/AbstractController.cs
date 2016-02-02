using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.DbProvider;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 抽象控制器
    /// </summary>
    public abstract class AbstractController : AbstractBusines
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        public AbstractDatabase oleDb
        {
            get
            {
                return GetDb();
            }
        }
        /// <summary>
        /// 获取机构ID
        /// </summary>
        public int WorkId
        {
            get { return oleDb.WorkId; }
        }

        /// <summary>
        /// 系统登录用户信息
        /// </summary>
        public SysLoginRight LoginUserInfo
        {
            get
            {
                return GetUserInfo();
            }
        }
        /// <summary>
        /// 实现不同类型控制器获取登录用户，Web模式从Session中获取，Winform模式从Cache中获取，WCF模式从ClientInfo中获取
        /// </summary>
        /// <returns></returns>
        protected virtual SysLoginRight GetUserInfo()
        {
            return new SysLoginRight();
        }
    }
}
