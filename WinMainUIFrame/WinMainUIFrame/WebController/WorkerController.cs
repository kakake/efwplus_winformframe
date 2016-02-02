using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using Newtonsoft.Json;
using WinMainUIFrame.Entity;

namespace WebUIFrame.WebController
{
    [WebController]
    public class WorkerController : JEasyUIController
    {
        [WebMethod]
        public void GetWorker()
        {
            try
            {
                List<BaseWorkers> workerlist = NewObject<BaseWorkers>().getlist<BaseWorkers>();
                JsonResult = ToGridJson(workerlist);
            }
            catch (Exception ex)
            {
                JsonResult = RetError(ex.Message);
            }
        }
    }
}