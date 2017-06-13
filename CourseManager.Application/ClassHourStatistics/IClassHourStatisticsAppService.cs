using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CourseManager.ClassHourStatistics.Dto;
using System.Threading.Tasks;

namespace CourseManager.ClassHourStatistics
{
    public interface IClassHourStatisticsAppService : IApplicationService
    {
        /// <summary>
        /// 异步获取签到记录列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<ClassHourStatisticsOutput>> GetClassHourStatisticsAsync();
        /// <summary>
        /// 获取签到记录列表
        /// </summary>
        /// <returns></returns>
        ListResultDto<ClassHourStatisticsOutput> GetClassHourStatistics(ClassHourStatisticsInput input);
    }
}
