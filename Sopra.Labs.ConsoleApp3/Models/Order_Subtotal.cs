using System;
using System.Collections.Generic;

#nullable disable

namespace Sopra.Labs.ConsoleApp3.Models
{
    public partial class Order_Subtotal
    {
        public int OrderID { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
