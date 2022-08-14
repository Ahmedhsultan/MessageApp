using System.ComponentModel.DataAnnotations;

namespace Booking.Model
{
    public class LoginDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
