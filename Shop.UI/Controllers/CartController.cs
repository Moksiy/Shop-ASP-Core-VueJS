using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Threading.Tasks;
using System.Linq;

namespace Shop.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        [HttpPost("{stockID}")]
        public async Task<IActionResult> AddOne(
            int stockID,
            [FromServices] AddToCart addToCart)
        {
            var request = new AddToCart.Request
            {
                StockID = stockID,
                Qty = 1
            };

            var success = await addToCart.Do(request);

            if (success)
                return Ok("Item added to cart");

            return BadRequest("Failed add to cart");
        }

        [HttpPost("{stockID}/{qty}")]
        public async Task<IActionResult> Remove(
            int stockID,
            int qty,
            [FromServices] RemoveFromCart removeFromCart)
        {
            var request = new RemoveFromCart.Request
            {
                StockID = stockID,
                Qty = qty
            };

            var success = await removeFromCart.Do(request);

            if (success)
                return Ok("Item removed from cart");

            return BadRequest("Failed remove item from cart");
        }


        [HttpGet]
        public IActionResult GetCartNav([FromServices] GetCart getCart)
        {
            var total = getCart.Do().Sum(x => x.RealValue * x.Qty);

            return PartialView("Components/Cart/Small", total);
        }


        [HttpGet]
        public IActionResult GetCartMain([FromServices] GetCart getCart)
        {
            var cart = getCart.Do();

            return PartialView("_CartPartial", cart);
        }
    }
}
