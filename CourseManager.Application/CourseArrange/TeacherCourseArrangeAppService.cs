using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CourseManager.CourseArrange.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.AutoMapper;
using CourseManager.Common;

namespace CourseManager.CourseArrange
{
    public class TeacherCourseArrangeAppService : CourseManagerAppServiceBase, ITeacherCourseArrangeAppService
    {
        private readonly IRepository<TeacherCourseArrange, string> _teacherCourseArrangeRepository;

        public TeacherCourseArrangeAppService(IRepository<TeacherCourseArrange, string> teacherCourseArrangeRepository)
        {
            this._teacherCourseArrangeRepository = teacherCourseArrangeRepository;
        }
        private IQueryable<TeacherCourseArrange> GetArrangesByCondition(TeacherCourseArrangeInput input)
        {
            //WhereIf 是ABP针对IQueryable<T>的扩展方法 第一个参数为条件，第二个参数为一个Predicate 当条件为true执行后面的条件过滤
            var query = _teacherCourseArrangeRepository.GetAll()
                        .WhereIf(!input.Id.IsNullOrEmpty(), o => o.Id == input.Id)
                        .WhereIf(!input.Filter.IsNullOrEmpty(), t => t.Type == input.Filter)
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderByDescending(t => t.CreationTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }
        public async Task<ListResultDto<TeacherCourseArrangeListDto>> GetArranagesAsync()
        {
            var stus = await _teacherCourseArrangeRepository.GetAllListAsync();

            return new ListResultDto<TeacherCourseArrangeListDto>(
                stus.MapTo<List<TeacherCourseArrangeListDto>>()
                );
        }

        public ListResultDto<TeacherCourseArrangeListDto> GetArranages()
        {
            var stus = _teacherCourseArrangeRepository.GetAllListAsync();

            return new ListResultDto<TeacherCourseArrangeListDto>(
                stus.MapTo<List<TeacherCourseArrangeListDto>>()
                );
        }
        public TeacherCourseArrange GetArranage(TeacherCourseArrangeInput input)
        {
            return _teacherCourseArrangeRepository.Get(input.Id);
        }

        public PagedResultDto<TeacherCourseArrangeListDto> GetPagedArranges(TeacherCourseArrangeInput input)
        {
            var query = GetArrangesByCondition(input);
            var count = query.Count();
            input.SkipCount = (input.PIndex ?? 1 - 1) * input.PSize ?? 10;
            input.MaxResultCount = input.PSize ?? 10;
            var list = query.PageBy(input).ToList();

            return new PagedResultDto<TeacherCourseArrangeListDto>(count, list.MapTo<List<TeacherCourseArrangeListDto>>());
        }

        /// <summary>
        /// 排课
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task TeacherArrangeCourse(CreateTeacherCourseArrangeInput input)
        {
            var arrange = input.MapTo<TeacherCourseArrange>();
            if (!string.IsNullOrEmpty(input.Id)) await _teacherCourseArrangeRepository.UpdateAsync(arrange);
            else
            {
                arrange.Id = IdentityCreator.NewGuid;
                await _teacherCourseArrangeRepository.InsertAsync(arrange);
            }
        }

    }
}
