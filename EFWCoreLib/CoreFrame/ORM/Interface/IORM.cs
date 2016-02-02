
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;

namespace EFWCoreLib.CoreFrame.Orm.Interface
{
    /// <summary>
    /// 简易ORM接口,其中alias别名是指实体配置的自定义标签Table属性Alias的值
    /// </summary>
    interface IORM
    {
        /// <summary>
        /// 插入或更新到数据库
        /// </summary>
        /// <returns></returns>
        int save();
        /// <summary>
        /// 插入或更新到数据库
        /// </summary>
        /// <param name="alias">实体别名</param>
        /// <returns></returns>
        int save(string alias);
        /// <summary>
        /// 根据ID获取实体数据
        /// </summary>
        /// <returns></returns>
        object getmodel();
        /// <summary>
        /// 指定key和别名获取实体数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        object getmodel(object key, string alias);
        /// <summary>
        /// 指定key获取实体数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object getmodel(object key);
        /// <summary>
        /// 指定key和别名删除实体数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        int delete(object key, string alias);
        /// <summary>
        /// 指定key删除实体数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int delete(object key);
        /// <summary>
        /// 根据ID删除实体数据
        /// </summary>
        /// <returns></returns>
        int delete();
        /// <summary>
        /// 获取实体List对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        System.Collections.Generic.List<T> getlist<T>();
        /// <summary>
        /// 获取实体List对象集合，根据where条件过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        System.Collections.Generic.List<T> getlist<T>(string where);
        /// <summary>
        /// 获取实体List对象集合，根据where条件过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageInfo">分页</param>
        /// <param name="where"></param>
        /// <returns></returns>
        System.Collections.Generic.List<T> getlist<T>(PageInfo pageInfo, string where);
        /// <summary>
        /// 获取实体List对象集合，根据where条件过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        System.Collections.Generic.List<T> getlist<T>(PageInfo pageInfo, string where, string alias);
        /// <summary>
        /// 获取实体datatable
        /// </summary>
        /// <returns></returns>
        System.Data.DataTable gettable();
        /// <summary>
        ///  获取实体datatable，where条件过滤
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        System.Data.DataTable gettable(string where);
        /// <summary>
        ///  获取实体datatable，分页
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        System.Data.DataTable gettable(PageInfo pageInfo, string where, string alias);
        /// <summary>
        ///  获取实体datatable，分页
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        System.Data.DataTable gettable(PageInfo pageInfo, string where);
    }
}
