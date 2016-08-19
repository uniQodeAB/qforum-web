using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QForum.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //todo: fetch from Azure Storage
            var data = new
            {
                RestaurantName = "Phil´s Burger",
                RestaurantMenuUrl = "http://www.philsburger.se/#pmeny",
                NextDate = DateTime.UtcNow.AddDays(7)
            };

            ViewData["RestaurantName"] = data.RestaurantName;
            ViewData["RestaurantMenuUrl"] = data.RestaurantMenuUrl;
            ViewData["NextDateReadable"] = data.NextDate.ToString("dddden dde MMMM", new CultureInfo("sv-SE"));

            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
