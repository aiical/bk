using System;
using System.ComponentModel;

namespace CourseManager.Common.Enums
{
    public enum FileTypesEnum
    {
        /// <summary>
        /// Banner
        /// </summary>
        Banner,
        /// <summary>
        /// 名师头像
        /// </summary>
        TeacherPic
    }

    /// <summary>
    /// 通用分类集合数据表的分类标识枚举
    /// 对应数据库Categorys表
    /// </summary>
    public enum EnumCategoryType
    {
        /// <summary>
        /// 签到类型（准时上课 迟到 未上课）
        /// </summary>
        SignInRecordType,
        /// <summary>
        /// 未上课原因类型
        /// </summary>
        NoCourseReasonType,
        /// <summary>
        /// 课程类型
        /// </summary>
        CourseType
    }



    /// <summary>
    ///出勤枚举
    /// </summary>
    public enum Attendence
    {
        //1：准时上课  2：迟到  3：早退  4：未上课
        [Description("准时上课")]
        Normal = 1,
        [Description("迟到")]
        Late = 2,
        [Description("早退")]
        Early = 3,
        [Description("旷课")]
        NoCourse = 4,
    }
    /// <summary>
    /// 课时长度
    /// </summary>
    public enum DurationType
    {
        //1：1小时  2：2小时
        [Description("1小时")]
        OneHour = 1,
        [Description("2小时")]
        TowHour = 2
    }

    public enum ArrageCourseStatus
    {
        [Description("预排")]//只有预排课才可以在排课时自动删除
        Default = 1,
        [Description("待放课")]
        Normal = 2,
        [Description("已放课")]//只有正式课表才可以放课
        Effective = 3,
        [Description("已取消")]//只有放课后的课才可以取消
        Cancel = 4,
        [Description("已结课")]//只有放课后的课才可以结课
        Finished = 5
    }
}
