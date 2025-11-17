using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Menu set (combo)
    public class MenuSet
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();

        public override string ToString() => $"{Id} Set: {Name} - {Price:C} ({Items.Count} items)";
    }
}
