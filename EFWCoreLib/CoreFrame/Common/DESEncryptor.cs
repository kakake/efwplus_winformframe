
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public class DESEncryptor
    {
        #region 私有成员
        /**/
        /// <summary>
        /// 输入字符串
        /// </summary>
        private string inputString = null;
        /**/
        /// <summary>
        /// 输出字符串
        /// </summary>
        private string outString = null;
        /**/
        /// <summary>
        /// 输入文件路径
        /// </summary>
        private string inputFilePath = null;
        /**/
        /// <summary>
        /// 输出文件路径
        /// </summary>
        private string outFilePath = null;
        /**/
        /// <summary>
        /// 加密密钥
        /// </summary>
        private string encryptKey = null;
        /**/
        /// <summary>
        /// 解密密钥
        /// </summary>
        private string decryptKey = null;
        /**/
        /// <summary>
        /// 提示信息
        /// </summary>
        private string noteMessage = null;
        #endregion

        #region 公共属性
        /**/
        /// <summary>
        /// 输入字符串
        /// </summary>
        public string InputString
        {
            get { return inputString; }
            set { inputString = value; }
        }
        /**/
        /// <summary>
        /// 输出字符串
        /// </summary>
        public string OutString
        {
            get { return outString; }
            set { outString = value; }
        }
        /**/
        /// <summary>
        /// 输入文件路径
        /// </summary>
        public string InputFilePath
        {
            get { return inputFilePath; }
            set { inputFilePath = value; }
        }
        /**/
        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string OutFilePath
        {
            get { return outFilePath; }
            set { outFilePath = value; }
        }
        /**/
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string EncryptKey
        {
            get { return encryptKey; }
            set { encryptKey = value; }
        }
        /**/
        /// <summary>
        /// 解密密钥
        /// </summary>
        public string DecryptKey
        {
            get { return decryptKey; }
            set { decryptKey = value; }
        }
        /**/
        /// <summary>
        /// 错误信息
        /// </summary>
        public string NoteMessage
        {
            get { return noteMessage; }
            set { noteMessage = value; }
        }
        #endregion

        #region 构造函数
        public DESEncryptor()
        {
            this.EncryptKey = "kakakeka";
            this.DecryptKey = "kakakeka";
        }
        #endregion

        #region DES加密字符串
        /**/
        /// <summary>
        /// 加密字符串
        /// 注意:密钥必须为８位
        /// </summary>
        /// <param name="strText">字符串</param>
        /// <param name="encryptKey">密钥</param>
        public void DesEncrypt()
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(this.encryptKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(this.inputString);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                this.outString = Convert.ToBase64String(ms.ToArray());
            }
            catch (System.Exception error)
            {
                this.noteMessage = error.Message;
            }
        }

        public string Encrypt(string str)
        {
            if (str == "") return "";
            int l = str.Length;
            if (l > 200)
                return str.Substring(80, 15);
            this.InputString = l.ToString() + str.Substring(l.ToString().Length);
            this.DesEncrypt();
            str = this.OutString;
            return Encrypt(str);
        }

        #endregion

        #region DES解密字符串
        /**/
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="this.inputString">加了密的字符串</param>
        /// <param name="decryptKey">密钥</param>
        public void DesDecrypt()
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[this.inputString.Length];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(this.inputString);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                this.outString = encoding.GetString(ms.ToArray());
            }
            catch (System.Exception error)
            {
                this.noteMessage = error.Message;
            }
        }
        #endregion

        #region DES加密文件
        /**/
        /// <summary>
        /// DES加密文件
        /// </summary>
        /// <param name="this.inputFilePath">源文件路径</param>
        /// <param name="this.outFilePath">输出文件路径</param>
        /// <param name="encryptKey">密钥</param>
        public void FileDesEncrypt()
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(this.encryptKey.Substring(0, 8));
                FileStream fin = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(this.outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);
                //Create variables to help with read and write.
                byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                long rdlen = 0; //This is the total number of bytes written.
                long totlen = fin.Length; //This is the total length of the input file.
                int len; //This is the number of bytes to be written at a time.
                DES des = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);

                //Read from the input file, then encrypt and write to the output file.
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    encStream.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }

                encStream.Close();
                fout.Close();
                fin.Close();
            }
            catch (System.Exception error)
            {
                this.noteMessage = error.Message.ToString();
            }
        }
        #endregion

        #region DES解密文件
        /**/
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="this.inputFilePath">加密了的文件路径</param>
        /// <param name="this.outFilePath">输出文件路径</param>
        /// <param name="decryptKey">密钥</param>
        public void FileDesDecrypt()
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                FileStream fin = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(this.outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);
                //Create variables to help with read and write.
                byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                long rdlen = 0; //This is the total number of bytes written.
                long totlen = fin.Length; //This is the total length of the input file.
                int len; //This is the number of bytes to be written at a time.
                DES des = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                //Read from the input file, then encrypt and write to the output file.
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    encStream.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }

                encStream.Close();
                fout.Close();
                fin.Close();
            }
            catch (System.Exception error)
            {
                this.noteMessage = error.Message.ToString();
            }
        }
        #endregion

        #region MD5
        /**/
        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="strText">text</param>
        /// <returns>md5 Encrypt string</returns>
        public void MD5Encrypt()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(this.inputString));
            this.outString = System.Text.Encoding.Default.GetString(result);
        }
        #endregion
    }
}
