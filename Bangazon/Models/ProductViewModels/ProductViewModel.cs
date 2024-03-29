﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductViewModel
    {
        public Product product { get; set; }

        public SelectList productTypes { get; set; }
        public IFormFile ProductImage { get; set; }

        public string ErrorMessage { get; set; }
    }

}
