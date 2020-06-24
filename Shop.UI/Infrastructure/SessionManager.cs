using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Application.Infrastructure;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shop.UI.Infrastructure
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void AddCustomerInformation(CustomerInformation customer)
        {
            var stringObject = JsonConvert.SerializeObject(customer);

            _session.SetString("customer-info", stringObject);
        }

        public void AddProduct(int stockID, int qty)
        {
            var cartList = new List<CartProduct>();

            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if (cartList.Any(x => x.StockID == stockID))
            {
                cartList.Find(x => x.StockID == stockID).Qty += qty;
            }
            else
            {
                cartList.Add(new CartProduct
                {
                    StockID = stockID,
                    Qty = qty
                });
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }

        public List<CartProduct> GetCart()
        {
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return null;

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            return cartList;
        }

        public CustomerInformation GetCustomerInformation()
        {
            var stringObject = _session.GetString("customer-info");

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

            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject)) 
                return;

            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (!cartList.Any(x => x.StockID == stockID)) 
                return;

            var product = cartList.FirstOrDefault(x => x.StockID == stockID);
            product.Qty -= qty;

            if(product.Qty <= 0)
            {
                cartList.Remove(product);
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }
    }
}
