using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.Models
{
    public class Cart_Product
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Cart Cart { get; set; }  


    }
}
