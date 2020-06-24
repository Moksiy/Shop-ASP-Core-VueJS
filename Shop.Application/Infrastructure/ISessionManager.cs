using Shop.Domain.Models;
using System.Collections.Generic;

namespace Shop.Application.Infrastructure
{
    public interface ISessionManager
    {
        string GetId();
        void AddProduct(int stockID, int qty);
        void RemoveProduct(int stockID, int qty);
        List<CartProduct> GetCart();

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
