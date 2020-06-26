using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Database;
using Shop.Domain.Infrastructure;
using GetOrderCart = Shop.Application.Cart.GetOrder;

namespace Shop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
        public IActionResult OnGet([FromServices] GetCustomerInformation getCustomerInformation)
        {
            var information = getCustomerInformation.Do();

            if (information == null)
                return RedirectToPage("/Checkout/CustomerInformation");
            else
                return Page();
        }

        public async Task<IActionResult> OnPost(
            [FromServices] GetOrderCart getOrder,
            [FromServices] CreateOrder createOrder,
            [FromServices] ISessionManager sessionManager)
        {
            var CartOrder = getOrder.Do();

            var sessionID = HttpContext.Session.Id;

            await createOrder.Do(new CreateOrder.Request
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

            sessionManager.ClearCart();

            return RedirectToPage("/Index");
        }
    }
}
