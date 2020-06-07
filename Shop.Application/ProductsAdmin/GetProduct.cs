using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.ProductsAdmin
{
    public class GetProduct
    {
        private ApplicationDBContext _ctx;

        public GetProduct(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public ProductViewModel Do(int id) => _ctx.Products.Where(x=>x.ID == id).Select(x => new ProductViewModel
        {
            ID = x.ID,
            Name = x.Name,
            Description = x.Description,
            Value = x.Value
        }).FirstOrDefault();

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
