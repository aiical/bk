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
        /// <summary>
        /// ÄÐ1 Å®2
        /// </summary>
        public int Sex { get; set; }

        [Required]
        [StringLength(32)]
        public string EnName { get; set; }

        [Required]
        [StringLength(128)]
        public string LocalCountryName { get; set; }

        [StringLength(32)]
        public string Position { get; set; }
        public string Mobile { get; set; }

        /// <summary>
        /// Î¢ÐÅ
        /// </summary>
        public string WeChat { get; set; }

        public int Age { get; set; }

        public string CountryName { get; set; }

        public bool IsActive { get; set; }
       
    }
}