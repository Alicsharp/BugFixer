using System.ComponentModel.DataAnnotations;

namespace BugFixer.Application.Contract.DTOS.Account
{
    public class ForgotPasswordDto
    {
        [Display(Name ="ایمیل")]
        [Required(ErrorMessage ="لطفا {0}را وارد کنید")]
        [MaxLength(100,ErrorMessage ="{0}نمی تواند بیشتر از{1} کاراکتر باشید")]
        public string Email { get; set;}
        public string Captcha { get; set; }

    }
  
}

