using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.Models
{
    public class Order
    {
        public Order()
        {
            this.Done = DateTime.Now == this.CreatedOn.AddDays(11)? true : false;
        }

        [Key]
        public int Id { get; set; }

        [Required , MaxLength(11)]
        public string Phone { get; set; }

        public decimal TotalPrice { get; set; }

        public bool Done { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [NotMapped]
        public DateTime DeliveredOn { get; set; } = (DateTime.Now.AddDays(10));

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Order_Product> Order_Products { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
