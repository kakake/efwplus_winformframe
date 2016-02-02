using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;

namespace WinMainUIFrame.Winform.IView.EmpUserManager
{
    public interface IfrmWorker:IBaseView
    {
        void loadWorkerGrid(List<BaseWorkers> workerList);
        BaseWorkers currWorker { get; set; }
    }
}
