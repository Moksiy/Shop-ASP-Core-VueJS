using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private readonly IStockManager _stockManager;
        private readonly IProductManager _productManager;

        public GetProduct(IStockManager stockManager, IProductManager productManager)
        {
            _stockManager = stockManager;
            _productManager = productManager;
        }

        public async Task<ProductViewModel> Do(string name)
        {
            await _stockManager.RetriveExpiredStockOnHold();

            return _productManager.GetProductByName(name, x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = x.Value.GetValueString(),

                Stock = x.Stock.Select(y => new StockViewModel
                {
                    ID = y.ID,
                    Description = y.Description,
                    Qty = y.Qty
                })
            });
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
