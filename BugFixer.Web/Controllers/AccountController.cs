using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Services.Account.Command;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Application.Services.Result;
using GoogleReCaptcha.V3.Interface;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace BugFixer.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Ctor
        private readonly IMediator _mediator;
        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(IMediator mediator, ICaptchaValidator captchaValidator)
        {
            _mediator = mediator;
            _captchaValidator = captchaValidator;
        }
        #endregion

        #region Login
        [HttpGet("login")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Login(string ReturnUrl = "")
        {
            var result = new LoginDto();
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                result.ReturnUrl = ReturnUrl;
            }
            return View(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData["ErrorMessage"] = "با خطا مواجه شد لطفا مجدد تلاش کنید capch";
                return View(login);
            }
            if (!ModelState.IsValid) return View(login);

            var result = await _mediator.Send(new CheckUserForLoginQuery(login.Email, login.Password));
            if (result.Status == OperationResultStatus.Success)
            {
                TempData["SuccessMessage"] = "خوش آمدید!";
                var user = await _mediator.Send(new GetUserByEmailQuery(login.Email, login.Password));

                #region Login User

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Data.Id.ToString()),
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties { IsPersistent = login.RememberMe };

                await HttpContext.SignInAsync(principal, properties);

                #endregion

                TempData["SuccessMessage"] = "خوش آمدید";
                if (!string.IsNullOrEmpty(login.ReturnUrl))
                    return Redirect(login.ReturnUrl);
                return Redirect("/");
            }

            TempData["ErrorMessage"] = result.Message;

            return View(login);

         
        }
        #endregion

        #region Register
        [HttpGet("register")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Register()
        {
            return View();
        }
        //بررسی کپچا

        [HttpPost("register")]
        [AutoValidateAntiforgeryToken]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if(!ModelState.IsValid)
                return View(register);
            if (await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData["ErrorMessage"] = "با خطا مواجه شد لطفا مجدد تلاش کنید capch";
                return View(register);
            }

            var result = await _mediator.Send(new RegisterUserCommand( register.Email, register.Password, register.RePassword));
       
            if (result.Status == OperationResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = result.Message;
            return View(register);
        }
        #endregion

        #region Logout
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion


        [HttpGet("Activate-Email/{activationCode}")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ActivationUserEmail(string activationCode)
        {
 
            var result = await _mediator.Send(new ActiveteUserEmailCommand(  activationCode));
            if(result.Status == OperationResultStatus.Error) TempData["ErrorMessage"]=result.Message;
            else if(result.Status == OperationResultStatus.Success)
            {
                TempData["SuccessMessage"]= result.Message;
            }
            
            return RedirectToAction("Login");
        }

        #region forgotPassword
        [HttpGet("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword()
        {
           return View();
        }
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData["ErrorMessage"] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(forgot);
            }

            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            var result = await _mediator.Send(new ForgotPasswordCommand(forgot.Email));

            if (result.Status == OperationResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = result.Message;
            return View(forgot);
        }
   #endregion


        [HttpGet("Reset-Password/{emailActivationCode}")]
        public async Task<IActionResult> ResetPassword(string emailActivationCode)
        {
            var user = await _mediator.Send(new GetUserByActivationCodeQuery(emailActivationCode));

            if (user == null || user.Data.IsBan || user.Data.IsDeleted) return NotFound();

            return View(new RestPasswordDto
            {
                EmailActivationCode = user.Data.EmailActivationCode,
            });
        }

        [HttpPost("Reset-Password/{emailActivationCode}"), ValidateAntiForgeryToken]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ResetPassword(RestPasswordDto reset)
        {
            if (await _captchaValidator.IsCaptchaPassedAsync(reset.Captcha))
            {
                TempData["ErrorMessage"] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(reset);
            }

            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var result = await _mediator.Send(new RestPasswordCommand(reset.EmailActivationCode,reset.Password,reset.RePassword));

            if (result.Status == OperationResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = result.Message;
            
            return View(reset);
        }
    }

}
