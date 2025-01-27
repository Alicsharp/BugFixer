using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Account;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Questions.Commands
{
    public record AddQuestionToBookMark(long questonid,long userId):IRequest<bool>;
    public class AddQuestionToBookMarkHandler : IRequestHandler<AddQuestionToBookMark, bool>
    {
        private readonly IQuestionRepository _questionRepository;

        public AddQuestionToBookMarkHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        //بررسی بوک کارک
        public async Task<bool> Handle(AddQuestionToBookMark request, CancellationToken cancellationToken)
        {
            var question=await _questionRepository.GetById(request.questonid);  
            if(question == null) { return false; }
            if(await _questionRepository.IsExistsQuestionInUserBookmarks(request.questonid,request.userId)) 
            {
                var bookmark = await _questionRepository.GetBookmarkByQuestionAndUserId(request.questonid, request.userId);
                if(bookmark == null) { return false; }  
                _questionRepository.RemoveBookMark(bookmark);    
            }
            else
            { 
                var newBookMark= new UserQuestionBookmark(request.questonid,request.userId );
              await  _questionRepository.AddBookMark(newBookMark);
            }
            await _questionRepository.SaveChanges();
            return true;
        }
    }
}
