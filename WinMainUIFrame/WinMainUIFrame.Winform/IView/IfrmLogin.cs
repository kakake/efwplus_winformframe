using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;

namespace WinMainUIFrame.Winform.IView
{
    public interface IfrmLogin : IBaseView
    {
        string usercode { get; set; }
        string password { get; set; }

        bool isReLogin { get; set; }
    }
}
