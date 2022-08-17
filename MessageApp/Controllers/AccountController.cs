#region libraries
using Booking.Model;
using Booking.Model.DTO;
using Booking.Model.Interface;
using Database.Interfaces;
using Database.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        #region Dependance Injection
        private IRepository<Users> _repository;

        public readonly ItokenService _Tokenservice;

        public AccountController(IRepository<Users> _repository, ItokenService _tokenservice)
        {
            this._repository = _repository;
            this._Tokenservice = _tokenservice;
        }
        #endregion

        #region Registration EndPoints
        [HttpPost("Register")]
        public async Task<ActionResult<UserClientDTO>> Register(RegisterDTO userDTO)
        {
            //Check if this userName is used from other user
            if (!await _repository.ExistUserName(userDTO.userName))
            {
                //Hashing the password befor saving in database
                using var hmac = new HMACSHA512();

                var user = new Users
                {
                    UserName = userDTO.userName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
                    PasswordSult = hmac.Key,
                    FullName = userDTO.fullName,
                    CreatedOn = DateTime.Now,
                    Gender = userDTO.Gender
                };
                //Add user to databse
                await _repository.addNewUser(user);

                return Ok(new UserClientDTO()
                {
                    userName = user.UserName,
                    token = _Tokenservice.GetToken(user)
                });
            }
            return BadRequest("UserName is taken");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserClientDTO>> Login(LoginDTO loginDTO)
        {
            //Check if username is right
            if (await _repository.ExistUserName(loginDTO.userName))
            {
                //Get user from databse
                Users user = await _repository.GetByUserName(loginDTO.userName);

                //Hash password which enterd by client
                using var hmac = new HMACSHA512(user.PasswordSult);
                byte[] loginPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

                //Compare between client password and the password wich exist in database
                for (int i = 0; i < loginPasswordHash.Length; i++)
                    if (loginPasswordHash[i] != user.PasswordHash[i])
                        return Unauthorized("Password is wrong");

                return Ok(new UserClientDTO()
                {
                    userName = user.UserName,
                    token = _Tokenservice.GetToken(user)
                });
            }
            return Unauthorized("Invalid Username");
        }
        #endregion
    }
}