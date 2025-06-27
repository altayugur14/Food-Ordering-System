using Microsoft.AspNetCore.Mvc;
using YemekProjeTest2.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace YemekProjeTest2.Controllers
{
    [CustomAuthorize]
    public class CartController : Controller
    {
        private readonly YemekSiparisSistemiContext _context;

        public CartController(YemekSiparisSistemiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuItemId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            // Find the restaurantId of the menuItem
            var restaurantId = _context.MenuItems
                                        .Where(mi => mi.MenuItemId == menuItemId)
                                        .Select(mi => mi.RestaurantId)
                                        .FirstOrDefault();

            if (userId == null)
            {
                TempData["ErrorMessage"] = "Sepete ekleme işlemi için giriş yapmalısınız.";
                return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
            }


            // Check if there are items from other restaurants in the cart
            var otherRestaurantItems = _context.CartItems
                                                .Include(ci => ci.MenuItem)
                                                .Where(ci => ci.UserId == userId && ci.MenuItem.RestaurantId != restaurantId)
                                                .Any();

            if (otherRestaurantItems)
            {
                // Redirect the user back to the restaurant page and show an error message
                TempData["ErrorMessage"] = "Sepetinizde farklı bir restoranın yemeği var. Yeni bir yemek eklemek için sepetinizi boşaltmanız gerekiyor.";
                return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
            }

            var cartItem = await _context.CartItems
                        .FirstOrDefaultAsync(i => i.MenuItemId == menuItemId && i.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new CartItem { MenuItemId = menuItemId, Quantity = quantity, UserId = userId };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();

            // Redirect to the page the user was at.
            // Pass the menuItemId to go back to the correct restaurant
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        [HttpPost]
        public async Task<IActionResult> EmptyCart(int userId)
        {
            // Get all items in the user's cart
            var cartItems = _context.CartItems.Where(ci => ci.UserId == userId);

            // Remove all items
            _context.CartItems.RemoveRange(cartItems);

            // Save changes
            await _context.SaveChangesAsync();

            // Redirect to the cart page
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity -= 1;
                if (cartItem.Quantity == 0)
                {
                    _context.CartItems.Remove(cartItem);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            var cartItems = await _context.CartItems
                                          .Where(ci => ci.UserId == userId)
                                          .ToListAsync();
            return Json(cartItems);
        }


        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId"); // Oturumdan alınan kullanıcı kimliği
            var cartItems = _context.CartItems
                .Include(ci => ci.MenuItem) // Include MenuItems in order to get item details
                .Where(ci => ci.UserId == userId)
                .ToList();

            double totalCost = cartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price);
            ViewData["TotalCost"] = totalCost;//totali yazdırmak için burada(backendde) hesaplayıp view e gönder

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var deliveryAddress = HttpContext.Session.GetString("DeliveryAddress");
            System.Diagnostics.Debug.WriteLine($"UserId: {userId}, DeliveryAddress: {deliveryAddress}"); // Bu satırı ekleyin
            // Kullanıcının sepet içeriğini al
            var cartItems = _context.CartItems
                                    .Include(ci => ci.MenuItem)
                                    .Where(ci => ci.UserId == userId)
                                    .ToList();

            // Yeni sipariş oluştur
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = cartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price),
                DeliveryAddress = deliveryAddress, // Varsayılan teslimat adresi
                Status = "Aktif"
            };
            System.Diagnostics.Debug.WriteLine($"Order nesnesi: UserId={order.UserId}, OrderDate={order.OrderDate}, TotalPrice={order.TotalPrice}, DeliveryAddress={order.DeliveryAddress}");
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Sipariş detaylarını oluştur
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();

            // Sepeti boşalt
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(); // Başarı durumunu döndür
        }

        public IActionResult Confirmation()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // En son onaylanan siparişi al
            var order = _context.Orders
                               .Include(o => o.OrderDetails)
                               .ThenInclude(od => od.MenuItem)
                               .Where(o => o.UserId == userId)
                               .OrderByDescending(o => o.OrderDate)
                               .FirstOrDefault();

            if (order == null)
            {
                return RedirectToAction("Index", "Home"); // Sipariş bulunamazsa anasayfaya yönlendir
            }

            return View(order);
        }


    }
}

