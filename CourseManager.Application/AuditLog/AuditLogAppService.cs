using Abp.Domain.Repositories;
using CourseManager.AuditLog;
using CourseManager.AuditLog.Dto;
using System;

namespace CourseManager.AuditLog
{
    /// <summary>
    /// �����־ ����
    /// </summary>
    public class AuditLogAppService : CourseManagerAppServiceBase, IAuditLogAppService
    {
        private readonly IRepository<AuditLogs, long> _auditLogRepository;

        public AuditLogAppService(IRepository<AuditLogs, long> auditLogRepository)
        {
            this._auditLogRepository = auditLogRepository;
        }
        /// <summary>
        /// ɾ�������־
        /// </summary>
        /// <param name="update"></param>
        public void DeleteLog(UpdateAuditLogInput update)
        {
            _auditLogRepository.Delete(a => a.ExecutionTime < DateTime.Now.AddDays(-7));//ɾ��һ��ǰ��־
        }
    }
}