using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlickETL.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "SlickETL - For people who want to work smarter, not harder";
            return View();
        }

        [Route("Connectors")]
        public ActionResult Connectors()
        {
            ViewBag.Title = "Connectors - SlickETL";
            ViewBag.Description = "Info about some available connectors and custom integrations with SlickETL";
            return View();
        }

        [Route("FAQ")]
        public ActionResult FAQ()
        {
            ViewBag.Title = "FAQ - SlickETL";
            ViewBag.Description = "Answers to all of your questions about the cool features of SlickETL";
            return View();
        }

        [Route("Features")]
        public ActionResult Features()
        {
            ViewBag.Title = "Features - SlickETL";
            ViewBag.Description = "Overview of the cool features in SlickETL";
            return View();
        }
    }
}
