using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using Books_Winform.Winform.IView;
using Books_Winform.Entity;
using System.Data;

namespace Books_Winform.Winform.Controller
{
    [WinformController(DefaultViewName = "frmBookManager")]//在菜单上显示
    [WinformView(Name = "frmBookManager", DllName = "Books_Winform.Winform.dll", ViewTypeName = "Books_Winform.Winform.ViewForm.frmBookManager")]//控制器关联的界面
    public class bookController : WinformController
    {
        IfrmBookManager _ifrmbookmanager;
        public override void Init()
        {
            _ifrmbookmanager = (IfrmBookManager)DefaultView;
            GetBooks();
        }

        //获取书籍目录
        [WinformMethod]
        public void GetBooks()
        {
            DataTable dt= NewObject<Books>().gettable();
            _ifrmbookmanager.loadbooks(dt);
        }
        //保存
        [WinformMethod]
        public void bookSave()
        {
            _ifrmbookmanager.currBook.BindDb(oleDb, _container,_cache,_pluginName);
            //从界面获取数据保存
            _ifrmbookmanager.currBook.save();
            //从数据库获取数据显示在界面上
            GetBooks();
        }

    }
}

