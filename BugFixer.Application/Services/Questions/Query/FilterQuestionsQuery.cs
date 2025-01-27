using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Extensions;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Security;
using BugFixer.Domain.Entities.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Query
{
    public record FilterQuestionsQuery(FilterQuestionDto filterQuestion) : IRequest<FilterQuestionDto>;
    public class FilterQuestionsQueryHandler : IRequestHandler<FilterQuestionsQuery, FilterQuestionDto>
    {
        private readonly IQuestionRepository _questionRepository;

        public FilterQuestionsQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<FilterQuestionDto> Handle(FilterQuestionsQuery request, CancellationToken cancellationToken)
        {
            var query = await _questionRepository.GetAllQuestions();

            // فیلتر بر اساس عنوان
            if (!string.IsNullOrEmpty(request.filterQuestion.Title))
            {
                query = query.Where(s => s.Title.Contains(request.filterQuestion.Title.SanitizeText().Trim()));
            }

            // مرتب‌سازی
            switch (request.filterQuestion.Sort)
            {
                case FilterQuestionSortEnum.NewToOld:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterQuestionSortEnum.OldToNew:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterQuestionSortEnum.ScoreHighToLow:
                    query = query.OrderByDescending(s => s.Score);
                    break;
                case FilterQuestionSortEnum.ScoreLowToHigh:
                    query = query.OrderBy(s => s.Score);
                    break;
            }

            // تعداد کل آیتم‌ها
            var totalItems = query.Count();

            // صفحه‌بندی
            var pageSize = request.filterQuestion.StartPage > 0 ? request.filterQuestion.TakeEntity : 3;
            var currentPage = request.filterQuestion.CurrentPage > 0 ? request.filterQuestion.CurrentPage : 1;
            var skip = (currentPage - 1) * pageSize;

            var paginatedData = query.Skip(skip).Take(pageSize).ToList();

            // محاسبه تعداد کل صفحات
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // نتیجه نهایی
            return new FilterQuestionDto
            {
                Entities = paginatedData.Select(q => new QuestionListDto
                {
                    
                    AnswersCount=q.Answers.Count(a=>!a.IsDeleted),
                    HasAnyAnswer=q.Answers.Any(a=>!a.IsDeleted),    
                    HasAnyTrueAnswer=q.Answers.Any(a=>!a.IsDeleted && a.IsTrue),
                    QuestionId = q.Id,
                    Score=q.Score,
                    Title = q.Title,
                    ViewCount=q.ViewCount,
                    UserDisplayName = q.User.GetUserDisplayName(),
                    Tags=q.SelectQuestionTags.Where(a=>!a.Tag.IsDeleted).Select(a=>a.Tag.Title).ToList(),
                    AnswerByDisplayName=q.Answers.Any(a=>!a.IsDeleted) ? q.Answers.OrderByDescending(a=>a.CreateDate).First().CreateDate.TimeAgo() : null,
                }).ToList(),
                CurrentPage = currentPage,
                TotalPage = totalPages,
                TakeEntity = pageSize,
                AllEntityCount = totalItems
            };


        }
    }

}

 