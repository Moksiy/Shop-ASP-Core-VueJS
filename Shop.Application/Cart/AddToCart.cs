using Shop.Domain.Models;
using System.Threading.Tasks;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart
{
    [Service]
    public class AddToCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly IStockManager _stockManager;

        public AddToCart(ISessionManager sessionManager, IStockManager stockManager)
        {
            _sessionManager = sessionManager;
            _stockManager = stockManager;
        }

        public class Request
        {
            public int StockID { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            if (!_stockManager.EnoughStock(request.StockID, request.Qty))
                return false;
            if (request.Qty <= 0)
                return false;

            await _stockManager
                .PutStockOnHold(request.StockID, request.Qty, _sessionManager.GetId());

            var stock = _stockManager.GetStockWithProduct(request.StockID);

            var cartProduct = new CartProduct() 
            {
                ProductID = stock.ProductID,
                StockID = stock.ID,
                Qty = request.Qty,
                ProductName = stock.Product.Name,
                Value = stock.Product.Value
            };

            _sessionManager.AddProduct(cartProduct);

            return true;
        }
    }
}
