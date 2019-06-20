using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Bangazon.Models.ProductViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Bangazon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index(string searchString, string locationString)
        {
            var currentUser = await GetCurrentUserAsync();

            var applicationDbContext = _context.Product.Include(p => p.ProductType).Include(p => p.User).Where(p=>p.UserId!=null);

            if (searchString != null)
            {
                applicationDbContext = applicationDbContext.Where(p => p.Title.Contains(searchString));
            }
            if (locationString != null)
            {
                applicationDbContext = applicationDbContext.Where(p => p.City.Contains(locationString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {

            ProductViewModel productModel = new ProductViewModel();
            SelectList productTypes = new SelectList(_context.ProductType, "ProductTypeId", "Label");
            // Add a 0 option to the select list
            SelectList productTypes0 = ProductTypeDropdown(productTypes);

            productModel.productTypes = productTypes0;
            return View(productModel);


        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productModel)
        {
            ModelState.Remove("product.User");
            ModelState.Remove("product.UserId");

            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();

                productModel.product.UserId = currentUser.Id;
                //Store the image in a temp location as it comes back from the uploader
                using (var memoryStream = new MemoryStream())
                {
                    await productModel.ProductImage.CopyToAsync(memoryStream);
                    productModel.product.ProductImage = memoryStream.ToArray();
                }

                _context.Add(productModel.product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = productModel.product.ProductId });
            }

            SelectList productTypes = new SelectList(_context.ProductType, "ProductTypeId", "Label");
            // Add a '0' option to the select list
            SelectList productTypes0 = ProductTypeDropdown(productTypes);
            productModel.productTypes = productTypes0;
            return View(productModel);
        }

        //Add to cart method - Authored by Sable Bowen
        [HttpGet]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> AddToCart(int id){

            //Gets user, products in cart, and the currently open order
            var currentUser = await GetCurrentUserAsync();

            var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == id);

            List<Order> orderList = await _context.Order.Where(o => o.UserId == currentUser.Id).ToListAsync();

            Order order = new Order()
            {
                DateCreated = DateTime.Now,
                UserId = currentUser.Id
            };

            //Checks if any orders are incomplete/current and adds the product to the incomplete order if it exists
            if (orderList.Any(o => o.PaymentTypeId == null))
            {
                Order currentOrder = orderList.Where(o => o.PaymentTypeId == null).FirstOrDefault();
                OrderProduct orderproduct = new OrderProduct()
                {
                    ProductId = id,
                    OrderId = currentOrder.OrderId
                };
                orderproduct.OrderId = currentOrder.OrderId;
                _context.Add(orderproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            //Adds a new order and the product to the new order if one is not already open
            {
                OrderProduct orderproduct = new OrderProduct()
                {
                    ProductId = id,
                    OrderId = order.OrderId
                };
                _context.Add(order);
                _context.Add(orderproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(product);
        }







        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,DateCreated,Description,Title,Price,Quantity,UserId,City,ImagePath,Active,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

        /// <summary>
        /// Adds a '0' option for the select list
        /// </summary>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public static SelectList ProductTypeDropdown(SelectList selectList)
        {

            SelectListItem firstItem = new SelectListItem()
            {
               Text = "Select a Product Type"
            };
            List<SelectListItem> newList = selectList.ToList();
            newList.Insert(0, firstItem);      

            var selectedItem = newList.FirstOrDefault(item => item.Selected);
            var selectedItemValue = String.Empty;
            if (selectedItem != null)
            {
                selectedItemValue = selectedItem.Value;
            }

            return new SelectList(newList, "Value", "Text", selectedItemValue);
        }

       
    }
}
