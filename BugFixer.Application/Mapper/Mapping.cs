using AutoMapper;
using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Contract.DTOS.Question;
using BugFixer.Domain.Entities.Account;
using BugFixer.Domain.Entities.Questions;
using BugFixer.Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User,UserLoginDto>().ReverseMap();    
            CreateMap<User,LoginDto>().ReverseMap();
            CreateMap<User,EditUserDto>().ReverseMap();
            CreateMap<Tag,FilterTagDto>().ReverseMap();
            CreateMap<Answer,EditAnswerDto>().ReverseMap();
            CreateMap<AnswerQuestionDto,Answer>().ReverseMap();
        }
    }
}
