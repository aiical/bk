namespace CourseManager.Common.ItextSharp.Entity
{
    using System;

    /// <summary>
    /// 图片框数据
    /// </summary>
    [Serializable]
    public class ImageBoxData
    {
        /// <summary>
        /// 图片框名称 
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// 图片URL
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 图片显示宽度
        /// </summary>
        public float width { get; set; }

        /// <summary>
        /// 图片显示高度
        /// </summary>
        public float height { get; set; }

        /// <summary>
        /// x偏移
        /// </summary>
        public float x { get; set; }

        /// <summary>
        /// y偏移
        /// </summary>
        public float y { get; set; }

        /// <summary>
        /// 缩放比例
        /// </summary>
        public float scale { get; set; }


    }
}
