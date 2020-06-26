using Shop.Domain.Infrastructure;
using System.Collections.Generic;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class GetProducts
    {
        private readonly IProductManager _productManager;

        public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _productManager.GetProductsWithStock(x => new ProductViewModel
            {
                ID = x.ID,
                Name = x.Name,
                Value = x.Value
            }); 

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
        }
    }
}
