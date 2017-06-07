using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using CourseManager.Authorization;
using CourseManager.Users.Dto;
using Microsoft.AspNet.Identity;
using CourseManager.Students;
using CourseManager.Students.Dto;
using System;

namespace CourseManager.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class StudentAppService : CourseManagerAppServiceBase, IStudentAppService
    {
        private readonly IRepository<Student.Students, string> _studentRepository;

        public StudentAppService(IRepository<Student.Students, string> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<ListResultDto<StudentListDto>> GetStudents()
        {
            var stus = await _studentRepository.GetAllListAsync();

            return new ListResultDto<StudentListDto>(
                stus.MapTo<List<StudentListDto>>()
                );
        }

        public async Task CreateStudent(CreateStudentInput input)
        {
            var student = input.MapTo<Student.Students>();
            await _studentRepository.InsertAsync(student);
        }
    }
}