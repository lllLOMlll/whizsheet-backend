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
			// 🧨 FAIL FAST
			if (string.IsNullOrWhiteSpace(to))
				throw new InvalidOperationException("SMTP 'to' address is empty");

			if (string.IsNullOrWhiteSpace(_settings.From))
				throw new InvalidOperationException("SMTP 'From' is empty");

			if (string.IsNullOrWhiteSpace(_settings.Username))
				throw new InvalidOperationException("SMTP 'Username' is empty");

			Console.WriteLine("📧 SMTP SEND START");
			Console.WriteLine($"TO: {to}");
			Console.WriteLine($"FROM: {_settings.From}");
			Console.WriteLine($"HOST: {_settings.Host}:{_settings.Port}");

			using var message = new MailMessage
			{
				From = new MailAddress(_settings.From),
				Subject = subject,
				Body = htmlBody,
				IsBodyHtml = true
			};

			message.To.Add(to);

			using var client = new SmtpClient(_settings.Host, _settings.Port)
			{
				Credentials = new NetworkCredential(
					_settings.Username,
					_settings.Password
				),
				EnableSsl = true
			};

			await client.SendMailAsync(message);

			Console.WriteLine("✅ SMTP SEND SUCCESS");
		}
	}
}
