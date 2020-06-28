using Microsoft.EntityFrameworkCore;
using Shop.Application.Cart;
using Shop.Application.OrdersAdmin;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using Shop.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shop.Tests.Database
{
    public class OrderManagerTests
    {
        public Order order = new Order
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Address1 = "Address1",
            Address2 = "Address2",
            City = "City",
            Email = "email@gmail.com",
            OrderRef = "REF",
            Status = 0,
            PhoneNumber = "8-800-555-35-35",
            PostCode = "123",
            StripeReference = "",
            ID = 5
        };

        [Fact]
        public void CreateOrderTest()
        {
            //Arrange

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(CreateOrderTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);

                orderManager.CreateOrder(order);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(CreateOrderTest)))
            {
                var savedOrder = ctx.Orders
                    .Single();

                Assert.Equal(order.Status, savedOrder.Status);
            }
        }

        [Fact]
        public void AdvanceOrderTest()
        {
            //Arrange

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(AdvanceOrderTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                orderManager.CreateOrder(order);
                orderManager.AdvanceOrder(5);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(AdvanceOrderTest)))
            {
                var savedOrder = ctx.Orders
                    .Single();

                Assert.Equal(order.Status, savedOrder.Status);
            }
        }

        [Fact]
        public void OrderReferenceExistsTest()
        {
            //Arrange

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(OrderReferenceExistsTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                orderManager.CreateOrder(order);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(OrderReferenceExistsTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                bool result = orderManager.OrderReferenceExists("REF");

                Assert.True(result);
            }
        }

        [Fact]
        public void GetOrderByIdTest()
        {
            //Arrange

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetOrderByIdTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                orderManager.CreateOrder(order);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetOrderByIdTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                var result = orderManager.GetOrderById(5,
                x => new Application.OrdersAdmin.GetOrder.Response
                {
                    ID = x.ID
                });

                Assert.Equal(order.ID, result.ID);
            }
        }

        [Fact]
        public void GetOrderByReferenceTest()
        {
            //Arrange

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetOrderByReferenceTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                orderManager.CreateOrder(order);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetOrderByReferenceTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                var result = orderManager.GetOrderByReference("REF",
                x => new Application.OrdersAdmin.GetOrder.Response
                {
                    ID = x.ID,
                    OrderRef = x.OrderRef
                });

                Assert.Equal(order.OrderRef, result.OrderRef);
            }
        }

        [Fact]
        public void GetOrdersByStatusTest()
        {
            //Arrange

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetOrdersByStatusTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                orderManager.CreateOrder(order);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetOrdersByStatusTest)))
            {
                IOrderManager orderManager = new OrderManagerTest(ctx);
                var result = orderManager.GetOrdersByStatus(0,
                x => new Order
                {
                    ID = x.ID,
                    Status = x.Status
                });

                Assert.Single(result);
            }
        }

        public class OrderManagerTest : IOrderManager
        {
            private readonly AppDbContext _ctx;

            public OrderManagerTest(AppDbContext ctx)
            {
                _ctx = ctx;
            }

            public Task<int> AdvanceOrder(int id)
            {
                _ctx.Orders.FirstOrDefault(x => x.ID == id).Status++;

                return _ctx.SaveChangesAsync();
            }

            public Task<int> CreateOrder(Order order)
            {
                _ctx.Orders.Add(order);

                return _ctx.SaveChangesAsync();
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

            public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector)
            {
                return _ctx.Orders
                .Where(x => x.Status == status)
                .Select(selector)
                .ToList();
            }

            public bool OrderReferenceExists(string reference)
            {
                return _ctx.Orders.Any(x => x.OrderRef == reference);
            }
        }
    }
}
