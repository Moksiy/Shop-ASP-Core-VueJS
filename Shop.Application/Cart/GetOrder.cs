using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Cart
{
    public class GetOrder
    {
        private readonly ISession _session;
        private readonly ApplicationDBContext _ctx;

        public GetOrder(ISession session, ApplicationDBContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Response
        {
            public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }
            public int GetTotalCharge() => Products.Sum(x => x.Value * x.Qty);
        }

        public class CustomerInformation
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
        }

        public class Product
        {
            public int ProductID { get; set; }
            public int Qty { get; set; }
            public int StockID { get; set; }
            public int Value { get; set; }
        }

        public Response Do()
        {
            var cart = _session.GetString("cart");

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(cart);

            var listOfProducts = _ctx.Stock
                .Include(x => x.Product)
                .Where(x => cartList.Any(y => y.StockID == x.ID))
                .Select(x => new Product
                {
                    ProductID = x.ProductID,
                    StockID = x.ID,
                    Value = (int)(x.Product.Value * 100),
                    Qty = cartList.FirstOrDefault(y => y.StockID == x.ID).Qty
                }).ToList();

            var customerInfoString = _session.GetString("customer-info");

            var customerInformation = JsonConvert.DeserializeObject<Shop.Domain.Models.CustomerInformation>(customerInfoString);

            return new Response
            {
                Products = listOfProducts,
                CustomerInformation = new CustomerInformation
                {
                    FirstName = customerInformation.FirstName,
                    LastName = customerInformation.LastName,
                    Email = customerInformation.Email,
                    PhoneNumber = customerInformation.PhoneNumber,
                    Address1 = customerInformation.Address1,
                    Address2 = customerInformation.Address2,
                    City = customerInformation.City,
                    PostCode = customerInformation.PostCode
                }
            };
        }
    }
}
