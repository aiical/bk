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
        [MaxLength(36)]
        public string CnName { get; set; }

        [MaxLength(128)]
        public string EnName { get; set; }
        /// <summary>
        /// 学生是哪个国家的人
        /// </summary>
        [MaxLength(128)]
        public string CountryName { get; set; }
        [MaxLength(36)]
        public string Mobile { get; set; }
        /// <summary>
        /// 性别男1女2
        /// </summary>
        public int Sex { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 工作职位（可选）
        /// </summary>
        [MaxLength(36)]
        public string Position { get; set; }
        /// <summary>
        /// 学生本国语言姓名
        /// </summary>
        [MaxLength(128)]
        public string LocalCountryName { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
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
