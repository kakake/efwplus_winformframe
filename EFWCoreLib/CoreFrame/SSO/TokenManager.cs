using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EFWCoreLib.CoreFrame.SSO
{
    public class TokenManager
    {
        private const int _TimerPeriod = 60000;//60秒
        private static Timer thTimer;

        static List<TokenInfo> tokenList = null;

        static TokenManager()
        {
            tokenList = new List<TokenInfo>();
            thTimer = new Timer(_ThreadTimerCallback, null, _TimerPeriod, _TimerPeriod);
        }

        public static bool AddToken(TokenInfo entity)
        {
            tokenList.Add(entity);
            return true;
        }

        public static bool RemoveToken(Guid token)
        {
            TokenInfo existToken = tokenList.SingleOrDefault(t => t.tokenId ==token);
            if (existToken != null)
            {
                tokenList.Remove(existToken);
                return true;
            }

            return false;
        }

        public static TokenInfo GetToken(Guid token)
        {
            TokenInfo existToken = tokenList.SingleOrDefault(t => t.tokenId == token);
            return existToken;
        }

        public static TokenInfo GetToken(string userId)
        {
            TokenInfo existToken = tokenList.SingleOrDefault(t => (t.UserId == userId && t.IsValid==true));
            return existToken;
        }

        private static void _ThreadTimerCallback(Object state)
        {
            DateTime now = DateTime.Now;

            Monitor.Enter(tokenList);
            try
            {
                // Searching for expired users
                foreach (TokenInfo t in tokenList)
                {
                    if (((TimeSpan)(now - t.ActivityTime)).TotalMilliseconds > _TimerPeriod)
                    {
                        t.IsValid = false;//失效
                    }
                }
            }
            finally
            {
                Monitor.Exit(tokenList);
            }
        }
    }
}
