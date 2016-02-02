using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using System.Windows.Forms;
using System.Management;

namespace WinMainUIFrame.Winform.IView
{
    public interface IfrmSetting : IBaseView
    {
        List<InputLanguage> languageList { set; }
        int inputMethod_CH { get; set; }
        int inputMethod_EN { get; set; }

        void loadPrinter(ManagementObjectCollection printers, int first, int second, int three);
        int printfirst { get; }
        int printsecond { get; }
        int printthree { get; }

        bool runacceptMessage { get; set; }
        bool displayWay { get; set; }

        int mainStyle { get; set; }

        string setbackgroundImage { get; set; }
    }
}
