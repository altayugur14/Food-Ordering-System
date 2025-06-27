using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using YemekProjeTest2.Models;
using Microsoft.EntityFrameworkCore;
using YemekProjeTest2.ViewModels;

public class AccountController : Controller
{
    private readonly YemekSiparisSistemiContext _context;

    public AccountController(YemekSiparisSistemiContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult RegisterCustomer()
    {
        return View();
    }

    public IActionResult RegisterOwner()
    {
        return View();
    }

    [HttpGet]
    [CustomAuthorize("P")]
    public IActionResult RegisterRestaurant()
    {
        return View();
    }



    [HttpPost]
    public IActionResult RegisterCustomer(string username, string password, string confirmPassword, string email, string address, string phoneNumber)
    {
        try
        {

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Şifreler eşleşmiyor. Lütfen tekrar deneyin.";
                return View();
            }
            // Kullanıcı adı uzunluğu kontrolü
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                ViewBag.ErrorMessage = "Kullanıcı adı en az 3 karakter uzunluğunda olmalıdır.";
                return View();
            }

            // Şifre karmaşıklığı kontrolü (daha düzgün bir kısıt getirilebilir geliştirme aşaması bitince)
            if (password.Length < 6 || !password.Any(char.IsDigit))
            {
                ViewBag.ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalı ve en az bir rakam içermelidir.";
                return View();
            }

            // Kullanıcı adının zaten var olmadığından emin ol
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu kullanıcı adı zaten alınmış. Lütfen farklı bir kullanıcı adı deneyin.";
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = password,
                Email = email,
                Role = "C", // Müşteri olarak otomatik atanıyor
                Address = address,
                PhoneNumber = phoneNumber
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Kullanıcı başarıyla kayıt olduktan sonra anasayfaya yönlendir
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            // Log.Error(ex, "Kullanıcı kaydı sırasında bir hata oluştu.");
            ViewBag.ErrorMessage = "Üzgünüz, kayıt işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
            return View();
        }
    }

    [HttpPost]
    public IActionResult RegisterOwner(string username, string password, string confirmPassword, string email, string address, string phoneNumber)
    {
        try
        {
            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Şifreler eşleşmiyor. Lütfen tekrar deneyin.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                ViewBag.ErrorMessage = "Kullanıcı adı en az 3 karakter uzunluğunda olmalıdır.";
                return View();
            }

            if (password.Length < 6 || !password.Any(char.IsDigit))
            {
                ViewBag.ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalı ve en az bir rakam içermelidir.";
                return View();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu kullanıcı adı zaten alınmış. Lütfen farklı bir kullanıcı adı deneyin.";
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = password,
                Email = email,
                Role = "P", // Restoran sahibi olarak atanır.
                Address = address,
                PhoneNumber = phoneNumber
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Üzgünüz, kayıt işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
            return View();
        }
    }

    [HttpPost]
    [CustomAuthorize("P")]
    public IActionResult RegisterRestaurant(string name, string address, string phoneNumber)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                ViewBag.ErrorMessage = "Geçersiz Kullanıcı ID.";
                return View();
            }

            // Yeni restoran nesnesi oluştur.
            Restaurant restaurant = new Restaurant
            {
                UserId = userId,
                Name = name,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Veritabanına kaydet.
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Restaurants"); // Başarılı olursa restoran yönetimi arayüzüne yönlendir.
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Hata oluştu.";
            return View();
        }

    }



    [HttpPost]
    public IActionResult Login(string username, string password, bool rememberMe)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        if (user != null)
        {
            if (rememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1), // 1 ay süreyle geçerli
                    HttpOnly = true, // JavaScript tarafından erişilemez
                    Secure = true, // Yalnızca HTTPS üzerinden gönderilir
                };
                Response.Cookies.Append("UserId", user.UserId.ToString(), cookieOptions);
            }
            //SESSION'A VERI KAYDETME
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("DeliveryAddress", user.Address);


            if (user.Role == "C")
            {
                return RedirectToAction("Index", "Home"); // Müşteri anasayfasına yönlendir
            }
            else if (user.Role == "P")
            {
                // Restoranın olup olmadığını kontrol et
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == user.UserId);
                if (restaurant != null)
                {
                    // Restoranı varsa dashboard'a yönlendir
                    return RedirectToAction("Dashboard", "Restaurants");
                }
                else
                {
                    // Restoranı yoksa restoran kayıt sayfasına yönlendir
                    return RedirectToAction("RegisterRestaurant");
                }
            }

            else if (user.Role == "A")
            {
                return RedirectToAction("Index", "Admin"); // Admin paneline yönlendir
            }
            else
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı tipi.";
                return View(); // Hatalı rol
            }
        }
        else
        {
            ViewBag.ErrorMessage = "Kullanıcı adı veya şifre yanlış girildi.";
            return View(); // Giriş başarısız
        }
    }


    public IActionResult Logout()
    {
        //Response.Cookies.Delete("BeniHatirla");
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [CustomAuthorize]
    public IActionResult Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        // Kullanıcının profil bilgilerini al
        var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

        // Aktif siparişleri al
        var activeOrders = _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.MenuItem)
            .ThenInclude(mi => mi.Restaurant)
            .Where(o => o.UserId == userId && o.Status == "Aktif")
            .Select(o => new ProfileViewModel.OrderWithRestaurant
            {
                Order = o,
                RestaurantName = o.OrderDetails.Select(od => od.MenuItem.Restaurant.Name).FirstOrDefault()
            })
            .ToList();

        // Geçmiş siparişleri al
        var pastOrders = _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.MenuItem)
            .ThenInclude(mi => mi.Restaurant)
            .Where(o => o.UserId == userId && o.Status == "Tamamlandı")
            .Select(o => new ProfileViewModel.OrderWithRestaurant
            {
                Order = o,
                RestaurantName = o.OrderDetails.Select(od => od.MenuItem.Restaurant.Name).FirstOrDefault()
            })
            .ToList();

        // Tamamlanmış ancak değerlendirilmemiş siparişleri al
        var unreviewedOrders = _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.MenuItem)
            .ThenInclude(mi => mi.Restaurant)
            .Where(o => o.UserId == userId && o.Status == "Tamamlandı" && !_context.Reviews.Any(r => r.OrderId == o.OrderId))
            .Select(o => new ProfileViewModel.OrderWithRestaurant
            {
                Order = o,
                RestaurantName = o.OrderDetails.Select(od => od.MenuItem.Restaurant.Name).FirstOrDefault()
            })
            .ToList();


        var model = new ProfileViewModel
        {
            User = user,
            ActiveOrders = activeOrders,
            PastOrders = pastOrders,
            UnreviewedOrders = unreviewedOrders
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult SubmitReview(int orderId, float rating, string comment)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        // İlgili siparişi bul
        var order = _context.Orders
                            .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.MenuItem)
                            .FirstOrDefault(o => o.OrderId == orderId);

        if (order != null && order.UserId == userId)
        {
            // İlgili restoranı bul
            var restaurantId = order.OrderDetails.FirstOrDefault()?.MenuItem?.RestaurantId;

            if (restaurantId != null)
            {
                // Yorumu ve puanı veritabanına ekle
                var review = new Review
                {
                    OrderId = orderId, // Bu satırı ekledik
                    UserId = userId.Value,
                    RestaurantId = restaurantId.Value,
                    Rating = rating,
                    Comment = comment
                };

                _context.Reviews.Add(review);

                // Restoranın yeni ortalama puanını hesapla
                var allRatings = _context.Reviews.Where(r => r.RestaurantId == restaurantId.Value).Select(r => r.Rating).ToList();
                allRatings.Add(rating); // Yeni eklenen puanı da dahil et
                var averageRating = allRatings.Average();

                // Restoranın puanını güncelle
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId.Value);
                if (restaurant != null)
                {
                    restaurant.Rating = averageRating;
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
        }

        return Json(new { success = false });
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

    [HttpGet]
    public JsonResult GetReviewDetails(int orderId)
    {
        try
        {
            // Fetching the review details based on the orderId
            var reviewDetails = _context.Reviews
                                        .Where(r => r.OrderId == orderId)
                                        .Select(r => new { comment = r.Comment, rating = r.Rating })
                                        .FirstOrDefault();

            var review = reviewDetails?.comment ?? "Bu sipariş için bir değerlendirme yapılmamış.";
            var rating = reviewDetails?.rating.ToString("0") + " / 5" ?? "Değerlendirme yapılmamış.";

            // Preparing the response
            var response = new { review, rating };

            return Json(response);
        }
        catch (Exception e)
        {
            // Log the error if needed
            return Json(new { error = e.Message });
        }
    }


}
