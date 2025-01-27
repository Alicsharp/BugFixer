using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.DTOS.Question
{
    public class AnswerQuestionDto 
    {
        public string Answer { get; set; }
        public string Content {  get; set; }
        public bool IsTrue {  get; set; }
        public int Score {  get; set; }
        public long QuestionId { get; set; }
        public long Id {  get; set; }
        public long UserId { get; set; }
    }
}
