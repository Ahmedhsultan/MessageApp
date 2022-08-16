using Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<IEnumerable<Users>> findAll();
        Task<Users> GetByUserName(string userName);
        Task<bool> addNewUser(Users user);
        Task<bool> ExistUserName(string userName);
        Task<bool> ExistMessage(Guid id);
        Task<bool> AddNewMessage(Messages message);
        Task<ICollection<string>> GetAllMessages(Users user);
        Task<bool> EditMessage (Guid messageId,string newMessage);
        Task<ICollection<string>> DeleteMessage (Guid messageId);
    }
}
