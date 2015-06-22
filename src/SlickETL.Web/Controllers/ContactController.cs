using SlickETL.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewBag.Title = "Contact - SlickETL";
            return View("ConfirmContact");
        }

    }
}