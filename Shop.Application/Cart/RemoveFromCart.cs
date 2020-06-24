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
    public class RemoveFromCart
    {
        private readonly ISession _session;
        private readonly ApplicationDBContext _ctx;

        public RemoveFromCart(ISession session, ApplicationDBContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Request
        {
            public int StockID { get; set; }
            public int Qty { get; set; }
            public bool All { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var cartList = new List<CartProduct>();

            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
            {
                return true;                
            }

            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (!cartList.Any(x => x.StockID == request.StockID))
            {
                return true;                
            }

            cartList.Find(x => x.StockID == request.StockID).Qty -= request.Qty;

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);

            var stockOnHold = _ctx.StockOnHolds
                .FirstOrDefault(x => x.StockID == request.StockID
                && x.SessionID == _session.Id);

            var stock = _ctx.Stock.FirstOrDefault(x => x.ID == request.StockID);

            if(request.All)
            {
                stock.Qty += stockOnHold.Qty;
                stockOnHold.Qty = 0;
            }
            else
            {
                stock.Qty += request.Qty;
                stockOnHold.Qty -= request.Qty;
            }

            if(stockOnHold.Qty <= 0)
            {
                _ctx.Remove(stockOnHold);
            }

            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}
