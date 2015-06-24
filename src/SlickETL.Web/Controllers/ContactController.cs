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
                    var mailChimpListID = "c048a56a60";
                    var email = new EmailParameter() { Email = contact.Email };
                    var mc = new MailChimpSignUpModel();
                    mc.FirstName = contact.FirstName;
                    mc.LastName = contact.LastName;
                    mc.Company = contact.Company;
                    mc.Hook = contact.Hook;
                    var mcm = new MailChimpManager("e60fe47c1d4845c1292f6f0352946403-us11");
                    EmailParameter results = mcm.Subscribe(mailChimpListID, email, mc,updateExisting:true);
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