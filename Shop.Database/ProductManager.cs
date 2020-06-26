using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDBContext _ctx;

        public ProductManager(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> Createproduct(Product product)
        {
            _ctx.Products.Add(product);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteProduct(int id)
        {
            var product = _ctx.Products.FirstOrDefault(x => x.ID == id);
            _ctx.Products.Remove(product);

            return _ctx.SaveChangesAsync();
        }

        public TResult GetProductById<TResult>(int id, Func<Product, TResult> selector)
        {
            return _ctx.Products
            .Where(x => x.ID == id)
            .Select(selector)
            .FirstOrDefault();
        }

        public TResult GetProductByName<TResult>(
            string name,
            Func<Product, TResult> selector)
        {
            return _ctx.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name)
            .Select(selector)
            .FirstOrDefault();
        }

        public IEnumerable<TResult> GetProductsWithStock<TResult>(
            Func<Product,
            TResult> selector)
        {
            return _ctx.Products
            .Include(x => x.Stock)
            .Select(selector)
            .ToList();
        }

        public Task<int> UpdateProduct(Product product)
        {
            _ctx.Products.Update(product);

            return _ctx.SaveChangesAsync();
        }
    }
}
