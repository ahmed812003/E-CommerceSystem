using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.DtoModels.Update
{
    public class UpdateProduct
    {
        public int Id { get; set; }


        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; } = null;

        public List<string>? Categories { get; set; } = null;
    }
}
