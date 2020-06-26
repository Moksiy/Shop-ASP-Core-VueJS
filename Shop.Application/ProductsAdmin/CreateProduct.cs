using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class CreateProduct
    {
        private readonly IProductManager _productManager;

        public CreateProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<Response> Do(Request request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Value = request.Value
            };

            if (await _productManager.Createproduct(product) <= 0)
                throw new Exception("Failed to create product");

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
