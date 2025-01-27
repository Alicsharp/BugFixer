using BugFixer.Application.Contract.Admin;
using BugFixer.Application.Services.Admin.Command;
using BugFixer.Application.Services.Questions.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

     
        public async Task<IActionResult> LoadFilterTagsPartial(FilterTagAdminDto filterTagDto)
        {
            filterTagDto.TakeEntity = 2;
            var result = await _mediator.Send(new FilterTagAdminQuery(filterTagDto));
       
            return PartialView("_FilterTagsPartial", result);
        }
        
     

        public async Task<IActionResult> test(FilterTagAdminDto filterTagDto)
        {
            
            var result= await _mediator.Send(new FilterTagAdminQuery(filterTagDto)); 
            return View("test", result);  
        }

   
        public async Task<IActionResult> Dashboard()
        {
            var result = await _mediator.Send(new GetTagJsonQuery());
            var s = JsonConvert.SerializeObject(result.Data);
            ViewData["ChartDataJson"] =s;
            return View( );
        }
    }
}
