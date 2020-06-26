using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class UpdateProduct
    {
        private readonly IProductManager _productManager;

        public UpdateProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<Response> Do(Request request)
        {
            var product = _productManager.GetProductById(request.ID, x => x);

            product.Name = request.Name;
            product.Description = request.Description;
            product.Value = request.Value;

            await _productManager.UpdateProduct(product);

            return new Response
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value
            };
        }

        public class Request
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public class Response
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
