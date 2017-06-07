using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.Dto
{
    /// <summary>
    /// 支持分页的InputDto
    /// </summary>
    public class PagedInputDto : IPagedResultRequest
    {
        public int? PIndex { get; set; }
        public int? PSize { get; set; }
        [Range(1, CourseManagerConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public PagedInputDto()
        {
            MaxResultCount = CourseManagerConsts.DefaultPageSize;
        }
    }
}
