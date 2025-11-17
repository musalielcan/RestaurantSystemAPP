using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    public class Order
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal Total => Items.Sum(i => i.TotalPrice);
    }
}
