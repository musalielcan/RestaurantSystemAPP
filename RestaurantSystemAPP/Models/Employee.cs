using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Employees
    public class Employee : Person
    {
        public string Position { get; set; }
        public Employee(string fullName, string phone, string position) : base(fullName, phone)
        {
            Position = position;
        }
    }
}
