using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using Books_Winform.Entity;
using System.Data;

namespace Books_Winform.Winform.IView
{
    public interface IfrmBookManager : IBaseView
    {
        //给网格加载数据
        void loadbooks(DataTable dt);
        //当前维护的书籍
        Books currBook { get; set; }
    }
}
