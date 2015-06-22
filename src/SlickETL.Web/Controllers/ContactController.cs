using MailChimp;
using MailChimp.Helper;
using SlickETL.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            string rcr = Request["g-recaptcha-response"];
            if (string.IsNullOrWhiteSpace(rcr) || !ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

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


            ViewBag.Title = "Contact - SlickETL";
            return View("ConfirmContact");
            
        }


    }
}