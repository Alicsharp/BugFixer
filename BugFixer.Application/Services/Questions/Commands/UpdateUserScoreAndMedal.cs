using BugFixer.Application.Contract.DTOS.Common;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record UpdateUserScoreAndMedal(long userId , int score):IRequest<OperationResult>;
    public class UpdateUserScoreAndMedalHandler : IRequestHandler<UpdateUserScoreAndMedal, OperationResult>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly ScoreManagementDto _scoreManagementDto;

        public UpdateUserScoreAndMedalHandler(IMediator mediator , IUserRepository userRepository , IOptions<ScoreManagementDto> scoreManagementDto)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _scoreManagementDto = scoreManagementDto.Value;

        }

        public async Task<OperationResult> Handle(UpdateUserScoreAndMedal request, CancellationToken cancellationToken)
        {
           var user=await _mediator.Send(new GetUserByIdQuery(request.userId));
            if (user == null) return OperationResult.Error();
            user.Data.IncresaeScore(request.score);
          await  _userRepository.Save();
            if (request.score >= _scoreManagementDto.MinScoreForBronzeMedal && request.score < _scoreManagementDto.MinScoreForSilverMedal)
                if (user.Data.Medal != null && user.Data.Medal == Domain.Enums.UserMedal.Bronze)
                    return OperationResult.Error();

                else if (user.Data.Score >= _scoreManagementDto.MinScoreForSilverMedal && user.Data.Score < _scoreManagementDto.MinScoreForGoldMedal)
                {
                    if (user.Data.Medal != null && user.Data.Medal == UserMedal.Silver)
                    {
                        return OperationResult.Error();

                    }

                    user.Data.Medal = UserMedal.Silver;

                    //await _userRepository.UpdateUser(user);
                    await _userRepository.Save();
                }
                else if (user.Data.Score >= _scoreManagementDto.MinScoreForGoldMedal)
                {
                    if (user.Data.Medal != null && user.Data.Medal == UserMedal.Gold)
                    {
                        return OperationResult.Error();

                    }

                    user.Data.Medal = UserMedal.Gold;
 
                    await _userRepository.Save();
                }
            return OperationResult.Success();

        }
    }

}
