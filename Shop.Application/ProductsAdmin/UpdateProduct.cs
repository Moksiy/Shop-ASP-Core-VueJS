using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class UpdateProduct
    {
        private ApplicationDBContext _context;

        public UpdateProduct(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Response> Do(Request request)
        {
            await _context.SaveChangesAsync();
            return new Response();
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
