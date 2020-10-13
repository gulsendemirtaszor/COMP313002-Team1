using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class BookSearch
    {
        public string Item { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal PriceTo { get; set; }

        public int Category { get; set; }
    }
}
