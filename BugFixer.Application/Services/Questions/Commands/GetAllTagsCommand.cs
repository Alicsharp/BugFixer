using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record GetAllTagsCommand:IRequest<OperationResult<List<Tag>>>;
    public class GetAllTagsCommandHandler : IRequestHandler<GetAllTagsCommand, OperationResult<List<Tag>>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetAllTagsCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<OperationResult<List<Tag>>> Handle(GetAllTagsCommand request, CancellationToken cancellationToken)
        {
         var result=await _questionRepository.GetAllTags();
            if (result == null) return OperationResult<List<Tag>>.Error();
            return OperationResult<List<Tag>>.Success(result);
        }
    }
}
