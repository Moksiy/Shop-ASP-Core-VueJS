using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class OrderProduct
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int Qty { get; set; }
        public int StockID { get; set; }
        public Stock Stock { get; set; }
    }
}
