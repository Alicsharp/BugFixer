using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record AnswerQuestionCommand(AnswerQuestionDto answerQuestionDto):IRequest<bool>;
    public class AnswerQuestionCommandHandler : IRequestHandler<AnswerQuestionCommand, bool>
    {
        private readonly IQuestionRepository _questionRepository;
        //private readonly IMediator _mediator;
        //public AnswerQuestionCommandHandler(IQuestionRepository questionRepository, IMediator mediator)
        //{
        //    _questionRepository = questionRepository;
        //    _mediator = mediator;
        //}
        public AnswerQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;   
        }

        public async Task<bool> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
        {
            var question=await _questionRepository.GetById(request.answerQuestionDto.QuestionId);   
             if(question == null) return false;
            var answer = new Answer()
            {
                Content = request.answerQuestionDto.Answer.SanitizeText(),
                QuestionId = request.answerQuestionDto.QuestionId,
                UserId = request.answerQuestionDto.UserId,
            };
            await _questionRepository.AddAnswer(answer);
            question.ViewCount += 1;

            await _questionRepository.UpdateQuestion(question);
        
            await _questionRepository.SaveChanges();

            return true;
        }
    }
}
