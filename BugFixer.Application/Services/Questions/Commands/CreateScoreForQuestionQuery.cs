using BugFixer.Application.Contract.DTOS.Common;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Domain.Entities.Account;
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
    public record CreateScoreForQuestionQuery(long questionId, QuestionScoreType type, long userId) :IRequest<CreateScoreForAnswerResult>;
    public class CreateScoreForQuestionQueryHandler : IRequestHandler<CreateScoreForQuestionQuery, CreateScoreForAnswerResult>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;   
        private readonly ScoreManagementDto _scoreManagementDto;
        public CreateScoreForQuestionQueryHandler(IQuestionRepository questionRepository,IMediator mediator ,IOptions<ScoreManagementDto> options)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;   
            _scoreManagementDto= options.Value; 
        }

        public async Task<CreateScoreForAnswerResult> Handle(CreateScoreForQuestionQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetById(request.questionId);

            if (question == null) return CreateScoreForAnswerResult.Error;
           
            var user = await _mediator.Send(new GetUserByIdQuery(request.userId));

            if (user == null) return CreateScoreForAnswerResult.Error;

            if (request.type == QuestionScoreType.Minus && user.Data.Score < _scoreManagementDto.MinScoreForDownScoreAnswer)
            {
                return CreateScoreForAnswerResult.NotEnoughScoreForDown;
            }

            if (request.type == QuestionScoreType.Plus && user.Data.Score < _scoreManagementDto.MinScoreForUpScoreAnswer)
            {
                return CreateScoreForAnswerResult.NotEnoughScoreForUp;
            }

            if (await _questionRepository.IsExistsUserScoreForQuestion(request.questionId, request.userId))
            {
                return CreateScoreForAnswerResult.UserCreateScoreBefore;
            }

            var score = new QuestionUserScore
            {
                QuestionId = request.questionId,
                UserId = request.userId,
                Type = request.type,
            };
            await _questionRepository.AddQuestionUserScore(score);

            if (request.type == QuestionScoreType.Minus)
            {
                question.Score -= 1;
            }
            else
            {
                question.Score += 1;
            }
            await _questionRepository.UpdateQuestion(question);

            await _questionRepository.SaveChanges();

            return CreateScoreForAnswerResult.Success;
        }
    }

}
