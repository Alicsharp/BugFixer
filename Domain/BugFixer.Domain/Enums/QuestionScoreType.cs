﻿using System.ComponentModel.DataAnnotations;

namespace BugFixer.Domain.Enums
{
    public enum QuestionScoreType
    {
        [Display(Name = "مثبت")] Plus,

        [Display(Name = "منفی")] Minus,
    }

    public enum AnswerScoreType
    {
        [Display(Name = "مثبت")] Plus,

        [Display(Name = "منفی")] Minus,
    }

    public enum CreateScoreForAnswerResult
    {
        Error,

        NotEnoughScoreForDown,

        NotEnoughScoreForUp,

        UserCreateScoreBefore,

        Success
    }
}
