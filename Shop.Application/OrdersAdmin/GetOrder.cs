using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.OrdersAdmin
{
    public class GetOrder
    {
        private readonly ApplicationDBContext _ctx;

        public GetOrder(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public class Response
        {
            public int ID { get; set; }
            public string OrderRef { get; set; }
            public string StripeReference { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }

            public ICollection<Product> Products { get; set; }
        }

        public class Product
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
            public string StockDescription { get; set; }
        }

        public Response Do(int id) =>
            _ctx.Orders
            .Where(x => x.ID == id)
            .Include(x => x.OrderStocks)
            .ThenInclude(x => x.Stock)
            .ThenInclude(x => x.Product)
            .Select(x => new Response
            {
                ID = x.ID,
                OrderRef = x.OrderRef,
                StripeReference = x.StripeReference,

                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                PostCode = x.PostCode,

                Products = (ICollection<Product>)x.OrderStocks.Select(y => new Product 
                {
                    Name = y.Stock.Product.Name,
                    Description = y.Stock.Product.Description,
                    Qty = y.Qty,
                    StockDescription = y.Stock.Description
                })
            }).FirstOrDefault();
    }
}
