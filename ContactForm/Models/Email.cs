using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;
using System.Collections;

namespace ContactForm.Classes
{
    public static class Email
    {
        //send email with attachment
        public static bool SendEmail(string emailToSend, string subjectToSend, string MsgBody)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("steveyi32@gmail.com");
            mail.Subject = subjectToSend;
            mail.To.Add(emailToSend);
            mail.Bcc.Add("steveyi32@gmail.com");
            mail.Body = MsgBody;
            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;
            System.Net.NetworkCredential aCredential = new System.Net.NetworkCredential("steveyi32@gmail.com", "");
            SmtpServer.Credentials = aCredential;
            try
            {
                SmtpServer.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                //attachment.Dispose();
            }

        }
    }
}