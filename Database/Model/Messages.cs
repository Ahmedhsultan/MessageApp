using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Model
{
    public class Messages
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(150)]
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public Users User { get; set; }
    }
}
