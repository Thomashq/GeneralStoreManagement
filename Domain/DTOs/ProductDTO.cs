using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }

        public int Status { get; set; }

        public int Amount { get; set; }

        public int Section { get; set; }

        public decimal Price { get; set; }
    }
}
