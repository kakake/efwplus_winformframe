using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreFrame.Business;
using LumiSoft.Net.Mail;
using LumiSoft.Net.SMTP.Client;
using LumiSoft.Net.MIME;
using WebUIFrame.Common;
using System.Net.Mail;
using System.Net;


public class FastFeedBack : AbstractController
{
    //最终状态
    public enum FeedbackState
    {
        UniqueCodeError = 1,
        Succeed = 5,
        Error = 10,
        Null = 20
    }

    public static string host = "smtp.sohu.com";
    public static string form = "kakake914@sohu.com";
    public static string to = "343588387@qq.com";
    public static string subject = "MarchApp留言板";

    public void SaveFeed()
    {
        context.Response.ContentType = "text/plain";
        context.Response.CacheControl = "no-cache"; //清空缓存

        string code = ParamsData["SecurityCode"];
        string _message = ParamsData["Message"];

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(_message))
        {
            context.Response.Write(FeedbackState.Null.ToString());
            context.Response.End();
            return;
        }

        try
        {
            if (code.ToUpper() == sessionData[ControllerHelper.ImageUniqueCode].ToString().ToUpper())
            {
                //执行成功,完成其他任务
                //Mail_Message mailmessage = CreateMessage(message);

                //SMTP_Client.QuickSendSmartHost(host, 25, false, mailmessage);
                MailMessage message = new MailMessage();
                message.From = new MailAddress("kakake914@sina.cn");
                message.To.Add("343588387@qq.com");

                message.Subject = subject;
                message.Body =_message;

                SmtpClient smtp = new SmtpClient("61.129.51.85");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = new NetworkCredential("kakake914@sina.cn", "kakake914");
                smtp.Send(message);
                context.Response.Write(FeedbackState.Succeed.ToString());

            }
            else
            {
                context.Response.Write(FeedbackState.UniqueCodeError.ToString());

            }
        }
        catch (Exception ex)
        {
            context.Response.Write(FeedbackState.Error.ToString());
            CoreFrame.Core.ZhyContainer.CreateException().HandleException(ex, "HISPolicy");
        }
    }

    private Mail_Message CreateMessage(string message)
    {
        Mail_Message m = new Mail_Message();
        m.MimeVersion = "1.0";
        m.Date = DateTime.Now;
        m.MessageID = MIME_Utils.CreateMessageID();
        m.From = Mail_t_MailboxList.Parse(form);
        m.To = Mail_t_AddressList.Parse(to);
        m.Subject = subject;

       
            //--- multipart/alternative -----------------------------------------------------------------------------------------
            MIME_h_ContentType contentType_multipartAlternative = new MIME_h_ContentType(MIME_MediaTypes.Multipart.alternative);
            contentType_multipartAlternative.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
            MIME_b_MultipartAlternative multipartAlternative = new MIME_b_MultipartAlternative(contentType_multipartAlternative);
            m.Body = multipartAlternative;

            //--- text/plain ----------------------------------------------------------------------------------------------------
            MIME_Entity entity_text_plain = new MIME_Entity();
            MIME_b_Text text_plain = new MIME_b_Text(MIME_MediaTypes.Text.plain);
            entity_text_plain.Body = text_plain;
            text_plain.SetText(MIME_TransferEncodings.QuotedPrintable, Encoding.UTF8, message);
            multipartAlternative.BodyParts.Add(entity_text_plain);

            //--- text/html ------------------------------------------------------------------------------------------------------
            MIME_Entity entity_text_html = new MIME_Entity();
            MIME_b_Text text_html = new MIME_b_Text(MIME_MediaTypes.Text.html);
            entity_text_html.Body = text_html;
            text_html.SetText(MIME_TransferEncodings.QuotedPrintable, Encoding.UTF8, message);
            multipartAlternative.BodyParts.Add(entity_text_html);

        return m;
    }
}
