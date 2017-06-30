
using System;

namespace CourseManager.Utils.Import.Models
{
    /// <summary>
    /// 学生课程ExcelModel
    /// </summary>
    public class ImportStudentCourseArrangeModel
    {
        public string StudentName { get; set; }
        public string StudentId { get; set; }
        public string ClassType { get; set; }
        public string CourseType { get; set; }
        public string CourseAddressType { get; set; }

        public string Address { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Remark { get; set; }
    }
}
