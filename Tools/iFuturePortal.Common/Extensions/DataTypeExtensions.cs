using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CourseManager.Common.Extensions
{
    public static class DataTypeExtensions
    {
        #region string

        #region 正则表达式

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return null;
            }
            return Regex.Match(value, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="options"></param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern, RegexOptions options)
        {
            if (value == null)
            {
                return null;
            }
            return Regex.Match(value, pattern, options).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合，包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> Matches(this string value, string pattern)
        {
            if (value == null)
            {
                return new string[] { };
            }
            MatchCollection matches = Regex.Matches(value, pattern);
            return from Match match in matches select match.Value;
        }

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.IsMatch(pattern);
        }


        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            const string pattern = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return value.IsMatch(pattern);
        }


        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }

        #endregion

        #region 其他操作

        /// <summary>
        /// 替换左边的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        public static string ReplaceLeft(this string value, string oldStr, string newStr)
        {
            if (value.IsNotNullAndNotEmpty() && value.IndexOf(oldStr) == 0)
            {
                value = newStr + value.Substring(oldStr.Length);
            }
            return value;
        }

        #region 移除字符串末尾指定字符
        /// <summary>   
        /// 移除字符串末尾指定字符   
        /// </summary>   
        /// <param name="str">需要移除的字符串</param>   
        /// <param name="value">指定字符</param>   
        /// <returns>移除后的字符串</returns>   
        public static string RemoveLastChar(this string origin, string value)
        {
            int _finded = origin.LastIndexOf(value);
            if (_finded != -1)
            {
                return origin.Substring(0, _finded);
            }
            return origin;
        }
        #endregion
        /// <summary>
        /// 字符串不为空（!=null and != String.Empty）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullAndNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 字符串不为空，也不为空串（!=null and != "  "）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullAndNotWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 检查类不能为空引用
        /// </summary>
        /// <param name="value"></param>
        public static bool IsNotNull<T>(this T value) where T : class
        {
            return null != value;
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.Split(new[] { strSplit }, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// 添加url地址参数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key">链接字符串键</param>
        /// <param name="value">链接字符串值</param>
        /// <returns></returns>
        public static string ToConcatQueryString(this string text, string key, string value)
        {
            string url;
            if (text.IndexOf('?') > -1)
            {
                url = "{0}&{1}={2}";
            }
            else
            {
                url = "{0}?{1}={2}";
            }
            return string.Format(url, text, key, value);
        }
        /// <summary>
        /// 添加url地址参数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keyValue">比如：name=1</param>
        /// <returns></returns>
        public static string ToConcatQueryString(this string text, string keyValue)
        {
            string url;
            if (text.IndexOf('?') > -1)
            {
                url = "{0}&{1}";
            }
            else
            {
                url = "{0}?{1}";
            }
            return string.Format(url, text, keyValue);
        }

        /// <summary>
        /// 网页空格html编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToHtmlEncodeKongGe(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text.Replace(" ", "&nbsp;");
            }
            return text;
        }

        #endregion

        #region html字符串操作

        public static string ToEncodeHtml(this string value)
        {
            if (null == value)
            {
                return value;
            }
            return value.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }
        /// <summary>
        /// <para>将 URL 中的参数名称/值编码为合法的格式。</para>
        /// <para>可以解决类似这样的问题：假设参数名为 tvshow, 参数值为 Tom&Jery，如果不编码，可能得到的网址： http://a.com/?tvshow=Tom&Jery&year=1965 编码后则为：http://a.com/?tvshow=Tom%26Jery&year=1965 </para>
        /// <para>实践中经常导致问题的字符有：'&', '?', '=' 等</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AsUrlData(this string data)
        {
            return data == null ? null : Uri.EscapeDataString(data);
        }
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }
        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }
        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&amp;连接，首位没有符号，如：a=1&amp;b=2&amp;c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }
        #endregion

        #endregion

        #region IEnumerable

        /// <summary>
        /// IEnumerable是否为空，没有包含任何元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> value)
        {
            return (value == null || !value.Any());
        }

        /// <summary>
        /// IEnumerable是否不为空，有存在元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> value)
        {
            return (value != null && value.Any());
        }

        /// <summary>
        /// 计算总和，保留指定的小数点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="pointCount">保持的小数点数</param>
        /// <param name="isTrimRightZero">是否去掉后面多余的0</param>
        /// <returns></returns>
        public static string SumWithPoint<T>(this IEnumerable<T> source, Func<T, double> selector, int pointCount = 2, bool isTrimRightZero = true)
        {
            var val = source.Sum(selector);
            if (pointCount < 1)
            {
                return val.ToString();
            }
            else
            {
                string format = new string('0', pointCount);
                string valStr = val.ToString("0." + format);
                if (isTrimRightZero)
                {
                    if (valStr.EndsWith("." + format))
                    {
                        valStr = valStr.Substring(0, valStr.LastIndexOf('.'));
                    }
                }
                return valStr;
            }
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            if (condition)
            {
                source = source.Where(predicate);
            }
            return source;
        }

        #endregion

        #region 字典类扩展
        /// <summary>
        /// SortedList添加值，为空不添加
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isTrimStr">去掉字符串两边的空格</param>
        public static bool AddKeyValue(this SortedList<string, object> sortedList, string key, object value, bool isTrimStr = true)
        {
            if (value.IsNotNull() && key.IsNotNullAndNotEmpty())
            {
                if ((value is string) && value.ToString() == string.Empty)
                {
                    return false;
                }
                if (sortedList == null)
                    sortedList = new SortedList<string, object>();
                if (isTrimStr && (value is string))
                {
                    value = value.ToString().Trim();
                }
                sortedList.Add(key, value);
                return true;
            }
            return false;
        }

        #endregion

        #region Exception
        /// <summary>
        /// 格式化异常消息
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="isHideStackTrace">是否隐藏异常规模信息</param>
        /// <returns>格式化后的异常信息字符串</returns>
        public static string FormatMessage(this Exception e, bool isHideStackTrace = false)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            string appString = string.Empty;
            while (e != null)
            {
                if (count > 0)
                {
                    appString += "  ";
                }
                sb.AppendLine(string.Format("{0}异常消息：{1}", appString, e.Message));
                sb.AppendLine(string.Format("{0}异常类型：{1}", appString, e.GetType().FullName));
                sb.AppendLine(string.Format("{0}异常方法：{1}", appString, (e.TargetSite == null ? null : e.TargetSite.Name)));
                sb.AppendLine(string.Format("{0}异常源：{1}", appString, e.Source));
                if (!isHideStackTrace && e.StackTrace != null)
                {
                    sb.AppendLine(string.Format("{0}异常堆栈：{1}", appString, e.StackTrace));
                }
                if (e.InnerException != null)
                {
                    sb.AppendLine(string.Format("{0}内部异常：", appString));
                    count++;
                }
                e = e.InnerException;
            }
            return sb.ToString();
        }
        #endregion


    }



}
