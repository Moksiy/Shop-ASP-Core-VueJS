using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Shop.Domain.Models;
using System;
using System.Linq;
using Xunit;

namespace Shop.Database.Tests
{
    public class DatabaseTests
    {
        [Fact]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(nameof(DatabaseTests))

            var product = new Product
            {
                Name = "Product1",
                Description = "Product1 Description",
                Value = 100
            };

            using (var ctx = new AppDbContext())
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            };

            using (var ctx = new AppDbContext())
            {
                var savedProduct = ctx.Products.Single();
                Assert.Equal(product.Name, savedProduct.Name);
            };
        }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
