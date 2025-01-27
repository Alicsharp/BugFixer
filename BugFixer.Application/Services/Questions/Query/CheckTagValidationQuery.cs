using BugFixer.Application.Contract.DTOS.Common;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Account;
using BugFixer.Domain.Entities.Tags;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record CheckTagValidationQuery(long UserId,List<string> tags):IRequest<OperationResult>;
    public class CheckTagValidationQueryHandler : IRequestHandler<CheckTagValidationQuery, OperationResult>
    { 
        private readonly IQuestionRepository _questionRepository;
        private  ScoreManagementDto _scoreManagement;
        public CheckTagValidationQueryHandler(IQuestionRepository questionRepository,IOptions<ScoreManagementDto> scoreManagementDto)
        {
            _questionRepository = questionRepository;
            _scoreManagement = scoreManagementDto.Value;   
        }

        public async Task<OperationResult> Handle(CheckTagValidationQuery request, CancellationToken cancellationToken)
        {
            if(request.tags !=null && request.tags.Any())
            {
                foreach(var tag in request.tags)
                {
                    var isExistsTag=await _questionRepository.IsExistsTagName(tag.SanitizeText().Trim().ToLower());
                    if (isExistsTag) continue;
                    var isUserRequestedForTag = await _questionRepository.CheckUserRequestedForTag(request.UserId, tag.SanitizeText().Trim().ToLower());
                    if (isUserRequestedForTag)
                    {
                        return new OperationResult
                        {
                            Status = OperationResultStatus.Error,
                            Message = $"تگ {tag} برای اعتبارسنجی نیاز به {_scoreManagement.MinRequestCountForVerifyTag} درخواست دارد ."
                        };
                    }
                    var tagRequest = new RequestTag
                    {
                        Title = tag.SanitizeText().Trim().ToLower(),
                        UserId = request.UserId,
                    };
                    await _questionRepository.AddRequestTag(tagRequest);
                    await _questionRepository.SaveChanges();
                    var requestedCount=await _questionRepository.RequestCountForTag(tag.SanitizeText().Trim().ToLower());

                    if(requestedCount <_scoreManagement.MinRequestCountForVerifyTag)
                    {
                        return new OperationResult
                        {
                            Status= OperationResultStatus.Error,
                            Message = $"تگ {tag} برای اعتبارسنجی نیاز به {_scoreManagement.MinRequestCountForVerifyTag} درخواست دارد ."
                        };
                    }
                    var newTag = new Tag
                    {
                        Title = tag.SanitizeText().Trim().ToLower()
                    };
                    await _questionRepository.AddTag(newTag);
                    await _questionRepository.SaveChanges();    
                }
                return new OperationResult
                {
                    Status = OperationResultStatus.Success,
                    Message = "تگ های ورودی معتبر می باشد"
                };
            }
            return new OperationResult
            {
                Status = OperationResultStatus.Error,
                Message = "تگ های ورودی نمی تواند خالی باشد"
            };

         
        }
    }
}
