using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace HelpDesk.API.Models
{
    public class EmailUtility
    {
        public static void sendEmail(string mailfrom, string mailTo, string strHTML, string Subject, string mailBCC)
        {
            string msg1 = string.Empty;
            string Information = string.Empty;
            try
            {
                MailMessage mMailMessage = new MailMessage();
                mMailMessage.From = new MailAddress(mailfrom);
                mMailMessage.To.Add(new MailAddress(mailTo));

                if (!string.IsNullOrWhiteSpace(mailBCC))
                    mMailMessage.Bcc.Add(mailBCC);

                mMailMessage.Subject = Subject;
                mMailMessage.Body = strHTML;
                mMailMessage.IsBodyHtml = true;
                mMailMessage.Priority = MailPriority.Normal;

                string SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
                int portNo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortNo"]);

                SmtpClient mSmtpClient = new SmtpClient(SmtpServer, portNo);
                mSmtpClient.UseDefaultCredentials = false;
                mSmtpClient.EnableSsl = false;

                mSmtpClient.Timeout = 10000;
                mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                string mailPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["mailCredentialPassword"]);
                mSmtpClient.Credentials = new System.Net.NetworkCredential(mailfrom, mailPassword);

                Information = "SMTP Server : " + SmtpServer + ", Port : " + portNo + ", Mail From : " + mailfrom + ", Mail Password :" + mailPassword + ".";

                mSmtpClient.Send(mMailMessage);
                //msg1 = "Successful";


                //var message = new MailMessage();

                //message.To.Add(new MailAddress("hussainibaigm@gmail.com"));
                //message.From = new MailAddress("info@creative-sols.com");

                //message.Subject = "Subject";
                //message.Body = "Body";

                //var smtpClient = new SmtpClient();



                //smtpClient.Host = "smtpout.asia.secureserver.net";
                //smtpClient.Port = 80;
                //smtpClient.EnableSsl = false;
                //smtpClient.UseDefaultCredentials = false;

                //smtpClient.Credentials = new System.Net.NetworkCredential("helpdesk@execution-planner.com", "SMTP@helpdesk");

                //smtpClient.Timeout = 100000;

                //smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //ErrorLogsDTO obj = new ErrorLogsDTO();
                //obj.Message = ex.InnerException.Message + ex.Message;
                //obj.Source = "UtilityMail -> Email";
                //obj.JsonData = Information;
                //obj.Information = strHTML;
                //Utility.ErrorLogsDb.ErrorLogs(obj);

                //BusinessModelExceptionUtility.LogException(ex.Message, "UtilityMail -> Email");
            }
        }
    }
}