using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.Models
{
    public class Category
    {

        [Key]
        public int Id { get; set; }

        [Required , MaxLength(100)]
        public string Name { get; set; }

        [Required , MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<Product_Category> Product_Categories { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
