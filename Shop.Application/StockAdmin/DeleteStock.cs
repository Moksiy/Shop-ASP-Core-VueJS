using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    public class DeleteStock
    {
        private ApplicationDBContext _ctx;

        public DeleteStock(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Do(int id)
        {
            var stock = _ctx.Stock.FirstOrDefault(x => x.ID == id);

            _ctx.Stock.Remove(stock);

            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}
