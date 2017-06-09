using System;

namespace CourseManager.Category.Dtos
{
    /// <summary>
    /// 这里不需要分页，因为都是少量数据才存这个表
    /// </summary>
    public class CategorysInput
    {
        public string CategoryName { get; set; }
        public string DictionaryValue { get; set; }
        public string ParentId { get; set; }
        public Nullable<int> Level { get; set; }
        public string CategoryType { get; set; }
        public Nullable<int> SysDefined { get; set; }

    }
}
