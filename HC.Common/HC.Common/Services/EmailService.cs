using HC.Common.Model.OrganizationSMTP;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HC.Common.Services
{

    public class EmailService : IEmailService
    {
        private readonly EmailConfig ec;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            this.ec = emailConfig.Value;

        }

        /// <summary>
        /// email send method
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(String email, String subject, String message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName)
        {
            try
            {
                var emailMessage = new MimeMessage();
                this.ec.FromAddress = organizationSMTPModel.SMTPUserName; //"smartDataHC@gmail.com";
                this.ec.FromName = organizationName.ToString();
                this.ec.MailServerAddress = organizationSMTPModel.ServerName; //"smtp.gmail.com";
                this.ec.UserPassword = organizationSMTPModel.SMTPPassword; //"hccare123*";
                this.ec.MailServerPort = organizationSMTPModel.Port; //"587";
                this.ec.UserId = organizationSMTPModel.SMTPUserName; //"smartDataHC@gmail.com";
                emailMessage.From.Clear();
                emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));

                emailMessage.To.Clear();
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = ec.LocalDomain;

                    await client.ConnectAsync(ec.MailServerAddress, Convert.ToInt32(ec.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string> SendEmails(String email, String subject, String message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName)
        {
            string error = "";
            try
            {
                var emailMessage = new MimeMessage();
                this.ec.FromAddress = organizationSMTPModel.SMTPUserName; //"smartDataHC@gmail.com";
                this.ec.FromName = organizationName.ToString();
                this.ec.MailServerAddress = organizationSMTPModel.ServerName; //"smtp.gmail.com";
                this.ec.UserPassword = organizationSMTPModel.SMTPPassword; //"hccare123*";
                this.ec.MailServerPort = organizationSMTPModel.Port; //"587";
                this.ec.UserId = organizationSMTPModel.SMTPUserName; //"smartDataHC@gmail.com";
                emailMessage.From.Clear();
                emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));

                emailMessage.To.Clear();
                emailMessage.To.Add(new MailboxAddress("", email));
                //emailMessage.To.Add(new MailboxAddress("", "jack123@yopmail.com"));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = ec.LocalDomain;

                    await client.ConnectAsync(ec.MailServerAddress, Convert.ToInt32(ec.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                    return "";
                }
            }
            catch (Exception ex)
            {
                error = "Message : " + ex.Message + ", Inner Exception : " + ex.InnerException;
                Console.WriteLine(ex.Message);
            }
            return error;
        }
        public bool SendEmail(String email, String subject, String bodyHtml, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName, string toEmail)
        {
            try
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new System.Net.Mail.MailAddress(organizationSMTPModel.SMTPUserName);
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = bodyHtml;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(organizationSMTPModel.ServerName, Convert.ToInt32(organizationSMTPModel.Port)))
                    {
                        smtp.Credentials = new NetworkCredential(organizationSMTPModel.SMTPUserName, organizationSMTPModel.SMTPPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

            //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            //message.From = new System.Net.Mail.MailAddress(organizationSMTPModel.SMTPUserName);
            //message.To.Add(new System.Net.Mail.MailAddress(toEmail));
            //message.Subject = subject;
            //message.IsBodyHtml = true; //to make message body as html  
            //message.Body = bodyHtml;
            //smtp.Port = 25;// Convert.ToInt32(organizationSMTPModel.Port);
            //smtp.Host =  organizationSMTPModel.ServerName;  
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential(organizationSMTPModel.SMTPUserName, organizationSMTPModel.SMTPPassword);
            //smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //smtp.Send(message);
        }

    }
}
