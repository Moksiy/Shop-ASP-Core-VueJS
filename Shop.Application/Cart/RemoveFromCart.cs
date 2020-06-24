using System.Linq;
using Shop.Database;
using System.Threading.Tasks;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart
{
    public class RemoveFromCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly IStockManager _stockManager;

        public RemoveFromCart(ISessionManager sessionManager, IStockManager stockManager)
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
            if (request.Qty <= 0)
                return false;

            await _stockManager.RemoveStockFromHold(request.StockID, request.Qty, _sessionManager.GetId());

            _sessionManager.RemoveProduct(request.StockID, request.Qty);

            return true;
        }
    }
}
