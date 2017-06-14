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
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetTeacherClassHourStatistics(ClassHourStatisticsInput input)
        {
            var result = _teacherClassHoursStatisticsAppService.GetClassHourStatistics(input).Items;
            List<decimal> durations = new List<decimal>();
            var totalDuration = result.Sum(r => r.Duration);
            var beginTimeDay = input.BeginTime.Day;
            var endTimeDay = input.EndTime.Day;
            decimal[] classHoursArray = new decimal[endTimeDay - beginTimeDay + 1]; //取得的值是大于等于开始时间 小于结束时间
            for (int i = beginTimeDay - 1; i < endTimeDay; i++)
            {
                var duration = 0.0M;
                classHoursArray[i] = duration;
                if (result.Any(v => v.EndTime.Day == i + 1))
                {
                    duration = decimal.Round(result.Where(r => r.EndTime.Day == i + 1).Sum(c => c.Duration) / 60,1);
                    classHoursArray[i] = duration;
                }
            }
            ResultData data = new ResultData();
            data.returnData = new Dictionary<string, object>
            {
                { "durations",classHoursArray},// durations.ToArray()
                {"total",decimal.Round(totalDuration/60,1) }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}