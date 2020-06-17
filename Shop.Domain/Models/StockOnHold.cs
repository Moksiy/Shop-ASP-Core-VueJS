using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class StockOnHold
    {
        public int ID { get; set; }

        public string SessionID { get; set; }

        public int StockID { get; set; }
        public Stock Stock { get; set; }

        public int Qty { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
