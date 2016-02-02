
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using EFWCoreLib.CoreFrame.Business.Interface;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Unity;

namespace EFWCoreLib.CoreFrame.Business
{

    /// <summary>
    /// 业务抽象基类，后台分层所有对象都继承此对象，包括controller、objectmodel、dao
    /// </summary>
    [Serializable]
    public abstract class AbstractBusines : MarshalByRefObject, ICloneable, IbindDb, INewObject, INewDao
    {
        protected AbstractDatabase _oleDb = null;
        protected Dictionary<string, AbstractDatabase> _oleDbDic = null;
        protected IUnityContainer _container = null;
        protected ICacheManager _cache = null;
        protected string _pluginName = null;

        #region ICloneable 成员

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        #region IBindDb 成员
        public T BindDb<T>(T model)
        {
            (model as IbindDb).BindDb(_oleDb,_container,_cache,_pluginName);
            return model;
        }

        public List<T> ListBindDb<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                (list[i] as IbindDb).BindDb(_oleDb, _container,_cache,_pluginName);
            }
            return list;
        }

        public void BindDb(AbstractBusines busines)
        {
            BindDb(busines.GetDb(), busines.GetUnityContainer(), busines.GetCache(), busines.GetPluginName());
        }
        /// <summary>
        /// 绑定默认数据库
        /// </summary>
        /// <param name="Db"></param>
        /// <param name="container"></param>
        public void BindDb(AbstractDatabase Db, IUnityContainer container, ICacheManager cache, string pluginName)
        {
            if (this is AbstractController)
            {
                if ((this as AbstractController).LoginUserInfo != null && Db!=null)
                    Db.WorkId = (this as AbstractController).LoginUserInfo.WorkId;
            }
            _oleDb = Db;
            _container = container;
            _cache = cache;
            _pluginName = pluginName;
        }

        /// <summary>
        /// 绑定其他数据库
        /// </summary>
        /// <param name="Db"></param>
        public void BindMoreDb(AbstractDatabase Db, string dbkey)
        {
            if (_oleDbDic == null)
                _oleDbDic = new Dictionary<string, AbstractDatabase>();

            lock (_oleDbDic)
            {
                if (_oleDbDic.ContainsKey(dbkey))
                {
                    _oleDbDic.Remove(dbkey);
                }
                _oleDbDic.Add(dbkey, Db);
            }
        }

        public AbstractDatabase GetDb()
        {
            return _oleDb;
        }

        public List<AbstractDatabase> GetMoreDb()
        {
            if (_oleDbDic != null)
                return _oleDbDic.Values.ToList();
            return null;
        }

        public void UseDb()
        {
            if (_oleDbDic != null && _oleDbDic.ContainsKey("default") == true)
            {
                _oleDb = _oleDbDic["default"];
            }
        }

        public void UseDb(string dbkey)
        {
            if (_oleDbDic != null && _oleDbDic.ContainsKey(dbkey) == true)
            {
                _oleDb = _oleDbDic[dbkey];
            }
        }

        public IUnityContainer GetUnityContainer()
        {
            return _container;
        }

        public ICacheManager GetCache()
        {
            return _cache;
        }

        public string GetPluginName()
        {
            return _pluginName;
        }
        #endregion

        #region INewObject 成员
        public T NewObject<T>()
        {
            if (this is AbstractController)
            {
                if ((this as AbstractController).LoginUserInfo != null && _oleDb != null)
                    _oleDb.WorkId = (this as AbstractController).LoginUserInfo.WorkId;
            }
            T t = FactoryModel.GetObject<T>(_oleDb,_container,_cache,_pluginName, null);
            return t;
        }

        public T NewObject<T>(string unityname)
        {
            if (this is AbstractController)
            {
                if ((this as AbstractController).LoginUserInfo != null && _oleDb != null)
                    _oleDb.WorkId = (this as AbstractController).LoginUserInfo.WorkId;
            }
            T t = FactoryModel.GetObject<T>(_oleDb,_container,_cache,_pluginName, unityname);
            return t;
        }

        #endregion

        #region INewDao 成员

        public T NewDao<T>()
        {
            if (this is AbstractController)
            {
                if ((this as AbstractController).LoginUserInfo != null && _oleDb != null)
                    _oleDb.WorkId = (this as AbstractController).LoginUserInfo.WorkId;
            }
            T t = FactoryModel.GetObject<T>(_oleDb,_container,_cache,_pluginName, null);
            return t;
        }

        public T NewDao<T>(string unityname)
        {
            if (this is AbstractController)
            {
                if ((this as AbstractController).LoginUserInfo != null && _oleDb != null)
                    _oleDb.WorkId = (this as AbstractController).LoginUserInfo.WorkId;
            }
            T t = FactoryModel.GetObject<T>(_oleDb,_container,_cache,_pluginName, unityname);
            return t;
        }

        #endregion

       

    }
}
