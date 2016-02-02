using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;

namespace WinMainUIFrame.Winform.IView
{
    public interface IFrmReportDesigner:IBaseView
    {
        void loadreportfile(string reportfile);
    }
}
