using System.Net.Mail;
using System.Web.Configuration;
using System;

namespace DCT.Monitor.Server.Extensions
{
	public static class MailSender
	{
		public static void SendEmail(string toEmail, string emailSender, string senderName, string subject, string text)
		{
			var emailFrom = new MailAddress(emailSender, senderName);
			var emailTo = new MailAddress(toEmail);
			var message = new MailMessage(emailFrom, emailTo);
			message.Body = text;
			message.Priority = MailPriority.High;
			message.Sender = emailFrom;
			message.Subject = subject;
			SmtpClient client = new SmtpClient();
            client.Port = 465;
			client.Send(message);
		}

        public static void SendPasswordRecoveryEmail(string userName, string userEmail, string newPassword)
        {
            string body = String.Format(MailSenderConstants.PASSWORD_RECOVERY_EMAIL_TEMPLATE, userName, newPassword);
            SendEmail(userEmail, MailSenderConstants.EMAIL_SENDER_ADDRESS, MailSenderConstants.EMAIL_SENDER_NAME, MailSenderConstants.EMAIL_PASSWORD_RECOVERY_SUBJECT, body);
        }
	}
}