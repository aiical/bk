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

        #endregion


    }
}