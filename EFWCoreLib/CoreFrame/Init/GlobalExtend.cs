using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using EFWCoreLib.CoreFrame.Plugin;

namespace EFWCoreLib.CoreFrame.Init
{
    /// <summary>
    /// 系统启动与停止扩展
    /// </summary>
    public abstract class GlobalExtend
    {
        public abstract void AppStartDo();
        public abstract void AppEndDo();

        internal static void StartInit()
        {

            IEnumerable<GlobalExtend> comms = AppGlobal.container.ResolveAll<GlobalExtend>();

            foreach (GlobalExtend comm in comms)
            {
                comm.AppStartDo();
            }

            foreach (ModulePlugin mp in AppPluginManage.PluginDic.Values)
            {
                comms = mp.container.ResolveAll<GlobalExtend>();
                foreach (GlobalExtend comm in comms)
                {
                    comm.AppStartDo();
                }
            }
        }

        internal static void EndInit()
        {
            if (AppGlobal.container == null) return;

            IEnumerable<GlobalExtend> comms = AppGlobal.container.ResolveAll<GlobalExtend>();

            foreach (GlobalExtend comm in comms)
            {
                comm.AppEndDo();
            }

            foreach (ModulePlugin mp in AppPluginManage.PluginDic.Values)
            {
                comms = mp.container.ResolveAll<GlobalExtend>();
                foreach (GlobalExtend comm in comms)
                {
                    comm.AppEndDo();
                }
            }
        }
    }
}
