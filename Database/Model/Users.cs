using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Model
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MinLength(5)]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSult { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Gender { get; set; }
    }
}
