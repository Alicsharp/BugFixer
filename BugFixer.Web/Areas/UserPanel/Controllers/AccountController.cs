using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Account.Command;
using BugFixer.Application.Services.Account.Query;
using BugFixer.Application.Services.Location.Query;
using BugFixer.Application.Services.Result;


//using BugFixer.Application.Services.Location.Query;
using BugFixer.infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class AccountController : UserPanelBaseController
    {
        #region Ctor
     
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion

        #region Edit User Info

        [HttpGet]
        public async  Task<IActionResult> EditInfo(long UsrId)
        {
            ViewData["States"] = await _mediator.Send(new GetAllStatesQuery());
           
            var user=await _mediator.Send(new GetUserByIdQuery(HttpContext.User.GetUserId()));
            var result = new EditUserDto
            {
                BirthDate = user.Data.BirthDate ,
                CityId = user.Data.CityId,
                CountryId = user.Data.CountryId,
                Description = user.Data.Description,
                FirstName = user.Data.FirstName ?? string.Empty,
                LastName = user.Data.LastName ?? string.Empty,
                GetNewsLetter = user.Data.GetNewsLetter,
                PhoneNumber = user.Data.PhoneNumber,
            };
            if(result.CountryId.HasValue)
            {
                ViewData["Cities"]=await _mediator.Send(new GetAllStatesQuery(result.CountryId.Value)); 
            }
            return View(result);
         }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo( EditUserDto editUserView)
        {

         var result= await  _mediator.Send(new UpdateUserCommand(HttpContext.User.GetUserId(),editUserView));
            if(result.Status ==  OperationResultStatus.Success)
                return Ok( );
//چک
            return View( );
        }

        #endregion

        #region Load Cities

        public async Task<IActionResult> LoadCities(long countryId)
        {
            var result = await _mediator.Send(new GetAllStatesQuery(countryId)); 

            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeUserPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordDto changeUserPasswordDto)
        {
            var result = await _mediator.Send(new ChangeUserPasswordCommand(HttpContext.User.GetUserId(), changeUserPasswordDto));
            if (result.Status == OperationResultStatus.Success)
            {
                TempData[SuccessMessage] = "رمز شما با موفقیت تغییر پیدا کرد";
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account", new { area = "" });
 
            }
            return View(changeUserPasswordDto);
        }
        #endregion
    }
}
