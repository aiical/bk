using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using CourseManager.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.HangfireTest
{
    /// <summary>
    /// 后台工作者生命周期后台工作者一般实现为单例的，但是没有严格限制。如果你需要相同工作者类的多个实例，那么可以使它成为transient（每次使用时创建），然后给IBackgroundWorkerManager添加多个实例。在这种情况下，你的工作者很可能会参数化（比如，你有单个LogCleaner类，但是两个LogCleaner工作者实例会监视并清除不同的log文件夹）。
    /// </summary>
    public class MakeInactiveUsersPassiveWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<User, long> _userRepository;

        public MakeInactiveUsersPassiveWorker(AbpTimer timer, IRepository<User, long> userRepository)
            : base(timer)
        {
            _userRepository = userRepository;
            Timer.Period = 5000; //5 seconds (good for tests, but normally will be more)
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var oneMonthAgo = Clock.Now.Subtract(TimeSpan.FromDays(30));

                var inactiveUsers = _userRepository.GetAllList(u =>
                    u.IsActive &&
                    ((u.LastLoginTime < oneMonthAgo && u.LastLoginTime != null) || (u.CreationTime < oneMonthAgo && u.LastLoginTime == null))
                    );

                foreach (var inactiveUser in inactiveUsers)
                {
                    inactiveUser.IsActive = false;
                    Logger.Info(inactiveUser + " made passive since he/she did not login in last 30 days.");
                }

                CurrentUnitOfWork.SaveChanges();
            }
        }
    }
}
