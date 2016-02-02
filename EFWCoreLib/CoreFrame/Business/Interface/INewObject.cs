using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.Interface
{
    /// <summary>
    /// 创建领域对象接口
    /// </summary>
    interface INewObject
    {
        /// <summary>
        /// 创建领域对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T NewObject<T>();
        /// <summary>
        /// 根据别名创建领域对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityname"></param>
        /// <returns></returns>
        T NewObject<T>(string unityname);
    }
}
