using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shop.Domain.Models;
using System.Linq;
using Xunit;
using System.Collections;
using System;

namespace Shop.Tests
{
    public class ApplicationDbContextTests
    {
        public AppDbContext CreateDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void SavesProductToDatabase()
        {
            //Arrange
            var product = new Product
            {
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            //Act
            using (var ctx = CreateDbContext(nameof(SavesProductToDatabase)))
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }

            //Assert
            using (var ctx = CreateDbContext(nameof(SavesProductToDatabase)))
            {
                var savedProduct = ctx.Products.Single();
                Assert.Equal(product.Name, savedProduct.Name);
            }
        }

        [Fact]
        public void SavesStockToDatabase()
        {
            //Arrange
            var product = new Product
            {
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var stock = new Stock
            {
                Description = "Description",
                Product = product,
                ProductID = 1,
                OrderStocks = null,
                Qty = 1
            };

            //Act
            using (var ctx = CreateDbContext(nameof(SavesStockToDatabase)))
            {
                ctx.Stock.Add(stock);
                ctx.SaveChanges();
            }

            //Assert
            using (var ctx = CreateDbContext(nameof(SavesStockToDatabase)))
            {
                var savedStock = ctx.Stock.Single();
                Assert.Equal(stock.Description, savedStock.Description);
            }
        }

        [Fact]
        public void SavesOrderToDatabase()
        {
            //Arrange
            var product = new Product
            {
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var order = new Order
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Address1 = "Address1",
                Address2 = "Address2",
                City = "City",
                Email = "email@gmail.com",
                OrderRef = "REF",
                OrderStocks = null,
                Status = 0,
                PhoneNumber = "8-800-555-35-35",
                PostCode = "123",
                StripeReference = ""
            };           

            //Act
            using (var ctx = CreateDbContext(nameof(SavesOrderToDatabase)))
            {
                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }

            //Assert
            using (var ctx = CreateDbContext(nameof(SavesOrderToDatabase)))
            {
                var savedOrder = ctx.Orders.Single();
                Assert.Equal(order.OrderRef, savedOrder.OrderRef);
            }
        }

        [Fact]
        public void SavesOrderStockToDatabase()
        {
            //Arrange
            var product = new Product
            {
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var order = new Order
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Address1 = "Address1",
                Address2 = "Address2",
                City = "City",
                Email = "email@gmail.com",
                OrderRef = "REF",
                OrderStocks = null,
                Status = 0,
                PhoneNumber = "8-800-555-35-35",
                PostCode = "123",
                StripeReference = ""
            };

            var stock = new Stock
            {
                Description = "Description",
                Product = product,
                ProductID = 1,
                OrderStocks = null,
                Qty = 1
            };

            var orderStock = new OrderStock
            {
                Order = order,
                OrderID = 1,
                Qty = 1,
                Stock = stock,
                StockID = 1
            };

            //Act
            using (var ctx = CreateDbContext(nameof(SavesOrderStockToDatabase)))
            {
                ctx.OrderProducts.Add(orderStock);
                ctx.SaveChanges();
            }

            //Assert
            using (var ctx = CreateDbContext(nameof(SavesOrderStockToDatabase)))
            {
                var savedOrderStock = ctx.OrderProducts.Single();
                Assert.Equal(orderStock.OrderID, savedOrderStock.OrderID);
            }
        }


        [Fact]
        public void SavesStockOnHoldToDatabase()
        {
            //Arrange
            var product = new Product
            {
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var stock = new Stock
            {
                Description = "Description",
                Product = product,
                ProductID = 1,
                OrderStocks = null,
                Qty = 1
            };

            var stockOnHold = new StockOnHold
            {
                Stock = stock,
                StockID = 1,
                Qty = 1,
                SessionID = "123",
                ExpiryDate = DateTime.Now.AddDays(1)
            };

            //Act
            using (var ctx = CreateDbContext(nameof(SavesStockOnHoldToDatabase)))
            {
                ctx.StockOnHolds.Add(stockOnHold);
                ctx.SaveChanges();
            }

            //Assert
            using (var ctx = CreateDbContext(nameof(SavesStockOnHoldToDatabase)))
            {
                var savedStockOnHold = ctx.StockOnHolds.Single();
                Assert.Equal(stockOnHold.SessionID, savedStockOnHold.SessionID);
            }
        }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStock> OrderProducts { get; set; }
        public DbSet<StockOnHold> StockOnHolds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderStock>()
                .HasKey(x => new { x.StockID, x.OrderID });
        }
    }
}
