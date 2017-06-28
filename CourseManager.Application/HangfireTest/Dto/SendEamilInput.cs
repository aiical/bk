using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.HangfireTest.Dto
{
    public class SendEamilInput
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public long SenderUserId { get; set; }
        public long TargetUserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
