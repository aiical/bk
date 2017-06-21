using Abp.Web.Mvc.Authorization;
using CourseManager.ClassHourStatistics;
using CourseManager.ClassHourStatistics.Dto;
using CourseManager.Core.EntitiesFromCustom;
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
    [AbpMvcAuthorize]
    public class ClassHourStatisticsController : CourseManagerControllerBase
    {
        private readonly IClassHourStatisticsAppService _teacherClassHoursStatisticsAppService;

        public ClassHourStatisticsController(IClassHourStatisticsAppService teacherClassHoursStatisticsAppService)
        {
            this._teacherClassHoursStatisticsAppService = teacherClassHoursStatisticsAppService;
        }

        public ActionResult TeacherClassHours()
        {
            ViewBag.ActiveMenu = "ClassHourStatistics";
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetTeacherClassHourStatistics(ClassHourStatisticsInput input)
        {
            var result = _teacherClassHoursStatisticsAppService.GetClassHourStatistics(input).Items;
            var classResult = result.Where(r => r.ClassType == "f756be8fe8b6487dbb50e6d63c69895c").ToList();
            var one2OneResult = result.Where(r => r.ClassType == "64c951d110044a51bd83c7e7e82f96ec").ToList();
            List<decimal> durations = new List<decimal>();
            var totalDuration = result.Sum(r => r.Duration);
            var one2oneDuration = one2OneResult.Sum(r => r.Duration);
            var classDuration = classResult.Sum(r => r.Duration);
            var beginTimeDay = input.BeginTime.Day;
            var endTimeDay = input.EndTime.Day;
            decimal[] classHoursArray = new decimal[endTimeDay - beginTimeDay + 1]; //取得的值是大于等于开始时间 小于结束时间
            decimal[] one2OneDurationsArray = new decimal[endTimeDay - beginTimeDay + 1];
            decimal[] classCourseDurationsArray = new decimal[endTimeDay - beginTimeDay + 1];
            int index = 0;
            for (int i = beginTimeDay - 1; i < endTimeDay; i++)
            {
                var duration = 0.0M;
                classHoursArray[index] = duration;
                if (result.Any(v => v.EndTime.Day == i + 1))
                {
                    duration = decimal.Round(result.Where(r => r.EndTime.Day == i + 1).Sum(c => c.Duration) / 60, 1);
                    classHoursArray[index] = duration;
                }
                if (one2OneResult.Any(v => v.EndTime.Day == i + 1))
                {
                    one2OneDurationsArray[index] = decimal.Round(one2OneResult.Where(r => r.EndTime.Day == i + 1).Sum(c => c.Duration) / 60, 1);
                }
                if (classResult.Any(v => v.EndTime.Day == i + 1))
                {
                    classCourseDurationsArray[index] = decimal.Round(classResult.Where(r => r.EndTime.Day == i + 1).Sum(c => c.Duration) / 60, 1);
                }
                index++;
            }
            ResultData data = new ResultData();
            data.returnData = new Dictionary<string, object>
            {
                { "durations",classHoursArray},// durations.ToArray()
                { "one2OneDurations",one2OneDurationsArray},
                  { "classCourseDurations",classCourseDurationsArray},
                {"total",decimal.Round(totalDuration/60,1) },
                        {"one2oneDuration",decimal.Round(one2oneDuration/60,1) },
                        {"classDuration",decimal.Round(classDuration/60,1) }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}