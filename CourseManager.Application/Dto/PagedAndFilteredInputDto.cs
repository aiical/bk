using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.Dto
{
    /// <summary>
    /// 支持分页、过滤的InputDto
    /// </summary>
    public class PagedAndFilteredInputDto : IPagedResultRequest
    {
        [Range(1, CourseManagerConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public string Filter { get; set; }

        public PagedAndFilteredInputDto()
        {
            MaxResultCount = CourseManagerConsts.DefaultPageSize;
        }
    }
}
