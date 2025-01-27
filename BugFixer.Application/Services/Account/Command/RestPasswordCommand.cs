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

namespace BugFixer.Application.Services.Account.Command
{
    public record RestPasswordCommand(string EmailActivationCode, string? Password,string? RePassword) : IRequest<OperationResult>;
    public class RestPasswordCommandHandler : IRequestHandler<RestPasswordCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public RestPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(RestPasswordCommand request, CancellationToken cancellationToken)
        {
           var user= await _userRepository.GetUserByActivationCode(request.EmailActivationCode.SanitizeText());
            if (user == null || user.IsDeleted) return OperationResult.Error("کاربر مورد نظر پیدا نشد");
            if (user.IsBan) return OperationResult.Error("کاربر مورد نظر غیر فعال  شد است ");
            var password = PasswordHelper.EncodePasswordMd5(request.Password.SanitizeText());
            user.Password = password;
            user.IsEmailConfirmed= true;
            await _userRepository.Save();
            return OperationResult.Success("عملیات با موفیقت انجام شد");
        }
    }


}
