using BugFixer.Domain.Entities.Questions;
using System.ComponentModel.DataAnnotations;

namespace BugFixer.Domain.Entities.Account
{
    public class UserQuestionBookmark
    {
        #region Properties

        [Key]
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public long UserId { get; set; }

        #endregion

        #region Relations

        public Question Question { get; set; }

        public User User { get; set; }

        public UserQuestionBookmark( long questionId, long userId )
        {
           QuestionId = questionId;
            UserId = userId;
          
        }

        public static UserQuestionBookmark AddBookMark (long QuestionId, long UserId)
        {
            return new UserQuestionBookmark( QuestionId, UserId);
        }
    
        #endregion
    }
}
