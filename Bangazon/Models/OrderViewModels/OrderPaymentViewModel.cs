using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.OrderViewModels
{
    //Adds a select list item to orders so it may be used for a dropdown in the complete payment view - Authored by Sable Bowen
    public class OrderPaymentViewModel
    {
        public Order Order { get; set; }
        public List<SelectListItem> PaymentTypes { get; set; } = new List<SelectListItem>();

    }
}
