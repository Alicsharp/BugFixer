using BugFixer.Application.Contract.DTOS.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Contract.Admin
{
    public class FilterTagAdminDto:Paging<TagDto>
    {
        public FilterTagAdminDto()
        {
            Status = FilterTagAdminStatus.All;
        }

        public string? Title { get; set; }

        public FilterTagAdminStatus Status { get; set; }
    }
    public enum FilterTagAdminStatus
    {
        [Display(Name = "همه")] All,

        [Display(Name = "دارای توضیحات")] HasDescription,

        [Display(Name = "بدون توضیحات")] NoDescription
    }
}
