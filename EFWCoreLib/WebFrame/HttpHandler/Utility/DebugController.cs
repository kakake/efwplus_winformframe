using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using Newtonsoft.Json;

namespace EFWCoreLib.WebFrame.HttpHandler.Utility
{
    /// <summary>
    /// 控制器调试
    /// </summary>
    [WebController]
    public class DebugController : WebHttpController
    {
        private List<Hashtable> getHashList(string[] str)
        {
            List<Hashtable> hashlist = new List<Hashtable>();
            for (int i = 0; i < str.Length; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("code", i);
                hash.Add("Name", str[i]);
                hashlist.Add(hash);
            }
            return hashlist;
        }

        [WebMethod]
        public void GetControllerClassNameData()
        {
            //List<Cmd_Controller> cmd = (List<Cmd_Controller>)AppGlobal.cache.GetData("cmdWebController");
            //List<string> classlist =new List<string>();
            //for (int i = 0; i < cmd.Count; i++)
            //{
            //    classlist.Add(cmd[i].controllerName);
            //}
            //context.Response.Charset = "UTF-8";
            ////把数据输出到页面
            //context.Response.Write(JavaScriptConvert.SerializeObject(getHashList(classlist.ToArray())));
        }

        [WebMethod]
        public void GetControllerMethodNameData()
        {
            //List<string> methodlist = new List<string>();

            //string ClassName = ParamsData["ClassName"];
            //List<Cmd_Controller> cmd = (List<Cmd_Controller>)AppGlobal.cache.GetData("cmdWebController");
            //Cmd_Controller cmdC = cmd.Find(x => x.controllerName == ClassName);
            //if (cmdC != null)
            //{
            //    for (int i = 0; i < cmdC.cmdMethod.Count; i++)
            //    {
            //        methodlist.Add(cmdC.cmdMethod[i].methodName);
            //    }
            //}

            //context.Response.Charset = "UTF-8";
            ////把数据输出到页面
            //context.Response.Write(JavaScriptConvert.SerializeObject(getHashList(methodlist.ToArray())));
        }
    }
}
