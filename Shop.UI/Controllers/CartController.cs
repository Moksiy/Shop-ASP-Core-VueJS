using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _ctx;

        public CartController(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost("{stockID}")]
        public async Task<IActionResult> AddOne(int stockID)
        {
            var request = new AddToCart.Request
            {
                StockID = stockID,
                Qty = 1
            };

            var addToCart = new AddToCart(HttpContext.Session, _ctx);

            var success = await addToCart.Do(request);

            if (success)
                return Ok("Item added to cart");

            return BadRequest("Failed add to cart");
        }

        [HttpPost("{stockID}")]
        public async Task<IActionResult> RemoveOne(int stockID)
        {
            var request = new RemoveFromCart.Request
            {
                StockID = stockID,
                Qty = 1
            };

            var addToCart = new RemoveFromCart(HttpContext.Session, _ctx);

            var success = await addToCart.Do(request);

            if (success)
                return Ok("Item removed from cart");

            return BadRequest("Failed remove item from cart");
        }

        [HttpPost("{stockID}")]
        public async Task<IActionResult> RemoveAll(int stockID)
        {
            var request = new RemoveFromCart.Request
            {
                StockID = stockID,
                All = true
            };

            var addToCart = new RemoveFromCart(HttpContext.Session, _ctx);

            var success = await addToCart.Do(request);

            if (success)
                return Ok("Item removed all from cart");

            return BadRequest("Failed to remove all items from cart");
        }
    }
}
