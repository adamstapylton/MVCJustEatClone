using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string Surname { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
