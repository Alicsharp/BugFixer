using AutoMapper;
using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{
    public record UpdateUserCommand(long UserId, EditUserDto EditUserDto):IRequest<OperationResult<EditUserDto>>;
    public class UpdateUsrCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<EditUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUsrCommandHandler(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<EditUserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null) 
                return OperationResult<EditUserDto>.Error();
            
            user.UserEdit(request.EditUserDto.FirstName, request.EditUserDto.LastName,request.EditUserDto.PhoneNumber,request.EditUserDto.Description,request.EditUserDto.BirthDate,request.EditUserDto.CountryId,request.EditUserDto.CityId, request.EditUserDto.GetNewsLetter);
            await _userRepository.Save();
        var usermap=   _mapper.Map(user, request.EditUserDto);
            return OperationResult<EditUserDto>.Success(usermap);
        }
    }

}
