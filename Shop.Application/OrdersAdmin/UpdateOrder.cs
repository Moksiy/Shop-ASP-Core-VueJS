using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.OrdersAdmin
{
    public class UpdateOrder
    {
        private readonly ApplicationDBContext _ctx;

        public UpdateOrder(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> DoAsync(int id)
        {
            var order = _ctx.Orders.FirstOrDefault(x => x.ID == id);

            order.Status += 1;

            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
