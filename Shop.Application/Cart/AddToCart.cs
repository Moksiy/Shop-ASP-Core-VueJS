using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Shop.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private readonly ISession _session;

        public AddToCart(ISession session)
        {
            _session = session;
        }

        public class Request
        {
            public int StockID { get; set; }
            public int Qty { get; set; }
        }

        public void Do(Request request)
        {
            var cartList = new List<CartProduct>();

            var stringObject = _session.GetString("cart");

            if(!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if (cartList.Any(x => x.StockID == request.StockID))
            {
                cartList.Find(x => x.StockID == request.StockID).Qty += request.Qty;
            }
            else
            {
                cartList.Add(new CartProduct
                {
                    StockID = request.StockID,
                    Qty = request.Qty
                });
            }

             stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart",stringObject);
        }
    }
}
