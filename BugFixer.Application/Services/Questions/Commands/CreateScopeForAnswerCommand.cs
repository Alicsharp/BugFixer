using BugFixer.Application.Contract.DTOS.Common;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Domain.Entities.Questions;
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
    public record CreateScopeForAnswerCommand(long answerId, AnswerScoreType Type, long userId) : IRequest<CreateScoreForAnswerResult>;
    public class CreateScopeForAnswerCommandHandler : IRequestHandler<CreateScopeForAnswerCommand, CreateScoreForAnswerResult>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        private ScoreManagementDto _scoreManagement;
        public CreateScopeForAnswerCommandHandler(IQuestionRepository questionRepository, IMediator mediator, IOptions<ScoreManagementDto> scoreManagement)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
            _scoreManagement = scoreManagement.Value;
        }

        public async Task<CreateScoreForAnswerResult> Handle(CreateScopeForAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await _questionRepository.GetAnswerById(request.answerId);
            if (answer == null) { return CreateScoreForAnswerResult.Error; }
            var user = await _mediator.Send(new GetUserByIdQuery(request.userId));
            if (user == null) { return CreateScoreForAnswerResult.Error; }
            if (request.Type == AnswerScoreType.Minus && user.Data.Score < _scoreManagement.MinScoreForDownScoreAnswer)
            {
                return CreateScoreForAnswerResult.NotEnoughScoreForDown;
            }
            if (request.Type == AnswerScoreType.Plus && user.Data.Score < _scoreManagement.MinScoreForDownScoreAnswer)
            {
                return CreateScoreForAnswerResult.NotEnoughScoreForUp;
            }
            if (await _questionRepository.IsExistsUserScoreForAnswer(request.answerId, request.userId))
            {
                return CreateScoreForAnswerResult.UserCreateScoreBefore;
            }
            var score = new AnswerUserScore
            {
                AnswerId = request.answerId,
                UserId = request.userId,
                Type = request.Type,
            };


            await _questionRepository.AddAnswerUserScore(score);
            if (request.Type == AnswerScoreType.Minus)
            {
                answer.Score -= 1;
            }
            else
            {
                answer.Score += 1;
            }
            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();
            return CreateScoreForAnswerResult.Success;
        }
    }


}
