
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EFWCoreLib.CoreFrame.EntLib.Aop
{
    /// <summary>
    /// AOP标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property |AttributeTargets.Interface)]
    public class AOPAttribute : HandlerAttribute
    {
        private List<Type> _types;
        /// <summary>
        /// 创建AOPAttribute实例
        /// </summary>
        /// <param name="types">AOP操作类类型数组</param>
        public AOPAttribute(params Type[] types)
        {
            _types = types.ToList();
        }
        /// <summary>
        /// 创建AOP管理对象
        /// </summary>
        /// <param name="container">示例容器</param>
        /// <returns></returns>
        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            if (_types.Count > 0)
            {
                return new AopCallHandler(_types);
            }

            return null;
        }
    }
}
