using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml.Linq;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using EFWCoreLib.WcfFrame.ServerController;

namespace EFWCoreLib.WcfFrame.WcfService
{
    
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any, ValidateMustUnderstand = false)]
    /// <summary>
    /// WCF路由服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Any, ValidateMustUnderstand = false)]
    public class RouterHandlerService : IRouterService
    {
        static public IDictionary<int, RegistrationInfo> RegistrationList = new Dictionary<int, RegistrationInfo>();
        static public IDictionary<string, int> RoundRobinCount = new Dictionary<string, int>();
        public static HostWCFMsgHandler hostwcfMsg;
        public static HostWCFRouterListHandler hostwcfRouter;

        public RouterHandlerService()
        {
            hostwcfMsg(DateTime.Now, "Router服务正在初始化...");
            RegistrationInfo.AddRouterBill();
            hostwcfMsg(DateTime.Now, "Router服务初始化完成");
            hostwcfRouter(RegistrationList.Values.ToList());
        }

        #region IRouterService Members

        /// <summary>
        /// 截获从Client端发送的消息转发到目标终结点并获得返回值给Client端
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public Message ProcessMessage(Message requestMessage)
        {
            //Binding binding = null;
            EndpointAddress endpointAddress = null;
            GetServiceEndpoint(requestMessage, out endpointAddress);
            IDuplexRouterCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexRouterCallback>();
            NetTcpBinding tbinding = new NetTcpBinding("netTcpExpenseService_ForSupplier");
            using (DuplexChannelFactory<IRouterService> factory = new DuplexChannelFactory<IRouterService>(new InstanceContext(null, new DuplexRouterCallback(callback)), tbinding, endpointAddress))
            {

                factory.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));
                IRouterService proxy = factory.CreateChannel();

                using (proxy as IDisposable)
                {
                    // 请求消息记录
                    IClientChannel clientChannel = proxy as IClientChannel;
                    //Console.WriteLine(String.Format("Request received at {0}, to {1}\r\n\tAction: {2}", DateTime.Now, clientChannel.RemoteAddress.Uri.AbsoluteUri, requestMessage.Headers.Action));
                    if (WcfServerManage.IsDebug)
                        hostwcfMsg(DateTime.Now, String.Format("路由请求消息发送：  {0}", clientChannel.RemoteAddress.Uri.AbsoluteUri));
                    // 调用绑定的终结点的服务方法
                    Message responseMessage = proxy.ProcessMessage(requestMessage);

                    // 应答消息记录
                    //Console.WriteLine(String.Format("Reply received at {0}\r\n\tAction: {1}", DateTime.Now, responseMessage.Headers.Action));
                    //Console.WriteLine();
                    //hostwcfMsg(DateTime.Now, String.Format("应答消息： {0}", responseMessage.Headers.Action));
                    return responseMessage;
                }
            }
        }
        /// <summary>
        /// 从注册表容器中根据Message的Action找到匹配的 binding和 endpointaddress
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="binding"></param>
        /// <param name="endpointAddress"></param>
        private void GetServiceEndpoint(Message requestMessage,out EndpointAddress endpointAddress)
        {

            string ns = "http://www.efwplus.cn/";
            string routerID = GetHeaderValue("routerID", ns);
            string cmd = GetHeaderValue("CMD", ns);
            string contractNamespace = requestMessage.Headers.Action.Substring(0, requestMessage.Headers.Action.LastIndexOf("/"));

           

            RegistrationInfo regInfo = null;

            if (RouterHandlerService.RoundRobinCount.ContainsKey(routerID))
            {
                int key = RouterHandlerService.RoundRobinCount[routerID];
                regInfo = RouterHandlerService.RegistrationList[key];
                if (cmd == "Quit")
                {
                    regInfo.ClientNum -= 1;
                }
            }
            else
            {
                //根据指定的协议名称空间从注册表容器中得到注册项列表
                var results = from item in RouterHandlerService.RegistrationList
                              where item.Value.ContractNamespace.Contains(contractNamespace)
                              orderby item.Value.ClientNum ascending
                              select item;
                if (results.Count<KeyValuePair<int, RegistrationInfo>>() > 0)
                {
                    var val = results.First<KeyValuePair<int, RegistrationInfo>>();
                    RouterHandlerService.RoundRobinCount.Add(routerID, val.Key);
                    val.Value.ClientNum += 1;
                    regInfo = val.Value;
                }
            }

            Uri addressUri = new Uri(regInfo.Address);

            //binding = CustomBindConfig.GetRouterBinding(addressUri.Scheme);
            endpointAddress = new EndpointAddress(regInfo.Address);
            //重设Message的目标终结点
            requestMessage.Headers.To = new Uri(regInfo.Address);

            hostwcfRouter(RegistrationList.Values.ToList());
        }

        private string GetHeaderValue(string name, string ns)
        {  
            var headers = OperationContext.Current.IncomingMessageHeaders;  
            var index = headers.FindHeader(name, ns);  
            if (index > -1)  
                return headers.GetHeader<string>(index);  
            else  
                return null;  
        } 

        #endregion

        public static void Dispose()
        {
            RegistrationList.Clear();
            hostwcfRouter(RegistrationList.Values.ToList());
        }
    }

    public class DuplexRouterCallback : IDuplexRouterCallback
    {

        private IDuplexRouterCallback m_clientCallback;

        public DuplexRouterCallback(IDuplexRouterCallback clientCallback)
        {
            m_clientCallback = clientCallback;
        }

        public void ProcessMessage(Message requestMessage)
        {
            this.m_clientCallback.ProcessMessage(requestMessage);
        }
    }

    public delegate void HostWCFRouterListHandler(List<RegistrationInfo> dic);

    [DataContract]
    public class RegistrationInfo
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string Address { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public string ContractName { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public string ContractNamespace { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public int ClientNum { get; set; }

        public override int GetHashCode()
        {
            return this.Address.GetHashCode() + this.ContractName.GetHashCode() + this.ContractNamespace.GetHashCode();
        }

        /// <summary>
        /// 加载路由器的路由表
        /// </summary>
        public static void AddRouterBill()
        {
            string _address = null;
            string _contractname = null;
            string _contractnamespace = null;

            XElement xes = XElement.Load(System.Windows.Forms.Application.StartupPath + "\\Config\\RouterBill.xml");
            IEnumerable<XElement> elements = from e in xes.Elements("record")
                                             select e;
            foreach (XElement xe in elements)
            {
                _address = xe.Element("address").Value;
                _contractname = xe.Element("ContractName").Value;
                _contractnamespace = xe.Element("ContractNamespace").Value;
                RegistrationInfo registrationInfo = new RegistrationInfo { Address = _address, ContractName = _contractname, ContractNamespace = _contractnamespace };
                if (!RouterHandlerService.RegistrationList.ContainsKey(registrationInfo.GetHashCode()))
                {
                    RouterHandlerService.RegistrationList.Add(registrationInfo.GetHashCode(), registrationInfo);
                }
            }

        }
    }

}
