using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using System.Data;

namespace WinMainUIFrame.Winform.IView
{
    public interface IfrmUnitData:IBaseView
    {
        void loadUnitData(DataTable dt);
        void UnitDataShowDialog();
    }
}
