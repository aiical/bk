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
        Task CreateSignInRecord(CreateSignInInput input);
        SignInRecord GetSignIn(SignInInput input);

        Task<ListResultDto<SignInListDto>> GetSignInRecordsAsync();
        ListResultDto<SignInListDto> GetSignInRecords();

        PagedResultDto<SignInListDto> GetPagedSignInRecords(SignInInput input);
    }
}
