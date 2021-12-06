using AspNetCore.MariaDB.Models;
using AspNetCore.MariaDB.Persistence;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Text;

namespace AspNetCore.MariaDB.HelpClasses
{
    public class popmail
    {
        /// <summary>
        /// Krypterar och struktuerar upp för mailutskick
        /// </summary>
        /// <param name="email"></param>
        /// <param name="query"></param>
        /// <param name="subject"></param>
        public static void SendEmail(string email, string query, string? subject)
        {

            if (subject == null)
            {
                subject = "Posting";
            }
            var sw = new StringBuilder();
            sw.Append(DateTime.Now.ToString() + "/()/");
            sw.Append(subject);

            string encrypt = Encryption.Encrypt(query, Globals.secretKey);

            var mailAddress = Globals.mailAddress;
            var password = Globals.password;
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jonatan", mailAddress));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = sw.ToString();
            message.Body = new TextPart("plain")
            {
                Text = $"{encrypt}XYXY/(/(XYXY7"
            };
            SmtpClient client = new SmtpClient();
            try
            {
                client.CheckCertificateRevocation = false;
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(mailAddress, password);
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
