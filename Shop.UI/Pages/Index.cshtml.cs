using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application.Products;
using Shop.Database;
using Shop.Application;
using static Shop.Application.Products.CreateProduct;

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
        public ProductViewModel Product { get; set; }        

        public void OnGet()
        {

        }

        public async Task<IActionResult> Onpost()
        {
            await new CreateProduct(_ctx).Do(Product);

            return RedirectToPage("Index");
        }
    }
}
