using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record CreateQuestionCommand(CreateQuestionDto createQuestionDto):IRequest<OperationResult>;

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, OperationResult>
    {
        private readonly IQuestionRepository _questionRepository;

        public CreateQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<OperationResult> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var qestion = new Question()
            {
                Content = request.createQuestionDto.Description.SanitizeText(),
                Title = request.createQuestionDto.Title.SanitizeText(),
                UserId = request.createQuestionDto.UserId,
            };
            await _questionRepository.AddQuestion(qestion);
            await _questionRepository.SaveChanges();
            if(request.createQuestionDto.SelectedTags !=null && request.createQuestionDto.SelectedTags.Any()) 
            {
                foreach(var questionSelectedTag in request.createQuestionDto.SelectedTags)
                {
                    var tag = await _questionRepository.GetTagName(questionSelectedTag.SanitizeText().Trim().ToLower());
                    var selected = new SelectQuestionTag()
                    {
                         QuestionId=qestion.Id,
                         TagId=tag.Id
                    };
                    await _questionRepository.AddSelectedQuestionTag(selected);
                } 
                await _questionRepository.SaveChanges();
            }
            return OperationResult.Success();
        }
    }
}
