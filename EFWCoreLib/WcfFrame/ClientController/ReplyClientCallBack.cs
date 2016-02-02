using EFWCoreLib.WcfFrame.WcfService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.WcfFrame.ClientController
{
    public class ReplyClientCallBack: IClientService
    {
        //回调委托
        public Action<string> ReplyClientAction {
            get; 
            set; 
        }

        #region IClientService 成员

        public void ReplyClient(string jsondata)
        {
            if (ReplyClientAction != null)
            {
                lock (ReplyClientAction)
                {
                    ReplyClientAction(jsondata);
                }
            }
        }

        #endregion
    }
}
