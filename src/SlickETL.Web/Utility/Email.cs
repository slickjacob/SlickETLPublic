using Newtonsoft.Json;
using SendGrid;
using SlickETL.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SlickETL.Web.Utility
{
    public class Email
    {
        public static void SendGmail(ContactModel contact)
        {
            var szd = JsonConvert.SerializeObject(contact);
            Trace.TraceInformation("Sending contact email for {0}", szd);
            var pwd = ConfigurationManager.AppSettings["EmailPassword"];
            //Trace.TraceInformation("SendGrid password acquired: {0}", string.IsNullOrEmpty(pwd));
            string contactFullName = string.Format("{0} {1}", contact.FirstName, contact.LastName);
            var myMessage = new MailMessage();
            myMessage.From = new MailAddress("jacob@slicketl.com");
            List<String> recipients = new List<String> { @"SlickETL Support <support@slicketl.com>" };
            myMessage.To.Add(new MailAddress("jacob@slicketl.com", "SlickETL Support"));
            myMessage.Subject = "Website Contact";
            myMessage.ReplyToList.Add(new MailAddress(contact.Email, contactFullName));
            var htmlFormat = "<div><strong>From:</strong> {0}</div><div><strong>Email</strong> {1}</div><div><strong>Message</strong> {2}</div><hr/><div>{3}</div>";
            myMessage.Body = string.Format(htmlFormat, contactFullName, contact.Email, contact.Message, szd);
            myMessage.IsBodyHtml = true;
            var credentials = new NetworkCredential("jacob@slicketl.com", pwd);
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credentials;
            smtp.Send(myMessage);
        }

        public static async void SendGridEmail(ContactModel contact)
        {
            var szd = JsonConvert.SerializeObject(contact);
            Trace.TraceInformation("Sending contact email for {0}", szd);
            var pwd = ConfigurationManager.AppSettings["SendGridPassword"];
            Trace.TraceInformation("SendGrid password acquired: {0}", string.IsNullOrEmpty(pwd));
            string contactFullName = string.Format("{0} {1}", contact.FirstName, contact.LastName);
            var myMessage = new SendGridMessage();
            myMessage.From = new MailAddress("contact@slicketl.com");
            List<String> recipients = new List<String> { @"SlickETL Support <support@slicketl.com>" };
            myMessage.AddTo(recipients);
            myMessage.Subject = "Website Contact";
            myMessage.ReplyTo = new MailAddress[] { new MailAddress(contact.Email, contactFullName) };
            var htmlFormat = "<div><strong>From:</strong> {0}</div><div><strong>Email</strong> {1}</div><div><strong>Message</strong> {2}</div><hr/><div>{3}</div>";
            myMessage.Html = string.Format(htmlFormat, contactFullName, contact.Email, contact.Message, szd);
            var credentials = new NetworkCredential("azure_9832f9098b12880e13fd581db647e04d@azure.com", pwd); //azure_9832f9098b12880e13fd581db647e04d, smtp.sendgrid.net
            var transportWeb = new SendGrid.Web(credentials);
            Trace.TraceInformation("Configuration complete; about to deliver SendGrid email.");
            await transportWeb.DeliverAsync(myMessage);

        }
    }
}