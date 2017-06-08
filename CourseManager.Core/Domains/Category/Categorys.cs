using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.Category
{
    public class Categorys : Entity<string>, IHasCreationTime
    {
        #region length
        public const short MaxIdLength = 36;
        public const short MaxCategoryNameLength = 64;
        public const short MaxCategoryTypeLength = 64;
        public const short MaxDescriptionLength = 512;

        #endregion
        [MaxLength(MaxCategoryNameLength)]
        public string CategoryName { get; set; }
        [MaxLength(MaxCategoryNameLength)]
        public string DictionaryValue { get; set; }
        [MaxLength(MaxIdLength)]
        public string ParentId { get; set; }
        public Nullable<int> Level { get; set; }
        [MaxLength(MaxCategoryTypeLength)]
        public string CategoryType { get; set; }
        public Nullable<int> SysDefined { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> SortNo { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int TenantId { get; set; }
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public Categorys()
        {
            TenantId = 1;
        }
    }
}
