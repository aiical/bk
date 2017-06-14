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
        public CourseArrangeController(ITeacherCourseArrangeAppService teacherCourseArrangeAppService)
        {
            this._teacherCourseArrangeAppService = teacherCourseArrangeAppService;
        }
        #region 教师排课
        //public ActionResult TeacherCourseArrange(TeacherCourseArrangeInput input)
        //{
        //    return View();
        //}
        public ActionResult TeacherCourseArrange(string teacherId,string yearMonth)
        {
            ViewBag.ActiveMenu = "TeacherCourseArrange";
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