using Shop.Database;
using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.ProductsAdmin
{
    public class GetProduct
    {
        private readonly IProductManager _productManager;

        public GetProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public ProductViewModel Do(int id) =>
            _productManager.GetProductById(id, x => new ProductViewModel
            {
                ID = x.ID,
                Name = x.Name,
                Description = x.Description,
                Value = x.Value
            });

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
