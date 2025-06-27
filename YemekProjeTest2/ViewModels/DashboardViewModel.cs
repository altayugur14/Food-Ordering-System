using YemekProjeTest2.Models;
using System.Collections.Generic;

namespace YemekProjeTest2.ViewModels
{
    public class DashboardViewModel
    {
        public Restaurant Restaurant { get; set; }
        public List<Order> Orders { get; set; }

        public Dictionary<int, string> UserNames { get; set; } // Sipariş sahiplerinin adlarını tutar
    }
}
