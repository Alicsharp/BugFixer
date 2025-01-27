using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugFixer.Application.Services.Result;

namespace BugFixer.Application.Services.Questions.Query
{
    public record GetQuestionByIdQuery(long  questionId):IRequest<OperationResult<Question>>;
    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, OperationResult<Question>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<OperationResult<Question>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _questionRepository.GetById(request.questionId);
            return OperationResult<Question>.Success(result);
        }
    }



}
