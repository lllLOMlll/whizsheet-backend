using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Whizsheet.Api.Email
{
	public class SmtpEmailSender : IEmailSender
	{
		private readonly SmtpSettings _settings;

		public SmtpEmailSender(IOptions<SmtpSettings> settings)
		{
			_settings = settings.Value;
		}

		public async Task SendAsync(string to, string subject, string htmlBody)
		{
			// 🔍 Lecture via settings (PAS IConfiguration)
			var host = _settings.Host;
			var port = _settings.Port;
			var username = _settings.Username;
			var password = _settings.Password;
			var from = _settings.From;

			// 🧨 Fail fast
			if (string.IsNullOrWhiteSpace(to))
				throw new InvalidOperationException("SMTP 'to' address is empty");

			if (string.IsNullOrWhiteSpace(username))
				throw new InvalidOperationException("SMTP 'Username' is empty");

			if (string.IsNullOrWhiteSpace(password))
				throw new InvalidOperationException("SMTP 'Password' is empty");

			if (string.IsNullOrWhiteSpace(from))
				throw new InvalidOperationException("SMTP 'From' is empty");

			using var message = new MailMessage
			{
				From = new MailAddress(from, "Whizsheet"),
				Subject = subject,
				Body = htmlBody,
				IsBodyHtml = true
			};

			message.To.Add(to);

			using var client = new SmtpClient(host, port)
			{
				Credentials = new NetworkCredential(username, password),
				EnableSsl = true
			};

			await client.SendMailAsync(message);
		}
	}
}
