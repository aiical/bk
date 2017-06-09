using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseManager.CourseArrange
{
    public class TeacherCourseArrangeEventData : EventData
    {
        public TeacherCourseArrange TeacherCourseArrange { get; set; }
    }
}
