using Abp.AutoMapper;
using PagedInputDto.Dto;
using System;

namespace CourseManager.SignIn.Dto
{
    [AutoMap(typeof(SignInRecord))]
    public class CreateSignInInput : PagedSortedAndFilteredInputDto
    {
        public string Id { get; set; }
        public string TeacherId { get; set; }
        /// <summary>
        /// 当前给哪个学生上课
        /// </summary>
        public string StudentId { get; set; }
        /// <summary>
        /// 上课签到类型（迟到，正常，未上课（如果是迟到或者未上课 请说明原因））
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 上课地点类型（如外派/学院）（因为外派会有车费补贴 和工资统计有关）
        /// </summary>
        public string CourseAddressType { get; set; }
        public string ClassType { get; set; }
        public string CourseType { get; set; }
        /// <summary>
        /// 当有请假的时候 请选择相应的类型（如果上课类型为非正常的时候 必须选择是学生请假 还是老师自己请假 和后期统计工资有关）
        /// </summary>
        public string UnNormalType { get; set; }
        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        public long? CreatorUserId { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public int TenantId { get; set; }
        /// <summary>
        /// 如果迟到或者旷课 请填写原因说明
        /// </summary>
        public string Reason { get; set; }
        public string Address { get; set; }

        public string Remark { get; set; }
    }
}
