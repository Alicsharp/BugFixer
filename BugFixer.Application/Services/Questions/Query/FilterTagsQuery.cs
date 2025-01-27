using AutoMapper;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugFixer.Application.Services.Result;

namespace BugFixer.Application.Services.Questions.Query
{
 
    public record FilterTagsQuery(FilterTagDto filter) : IRequest<OperationResult<FilterTagDto>>;

    public class FilterTagsQueryHandler : IRequestHandler<FilterTagsQuery, OperationResult<FilterTagDto>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public FilterTagsQueryHandler(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<FilterTagDto>> Handle(FilterTagsQuery request, CancellationToken cancellationToken)
        {
            var query = await _questionRepository.GetAllTags();

            // Filter by title if provided
            if (!string.IsNullOrEmpty(request.filter.Title))
            {
                query = query.Where(s => s.Title.Contains(request.filter.Title)).ToList();
            }

            // Apply sorting
            switch (request.filter.Sort)
            {
                case FilterTagEnum.NewToOld:
                    query = query.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case FilterTagEnum.OldToNew:
                    query = query.OrderBy(s => s.CreateDate).ToList();
                    break;
                case FilterTagEnum.UseCountHighToLow:
                    query = query.OrderByDescending(s => s.UseCount).ToList();
                    break;
                case FilterTagEnum.UseCountLowToHigh:
                    query = query.OrderBy(s => s.UseCount).ToList();
                    break;
            }

            // Map tags to DTO including Entities for the view
            var filterTagDto = new FilterTagDto
            {
                Entities = query.Select(tag => new TagDto
                {
                    Title = tag.Title
                }).ToList()
            };

            return OperationResult<FilterTagDto>.Success(filterTagDto);
        }
    }

}
