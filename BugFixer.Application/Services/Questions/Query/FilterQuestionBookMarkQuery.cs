using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Contract.UserPanel.QuestionPanel;
using BugFixer.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record FilterQuestionBookMarkQuery(FilterQuestionBookMarksDto Filter):IRequest<FilterQuestionBookMarksDto>;
    public class FilterQuestionBookMarkQueryHandler : IRequestHandler<FilterQuestionBookMarkQuery, FilterQuestionBookMarksDto>
    {
        private readonly IQuestionRepository _questionRepository;

        public FilterQuestionBookMarkQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<FilterQuestionBookMarksDto> Handle(FilterQuestionBookMarkQuery request, CancellationToken cancellationToken)
        {
            var query = _questionRepository.GetAllBookMark();
            query = query.Where(s => s.UserId == request.Filter.UserId);

            // محاسبه تعداد کل داده‌ها
            var totalItems = query.Count();

            // تنظیم صفحه‌بندی
            var pageSize = request.Filter.TakeEntity > 0 ? request.Filter.TakeEntity : 3;
            var currentPage = request.Filter.CurrentPage > 0 ? request.Filter.CurrentPage : 1;
            var skip = (currentPage - 1) * pageSize;

            // اعمال صفحه‌بندی
            var paginatedData = query.Skip(skip).Take(pageSize).ToList();

            // محاسبه تعداد کل صفحات
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // بررسی داده‌های صفحه‌بندی شده و نگاشت به DTO
            var entities = query
                .Select(q => new QuestionListDto
                {
                    QuestionId = q.Question.Id,
                    Title = q.Question.Title,
                    UserDisplayName = "dsfdsf",
                    CreateDate = q.Question.CreateDate.ToString("yyyy-MM-dd"),
                    HasAnyAnswer = q.Question.Answers.Any(),
                    HasAnyTrueAnswer = q.Question.Answers.Any(a => a.IsTrue),
                    AnswersCount = q.Question.Answers.Count,
                    Score = q.Question.Score,
                    ViewCount = q.Question.ViewCount
                })
                .ToList();

            // مقداردهی DTO خروجی
            return new FilterQuestionBookMarksDto
            {
                CurrentPage = currentPage,
                TotalPage = totalPages,
                AllEntityCount = totalItems,
                Entities = entities
            };
        }
    }
    }
 
