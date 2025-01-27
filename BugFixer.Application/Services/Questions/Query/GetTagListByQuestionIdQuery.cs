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
    public record GetTagListByQuestionIdQuery(long questionId):IRequest<ICollection<string>>;
    public class GetTagListByQuestionIdQueryHandler : IRequestHandler<GetTagListByQuestionIdQuery, ICollection<string>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetTagListByQuestionIdQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<ICollection<string>> Handle(GetTagListByQuestionIdQuery request, CancellationToken cancellationToken)
        {
          return await _questionRepository.GetTagListByQuestionId(request.questionId);
    
        }
    }
}
