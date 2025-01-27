using AutoMapper;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record FilterEditAnswerQuery(long answerId, long userId) : IRequest<OperationResult  <EditAnswerDto>>;
    public class FilterEditAnswerCommandQueryHandler : IRequestHandler<FilterEditAnswerQuery, OperationResult<EditAnswerDto>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public FilterEditAnswerCommandQueryHandler(IQuestionRepository questionRepository, IMediator mediator,IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<OperationResult<EditAnswerDto>> Handle(FilterEditAnswerQuery request, CancellationToken cancellationToken)
        {
            var answer = await _questionRepository.GetAnswerById(request.answerId);
            if (answer == null) return null;
            var user = await _mediator.Send(new GetUserByIdQuery(request.userId));
            if (user == null) return null;
            if (answer.UserId != user.Data.Id && !user.Data.IsAdmin)
                return null;
            var result= _mapper.Map<EditAnswerDto>(answer);
            return OperationResult<EditAnswerDto>.Success(result);
                
        }
    }
}
