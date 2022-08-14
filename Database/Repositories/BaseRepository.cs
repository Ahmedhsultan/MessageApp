using Database.Interfaces;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private DBContext _context;
        public BaseRepository(DBContext _context)
        {
            this._context = _context;
        }

        public async Task<bool> addNewUser(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> findAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Users> GetByUserName(string userName)
        {
            if (await ExistUserName(userName))
            {
                Users user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName);
                return user;
            }
            return null;
        }

        public async Task<bool> ExistUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }
    }
}
