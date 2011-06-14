using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoData;

namespace GeoDataChangeNotifier.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeleteParksRssItems()
        {
            var rssPublisher = new ParksRssFeed();
            rssPublisher.DeleteAllItems();
            return View();
        }

    }
}
