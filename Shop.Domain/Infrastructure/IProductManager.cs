using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IProductManager
    {
        Task<int> Createproduct(Product product);
        Task<int> DeleteProduct(int id);
        Task<int> UpdateProduct(Product product);
        IEnumerable<TResult> GetProductsWithStock<TResult>(Func<Product, TResult> selector);
        TResult GetProductByName<TResult>(string name, Func<Product, TResult> selector);
        TResult GetProductById<TResult>(int id, Func<Product, TResult> selector);
    }
}
