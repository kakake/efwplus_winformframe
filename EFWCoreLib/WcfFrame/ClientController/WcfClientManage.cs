using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using EFWCoreLib.CoreFrame.Init;
using System.Net;
using System.Text.RegularExpressions;

namespace EFWCoreLib.WcfFrame.ClientController
{
    /// <summary>
    /// WCF通讯客户端管理
    /// </summary>
    public class WcfClientManage
    {
        private static readonly string myNamespace = "http://www.efwplus.cn/";
        /// <summary>
        /// 创建wcf服务连接
        /// </summary>
        /// <param name="mainfrm"></param>
        public static void CreateConnection(IClientService client)
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding("NetTcpBinding_WCFHandlerService");
                //binding.OpenTimeout = TimeSpan.FromSeconds(10);
                //binding.TransferMode = TransferMode.Buffered;
                DuplexChannelFactory<IWCFHandlerService> mChannelFactory = new DuplexChannelFactory<IWCFHandlerService>(client, binding, System.Configuration.ConfigurationSettings.AppSettings["WCF_endpoint"]);
                IWCFHandlerService wcfHandlerService = mChannelFactory.CreateChannel();

                string routerID;
                string mProxyID;
                using (var scope = new OperationContextScope(wcfHandlerService as IContextChannel))
                {
                    // 注意namespace必须和ServiceContract中定义的namespace保持一致，默认是：http://tempuri.org  
                    //var myNamespace = "http://www.efwplus.cn/";
                    // 注意Header的名字中不能出现空格，因为要作为Xml节点名。  
                    routerID = Guid.NewGuid().ToString();
                    var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("routerID", myNamespace, routerID);
                    OperationContext.Current.OutgoingMessageHeaders.Add(router);
                    mProxyID = wcfHandlerService.CreateDomain(getLocalIPAddress());
                }


                if (AppGlobal.cache.Contains("WCFClientID")) AppGlobal.cache.Remove("WCFClientID");
                if (AppGlobal.cache.Contains("WCFService")) AppGlobal.cache.Remove("WCFService");
                if (AppGlobal.cache.Contains("ClientService")) AppGlobal.cache.Remove("ClientService");
                if (AppGlobal.cache.Contains("routerID")) AppGlobal.cache.Remove("routerID");

                AppGlobal.cache.Add("routerID", routerID);
                AppGlobal.cache.Add("WCFClientID", mProxyID);
                AppGlobal.cache.Add("WCFService", wcfHandlerService);
                AppGlobal.cache.Add("ClientService", client);
                

                //开启发送心跳
                if (timer == null)
                    StartTimer();
                else
                    timer.Start();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        /// <summary>
        /// 重新连接wcf服务
        /// </summary>
        /// <param name="mainfrm"></param>
        public static void ReConnection()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding("NetTcpBinding_WCFHandlerService");
                //binding.OpenTimeout = TimeSpan.FromSeconds(10);
                //binding.TransferMode = TransferMode.Buffered;
                DuplexChannelFactory<IWCFHandlerService> mChannelFactory = new DuplexChannelFactory<IWCFHandlerService>(AppGlobal.cache.GetData("ClientService") as IClientService, binding, System.Configuration.ConfigurationSettings.AppSettings["WCF_endpoint"]);
                IWCFHandlerService wcfHandlerService = mChannelFactory.CreateChannel();

                using (var scope = new OperationContextScope(wcfHandlerService as IContextChannel))
                {
                    var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("routerID", myNamespace, AppGlobal.cache.GetData("routerID").ToString());
                    OperationContext.Current.OutgoingMessageHeaders.Add(router);
                    wcfHandlerService.Heartbeat(AppGlobal.cache.GetData("WCFClientID").ToString());
                }

                if (AppGlobal.cache.Contains("WCFService")) AppGlobal.cache.Remove("WCFService");
                AppGlobal.cache.Add("WCFService", wcfHandlerService);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <returns></returns>
        public static bool Heartbeat()
        {
            try
            {
                bool ret = false;
                IWCFHandlerService _wcfService = AppGlobal.cache.GetData("WCFService") as IWCFHandlerService;
                using (var scope = new OperationContextScope(_wcfService as IContextChannel))
                {
                    var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("routerID", myNamespace, AppGlobal.cache.GetData("routerID").ToString());
                    OperationContext.Current.OutgoingMessageHeaders.Add(router);
                    ret = _wcfService.Heartbeat(AppGlobal.cache.GetData("WCFClientID").ToString());
                }

                if (ret == false)//表示服务主机关闭过，丢失了clientId，必须重新创建连接
                {
                    CreateConnection(AppGlobal.cache.GetData("ClientService") as IClientService);
                }
                return ret;
            }
            catch
            {
                ReConnection();//连接服务主机失败，重连
                return false;
            }
        }

