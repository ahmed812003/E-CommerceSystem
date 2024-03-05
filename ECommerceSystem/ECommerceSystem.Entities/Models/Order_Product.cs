using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.Models
{
    public class Order_Product
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Price { get; set; }
        
        public int Amount { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

    }
}
