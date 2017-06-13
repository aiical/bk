using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Students.Dto
{
    public class StudentUpdateInput: ICustomValidate
    {
        public string Id { get; set; }
        [Required]
        public string CnName { get; set; }

        public string EnName { get; set; }

        public string LocalCountryName { get; set; }

        public int Sex { get; set; }

        public string Position { get; set; }

        public int Age { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }
        public string CountryName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
        public void AddValidationErrors(CustomValidationContext context)
        {

        }
    }
}
