using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class DeleteProduct
    {
        private ApplicationDBContext _context;

        public DeleteProduct(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Do(int id)
        {
            var Product = _context.Products.FirstOrDefault(x => x.ID == id);
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
