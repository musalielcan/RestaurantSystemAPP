using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Restaurant container
    public class Restaurant
    {
        public string Name { get; set; }
        public decimal Balance { get; set; } = 0m;

        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<MenuItem> Menu { get; set; } = new List<MenuItem>();
        public List<MenuSet> Sets { get; set; } = new List<MenuSet>();
        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
