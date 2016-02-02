
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
    /// IAopOperator AOP操作符接口，包括前处理和后处理
    /// </summary>
    public interface IAopOperator
    {
        /// <summary>
        /// 前处理
        /// </summary>
        /// <param name="input"></param>
        void PreProcess(IMethodInvocation input);
        /// <summary>
        /// 后处理
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        void PostProcess(IMethodInvocation input, IMethodReturn result);
    }
}
