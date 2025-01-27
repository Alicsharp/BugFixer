using BugFixer.Application.Services.Questions.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class QuestionAnswersListViewComponent : ViewComponent
    {
        #region Ctor

        private IMediator  _mediator1;

        public QuestionAnswersListViewComponent(IMediator  mediator)
        {
            _mediator1 = mediator;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(long questionId)
        {
            var answers = await _mediator1.Send(new GetAllQuestionAnswersQuery(questionId));

            return View("QuestionAnswersList", answers.Data);
        }
    }

}
