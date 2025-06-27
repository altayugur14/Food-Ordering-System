using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using YemekProjeTest2.Models;
using YemekProjeTest2.ViewModels;

namespace YemekProjeTest2.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly YemekSiparisSistemiContext _context;

        public RestaurantsController(YemekSiparisSistemiContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var restaurants = _context.Restaurants.Where(r => string.IsNullOrEmpty(searchString) || r.Name.Contains(searchString)).ToList();
            return View(restaurants);
        }

        public IActionResult Details(int id)
        {
            // Restoran verisini çekerken, ilişkili MenuItems ve Reviews verilerini de dahil ediyoruz.
            var restaurant = _context.Restaurants
                                     .Include(r => r.MenuItems)
                                     .Include(r => r.Reviews) // Yorumları da dahil et
                                     .FirstOrDefault(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return NotFound("Restoran bulunamadı.");
            }

            // Ortalama puanı ve yorum sayısını hesapla
            restaurant.Rating = restaurant.Reviews.Any() ? restaurant.Reviews.Average(rev => rev.Rating) : (double?)null;
            ViewBag.RatingCount = restaurant.Reviews.Count();

            return View(restaurant);
        }



        //Dashboard başlangıç
        [CustomAuthorize("A,P")]
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == userId);

            // Siparişleri restoranın menüsündeki ürünlerle ilişkilendirerek almak için
            var orders = _context.Orders
                .Where(o => o.Status == "Aktif" && _context.MenuItems
                    .Any(m => m.RestaurantId == restaurant.RestaurantId && _context.OrderDetails
                        .Any(od => od.OrderId == o.OrderId && od.MenuItemId == m.MenuItemId)))
                .ToList();

            // Kullanıcı adlarını getir
            var userNames = new Dictionary<int, string>();
            foreach (var order in orders)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == order.UserId);
                if (user != null)
                {
                    userNames[order.OrderId] = user.Username;
                }
            }

            if (restaurant == null)
            {
                return View("RegisterRestaurant", "Account");
            }

            var viewModel = new DashboardViewModel
            {
                Restaurant = restaurant,
                Orders = orders,
                UserNames = userNames // Kullanıcı adlarını ekledik
            };

            return View(viewModel);
        }


        public IActionResult CompletedOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == userId);

            var completedOrders = _context.Orders
                .Where(o => o.Status == "Tamamlandı" && _context.MenuItems
                    .Any(m => m.RestaurantId == restaurant.RestaurantId && _context.OrderDetails
                        .Any(od => od.OrderId == o.OrderId && od.MenuItemId == m.MenuItemId)))
                .ToList();

            
            return View(completedOrders);
        }


        //READ BAŞI
        [CustomAuthorize("A,P")]
        public IActionResult MenuItems()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == userId);
                var menuItems = _context.MenuItems.Where(m => m.RestaurantId == restaurant.RestaurantId).ToList();

                ViewBag.RestaurantId = restaurant.RestaurantId; // Restaurant ID'yi görünüme ilet

                return View(menuItems); // Sadece kullanıcının restoranının menü öğelerini görüntüle
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }

        }

        [CustomAuthorize("A,P")]
        public IActionResult Orders()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == userId);

                if (restaurant == null)
                {
                    // Eğer restoran bulunamazsa, hata mesajı göster
                    return View("Error");
                }

                var restaurantId = restaurant.RestaurantId;

                var orders = _context.Orders
                            .Join(_context.MenuItems,
                                orderDetail => orderDetail.OrderId,
                                menuItem => menuItem.RestaurantId,
                                (orderDetail, menuItem) => new { orderDetail, menuItem })
                            .Where(joined => joined.menuItem.RestaurantId == restaurantId)
                            .Select(joined => joined.orderDetail)
                            .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.MenuItem)
                            .ToList();

                ViewBag.Orders = orders;

                return View(); // Restoran sahibinin siparişlerini görüntüle
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }


        }

        [CustomAuthorize("A,P")]
        [HttpPost]
        public IActionResult CreateMenuItem(MenuItem menuItem)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == userId);
                menuItem.RestaurantId = restaurant.RestaurantId;

                if (ModelState.IsValid)
                {
                    _context.MenuItems.Add(menuItem);
                    _context.SaveChanges();
                    return RedirectToAction("MenuItems");
                }

                return View("MenuItems");
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }

        }

        [HttpPost]
        public IActionResult UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                // Veritabanından menü öğesini bulma
                var existingMenuItem = _context.MenuItems.FirstOrDefault(m => m.MenuItemId == menuItem.MenuItemId);

                if (existingMenuItem != null)
                {
                    // Gelen verilerle mevcut menü öğesini güncelleme
                    existingMenuItem.Name = menuItem.Name;
                    existingMenuItem.Description = menuItem.Description;
                    existingMenuItem.Price = menuItem.Price;

                    // Değişiklikleri kaydetme
                    _context.MenuItems.Update(existingMenuItem);
                    _context.SaveChanges();

                    return RedirectToAction("MenuItems");
                }

                // Hata durumunda aynı sayfaya geri dönme (veya başka bir hata işleme yöntemi kullanma)
                return View("MenuItems");
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }

        }

        [HttpPost]
        public IActionResult DeleteMenuItem(int MenuItemId)
        {
            try
            {
                // Burada, veritabanından belirtilen ID'ye sahip menü öğesini bulup siliyoruz

                var menuItem = _context.MenuItems.FirstOrDefault(m => m.MenuItemId == MenuItemId);

                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound("Menü öğesi bulunamadı.");
                }

                // Silme işlemi başarılı bir şekilde gerçekleştirildiyse,
                return RedirectToAction("MenuItems", "Restaurants"); // Yönlendirme işlemi
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun bir hata mesajı dönülür.
                return BadRequest("Menü öğesinin silinmesi sırasında bir hata oluştu. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            // Belirtilen siparişi veritabanından al
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound(); // Sipariş bulunamazsa hata döndür
            }

            // Sipariş durumunu "Tamamlandı" olarak güncelle
            order.Status = "Tamamlandı";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(); // Başarı durumunu döndür
        }

        public IActionResult Reviews(int id)
        {
            var restaurant = _context.Restaurants
                                     .Include(r => r.Reviews)
                                     .FirstOrDefault(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return NotFound("Restoran bulunamadı.");
            }

            var model = new RestaurantReviewsViewModel
            {
                RestaurantName = restaurant.Name,
                Reviews = restaurant.Reviews.OrderByDescending(r => r.ReviewId).ToList() // Burada yorumları ters sıralıyoruz
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult GetOrderDetails(int orderId)
        {
            try
            {
                // Fetching the order details
                var orderDetails = _context.OrderDetails
                                           .Include(od => od.MenuItem)
                                           .Where(od => od.OrderId == orderId)
                                           .ToList();

                // Preparing the response
                var response = new
                {
                    details = orderDetails.Select(od => new
                    {
                        menuItem = od.MenuItem.Name,
                        price = od.MenuItem.Price,
                        quantity = od.Quantity
                    }).ToList()
                };

                return Json(response);
            }
            catch (Exception e)
            {
                // Log the error if needed
                return Json(new { error = e.Message });
            }
        }

    }


}
