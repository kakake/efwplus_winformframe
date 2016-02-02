using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EFWCoreLib.CoreFrame.Init;
using System.IO;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WebFrame.HttpHandler.Controller;

namespace EFWCoreLib.WebFrame.HttpHandler.Utility
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [WebController]
    public class UploadifyController : WebHttpController
    {
        [WebMethod]
        public void Upload()
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath = AppGlobal.AppRootPath + "userfiles\\";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                file.SaveAs(uploadPath + file.FileName);
                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                context.Response.Write("1");
            }
            else
            {
                context.Response.Write("0");
            }  
        }
    }
}
