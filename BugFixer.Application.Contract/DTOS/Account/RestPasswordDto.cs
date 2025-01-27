using System.ComponentModel.DataAnnotations;

namespace BugFixer.Application.Contract.DTOS.Account
{
    public class RestPasswordDto
    {
        [Required]
        public string EmailActivationCode {  get; set; }    

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند .")]
        public string RePassword { get; set; }
        public string Captcha { get; set; }
    }
    public enum ResetPasswordResult
    {
        Success,
        UserNotFound ,
        UserIsBan
    }
}

