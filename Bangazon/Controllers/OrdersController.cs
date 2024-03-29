﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Bangazon.Models.OrderViewModels;

namespace Bangazon.Controllers
{
    //Manages order creation and completion - Authored by Sable Bowen
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.Order.Include(o => o.PaymentType).Where(o => o.UserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> EmptyCart()
        {
            //View that displays when a cart is empty
            return View();
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id, Order model)
        {
            //If cart is empty, redirect to a view with a message saying the cart is empty


           var user = await GetCurrentUserAsync();



            if(id == 0)
            {
                var Order = await _context.Order.Include(o => o.PaymentType).Include(o => o.User).Include(o => o.OrderProducts).Where(o => o.PaymentTypeId == null && o.UserId == user.Id).FirstOrDefaultAsync();



                if (Order == null)
                {
                    return RedirectToAction(nameof(EmptyCart));
                }

                var OrderProducts = await _context.OrderProduct.Where(o => o.OrderId == Order.OrderId).Include(o => o.Product).ToListAsync();


                Order.OrderProducts = OrderProducts.ToList();



                if (Order == null || Order.UserId != user.Id)
                {
                    return NotFound();
                }

                return View(Order);

            } else
            {
                var Order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User).Include(o => o.OrderProducts).Where(o => o.OrderId == id)
                .FirstOrDefaultAsync(o => o.OrderId == id);

                var OrderProducts = await _context.OrderProduct.Where(o => o.OrderId == Order.OrderId).Include(o => o.Product).ToListAsync();


                Order.OrderProducts = OrderProducts.ToList();



                if (Order == null || Order.UserId != user.Id)
                {
                    return NotFound();
                }

                return View(Order);
            }


        }

        // GET: Orders/CompletePayment/5

        //Gets order and payment types for payment completion dropdown list in view
        public async Task<IActionResult> CompletePayment(int id)
        {
            var user = await GetCurrentUserAsync();
            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderId == id);
            var paymentTypes = await _context.PaymentType.Where(p => p.UserId == user.Id).ToListAsync();


            //Creates dropdown for payment types
            var viewModel = new OrderPaymentViewModel()
            {
                Order = order,
                PaymentTypes = paymentTypes.Select(c => new SelectListItem
                {
                    Value = c.PaymentTypeId.ToString(),
                    Text = c.AccountNumber
                }).ToList()
            };
            return View(viewModel);

        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        [HttpPost]
        //GET: Orders/PaymentUpdate/5
        public async Task<IActionResult> CompletePayment(int id, OrderPaymentViewModel vm)
            {


            //Gets Order
            var order = await _context.Order.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.OrderId == id);

            var OrderProducts = await _context.OrderProduct.Include(o => o.Product).Where(o => o.OrderId == id).ToListAsync();

            order.DateCompleted = DateTime.Now;
            order.PaymentTypeId = vm.Order.PaymentTypeId;

            //Loops through products, decrements their quantity, and updates database
            foreach(OrderProduct singleOrderProduct in OrderProducts)
            {
                singleOrderProduct.Product.Quantity = singleOrderProduct.Product.Quantity - 1;
                _context.Update(singleOrderProduct.Product);
            }

            ModelState.Remove("Order.UserId");
            ModelState.Remove("Order.User");
            if (ModelState.IsValid)
            {
                _context.Update(order);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Confirmation));
            }


            
            return View(vm);
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .Include(o=>o.OrderProducts)
                .ThenInclude(op=>op.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItems = await _context.OrderProduct.Where(op=>op.OrderId==id).ToListAsync();
            foreach(var orderItem in orderItems) { 
            _context.OrderProduct.Remove(orderItem);
            }
            await _context.SaveChangesAsync();


            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

       
    }
}
