using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Query
{
    public record CheckUserForLoginQuery(string Email, string Password) : IRequest<OperationResult>;
    public class CheckUserForLoginQueryHandler : IRequestHandler<CheckUserForLoginQuery, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public CheckUserForLoginQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(CheckUserForLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null || user.IsDeleted) return OperationResult.NotFound("کاربر پیدا نشد");
            var hashedPassword = request.Password;
            //var hashedPassword = PasswordHelper.EncodePasswordMd5(request.Password );
            if (user.Password != hashedPassword) return OperationResult.Error(" رمز عبور اشتباه است");


            if (user.IsBan) return OperationResult.Error(" کاربر بن شده است");
            if (!user.IsEmailConfirmed) return OperationResult.Error(" ایمیل تایید نشده سات");
            return OperationResult.Success();

        }
    }

}
