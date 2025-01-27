using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record GetAllQuestionsQuery:IRequest<IQueryable<Question>>;
    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IQueryable<Question>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetAllQuestionsQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<IQueryable<Question>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _questionRepository.GetAllQuestions();
        }
    }
}
