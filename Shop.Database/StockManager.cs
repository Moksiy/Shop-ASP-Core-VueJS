﻿using System;
using Shop.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Shop.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart
{
    public class StockManager : IStockManager
    {
        private readonly ApplicationDBContext _ctx;

        public StockManager(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        public bool EnoughStock(int stockId, int qty)
        {
            return _ctx.Stock.FirstOrDefault(x => x.ID == stockId).Qty >= qty;
        }

        public Stock GetStockWithProduct(int stockID)
        {
            return _ctx.Stock
            .Include(x => x.Product)
            .FirstOrDefault(x => x.ID == stockID);
        }

        public Task PutStockOnHold(int stockID, int qty, string sessionID)
        {
            _ctx.Stock.FirstOrDefault(x => x.ID == stockID).Qty -= qty;

            var stockOnHold = _ctx.StockOnHolds
                .Where(x => x.SessionID == sessionID)
                .ToList();

            if (stockOnHold.Any(x => x.StockID == stockID))
            {
                stockOnHold.Find(x => x.StockID == stockID).Qty += qty;
            }
            else
            {
                _ctx.StockOnHolds.Add(new StockOnHold
                {
                    StockID = stockID,
                    SessionID = sessionID,
                    Qty = qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }

            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            return _ctx.SaveChangesAsync();
        }

        public Task RemoveStockFromHold(int stockID, int qty, string sessionID)
        {
            var stockOnHold = _ctx.StockOnHolds
                 .FirstOrDefault(x => x.StockID == stockID
                 && x.SessionID == sessionID);

            var stock = _ctx.Stock.FirstOrDefault(x => x.ID == stockID);

            stock.Qty += qty;
            stockOnHold.Qty -= qty;

            if (stockOnHold.Qty <= 0)
            {
                _ctx.Remove(stockOnHold);
            }

            return _ctx.SaveChangesAsync();
        }
    }
}