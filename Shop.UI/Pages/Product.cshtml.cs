using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ApplicationDBContext _ctx;

        public ProductModel(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty]
        public Test ProductTest { get; set; }

        public class Test
        {
            public string ID { get; set; }
        }

        public GetProduct.ProductViewModel Product { get; set; }

        public IActionResult OnGet(string name)
        {
            Product = new GetProduct(_ctx).Do(name.Replace("-", " "));
            if (Product == null)
                return RedirectToPage("Index");
            else
                return Page();
        }

        public IActionResult OnPost()
        {
            var current_id = HttpContext.Session.GetString("id");

            HttpContext.Session.SetString("id", ProductTest.ID);

            return RedirectToPage("Index");
        }
    }
}
