using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Common
{
    public class CalendarHelper
    {
        /// <summary>
        /// 作者：vincen
        /// 时间：2012.11.02
        /// 描述：获取当前月第一天（星期几（1，2..,7））
        /// </summary>
        /// <returns></returns>
        public static int GetCurrMonthFirstDayWeekIndex()
        {
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;
            return GetYearMonthFirstDayWeekIndex(_year, _month);
        }
        /// <summary>
        ///  作者：vincen
        /// 时间：2012.11.02
        /// 描述：获取指定年月第一天（星期几（1，2..,7））
        /// </summary>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        /// <returns></returns>
        public static int GetYearMonthFirstDayWeekIndex(int _year, int _month)
        {
            DateTime _fday = new DateTime(_year, _month, 1);

            string _week = _fday.DayOfWeek.ToString().ToLower();
            int _index = 0;
            switch (_week)
            {
                case "monday":
                    _index = 1;
                    break;
                case "tuesday":
                    _index = 2;
                    break;
                case "wednesday":
                    _index = 3;
                    break;
                case "thursday":
                    _index = 4;
                    break;
                case "friday":
                    _index = 5;
                    break;
                case "saturday":
                    _index = 6;
                    break;
                case "sunday":
                    _index = 7;
                    break;
            }

            return _index;
        }
        /// <summary>
        /// 作者：Vincen
        /// 时间：2012.11.02
        /// 描述：获取指定年月的天数
        /// </summary>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        /// <returns></returns>
        public static int GetYearMonthDays(int _year, int _month)
        {
            if (_month == 1 || _month == 3 || _month == 5 || _month == 7 || _month == 8 || _month == 10 || _month == 12)
            {
                return 31;
            }
            else if (_month == 4 || _month == 6 || _month == 9 || _month == 11)
            {
                return 30;
            }
            else
            {
                if (_year % 4 == 0)
                {
                    return 29;
                }
                else
                {
                    return 28;
                }
            }
        }
        /// <summary>
        /// 获取当前日期的下周周一日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextStartWeek(DateTime currentDateTime)
        {
            //当前时间
            if (currentDateTime == null)
                currentDateTime = DateTime.Now;
            //今天星期几
            int currentDayOfWeek;
            //本周周一
            DateTime currentStartWeek;
            //下周周一
            DateTime nextStartWeek;
            currentDayOfWeek = Convert.ToInt32(currentDateTime.DayOfWeek.ToString("d"));    //今天星期几
            currentStartWeek = currentDateTime.AddDays(1 - ((currentDayOfWeek == 0) ? 7 : currentDayOfWeek));
            nextStartWeek = currentStartWeek.AddDays(7);
            return nextStartWeek;
        }
        /// <summary>
        /// 当前周 周一日期
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static DateTime GetCurStartWeek(DateTime currentDateTime)
        {
            //当前时间
            if (currentDateTime == null)
                currentDateTime = DateTime.Now;
            //今天星期几
            int currentDayOfWeek;
            //本周周一
            DateTime currentStartWeek;
            //下周周一         
            currentDayOfWeek = Convert.ToInt32(currentDateTime.DayOfWeek.ToString("d"));    //今天星期几
            int days = 1 - ((currentDayOfWeek == 0) ? 7 : currentDayOfWeek);
            currentStartWeek = currentDateTime.AddDays(days);// DateTime.Now.AddDays(days);            
            return currentStartWeek;
        }
        /// <summary>
        /// 前一周 周一日期
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static DateTime GetPreStartWeek(DateTime currentDateTime)
        {
            //当前时间
            if (currentDateTime == null)
                currentDateTime = DateTime.Now;
            //今天星期几
            int currentDayOfWeek;
            //本周周一
            DateTime currentStartWeek;
            //下周周一
            DateTime preStartWeek;
            currentDayOfWeek = Convert.ToInt32(currentDateTime.DayOfWeek.ToString("d"));    //今天星期几
            currentStartWeek = currentDateTime.AddDays(1 - ((currentDayOfWeek == 0) ? 7 : currentDayOfWeek));
            preStartWeek = currentStartWeek.AddDays(-7);
            return preStartWeek;
        }
        public static string GetclassName(int status)
        {
            string classitem = "Status_Normal";//状态为2
            if (status == 1)
            {
                classitem = "Status_Default";
            }
            if (status == 3)
            {
                classitem = "Status_Effect";
            }
            else if (status == 4)
            {
                classitem = "Status_Cancel";
            }
            else if (status == 5)
            {
                classitem = "Status_Finished";
            }
            return classitem;
        }


    }

    
}
