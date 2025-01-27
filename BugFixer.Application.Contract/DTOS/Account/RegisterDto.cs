using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.DTOS.Account
{
    public class RegisterDto
    {
        #region RegisterWithName
        //[Display(Name = "نام")]
        //[MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //public string FirstName { get; set; }

        //[Display(Name = "نام خانوادگی")]
        //[MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //public string LastName { get; set; }

        //[Display(Name = "شماره موبایل")]
        //[MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //public string PhoneNumber { get; set; }
        //[Display(Name = "کلمه عبور")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //public string Password { get; set; }

        //[Display(Name = "تکرار کلمه عبور")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        //[Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند .")]
        //public string RePassword { get; set; }
        //public string Captcha { get; set; }
        #endregion

        #region RegisterWithEmail
        [Display(Name = "ایمیل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند .")]
        public string RePassword { get; set; }
        public string Captcha {  get; set; }
        #endregion
    }
}
