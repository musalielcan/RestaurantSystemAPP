using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Order & OrderItem
    public class OrderItem
    {
        public MenuItem Item { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal TotalPrice => Item.Price * Quantity;
    }
}
