using System.Linq;
using Shop.Database;
using System.Threading.Tasks;
using Shop.Application.Infrastructure;

namespace Shop.Application.Cart
{
    public class RemoveFromCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly ApplicationDBContext _ctx;

        public RemoveFromCart(ISessionManager sessionManager, ApplicationDBContext ctx)
        {
            _sessionManager = sessionManager;
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
            var stockOnHold = _ctx.StockOnHolds
                .FirstOrDefault(x => x.StockID == request.StockID
                && x.SessionID == _sessionManager.GetId());

            var stock = _ctx.Stock.FirstOrDefault(x => x.ID == request.StockID);

            if(request.All)
            {
                stock.Qty += stockOnHold.Qty;
                _sessionManager.RemoveProduct(request.StockID, stockOnHold.Qty);
                stockOnHold.Qty = 0;
            }
            else
            {
                stock.Qty += request.Qty;
                stockOnHold.Qty -= request.Qty;
                _sessionManager.RemoveProduct(request.StockID, request.Qty);
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
