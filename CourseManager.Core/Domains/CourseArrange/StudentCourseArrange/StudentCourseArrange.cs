using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.CourseArrange
{
    public class StudentCourseArrange : Entity<string>, IHasCreationTime
    {

        #region length
        public const short MaxIdLength = 36;
        public const short MaxRemarkLength = 1024;
        public const short MaxCategoryTypeLength = 64;
        public const short MaxCourseStatusLength = 64;
        #endregion

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

        /// <summary>
        /// 上课地点类型 （学院上课/外派）
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string CourseAddressType { get; set; }
        [Required]
        /// <summary>
        /// 如果是选择了外派就必须填写具体的上课地点 如东角头/学院
        /// </summary>
        public string Address { get; set; }

        public DateTime? ArrangeTime { get; set; }

        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 必须选择学生 因为要知道是安排给哪个学生上课 如果是班级课 那么是选择多个学生用逗号,隔开填写
        /// </summary>
        [Required]
        public string StudentId { get; set; }
        /// <summary>
        /// 对应系统存储的学生中文名
        /// </summary>
        [MaxLength(MaxIdLength)]
        public string StudentName { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int TenantId { get; set; }
        /// <summary>
        ///  约定 必填 按照模板 如：学生名 上课时间 在哪里 什么类型的课上什么课 （eg:崔俊燮 下午3点到4点 在蛇口 1v1上课 TSC）
        /// </summary>
        [Required]
        [MaxLength(MaxRemarkLength)]
        public string Remark { get; set; }
        /// <summary> 
        /// 上课状态 刚刚创建的时候为默认值 签到之后更新为Effective
        /// </summary>
        [Required]
        [MaxLength(MaxCourseStatusLength)]
        public string CourseStatus { get; set; }
        public StudentCourseArrange()
        {
            CourseStatus = "Default";
            TenantId = 1;
        }
    }
}
