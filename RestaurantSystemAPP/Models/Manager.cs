using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    public class Manager : Employee
    {
        public Manager(string fullName, string phone) : base(fullName, phone, "Manager") { }
    }
}
