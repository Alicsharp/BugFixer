using BugFixer.Application.Contract.DTOS.Question;
 
using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Account.Command;
using BugFixer.Application.Services.Questions.Commands;
using BugFixer.Application.Services.Questions.Query;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Questions;
using BugFixer.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
 
using System.Drawing.Printing;
using BugFixer.Application.Security;


namespace BugFixer.Web.Controllers
{
    public class QuestionController : BaseController
    {
       
        private readonly IMediator _mediator;
       
        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("create-question")]
        public async Task<IActionResult> CreateQuestion( )
        {
            return View();
        }
        [Authorize]
        [HttpPost("create-question"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateQuestionDto createQuestionDto)
        {
            var tagResult =
              await _mediator.Send(new CheckTagValidationQuery(HttpContext.User.GetUserId(),createQuestionDto.SelectedTags ));

            if (tagResult.Status == OperationResultStatus.Error)
            {
                createQuestionDto.SelectedTagsJson = JsonConvert.SerializeObject(createQuestionDto.SelectedTags);
                createQuestionDto.SelectedTags = null;

                TempData["WarningMessage"] = tagResult.Message;

                return View(createQuestionDto);
            }
            createQuestionDto.UserId = HttpContext.User.GetUserId();    
            var result=await _mediator.Send(new CreateQuestionCommand(createQuestionDto));  
            if(result.Status==OperationResultStatus.Success)
            {
                TempData["SuccessMassage"] = "عملیات با موفیقت انجام شد";
                await _mediator.Send(new UpdateUserScoreAndMedal(createQuestionDto.UserId, 10));
                return Redirect("/");
            }
            createQuestionDto.SelectedTagsJson = JsonConvert.SerializeObject(createQuestionDto.SelectedTags);
            createQuestionDto.SelectedTags = null;

            return View(createQuestionDto);

        }
        [HttpGet("Get-QuestionList")]
        public async Task<IActionResult> QuestionList(FilterQuestionDto filterQuestionDto)
        {
            var result = await _mediator.Send(new FilterQuestionsQuery(filterQuestionDto));
          
            return View(result);
        }

        [HttpGet("q/{questionId}")]
        public async Task<IActionResult> QuestionDetailByShortLink(long questionId)
        {
            var question = await _mediator.Send(new GetQuestionByIdQuery(questionId));

            if (question == null) return NotFound();

            return RedirectToAction("QuestionDetail", "Question", new { questionId = questionId });
        } 
        [HttpGet("get-tags")]
        public async Task<IActionResult> GetTagsForSuggest(string name)
        {
            var tags = await _mediator.Send(new GetAllTagsCommand());
            var filteredTag = tags.Data.Where(s => s.Title.Contains(name)).Select(s => s.Title).ToList();
            return Json(filteredTag);
        }
 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AnswerQuestion(AnswerQuestionDto answerQuestionDto)
        {
            answerQuestionDto.UserId=User.GetUserId();
            var result= await _mediator.Send(new AnswerQuestionCommand(answerQuestionDto));
        await    _mediator.Send(new UpdateUserScoreAndMedal(answerQuestionDto.UserId, 10));
            if(result)
                return new JsonResult(new {status="Success"});
           return new JsonResult(new {status="Error"});
        }
        [HttpPost("selectTrueAnswer")]
        public async Task<IActionResult> SelectTrueAnswer(long answerId)
        {
            if (!User.Identity.IsAuthenticated) return new JsonResult(new { status = "NotAuthorize" });
            if (await _mediator.Send(new HasUserAccessToSelectTrueAnswerQuery(User.GetUserId(), answerId))) return new JsonResult(new { status = "NotAccess" });
            await _mediator.Send(new SelectTrueAnswerCommand(User.GetUserId(),answerId));
            return new JsonResult(new { status = "Success" });

        }

        [HttpPost("ScoreUpForAnswer")]
        public async Task<IActionResult> ScoreUpForAnswer(long answerId)
        {
            var result = await _mediator.Send(new CreateScopeForAnswerCommand(answerId, AnswerScoreType.Plus, User.GetUserId()));
            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });
                case CreateScoreForAnswerResult.NotEnoughScoreForDown:
                    return new JsonResult(new { status = "NotEnoughScoreForDown" });
                case CreateScoreForAnswerResult.NotEnoughScoreForUp:
                    return new JsonResult(new { status = "NotEnoughScoreForUp" });
                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "UserCreateScoreBefore" });
                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("ScoreDownForAnswer")]
        public async Task<IActionResult> ScoreDownForAnswer(long answerId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new JsonResult(new { status = "NotAuthorize" });
            }

            if (!await _mediator.Send(new HasUserAccessToSelectTrueAnswerQuery(User.GetUserId(), answerId)))
            {
                return new JsonResult(new { status = "NotAccess" });
            }

            await _mediator.Send(new SelectTrueAnswerCommand(User.GetUserId(), answerId));

            return new JsonResult(new { status = "Success" });
        }

        [HttpPost("ScoreUpForQuestion")]
        public async Task<IActionResult> ScoreUpForQuestion(long questionId)
        {
            var result = await _mediator.Send(new CreateScoreForQuestionQuery(questionId, QuestionScoreType.Plus, User.GetUserId()));

            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });
                case CreateScoreForAnswerResult.NotEnoughScoreForDown:
                    return new JsonResult(new { status = "NotEnoughScoreForDown" });
                case CreateScoreForAnswerResult.NotEnoughScoreForUp:
                    return new JsonResult(new { status = "NotEnoughScoreForUp" });
                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "UserCreateScoreBefore" });
                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("ScoreDownForQuestion")]
        public async Task<IActionResult> ScoreDownForQuestion(long questionId)
        {
            var result = await _mediator.Send(new CreateScoreForQuestionQuery(questionId, QuestionScoreType.Minus, User.GetUserId()));

            switch (result)
            {
                case CreateScoreForAnswerResult.Error:
                    return new JsonResult(new { status = "Error" });
                case CreateScoreForAnswerResult.NotEnoughScoreForDown:
                    return new JsonResult(new { status = "NotEnoughScoreForDown" });
                case CreateScoreForAnswerResult.NotEnoughScoreForUp:
                    return new JsonResult(new { status = "NotEnoughScoreForUp" });
                case CreateScoreForAnswerResult.UserCreateScoreBefore:
                    return new JsonResult(new { status = "UserCreateScoreBefore" });
                case CreateScoreForAnswerResult.Success:
                    return new JsonResult(new { status = "Success" });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        [HttpPost("AddQuestionToBookMark")]
        [Authorize]
        public async Task<IActionResult> AddQuestionToBookMark(long questionId)
        {
            var result= await _mediator.Send(new AddQuestionToBookMark(questionId,User.GetUserId()));
            return new JsonResult(new { status = "Success" });
        }
        [HttpGet("edit-question/{id}")]
        
        public async Task<IActionResult> EditQuestion(long questionId=10)
        {
            var result = await _mediator.Send(new FilterEditQuestionDtoCommand(questionId, User.GetUserId()));
            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost("edit-question/{id}"), ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditQuestion(EditQuestionDto edit)
        {
            var result = await _mediator.Send(new EditQuestionCommand(edit));
            return View();
        }
        [HttpGet("EditAnswer/{answerId}")]
        [Authorize]
        public async Task<IActionResult> EditAnswer(long answerId)
        {
            var resullt= await _mediator.Send(new FilterEditAnswerQuery(answerId,User.GetUserId())); 
            if(resullt == null) return NotFound();  
            return View(resullt.Data);
        }

        [HttpPost("EditAnswer/{answerId}"),ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditAnswer(EditAnswerDto editAnswerDto)
        {
            if(!ModelState.IsValid) return View(editAnswerDto);
            editAnswerDto.UserId=User.GetUserId();  
            var result= await _mediator.Send(new EditAnswerCommand(editAnswerDto));
            if (result)
            {
                TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد.";
                return RedirectToAction("QuestionDetail", "Question", new { questionId = editAnswerDto.QuestionId });
            }

            TempData["ErrorMessage"] = "خطایی رخ داده است.";

            return View(editAnswerDto);
        }
        [HttpGet("get-questions")]
        public async Task<IActionResult> GetQuestionsForSuggest(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(null);
            }

            var questions = await _mediator.Send(new GetAllQuestionsQuery());

            var filteredQuestions = questions.Where(s => s.Title.Contains(name))
                .Select(s => s.Title)
                .ToList();

            return Json(filteredQuestions);
        }
     
        [HttpGet("questions/{questionId}")]
        public async Task<IActionResult> QuestionDetail(long questionId)
        {
            var question = await _mediator.Send(new GetQuestionByIdQuery(questionId));

            if (question == null) return NotFound();

            ViewBag.IsBookMark = false;

            if (User.Identity.IsAuthenticated && await _mediator.Send(new IsExistsQuestionInUserBookmarksCommand(question.Data.Id, User.GetUserId())))
            {
                ViewBag.IsBookMark = true;
            }

            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            // if (userIp != null)
            // {
            //     await _mediator.Send( new AddViewForQuestion(userIp.ToString(), question));
            // }

            ViewData["TagsList"] = await _mediator.Send( new GetTagListByQuestionIdQuery(question.Data.Id));

            return View(question.Data);
        }
        
        [HttpGet("tags/{tagName}")]
        public async Task<IActionResult> QuestionListBtTag(FilterQuestionDto filter, string tagName)
        {
            tagName = tagName.Trim().ToLower().SanitizeText();

            filter.Title = tagName;

            var result = await _mediator.Send(new FilterQuestionsQuery(filter));

            ViewBag.TagTitle = tagName;

            return View(result);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> FilterTags(FilterTagDto filter)
        {
            filter.TakeEntity = 12;

            var result = await _mediator.Send(new FilterTagsQuery(filter));

            return View(result.Data);
        }

    } 
}
