using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using WinMainUIFrame.Winform.IView;

namespace WinMainUIFrame.Winform.Controller
{
    [WinformController(DefaultViewName = "form1")]//与系统菜单对应
    [WinformView(Name = "form1", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.Form1")]
    public class from1winController : WinformController
    {
        IForm1 frm;
        public override void Init()
        {
            frm = (IForm1)DefaultView;
        }

        //获取书籍目录
        [WinformMethod]
        public string GetHello()
        {
            return "hello world !";
        }
    }
}
