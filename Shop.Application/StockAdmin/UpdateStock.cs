using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    public class UpdateStock
    {
        private readonly ApplicationDBContext _ctx;

        public UpdateStock(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response> Do(Request request)
        {
            var stocks = new List<Stock>();

            foreach (var stock in request.Stock)
            {
                stocks.Add(new Stock
                {
                    ID = stock.ID,
                    Description = stock.Description,
                    Qty = stock.Qty,
                    ProductID = stock.ProductID
                });
            }

            _ctx.Stock.UpdateRange(stocks);

            await _ctx.SaveChangesAsync();

            return new Response
            {
                Stock = request.Stock
            };
        }

        public class StockViewModel
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
            public int ProductID { get; set; }
        }

        public class Request
        {
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class Response
        {
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

    }
}
