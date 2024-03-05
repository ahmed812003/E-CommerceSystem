using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Entities.DtoModels.Create
{
    public class CreateUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set;}

        [Required, MaxLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(11)]
        public string Phone { get; set; }
    }
}
