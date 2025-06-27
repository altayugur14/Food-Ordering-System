using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekProjeTest2.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace YemekProjeTest2.Controllers
{
    [CustomAuthorize("A")]
    public class AdminController : Controller
    {
        private readonly YemekSiparisSistemiContext _context;

        public AdminController(YemekSiparisSistemiContext context)
        {
            _context = context;
        }

        //READ BAŞI

        public JsonResult GetUsers(int page = 1, int pageSize = 10)
        {
            var users = _context.Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(users);
        }

        public JsonResult GetRestaurants(int page = 1, int pageSize = 10)
        {
            var restaurants = _context.Restaurants.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(restaurants);
        }

        public JsonResult GetOrders(int page = 1, int pageSize = 10)
        {
            var orders = _context.Orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(orders);
        }

        public JsonResult GetMenuItems(int page = 1, int pageSize = 10)
        {
            var menuItems = _context.MenuItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(menuItems);
        }

        public JsonResult GetCuisines(int page = 1, int pageSize = 10)
        {
            var cuisines = _context.Cuisines.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(cuisines);
        }

        public JsonResult GetReviews(int page = 1, int pageSize = 10)
        {
            var reviews = _context.Reviews.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(reviews);
        }

        public JsonResult GetOrderDetails(int page = 1, int pageSize = 10)
        {
            var orderDetails = _context.OrderDetails.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(orderDetails);
        }

        public JsonResult GetPromotions(int page = 1, int pageSize = 10)
        {
            var promotions = _context.Promotions.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(promotions);
        }

        public JsonResult GetPaymentMethods(int page = 1, int pageSize = 10)
        {
            var paymentMethods = _context.PaymentMethods.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(paymentMethods);
        }
        //READ SONU
        //CREATE BAŞI

        [HttpPost]
        public JsonResult AddUsers([FromBody] User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Json(new { success = true, message = "Kullanıcı başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddRestaurants([FromBody] Restaurant restaurant)
        {
            try
            {
                _context.Restaurants.Add(restaurant);
                _context.SaveChanges();
                return Json(new { success = true, message = "Restoran başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddOrders([FromBody] Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return Json(new { success = true, message = "Sipariş başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddMenuItems([FromBody] MenuItem menuItem)
        {
            try
            {
                _context.MenuItems.Add(menuItem);
                _context.SaveChanges();
                return Json(new { success = true, message = "Menü içeriği başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddCuisines([FromBody] Cuisine cuisine)
        {
            try
            {
                _context.Cuisines.Add(cuisine);
                _context.SaveChanges();
                return Json(new { success = true, message = "Mutfak başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddReviews([FromBody] Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return Json(new { success = true, message = "Değerlendirme başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddOrderDetails([FromBody] OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
                return Json(new { success = true, message = "Sipariş detayı başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddPromotions([FromBody] Promotion promotion)
        {
            try
            {
                _context.Promotions.Add(promotion);
                _context.SaveChanges();
                return Json(new { success = true, message = "Promosyon başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost]
        public JsonResult AddPaymentMethods([FromBody] PaymentMethod paymentMethod)
        {
            try
            {
                _context.PaymentMethods.Add(paymentMethod);
                _context.SaveChanges();
                return Json(new { success = true, message = "Ödeme yöntemi başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }
        //CREATE SONU
        //UPDATE BAŞI

        [HttpPost("UpdateUsers")]
        public IActionResult UpdateUsers([FromBody] User userToUpdate)
        {
            try
            {
                if (userToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen kullanıcı bilgisi hatalı!" });
                }

                var existingUser = _context.Users.Find(userToUpdate.UserId);
                if (existingUser == null)
                {
                    return NotFound(new { success = false, message = "Kullanıcı bulunamadı!" }); // User not found!
                }

                // Update fields. All of them except primary key (UserId)
                existingUser.Username = userToUpdate.Username;
                existingUser.PasswordHash = userToUpdate.PasswordHash;
                existingUser.Email = userToUpdate.Email;
                existingUser.Role = userToUpdate.Role;
                existingUser.Address = userToUpdate.Address;
                existingUser.PhoneNumber = userToUpdate.PhoneNumber;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Kullanıcı başarıyla güncellendi!" }); // User successfully updated!
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdateOrders")]
        public IActionResult UpdateOrders([FromBody] Order orderToUpdate)
        {
            try
            {
                if (orderToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen sipariş bilgisi hatalı!" });
                }
                var existingOrder = _context.Orders.Find(orderToUpdate.OrderId);
                if (existingOrder == null)
                {
                    return NotFound(new { success = false, message = "Sipariş bulunamadı!" });
                }

                // Update fields. All of them except primary key
                existingOrder.UserId = orderToUpdate.UserId;
                existingOrder.OrderDate = orderToUpdate.OrderDate;
                existingOrder.TotalPrice = orderToUpdate.TotalPrice;
                existingOrder.DeliveryAddress = orderToUpdate.DeliveryAddress;
                existingOrder.Status = orderToUpdate.Status;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Sipariş başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdateRestaurants")]
        public IActionResult UpdateRestaurants([FromBody] Restaurant restaurantToUpdate)
        {
            try
            {
                if (restaurantToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen restoran bilgisi hatalı!" });
                }
                var existingRestaurant = _context.Restaurants.Find(restaurantToUpdate.RestaurantId);
                if (existingRestaurant == null)
                {
                    return NotFound(new { success = false, message = "Restoran bulunamadı!" });
                }

                // Update fields. All of them except primary ked (UserId)
                existingRestaurant.UserId = restaurantToUpdate.UserId;
                existingRestaurant.Name = restaurantToUpdate.Name;
                existingRestaurant.Address = restaurantToUpdate.Address;
                existingRestaurant.PhoneNumber = restaurantToUpdate.PhoneNumber;
                existingRestaurant.Rating = restaurantToUpdate.Rating;


                _context.SaveChanges();

                return Ok(new { success = true, message = "Restoran başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdateMenuItems")]
        public IActionResult UpdateMenuItems([FromBody] MenuItem menuItemToUpdate)
        {
            try
            {
                if (menuItemToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen menü öğesi bilgisi hatalı!" });
                }
                var existingMenuItem = _context.MenuItems.Find(menuItemToUpdate.MenuItemId);
                if (existingMenuItem == null)
                {
                    return NotFound(new { success = false, message = "Menü içeriği bulunamadı!" });
                }

                // Update fields. All of them except primary ked 
                existingMenuItem.RestaurantId = menuItemToUpdate.RestaurantId;
                existingMenuItem.Name = menuItemToUpdate.Name;
                existingMenuItem.Description = menuItemToUpdate.Description;
                existingMenuItem.Price = menuItemToUpdate.Price;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Menü içeriği başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdateCuisines")]
        public IActionResult UpdateCuisines([FromBody] Cuisine cuisineToUpdate)
        {
            try
            {
                if (cuisineToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen mutfak bilgisi hatalı!" });
                }
                var existingCuisine = _context.Cuisines.Find(cuisineToUpdate.CuisineId);
                if (existingCuisine == null)
                {
                    return NotFound(new { success = false, message = "Mutfak bulunamadı!" });
                }

                // Update fields. All of them except primary ked 
                existingCuisine.Name = cuisineToUpdate.Name;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Mutfak başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdateReviews")]
        public IActionResult UpdateReviews([FromBody] Review reviewToUpdate)
        {
            try
            {
                if (reviewToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen değerlendirme bilgisi hatalı!" });
                }

                var existingReview = _context.Reviews.Find(reviewToUpdate.ReviewId);
                if (existingReview == null)
                {
                    return NotFound(new { success = false, message = "Değerlendirme bulunamadı!" });
                }

                // Update fields. All of them except primary ked (UserId)
                existingReview.UserId = reviewToUpdate.UserId;
                existingReview.RestaurantId = reviewToUpdate.RestaurantId;
                existingReview.Rating = reviewToUpdate.Rating;
                existingReview.Comment = reviewToUpdate.Comment;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Değerlendirme başarıyla güncellendi!" }); // User successfully updated!
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdateOrderDetails")]
        public IActionResult UpdateOrderDetails([FromBody] OrderDetail orderDetailToUpdate)
        {
            try
            {
                if (orderDetailToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen sipariş detay bilgisi hatalı!" });
                }
                var existingOrderDetail = _context.OrderDetails.Find(orderDetailToUpdate.OrderDetailsId);
                if (existingOrderDetail == null)
                {
                    return NotFound(new { success = false, message = "Sipariş detayı bulunamadı!" });
                }

                // Update fields. All of them except primary ked 
                existingOrderDetail.OrderId = orderDetailToUpdate.OrderId;
                existingOrderDetail.MenuItemId = orderDetailToUpdate.MenuItemId;
                existingOrderDetail.Quantity = orderDetailToUpdate.Quantity;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Sipariş detayı başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdatePromotions")]
        public IActionResult UpdatePromotions([FromBody] Promotion promotionToUpdate)
        {
            try
            {
                if (promotionToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen promosyon bilgisi hatalı!" });
                }
                var existingPromotion = _context.Promotions.Find(promotionToUpdate.PromotionId);
                if (existingPromotion == null)
                {
                    return NotFound(new { success = false, message = "Promosyon bulunamadı!" });
                }

                // Update fields. All of them except primary ked 
                existingPromotion.RestaurantId = promotionToUpdate.RestaurantId;
                existingPromotion.PromoCode = promotionToUpdate.PromoCode;
                existingPromotion.DiscountPercentage = promotionToUpdate.DiscountPercentage;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Promosyon başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("UpdatePaymentMethods")]
        public IActionResult UpdatePaymentMethods([FromBody] PaymentMethod paymentMethodToUpdate)
        {
            try
            {
                if (paymentMethodToUpdate == null)
                {
                    return BadRequest(new { success = false, message = "Gönderilen ödeme yöntemi bilgisi hatalı!" });
                }
                var existingPaymentMethod = _context.PaymentMethods.Find(paymentMethodToUpdate.PaymentMethodId);
                if (existingPaymentMethod == null)
                {
                    return NotFound(new { success = false, message = "Ödeme yöntemi bulunamadı!" });
                }

                // Update fields. All of them except primary ked 
                existingPaymentMethod.UserId = paymentMethodToUpdate.UserId;
                existingPaymentMethod.CardName = paymentMethodToUpdate.CardName;
                existingPaymentMethod.CardNumber = paymentMethodToUpdate.CardNumber;
                existingPaymentMethod.ExpiryDate = paymentMethodToUpdate.ExpiryDate;

                _context.SaveChanges();

                return Ok(new { success = true, message = "Ödeme yöntemi başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }


        //UPDATE SONU
        //DELETE BAŞI
        [HttpPost("DeleteUsers")]
        public IActionResult DeleteUsers([FromBody] int userId)
        {
            try
            {
                var userToDelete = _context.Users.Find(userId);
                if (userToDelete == null)
                {
                    return NotFound(new { success = false, message = "Kullanıcı bulunamadı!" }); // User not found!
                }

                _context.Users.Remove(userToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Kullanıcı başarıyla silindi!" }); // User successfully deleted!
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        [HttpPost("DeleteOrders")]
        public IActionResult DeleteOrders([FromBody] int orderId)
        {
            try
            {
                var orderToDelete = _context.Orders.Find(orderId);
                if (orderToDelete == null)
                {
                    return NotFound(new { success = false, message = "Sipariş bulunamadı!" });
                }

                _context.Orders.Remove(orderToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Sipariş başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // Restaurants Table
        [HttpPost("DeleteRestaurants")]
        public IActionResult DeleteRestaurants([FromBody] int restaurantId)
        {
            try
            {
                var restaurantToDelete = _context.Restaurants.Find(restaurantId);
                if (restaurantToDelete == null)
                {
                    return NotFound(new { success = false, message = "Restoran bulunamadı!" });
                }

                _context.Restaurants.Remove(restaurantToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Restoran başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // MenuItems Table
        [HttpPost("DeleteMenuItems")]
        public IActionResult DeleteMenuItems([FromBody] int menuItemId)
        {
            try
            {
                var menuItemToDelete = _context.MenuItems.Find(menuItemId);
                if (menuItemToDelete == null)
                {
                    return NotFound(new { success = false, message = "Menü içeriği bulunamadı!" });
                }

                _context.MenuItems.Remove(menuItemToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Menü içeriği başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // Cuisines Table
        [HttpPost("DeleteCuisines")]
        public IActionResult DeleteCuisines([FromBody] int cuisineId)
        {
            try
            {
                var cuisineToDelete = _context.Cuisines.Find(cuisineId);
                if (cuisineToDelete == null)
                {
                    return NotFound(new { success = false, message = "Mutfak bulunamadı!" });
                }

                _context.Cuisines.Remove(cuisineToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Mutfak başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // Reviews Table
        [HttpPost("DeleteReviews")]
        public IActionResult DeleteReviews([FromBody] int reviewId)
        {
            try
            {
                var reviewToDelete = _context.Reviews.Find(reviewId);
                if (reviewToDelete == null)
                {
                    return NotFound(new { success = false, message = "Değerlendirme bulunamadı!" });
                }

                _context.Reviews.Remove(reviewToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Değerlendirme başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // OrderDetails Table
        [HttpPost("DeleteOrderDetails")]
        public IActionResult DeleteOrderDetails([FromBody] int orderDetailsId)
        {
            try
            {
                var orderDetailToDelete = _context.OrderDetails.Find(orderDetailsId);
                if (orderDetailToDelete == null)
                {
                    return NotFound(new { success = false, message = "Sipariş detayı bulunamadı!" });
                }

                _context.OrderDetails.Remove(orderDetailToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Sipariş detayı başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // Promotions Table
        [HttpPost("DeletePromotions")]
        public IActionResult DeletePromotions([FromBody] int promotionId)
        {
            try
            {
                var promotionToDelete = _context.Promotions.Find(promotionId);
                if (promotionToDelete == null)
                {
                    return NotFound(new { success = false, message = "Promosyon bulunamadı!" });
                }

                _context.Promotions.Remove(promotionToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Promosyon başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }

        // PaymentMethods Table
        [HttpPost("DeletePaymentMethods")]
        public IActionResult DeletePaymentMethods([FromBody] int paymentMethodId)
        {
            try
            {
                var paymentMethodToDelete = _context.PaymentMethods.Find(paymentMethodId);
                if (paymentMethodToDelete == null)
                {
                    return NotFound(new { success = false, message = "Ödeme metodu bulunamadı!" });
                }

                _context.PaymentMethods.Remove(paymentMethodToDelete);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Ödeme metodu başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bir hata oluştu!" });
            }
        }


        //DELETE SONU
        public IActionResult Index()
        {
            return View();
        }
    }
}
