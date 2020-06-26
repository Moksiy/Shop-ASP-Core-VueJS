using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    public class CreateStock
    {
        private readonly IStockManager _stockManager;

        public CreateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new Stock
            {
                Description = request.Description,
                Qty = request.Qty,
                ProductID = request.ProductID
            };

            await _stockManager.CreateStock(stock);            

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
