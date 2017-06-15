using CourseManager.Category;
using CourseManager.Common.Extensions;
using CourseManager.CourseArrange;
using CourseManager.CourseArrange.Dto;
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
        private readonly ICategorysAppService _categoryAppService;
        public CourseArrangeController(
            ITeacherCourseArrangeAppService teacherCourseArrangeAppService,
            ICategorysAppService categoryAppService
            )
        {
            this._teacherCourseArrangeAppService = teacherCourseArrangeAppService;
            this._categoryAppService = categoryAppService;
        }
        #region 教师排课
        public ActionResult TeacherCourseArrange(int teacherId,string yearMonth)
        {
            ViewBag.ActiveMenu = "ArrangeCourse";
            var categorys = _categoryAppService.GetAllCategorys();
            ViewBag.ClassType = categorys.Where(c=>c.CategoryType=="ClassType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseType = categorys.Where(c => c.CategoryType == "CourseType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseAddressType = categorys.Where(c => c.CategoryType == "CourseAddressType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseArranges = _teacherCourseArrangeAppService.GetArranages(new TeacherCourseArrangeInput()
            {
                TeacherId = teacherId < 1 ? 1 : teacherId,
                BeginTime = new DateTime(2017, 06, 01, 00, 00, 00),
                EndTime = new DateTime(2017, 07, 01, 00, 00, 00)
            });
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddTeacherCourseArrange(CreateTeacherCourseArrangeInput input)
        {
            var teacherCourseArrange = _teacherCourseArrangeAppService.TeacherArrangeCourse(input);
            return AbpJson(teacherCourseArrange.Id);
            //return AbpJson((teacherCourseArrange!=null&&!string.IsNullOrEmpty(teacherCourseArrange.Id))?true:false);
        }

        #endregion

    }
}