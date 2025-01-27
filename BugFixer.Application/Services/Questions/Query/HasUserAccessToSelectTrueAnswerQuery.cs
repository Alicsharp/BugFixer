using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Account;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record  HasUserAccessToSelectTrueAnswerQuery(long userId, long answerId):IRequest<bool>;
    public class HasUserAccessToSelectTrueAnswerQueryHandler : IRequestHandler<HasUserAccessToSelectTrueAnswerQuery, bool>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        public HasUserAccessToSelectTrueAnswerQueryHandler(IQuestionRepository questionRepository, IUserRepository userRepository)
        {
            _questionRepository = questionRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(HasUserAccessToSelectTrueAnswerQuery request, CancellationToken cancellationToken)
        {
            var answer = await _questionRepository.GetAnswerById(request.answerId);
            if (answer == null) return false;

            var user = await _userRepository.GetUserById(request.answerId);

            if (user == null) return false;
            if (user.IsAdmin) return true;

            if (answer.Question.UserId != request.userId)
            {
                return false;
            }

            return true;
        }
    }
}
