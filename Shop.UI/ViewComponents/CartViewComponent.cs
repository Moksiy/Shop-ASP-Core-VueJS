using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private readonly ApplicationDBContext _ctx;

        public CartViewComponent(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public IViewComponentResult Invoke(string view = "Default")
        {
            if(view =="Small")
            {
                var total = new GetCart(HttpContext.Session, _ctx).Do().Sum(x => x.RealValue * x.Qty);
                return View(view, $"{total:N2}₽");
            }

            return View(view, new GetCart(HttpContext.Session, _ctx).Do());
        }
    }
}
