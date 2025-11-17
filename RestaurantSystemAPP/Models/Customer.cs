using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Customer / User
    public class Customer : Person
    {
        public decimal Balance {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Yalnız demo üçün; real tətbiqlərdə düz mətni SAXLAMAYIN
        public Customer(string fullName, string phone, decimal balance, string email, string password) : base(fullName, phone)
        {
            Balance = balance;
            Email = email;
            Password = password;
        }
    }
}
