using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Services.Questions.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
 
namespace BugFixer.Web.ViewComponents
{
    public class ScoreDesQuestionsViewComponent : ViewComponent
    {
        #region Ctor

        private IMediator _mediator;

        public ScoreDesQuestionsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var options = new FilterQuestionDto
            {
                TakeEntity = 5,
                Sort = FilterQuestionSortEnum.ScoreHighToLow
            };

            var result = await _mediator.Send(new FilterQuestionsQuery(options));

            return View("ScoreDesQuestions", result);
        }
    }

    public class CreateDateDesQuestionsViewComponent : ViewComponent
    {
        #region Ctor

        private IMediator _mediator;

        public CreateDateDesQuestionsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var options = new FilterQuestionDto
            {
                TakeEntity = 5,
                Sort = FilterQuestionSortEnum.NewToOld
            };

            var result = await _mediator.Send(new FilterQuestionsQuery(options));

            return View("CreateDateDesQuestions", result);
        }
    }

    public class UseCountDesTagsViewComponent : ViewComponent
    {
        #region Ctor

        private IMediator _mediator;

        public UseCountDesTagsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var options = new FilterTagDto
            {
                TakeEntity = 10,
                Sort = FilterTagEnum.UseCountHighToLow
            };

            var result = await _mediator.Send(new FilterTagsQuery(options));

            return View("UseCountDesTags", result.Data);
        }

    }

}
