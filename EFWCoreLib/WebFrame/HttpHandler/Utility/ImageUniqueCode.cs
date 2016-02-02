using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using EFWCoreLib.WebFrame.HttpHandler.Controller;

namespace EFWCoreLib.WebFrame.HttpHandler.Utility
{
    /// <summary>
    /// 登录验证码控制器
    /// </summary>
    public class ImageUniqueCode : WebHttpController
    {
        public static readonly string ImageUniqueCode_Session = "ImageUniqueCode";

        public void CreateCode()
        {
            context.Response.ContentType = "image/gif";
            //建立Bitmap对象，绘图
            Bitmap basemap = new Bitmap(160, 60);
            Graphics graph = Graphics.FromImage(basemap);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, 160, 60);
            Font font = new Font(FontFamily.GenericSerif, 48, FontStyle.Bold, GraphicsUnit.Pixel);
            Random r = new Random();
            string letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";
            string letter;
            StringBuilder s = new StringBuilder();

            //添加随机字符
            for (int x = 0; x < 4; x++)
            {
                letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
                s.Append(letter);
                graph.DrawString(letter, font, new SolidBrush(Color.Black), x * 38, r.Next(0, 15));
            }

            //混淆背景
            Pen linePen = new Pen(new SolidBrush(Color.Black), 2);
            for (int x = 0; x < 6; x++)
                graph.DrawLine(linePen, new Point(r.Next(0, 159), r.Next(0, 59)), new Point(r.Next(0, 159), r.Next(0, 59)));

            //将图片保存到输出流中      
            basemap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
            context.Session[ImageUniqueCode_Session] = s.ToString();
        }

        public void CreateNumCode()
        {
            context.Response.ContentType = "image/gif";
            //建立Bitmap对象，绘图
            Bitmap basemap = new Bitmap(160, 60);
            Graphics graph = Graphics.FromImage(basemap);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, 160, 60);
            Font font = new Font(FontFamily.GenericSerif, 48, FontStyle.Bold, GraphicsUnit.Pixel);
            Random r = new Random();
            string letters = "0123456789";
            string letter;
            StringBuilder s = new StringBuilder();

            //添加随机字符
            for (int x = 0; x < 4; x++)
            {
                letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
                s.Append(letter);
                graph.DrawString(letter, font, new SolidBrush(Color.Black), x * 38, r.Next(0, 15));
            }

            //混淆背景
            Pen linePen = new Pen(new SolidBrush(Color.Black), 2);
            for (int x = 0; x < 6; x++)
                graph.DrawLine(linePen, new Point(r.Next(0, 159), r.Next(0, 59)), new Point(r.Next(0, 159), r.Next(0, 59)));

            //将图片保存到输出流中      
            basemap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
            context.Session[ImageUniqueCode_Session] = s.ToString();
        }

        public void CheckCode()
        {
            string code = ParamsData["UniqueCode"];
            if (code.ToUpper() == sessionData[ImageUniqueCode_Session].ToString().ToUpper())
            {
                context.Response.Write("0");
            }
            else
            {
                context.Response.Write("1");
            }
        }
    }
}
