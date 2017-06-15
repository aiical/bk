using CourseManager.Category;
using CourseManager.CourseArrange;
using CourseManager.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseManager.Migrations.SeedData
{
    public class DefaultTeacherCourseArrangesCreator
    {
        private readonly CourseManagerDbContext _context;

        private readonly List<TeacherCourseArrange> _teacherCourseArrange;

        public DefaultTeacherCourseArrangesCreator(CourseManagerDbContext context)
        {
            _context = context;
            _teacherCourseArrange = new List<TeacherCourseArrange>()
            {
                //方法一：Convert.ToDateTime(string)string格式有要求，必须是yyyy-MM-dd hh:mm:ss
                //崔俊燮
                new TeacherCourseArrange{ Id=Common.IdentityCreator.NewGuid,CreationTime=DateTime.Now, CreatorUserId=0,IsDeleted=false, TenantId=1,TeacherId=1,BeginTime=new DateTime(2017,6,13,13,00,00),EndTime=new DateTime(2017,6,13,15,00,00),StudentId="c2d4e232d382474ca8ab40f9b0144767",ClassType="64c951d110044a51bd83c7e7e82f96ec",CourseType="e64269153d394e9e94ff970f2c16ae89",Remark="崔俊燮 星期二下午1点到三点 1v1 上 提高课2个小时" ,CourseAddressType="03648bb7f50442609b0936158d1b92b4",Address="学院"},
                 new TeacherCourseArrange{ Id=Common.IdentityCreator.NewGuid,CreationTime=DateTime.Now, CreatorUserId=0,IsDeleted=false, TenantId=1,TeacherId=1,BeginTime=new DateTime(2017,6,15,13,00,00),EndTime=new DateTime(2017,6,15,15,00,00),StudentId="c2d4e232d382474ca8ab40f9b0144767",ClassType="64c951d110044a51bd83c7e7e82f96ec",CourseType="e64269153d394e9e94ff970f2c16ae89",Remark="崔俊燮 星期四下午1点到三点 1v1 上 提高课2个小时",CourseAddressType="03648bb7f50442609b0936158d1b92b4",Address="学院" },

                      new TeacherCourseArrange{ Id=Common.IdentityCreator.NewGuid,CreationTime=DateTime.Now, CreatorUserId=0,IsDeleted=false, TenantId=1,TeacherId=1,BeginTime=new DateTime(2017,6,16,13,00,00),EndTime=new DateTime(2017,6,16,13,00,00),StudentId="c2d4e232d382474ca8ab40f9b0144767",ClassType="64c951d110044a51bd83c7e7e82f96ec",CourseType="e64269153d394e9e94ff970f2c16ae89",Remark="崔俊燮 星期五下午1点到三点 1v1 上 提高课2个小时",CourseAddressType="03648bb7f50442609b0936158d1b92b4",Address="学院" },

                      //程多运
                      new TeacherCourseArrange{ Id=Common.IdentityCreator.NewGuid,CreationTime=DateTime.Now, CreatorUserId=0,IsDeleted=false, TenantId=1,TeacherId=1,BeginTime=new DateTime(2017,6,16,15,00,00),EndTime=new DateTime(2017,6,16,17,00,00),StudentId="30333bec8e8e4634bb78b65cdd8ff8ef",ClassType="64c951d110044a51bd83c7e7e82f96ec",CourseType="e64269153d394e9e94ff970f2c16ae89",Remark="程多运 星期四下午3点到5点 1v1 上 提高课2个小时",CourseAddressType="03648bb7f50442609b0936158d1b92b4" ,Address="学院"},

                      //赵晟奎
    new TeacherCourseArrange{ Id=Common.IdentityCreator.NewGuid,CreationTime=DateTime.Now, CreatorUserId=0,IsDeleted=false, TenantId=1,TeacherId=1,BeginTime=new DateTime(2017,6,13,18,30,00),EndTime=new DateTime(2017,6,13,20,00,00),StudentId="eb7ca4ebdc5e48c582c224fd7f9854f0",ClassType="64c951d110044a51bd83c7e7e82f96ec",CourseType="e64269153d394e9e94ff970f2c16ae89",Remark="赵晟奎 星期二晚上六点半到八点 1v1 上 提高课1个半小时",CourseAddressType="55c431fd26b845b0a00880243d1a25a3",Address="蛇口东角头" },
        new TeacherCourseArrange{ Id=Common.IdentityCreator.NewGuid,CreationTime=DateTime.Now, CreatorUserId=0,IsDeleted=false, TenantId=1,TeacherId=1,BeginTime=new DateTime(2017,6,17,09,30,00),EndTime=new DateTime(2017,6,17,11,00,00),StudentId="eb7ca4ebdc5e48c582c224fd7f9854f0",ClassType="64c951d110044a51bd83c7e7e82f96ec",CourseType="e64269153d394e9e94ff970f2c16ae89",Remark="赵晟奎 星期六上午九点半到十一点 1v1 上 提高课1个半小时",CourseAddressType="55c431fd26b845b0a00880243d1a25a3",Address="蛇口东角头" }
            };
        }


        public void Create()
        {
            foreach (var c in _teacherCourseArrange)
            {
                //先限定逻辑为当非同一个老师在同一个开始时间则添加，后期需要处理时间交叉的情况
                if (_context.TeacherCourseArrange.Any(a => a.TeacherId == c.TeacherId && a.BeginTime == c.BeginTime) == false)
                {
                    _context.TeacherCourseArrange.Add(c);
                }
            }
            _context.SaveChanges();
        }
    }
}
