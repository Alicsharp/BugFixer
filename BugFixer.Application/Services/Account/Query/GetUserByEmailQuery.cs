using AutoMapper;
using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
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
    public record GetUserByEmailQuery(string email,string password):IRequest<OperationResult<LoginDto>>;
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, OperationResult<LoginDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByEmailQueryHandler(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<LoginDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.email);
            if (user == null) return OperationResult<LoginDto>.NotFound();
            var hashedPassword =  request.password ;
            //var hashedPassword = PasswordHelper.EncodePasswordMd5(request.password.SanitizeText());
            if (user.Password != hashedPassword) return OperationResult<LoginDto>.Error();
          var result=   _mapper.Map<LoginDto>(user);
            return OperationResult<LoginDto>.Success(result);
        }
    }


}
