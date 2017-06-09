using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.CourseArrange
{
    public class TeacherCourseArrange : Entity<string>, IHasCreationTime
    {

        #region length
        public const short MaxIdLength = 36;
        public const short MaxRemarkLength = 1024;
        public const short MaxCategoryTypeLength = 64;

        #endregion
        /// <summary>
        /// 必须选择老师 因为要知道给哪个老师安排课
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string TeacherId { get; set; }
        /// <summary>
        /// 必须选择上课类型（不同类型 如1v1和班级课 课时费不同）
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string ClassType { get; set; }


        /// <summary>
        /// 必须选择课程类型（如基础入门上 入门下 口语初级等）
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string CourseType { get; set; }

        public DateTime ArrangeTime { get; set; }

        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 必须选择学生 因为要知道是安排给哪个学生上课 如果是班级课 那么是选择多个学生
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string StudentId { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int TenantId { get; set; }
        [MaxLength(MaxRemarkLength)]
        public string Remark { get; set; }

        public TeacherCourseArrange()
        {
            TenantId = 1;
        }
    }
}
