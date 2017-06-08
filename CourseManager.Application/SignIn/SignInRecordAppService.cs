using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CourseManager.SignIn.Dto;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.AutoMapper;
using CourseManager.Common;

namespace CourseManager.SignIn
{
    public class SignInRecordAppService : CourseManagerAppServiceBase, ISignInRecordAppService
    {

        private readonly IRepository<SignInRecord, string> _signInRepository;

        public SignInRecordAppService(IRepository<SignInRecord, string> signInRepository)
        {
            this._signInRepository = signInRepository;
        }

        private IQueryable<SignInRecord> GetArrangesByCondition(SignInInput input)
        {
            var query = _signInRepository.GetAll()
                        .WhereIf(!input.Id.IsNullOrEmpty(), o => o.Id == input.Id)
                        .WhereIf(!input.Filter.IsNullOrEmpty(), t => t.Type == input.Filter)
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderByDescending(t => t.CreationTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }
        public async Task CreateSignInRecord(CreateSignInInput input)
        {
            var record = input.MapTo<SignInRecord>();
            if (!string.IsNullOrEmpty(input.Id)) await _signInRepository.UpdateAsync(record);
            else
            {
                record.Id = IdentityCreator.NewGuid;
                await _signInRepository.InsertAsync(record);
            }
        }

        public PagedResultDto<SignInListDto> GetPagedSignInRecords(SignInInput input)
        {
            var query = GetArrangesByCondition(input);
            var count = query.Count();
            input.SkipCount = (input.PIndex ?? 1 - 1) * input.PSize ?? 10;
            input.MaxResultCount = input.PSize ?? 10;
            var list = query.PageBy(input).ToList();

            return new PagedResultDto<SignInListDto>(count, list.MapTo<List<SignInListDto>>());
        }

        public SignInRecord GetSignIn(SignInInput input)
        {
            return _signInRepository.Get(input.Id);
        }

        public ListResultDto<SignInListDto> GetSignInRecords()
        {
            var stus = _signInRepository.GetAllListAsync();

            return new ListResultDto<SignInListDto>(
                stus.MapTo<List<SignInListDto>>()
                );
        }

        public async Task<ListResultDto<SignInListDto>> GetSignInRecordsAsync()
        {
            var stus = await _signInRepository.GetAllListAsync();

            return new ListResultDto<SignInListDto>(
                stus.MapTo<List<SignInListDto>>()
                );
        }
    }
}
