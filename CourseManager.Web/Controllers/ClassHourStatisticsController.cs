using CourseManager.ClassHourStatistics;
using CourseManager.ClassHourStatistics.Dto;
using CourseManager.CourseArrange;
using CourseManager.CourseArrange.Dto;
using CourseManager.Web.Models.ClassHourStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManager.Web.Controllers
{
    public class ClassHourStatisticsController : CourseManagerControllerBase
    {
        private readonly IClassHourStatisticsAppService _teacherClassHoursStatisticsAppService;
        public ClassHourStatisticsController(IClassHourStatisticsAppService teacherClassHoursStatisticsAppService)
        {
            this._teacherClassHoursStatisticsAppService = teacherClassHoursStatisticsAppService;
        }

        public ActionResult TeacherClassHours()
        {
            return View();
        }
        public JsonResult GetTeacherClassHourStatistics(ClassHourStatisticsInput input)
        {
            var result = _teacherClassHoursStatisticsAppService.GetClassHourStatistics(input).Items;
            ClassHourChartViewModel cvm = new ClassHourChartViewModel();
            List<ClassHourChartViewModel> vmList = new List<ClassHourChartViewModel>();
            foreach (var item in result)
            {
                if (!vmList.Any(v => v.Date == item.BeginTime))
                    vmList.Add(new ClassHourChartViewModel()
                    {
                        Date = item.BeginTime,
                        Duration = item.Duration / 60
                    });
            }
            return Json(vmList, JsonRequestBehavior.AllowGet);
        }
    }
}