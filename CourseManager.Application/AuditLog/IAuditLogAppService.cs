using Abp.Application.Services;
using CourseManager.AuditLog.Dto;

namespace CourseManager.AuditLog
{
    /// <summary>
    /// 审计日志 服务
    /// </summary>
    public interface IAuditLogAppService : IApplicationService
    {
        /// <summary>
        /// 删除审计日志
        /// </summary>
        /// <param name="update"></param>
        void DeleteLog(UpdateAuditLogInput update);
    }
}