using System;

namespace CourseManager.Core.EntitiesFromCustom
{
    public class UploadifyResult
    {
        public UploadifyResult()
        {
            isSuccess = true;
        }
        public bool isSuccess { get; set; }
        public string uploadPath { get; set; }
        public string fileName { get; set; }
        public int fileSize { get; set; }
        public string msg { get; set; }

        #region 前端上传控件的html属性
        /// <summary>
        /// 控件ID
        /// </summary>
        public string inputId { get; set; }
        /// <summary>
        /// 控件name属性
        /// </summary>
        public string inputName { get; set; }

        public bool multi { get; set; }

        #endregion

        #region 图片属性
        public int width { get; set; }
        public int height { get; set; }
        #endregion
        /// <summary>
        /// 视频的具体信息
        /// </summary>
        public string videoInfo { get; set; }

    }
}
