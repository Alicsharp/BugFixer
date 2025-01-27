using BugFixer.Application.Contract.UserPanel.QuestionPanel;
using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Questions.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class QuestionController : UserPanelBaseController
    {
        private readonly IMediator _meadiator;

        public QuestionController(IMediator meadiator)
        {
            _meadiator = meadiator;
        }

        [HttpGet]
        public async Task<IActionResult> QuestionBookMarks(FilterQuestionBookMarksDto filter)
        {
            filter.UserId = User.GetUserId();
            var result = await _meadiator.Send(new FilterQuestionBookMarkQuery(filter));

            return View(result);
        }
    }
}
