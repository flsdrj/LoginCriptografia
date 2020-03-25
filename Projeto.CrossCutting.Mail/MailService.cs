using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Projeto.CrossCutting.Mail
{
    public class MailService
    {
        //atributo
        private readonly MailSettings mailSettings;

        //construtor pra injeção de dependência
        public MailService(MailSettings mailSettings)
        {
            this.mailSettings = mailSettings;
        }

        //método para enviar o email
        public void SendMail(string email, string subject, string body)
        {
            //montando a mensagem de email
            var mail = new MailMessage(mailSettings.EmailAddress, email);
            mail.Subject = subject;
            mail.Body = body;

            //enviando o email
            var client = new SmtpClient(mailSettings.Smtp, mailSettings.Port);
            client.EnableSsl = mailSettings.EnableSsl;
            client.Credentials = new NetworkCredential(mailSettings.EmailAddress, mailSettings.Password);
            client.Send(mail);

        }

    }
}
