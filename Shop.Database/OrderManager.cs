﻿using Microsoft.EntityFrameworkCore;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class OrderManager : IOrderManager
    {
        private readonly ApplicationDBContext _ctx;

        public OrderManager(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public bool OrderReferenceExists(string reference)
        {
            return _ctx.Orders.Any(x => x.OrderRef == reference);
        }
        public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector)
        {
            return _ctx.Orders
            .Where(x => x.Status == status)
            .Select(selector)
            .ToList();
        }

        private TResult GetOrder<TResult>(Expression<Func<Order, bool>> condition, Expression<Func<Order, TResult>> selector)
        {
            return _ctx.Orders
                .Where(condition)
                .Include(x => x.OrderStocks)
                    .ThenInclude(x => x.Stock)
                        .ThenInclude(x => x.Product)
                .Select(selector)
                .FirstOrDefault();
        }

        public TResult GetOrderById<TResult>(int id, Expression<Func<Order, TResult>> selector)
        {
            return GetOrder(order => order.ID == id, selector);
        }

        public TResult GetOrderByReference<TResult>(string reference, Expression<Func<Order, TResult>> selector)
        {
            return GetOrder(order => order.OrderRef == reference, selector);
        }

        public Task<int> CreateOrder(Order order)
        {
            _ctx.Orders.Add(order);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> AdvanceOrder(int id)
        {
            _ctx.Orders.FirstOrDefault(x => x.ID == id).Status++;

            return  _ctx.SaveChangesAsync();
        }
    }
}
