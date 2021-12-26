using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MacBookCrawler.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            TempData["Admin"] = "1";
            return RedirectToAction("Index","Home");
        }
    }
}