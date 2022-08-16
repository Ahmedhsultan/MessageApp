using Database.Interfaces;
using Database.Model;
using MessageApp.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MessageApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IRepository<Messages> _message_Irepository;
        private readonly IRepository<Users> _userIrepository;
        public MessageController(IRepository<Messages> _message_Irepository, IRepository<Users> _userIrepository)
        {
            this._message_Irepository = _message_Irepository;
            this._userIrepository = _userIrepository;
        }

        [HttpPost("AddMessage")]
        public async Task<ActionResult<ICollection<string>>> AddMessage(MessageDTO MessageDTO)
        {
            if (await _userIrepository.ExistUserName(MessageDTO.userName))
            {
                Users user = await _userIrepository.GetByUserName(MessageDTO.userName);
                Messages message = new Messages()
                {
                    Content = MessageDTO.Message,
                    User = user,
                    Created = DateTime.Now
                };

                await _message_Irepository.AddNewMessage(message);

                ICollection<string> userMessages = await _message_Irepository.GetAllMessages(user);

                return Ok(userMessages);
            }
            return BadRequest();
        }

        [HttpPost("GetMessages")]
        public async Task<ActionResult<ICollection<string>>> AddMessage(string userName)
        {
            if (await _userIrepository.ExistUserName(userName))
            {
                Users user = await _userIrepository.GetByUserName(userName);

                ICollection<string> userMessages = await _message_Irepository.GetAllMessages(user);

                return Ok(userMessages);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteMessage/{id}")]
        public async Task<ActionResult<ICollection<string>>> DeleteMessage(Guid id)
        {
            if (await _message_Irepository.ExistMessage(id))
            {
                return Ok(await _message_Irepository.DeleteMessage(id));
            }
            return BadRequest();
        }

        [HttpPost("EditMessage")]
        public async Task<ActionResult<ICollection<string>>> EditMessage(EditMessageDTO editMessageDTO)
        {
            if (await _userIrepository.ExistUserName(editMessageDTO.userName))
                if (await _message_Irepository.ExistMessage(editMessageDTO.messageId))
                {
                    Users user = await _userIrepository.GetByUserName(editMessageDTO.userName);

                    await _message_Irepository.EditMessage(editMessageDTO.messageId, editMessageDTO.Message);

                    ICollection<string> userMessages = await _message_Irepository.GetAllMessages(user);

                    return Ok(userMessages);
                }
            return BadRequest();
        }
    }
}
