using System;

namespace EFWCoreLib.CoreFrame.SSO
{
    /// <summary>
    /// 单点登录辅助类
    /// </summary>
    public class SsoHelper
    {        
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tokenid"></param>
        /// <returns></returns>
        public static bool SignIn(string userId,string userName, out Guid tokenid)
        {
            TokenInfo existToken = TokenManager.GetToken(userId);
            if (existToken != null)
            {
                tokenid = existToken.tokenId;
                return true;
            }

            TokenInfo token = new TokenInfo()
            {
                tokenId = Guid.NewGuid(),
                IsValid = true,
                CreateTime = DateTime.Now,
                ActivityTime=DateTime.Now,
                UserId = userId,
                UserName=userName//,
                //RemoteIp = Utility.RemoteIp
            };
            tokenid = token.tokenId;
            return TokenManager.AddToken(token);
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool SignOut(Guid token)
        {
            return TokenManager.RemoveToken(token);
        }
        /// <summary>
        /// 是否有效登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static AuthResult ValidateToken(string token)
        {
            Guid guid= ConvertHelper.GetGuid(token, Guid.NewGuid());

            AuthResult result = new AuthResult() { ErrorMsg = "Token不存在" };
            TokenInfo existToken = TokenManager.GetToken(guid);

            if (existToken != null)
            {
                #region 客户端IP不一致
                //if (existToken.RemoteIp != entity.RemoteIp)
                //{
                //    result.ErrorMsg = "客户端IP不一致";
                //}
                #endregion

                if (existToken.IsValid == false)
                {
                    result.ErrorMsg = "Token已过期" + existToken.ActivityTime.ToLongTimeString() + ":" + DateTime.Now.ToLocalTime();
                    TokenManager.RemoveToken(existToken.tokenId);//移除
                }
                else  
                {
                    result.User = new UserInfo() { UserId = existToken.UserId,UserName=existToken.UserName, CreateDate = existToken.CreateTime };
                    result.ErrorMsg = string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        /// 定时触发登录码的活动时间，频率必须小于4分钟
        /// </summary>
        /// <param name="token"></param>
        public static void UserActivity(Guid token)
        {
            TokenInfo existToken = TokenManager.GetToken(token);
            existToken.ActivityTime = DateTime.Now;
        }

        /// <summary>
        /// 用户是否在线
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool IsUserOnline(string userId)
        {
            TokenInfo existToken = TokenManager.GetToken(userId);
            if (existToken != null) return true;
            return false;
        }
    }
}
