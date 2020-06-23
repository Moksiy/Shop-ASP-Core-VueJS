using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private readonly ApplicationDBContext _ctx;

        public GetProduct(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ProductViewModel> Do(string name)
        {
            var stockOnHold = _ctx.StockOnHolds.Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stockOnHold.Count > 0)
            {
                var stockToReturn = _ctx.Stock.Where(x => stockOnHold.Any(y => y.StockID == x.ID)).ToList();

                foreach (var stock in stockToReturn)
                    stock.Qty += stockOnHold.FirstOrDefault(x => x.StockID == stock.ID).Qty;

                _ctx.StockOnHolds.RemoveRange(stockOnHold);

                await _ctx.SaveChangesAsync();
            }

            return _ctx.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name)
            .Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = $"{x.Value:N2}₽",
                Stock = x.Stock.Select(y => new StockViewModel
                {
                    ID = y.ID,
                    Description = y.Description,
                    Qty = y.Qty
                })
            }).FirstOrDefault();
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
