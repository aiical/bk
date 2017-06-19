using CourseManager.ClassHourStatistics.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManager.Web.Models.PayCalculation
{
    /// <summary>
    /// 教师工资统计 前台展示
    /// </summary>
    public class PayCalculationViewModel
    {
        /// <summary>
        /// 1v1课时数
        /// </summary>
        public decimal? One2OneDuration { get; set; }
        /// <summary>
        /// 班级课课时数
        /// </summary>
        public decimal? ClassDuration { get; set; }
        /// <summary>
        /// 续学人数（当前月 某个学生学时达到15小时）
        /// </summary>
        public int RenewNum { get; set; }
        /// <summary>
        /// 续学奖金
        /// </summary>
        public decimal? RenewFee { get; set; }

        /// <summary>
        /// 当前月根据老师课时数 计算老师基础工资（底薪：20小时以下2000元，20小时～40小时3000元，40以上4000元）
        /// </summary>
        public decimal BasicSalary { get; set; }

        /// <summary>
        /// 总课时数
        /// </summary>
        public decimal? TotalDuration { get; set; }

        public string TeacherName { get; set; }
        /// <summary>
        /// 早课次数（时间小于等于早上七点半）
        /// </summary>
        public int EarlyCourseTimes { get; set; }
        /// <summary>
        /// 晚课次数（时间大于等于晚上七点）
        /// </summary>
        public int NightCourseTimes { get; set; }
        /// <summary>
        /// 课程外派次数
        /// </summary>
        public int AssignmentTimes { get; set; }

        /// <summary>
        /// 坐班时间 坐班时间达到规定时间 会有全勤奖200元
        /// </summary>
        public decimal? OfficeHours { get; set; }
        /// <summary>
        /// 全勤奖
        /// </summary>
        public decimal? AllOfficeHoursBonus { get; set; }
    }
}