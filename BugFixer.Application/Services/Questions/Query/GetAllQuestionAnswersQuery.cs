using AutoMapper;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record GetAllQuestionAnswersQuery(long questionId) : IRequest< OperationResult<List<AnswerQuestionDto>>>;
    public class GetAllQuestionAnswersQueryHandler : IRequestHandler<GetAllQuestionAnswersQuery, OperationResult<List<AnswerQuestionDto>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public GetAllQuestionAnswersQueryHandler(IQuestionRepository questionRepository, IMapper mapper) 
        {
             _questionRepository = questionRepository;
            _mapper = mapper;
        }

   

        public async Task<OperationResult<List<AnswerQuestionDto>>> Handle(GetAllQuestionAnswersQuery request, CancellationToken cancellationToken)
        {
            var result= await   _questionRepository.GetAllQuestionAnswers(request.questionId);
            if (result == null) { return   OperationResult<List<AnswerQuestionDto>>.NotFound(); }
            var data = _mapper.Map<List<AnswerQuestionDto>>(result).ToList();
            return    OperationResult<List<AnswerQuestionDto>>.Success(data);
        }
    }
}
