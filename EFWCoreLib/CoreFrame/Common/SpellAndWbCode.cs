
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using EFWCoreLib.Properties;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 拼音五笔码生成
    /// </summary>
    public static class SpellAndWbCode
    {
        #region 变量

        /// <summary>
        /// XMLDoc
        /// </summary>
        static XmlDocument xmld = null;

        static Dictionary<string, string> dicPy = null;
        static Dictionary<string, string> dicWb = null;

        #endregion

        #region 私有方法

        /// <summary>
        /// 读取XML文件中数据
        /// </summary>
        /// <returns>返回String[]</returns>
        private static void getXmlData()
        {


            try
            {
                xmld = new XmlDocument();
                xmld.LoadXml(Resources.CodeConfig);

                //得到拼音字典
                dicPy = new Dictionary<string, string>();

                XmlNodeList xnl = xmld.GetElementsByTagName("SpellCode");

                foreach (XmlNode xn in xnl)
                {
                    foreach (XmlNode xnn in xn.ChildNodes)
                    {
                        char[] texts = xnn.InnerText.ToCharArray();
                        for (int i = 0; i < texts.Length; i++)
                        {
                            if (!dicPy.ContainsKey(texts[i].ToString()))
                                dicPy.Add(texts[i].ToString(), xnn.Name);
                        }
                    }
                }

                //得到五笔字典
                dicWb = new Dictionary<string, string>();

                XmlNodeList _xnl = xmld.GetElementsByTagName("WBCode");

                foreach (XmlNode xn in _xnl)
                {
                    foreach (XmlNode xnn in xn.ChildNodes)
                    {
                        char[] texts = xnn.InnerText.ToCharArray();
                        for (int i = 0; i < texts.Length; i++)
                        {
                            if (!dicWb.ContainsKey(texts[i].ToString()))
                                dicWb.Add(texts[i].ToString(), xnn.Name);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("生成拼音五笔码导入资源数据错误！\n" + e.Message);
            }

        }

        #endregion

        #region 公开方法
        /// <summary>
        /// 获得汉语的拼音码
        /// </summary>
        /// <param name="strName">汉字</param>
        /// <param name="start">搜索的开始位置</param>
        /// <param name="end">搜索的结束位置</param>
        /// <returns>汉语反义成字符串，该字符串只包含大写的英文字母</returns>
        public static string GetSpellCode(string strName, int start, int len)
        {
            getXmlData();

            strName = strName.Trim().Replace(" ", "");
            if (string.IsNullOrEmpty(strName))
            {
                return strName;
            }

            strName = strName.Substring(start, len);

            StringBuilder myStr = new StringBuilder();
            foreach (char vChar in strName)
            {
                // 若是字母或数字则直接输出
                if ((vChar >= 'a' && vChar <= 'z') || (vChar >= 'A' && vChar <= 'Z') || (vChar >= '0' && vChar <= '9'))
                    myStr.Append(char.ToUpper(vChar));
                else
                {
                    // 若字符Unicode编码在编码范围则 查汉字列表进行转换输出
                    if (dicPy.ContainsKey(vChar.ToString()))
                    {
                        myStr.Append(dicPy[vChar.ToString()].ToUpper());
                    }
                }
            }
            return myStr.ToString();
        }
        /// <summary>
        /// 获得汉语的拼音码
        /// </summary>
        /// <param name="strName">汉字</param>
        /// <returns></returns>
        public static string GetSpellCode(string strName)
        {
            return GetSpellCode(strName, 0, strName.Length);
        }
        /// <summary>
        /// 获得汉语的五笔码
        /// </summary>
        /// <param name="strName">汉字</param>
        /// <param name="start">搜索的开始位置</param>
        /// <param name="end">搜索的结束位置</param>
        /// <returns>汉语反义成字符串，该字符串只包含大写的英文字母</returns>
        public static string GetWBCode(string strName, int start, int len)
        {
            getXmlData();

            strName = strName.Trim().Replace(" ", "");
            if (string.IsNullOrEmpty(strName))
            {
                return strName;
            }

            strName = strName.Substring(start, len);

            StringBuilder myStr = new StringBuilder();
            foreach (char vChar in strName)
            {
                // 若是字母或数字则直接输出
                if ((vChar >= 'a' && vChar <= 'z') || (vChar >= 'A' && vChar <= 'Z') || (vChar >= '0' && vChar <= '9'))
                    myStr.Append(char.ToUpper(vChar));
                else
                {
                    // 若字符Unicode编码在编码范围则 查汉字列表进行转换输出
                    if (dicWb.ContainsKey(vChar.ToString()))
                    {
                        myStr.Append(dicWb[vChar.ToString()].ToUpper());
                    }
                }
            }
            return myStr.ToString();
        }
        /// <summary>
        /// 获得汉语的五笔码
        /// </summary>
        /// <param name="strName">汉字</param>
        /// <returns></returns>
        public static string GetWBCode(string strName)
        {
            return GetWBCode(strName, 0, strName.Length);
        }

        #endregion
    }
}
