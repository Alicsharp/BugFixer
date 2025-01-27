using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Extensions;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Application.Statics;
using BugFixer.Domain.Entities.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{
    public record EditQuestionCommand(EditQuestionDto EditQuestionDto):IRequest<bool>;
    public class EditQuestionCommandHandler : IRequestHandler<EditQuestionCommand, bool>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;

        public EditQuestionCommandHandler(IQuestionRepository questionRepository, IMediator mediator)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(EditQuestionCommand request, CancellationToken cancellationToken)
        {
           var question= await _questionRepository.GetById(request.EditQuestionDto.QuestionId);   
            if(question == null) return false;
            var user = await _mediator.Send(new GetUserByIdQuery(question.UserId));
            if(user == null) return false;
            FileExtensions.ManageEditorImages(question.Content,request.EditQuestionDto.Description,PathTools.EditorImageServerPath);
            //if(question.UserId != request.EditQuestionDto.UserId ) return false;
            question.Title = request.EditQuestionDto.Title;
            question.Content = request.EditQuestionDto.Description;
            //var currenttag= question.SelectQuestionTags.ToList();
            //foreach (var tag in currenttag)
            //{
            //    await _questionRepository.AddSelectedQuestionTag(tag);
            //}

            //بررسی تگ ها 

 

            if (request.EditQuestionDto.SelectedTags != null && request.EditQuestionDto.SelectedTags.Any())
            {
                foreach (var questionSelectedTag in request.EditQuestionDto.SelectedTags)
                {
                    var tag = await _questionRepository.GetTagName(questionSelectedTag.SanitizeText().Trim().ToLower());

                    if (tag == null) continue;

                    tag.UseCount += 1;
           //بررسی ریچ مدل
                    await _questionRepository.UpdateTag(tag);

                    var selectedTag = new SelectQuestionTag()
                    {
                        QuestionId = question.Id,
                        TagId = tag.Id
                    };

                    await _questionRepository.AddSelectedQuestionTag(selectedTag);
                }

                await _questionRepository.SaveChanges();

            }
            return true;
        }
    }
}
