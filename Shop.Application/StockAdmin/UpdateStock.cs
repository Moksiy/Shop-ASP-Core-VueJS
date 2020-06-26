using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    [Service]
    public class UpdateStock
    {
        private readonly IStockManager _stockManager;

        public UpdateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<Response> Do(Request request)
        {
            var stockList = new List<Stock>();

            foreach (var stock in request.Stock)
            {
                stockList.Add(new Stock
                {
                    ID = stock.ID,
                    Description = stock.Description,
                    Qty = stock.Qty,
                    ProductID = stock.ProductID
                });
            }

            await _stockManager.UpdateStockRange(stockList);

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
