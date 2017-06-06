using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CourseManager.Common
{
    /// <summary>
    /// DataTable转List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectToListModel<T> where T : new()
    {
        /// <summary>
        /// DataTable转实体类
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="customAction">自定义操作某列的值</param>
        /// <param name="ignoreVals">忽略的值</param>
        /// <returns></returns>
        public List<T> DataTableConvertToModel(DataTable dt, Dictionary<string, Action<string, T>> customAction = null, List<string> ignoreVals = null)
        {
            //定义集合
            List<T> ts = new List<T>();
            T t = new T();
            string tempName = "";
            //获取此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            string val = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                t = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    try
                    {
                        tempName = pi.Name;
                        //检查DataTable是否包含此列
                        if (dt.Columns.Contains(tempName))
                        {
                            //判断此属性是否有set
                            if (!pi.CanWrite)
                                continue;
                            object value = row[tempName];

                            if (value != null && (val = value.ToString().Trim()) != "")
                            {
                                if (null != ignoreVals && ignoreVals.Any(a => a == val))
                                {
                                    continue;
                                }
                                if (customAction != null && customAction.ContainsKey(tempName))//特殊处理的列就调用封装好的方法处理
                                {
                                    customAction[tempName](val, t);
                                }
                                else
                                {
                                    if (pi.PropertyType.FullName.Contains("System.Int32"))
                                    {
                                        pi.SetValue(t, Convert.ToInt32(val.Replace(",", "")), null);
                                    }
                                    else if (pi.PropertyType.FullName.Contains("System.Double"))
                                    {
                                        pi.SetValue(t, Convert.ToDouble(val.Replace(",", "")), null);
                                    }
                                    else
                                    {
                                        pi.SetValue(t, val, null);
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ts.Add(t);
            }

            return ts;
        }

        public T ModelConvertToModel(T newModel, T resultModel, List<string> unlessColumns, bool unlessNullValue = true)
        {
            //获取此模型的公共属性
            PropertyInfo[] propertyse = newModel.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyse)
            {
                if (!pi.CanWrite)
                    continue;
                if (unlessColumns != null && unlessColumns.Contains(pi.Name))
                {
                    continue;
                }
                if (pi.PropertyType.FullName.Contains("ICollection"))
                {
                    continue;
                }
                object val = pi.GetValue(newModel, null);
                if (unlessNullValue)
                {
                    if (val != null)
                    {
                        pi.SetValue(resultModel, val, null);
                    }
                }
                else
                {
                    pi.SetValue(resultModel, val, null);
                }
            }
            return resultModel;
        }

        /// <summary>
        /// 把字符串的NULL值改成string.Empty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public T SetNullToEmptyForString(T model)
        {
            PropertyInfo[] propertyse = model.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyse)
            {
                if (!pi.CanWrite)
                    continue;
                object val = pi.GetValue(model, null);
                if (pi.PropertyType.FullName.Contains("System.String") && val == null)
                {
                    pi.SetValue(model, "", null);
                }
            }
            return model;
        }

    }

}
