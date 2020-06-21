using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Cart
{
    public class GetCart
    {
        private readonly ISession _session;
        private readonly ApplicationDBContext _ctx;

        public GetCart(ISession session, ApplicationDBContext ctx)
        {
            _session = session;
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
            var stringObject = _session.GetString("cart");

            if(string.IsNullOrEmpty(stringObject))
                return new List<Response>();

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

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
