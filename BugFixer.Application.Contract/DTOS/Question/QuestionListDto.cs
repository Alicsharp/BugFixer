using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.DTOS.Question
{
    public class QuestionListDto
    {
        //public long Id { get; set; }
        public long QuestionId { get; set; }

        public string Title { get; set; }

        public string UserDisplayName { get; set; }

        public string CreateDate { get; set; }

        public bool HasAnyAnswer { get; set; }

        public bool HasAnyTrueAnswer { get; set; }

        public int AnswersCount { get; set; }

        public int Score { get; set; }

        public int ViewCount { get; set; }

        public string? AnswerByDisplayName { get; set; }

        public string? AnswerByCreateDate { get; set; }

        public List<string>? Tags { get; set; }
         
    }
    public class Paging<T>
    {
        public Paging()
        {
        
        }

        public int CurrentPage { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TotalPage { get; set; }

        public int HowManyShowBeforeAfter { get; set; }

        public int TakeEntity { get; set; }

        public int SkipEntity { get; set; }

        public int AllEntityCount { get; set; }

        public List<T> Entities { get; set; }

 
    }
}
