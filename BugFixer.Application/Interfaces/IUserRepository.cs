using BugFixer.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Interfaces
{
    public interface IUserRepository
    {
        bool  IsExistsUserByEmail(string email);
        Task<bool> IsExistsUserByPhoneNumber(string phoneNumber);
        Task<User> GetUserByEmail(string email);
        Task CreateUser(User user);
        Task<User> GetUserByActivationCode(string activationCode);
        Task<User> GetUserById(long UserId);    
        Task Save();
    }


}
