namespace BugFixer.Application.Contract.DTOS.Question
{
    public class FilterQuestionDto:Paging<QuestionListDto>  
    {
        public long Id {  get; set; }   
        public string? Title { get; set; }
        public FilterQuestionSortEnum Sort { get; set; }    
    }
}
