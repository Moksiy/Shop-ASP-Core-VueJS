using Shop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IStockManager
    {
        Task<int> CreateStock(Stock stock);
        Task<int> DeleteStock(int id);
        Task<int> UpdateStockRange(List<Stock> stockList);
        Stock GetStockWithProduct(int stockID);
        bool EnoughStock(int stockId, int qty);
        Task PutStockOnHold(int stockID, int qty, string sessionID);
        Task RetriveExpiredStockOnHold();
        Task RemoveStockFromHold(int stockID, int qty, string sessionID);
        Task RemoveStockFromHold(string sessionID);
    }
}
