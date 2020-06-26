using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using System.Collections.Generic;

namespace Shop.Application.OrdersAdmin
{
    [Service]
    public class GetOrders
    {
        private readonly IOrderManager _orderManager;

        public GetOrders(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public class Response
        {
            public int ID { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }

        public IEnumerable<Response> Do(int status) =>
            _orderManager.GetOrdersByStatus((OrderStatus)status,
                x => new Response
                {
                    ID = x.ID,
                    OrderRef = x.OrderRef,
                    Email = x.Email
                });
    }
}
