using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace TMCoreV3.Services
{
    public class MailService : IMailService, ISmsService
    {
        public EmailSettings _emailSettings { get; }

        public MailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string from_email, string subject, string message)
        {

            Execute(from_email, subject, message).Wait();
            return Task.FromResult(0);
        }

        //public Task SendEmailAsync(string from_email, string subject, string message)
        //{
        //    this.SendEmailAsync(_config["MailSettings:ToAddress"], from_email, subject, message);
        //    // Plug in your email service here to send an email.
        //    return Task.FromResult(0);
        //}

        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string ToEmail = _emailSettings.ToEmail;
                MailAddressCollection TO_addressList = new MailAddressCollection();
                //3.Prepare the Destination email Addresses list
                foreach (var curr_address in ToEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MailAddress mytoAddress = new MailAddress(curr_address);
                    TO_addressList.Add(mytoAddress);
                }

                string CcEmail = _emailSettings.CcEmail;
                MailAddressCollection Cc_addressList = new MailAddressCollection();
                //3.Prepare the Destination email Addresses list
                foreach (var curr_address in CcEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MailAddress myCcAddress = new MailAddress(curr_address);
                    Cc_addressList.Add(myCcAddress);
                }

                using (MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.FromEmail),
                    Subject = "ATapplianceServiceInc.com - " + subject,
                    Body = message,
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                })
                {
                    mail.To.Add(TO_addressList.ToString());
                    mail.CC.Add(Cc_addressList.ToString());

                    using (SmtpClient smtp = new SmtpClient(_emailSettings.SecondayDomain, _emailSettings.SecondaryPort))
                    {
                        smtp.Host = _emailSettings.PrimaryDomain;
                        smtp.Port = _emailSettings.PrimaryPort;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                //do something here
            }
        }


        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }


    }
}
