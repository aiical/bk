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
        //public ActionResult TeacherCourseArrange(TeacherCourseArrangeInput input)
        //{
        //    return View();
        //}
        public ActionResult TeacherCourseArrange(string teacherId,string yearMonth)
        {
            ViewBag.ActiveMenu = "ArrangeCourse";
            var categorys = _categoryAppService.GetAllCategorys();
            ViewBag.ClassTypeList = categorys.Where(c=>c.CategoryType=="ClassType").ToList().CreateSelect("CategoryName", "Id", "");
            ViewBag.CourseType = categorys.Where(c => c.CategoryType == "CourseType").ToList().CreateSelect("Name", "Id", "");
            ViewBag.CourseAddressType = categorys.Where(c => c.CategoryType == "CourseAddressType").ToList().CreateSelect("Name", "Id", "");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddTeacherCourseArrange(CreateTeacherCourseArrangeInput input)
        {
            var teacherCourseArrange = _teacherCourseArrangeAppService.TeacherArrangeCourse(input);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}