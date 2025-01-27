using BugFixer.Application.Contract;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Domain.Entities.Account;
using BugFixer.Domain.Entities.Questions;
using BugFixer.Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Tag>> GetAllTags();
        Task<IQueryable<Tag>> GetAllTagsAsQueryable();
        Task<Question> GetById(long id);
        Task<bool> IsExistsTagName(string name);
        Task<Tag?> GetTagName(string name);
        Task<bool> CheckUserRequestedForTag(long userId, string tag);
        Task AddRequestTag(RequestTag tag);
        Task AddTag(Tag tag);
        Task RemoveTag(Tag tag);
        Task RemoveSelectQuestionTag(SelectQuestionTag tag);
        Task SaveChanges();
        Task<int> RequestCountForTag(string tag);
        Task AddSelectedQuestionTag(SelectQuestionTag selectQuestionTag);
        Task AddQuestion(Question question);
        Task<IQueryable<Question>> GetAllQuestions();
        Task<ICollection<string>> GetTagListByQuestionId(long questionId);
        Task AddAnswer(Answer answer);
        Task<List<Answer>> GetAllQuestionAnswers( long questionId);
        Task<bool> AddViewForQuestion(string userIp,long questionId);
        Task AddQuestionView(QuestionView view);
        Task<bool> IsExistsViewForQuestion(string userIp, long questionId);
        Task UpdateQuestion(Question question);
        Task<Answer?> GetAnswerById(long id);
        Task UpdateAnswer(Answer answer);
        Task<bool> IsExistsUserScoreForAnswer(long answerId, long userId);
        Task AddAnswerUserScore(AnswerUserScore score);
        Task<bool> IsExistsUserScoreForQuestion(long questionId, long userId);
        Task AddQuestionUserScore(QuestionUserScore score);
        Task AddBookMark(UserQuestionBookmark bookMark);
        void RemoveBookMark(UserQuestionBookmark bookMark);
        Task<bool> IsExistsQuestionInUserBookmarks(long questionId, long userId);
        Task<UserQuestionBookmark?> GetBookmarkByQuestionAndUserId(long questionId, long userId);
        Task UpdateTag(Tag tag);
        IQueryable<UserQuestionBookmark> GetAllBookMark();

    }
}
