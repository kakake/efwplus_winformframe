using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Practices.Unity;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Business;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Reflection;

namespace EFWCoreLib.CoreFrame.Init
{
    /*委托代码使用
     * 1.继承ExecuteFun并实现LoadFun方法
     *  public class BaseLibDelegateCode : BaseDelegateCode
        {
            [DelegateCode]
            public DataTable getPageRight(int menuId, int userId)
            {
                DataTable data = null;
                string strsql = @"";
                strsql = string.Format(strsql, menuId, userId);
                data = oleDb.GetDataTable(strsql);
                return data;
            }
        }
     * 2.调用委托代码
     * BaseDelegateCode.invoke("getPageRight",1,1);
     */
    public delegate object FunCode(params object[] args);

    public class FunClass
    {
        public string funName { get; set; }
        public FunCode funCode { get; set; }
    }

    [AttributeUsageAttribute(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class DelegateCodeAttribute : Attribute
    {

    }

    /// <summary>
    /// 委托代码
    /// </summary>
    public abstract class BaseDelegateCode
    {
        public AbstractDatabase oleDb
        {
            get { return AppGlobal.database; }
        }

        public ICacheManager cache
        {
            get { return AppGlobal.cache; }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        public virtual void LoadFun(List<FunClass> funList)
        {
            MethodInfo[] property = this.GetType().GetMethods();
            for (int n = 0; n < property.Length; n++)
            {
                DelegateCodeAttribute[] dcodeattr = (DelegateCodeAttribute[])property[n].GetCustomAttributes(typeof(DelegateCodeAttribute), true);
                if (dcodeattr.Length > 0)
                {
                    FunClass fun = new FunClass();
                    fun.funName = property[n].Name;
                    fun.funCode = delegate(object[] para)
                    {
                        return property[n].Invoke(this, para);
                    };
                    funList.Add(fun);
                }
            }
        }

        private static readonly object syncRoot = new object();
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="funName">方法名称</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object Invoke(string funName, params object[] args)
        {

            List<FunClass> codeList = AppGlobal.codeList;
            if (codeList.FindIndex(x=>x.funName==funName)>-1)
            {
                FunCode fun = codeList.Find(x => x.funName == funName).funCode;
                return fun(args);
            }
            else
            {
                throw new Exception("找不到funName:[" + funName + "]！");
            }
        }

        internal static void Init(IUnityContainer container, List<FunClass> funList)
        {
            funList.Clear();

            IEnumerable<BaseDelegateCode> comms = container.ResolveAll<BaseDelegateCode>();

            foreach (BaseDelegateCode comm in comms)
            {
                comm.LoadFun(funList);
            }
        }
    }
}
