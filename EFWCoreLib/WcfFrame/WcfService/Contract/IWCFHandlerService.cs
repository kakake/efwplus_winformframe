using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EFWCoreLib.WcfFrame.WcfService.Contract
{
    /// <summary>
    /// WCF处理服务
    /// </summary>
    [ServiceKnownType(typeof(DBNull))]
    [ServiceContract(Namespace = "http://www.efwplus.cn/",Name = "WCFHandlerService", SessionMode = SessionMode.Required, CallbackContract = typeof(IClientService))]
    public interface IWCFHandlerService
    {
        /// <summary>
        /// 创建客户端运行环境
        /// </summary>
        /// <returns>返回mProxyID</returns>
        [OperationContract(IsOneWay = false)]
        string CreateDomain(string ipAddress);
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="paramValue">参数</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        string ProcessRequest(string mProxyID,string controller, string method, string jsondata);
        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="mProxyID"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        /// <param name="jsondata"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false,AsyncPattern = true)]
        IAsyncResult BeginProcessRequest(string mProxyID, string controller, string method, string jsondata, AsyncCallback callback, object asyncState);

        //[OperationContract(IsOneWay = false)]
        string EndProcessRequest(IAsyncResult result);

        /// <summary>
        /// 卸载制定客户端环境
        /// </summary>
        /// <param name="mProxyID"></param>
        [OperationContract(IsOneWay = false)]
        bool UnDomain(string mProxyID);

        /// <summary>
        /// WCF心跳检测
        /// </summary>
        /// <param name="mProxyID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false)]
        bool Heartbeat(string mProxyID);
        /// <summary>
        /// 发送广播消息
        /// </summary>
        /// <param name="jsondata"></param>
        [OperationContract(IsOneWay = true)]
        void SendBroadcast(string jsondata); 
    }

    /// <summary>
    /// 回调Winform界面契约
    /// </summary>
    [ServiceKnownType(typeof(System.DBNull))]
    [ServiceContract(Name = "ClientService")]
    public interface IClientService
    {
        /// <summary>
        /// 回调界面方法
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="paramValue">参数</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        void ReplyClient(string jsondata);
    }
}
