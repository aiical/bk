using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CourseManager.SignIn.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.SignIn
{
    public interface ISignInRecordAppService : IApplicationService
    {
        /// <summary>
        /// 添加签到记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateSignInRecord(CreateSignInInput input);
        /// <summary>
        /// 获取某条签到记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        SignInRecord GetSignIn(SignInInput input);

        /// <summary>
        /// 异步获取签到记录列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<SignInListDto>> GetSignInRecordsAsync();
        /// <summary>
        /// 获取签到记录列表
        /// </summary>
        /// <returns></returns>
        ListResultDto<SignInListDto> GetSignInRecords();
        /// <summary>
        /// 获取分页签到记录列表
        /// </summary>
        /// <returns></returns>
        PagedResultDto<SignInListDto> GetPagedSignInRecords(SignInInput input);
    }
}
