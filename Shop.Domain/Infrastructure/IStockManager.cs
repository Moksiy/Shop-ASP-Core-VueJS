using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IStockManager
    {
        Stock GetStockWithProduct(int stockID);
        bool EnoughStock(int stockId, int qty);
        Task PutStockOnHold(int stockID, int qty, string sessionID);
        Task RemoveStockFromHold(int stockID, int qty, string sessionID);
    }
}
