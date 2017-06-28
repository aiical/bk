using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Net.Mail;
using CourseManager.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.HangfireTest
{
    /// <summary>
    /// 注入了user仓储（为了获得用户信息）和email发送者（发送邮件的服务），然后简单地发送了该邮件
    /// </summary>
    public class SimpleSendEmailJob : BackgroundJob<SimpleSendEmailJobArgs>, ITransientDependency
    {


        private readonly IRepository<User, long> _userRepository;
        private readonly IEmailSender _emailSender;

        public SimpleSendEmailJob(IRepository<User, long> userRepository, IEmailSender emailSender)
        {
            this._userRepository = userRepository;
            this._emailSender = emailSender;
        }
        public override void Execute(SimpleSendEmailJobArgs args)
        {
            var senderUser = _userRepository.Get(args.SenderUserId.Value);
            var targetUser = _userRepository.Get(args.TargetUserId);
            _emailSender.Send(senderUser.EmailAddress, targetUser.EmailAddress, args.Subject, args.Body);
        }
    }
    /// <summary>
    /// 参数应该是serializable(可序列化)，因为要将它 序列化并存储到数据库中。虽然ABP默认的后台工作管理者使用了JSON序列化（它不需要[Serializable]特性），但是最好定义 [Serializable]特性，因为我们将来可能会转换到其他使用二进制序列化的工作管理者
    /// </summary>
    [Serializable]
    public class SimpleSendEmailJobArgs
    {
        public long? SenderUserId { get; set; }

        public long TargetUserId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
    public class TestJob : BackgroundJob<int>, ITransientDependency
    {
        public override void Execute(int number)
        {
            Logger.Debug(number.ToString());
        }
    }

    /// <summary>
    /// 添加一个新工作队列
    /// </summary>
    public class MyService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        public MyService(IBackgroundJobManager backgroundJobManger)
        {
            _backgroundJobManager = backgroundJobManger;
        }
        /// <summary>
        /// 当入队（Enqueue）时，我们将42作为参数传递。IBackgroundJobManager将会实例化并使用42作为参数执行TestJob
        /// </summary>
        public void Test()
        {
            _backgroundJobManager.Enqueue<TestJob, int>(42);
        }
    }
}
