using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Linq;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private readonly GetCart _getCart;

        public CartViewComponent(GetCart getCart)
        {
            _getCart = getCart;
        }
        public IViewComponentResult Invoke(string view = "Default")
        {
            if(view =="Small")
            {
                var total = _getCart.Do().Sum(x => x.RealValue * x.Qty);
                return View(view, $"{total:N2}₽");
            }

            return View(view, _getCart.Do());
        }
    }
}
