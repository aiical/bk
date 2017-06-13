using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.SignIn
{
    public class SignInRecord : Entity<string>, IHasCreationTime
    {
        #region length
        public const short MaxIdLength = 36;
        public const short MaxRemarkLength = 1024;
        public const short MaxCategoryTypeLength = 64;

        #endregion

        [Required]
        [MaxLength(MaxIdLength)]
        public string TeacherId { get; set; }
        /// <summary>
        /// 当前给哪个学生上课
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string StudentId { get; set; }
        /// <summary>
        /// 上课签到类型（迟到，正常，未上课（如果是迟到或者未上课 请说明原因））
        /// </summary>
        [Required]
        [MaxLength(MaxIdLength)]
        public string Type { get; set; }
        /// <summary>
        /// 班级课/1v1
        /// </summary>
        public string ClassType { get; set; }
        /// <summary>
        /// 课程类型 ：入门上/下
        /// </summary>
        public string CourseType { get; set; }
        /// <summary>
        /// 当有请假的时候 请选择相应的类型（如果上课类型为非正常的时候 必须选择是学生请假 还是老师自己请假 和后期统计工资有关）
        /// </summary>
        [MaxLength(MaxIdLength)]
        public string UnNormalType { get; set; }
        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 上了多少个小时 如1个半小时 就是90分钟 这里以分钟为单位
        /// </summary>
        public decimal Duration { get; set; }
        /// <summary>
        /// 上课地点 如蛇口东角头 侨城东 
        /// </summary>
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatorUserId { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public int TenantId { get; set; }
        /// <summary>
        /// 如果迟到或者旷课 请填写原因说明
        /// </summary>
        [MaxLength(MaxRemarkLength)]
        public string Reason { get; set; }

        [MaxLength(MaxRemarkLength)]
        public string Remark { get; set; }

        public SignInRecord()
        {
            TenantId = 1;
        }
    }
}
