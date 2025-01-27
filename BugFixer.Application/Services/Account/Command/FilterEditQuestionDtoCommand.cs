using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Application.Services.Questions.Query;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{
    public record FilterEditQuestionDtoCommand(long questionId,long userId):IRequest<EditQuestionDto>;
    public class FilterEditQuestionDtoCommandHandler : IRequestHandler<FilterEditQuestionDtoCommand, EditQuestionDto>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;

        public FilterEditQuestionDtoCommandHandler(IQuestionRepository questionRepository, IMediator mediator)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
        }

        public async Task<EditQuestionDto> Handle(FilterEditQuestionDtoCommand request, CancellationToken cancellationToken)
        {
            var question= await _mediator.Send(new GetQuestionByIdQuery(request.questionId));
            if (question == null) return null;
            var user=await _mediator.Send(new GetUserByIdQuery(request.userId));
            if (user == null) return null;
            if (question.Data.UserId != user.Data.Id && !user.Data.IsAdmin) return null;
            var tags=  await   _mediator.Send(new GetTagListByQuestionIdQuery(request.questionId));
        
            await question.Data.EditQuestion(title: question.Title, description: question.Data.Content, selectedTags:tags);
           return new EditQuestionDto() { Title=question.Title, QuestionId=question.Data.Id,Description=question.Data.Content,SelectedTags=tags};
        }
    }
}
