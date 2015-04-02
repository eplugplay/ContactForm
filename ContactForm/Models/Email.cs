using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;
using System.Collections;
using System.IO;

namespace ContactForm.Classes
{
    public static class Email
    {
        //send email with attachment
        public static bool SendEmail(string emailToSend, string subjectToSend, string MsgBody, Stream[] files, string[] fileNames, bool isCustomer)
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

            if (!isCustomer)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    Attachment attachment = new Attachment(files[i], fileNames[i]);
                    attachment.Name = fileNames[i];
                    mail.Attachments.Add(attachment);
                }
            }
            try
            {
                SmtpServer.Send(mail);
                if (!isCustomer)
                {
                    mail.Attachments.Clear();
                    mail.Attachments.Dispose();
                }
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