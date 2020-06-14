using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Checkout
{
    public class CustomerinformationModel : PageModel
    {
        [Obsolete]
        private readonly IHostingEnvironment _env;

        [Obsolete]
        public CustomerinformationModel(IHostingEnvironment env)
        {
            _env = env;
        }

        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }

        [Obsolete]
        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            if (information == null)
            {
                if(_env.IsDevelopment())
                {
                    CustomerInformation = new AddCustomerInformation.Request
                    {
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "Email@gmail.com",
                        PhoneNumber = "PhoneNumber",
                        Address1 = "Address1",
                        Address2 = "Address2",
                        City = "City",
                        PostCode = "PostCode"
                    }; 
                }
                return Page();
            }
            else
                return RedirectToPage("/Checkout/Payment");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            new AddCustomerInformation(HttpContext.Session).Do(CustomerInformation);

            return RedirectToPage("/Checkout/Payment");
        }
    }
}
