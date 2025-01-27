using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Result;
using BugFixer.Application.Statics;
using MediatR;

namespace BugFixer.Application.Services.Account.Command
{
    public record ForgotPasswordCommand(string email) : IRequest<OperationResult>;
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;   
        public ForgotPasswordCommandHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;   
        }

        public async Task<OperationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var email=request.email.SanitizeText().Trim().ToLower();
            var user=await _userRepository.GetUserByEmail(email);
            if (user == null || user.IsDeleted) return OperationResult.Error("کاربر پیدا نشد");
            if (user.IsBan) return OperationResult.Error("کاربر  غیر فعال شده است  ");
            var body = $@"
                <div> برای فراموشی کلمه عبور روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Reset-Password/{user.EmailActivationCode}'>فراموشی کلمه عبور</a>
                ";

            await _mediator.Send(new SendEmailCommand(user.Email, "فراموشی کلمه عبور", body));
            return OperationResult.Success("عاملیت با موفیقت انجام شد");
        }
    }
}
    
 