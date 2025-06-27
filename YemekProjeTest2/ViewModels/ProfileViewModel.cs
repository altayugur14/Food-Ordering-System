using YemekProjeTest2.Models;
using System.Collections.Generic;

namespace YemekProjeTest2.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; } // Kullanıcının profil bilgileri
        public List<OrderWithRestaurant> ActiveOrders { get; set; } // Kullanıcının aktif siparişleri
        public List<OrderWithRestaurant> PastOrders { get; set; } // Kullanıcının tamamlanmış/geçmiş siparişleri
        public List<OrderWithRestaurant> UnreviewedOrders { get; set; } // Değerlendirilmemiş siparişler

        public class OrderWithRestaurant
        {
            public Order Order { get; set; }
            public string RestaurantName { get; set; }
        }
    }
}
