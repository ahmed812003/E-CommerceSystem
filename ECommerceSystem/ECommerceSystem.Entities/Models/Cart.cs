using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.Models
{
    public class Cart
    {
     
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Cart_Product> Cart_Products { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
