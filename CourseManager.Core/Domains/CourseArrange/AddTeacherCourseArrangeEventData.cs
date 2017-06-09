using Abp.Events.Bus;
using CourseManager.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.CourseArrange
{
    public class AddTeacherCourseArrangeEventData : TeacherCourseArrangeEventData
    {
        public User User { get; set; }
        public AddTeacherCourseArrangeEventData(TeacherCourseArrange arrange, User user)
        {
            this.TeacherCourseArrange = arrange;
            this.User = user;
        }
    }
}
