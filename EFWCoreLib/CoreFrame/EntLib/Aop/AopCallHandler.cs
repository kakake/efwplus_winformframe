
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
    /// AOP调用管理类
    /// </summary>
    public class AopCallHandler : ICallHandler
    {
        private List<IAopOperator> _list;  //AOP操作对象列表
        /// <summary>
        /// 创建AopCallHandler实例
        /// </summary>
        /// <param name="list">AOP操作类类型列表</param>
        public AopCallHandler(List<Type> list)
        {
            _list = new List<IAopOperator>();
            for (int i = 0; i < list.Count; i++)
            {
                _list.Add((IAopOperator)Activator.CreateInstance(list[i]));
            }
        }
        #region ICallHandler 成员
        /// <summary>
        /// 调用执行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="getNext"></param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result;
            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].PreProcess(input);
            }
            //log
            result = getNext()(input, getNext);
            //if (result.Exception == null)
            //{
            //    Logger.Write("Action Done.");
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].PostProcess(input, result);
            }
            //}
            //log

            return result;
        }
        /// <summary>
        /// 执行顺序
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        #endregion
    }
}
