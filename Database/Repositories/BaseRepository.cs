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

        public async Task<IEnumerable<Users>> findAll()
        {
            //return await _context.Users.Include(m => m.messages).ToListAsync();
            return null;
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
                //.Include(m => m.messages)
                return user;
            }
            return null;
        }

        public async Task<bool> ExistUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }

        public async Task<bool> ExistMessage(Guid id)
        {
            return await _context.Messages.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> AddNewMessage(Messages message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<string>> GetAllMessages(Users user)
        {
            var userMessages = await _context.Messages.Where(x => x.UserId == user.Id).ToListAsync();
            ICollection<string> Messages = new List<string>();
            foreach (var text in userMessages)
            {
                Messages.Add(text.Content);
            }
            return Messages;
        }

        public async Task<bool> EditMessage(Guid messageId, string newMessage)
        {
            if (await _context.Messages.AnyAsync(x => x.Id == messageId))
            {
                var message = await _context.Messages.FindAsync(messageId);
                message.Content = newMessage;
                return true;
            }
            return false;
        }

        public async Task<ICollection<string>> DeleteMessage(Guid messageId)
        {
            Messages message = await _context.Messages.FindAsync(messageId);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            Users user = await _context.Users.FindAsync(message.UserId);
            return await GetAllMessages(user);
        }
    }
}
