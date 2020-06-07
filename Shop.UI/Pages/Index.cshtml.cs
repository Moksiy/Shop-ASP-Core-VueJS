using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application.CreateProducts;
using Shop.Database;
using Shop.Application;
using Shop.Application.GetProducts;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationDBContext _ctx;
        public IndexModel(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty]
        public Application.CreateProducts.CreateProduct.ProductViewModel Product { get; set; }        

        public IEnumerable<Application.GetProducts.GetProducts.ProductViewModel> Products { get; set; }

        public void OnGet()
        {
            Products = new GetProducts(_ctx).Do();
        }

        public async Task<IActionResult> Onpost()
        {
            await new CreateProduct(_ctx).Do(Product);

            return RedirectToPage("Index");
        }
    }
}
