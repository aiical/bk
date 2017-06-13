using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Student
{
    public class Students : Entity<string>, IHasCreationTime
    {

        #region 字段长度声明 方便前端统一验证
        public const int MaxCnNameLength = 36;
        public const int MaxEnNameLength = 128;
        public const int MaxCountryNameLength = 128;
        public const int MaxPositionLength = 36;
        public const int MaxMobileLength = 36;
        public const int MaxLocalCountryNameLength = 128;
        public const int MaxWeChatLength = 128;
        #endregion
        public Students()
        {
            CreationTime = Clock.Now;
            TenantId = 1;
            IsActive = true;
            IsDeleted = false;
        }
        /// <summary>
        /// 中文名
        /// </summary>
        [Required]
        [MaxLength(MaxCnNameLength)]
        public string CnName { get; set; }

        [MaxLength(MaxEnNameLength)]
        public string EnName { get; set; }
        /// <summary>
        /// 学生是哪个国家的人
        /// </summary>
        [MaxLength(MaxCountryNameLength)]
        public string CountryName { get; set; }
        [MaxLength(MaxMobileLength)]
        public string Mobile { get; set; }
        /// <summary>
        /// 性别男1女2
        /// </summary>
        public int Sex { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 工作职位（可选）
        /// </summary>
        [MaxLength(MaxPositionLength)]
        public string Position { get; set; }
        /// <summary>
        /// 学生本国语言姓名
        /// </summary>
        [MaxLength(MaxLocalCountryNameLength)]
        public string LocalCountryName { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(MaxWeChatLength)]
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [MaxLength(512)]
        public string Extend1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        [MaxLength(512)]
        public string Extend2 { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
