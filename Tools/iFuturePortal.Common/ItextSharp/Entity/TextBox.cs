namespace CourseManager.Common.ItextSharp.Entity
{
    using System;

    /// <summary>
    /// 文本框 
    /// </summary>
    public class TextBox
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// x坐标
        /// </summary>
        public float x { get; set; }
        /// <summary>
        /// y坐标
        /// </summary>
        public float y { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public float width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public float height { get; set; }
        /// <summary>
        /// 对齐，默认对齐方式
        /// </summary>
        public string align { get; set; }
        /// <summary>
        /// 行距
        /// </summary>
        public float leading { get; set; }
        /// <summary>
        /// 字间距
        /// </summary>
        public float space { get; set; }
        /// <summary>
        /// 最大长度,全角汉字
        /// </summary>
        public int max_length { get; set; }
        /// <summary>
        /// 最大行数
        /// </summary>
        public int maxline { get; set; }
        /// <summary>
        /// Pt
        /// </summary>
        public float pt { get; set; }
        /// <summary>
        /// 像素
        /// </summary>
        public float px { get; set; }
        /// <summary>
        /// 是否粗体
        /// </summary>
        public bool bold { get; set; }

        /// <summary>
        /// 文字排版
        /// vertical 竖排 horizontal 水平
        /// </summary>
        public string direction { get; set; }

    }

}
