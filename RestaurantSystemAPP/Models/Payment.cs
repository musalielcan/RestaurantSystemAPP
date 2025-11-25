using RestaurantSystemAPP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Payment
    public class Payment
    {
        public Guid Id { get; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public PaymentType Type { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
