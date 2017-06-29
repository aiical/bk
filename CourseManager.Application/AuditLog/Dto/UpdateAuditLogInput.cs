using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.AuditLog.Dto
{
    /// <summary>
    /// 审计日志 处理
    /// </summary>
    public class UpdateAuditLogInput
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
    }
}
