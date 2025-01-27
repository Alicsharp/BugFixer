using BugFixer.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record SelectTrueAnswerCommand(long userId, long answerId):IRequest<Unit>;
    public class SelectTrueAnswerHandler : IRequestHandler<SelectTrueAnswerCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public SelectTrueAnswerHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(SelectTrueAnswerCommand request, CancellationToken cancellationToken)
        {
             var answer= await _questionRepository.GetAnswerById(request.answerId);
            if (answer == null) return Unit.Value;

            answer.IsTrue = !answer.IsTrue;

            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();
            return Unit.Value;
        }
    }
}
