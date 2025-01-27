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
    public record GetUserByIdQuery(long UserId):IRequest<OperationResult<User>>;
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, OperationResult<User>>
    { 
       private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
             var user=await _userRepository.GetUserById(request.UserId);
            if (user == null) return OperationResult<User>.Error("کاربر یافت نشد");
            return OperationResult<User>.Success(user);
        }
    }


}
