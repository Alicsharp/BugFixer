using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Account.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.ViewComponents
{
    public class UserSideBarViewComponent : ViewComponent
    {
        
        private readonly IMediator _mediator;

        public UserSideBarViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _mediator.Send(new GetUserByIdQuery(HttpContext.User.GetUserId()));

            return View("UserSideBar", user.Data);
        }
    }
 }
