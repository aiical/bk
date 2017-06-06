using System;
using System.Collections.Generic;

namespace CourseManager.Core.EntitiesFromCustom
{
    /// <summary>
    /// 列表返回公共类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RowListData<T> where T :class
    {
        public List<T> rows { get; set; }
        public int total { get; set; }
        public int pagesize { get; set; }
        public dynamic otherData { get; set; }
    }
}
