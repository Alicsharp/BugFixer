namespace BugFixer.Application.Contract.DTOS.Question
{
    public class EditAnswerDto
    {
        public string Content { get; set; }

        public long AnswerId { get; set; }

        public long UserId { get; set; }
        public long QuestionId { get; set; }

    }
}
