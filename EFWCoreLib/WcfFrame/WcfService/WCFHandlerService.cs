using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using EFWCoreLib.WcfFrame.ServerController;
using System.Threading;

namespace EFWCoreLib.WcfFrame.WcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant,UseSynchronizationContext=false, IncludeExceptionDetailInFaults = true)]
    public class WCFHandlerService : MarshalByRefObject, IWCFHandlerService
    {
        //public static List<IClientService> mCallBackList;

        public WCFHandlerService()
        {
            //mCallBackList = new List<IClientService>();

            //WcfServerManage.StartWCFHost();
        }

        #region IapiWCFHandlerService 成员

        public string CreateDomain(string ipAddress)
        {
            //客户端回调
            IClientService mCallBack = OperationContext.Current.GetCallbackChannel<IClientService>();

            string ClientID= WcfServerManage.CreateDomain(OperationContext.Current.SessionId, ipAddress, DateTime.Now, mCallBack);
          
            //mCallBackList.Add(mCallBack);

            OperationContext.Current.Channel.Closing += new EventHandler(Channel_Closing);
            //OperationContext.Current.Channel.Faulted += new EventHandler(Channel_Faulted);
            return ClientID;
        }


        public string ProcessRequest(string mProxyID, string controller, string method, string jsondata)
        {
            return WcfServerManage.ProcessRequest(mProxyID, controller, method, jsondata);
        }

        //异步请求
        public IAsyncResult BeginProcessRequest(string mProxyID, string controller, string method, string jsondata, AsyncCallback callback, object asyncState)
        {
            return new CompletedAsyncResult<string>(WcfServerManage.ProcessRequest(mProxyID, controller, method, jsondata));
        }

        public string EndProcessRequest(IAsyncResult result)
        {
            CompletedAsyncResult<string> ret = result as CompletedAsyncResult<string>;
            return ret.Data as string;
        }

        public bool UnDomain(string mProxyID)
        {
            return WcfServerManage.UnDomain(mProxyID);
        }

        public bool Heartbeat(string mProxyID)
        {
            return WcfServerManage.Heartbeat(OperationContext.Current.SessionId, mProxyID);
        }

        public void SendBroadcast(string jsondata)
        {
            WcfServerManage.SendBroadcast(jsondata);
        }

        #endregion

        void Channel_Faulted(object sender, EventArgs e)
        {
            //throw new Exception("WCF通道出错");
        }

        void Channel_Closing(object sender, EventArgs e)
        {
            //Loader.ShowHostMsg(DateTime.Now, "WCF通道关闭");
            //throw new Exception("WCF通道关闭");
        }

    }


    class CompletedAsyncResult<T> : IAsyncResult
    {
        T data;

        public CompletedAsyncResult(T data)
        { this.data = data; }

        public T Data
        { get { return data; } }

        #region IAsyncResult Members
        public object AsyncState
        { get { return (object)data; } }

        public WaitHandle AsyncWaitHandle
        { get { throw new Exception("The method or operation is not implemented."); } }

        public bool CompletedSynchronously
        { get { return true; } }

        public bool IsCompleted
        { get { return true; } }
        #endregion
    }

}
