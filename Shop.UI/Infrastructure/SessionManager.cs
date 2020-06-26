using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shop.UI.Infrastructure
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _session;

        private const string KeyCart = "cart";
        private const string KeyCustomerinfo = "customer-info";

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void AddCustomerInformation(CustomerInformation customer)
        {
            var stringObject = JsonConvert.SerializeObject(customer);

            _session.SetString(KeyCustomerinfo, stringObject);
        }

        public void AddProduct(CartProduct cartProduct)
        {
            var cartList = new List<CartProduct>();

            var stringObject = _session.GetString(KeyCart);

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if (cartList.Any(x => x.StockID == cartProduct.StockID))
            {
                cartList.Find(x => x.StockID == cartProduct.StockID).Qty += cartProduct.Qty;
            }
            else
                cartList.Add(cartProduct);

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString(KeyCart, stringObject);
        }

        public void ClearCart()
        {
            _session.Remove(KeyCart);
        }

        public IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector)
        {
            var stringObject = _session.GetString(KeyCart);

            if (string.IsNullOrEmpty(stringObject))
                return new List<TResult>();

            var cartList = JsonConvert.DeserializeObject<IEnumerable<CartProduct>>(stringObject);

            return cartList.Select(selector);
        }

        public CustomerInformation GetCustomerInformation()
        {
            var stringObject = _session.GetString(KeyCustomerinfo);

            if (string.IsNullOrEmpty(stringObject))
                return null;

            var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);

            return customerInformation;
        }

        public string GetId()
            => _session.Id;

        public void RemoveProduct(int stockID, int qty)
        {
            var cartList = new List<CartProduct>();

            var stringObject = _session.GetString(KeyCart);

            if (string.IsNullOrEmpty(stringObject))
                return;

            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (!cartList.Any(x => x.StockID == stockID))
                return;

            var product = cartList.FirstOrDefault(x => x.StockID == stockID);
            product.Qty -= qty;

            if (product.Qty <= 0)
            {
                cartList.Remove(product);
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString(KeyCart, stringObject);
        }
    }
}
