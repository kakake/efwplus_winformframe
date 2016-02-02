using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Init;

namespace WinMainUIFrame.ObjectModel
{
//    public class BaseLibDelegateCode : BaseDelegateCode
//    {
//        [DelegateCode]
//        public DataTable getPageRight(int menuId, int userId)
//        {
//            DataTable data = null;
//            string strsql = @"SELECT Code,Name,
//                                            (
//                                            CASE WHEN 
//                                            (SELECT COUNT(*) FROM BaseGroupPage a 
//                                            LEFT JOIN BaseGroupUser b ON a.GroupId=b.GroupId
//                                            WHERE b.UserId={1} AND a.PageId=P.Id)>0 
//                                            THEN 1 ELSE 0 END
//                                            ) Val
//                                             FROM BasePageMenu P WHERE MenuId={0}";
//            strsql = string.Format(strsql, menuId, userId);
//            data = oleDb.GetDataTable(strsql);
//            return data;
//        }
//    }
}
