using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductCountViewModel
    {
        public IEnumerable<Product> products { get; set; } 
        public IEnumerable<OrderProduct> orderProducts { get; set; }

        public IEnumerable<Int32> counts { get; set; }
    }
}
