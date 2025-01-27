using System.ComponentModel.DataAnnotations;

namespace BugFixer.Application.Contract.DTOS.Question
{
    public enum FilterQuestionSortEnum
    {
        [Display(Name = "تاریخ ثبت نزولی")] NewToOld,
        [Display(Name = "تاریخ ثبت صعودی")] OldToNew,
        [Display(Name = "امتیاز نزولی")] ScoreHighToLow,
        [Display(Name = "امتیاز صعودی")] ScoreLowToHigh
    }
}
