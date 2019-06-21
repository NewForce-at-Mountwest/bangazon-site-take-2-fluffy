using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models
{
    public class ProductRecommendation
    {
        [Required]
        public string UserId { get; set; }
        public string RecommenderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Recommender { get; set; }

    }
}
