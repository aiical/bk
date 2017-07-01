using Abp.AutoMapper;
using PagedInputDto.Dto;
using System;

namespace CourseManager.ClassHourStatistics.Dto
{
    public class ClassHourStatisticsInput : PagedSortedAndFilteredInputDto
    {

        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        ///// <summary>
        ///// 签到时间
        ///// </summary>
        //public DateTime CreationTime { get; set; }

        /// <summary>
        /// 目前系统仅仅是给自己用 老师数据没有建立默认就是 我们自己一个 老师 
        /// </summary>

        public string TeacherName { get; set; }

        public long? TeacherId { get; set; }
        public string StudentId { get; set; }
    }
}
