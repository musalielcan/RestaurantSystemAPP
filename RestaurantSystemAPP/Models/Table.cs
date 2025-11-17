using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystemAPP.Models
{
    // Table
    public class Table
    {
        public Guid Id { get; } = Guid.NewGuid();
        public int Number { get; set; }
        public int Seats { get; set; }
        public decimal Deposit {  get; set; }
        public bool IsAvailable { get; set; } = false;

        public override string ToString() => $"{Id} Masa {Number} (Oturacaq: {Seats}) (Depozit: {Deposit}) - {(IsAvailable ? "Rezerv" : "Bos")}";
    }
}
