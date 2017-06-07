using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.Students.Dto
{
    [AutoMap(typeof(Student.Students))]
    public class CreateStudentInput
    {
        public string Id { get; set; }
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
        public string Mobile { get; set; }

        public int Age { get; set; }

        public string CountryName { get; set; }

        public bool IsActive { get; set; }
       
    }
}