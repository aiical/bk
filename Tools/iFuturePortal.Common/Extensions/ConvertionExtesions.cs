using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CourseManager.Common.Extensions
{
    public static class ConvertionExtesions
    {

        public static List<SelectListItem> CreateSelect<T>(this IList<T> t, string text, string value, string defaultVal = "")
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            if (string.IsNullOrEmpty(defaultVal))
            {
                selectListItems.Add(new SelectListItem() { Text = "请选择", Value = "", Selected = true });
            }
            else selectListItems.Add(new SelectListItem() { Text = defaultVal, Value = "-1", Selected = true });
            foreach (var item in t)
            {
                var propers = item.GetType().GetProperty(text);
                var valpropers = item.GetType().GetProperty(value);
                string selectValue = valpropers.GetValue(item, null).ToString();
                selectListItems.Add(new SelectListItem { Text = propers.GetValue(item, null).ToString(), Selected = selectValue == defaultVal, Value = selectValue });
            }
            return selectListItems;
        }
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
            {
                return default(T);
            }

            if (!typeof(T).IsGenericType)
            {
                return (T)Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }
    }
}
