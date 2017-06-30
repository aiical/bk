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
    public interface IStudentCourseArrangeAppService : IApplicationService
    {
        /// <summary>
        /// 排课
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool StudentArrangeCourse(CreateStudentCourseArrangeInput input);
        StudentCourseArrange GetArranage(StudentCourseArrangeInput input);

        Task<ListResultDto<StudentCourseArrangeListDto>> GetArranagesAsync();
        ListResultDto<StudentCourseArrangeListDto> GetArranages(StudentCourseArrangeInput input);

        PagedResultDto<StudentCourseArrangeListDto> GetPagedArranges(StudentCourseArrangeInput input);
        /// <summary>
        /// 获取签到用的课程安排时间段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<StudentCourseArrange2SignInOutput> GetStudentCourseArrange2SignIn(StudentCourseArrangeInput input);

        bool UpdateCourseArrange(UpdateStudentCourseArrangeInput updateInput);
        bool UpdateCourseArrange(StudentCourseArrange updateModel);

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        void BatchInsert(List<CreateStudentCourseArrangeInput> list);
    }
}
