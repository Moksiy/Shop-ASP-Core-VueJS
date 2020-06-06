using System;
using System.Collections.Generic;
using System.Text;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Products
{
    public class CreateProduct
    {
        private ApplicationDBContext _context;

        public CreateProduct(ApplicationDBContext context)
        {
            _context = context;
        }

        public void Do(int id, string name, string description, decimal value)
        {
            _context.Products.Add(new Product
            {
                ID = id,
                Name = name,
                Description = description,
                Value = value
            }); ;
        }
    }
}
