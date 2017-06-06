namespace CourseManager.Common.ItextSharp.PdfBase
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using CourseManager.Common.ItextSharp.Entity;
    using System;

    public class PdfPCellTypeHelper
    {
        /// <summary>
        /// 表格的边颜色
        /// </summary>
        private readonly string tableBorderColor = "#ccc";
        public string _fontPath = AppDomain.CurrentDomain.BaseDirectory + @"fonts\msyh.ttf"; //微软雅黑
        private BaseColor _baseColor = null;
        public PdfPCellTypeHelper(string cellBorderColor=null)
        {
            if (cellBorderColor!=null)
            {
                tableBorderColor = cellBorderColor;
            }
            //边框颜色
            System.Drawing.Color borderColor = System.Drawing.ColorTranslator.FromHtml(tableBorderColor);
            _baseColor = new iTextSharp.text.BaseColor(borderColor);
        }

        public  PdfPCell GetPdfPCell(PdfPCellEntity pdfPCellEntity,BaseFont bfChinese)
        {
            if (bfChinese==null)
            {
                FontFactory.Register(_fontPath);
                bfChinese = BaseFont.CreateFont(pdfPCellEntity.FontPath?? _fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            }
            PdfPCell cell = new PdfPCell(new Phrase(pdfPCellEntity.Content, new iTextSharp.text.Font(bfChinese, pdfPCellEntity.FontSize, pdfPCellEntity.FontStyle, iTextSharp.text.BaseColor.BLACK)));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            //背景颜色
            System.Drawing.Color bgColor = System.Drawing.ColorTranslator.FromHtml(pdfPCellEntity.BackgroundHexColor);
            cell.BackgroundColor = new iTextSharp.text.BaseColor(bgColor);
            
            cell.BorderColor = _baseColor;
            cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell.BorderWidthBottom = pdfPCellEntity.BorderWidth;
            cell.BorderWidthTop = pdfPCellEntity.BorderWidth;
            cell.BorderWidthLeft = pdfPCellEntity.BorderLeftWidth?? pdfPCellEntity.BorderWidth;
            cell.BorderWidthRight = pdfPCellEntity.BorderRightWidth?? pdfPCellEntity.BorderWidth;
            cell.Colspan = pdfPCellEntity.Colspan;
            cell.Rowspan = pdfPCellEntity.Rowspan;
            if (pdfPCellEntity.Leading>0.001f)
            {
                cell.SetLeading(pdfPCellEntity.Leading, 0);
            }
            cell.Padding = pdfPCellEntity.Padding;
            cell.PaddingTop = pdfPCellEntity.PaddingTop;
            cell.PaddingBottom = pdfPCellEntity.PaddingBottom;
            cell.PaddingLeft = pdfPCellEntity.Padding;
            cell.PaddingRight = pdfPCellEntity.Padding;
            if (pdfPCellEntity.FixedHeight.HasValue)
            {
                cell.FixedHeight = pdfPCellEntity.FixedHeight.Value;
            }
            return cell;
        }


    }
}
