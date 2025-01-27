using BugFixer.Application.Generators;
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
    public  record ActiveteUserEmailCommand( string activationCode):IRequest<OperationResult>;
    public class ActiveteUserEmailCommandHandler : IRequestHandler<ActiveteUserEmailCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public ActiveteUserEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(ActiveteUserEmailCommand request, CancellationToken cancellationToken)
        {
            var User = await _userRepository.GetUserByActivationCode(request.activationCode);
            if (User == null) return OperationResult.Error("کاربر یافت نشد");
            if (User.IsBan || User.IsDeleted) return OperationResult.Error("کاربر غیرفعال شده است");
            User.IsEmailConfirmed = true;
            User.EmailActivationCode = CodeGenerator.CreateActivationCode();
         await  _userRepository.Save();
            return OperationResult.Success("عملیات به درستی انجام شد");
        }
    }


}
