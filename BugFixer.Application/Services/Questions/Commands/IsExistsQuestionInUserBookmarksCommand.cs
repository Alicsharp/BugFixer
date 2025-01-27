using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Account;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record IsExistsQuestionInUserBookmarksCommand(long questionId, long userId):IRequest<bool>;
    public class IsExistsQuestionInUserBookmarksCommandHandler : IRequestHandler<IsExistsQuestionInUserBookmarksCommand, bool>
    {
        private readonly IQuestionRepository _questionRepository;

        public IsExistsQuestionInUserBookmarksCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<bool> Handle(IsExistsQuestionInUserBookmarksCommand request, CancellationToken cancellationToken)
        {
            return await _questionRepository.IsExistsQuestionInUserBookmarks(request.questionId, request.userId);
            
        }
    }
}
