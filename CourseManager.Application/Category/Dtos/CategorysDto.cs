using System;

namespace CourseManager.Category.Dtos
{
    public class CategorysDto
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string DictionaryValue { get; set; }
        public string ParentId { get; set; }
        public Nullable<int> Level { get; set; }
        public string CategoryType { get; set; }
        public Nullable<int> SysDefined { get; set; }
        public Nullable<int> SortNo { get; set; }
        public string Description { get; set; }
    }
}
