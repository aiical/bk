using iTextSharp.text;
using CourseManager.Common.ItextSharp.PdfBase;

namespace CourseManager.Common.ItextSharp.Entity
{
    public class PdfPCellEntity
    {
        public PdfPCellEntity()
        {
            BackgroundHexColor = "#f5f5f5";
            FontSize = PaintTools.PdfPxToHtmlPx(12);
            Colspan = 1;
            Rowspan = 1;
            BorderWidth = 0.01f;
            Padding = 0;
            FontStyle = Font.NORMAL;
            PaddingTop = 2;
            PaddingBottom = 2;
        }
        public string Content { get; set; }
        /// <summary>
        /// 背景颜色，Hex格式：#000000
        /// </summary>
        public string BackgroundHexColor { get; set; }
        public string FontPath { get; set; }
        /// <summary>
        /// Font.NORMAL
        /// </summary>
        public int FontStyle { get; set; }
        public float FontSize { get; set; }
        public float BorderWidth { get; set; }
        public float? BorderLeftWidth { get; set; }
        public float? BorderRightWidth { get; set; }
        public int Colspan { get; set; }
        public int Rowspan { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int HorizontalAlignment { get; set; }
        public int VerticalAlignment { get; set; }
        public float Leading { get; set; }
        /// <summary>
        /// 左右padding
        /// </summary>
        public float Padding { get; set; }
        public float PaddingTop { get; set; }        
        public float PaddingBottom { get; set; }

        public float? FixedHeight { get; set; }
    }
}
