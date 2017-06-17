using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CourseManager.CourseArrange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.CourseArrange
{
    public interface ITeacherCourseArrangeAppService : IApplicationService
    {
        /// <summary>
        /// 排课
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool TeacherArrangeCourse(CreateTeacherCourseArrangeInput input);
        TeacherCourseArrange GetArranage(TeacherCourseArrangeInput input);

        Task<ListResultDto<TeacherCourseArrangeListDto>> GetArranagesAsync();
        ListResultDto<TeacherCourseArrangeListDto> GetArranages(TeacherCourseArrangeInput input);

        PagedResultDto<TeacherCourseArrangeListDto> GetPagedArranges(TeacherCourseArrangeInput input);
        /// <summary>
        /// 获取签到用的课程安排时间段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<TeacherCourseArrange2SignInOutput> GetTeacherCourseArrange2SignIn(TeacherCourseArrangeInput input);

        bool UpdateCourseArrange(UpdateTeacherCourseArrangeInput updateInput);
        bool UpdateCourseArrange(TeacherCourseArrange updateModel);
    }
}
