using Abp.Application.Services.Dto;
using AutoMapper;
using CourseManager.Category;
using CourseManager.Common.Extensions;
using CourseManager.CourseArrange;
using CourseManager.CourseArrange.Dto;
using CourseManager.Students;
using CourseManager.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManager.Web.Controllers
{
    public class CourseArrangeController : CourseManagerControllerBase
    {
        private readonly ITeacherCourseArrangeAppService _teacherCourseArrangeAppService;
        private readonly IStudentCourseArrangeAppService _studentCourseArrangeAppService;
        private readonly IStudentAppService _studentAppService;
        private readonly ICategorysAppService _categoryAppService;
        private readonly IUserAppService _userAppService;
        public CourseArrangeController(
            ITeacherCourseArrangeAppService teacherCourseArrangeAppService,
            IStudentCourseArrangeAppService studentCourseArrangeAppService,
            IStudentAppService studentAppService,
        ICategorysAppService categoryAppService,
            IUserAppService userAppService
            )
        {
            this._teacherCourseArrangeAppService = teacherCourseArrangeAppService;
            this._studentCourseArrangeAppService = studentCourseArrangeAppService;
            this._studentAppService = studentAppService;
            this._categoryAppService = categoryAppService;
            this._userAppService = userAppService;
        }
        #region 教师排课
        public ActionResult TeacherCourseArrange(int teacherId = 1, string yearMonth = "")
        {
            ViewBag.ActiveMenu = "ArrangeCourse";
            CommonCourseArrangeSelectData();

            //要加载数据需要先选择老师 初始化页面的时候 数据为空
            var courseArrangeData = new ListResultDto<TeacherCourseArrangeListDto>();
            if (!string.IsNullOrEmpty(yearMonth))
            {
                string[] yearMonthArr = yearMonth.Split('-');
                int year = Convert.ToInt32(yearMonthArr[0]);
                int month = Convert.ToInt32(yearMonthArr[1]);
                ViewBag.YearMonth = new DateTime(year, month, 01, 00, 00, 00);
                courseArrangeData = _teacherCourseArrangeAppService.GetArranages(new TeacherCourseArrangeInput()
                {
                    TeacherId = teacherId < 1 ? 1 : teacherId,
                    BeginTime = new DateTime(year, month, 01, 00, 00, 00),
                    EndTime = new DateTime(year, month + 1, 01, 00, 00, 00)
                });
            }
            var teachers = _userAppService.GetUsers(new Users.Dto.UserInput() { }).Items; ;
            ViewBag.Teachers = teachers;
            string name = string.Empty;
            var teacher = teachers.Where(t => t.Id == (teacherId < 1 ? 1 : teacherId)).FirstOrDefault();
            if (teacher != null && teacher.Id > 0) name = teacher.FullName;
            ViewBag.TeacherName = name;
            ViewBag.CourseArranges = courseArrangeData;
            return View();
        }
        private void CommonCourseArrangeSelectData()
        {
            var categorys = _categoryAppService.GetAllCategorys();
            ViewBag.ClassType = categorys.Where(c => c.CategoryType == "ClassType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseType = categorys.Where(c => c.CategoryType == "CourseType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseAddressType = categorys.Where(c => c.CategoryType == "CourseAddressType").ToList().CreateSelect("CategoryName", "Id", "");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddTeacherCourseArrange(CreateTeacherCourseArrangeInput input)
        {
            var teacherCourseArrangeResult = _teacherCourseArrangeAppService.TeacherArrangeCourse(input);
            return AbpJson(teacherCourseArrangeResult);
            //return AbpJson((teacherCourseArrange!=null&&!string.IsNullOrEmpty(teacherCourseArrange.Id))?true:false);
        }

        public PartialViewResult EditTeacherCourseArrange(string id)
        {
            var singleCourseArrange = _teacherCourseArrangeAppService.GetArranage(new TeacherCourseArrangeInput() { Id = id });

            var updateCourseArrangeDto = Mapper.Map<UpdateTeacherCourseArrangeInput>(singleCourseArrange);

            //var userList = _userAppService.GetUsers();
            //ViewBag.AssignedPersonId = new SelectList(userList.Items, "Id", "Name", updateTaskDto.AssignedPersonId);
            var categorys = _categoryAppService.GetAllCategorys();
            ViewBag.ClassType = categorys.Where(c => c.CategoryType == "ClassType").ToList().CreateSelect("CategoryName", "Id", categorys.FirstOrDefault(c => c.Id == singleCourseArrange.ClassType).CategoryName);
            ViewBag.CourseType = categorys.Where(c => c.CategoryType == "CourseType").ToList().CreateSelect("CategoryName", "Id", categorys.FirstOrDefault(c => c.Id == singleCourseArrange.CourseType).CategoryName);
            ViewBag.CourseAddressType = categorys.Where(c => c.CategoryType == "CourseAddressType").ToList().CreateSelect("CategoryName", "Id", categorys.FirstOrDefault(c => c.Id == singleCourseArrange.CourseAddressType).CategoryName);
            return PartialView("_EditTeacherCourseArrange", updateCourseArrangeDto);
        }

        #endregion
        #region 学生排课
        public ActionResult StudentCourseArrange(string studentId = "", string yearMonth = "")
        {
            ViewBag.ActiveMenu = "ArrangeCourse";
            var categorys = _categoryAppService.GetAllCategorys();
            ViewBag.ClassType = categorys.Where(c => c.CategoryType == "ClassType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseType = categorys.Where(c => c.CategoryType == "CourseType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseAddressType = categorys.Where(c => c.CategoryType == "CourseAddressType").ToList().CreateSelect("CategoryName", "Id", "");

            var courseArrangeData = new ListResultDto<StudentCourseArrangeListDto>();
            var now = DateTime.Now;
            yearMonth = string.IsNullOrEmpty(yearMonth) ? string.Format("{0}-{1}", now.Year, now.Month) : yearMonth;
            if (!string.IsNullOrEmpty(yearMonth))
            {
                string[] yearMonthArr = yearMonth.Split('-');
                int year = Convert.ToInt32(yearMonthArr[0]);
                int month = Convert.ToInt32(yearMonthArr[1]);
                ViewBag.YearMonth = new DateTime(year, month, 01, 00, 00, 00);
                if (!string.IsNullOrEmpty(studentId))
                    courseArrangeData = _studentCourseArrangeAppService.GetArranages(new StudentCourseArrangeInput()
                    {
                        StudentId = studentId,
                        BeginTime = new DateTime(year, month, 01, 00, 00, 00),
                        EndTime = new DateTime(year, month + 1, 01, 00, 00, 00)
                    });
                else courseArrangeData = _studentCourseArrangeAppService.GetArranages(new StudentCourseArrangeInput()
                {
                    BeginTime = new DateTime(year, month, 01, 00, 00, 00),
                    EndTime = new DateTime(year, month + 1, 01, 00, 00, 00)
                });
            }
            var students = _studentAppService.GetStudents().Items;
            ViewBag.Students = students;
            string stuName = string.Empty;
            if (!string.IsNullOrEmpty(studentId))
            {
                var stu = students.Where(t => t.Id == studentId).FirstOrDefault();
                if (stu != null)
                {
                    stuName = stu.CnName;
                }
            }
            ViewBag.StudentName = stuName;
            ViewBag.CourseArranges = courseArrangeData;
            return View("~/Views/CourseArrange/StudentCourseArrange/StudentCourseArrange.cshtml");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddStudentCourseArrange(CreateStudentCourseArrangeInput input)
        {
            var courseArrangeResult = _studentCourseArrangeAppService.StudentArrangeCourse(input);
            return AbpJson(courseArrangeResult);
        }

        #endregion
    }
}