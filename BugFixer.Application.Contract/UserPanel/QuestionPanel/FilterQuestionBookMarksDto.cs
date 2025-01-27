using BugFixer.Application.Contract.DTOS.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.UserPanel.QuestionPanel
{
    public class FilterQuestionBookMarksDto:Paging<QuestionListDto>
    {
      public long UserId {  get; set; } 
    }
}
