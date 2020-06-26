using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class DeleteProduct
    {
        private readonly IProductManager _productManager;

        public DeleteProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public Task<int> Do(int id)
        {
            return _productManager.DeleteProduct(id);
        }
    }
}
