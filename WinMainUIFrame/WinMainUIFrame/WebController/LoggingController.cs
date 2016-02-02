using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.EntLib;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using WinMainUIFrame.ObjectModel.UserLogin;

namespace WebUIFrame.WebController
{
    [WebController]
    public class LoggingController : JEasyUIController
    {
        //登录
        [WebMethod]
        public void Login()
        {
            try
            {
                string strUsertitle = FormData["txtUserName"];
                string strPassWord = FormData["txtUserPWD"];
                string strIP = FormData["txtUserIP"];
                string strMac = FormData["txtMac"];

                User user = NewObject<User>();
                bool islogin = user.UserLogin(strUsertitle, strPassWord);


                if (islogin)
                {

                    BaseUser EbaseUser = user.GetUser(strUsertitle);
                    SysLoginRight right = new SysLoginRight();
                    right.UserId = EbaseUser.UserId;
                    right.EmpId = EbaseUser.EmpId;
                    right.WorkId = EbaseUser.WorkId;
                    oleDb.WorkId = EbaseUser.WorkId;

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
                            if (PutOutData.ContainsKey("RoleUser")) PutOutData.Remove("RoleUser");
                            PutOutData.Add("RoleUser", right);
                            if (PutOutData.ContainsKey("WorkId")) PutOutData.Remove("WorkId");
                            PutOutData.Add("WorkId", right.WorkId);

                            DataTable ListModeules = ConvertExtend.ToDataTable(NewObject<Module>().GetModuleList(right.UserId).OrderBy(x => x.SortId).ToList());
                            List<BaseMenu> listM = NewObject<Menu>().GetMenuList(right.UserId);
                            //给菜单url后面绑定MenuId参数
                            for (int i = 0; i < listM.Count; i++)
                            {
                                listM[i].UrlName = ConvertExtend.UrlAddParams(listM[i].UrlName, "MenuId", listM[i].MenuId.ToString());
                            }
                            DataTable ListMenus = ConvertExtend.ToDataTable(listM);
                            DataTable ListDepts = ConvertExtend.ToDataTable(NewObject<Dept>().GetHaveDept(right.EmpId));
                            DataTable ListGroups = ConvertExtend.ToDataTable(NewObject<Group>().GetGroupList(right.UserId));

                            if (PutOutData.ContainsKey("ListModeules")) PutOutData.Remove("ListModeules");
                            PutOutData.Add("ListModeules", ListModeules);
                            if (PutOutData.ContainsKey("ListMenus")) PutOutData.Remove("ListMenus");
                            PutOutData.Add("ListMenus", ListMenus);
                            if (PutOutData.ContainsKey("ListDepts")) PutOutData.Remove("ListDepts");
                            PutOutData.Add("ListDepts", ListDepts);
                            if (PutOutData.ContainsKey("ListGroups")) PutOutData.Remove("ListGroups");
                            PutOutData.Add("ListGroups", ListGroups);

                            JsonResult = RetSuccess("");
                        }
                        else
                        {
                            //throw new Exception("登录用户的当前机构注册码不正确！");
                            JsonResult = RetError("登录用户的当前机构注册码不正确！");
                        }
                    }
                    else
                    {
                        //throw new Exception("登录用户的当前机构还未启用！");
                        JsonResult = RetError("登录用户的当前机构还未启用！");
                    }
                }
                else
                {
                    //throw new Exception("输入的用户名密码不正确！");
                    JsonResult = RetError("输入的用户名密码不正确！");
                }
                //JsonResult = RetSuccess("");
            }
            catch (Exception err)
            {
                //ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                JsonResult = RetError("登录失败，请联系系统管理员！" + err.Message);
            }
        }
        //修改密码
        [WebMethod]
        public void ChangePassWord()
        {
            try
            {
                string oldpassword = FormData["oldpasswd"];
                string password = FormData["newpasswd"];

                SysLoginRight slr = (SysLoginRight)sessionData["RoleUser"];

                bool b = NewObject<User>().AlterPassWrod(LoginUserInfo.UserId, oldpassword, password);
                if (b == true)
                    JsonResult = RetSuccess("修改成功！");
                else
                    JsonResult = RetError("输入的原始密码有误!");
            }
            catch (Exception e)
            {
                JsonResult = RetError(e.Message);
            }
        }
    }
}