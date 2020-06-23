using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application.ProductsAdmin;
using Shop.Database;
using Shop.Application;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _ctx;
        public IndexModel(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty]
        public CreateProduct.Request Product { get; set; }        

        public IEnumerable<Application.Products.GetProducts.ProductViewModel> Products { get; set; }

        public void OnGet()
        {
            Products = new Application.Products.GetProducts(_ctx).Do();
        }
    }
}
