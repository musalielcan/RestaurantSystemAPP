using RestaurantSystemAPP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Reservation
    public class Reservation
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Customer Customer { get; set; }
        public Table Table { get; set; }
        public DateTime DateTime { get; set; }
        public decimal DepositAmount { get; set; }
        public bool DepositPaid { get; set; } = true;
        public PaymentType? DepositPaymentType { get; set; }

        public override string ToString() => $"Reservation {Id} | {Customer.FullName} | Table {Table.Number} | {DateTime} | Deposit: {DepositAmount:C} Paid: {DepositPaid}";
    }
}
