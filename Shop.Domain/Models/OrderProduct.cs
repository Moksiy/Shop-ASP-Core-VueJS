using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class OrderStock
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }        
        public int StockID { get; set; }
        public Stock Stock { get; set; }
        public int Qty { get; set; }
    }
}
