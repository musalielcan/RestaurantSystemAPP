using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Simple Person base
    public abstract class Person
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string Phone { get; set; }

        protected Person(string fullName, string phone)
        {
            FullName = fullName ?? throw new ValidationException("FullName teleb olunur");
            Phone = phone;
        }
    }

}
