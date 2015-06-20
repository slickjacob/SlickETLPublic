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
            ViewBag.Title = "SlickETL";
            return View();
        }

        [Route("Connectors")]
        public ActionResult Connectors()
        {
            ViewBag.Title = "Connectors - SlickETL";
            return View();
        }

        [Route("FAQ")]
        public ActionResult FAQ()
        {
            ViewBag.Title = "FAQ - SlickETL";
            return View();
        }
    }
}
