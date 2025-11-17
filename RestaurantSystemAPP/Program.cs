using RestaurantSystemAPP.Enums;
using RestaurantSystemAPP.Exceptions;
using RestaurantSystemAPP.Models;

namespace RestaurantSystemAPP
{
    internal class Program
    {
        static void Main()
        {
            RestaurantSystem restaurantSystem = new RestaurantSystem();
            restaurantSystem.Run();            
        }
    }
}
