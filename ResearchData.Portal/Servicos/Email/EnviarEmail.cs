using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Servicos.Email
{
    public class EnviarEmail : IEmailSender
    {
        public EnviarEmail(IOptions<AutorizacaoEmail> acessorDeOpcoes)
        {
            Opcoes = acessorDeOpcoes.Value;
        }

        public AutorizacaoEmail Opcoes { get; }

        public Task SendEmailAsync(string email, string assunto, string mensagem)
        {
            if (String.IsNullOrWhiteSpace(Opcoes.SendGridKey))
                return null;
            return Executar(Opcoes.SendGridKey, assunto, mensagem, email);
        }

        public Task Executar(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("rdfacility@contato.com", "Research Data Facility"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
        
            
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
