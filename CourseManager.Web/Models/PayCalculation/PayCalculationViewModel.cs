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
        /// <summary>
        /// 学生当前月上课是否80%达标，防止学生随意请假，影响老师收益（如果学生当月需要上课20小时，然后请假次数太多导致不够80% 也就是不够16个小时 那么缺少的课时按照正常课时的80%费用收取 如仅仅上了8个小时，那么就是缺少8个小时也就是需要补贴老师8*50*0.8=320元） 这里暂只处理 1对1
        /// </summary>
        public decimal? StudentAbsentFees { get; set; }
        /// <summary>
        /// 费用说明
        /// </summary>
        public string StudentAbsentFeeDes { get; set; }
    }
    /// <summary>
    /// 统计学生请假
    /// </summary>
    public class PayCalculation2StuViewModel
    {
        /// <summary>
        /// 应上课时
        /// </summary>
        public decimal? TotalDuration { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        /// <summary>
        /// 实际课时
        /// </summary>
        public decimal? RealDuration { get; set; }
        /// <summary>
        /// 差额
        /// </summary>
        public decimal? Balance { get; set; }
    }
}