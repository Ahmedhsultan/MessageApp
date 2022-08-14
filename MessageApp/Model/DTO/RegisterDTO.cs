using System.ComponentModel.DataAnnotations;

namespace Booking.Model
{
    public class RegisterDTO
    {
        [Required]
        public string fullName { get; set; }
        [Required,MinLength(5)]
        public string userName { get; set; }
        [Required,MinLength(5)]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
