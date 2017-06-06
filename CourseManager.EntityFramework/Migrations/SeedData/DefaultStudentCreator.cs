using Abp.Localization;
using Abp.Timing;
using CourseManager.EntityFramework;
using CourseManager.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Migrations.SeedData
{
    public class DefaultStudentCreator
    {
        private readonly CourseManagerDbContext _context;

        public DefaultStudentCreator(CourseManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateStudents();
        }

        private void CreateStudents()
        {
            var stu = new Students
            {
                Id = Guid.NewGuid().ToString(),
                CnName = "张三",
                EnName = "ZhangSan",
                LocalCountryName = "장삼",
                Age = 29,
                Sex = 1,
                CountryName = "韩国",
                CreationTime = Clock.Now,
                IsActive = true,
                Mobile = "15878985888",
                Position = "经理"
            };
            if (_context.Students.Any(a => a.CnName == stu.CnName) == false)
            {
                _context.Students.Add(stu);
                _context.SaveChanges();
            }
        }
    }
}
