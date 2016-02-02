using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.Interface
{
    /// <summary>
    /// 创建Dao对象接口
    /// </summary>
    interface INewDao
    {
        /// <summary>
        /// 创建Dao
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T NewDao<T>();
        /// <summary>
        /// 创建带别名的Dao
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityname"></param>
        /// <returns></returns>
        T NewDao<T>(string unityname);
    }
}
