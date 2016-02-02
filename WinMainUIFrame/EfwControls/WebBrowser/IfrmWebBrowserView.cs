using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfwControls.WebBrowser
{
    public interface IfrmWebBrowserView 
    {
        string Url { get; set; }
        void NavigateUrl();
    }
}
