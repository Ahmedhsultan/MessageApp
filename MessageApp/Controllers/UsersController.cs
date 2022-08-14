using Database.Interfaces;
using Database.Model;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IRepository<Users> _usersRepository;
        public UsersController(IRepository<Users> _usersRepository)
        {
            this._usersRepository = _usersRepository;
        }

        [HttpGet("GetUserId")]
        public async Task<ActionResult<Users>> GetUserId(Guid Id)
        {
            return await _usersRepository.GetById(Id);
        }

        [HttpGet("GetUserByUserName")]
        public async Task<ActionResult<Users>> GetUserByUserName(string userName)
        {
            return await _usersRepository.GetByUserName(userName);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _usersRepository.findAll();
        }
    }
}
