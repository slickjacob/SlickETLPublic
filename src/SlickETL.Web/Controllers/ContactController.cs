using MailChimp;
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
using System.Threading.Tasks;
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
            ViewBag.Description = "Send us an email or subscribe to our updates";
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
        public void QuickSignup(string EmailAddress)
        {
            var tc = new TelemetryClient();
            tc.TrackEvent("QuickSignupPost");
            Trace.TraceInformation("QuickSignup:{0}", EmailAddress);
            try
            {
                SlickETL.Web.Utility.MailChimp.Signup(EmailAddress, "N/A", "N/A", "N/A", "QuickSignup");
            }
            catch (Exception ex)
            {
               
                tc.TrackEvent("QuickSignupError");
                Trace.TraceError("Quick Signup Error: {0}",ex.Message);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ContactModel contact)
        {
            try
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
                    SlickETL.Web.Utility.MailChimp.Signup(contact.Email, contact.FirstName, contact.LastName, contact.Company, contact.Hook);
                }

                //send message if included
                if (!string.IsNullOrWhiteSpace(contact.Message))
                {
                    Utility.Email.SendGmail(contact);
                }

                ViewBag.Title = "Contact - SlickETL";
                return View("ConfirmContact");
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error occurred on the contact page: {0}. {1}", ex.Message, ex.StackTrace);
                ViewBag.Title = "Contact Error - SlickETL";
                return View("ContactError");
            }
        }

    }
}