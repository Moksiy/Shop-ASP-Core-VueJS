using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    public class CreateStock
    {
        private ApplicationDBContext _ctx;

        public CreateStock(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new Stock
            {
                Description = request.Description,
                Qty = request.Qty,
                ProductID = request.ProductID
            };

            _ctx.Stock.Add(stock);

            await _ctx.SaveChangesAsync();

            return new Response
            { 
            ID = stock.ID,
            Description = stock.Description,
            Qty = stock.Qty
            };
        }

        public class Request
        {
            public string Description { get; set; }
            public int Qty { get; set; }
            public int ProductID { get; set; }
        }

        public class Response
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
