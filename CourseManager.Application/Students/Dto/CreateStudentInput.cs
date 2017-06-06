using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using CourseManager.Users;

namespace CourseManager.Students.Dto
{
    [AutoMap(typeof(Student.Students))]
    public class CreateStudentInput
    {
        [Required]
        [StringLength(32)]
        public string CnName { get; set; }

        [Required]
        [StringLength(32)]
        public string EnName { get; set; }

        [Required]
        [StringLength(128)]
        public string LocalCountryName { get; set; }

        [StringLength(32)]
        public string Position { get; set; }

        public int Age { get; set; }

        public bool IsActive { get; set; }
    }
}