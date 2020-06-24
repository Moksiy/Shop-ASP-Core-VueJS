using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class CartProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Value { get; set; }
        public int StockID { get; set; }
        public int Qty { get; set; }
    }
}
