using BugFixer.Application.Contract;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Account;
using BugFixer.Domain.Entities.Questions;
using BugFixer.Domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly BugFixerDbContext _dbContext;

        public QuestionRepository(BugFixerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddQuestion(Question question)
        {
           await _dbContext.Questions.AddAsync(question);   
        }

        public async  Task AddRequestTag(RequestTag tag)
        {
            await _dbContext.RequestTags.AddAsync(tag);
        }

        public async Task AddSelectedQuestionTag(SelectQuestionTag selectQuestionTag)
        {
             await _dbContext.AddAsync(selectQuestionTag);
        }

        public async Task AddTag(Tag tag)
        {
             await _dbContext.Tags.AddAsync(tag);
        }

        public async Task<bool> CheckUserRequestedForTag(long userId, string tag)
        {
            return await _dbContext.RequestTags.AnyAsync(s=>s.UserId ==userId && s.Title.Equals(tag) && !s.IsDeleted);
        }

        

        public async Task<List<Tag>> GetAllTags()
        {
            return await _dbContext.Tags.Where(s => !s.IsDeleted).ToListAsync();
        }

 
        public async Task<Tag?> GetTagName(string name)
        {
           return await _dbContext.Tags.FirstOrDefaultAsync(s=>!s.IsDeleted && s.Title.Equals(name));
        }

        public async Task<bool> IsExistsTagName(string name)
        {
            return await _dbContext.Tags.AnyAsync(s => s.Title.Equals(name) && !s.IsDeleted);
        }

       

        public async Task<int> RequestCountForTag(string tag)
        {
             return await _dbContext.RequestTags.CountAsync(s=>!s.IsDeleted && s.Title.Equals(tag));
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<Question>> GetAllQuestions()
        {
            return _dbContext.Questions.Include(s=>s.Answers).Include(s=>s.SelectQuestionTags).ThenInclude(a=>a.Tag).Include(s=>s.User).Where(s => !s.IsDeleted).AsQueryable();
        }

        public async Task<Question?> GetById(long id)
        {
            return await _dbContext.Questions.Include(s=>s.Answers).Include(s=>s.User).Include(s=>s.SelectQuestionTags).FirstOrDefaultAsync(s=>s.Id == id && !s.IsDeleted) ;
        }

        public async Task<ICollection<string>> GetTagListByQuestionId(long questionId)
        {
             return await _dbContext.SelectQuestionTags.Include(s=>s.Tag).Where(s=>s.QuestionId == questionId).Select(s=>s.Tag.Title).ToListAsync();
        }

        public async Task AddAnswer(Answer answer)
        {
           await _dbContext.Answers.AddAsync(answer);
        }

        public async Task<List<Answer>> GetAllQuestionAnswers(long questionId)
        {
            return await _dbContext.Answers.Include(u => u.User).Where(s => s.QuestionId == questionId && !s.IsDeleted).OrderByDescending(s=>s.CreateDate).ToListAsync();
        }

        public async Task<bool> AddViewForQuestion(string userIp, long questionId)
        {
           return await _dbContext.QuestionViews.AnyAsync(s=>s.UserId.Equals(userIp) && s.QuestionId == questionId);    
        }

        public async Task AddQuestionView(QuestionView view)
        {
          await _dbContext.QuestionViews.AddAsync(view);
        }
        public async Task<bool> IsExistsViewForQuestion(string userIp, long questionId)
        {
            return await _dbContext.QuestionViews.AnyAsync(s => s.UserId.Equals(userIp) && s.QuestionId == questionId);
        }

        public async Task UpdateQuestion(Question question)
        {
               _dbContext.Questions.Update(question);
        }

        public async Task<Answer?> GetAnswerById(long id)
        {
            return await _dbContext.Answers.Include(s => s.Question).FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }
        public async Task UpdateAnswer(Answer answer)
        {
            _dbContext.Answers.Update(answer);
        }
        public async Task<bool> IsExistsUserScoreForAnswer(long answerId, long userId)
        {
            return await _dbContext.AnswerUserScores.AnyAsync(s => s.AnswerId == answerId && s.UserId == userId);
        }

        public async Task AddAnswerUserScore(AnswerUserScore score)
        {
           await _dbContext.AnswerUserScores.AddAsync(score);
        }
        public async Task<bool> IsExistsUserScoreForQuestion(long questionId, long userId)
        {
            return await _dbContext.QuestionUserScores.AnyAsync(s => s.QuestionId == questionId && s.UserId == userId);
        }
        public async Task AddQuestionUserScore(QuestionUserScore score)
        {
            await _dbContext.QuestionUserScores.AddAsync(score);
        }

        public async Task AddBookMark(UserQuestionBookmark bookMark)
        {
            await _dbContext.AddAsync(bookMark);
        }

        public void RemoveBookMark(UserQuestionBookmark bookMark)
        {
             _dbContext.Remove(bookMark);  
        }

        public async Task<bool> IsExistsQuestionInUserBookmarks(long questionId, long userId)
        {
            return await _dbContext.Bookmarks.AnyAsync(s=>s.QuestionId == questionId && s.UserId == userId);    
        }

        public async Task<UserQuestionBookmark?> GetBookmarkByQuestionAndUserId(long questionId, long userId)
        {
            return await _dbContext.Bookmarks.FirstOrDefaultAsync(s => s.QuestionId == questionId && s.UserId == userId);

        }

        public async Task RemoveTag(Tag tag)
        {
            _dbContext.Remove(tag);
        }

        public async Task RemoveSelectQuestionTag(SelectQuestionTag tag)
        {
            _dbContext.Remove(tag);
        }

        public async Task UpdateTag(Tag tag)
        {
            _dbContext.Update(tag);
        }

        public IQueryable<UserQuestionBookmark> GetAllBookMark()
        {
            return   _dbContext.Bookmarks.Include(s=>s.Question).AsQueryable();
        }
        public async Task<IQueryable<Tag>> GetAllTagsAsQueryable()
        {
            return _dbContext.Tags.Where(s => !s.IsDeleted).AsQueryable();
        }

    }
}
