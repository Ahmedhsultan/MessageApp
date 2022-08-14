using Database.Model;

namespace Booking.Model.Interface
{
    public interface ItokenService
    {
        string GetToken(Users user);
    }
}
