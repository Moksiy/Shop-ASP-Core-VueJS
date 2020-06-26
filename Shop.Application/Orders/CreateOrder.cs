using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Orders
{
    [Service]
    public class CreateOrder
    {
        private readonly IOrderManager _orderManager;
        private readonly IStockManager _stockManager;

        public CreateOrder(
            IOrderManager orderManager,
            IStockManager stockManager)
        {
            _orderManager = orderManager;
            _stockManager = stockManager;
        }

        public class Request
        {
            public string StripeReference { get; set; }
            public string SessionID { get; set; }
            public string OrderRef { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
            public List<Stock> Stocks { get; set; }
        }

        public class Stock
        {
            public int StockID { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var order = new Order
            {
                StripeReference = "",
                OrderRef = CreateOrderReference(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                PostCode = request.PostCode,

                OrderStocks = request.Stocks.Select(x => new OrderStock
                {
                    StockID = x.StockID,
                    Qty = x.Qty
                }).ToList()
            };

            var success = await _orderManager.CreateOrder(order) > 0;

            if (success)
            {
                await _stockManager.RemoveStockFromHold(request.SessionID);
               
                return true;
            }

            return false;
        }

        public string CreateOrderReference()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new char[12];
            var random = new Random();

            do
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = chars[random.Next(chars.Length)];
            } while (_orderManager.OrderReferenceExists(new string(result)));

            return new string(result);
        }
    }
}
