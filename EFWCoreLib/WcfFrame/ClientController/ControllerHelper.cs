using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using System.Reflection;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.WcfFrame.ClientController
{
    public class ControllerHelper
    {
        public static WcfClientController CreateController(string controllername)
        {
            string[] names = controllername.Split(new char[] { '@' });
            if (names.Length != 2) throw new Exception("控制器名称错误!");
            string pluginname = names[0];
            string cname = names[1];

            ModulePlugin mp;
            WinformControllerAttributeInfo wattr = AppPluginManage.GetPluginWinformControllerAttributeInfo(pluginname, cname, out mp);
            if (wattr != null)
            {
                WcfClientController iController = wattr.winformController as WcfClientController;
                if (iController.InitFinish == false)
                {
                    iController.BindDb(mp.database, mp.container, mp.cache, mp.plugin.name);


                    //IBaseView deview = (IBaseView)System.Activator.CreateInstance(wattr.ViewList.Find(x => x.IsDefaultView).ViewType);
                    //iController._defaultView = deview;

                    Dictionary<string, IBaseViewBusiness> viewDic = new Dictionary<string, IBaseViewBusiness>();
                    for (int i = 0; i < wattr.ViewList.Count; i++)
                    {
                        IBaseViewBusiness view = (IBaseViewBusiness)System.Activator.CreateInstance(wattr.ViewList[i].ViewType);
                        viewDic.Add(wattr.ViewList[i].Name, view);

                        if (wattr.ViewList[i].IsDefaultView)
                            iController._defaultView = view;
                    }
                    iController.iBaseView = viewDic;


                    iController.Init();
                    iController.InitFinish = true;
                }

                return iController;
            }
            else
                return null;
        }

        public static MethodInfo CreateMethodInfo(string controllername, string methodname)
        {
            string[] names = controllername.Split(new char[] { '@' });
            if (names.Length != 2) throw new Exception("控制器名称错误!");
            string pluginname = names[0];
            string cname = names[1];

            ModulePlugin mp;
            WinformControllerAttributeInfo cattr = AppPluginManage.GetPluginWinformControllerAttributeInfo(pluginname,cname, out mp);

            WinformMethodAttributeInfo mattr = cattr.MethodList.Find(x => x.methodName == methodname);
            if (mattr == null) throw new Exception("控制器中没有此方法名");

            if (mattr.dbkeys != null && mattr.dbkeys.Count > 0)
            {
                cattr.winformController.BindMoreDb(mp.database, "default");
                foreach (string dbkey in mattr.dbkeys)
                {
                    EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase _Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase(dbkey);
                    _Rdb.WorkId = cattr.winformController.LoginUserInfo.WorkId;
                    //创建数据库连接
                    cattr.winformController.BindMoreDb(_Rdb, dbkey);
                }
            }

            return mattr.methodInfo;
        }
    }
}
