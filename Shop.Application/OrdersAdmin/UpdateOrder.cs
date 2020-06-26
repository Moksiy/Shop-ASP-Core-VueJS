
using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.OrdersAdmin
{
    public class UpdateOrder
    {
        private readonly IOrderManager _orderManager;

        public UpdateOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public Task<int> DoAsync(int id)
        {
            return _orderManager.AdvanceOrder(id);
        }
    }
}
