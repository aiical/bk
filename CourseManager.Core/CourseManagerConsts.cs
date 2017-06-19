namespace CourseManager
{
    public class CourseManagerConsts
    {
        public const string LocalizationSourceName = "CourseManager";

        public const bool MultiTenancyEnabled = true;
        /// <summary>
        /// 不启用多租户
        /// </summary>
        public const bool MultiTenancyUnEnabled = false;

        public const int MaxPageSize = 200;
        public const int DefaultPageSize = 10;

        #region 业务相关
        /// <summary>
        /// 每个月的全勤时间可能都会有变化  注意对应修改
        /// </summary>
        public const decimal JanuaryOfficeHours = 180;
        public const decimal FebruaryOfficeHours = 180;
        public const decimal MarchOfficeHours = 180;
        public const decimal AprilOfficeHours = 180;
        public const decimal MayOfficeHours = 180;
        public const decimal JuneOfficeHours = 180;
        public const decimal JulyOfficeHours = 180;
        public const decimal AugustOfficeHours = 180;
        public const decimal SeptemberOfficeHours = 180;
        public const decimal OctoberOfficeHours = 180;
        public const decimal NovemberOfficeHours = 180;
        public const decimal DecemberOfficeHours = 180;
        /// <summary>
        /// 全勤奖金
        /// </summary>
        public const decimal AllOfficeHoursBonus = 200;
        /// <summary>
        /// 外派补助
        /// </summary>
        public const decimal PerExternalAssignmentAllowance = 10;
        /// <summary>
        /// 早课补助
        /// </summary>
        public const decimal PerMorningAllowance = 5;
        /// <summary>
        /// 达到坐班时间 补助
        /// </summary>
        public const decimal ReachOfficeHourAllowance = 200;
        /// <summary>
        /// 一对一课时费
        /// </summary>
        public const decimal One2OneClassFees = 50;

        /// <summary>
        /// 班级课课时费
        /// </summary>
        public const decimal ClassCourseFees = 70;

        /// <summary>
        /// 学生续学（当前月学生上课达到15个小时） 奖金
        /// </summary>
        public const decimal RenewBonus = 25;

        /// <summary>
        /// 早课时间边界
        /// </summary>
        public const int EarlyHour = 8;
        /// <summary>
        /// 晚课时间边界
        /// </summary>
        public const int NigthHour = 19;
        /// <summary>
        /// 到底70个小时 底薪为4000 底薪说明：20小时以下2000元，20小时～40小时3000元，40以上4000元
        /// </summary>
        public const decimal BasicSalary = 4000;
        /// <summary>
        /// 拿到全额底薪的课时数
        /// </summary>
        public const int BasicSalaryHours = 70;
        #endregion


    }
}