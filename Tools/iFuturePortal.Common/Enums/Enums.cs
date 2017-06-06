using System;

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
       TeacherPic,
       /// <summary>
       /// 成功案例学生的头像
       /// </summary>
        SuccessCasePic,
        /// <summary>
        /// 成功案例录取通知书
        /// </summary>
        SuccessCaseAdmission
    }

    /// <summary>
    /// 通用分类集合数据表的分类标识枚举
    /// 对应数据库Categorys表
    /// </summary>
    public enum EnumCategoryType
    {
        /// <summary>
        /// 国家学校等级
        /// </summary>
        CountrySchoolLevel,
        /// <summary>
        /// Banner类型（如：首页 ，美高）
        /// </summary>
        BannerType
    }
}
