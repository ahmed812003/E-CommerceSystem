using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required , MaxLength(50)]
        public string Name { get; set; }

        [Required , MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public virtual ICollection<Cart_Product> Cart_Products { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Product_Category> Product_Categories { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Order_Product> Order_Products { get; set; }

        public virtual ICollection<Order> Orders { get; set; }


    }
}
