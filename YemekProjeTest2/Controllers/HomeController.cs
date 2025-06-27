using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YemekProjeTest2.Models;

namespace YemekProjeTest2.Controllers
{
    public class HomeController : Controller
    {
        private readonly YemekSiparisSistemiContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, YemekSiparisSistemiContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // En yüksek puanlı 3 restoranı çekiyoruz.
            var topRestaurants = _context.Restaurants
                                         .Where(r => r.Rating != null) // NULL olmayan puanları alın
                                         .OrderByDescending(r => r.Rating)
                                         .Take(3)
                                         .ToList();

            return View(topRestaurants);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}