using Abp.Application.Services;
using CourseManager.AuditLog.Dto;

namespace CourseManager.AuditLog
{
    /// <summary>
    /// �����־ ����
    /// </summary>
    public interface IAuditLogAppService : IApplicationService
    {
        /// <summary>
        /// ɾ�������־
        /// </summary>
        /// <param name="update"></param>
        void DeleteLog(UpdateAuditLogInput update);
    }
}