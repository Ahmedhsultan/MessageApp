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
        Task<IEnumerable<T>> findAll();
        Task<Users> GetByUserName(string userName);
        Task<bool> addNewUser(Users user);
        Task<bool> ExistUserName(string userName);
    }
}
