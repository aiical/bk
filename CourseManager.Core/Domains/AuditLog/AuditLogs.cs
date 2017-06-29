using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.AuditLog
{
    public class AuditLogs : Entity<long>
    {
        public int TenantId { get; set; }
        public long UserId { get; set; }
        [MaxLength(256)]
        public string ServiceName { get; set; }
        [MaxLength(256)]
        public string MethodName { get; set; }
        [MaxLength(1024)]
        public string Parameters { get; set; }
        public string ClientName { get; set; }
        [MaxLength(256)]
        public string BrowserInfo { get; set; }

        public DateTime ExecutionTime { get; set; }
        [MaxLength(64)]
        public string ClientIpAddress { get; set; }
        [MaxLength(2000)]
        public string Exception { get; set; }

        public long ImpersonatorUserId { get; set; }
        public int ImpersonatorTenantId { get; set; }
        [MaxLength(2000)]
        public string CustomData { get; set; }
    }
}
