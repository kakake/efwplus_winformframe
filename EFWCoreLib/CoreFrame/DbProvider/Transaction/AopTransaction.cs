
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using EFWCoreLib.CoreFrame.EntLib.Aop;
using System.Collections.Generic;
using EFWCoreLib.CoreFrame.Business.Interface;

namespace EFWCoreLib.CoreFrame.DbProvider.Transaction
{
    public class AopTransaction : IAopOperator
    {
        public AopTransaction()
        {
        }

        #region IAopOperator 成员

        public void PreProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input)
        {
            List<AbstractDatabase> _RdbList = ((IbindDb)input.Target).GetMoreDb();
            AbstractDatabase Rdb = ((IbindDb)input.Target).GetDb();
            if (_RdbList == null)
            {
                Rdb.BeginTransaction();
            }
            else
            {
                foreach (AbstractDatabase db in _RdbList)
                {
                    db.BeginTransaction();
                }
            }
        }

        public void PostProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input, Microsoft.Practices.Unity.InterceptionExtension.IMethodReturn result)
        {
            List<AbstractDatabase> _RdbList = ((IbindDb)input.Target).GetMoreDb();
            AbstractDatabase Rdb = ((IbindDb)input.Target).GetDb();
            if (_RdbList == null)
            {
                if (result.Exception == null)
                {
                    Rdb.CommitTransaction();
                }
                else
                {
                    Rdb.RollbackTransaction();
                }
            }
            else
            {
                List<AbstractDatabase> RdbList = new List<AbstractDatabase>();
                foreach (AbstractDatabase db in _RdbList)
                {
                    RdbList.Add(db);
                }
                RdbList.Reverse();//反序

                if (result.Exception == null)
                {
                    foreach (AbstractDatabase db in RdbList)
                    {
                        db.CommitTransaction();
                    }
                }
                else
                {
                    foreach (AbstractDatabase db in RdbList)
                    {
                        db.RollbackTransaction();
                    }
                }
            }
        }

        #endregion
    }
}
