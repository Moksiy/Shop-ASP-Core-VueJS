using Microsoft.EntityFrameworkCore;
using Shop.Application.Infrastructure;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Cart
{
    public class GetCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly ApplicationDBContext _ctx;

        public GetCart(ISessionManager sessionManager, ApplicationDBContext ctx)
        {
            _sessionManager = sessionManager;
            _ctx = ctx;
        }

        public class Response
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public decimal RealValue { get; set; }
            public int Qty { get; set; }
            public int StockID { get; set; }
        }

        public IEnumerable<Response> Do()
        {                       
            var cartList = _sessionManager.GetCart();

            if (cartList == null)
                return new List<Response>();

            var itemsInCart = cartList.Select(x => x.StockID).ToList();

            var response = _ctx.Stock
                .Include(x => x.Product)
                .Where(x => itemsInCart.Contains(x.ID))
                .Select(x => new Response
                {
                    Name = x.Product.Name,
                    Value = $"{x.Product.Value:N2}₽",
                    RealValue = x.Product.Value,
                    StockID = x.ID,
                }).ToList();

            response = response.Select(x =>
            {
                x.Qty = cartList.FirstOrDefault(y => y.StockID == x.StockID).Qty;
                return x;
            }).ToList();

            return response;
        }
    }
}
