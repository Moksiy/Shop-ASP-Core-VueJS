using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.StockAdmin
{
    [Service]
    public class GetStock
    {
        private readonly IProductManager _productManager;

        public GetStock(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            return _productManager.GetProductsWithStock(x => new ProductViewModel
            {
                ID = x.ID,
                Name = x.Name,
                Description = x.Description,
                Stock = x.Stock.Select(y =>
                    new StockViewModel
                    {
                        ID = y.ID,
                        Description = y.Description,
                        Qty = y.Qty
                    })
            });            
        }

        public class StockViewModel
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
    }
}
