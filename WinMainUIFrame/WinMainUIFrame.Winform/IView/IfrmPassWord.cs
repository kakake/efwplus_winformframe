using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;

namespace WinMainUIFrame.Winform.IView
{
    public interface IfrmPassWord : IBaseView
    {
        string oldpass { get;  }
        string newpass { get;  }
        string newpass2 { get;  }

        void clearPass();
    }
}
