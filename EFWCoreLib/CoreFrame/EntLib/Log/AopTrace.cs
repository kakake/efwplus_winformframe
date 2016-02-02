using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using EFWCoreLib.CoreFrame.EntLib.Aop;

namespace EFWCoreLib.CoreFrame.EntLib.Log
{
    /// <summary>
    /// 用AOP拦截跟踪代码
    /// </summary>
    public class AopTrace : IAopOperator
    {
        Tracer trace;
        #region IAopOperator 成员
        /// <summary>
        /// 前处理
        /// </summary>
        /// <param name="input"></param>
        public void PreProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input)
        {
            trace = LogHelper.StartTrace();
        }
        /// <summary>
        /// 后处理
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        public void PostProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input, Microsoft.Practices.Unity.InterceptionExtension.IMethodReturn result)
        {
            LogHelper.EndTrace(trace);
        }

        #endregion
    }
}
