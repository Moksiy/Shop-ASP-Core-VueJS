using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Shop.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Shop.Database;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private readonly ISession _session;
        private readonly ApplicationDBContext _ctx;

        public AddToCart(ISession session, ApplicationDBContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Request
        {
            public int StockID { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var stockOnHold = _ctx.StockOnHolds.Where(x => x.SessionID == _session.Id).ToList();

            var stockToHold = _ctx.Stock.Where(x => x.ID == request.StockID).FirstOrDefault();

            if (stockToHold.Qty < request.Qty)
                return false;


            _ctx.StockOnHolds.Add(new StockOnHold
            {
                StockID = stockToHold.ID,
                SessionID = _session.Id,
                Qty = request.Qty,
                ExpiryDate = DateTime.Now.AddMinutes(20)
            });

            stockToHold.Qty -= request.Qty;

            foreach(var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            await _ctx.SaveChangesAsync();

            var cartList = new List<CartProduct>();

            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
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

            _session.SetString("cart", stringObject);

            return true;
        }
    }
}
