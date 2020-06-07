using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.GetProducts
{
    public class GetProducts
    {
        private ApplicationDBContext _ctx;

        public GetProducts(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do() => _ctx.Products.ToList().Select(x => new ProductViewModel
        {
            Name = x.Name,
            Description = x.Description,
            Value = $"{x.Value:N2}₽"
        });

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
        }
    }
}
