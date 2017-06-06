namespace CourseManager.Common.ItextSharp.Entity
{
    using System;
    using System.Collections.Generic;

    public class PdfTableEntity
    {
        public int TotalWidth { get; set; }
        public int TotalHeight { get; set; }
        public int[] Widths { get; set; }
        /// <summary>
        /// PdfPCell.ALIGN_CENTER
        /// </summary>
        public int HorizontalAlignment { get; set; }
        /// <summary>
        /// PdfPCell.ALIGN_MIDDLE
        /// </summary>
        public int VerticalAlignment { get; set; }
        public int BeginYear { get; set; }
        public int BeginMonth { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public List<PdfPCellEntity> PdfPCellEntityList { get; set; }

    }
}
