#region Library
using Database.Interfaces;
using Database.Model;
using MessageApp.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#endregion

namespace MessageApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : Controller
    {
        #region Dependency injection
        private readonly IRepository<Messages> _message_Irepository;
        private readonly IRepository<Users> _userIrepository;
        public MessageController(IRepository<Messages> _message_Irepository, IRepository<Users> _userIrepository)
        {
            this._message_Irepository = _message_Irepository;
            this._userIrepository = _userIrepository;
        }
        #endregion

        #region EndPoints
        [HttpPost("AddMessage")]
        public async Task<ActionResult<ICollection<string>>> AddMessage(MessageDTO MessageDTO)
        {
            //Check user is exist
            if (await _userIrepository.ExistUserName(MessageDTO.userName))
            {
                //Get user
                Users user = await _userIrepository.GetByUserName(MessageDTO.userName);
                //Create new message
                Messages message = new Messages()
                {
                    Content = MessageDTO.Message,
                    User = user,
                    Created = DateTime.Now
                };

                //Add new message to database
                await _message_Irepository.AddNewMessage(message);

                //Get all user message from database and return to client
                ICollection<string> userMessages = await _message_Irepository.GetAllMessages(user);

                return Ok(userMessages);
            }
            return BadRequest("user isnt exist");
        }

        [HttpPost("GetMessages")]
        public async Task<ActionResult<ICollection<string>>> AddMessage(string userName)
        {
            //Check user is exist
            if (await _userIrepository.ExistUserName(userName))
            {
                //Get user from database
                Users user = await _userIrepository.GetByUserName(userName);

                //Get all user message from database and return to client
                ICollection<string> userMessages = await _message_Irepository.GetAllMessages(user);

                return Ok(userMessages);
            }
            return BadRequest("User isnt exist");
        }

        [HttpDelete("DeleteMessage/{id}")]
        public async Task<ActionResult<ICollection<string>>> DeleteMessage(Guid id)
        {
            //Check if this id belong to message in database
            if (await _message_Irepository.ExistMessage(id))
            {
                //Delete this message
                return Ok(await _message_Irepository.DeleteMessage(id));
            }
            return BadRequest("Wrong id");
        }

        [HttpPost("EditMessage")]
        public async Task<ActionResult<ICollection<string>>> EditMessage(EditMessageDTO editMessageDTO)
        {
            //Check if user and message are exist
            if (await _userIrepository.ExistUserName(editMessageDTO.userName))
                if (await _message_Irepository.ExistMessage(editMessageDTO.messageId))
                {
                    //Get user from datebase
                    Users user = await _userIrepository.GetByUserName(editMessageDTO.userName);

                    //Edit message in database
                    await _message_Irepository.EditMessage(editMessageDTO.messageId, editMessageDTO.Message);

                    //Get all message which belong for this user in Databse
                    ICollection<string> userMessages = await _message_Irepository.GetAllMessages(user);

                    return Ok(userMessages);
                }
            return BadRequest();
        }
        #endregion
    }
}