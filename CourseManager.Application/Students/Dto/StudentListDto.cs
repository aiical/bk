using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace CourseManager.Students.Dto
{
    [AutoMapFrom(typeof(Student.Students))]
    public class StudentListDto : EntityDto<long>
    {
        public string CnName { get; set; }

        public string EnName { get; set; }

        public string LocalCountryName { get; set; }

        public int Sex { get; set; }

        public string Position { get; set; }

        public int Age { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
    }
}