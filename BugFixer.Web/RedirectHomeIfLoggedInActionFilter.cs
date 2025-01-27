using Microsoft.AspNetCore.Mvc.Filters;

namespace BugFixer.Web
{
    public class RedirectHomeIfLoggedInActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (context.HttpContext.User.Identity.IsAuthenticated)
                  context.HttpContext.Response.Redirect("/");
        }
    }
}
