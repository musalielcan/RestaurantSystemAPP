using RestaurantSystemAPP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Menu item
    public class MenuItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public MenuCategory Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Id} {Name} ({Category}) - {Price:C}";
    }
}
