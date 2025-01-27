using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Account;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Query
{
    public record GetUserByActivationCodeQuery(string activationCode):IRequest<OperationResult<User>>;
    public class GetUserByActivationCodeQueryHandler : IRequestHandler<GetUserByActivationCodeQuery, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByActivationCodeQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
       
        public async Task<OperationResult<User>> Handle(GetUserByActivationCodeQuery request, CancellationToken cancellationToken)
        {
            var User = await _userRepository.GetUserByActivationCode(request.activationCode);
            if (User == null) { OperationResult.Error(); }
            
            return  OperationResult<User>.Success(User);

        }
    }
}
