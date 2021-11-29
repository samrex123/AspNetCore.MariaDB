using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace AspNetCore.MariaDB.HelpClasses
{
    public class popmail
    {
        public static void SendEmail(string email, string query)
        {
            //Rad r = new Rad(Tabell, meddelande, toEmail, (int)DateTimeOffset.Now.ToUnixTimeSeconds());

            string mailAddress = "mintestmail321@gmail.com";
            string passwordMail = "1Kalaskula!";
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sam", mailAddress));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Update";
            message.Body = new TextPart("plain")
            {
                Text = $"{query}"
            };
            SmtpClient client = new SmtpClient();
            try
            {
                client.CheckCertificateRevocation = false;
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(mailAddress, passwordMail);
                client.Send(message);
                //Console.WriteLine("Email Sent!");
                //dh.SendSqlQuery(r.ToSQL());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
