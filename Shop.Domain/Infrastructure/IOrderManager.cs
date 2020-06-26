using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shop.Domain.Enums;
using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure
{
    public interface IOrderManager
    {
        bool OrderReferenceExists(string reference);
        Task<int> AdvanceOrder(int id);
        IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status,Func<Order, TResult> selector);
        TResult GetOrderByReference<TResult>(string reference, Expression<Func<Order, TResult>> selector);
        TResult GetOrderById<TResult>(int id, Expression<Func<Order, TResult>> selector);
        Task<int> CreateOrder(Order order);
    }
}
