namespace CourseManager.Common.ItextSharp.Entity
{
    using System;

    /// <summary>
    /// 文本框数据
    /// </summary>
    [Serializable]
    public class TextBoxData
    {
        /// <summary>
        /// 文本框名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 文字对齐方式
        /// </summary>
        public string align { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
    }

}
