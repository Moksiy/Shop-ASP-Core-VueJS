using Shop.Database;
using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.OrdersAdmin
{
    public class GetOrders
    {
        private readonly ApplicationDBContext _ctx;

        public GetOrders(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public class Response
        {
            public int ID { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }

        public IEnumerable<Response> Do(int status) =>
            _ctx.Orders
            .Where(x => x.Status == (OrderStatus)status)
            .Select(x => new Response
            {
                ID = x.ID,
                OrderRef = x.OrderRef,
                Email = x.Email
            })
            .ToList();
    }
}
