using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Account.Command;
using BugFixer.Application.Services.Result;
using BugFixer.Application.Statics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("UserPanel")]
    public class HomeController : UserPanelBaseController
    {
        #region Ctor

        private readonly IMediator _mediator;
      
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        #region Change User Avatar
        [Route("ChangeUserAvatar")]
        public async Task<IActionResult> ChangeUserAvatar(IFormFile userAvatar)
        {
         
         var result=   await _mediator.Send(new ChangeUserAvatarCommand(HttpContext.User.GetUserId(), userAvatar));

            if(result.Status==OperationResultStatus.Error)
            {
                TempData[ErrorMessage] = "عملیات با موفقیت انجام شد .";


            }
            else
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";

            return new JsonResult(new { status = "Success" });
        }

        #endregion
    }
}
