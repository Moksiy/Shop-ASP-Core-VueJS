﻿using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.StockAdmin
{
    public class GetStock
    {
        private ApplicationDBContext _ctx;

        public GetStock(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            var stock = _ctx.Products
                .Include(x=> x.Stock)
                .Select(x => new ProductViewModel
                {
                    ID = x.ID,
                    Description = x.Description,
                    Stock = x.Stock.Select(y =>
                        new StockViewModel
                        {
                            ID = y.ID,
                            Description = y.Description,
                            Qty = y.Qty
                        })
                }).ToList();

            return stock;
        }

        public class StockViewModel
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }

        public class ProductViewModel
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
    }
}
