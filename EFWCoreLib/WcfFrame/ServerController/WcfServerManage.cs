using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using System.Reflection;
using EFWCoreLib.CoreFrame.Init;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// WCF通讯服务端管理
    /// </summary>
    public class WcfServerManage
    {
        /// <summary>
        /// 客户端列表
        /// </summary>
        public static Dictionary<string, WCFClientInfo> wcfClientDic = new Dictionary<string, WCFClientInfo>();
        public static bool IsDebug = false;
        public static bool IsHeartbeat = false;
        /// <summary>
        /// 开始服务主机
        /// </summary>
        public static void StartWCFHost()
        {
            ShowHostMsg(DateTime.Now, "WCFHandlerService服务正在初始化...");
            AppGlobal.AppStart();
            ShowHostMsg(DateTime.Now, "WCFHandlerService服务初始化完成");

            if (IsHeartbeat == true)
                StartListenClients();
        }
        /// <summary>
        /// 停止服务主机
        /// </summary>
        public static void StopWCFHost()
        {
            foreach (WCFClientInfo client in wcfClientDic.Values)
            {
                client.IsConnect = false;
            }
        }

        public static string CreateDomain(string sessionId, string ipaddress, DateTime time, IClientService clientService)
        {
            string clientId = Guid.NewGuid().ToString();

            try
            {
                AddClient(sessionId, clientId, ipaddress, time, clientService);
                return clientId;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Source + "：创建客户端运行环境失败！");
            }
        }

        public static bool Heartbeat(string sessionId,string clientId)
        {
            return UpdateClient(sessionId, clientId);
        }

        public static string ProcessRequest(string clientId, string controller, string method, string jsondata)
        {
            try
            {
                if (IsDebug==true)
                    ShowHostMsg(DateTime.Now, "客户端[" + clientId + "]正在执行：" + controller + "." + method + "(" + jsondata + ")");

                object[] paramValue = null;//jsondata?
                string retJson = null;

                WcfServerController wscontroller = ControllerHelper.CreateController(controller);
                lock (wscontroller)
                {
                    wscontroller.ParamJsonData = jsondata;
                    wscontroller.ClientInfo = wcfClientDic[clientId];
                    MethodInfo methodinfo = ControllerHelper.CreateMethodInfo(controller, method, wscontroller);
                    Object retObj = methodinfo.Invoke(wscontroller, paramValue); //带参数方法的调用并返回值
                    if (retObj != null)
                        retJson = retObj.ToString();
                }
                return "{\"flag\":0,\"msg\":" + "\"\"" + ",\"data\":" + retJson + "}";
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");

                if (err.InnerException == null)
                {
                    ShowHostMsg(DateTime.Now, "客户端[" + clientId + "]执行失败：" + controller + "." + method + "(" + jsondata + ")\n错误原因：" + err.Message);
                    return "{\"flag\":1,\"msg\":" + "\"" + err.Message + "\"" + "}";
                }
                else
                {
                    ShowHostMsg(DateTime.Now, "客户端[" + clientId + "]执行失败：" + controller + "." + method + "(" + jsondata + ")\n错误原因：" + err.InnerException.Message);
                    return "{\"flag\":1,\"msg\":" + "\"" + err.InnerException.Message + "\"" + "}";
                }
            }
        }

        public static bool UnDomain(string clientId)
        {
            RemoveClient(clientId);
            hostwcfclientinfoList(wcfClientDic.Values.ToList());
            return true;
        }

        public static void SendBroadcast(string jsondata)
        {
            foreach (WCFClientInfo client in wcfClientDic.Values)
            {
                IClientService mCallBack = client.clientServiceCallBack;
                //?
                mCallBack.ReplyClient(jsondata);
            }
        }

        public static HostWCFClientInfoListHandler hostwcfclientinfoList;
        public static HostWCFMsgHandler hostwcfMsg;
        private static void AddClient(string sessionId, string clientId, string ipaddress, DateTime time, IClientService clientService)
        {
            WCFClientInfo info = new WCFClientInfo();
            info.clientId = clientId;
            info.ipAddress = ipaddress;
            info.startTime = time;
            info.clientServiceCallBack = clientService;
            info.IsConnect = true;
            lock (wcfClientDic)
            {
                wcfClientDic.Add(clientId, info);
            }
            ShowHostMsg(DateTime.Now, "客户端[" + ipaddress + "]已连接WCF服务主机");
            hostwcfclientinfoList(wcfClientDic.Values.ToList());
        }
        private static bool UpdateClient(string sessionId, string clientId)
        {
            if (wcfClientDic.ContainsKey(clientId))
            {
                lock (wcfClientDic)
                {
                    if (wcfClientDic[clientId].IsConnect == false)
                    {
                        ShowHostMsg(DateTime.Now, "客户端[" + clientId + "]已重新连接WCF服务主机");
                        wcfClientDic[clientId].IsConnect = true;
                    }

                    wcfClientDic[clientId].startTime = DateTime.Now;
                    wcfClientDic[clientId].HeartbeatCount += 1;
                }
                hostwcfclientinfoList(wcfClientDic.Values.ToList());
                return true;
            }
            else
                return false;
        }
        private static void RemoveClient(string clientId)
        {
            if (wcfClientDic.ContainsKey(clientId))
            {
                //string ipaddress = wcfClientDic[clientId].ipAddress;
                lock (wcfClientDic)
                {
                    wcfClientDic.Remove(clientId);
                }
                //wcfClientDic[clientId].IsConnect = false;
                ShowHostMsg(DateTime.Now, "客户端[" + clientId + "]已退出断开连接WCF服务主机");
                //hostwcfclientinfoList(wcfClientDic.Values.ToList());
            }
        }
        private static void DisConnectionClient(string clientId)
        {
            if (wcfClientDic.ContainsKey(clientId) && wcfClientDic[clientId].IsConnect == true)
            {
                wcfClientDic[clientId].IsConnect = false;
                ShowHostMsg(DateTime.Now, "客户端[" + clientId + "]已超时断开连接WCF服务主机");
                //hostwcfclientinfoList(wcfClientDic.Values.ToList());
            }
        }
        private static void ShowHostMsg(DateTime time, string text)
        {
            hostwcfMsg(time, text);
        }


        //检测客户端是否在线，超时时间为10s
        static System.Timers.Timer timer;
        private static void StartListenClients()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 500;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        static Object syncObj = new Object();////定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (syncObj)
                {
                    foreach (WCFClientInfo client in wcfClientDic.Values)
                    {
                        if (client.startTime.AddSeconds(10) < DateTime.Now)//断开10秒就置为断开
                        {
                            DisConnectionClient(client.clientId);
                        }

                        if (client.startTime.AddMinutes(10) < DateTime.Now)//断开10分钟直接移除客户端
                        {
                            RemoveClient(client.clientId);
                        }
                    }

                    hostwcfclientinfoList(wcfClientDic.Values.ToList());
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 连接客户端信息
    /// </summary>
    public class WCFClientInfo
    {
        public string clientId { get; set; }
        public string ipAddress { get; set; }
        public DateTime startTime { get; set; }
        public IClientService clientServiceCallBack { get; set; }
        public SysLoginRight LoginRight { get; set; }
        public int HeartbeatCount { get; set; }
        public bool IsConnect { get; set; }
    }

    public delegate void HostWCFClientInfoListHandler(List<WCFClientInfo> dic);
    public delegate void HostWCFMsgHandler(DateTime time, string text);
}
