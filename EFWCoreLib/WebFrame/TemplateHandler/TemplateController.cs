using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
#if !CSHARP30
using RazorEngine.Templating;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace EFWCoreLib.WebFrame.TemplateHandler
{
    public class TemplateController : JEasyUIController
    {
        public string ToView(string viewfile)
        {
            FileInfo file = new FileInfo(AppGlobal.AppRootPath+"ModulePlugin\\" + viewfile);
            if (file.Exists == false)
                return "界面文件不存在！";

#if CSHARP30
            TemplateHelper template = new TemplateHelper(file.Directory.FullName);
            foreach (var dic in ViewData)
            {
                template.Put(dic.Key, dic.Value);
            }
            string html = template.BuildString(file.Name);
            return html;
#else
            if (file.Extension.ToLower() == ".cshtml")
            {
                RazorEngine.Razor.SetTemplateBase(typeof(WebAppTemplateBase<>));

                if (ViewData.ContainsKey("Razor_Include") == true)
                {
                    string[] paths = ViewData["Razor_Include"].ToString().Split(new char[] { '|' });
                    for (int i = 0; i < paths.Length; i++)
                    {
                        FileInfo fileinfo = new FileInfo(AppGlobal.AppRootPath + "ModulePlugin\\" + paths[i]);
                        if (fileinfo.Exists == false)
                            throw new Exception(fileinfo.Name + "文件不存在！");
                        string _html = File.ReadAllText(fileinfo.FullName);
                        RazorEngine.Razor.Compile(_html, paths[i]);
                    }
                }
                string html = "";
                //if (AppGlobal.Razor_compile)
                //    html = RazorEngine.Razor.Parse(File.ReadAllText(file.FullName), new { ViewData = ViewData, Request = context.Request, Response = context.Response });
                //else
                html = RazorEngine.Razor.Parse(File.ReadAllText(file.FullName), new { ViewData = ViewData, Request = context.Request, Response = context.Response }, viewfile);
                return html;
            }
            else
            {
                TemplateHelper template = new TemplateHelper(file.Directory.FullName);
                foreach (var dic in ViewData)
                {
                    template.Put(dic.Key, dic.Value);
                }
                string html = template.BuildString(file.Name);
                return html;
            }
#endif

        }
    }

    #if !CSHARP30
    public abstract class WebAppTemplateBase<T> : TemplateBase<T>
    {
        public string ToUrl(string path)
        {
            dynamic m = Model;
            HttpRequest Request = m.Request;
            //HttpRequest Request = Model.GetType().GetProperty("Request").GetValue(Model, null) as HttpRequest;
            //HttpResponse Response = Model.GetType().GetProperty("Response").GetValue(Model, null) as HttpResponse;

            return Request.Url.GetLeftPart(UriPartial.Authority) + (Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + path;
        }

        public string LoadJs(params string[] jsname)
        {
            if (jsname.Length > 0)
            {
                List<string> jslist = jsname.ToList();
                List<string> retlist = new List<string>();
                for (int i = 0; i < jslist.Count; i++)
                {
                    if (jslist[i].Trim() == "jquery")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                    }
                    else if (jslist[i].Trim() == "easyui")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                        if (retlist.Contains("easyui") == false)
                            retlist.Add("easyui");
                    }
                    else if (jslist[i].Trim() == "bootstrap")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                        if (retlist.Contains("bootstrap") == false)
                            retlist.Add("bootstrap");
                    }
                    else if (jslist[i].Trim() == "jquery.query")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                        if (retlist.Contains("jquery.query") == false)
                            retlist.Add("jquery.query");
                    }
                    else if (jslist[i].Trim() == "jquery.json")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                        if (retlist.Contains("jquery.json") == false)
                            retlist.Add("jquery.json");
                    }
                    else if (jslist[i].Trim() == "common")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                        if (retlist.Contains("bootstrap") == false)
                            retlist.Add("bootstrap");
                        if (retlist.Contains("easyui") == false)
                            retlist.Add("easyui");
                    }
                    else if (jslist[i].Trim() == "all")
                    {
                        if (retlist.Contains("jquery") == false)
                            retlist.Add("jquery");
                        if (retlist.Contains("bootstrap") == false)
                            retlist.Add("bootstrap");
                        if (retlist.Contains("easyui") == false)
                            retlist.Add("easyui");
                        if (retlist.Contains("jquery.query") == false)
                            retlist.Add("jquery.query");
                        if (retlist.Contains("jquery.json") == false)
                            retlist.Add("jquery.json");
                    }
                }

                StringBuilder jssb = new StringBuilder();
                Hashtable jslib = getJs();
                foreach (DictionaryEntry n in jslib)
                {
                    if (retlist.Contains(n.Key.ToString()))
                    {
                        jssb.AppendLine(n.Value.ToString());
                    }
                }

                return jssb.ToString();
            }
            return "";
        }

        private Hashtable getJs()
        {
            dynamic m = Model;
            HttpRequest Request = m.Request;
            string rooturl = Request.Url.GetLeftPart(UriPartial.Authority) + (Request.ApplicationPath == "/" ? "" : Request.ApplicationPath);
            Hashtable hash = new Hashtable();
            hash.Add("jquery", "<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/jquery-1.8.0.min.js\"></script>");
            hash.Add("jquery.query", "<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/jquery.query-2.1.7.js\"></script>");
            hash.Add("jquery.json", "<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/jquery.json-2.3.min.js\"></script>");
            hash.Add("bootstrap", "<link href=\"" + rooturl + "/WebPlugin/bootstrap/css/bootstrap.min.css\" rel=\"stylesheet\">\n<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/bootstrap/js/bootstrap.min.js\"></script>");
            hash.Add("easyui", "<link href=\"" + rooturl + "/WebPlugin/jquery-easyui-1.4.1/themes/bootstrap/easyui.css\" rel=\"stylesheet\">\n<link href=\"" + rooturl + "/WebPlugin/jquery-easyui-1.4.1/themes/icon.css\" rel=\"stylesheet\">\n<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/jquery-easyui-1.4.1/jquery.easyui.min.js\"></script>\n<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/jquery-easyui-1.4.1/locale/easyui-lang-zh_CN.js\"></script>\n<script type=\"text/javascript\" src=\"" + rooturl + "/WebPlugin/JQueryCommon2.5.js\"></script>");
            return hash;
        }
    }
#endif
}
