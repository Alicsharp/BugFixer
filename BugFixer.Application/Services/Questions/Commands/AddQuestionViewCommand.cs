using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record AddQuestionViewCommand(string userId,long questionId):IRequest<Unit>;
    public class AddQuestionViewCommandHandler : IRequestHandler<AddQuestionViewCommand, Unit>
    {
        private readonly IQuestionRepository _questionRepository;

        public AddQuestionViewCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Unit> Handle(AddQuestionViewCommand request, CancellationToken cancellationToken)
        {
            if (await _questionRepository.IsExistsViewForQuestion(request.userId, request.questionId)) return Unit.Value;
            var view = new QuestionView()
            {
                QuestionId = request.questionId,
                UserId = request.userId
            };
            await _questionRepository.AddQuestionView(view);
            await _questionRepository.SaveChanges();
            return Unit.Value;
        }
    }
}
