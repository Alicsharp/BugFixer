using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.DTOS.Question
{
    public class TagDto
    {
       public long Id { get; set; }
        public string Title { get; set; }
         
        public string? Description { get; set; }

        public int UseCount { get; set; } = 0;
    }
}
