using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Account.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record EditAnswerCommand(EditAnswerDto EditAnswerDto):IRequest<bool>;
    public class EditAnswerCommandHandler : IRequestHandler<EditAnswerCommand, bool>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;

        public EditAnswerCommandHandler(IQuestionRepository questionRepository, IMediator mediator)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(EditAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer= await _questionRepository.GetAnswerById(request.EditAnswerDto.AnswerId);    
            if(answer == null)  return false;
            var user= await _mediator.Send(new GetUserByIdQuery(request.EditAnswerDto.UserId));
            if(user == null) return false;
            if(answer.UserId != user.Data.Id && !user.Data.IsAdmin) return false;
            answer.Content = request.EditAnswerDto.Content;
            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();
            return true;    
        }
    }
}
