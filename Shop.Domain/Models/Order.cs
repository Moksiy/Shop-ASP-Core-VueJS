using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Shop.Domain.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string OrderRef { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public ICollection<OrderProduct> Orderproducts { get; set; }
    }
}
