using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Ctor

        private readonly BugFixerDbContext _context;

        public UserRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        #endregion

        public   bool IsExistsUserByEmail(string email)
        {
            return   _context.Users.Any(s => s.Email == email);
        }

        public async Task CreateUser(User user)
        {
            await _context.AddAsync(user);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistsUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.AnyAsync(s => s.PhoneNumber == phoneNumber);

        }
        public async Task<User> GetUserByEmail(string email)
            {
                return await _context.Users.FirstOrDefaultAsync(s => s.Email.Equals(email));
            }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            return await _context.Users.Where(s=>s.EmailActivationCode==activationCode).FirstOrDefaultAsync( );
        }

        public async Task<User> GetUserById(long UserId)
        {
            return await _context.Users.FirstOrDefaultAsync(s=>!s.IsDeleted && s.Id==UserId);

        }
    }
}
