using BugFixer.Application.Statics;
using BugFixer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BugFixer.Application.Extensions;
using MediatR;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Application.Services.Questions.Query;
namespace BugFixer.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var options = new FilterQuestionDto
            {
                TakeEntity = 10,
                Sort = FilterQuestionSortEnum.NewToOld,
            };
            ViewData["Questions"]=await _mediator.Send(new FilterQuestionsQuery(options));
            return View();
        }

        
        public async Task<IActionResult> UploadEditorImage(IFormFile upload)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);

            upload.UploadFile(fileName, PathTools.EditorImageServerPath);

            return Json(new { url = $"{PathTools.EditorImagePath}{fileName}" });
        }
      
    }
}
