using Abp.Application.Services.Dto;

namespace CourseManager.Dto
{
    /// <summary>
    ///支持分页、排序的InputDto
    /// </summary>
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = CourseManagerConsts.DefaultPageSize;
        }
    }
}
