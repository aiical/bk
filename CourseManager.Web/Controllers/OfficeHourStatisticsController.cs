using Abp.Web.Mvc.Authorization;
using CourseManager.ClassHourStatistics;
using CourseManager.ClassHourStatistics.Dto;
using CourseManager.Core.EntitiesFromCustom;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CourseManager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class OfficeHourStatisticsController : CourseManagerControllerBase
    {
        private readonly IClassHourStatisticsAppService _teacherOfficeHoursStatisticsAppService;
        public OfficeHourStatisticsController(IClassHourStatisticsAppService teacherOfficeHoursStatisticsAppService)
        {
            this._teacherOfficeHoursStatisticsAppService = teacherOfficeHoursStatisticsAppService;
        }

        public ActionResult OfficeHourStatistics()
        {
            ViewBag.ActiveMenu = "OfficeHourStatistics";
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetTeacherOfficeHourStatistics(ClassHourStatisticsInput input)
        {
            var result = _teacherOfficeHoursStatisticsAppService.GetClassHourStatistics(input).Items;
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