        /// <summary>
        /// 向服务发送请求
        /// </summary>
        /// <param name="controller">控制器名称</param>
        /// <param name="method">方法名称</param>
        /// <param name="jsondata">数据</param>
        /// <returns>返回Json数据</returns>
        public static string Request(string controller, string method, string jsondata)
        {
            IWCFHandlerService _wcfService = AppGlobal.cache.GetData("WCFService") as IWCFHandlerService;
            string retJson;
            using (var scope = new OperationContextScope(_wcfService as IContextChannel))
            {
                var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("routerID", myNamespace, AppGlobal.cache.GetData("routerID").ToString());
                OperationContext.Current.OutgoingMessageHeaders.Add(router);
                retJson = _wcfService.ProcessRequest(AppGlobal.cache.GetData("WCFClientID").ToString(), controller, method, jsondata);
            }

            return retJson;
        }

        /// <summary>
        /// 向服务发送请求
        /// </summary>
        /// <param name="controller">控制器名称</param>
        /// <param name="method">方法名称</param>
        /// <param name="jsondata">数据</param>
        /// <returns>返回Json数据</returns>
        public static IAsyncResult RequestAsync(string controller, string method, string jsondata, Action<string> action)
        {
            IWCFHandlerService _wcfService = AppGlobal.cache.GetData("WCFService") as IWCFHandlerService;
            //string retJson;
            IAsyncResult result = null;
            using (var scope = new OperationContextScope(_wcfService as IContextChannel))
            {
                var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("routerID", myNamespace, AppGlobal.cache.GetData("routerID").ToString());
                OperationContext.Current.OutgoingMessageHeaders.Add(router);

                AsyncCallback callback = delegate(IAsyncResult r)
                {
                    string retJson= _wcfService.EndProcessRequest(r);
                    action(retJson);
                };
                result = _wcfService.BeginProcessRequest(AppGlobal.cache.GetData("WCFClientID").ToString(), controller, method, jsondata, callback, null);
            }

            //return retJson;
            return result;
        }


        /// <summary>
        /// 卸载连接
        /// </summary>
        public static void UnConnection()
        {
            if (AppGlobal.cache.GetData("WCFClientID") == null) return;

            bool b = false;
            string mClientID = AppGlobal.cache.GetData("WCFClientID").ToString();
            IWCFHandlerService mWcfService = AppGlobal.cache.GetData("WCFService") as IWCFHandlerService;
            if (mClientID != null)
            {
                using (var scope = new OperationContextScope(mWcfService as IContextChannel))
                {
                    var router = System.ServiceModel.Channels.MessageHeader.CreateHeader("routerID", "http://www.efwplus.cn/", AppGlobal.cache.GetData("routerID").ToString());
                    OperationContext.Current.OutgoingMessageHeaders.Add(router);
                    var cmd = System.ServiceModel.Channels.MessageHeader.CreateHeader("CMD", "http://www.efwplus.cn/", "Quit");
                    OperationContext.Current.OutgoingMessageHeaders.Add(cmd);
                    b = mWcfService.UnDomain(mClientID);
                }
            }
        }
        /// <summary>
        /// 广播消息接收
        /// </summary>
        /// <param name="jsondata"></param>
        public static void ReplyClient(string jsondata)
        {

        }

        //向服务端发送心跳，间隔时间为5s
        static System.Timers.Timer timer;
        static void StartTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 5000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        static Object syncObj = new Object();////定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (syncObj)
            {
                try
                {
                    Heartbeat();
                    //if (Heartbeat() == false)
                    //{
                    //    ReConnection();
                    //}
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
            }
        }
        static string getLocalIPAddress()
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            string myip = "";
            foreach (IPAddress ip in IpEntry.AddressList)
            {
                if (Regex.IsMatch(ip.ToString(), @"\d{0,3}\.\d{0,3}\.\d{0,3}\.\d{0,3}"))
                {
                    myip = ip.ToString();
                    break;
                }
            }
            return myip;
        }
    }
}
