using System;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace Budget.Common
{
    public class Email
    {
        public static void EmailDelivery(string emailAddress, string subject, string body, string priority)
        {
            try
            {
                SmtpClient mailServer = new SmtpClient()
                {
                    Host = ConfigurationManager.AppSettings["SMTPServer"].ToString(),
                    Port = (Convert.ToInt32(ConfigurationManager.AppSettings["SMTPServerPort"])),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential()
                    {
                        UserName = ConfigurationManager.AppSettings["SMTPServerUsername"],
                        Password = ConfigurationManager.AppSettings["SMTPServerPassword"]
                    }
                };

                MailMessage email = new MailMessage(ConfigurationManager.AppSettings["EmailFrom"], emailAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                switch (priority)
                {
                    case "H":
                        email.Priority = MailPriority.High;
                        break;
                    case "M":
                        email.Priority = MailPriority.Normal;
                        break;
                    case "L":
                        email.Priority = MailPriority.Low;
                        break;
                }

                mailServer.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
