using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 控制器委托
    /// </summary>
    /// <param name="eventname">方法名称</param>
    /// <param name="objs">参数数组</param>
    /// <returns></returns>
    [Serializable]
    public delegate object ControllerEventHandler(string funname, params object[] objs);
    /// <summary>
    /// 控制器基础接口
    /// </summary>
    public interface IBaseViewBusiness
    {
        /// <summary>
        /// 控制器事件
        /// </summary>
        //event ControllerEventHandler ControllerEvent;

        ControllerEventHandler InvokeController { get; set; }
    }
}
