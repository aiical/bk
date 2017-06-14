using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace CourseManager.Web.Models.ClassHourStatistics
{
    [Serializable]
    [ModelBinder]
    public class ClassHourChartViewModel
    {
        /// <summary>
        /// y轴显示 当天目前上了多少课时
        /// </summary>
        [JsonProperty("y")]
        public decimal Duration { get; set; }
        /// <summary>
        /// 当前日期 是哪一天 如2017-6-13 16:55---2017-6-13 18:55 这一天总共上了2个小时课 那么显示Date 为2017-6-13 duration为2
        /// </summary>
        [JsonProperty("name")]
        public DateTime Date { get; set; }

        public int Day { get; set; }
    }
}