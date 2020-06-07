using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
