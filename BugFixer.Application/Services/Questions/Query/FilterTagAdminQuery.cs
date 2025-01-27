using AutoMapper;
using BugFixer.Application.Contract.Admin;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//بررسی پیچینگ
namespace BugFixer.Application.Services.Questions.Query
{
    public record FilterTagAdminQuery(FilterTagAdminDto filter) :IRequest<FilterTagAdminDto>;
    public class FilterTagAdminQueryHandler : IRequestHandler<FilterTagAdminQuery, FilterTagAdminDto>
    {
       private readonly IQuestionRepository _questionRepository;
       private readonly IMapper _mapper;

        public FilterTagAdminQueryHandler(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<FilterTagAdminDto> Handle(FilterTagAdminQuery request, CancellationToken cancellationToken)
        {
            var query = await _questionRepository.GetAllTagsAsQueryable(); // دریافت تمامی تگ‌ها به عنوان لیست

            // فیلتر بر اساس عنوان
            if (!string.IsNullOrEmpty(request.filter.Title))
            {
                query = query.Where(s => s.Title.Contains(request.filter.Title));
            }

            // فیلتر بر اساس وضعیت
            switch (request.filter.Status)
            {
                case FilterTagAdminStatus.HasDescription:
                    query =  query.Where(s => !string.IsNullOrEmpty(s.Description));
                    break;
                case FilterTagAdminStatus.NoDescription:
                    query = query.Where(s => string.IsNullOrEmpty(s.Description));
                    break;
            }

 
            // محاسبه تعداد کل صفحات
            // تعداد کل آیتم‌ها
            var totalItems = query.Count();

            // صفحه‌بندی
            var pageSize = request.filter.StartPage > 0 ? request.filter.TakeEntity : 3;
            var currentPage = request.filter.CurrentPage > 0 ? request.filter.CurrentPage : 1;
            var skip = (currentPage - 1) * pageSize;

            var paginatedData = query.Skip(skip).Take(pageSize).Select(tag => new TagDto
            {
             Id = tag.Id,
             Title = tag.Title,
             Description = tag.Description,
             UseCount = tag.UseCount
             })
         .ToList();

            // محاسبه تعداد کل صفحات
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // مقداردهی داده‌ها و تنظیم خروجی
            return new FilterTagAdminDto
            {
                Entities = paginatedData.Select(q => new TagDto
                {
                    Id = q.Id,
                    Description = q.Description,
                    Title = q.Title,
                    UseCount = q.UseCount
                }).ToList(),
                CurrentPage = currentPage,
                TotalPage = totalPages,
                TakeEntity = pageSize,
                AllEntityCount = totalItems
            };
        }
        
    }
}
