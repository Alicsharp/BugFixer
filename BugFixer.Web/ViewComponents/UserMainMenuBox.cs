using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Account.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class UserMainMenuBoxViewComponent : ViewComponent
    {
        #region Ctor

        private readonly IMediator _mediator;

        public UserMainMenuBoxViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _mediator.Send(new GetUserByIdQuery(HttpContext.User.GetUserId()));
            

            return View("UserMainMenuBox", user.Data);
        }
    }

}
