using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using Shop.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shop.Tests.Database
{
    public class ProductManagerTests
    {
        [Fact]
        public void CreatingProductTest()
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
            using (var ctx = DbContextfactory.CreateDbContext(nameof(CreatingProductTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);
                productManager.Createproduct(product);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(CreatingProductTest)))
            {
                var savedProduct = ctx.Products
                    .Single();

                Assert.Equal(product.Name, savedProduct.Name);
            }
        }

        [Fact]
        public void UpdatingProductTest()
        {
            //Arrange
            var product = new Product
            {
                ID = 10,
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(UpdatingProductTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);
                productManager.Createproduct(product);
                product.Name = "UpdatedName";
                productManager.UpdateProduct(product);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(UpdatingProductTest)))
            {
                var savedProduct = ctx.Products
                    .Single();

                Assert.Equal(product.Name, savedProduct.Name);
            }
        }

        [Fact]
        public void DelitingProductTest()
        {
            //Arrange
            var product = new Product
            {
                ID = 10,
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(DelitingProductTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);
                productManager.Createproduct(product);
                productManager.DeleteProduct(10);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(DelitingProductTest)))
            {
                var count = ctx.Products.Count();

                Assert.Equal(0, count);
            }
        }

        [Fact]
        public void GetProductByIdTest()
        {
            //Arrange
            var product = new Product
            {
                ID = 10,
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var product2 = new Product
            {
                ID = 5,
                Name = "ProductName2",
                Description = "ProductDescription2",
                Value = 100,
                Stock = null
            };

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetProductByIdTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);
                productManager.Createproduct(product);
                productManager.Createproduct(product2);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetProductByIdTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);

                ProductViewModel result = productManager.GetProductById(5, x => new ProductViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value
                });

                Assert.Equal(product2.Name, result.Name);
            }
        }


        [Fact]
        public void GetProductByNameTest()
        {
            //Arrange
            var product = new Product
            {
                ID = 10,
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var product2 = new Product
            {
                ID = 5,
                Name = "ProductName2",
                Description = "ProductDescription2",
                Value = 100,
                Stock = null
            };

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetProductByNameTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);
                productManager.Createproduct(product);
                productManager.Createproduct(product2);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetProductByNameTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);

                ProductViewModel result = productManager.GetProductByName("ProductName", x => new ProductViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value
                });

                Assert.Equal(product.ID, result.ID);
            }
        }

        [Fact]
        public void GetProductsWithStockTest()
        {
            //Arrange
            var product = new Product
            {
                ID = 10,
                Name = "ProductName",
                Description = "ProductDescription",
                Value = 100,
                Stock = null
            };

            var product2 = new Product
            {
                ID = 5,
                Name = "ProductName2",
                Description = "ProductDescription2",
                Value = 100,
                Stock = null
            };                       

            //Act
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetProductsWithStockTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);
                productManager.Createproduct(product);
                productManager.Createproduct(product2);
            }

            //Assert
            using (var ctx = DbContextfactory.CreateDbContext(nameof(GetProductsWithStockTest)))
            {
                IProductManager productManager = new ProductManagerTest(ctx);

                List<ProductViewModel> result = productManager.GetProductsWithStock(x => new ProductViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value
                }).ToList();

                Assert.Equal(2, result.Count);
            }
        }

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public class ProductManagerTest : IProductManager
        {
            private readonly AppDbContext _ctx;

            public ProductManagerTest(AppDbContext ctx)
            {
                _ctx = ctx;
            }

            public Task<int> Createproduct(Product product)
            {
                _ctx.Products.Add(product);

                return _ctx.SaveChangesAsync();
            }

            public Task<int> DeleteProduct(int id)
            {
                var product = _ctx.Products.FirstOrDefault(x => x.ID == id);
                _ctx.Products.Remove(product);

                return _ctx.SaveChangesAsync();
            }

            public TResult GetProductById<TResult>(int id, Func<Product, TResult> selector)
            {
                return _ctx.Products
                .Where(x => x.ID == id)
                .Select(selector)
                .FirstOrDefault();
            }

            public TResult GetProductByName<TResult>(string name, Func<Product, TResult> selector)
            {
                return _ctx.Products
                .Include(x => x.Stock)
                .Where(x => x.Name == name)
                .Select(selector)
                .FirstOrDefault();
            }

            public IEnumerable<TResult> GetProductsWithStock<TResult>(Func<Product, TResult> selector)
            {
                return _ctx.Products
                .Include(x => x.Stock)
                .Select(selector)
                .ToList();
            }

            public Task<int> UpdateProduct(Product product)
            {
                _ctx.Products.Update(product);

                return _ctx.SaveChangesAsync();
            }
        }
    }
}
