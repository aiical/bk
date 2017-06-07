using PagedInputDto.Dto;

namespace CourseManager.Students.Dto
{
    public class StudentInput : PagedSortedAndFilteredInputDto
    {
        public string Id { get; set; }
        public bool? IsActive { get; set; }
        public string CnName { get; set; }
        public StudentInput()
        {

        }
        public StudentInput(int? page, int? pSize)
        {
            var pageSize = pSize ?? 10;
            var pageNumber = page ?? 1;
            base.SkipCount = (pageNumber - 1) * pageSize;
            base.MaxResultCount = pageSize;
        }
    }
}
