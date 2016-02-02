using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using WinMainUIFrame.ObjectModel.UserLogin;

namespace WinMainUIFrame.WcfController
{
    [WCFController]
    public class LoginController : JsonWcfServerController
    {
        [WCFMethod]
        public string UserLogin()
        {
            //UseDb("SQL20052");
            //string strsql = @"select count(*) from appcenter";
            //object o = oleDb.GetDataResult(strsql);
            //UseDb();

            string usercode = ToArray(ParamJsonData)[0].ToString();
            string password = ToArray(ParamJsonData)[1].ToString();

            User user = NewObject<User>();
            bool islogin = user.UserLogin(usercode, password);

            if (islogin)
            {
                BaseUser EbaseUser = user.GetUser(usercode);
                SysLoginRight right = new SysLoginRight();
                right.UserId = EbaseUser.UserId;
                right.EmpId = EbaseUser.EmpId;
                right.WorkId = EbaseUser.WorkId;

                Dept dept = NewObject<Dept>();
                BaseDept EbaseDept = dept.GetDefaultDept(EbaseUser.EmpId);
                if (EbaseDept != null)
                {
                    right.DeptId = EbaseDept.DeptId;
                    right.DeptName = EbaseDept.Name;
                }

                BaseEmployee EbaseEmp = (BaseEmployee)NewObject<BaseEmployee>().getmodel(EbaseUser.EmpId);
                right.EmpName = EbaseEmp.Name;

                BaseWorkers EbaseWork = (BaseWorkers)NewObject<BaseWorkers>().getmodel(EbaseUser.WorkId);
                right.WorkName = EbaseWork.WorkName;

                if (EbaseWork.DelFlag == 0)
                {
                    string regkey = EbaseWork.RegKey;
                    DESEncryptor des = new DESEncryptor();
                    des.InputString = regkey;
                    des.DesDecrypt();
                    string[] ret = (des.OutString == null ? "" : des.OutString).Split(new char[] { '|' });
                    if (ret.Length == 2 && ret[0] == EbaseWork.WorkName && Convert.ToDateTime(ret[1]) > DateTime.Now)
                    {
                        ClientInfo.LoginRight = right;//缓存登录用户信息

                        Object[] retObjs = new Object[]{
                            right.EmpName,right.DeptName,right.WorkName
                            ,NewObject<Module>().GetModuleList(right.UserId).OrderBy(x => x.SortId).ToList()
                            ,NewObject<Menu>().GetMenuList(right.UserId)
                            ,NewObject<Dept>().GetHaveDept(right.EmpId)
                            ,right
                        };
                        return ToJson(retObjs);
                    }
                    else
                    {
                        throw new Exception("登录用户的当前机构注册码不正确！");
                    }
                }
                else
                {
                    throw new Exception("登录用户的当前机构还未启用！");
                }
            }
            else
            {
                throw new Exception("输入的用户名密码不正确！");
            }
        }
         [WCFMethod]
        public string SaveReDept()
        {
            string deptId = ToArray(ParamJsonData)[0].ToString();
            string deptName = ToArray(ParamJsonData)[1].ToString();
            ClientInfo.LoginRight.DeptId = Convert.ToInt32(deptId);
            ClientInfo.LoginRight.DeptName = deptName;
            return ToJson(true);
        }
         [WCFMethod]
        public string AlterPass()
        {
            string userId = ToArray(ParamJsonData)[0].ToString();
            string oldpass = ToArray(ParamJsonData)[1].ToString();
            string newpass = ToArray(ParamJsonData)[2].ToString();
            bool b = NewObject<User>().AlterPassWrod(Convert.ToInt32(userId), oldpass, newpass);
            return ToJson(b);
        }
        // [WCFMethod]
        //public string GetNotReadMessages()
        //{
        //    List<BaseMessage> listmsg= NewObject<EFWBaseLib.ObjectModel.SysMessage.Message>().GetNotReadMessages(GetSysLoginRight.UserId, GetSysLoginRight.DeptId, GetSysLoginRight.WorkId);
        //    return ToJson(listmsg);
        //}
        // [WCFMethod]
        // public string MessageRead()
        // {
        //     int messageId = ToInt32(ParamJsonData);

        //     NewDao<EFWBaseLib.Dao.MessageDao>().MessageRead(messageId, GetSysLoginRight.UserId);
        //     return ToJson(true);
        // }
    }
}
