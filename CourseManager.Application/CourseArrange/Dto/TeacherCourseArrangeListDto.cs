using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace CourseManager.CourseArrange.Dto
{
    [AutoMapFrom(typeof(TeacherCourseArrange))]
    public class TeacherCourseArrangeListDto : EntityDto<string>
    {
        /// <summary>
        /// 必须选择老师 因为要知道给哪个老师安排课
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 必须选择上课类型（不同类型 如1v1和班级课 课时费不同）
        /// </summary>
        public string ClassType { get; set; }
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
        public string StudentId { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int TenantId { get; set; }
        public string Remark { get; set; }
    }
}
