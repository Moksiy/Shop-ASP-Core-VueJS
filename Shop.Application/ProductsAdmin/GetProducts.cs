﻿using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.ProductsAdmin
{
    public class GetProducts
    {
        private readonly ApplicationDBContext _ctx;

        public GetProducts(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do() => _ctx.Products.ToList().Select(x => new ProductViewModel
        {
            ID = x.ID,
            Name = x.Name,
            Value = x.Value
        });

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
        }
    }
}
