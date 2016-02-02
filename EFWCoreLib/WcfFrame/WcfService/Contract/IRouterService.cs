using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EFWCoreLib.WcfFrame.WcfService.Contract
{
    /// <summary>
    /// WCF路由服务
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDuplexRouterCallback))]
    public interface IRouterService
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message ProcessMessage(Message requestMessage);
    }

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IDuplexRouterCallback
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void ProcessMessage(Message requestMessage);
    }
}
