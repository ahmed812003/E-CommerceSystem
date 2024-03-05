using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.DtoModels.Display
{
    public class DisplayOrder
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public decimal TotalPrice { get; set; }

        public bool Done { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime DeliveredOn { get; set; } = (DateTime.Now.AddDays(10));

        public string UserId { get; set; }
    }
}
