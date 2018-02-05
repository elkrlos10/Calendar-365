using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.ComponentModel;


namespace LogicaNegocio.Mail
{
    public class SendMail
    {
        public static object Credentials { get; private set; }
        public static bool UseDefaultCredentials { get; private set; }

        public static void SendMailMessage(string subject, string body, string to)
        {

            var fromAddress = "programacionambientessena@gmail.com";
            const string fromPassword = "programacionsena";
            MailAddress fromAddress1 = new MailAddress(fromAddress);
            MailAddress toAddress = new MailAddress(to);
            var smtp = new SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 100000;
            }
            using (var message = new MailMessage(fromAddress1, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
            {
                smtp.Send(message);
            }
        }

        public async static Task SendMailMessage1(string subject, string body, string to)
        {
            try
            {
                var fromAddress = "programacionambientessena@gmail.com";
                const string fromPassword = "programacionsena";
                MailAddress fromAddress1 = new MailAddress(fromAddress);
                MailAddress toAddress = new MailAddress(to);
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 200000;
                }
                if (smtp.Port == 25)
                {
                    smtp.Port = 587;
                }
                else
                {
                    smtp.Port = 25;
                }
                using (var message = new MailMessage(fromAddress1, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception exc)
            {

                throw;
            }

        }
    }
}
