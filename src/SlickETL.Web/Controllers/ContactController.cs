﻿using MailChimp;
using MailChimp.Helper;
using Microsoft.ApplicationInsights;
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
using System.Web.Mvc;

namespace SlickETL.Web.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        [HttpGet]
        [Route("Contact")]
        public ActionResult Index(string req = null)
        {
            //log the request
            if (req != null)
            {
                var tc = new TelemetryClient();
                tc.TrackEvent("Contact-" + req);
                Trace.TraceInformation("Contact page loaded with hook: {0}", req);
            }

            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            ViewBag.Title = "Contact - SlickETL";
            ViewBag.Hook = req;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ContactModel contact)
        {
            var szd = JsonConvert.SerializeObject(contact);
            Trace.TraceInformation("Contact model posted: {0}", szd);
            var tc = new TelemetryClient();
            tc.TrackEvent(string.Format("ContactPost-{0}", contact.Hook));

            //redirect if invalid or they skipped captcha
            string rcr = Request["g-recaptcha-response"];
            if (string.IsNullOrWhiteSpace(rcr) || !ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            //verify recaptcha
            var recaptchaWasSuccessful = Utility.ReCaptcha.VerifyUserResponse(rcr, Request.UserHostAddress);
            if (!recaptchaWasSuccessful)
            {
                return RedirectToAction("Index");
            }

            //subscribe if requested
            if (contact.Subscribe)
            {
                var email = new EmailParameter() { Email = contact.Email };
                var mc = new MailChimpSignUpModel();
                mc.FirstName = contact.FirstName;
                mc.LastName = contact.LastName;
                mc.Company = contact.Company;
                mc.Hook = contact.Hook;
                var mcm = new MailChimpManager("e60fe47c1d4845c1292f6f0352946403-us11");
                EmailParameter results = mcm.Subscribe("c048a56a60", email, mc);
            }

            //send message if included
            if (!string.IsNullOrWhiteSpace(contact.Message))
            {
                SendGridEmail(contact);
            }

            ViewBag.Title = "Contact - SlickETL";
            return View("ConfirmContact");

        }

        private void SendGridEmail(ContactModel contact)
        {
            var szd = JsonConvert.SerializeObject(contact);
            Trace.TraceInformation("Sending contact email for {0}", szd);
            var pwd = ConfigurationManager.AppSettings["SendGridPassword"];
            Trace.TraceInformation("SendGrid password acquired: {0}", string.IsNullOrEmpty(pwd));
            string contactFullName = string.Format("{0} {1}",contact.FirstName, contact.LastName);
            var myMessage = new SendGridMessage();
            myMessage.From = new MailAddress("contact@slicketl.com");
            List<String> recipients = new List<String>{@"SlickETL Support <support@slicketl.com>"};
            myMessage.AddTo(recipients);
            myMessage.Subject = "Website Contact";
            myMessage.ReplyTo = new MailAddress[]{new MailAddress(contact.Email,contactFullName)};
            var htmlFormat = "<div><strong>From:</strong> {0}</div><div><strong>Email</strong> {1}</div><div><strong>Message</strong> {2}</div><hr/><div>{3}</div>";
            myMessage.Html = string.Format(htmlFormat, contactFullName, contact.Email, contact.Message, szd);
            var credentials = new NetworkCredential("SlickETL", pwd); //azure_9832f9098b12880e13fd581db647e04d, smtp.sendgrid.net
            var transportWeb = new SendGrid.Web(credentials);
            Trace.TraceInformation("Configuration complete; about to deliver SendGrid email.");
            // You can also use the **DeliverAsync** method, which returns an awaitable task.
            transportWeb.DeliverAsync(myMessage);
        }


    }
}