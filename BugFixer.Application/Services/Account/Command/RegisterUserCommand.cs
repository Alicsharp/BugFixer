using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Result;
using BugFixer.Application.Statics;
using BugFixer.Domain.Entities.Account;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{
    public record RegisterUserCommand( string Email, string Password, string RePassword) : IRequest<OperationResult>;


    public record RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public RegisterUserCommandHandler(IUserRepository userRepository ,IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<OperationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
          

             if (request.Password != request.RePassword) { return OperationResult.Error("کلمه عبور یکسان نمی باشد"); }
              if (   _userRepository.IsExistsUserByEmail(request.Email.SanitizeText().Trim().ToLower()))
                return OperationResult.Error("ایمیل دیگیری  استفاده کنید ");
        
            var activationCode = Generators.CodeGenerator.CreateActivationCode();
            var newUser = User.RegisterUser( request.Email.SanitizeText().Trim().ToLower(), request.Password, activationCode );
            await _userRepository.CreateUser(newUser);
            await _userRepository.Save();
            var body = $@"
                <div> برای فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Activate-Email/{newUser.EmailActivationCode}'>فعالسازی حساب کاربری</a>
                ";
            await _mediator.Send(new SendEmailCommand(newUser.Email, "فعال سازی حساب کاربری", body));
            

            return OperationResult.Success("ثبت نام کاربر با موفقیت انجام شد");
        }
    }
}
