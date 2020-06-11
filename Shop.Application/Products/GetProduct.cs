using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private readonly ApplicationDBContext _ctx;

        public GetProduct(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }
         
        public ProductViewModel Do(string name) =>
            _ctx.Products
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
                    InStock = y.Qty > 0
                })
            }).FirstOrDefault();

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
            public bool InStock { get; set; }
        }
    }
}
