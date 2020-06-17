using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Database;

namespace Shop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
        private readonly ApplicationDBContext _ctx;

        public PaymentModel(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            if (information == null)
                return RedirectToPage("/Checkout/CustomerInformation");
            else
                return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var CartOrder = new Application.Cart.GetOrder(HttpContext.Session, _ctx).Do();

            var sessionID = HttpContext.Session.Id;

            await new CreateOrder(_ctx).Do(new CreateOrder.Request
            {              
                StripeReference = "",
                SessionID = sessionID,
                FirstName = CartOrder.CustomerInformation.FirstName,
                LastName = CartOrder.CustomerInformation.LastName,
                Email = CartOrder.CustomerInformation.Email,
                PhoneNumber = CartOrder.CustomerInformation.PhoneNumber,
                Address1 = CartOrder.CustomerInformation.Address1,
                Address2 = CartOrder.CustomerInformation.Address2,
                City = CartOrder.CustomerInformation.City,
                PostCode = CartOrder.CustomerInformation.PostCode,

                Stocks = CartOrder.Products.Select(x => new CreateOrder.Stock
                {
                    StockID = x.StockID,
                    Qty = x.Qty
                }).ToList()
            });
            return RedirectToPage("/Index");
        }
    }
}
