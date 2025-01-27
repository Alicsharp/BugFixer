using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.DTOS.Account
{
    public class UserLoginDto
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }


        public string? LastName { get; set; }


        public string? PhoneNumber { get; set; }


        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }

        public string? Description { get; set; }

        public string Avatar { get; set; }
        public bool RememberMe { get; set; }
        public string Captcha { get; set; }
    }
}

