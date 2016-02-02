using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using System.Reflection;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.WcfFrame.ServerController
{
    public class ControllerHelper
    {
        public static WcfServerController CreateController(string controllername)
        {
            string[] names = controllername.Split(new char[] { '@' });
            if (names.Length != 2) throw new Exception("控制器名称错误!");
            string pluginname = names[0];
            string cname = names[1];
            ModulePlugin mp;
            WcfControllerAttributeInfo wattr = AppPluginManage.GetPluginWcfControllerAttributeInfo(pluginname,cname, out mp);
            //WcfServerController iController = wattr.wcfController as WcfServerController;
            WcfServerController iController = (WcfServerController)EFWCoreLib.CoreFrame.Business.FactoryModel.GetObject(wattr.wcfControllerType, mp.database, mp.container, mp.cache, mp.plugin.name, null);
            iController.BindDb(mp.database, mp.container, mp.cache,mp.plugin.name);

            iController.ParamJsonData = null;
            iController.ClientInfo = null;
           
            return iController;
        }

        public static MethodInfo CreateMethodInfo(string controllername, string methodname, AbstractController controller)
        {
            string[] names = controllername.Split(new char[] { '@' });
            if (names.Length != 2) throw new Exception("控制器名称错误!");
            string pluginname = names[0];
            string cname = names[1];

            ModulePlugin mp;
            WcfControllerAttributeInfo cattr = AppPluginManage.GetPluginWcfControllerAttributeInfo(pluginname,cname, out mp);

            WcfMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
            if (mattr == null) throw new Exception("控制器中没有此方法名");

            if (mattr.dbkeys != null && mattr.dbkeys.Count > 0)
            {
                controller.BindMoreDb(mp.database, "default");
                foreach (string dbkey in mattr.dbkeys)
                {
                    EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase _Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase(dbkey);
                    _Rdb.WorkId = controller.LoginUserInfo.WorkId;
                    //创建数据库连接
                    controller.BindMoreDb(_Rdb, dbkey);
                }
            }

            return mattr.methodInfo;
        }
    }
}
