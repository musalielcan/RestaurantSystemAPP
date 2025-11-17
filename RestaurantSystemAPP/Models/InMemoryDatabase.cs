using RestaurantSystemAPP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    public static class InMemoryDatabase
    {
        public static Restaurant Restaurant { get; } = new Restaurant { Name = "Demo Restaurant", Balance = 1000m };

        // Seed some data
        static InMemoryDatabase()
        {
            // Tables
            for (int i = 1; i <= 8; i++)
                Restaurant.Tables.Add(new Table { Number = i, Seats = (i <= 4 ? 2 : 4), Deposit = (i <= 4 ? 30 : 50) });

            // Menu items
            Restaurant.Menu.AddRange(new[]
            {
                new MenuItem { Name = "Kabab", Category = MenuCategory.HotMeals, Price = 15.00m, Description = "Qızardılmış ət" },
                new MenuItem { Name = "Salad", Category = MenuCategory.Appetizers, Price = 6.50m, Description = "Təzə salat" },
                new MenuItem { Name = "Ice Cream", Category = MenuCategory.Desserts, Price = 4.00m, Description = "Vanil" },
                new MenuItem { Name = "Tea", Category = MenuCategory.HotDrinks, Price = 1.50m, Description = "Qara çay" },
                new MenuItem { Name = "Cola", Category = MenuCategory.ColdDrinks, Price = 2.00m, Description = "Qazlı içki" },
                new MenuItem { Name = "Beer", Category = MenuCategory.AlcoholicDrinks, Price = 3.50m, Description = "Yerli pivə" }
            });

            // One admin and one manager user for demo
            var admin = new Admin("Super Admin", "+994555000001");
            //var manager = new Manager("Main Manager", "+994555000002");
            Restaurant.Employees.Add(admin);
            //Restaurant.Employees.Add(manager);

            // Demo customer
            //var customer = new Customer("Test User", "+994555000003", "user@example.com", "password");
            //Restaurant.Customers.Add(customer);
        }
    }

}
