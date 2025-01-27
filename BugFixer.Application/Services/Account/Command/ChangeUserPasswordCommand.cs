using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{
    public record ChangeUserPasswordCommand(long userId,ChangeUserPasswordDto ChangeUserPasswordDto):IRequest<OperationResult>;
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user=await _userRepository.GetUserById(request.userId); 
            if (user == null) { return OperationResult.Error("کاربری پیدا نشد"); }
            if(user.Password == request.ChangeUserPasswordDto.OldPassword)
            {
                user.ChangePassword(request.ChangeUserPasswordDto.Password);
            }
               await _userRepository.Save();
            return OperationResult.Success("عملیات تغییر رمز انجام شد");
        }
    }
}
