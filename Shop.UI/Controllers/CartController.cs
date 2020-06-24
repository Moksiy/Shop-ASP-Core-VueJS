using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Threading.Tasks;

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

        [HttpPost("{stockID}")]
        public async Task<IActionResult> RemoveOne(
            int stockID,
            [FromServices] RemoveFromCart removeFromCart)
        {
            var request = new RemoveFromCart.Request
            {
                StockID = stockID,
                Qty = 1
            };

            var success = await removeFromCart.Do(request);

            if (success)
                return Ok("Item removed from cart");

            return BadRequest("Failed remove item from cart");
        }

        [HttpPost("{stockID}")]
        public async Task<IActionResult> RemoveAll(
            int stockID,
            [FromServices] RemoveFromCart removeFromCart
            )
        {
            var request = new RemoveFromCart.Request
            {
                StockID = stockID,
                All = true
            };

            var success = await removeFromCart.Do(request);

            if (success)
                return Ok("Item removed all from cart");

            return BadRequest("Failed to remove all items from cart");
        }
    }
}
