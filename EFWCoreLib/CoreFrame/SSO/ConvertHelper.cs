using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EFWCoreLib.CoreFrame.SSO
{
    /// <summary>
    /// 数据转换助手类 
    /// add by wildweeds
    /// 2011-06-08
    /// </summary>
    public class ConvertHelper
    {
        public static int GetInt(string str)
        {
            return GetInt(str, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue">失败时，默认返回值</param>
        /// <returns></returns>
        public static int GetInt(string str,int defValue)
        {
            int rtn = defValue;
            if (!int.TryParse(str, out rtn)) return defValue;
            return rtn;
        }

        public static short GetShort(string str)
        {
            return GetShort(str, 0);
        }

        public static short GetShort(string str, short defValue)
        {
            short rtn = defValue;
            if (!short.TryParse(str, out rtn)) return defValue;
            return rtn;
        }

        public static decimal GetDecimal(string str)
        {
            return GetDecimal(str,0);
        }

        public static decimal GetDecimal(string str, decimal defValue)
        {
            decimal rtn = defValue;
            if (!decimal.TryParse(str, out rtn)) return defValue;
            return rtn;
        }

        public static DateTime GetDateTime(string str)
        {
            return GetDateTime(str, DateTime.Now);
        }
        public static DateTime GetDateTime(string str, DateTime defValue)
        {
            DateTime dt = defValue;
            if (!DateTime.TryParse(str, out dt)) return defValue;
            return dt;
        }

        private static bool isGuid(string str, out Guid guid)
        {
            Match m = Regex.Match(str, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                guid = new Guid(str);
                return true;
            }
            guid = Guid.NewGuid();
            return false;
        }

        public static Guid GetGuid(string str, Guid defValue)
        {
            Guid guid = defValue;
            if (!isGuid(str, out guid)) return defValue;
            return guid;
        }

        public static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder(s.Length + 20);
            sb.Append('\"');
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            sb.Append('\"');
            return sb.ToString();
        }
    }
}
