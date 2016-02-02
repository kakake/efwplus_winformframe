using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Plugin;
using System.Reflection;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.WebFrame.HttpHandler.Controller
{
    public class ControllerHelper
    {
        public static WebHttpController CreateController(string controllername)
        {
            string[] names = controllername.Split(new char[] { '@' });
            if (names.Length != 2) throw new Exception("控制器名称错误!");
            string pluginname = names[0];
            string cname = names[1];
            ModulePlugin mp;
            WebControllerAttributeInfo webcAttr= AppPluginManage.GetPluginWebControllerAttributeInfo(pluginname,cname, out mp);
            WebHttpController iController =  null;
            if(webcAttr.webController==null)
                iController = (WebHttpController)EFWCoreLib.CoreFrame.Business.FactoryModel.GetObject(webcAttr.webControllerType, mp.database, mp.container, mp.cache, mp.plugin.name, null);
            iController.BindDb(mp.database, mp.container, mp.cache,mp.plugin.name);

            iController.ClearKey = null;
            iController.FormData = null;
            iController.ParamsData = null;
            iController.PutOutData = null;
            iController.sessionData = null;

            iController.JsonResult = "";
            iController.ViewResult = "";
            iController.ViewData = null;

            return iController;
        }

        public static MethodInfo CreateMethodInfo(string controllername, string methodname, AbstractController webController)
        {
            string[] names = controllername.Split(new char[] { '@' });
            if (names.Length != 2) throw new Exception("控制器名称错误!");
            string pluginname = names[0];
            string cname = names[1];

            ModulePlugin mp;
            WebControllerAttributeInfo cattr = AppPluginManage.GetPluginWebControllerAttributeInfo(pluginname,cname, out mp);

            WebMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
            if (mattr == null) throw new Exception("控制器中没有此方法名");

            if (mattr.dbkeys != null && mattr.dbkeys.Count > 0)
            {
                webController.BindMoreDb(mp.database, "default");
                foreach (string dbkey in mattr.dbkeys)
                {
                    EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase _Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase(dbkey);
                    _Rdb.WorkId = webController.LoginUserInfo.WorkId;
                    //创建数据库连接
                    webController.BindMoreDb(_Rdb, dbkey);
                }
            }

            return mattr.methodInfo;
        }

        public static void ControllerWrite(WebHttpController controller)
        {
            if (string.IsNullOrEmpty(controller.JsonResult)==false)
            {
                controller.context.Response.Charset = "UTF-8";
                controller.context.Response.Write(controller.JsonResult);
            }
            else if (string.IsNullOrEmpty(controller.ViewResult) == false)
            {
                controller.context.Response.Write(controller.ViewResult);
            }
        }
    }
}
