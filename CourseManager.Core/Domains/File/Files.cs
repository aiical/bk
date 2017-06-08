using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.File
{
    public class Files : Entity, IHasCreationTime
    {
        #region length
        public const short MaxIdLength = 36;
        public const short MaxFileNameLength = 128;
        public const short MaxUriLength = 255;
        public const short MaxCategoryTypeLength = 32;
        public const short MaxLinkUrlLength = 255;
        public const short MaxDescriptionLength = 128;

        #endregion

        [MaxLength(MaxIdLength)]
        public string FolderId { get; set; }
        [MaxLength(MaxFileNameLength)]
        public string FileName { get; set; }
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        [MaxLength(MaxCategoryTypeLength)]
        public string CategoryType { get; set; }
        [MaxLength(MaxUriLength)]
        public string Url { get; set; }

        [MaxLength(MaxUriLength)]
        public string OldUrl { get; set; }
        public int Click { get; set; }
        public int SortNo { get; set; }
        public bool? IsTop { get; set; }
        public DateTime? SetTopTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public long? CreatorUserId { get; set; }
        public int FileSize { get; set; }
        [MaxLength(MaxIdLength)]
        public string RelateId { get; set; }
        public int TenantId { get; set; }

        public Files()
        {
            CreationTime = Clock.Now;
            TenantId = 1;
            IsActive = true;
            IsDeleted = false;
            Click = 0;
        }

    }
}
