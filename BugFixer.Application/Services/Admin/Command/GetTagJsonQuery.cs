using BugFixer.Application.Contract.Admin.DTOS;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Admin.Command
{
    public record GetTagJsonQuery:IRequest<OperationResult<List<TagViewModelJson>>>;
    public class GetTagJsonQueryHnadler : IRequestHandler<GetTagJsonQuery, OperationResult<List<TagViewModelJson>>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetTagJsonQueryHnadler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<OperationResult<List<TagViewModelJson>>> Handle(GetTagJsonQuery request, CancellationToken cancellationToken)
        {
            var tags = await _questionRepository.GetAllTagsAsQueryable();

            var result= tags.OrderByDescending(s=>s.UseCount).Take(10).Select(s=>new TagViewModelJson
            {
                Title= s.Title,
                UseCount=s.UseCount
            }).ToList();  
             return OperationResult<List<TagViewModelJson>>.Success(result);
        }
    }
}
