using Abp.Application.Services;
using Abp.BackgroundJobs;
using CourseManager.HangfireTest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.HangfireTest
{
    public class MyEmailAppService: ApplicationService,IMyEmailAppService
    {
        private readonly IBackgroundJobManager _backgroundJobManger;
        public MyEmailAppService(IBackgroundJobManager backgroundJobManger)
        {
            this._backgroundJobManger = backgroundJobManger;
        }

        public async Task SendEmail(SendEamilInput input)
        {
            //Enqueue (或 EnqueueAsync)方法还有其他的参数，比如 priority和 delay（优先级和延迟）。
            await _backgroundJobManger.EnqueueAsync<SimpleSendEmailJob, SimpleSendEmailJobArgs>(
            new SimpleSendEmailJobArgs
            {
                Subject = input.Subject,
                Body = input.Body,
                SenderUserId = AbpSession.UserId,
                TargetUserId = input.TargetUserId
            });
        }
    }
}
