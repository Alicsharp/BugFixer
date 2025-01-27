using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.DTOS.Account
{
    public enum LoginResult
    {
        Success,
        UserIsBan,
        UserNotFound,
        EmailNotActivated,
        PasswordInCorrect
    }
}
