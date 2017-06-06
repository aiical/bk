using iTextSharp.text;
using iTextSharp.text.pdf;
using CourseManager.Common.ItextSharp.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace CourseManager.Common.ItextSharp.PdfBase
{

    /// <summary>
    /// 生成PDF的基本工具
    /// 单位统一为mm
    /// 坐标计算方式统一同左下角算起:左下角的坐标就是(0, 0)
    /// </summary>
    public class PaintTools
    {
        /// <summary>
        /// 文档
        /// </summary>
        static int DPI = 72;

        /// <summary>
        /// 默认背景色：白色#ffffff
        /// </summary>
        public static string DefaultBgColor = "#ffffff";

        static float ratio = 4.16668f;
        /// <summary>
        /// 网页的像素转pdf的像素
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        public static float HtmlPxToPdfPx(float px)
        {
            return px * ratio;
        }
        /// <summary>
        /// pdf
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        public static float PdfPxToHtmlPx(float px, float? _ratio=null)
        {
            if (_ratio.HasValue)
            {
                ratio = _ratio.Value;
            }
            return px / ratio;
        }

        /// <summary>
        /// 毫米转换成像素
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static float mmToPx(float mm)
        {
            //象素数 / DPI = 英寸数 
            //英寸数 * 25.4 = 毫米数
            //公式:1cm = 10mm * DPI * (10 /254) = xxPX
            return mm * DPI * (float)(1 / 25.4);
        }

        /// <summary>
        /// 像素转换为毫米
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        public static float pxToMm(float px)
        {
            return px / DPI / (float)(1 / 25.4);
        }

        /// <summary>
        /// 像素转化为磅值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static float PxToP(float px)
        {
            //3磅 ＝ 4像素
            return 3 * px / 4;
        }
        /// <summary>
        /// 磅值转化为像素
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static float pToPX(float p)
        {
            //3磅 ＝ 4像素
            return 4 * p / 3;
        }

        /// <summary>
        /// 获取填补内容的空格
        /// </summary>
        /// <param name="bfChinese"></param>
        /// <param name="textBoxWidth"></param>
        /// <param name="sourceContentWidth"></param>
        /// <param name="pt"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static string GetKongGeFullContent(BaseFont bfChinese, float textBoxWidth, float sourceContentWidth, float pt, string align)
        {
            string kongGe = "";
            for (int i = 0; i < 500; i++)  //一行文字不会超过500个空格
            {
                if (textBoxWidth > sourceContentWidth)
                {
                    kongGe += " ";
                    switch (align)
                    {
                        case "center":
                            sourceContentWidth += bfChinese.GetWidthPointKerned(" ", pt) * 2;
                            break;
                        default:
                            sourceContentWidth += bfChinese.GetWidthPointKerned(" ", pt);
                            break;
                    }

                }

            }
            return kongGe;
        }

        /// <summary>
        /// 判断字符是否是需要旋转绘制的字符
        /// </summary>
        /// <param name="zimu"></param>
        /// <returns></returns>
        public static bool IsRotateChar(char zimu)
        {
            //特殊中文符号旋转
            char[] specialChar = new char[] { '（', '）', '[', ']', '{', '}', '《', '》' };
            if (zimu <= 255 || specialChar.Contains(zimu))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 剪切图片
        /// </summary>
        /// <param name="bitmapCut">输出图片</param>
        /// <param name="bitmapSource">源图片</param>
        /// <param name="outRectangle">截取框</param>
        /// <param name="srcX">起截点X坐标</param>
        /// <param name="srcY">起截点Y坐标</param>
        /// <param name="srcWidth">截取宽度</param>
        /// <param name="srcHeight">截取高度</param>
        /// <param name="backgroundColor">图片背景颜色,默认是白色,格式：#ffffff</param>
        public static void CutPhoto(ref Bitmap bitmapCut, Bitmap bitmapSource, System.Drawing.Rectangle outRectangle, float srcX, float srcY, float srcWidth, float srcHeight, string backgroundColor = "#ffffff")
        {
            Graphics g = Graphics.FromImage(bitmapCut);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.Clear(System.Drawing.ColorTranslator.FromHtml(backgroundColor.ToUpper()));
            g.DrawImage(bitmapSource, outRectangle, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel);
            g.Dispose();
        }

        /// <summary>
        /// 剪切图片,裁切png图
        /// </summary>
        /// <param name="bitmapCut">输出图片</param>
        /// <param name="bitmapSource">源图片</param>
        /// <param name="outRectangle">截取框</param>
        /// <param name="srcX">起截点X坐标</param>
        /// <param name="srcY">起截点Y坐标</param>
        /// <param name="srcWidth">截取宽度</param>
        /// <param name="srcHeight">截取高度</param>
        /// <param name="backgroundColor">图片背景颜色,默认是白色,格式：#ffffff</param>
        public static void CutPhoto2(ref Bitmap bitmapCut, Bitmap bitmapSource, System.Drawing.Rectangle outRectangle, float srcX, float srcY, float srcWidth, float srcHeight, string backgroundColor = "#ffffff")
        {
            Graphics g = Graphics.FromImage(bitmapCut);
            g.Clear(System.Drawing.ColorTranslator.FromHtml(backgroundColor.ToUpper()));
            g.DrawImage(bitmapSource, outRectangle, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel);
            g.Dispose();
        }

        /// <summary>
        /// 判断连接的字母是否过长，确定是否换行
        /// </summary>
        /// <param name="nextLineZimu">下一个字符</param>
        /// <param name="strOutputContent">当前内容</param>
        /// <returns></returns>
        public static string JudgeContentNeedNewline(char nextLineZimu, string strOutputContent)
        {
            bool hasChina = false;
            for (int i = 0; i < strOutputContent.Length; i++)
            {
                char tempChar = strOutputContent[i];
                if (!((tempChar >= 'A' && tempChar <= 'Z') || (tempChar >= 'a' && tempChar <= 'z')))
                {
                    hasChina = true;
                }
            }
            if (hasChina)
            {
                if ((nextLineZimu >= 'A' && nextLineZimu <= 'Z') || (nextLineZimu >= 'a' && nextLineZimu <= 'z'))
                {
                    nextLineZimu = strOutputContent[strOutputContent.Length - 1];
                    while ((nextLineZimu >= 'A' && nextLineZimu <= 'Z') || (nextLineZimu >= 'a' && nextLineZimu <= 'z'))
                    {
                        strOutputContent = strOutputContent.Substring(0, strOutputContent.Length - 1);
                        nextLineZimu = strOutputContent[strOutputContent.Length - 1];
                    }


                }
            }
            return strOutputContent;
        }

        /// <summary>
        /// 截断过长的文字
        /// </summary>
        /// <param name="maxWidth">允许的最大长度</param>
        /// <param name="content">文本内容</param>
        /// <param name="bfChinese"></param>
        /// <param name="space">字间距</param>
        /// <param name="pt">字体大小</param>
        /// <returns>返回一个泛型，第一个是截断后的内容，第二个是文本长度</returns>
        public static List<string> CalculateLineContentWidth(float maxWidth, string content, BaseFont bfChinese, float space, float pt)
        {
            float floLineContentSize = bfChinese.GetWidthPointKerned(content, pt) + (content.Length - 1) * space;
            while (floLineContentSize > maxWidth)
            {
                content = content.Substring(0, content.Length - 1);
                floLineContentSize = bfChinese.GetWidthPointKerned(content, pt) + (content.Length - 1) * space;
            }
            List<string> contentAndSize = new List<string>();
            contentAndSize.Add(content);
            contentAndSize.Add(floLineContentSize.ToString());
            return contentAndSize;

        }
        #region 计算宽度
        /// <summary>
        /// 中文首行缩进宽度
        /// </summary>
        /// <param name="bfChinese"></param>
        /// <param name="space"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static float CalculateChinaIndentWidth(BaseFont bfChinese, float space, float pt)
        {
            string content = "中国";
            return CalculateWidth(content,bfChinese,space,pt);
        }
        /// <summary>
        /// 中文首行缩进宽度
        /// </summary>
        /// <param name="bfChinese"></param>
        /// <param name="space"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static float CalculateChinaIndentWidth(string fontFilePath, float space, float pt)
        {
            string content = "中国";
            return CalculateWidth(content, fontFilePath, space, pt);
        }

        /// <summary>
        /// 计算单行文本的宽度
        /// </summary>
        /// <param name="content"></param>
        /// <param name="bfChinese"></param>
        /// <param name="space"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static float CalculateWidth(string content, BaseFont bfChinese, float space, float pt)
        {
            return bfChinese.GetWidthPointKerned(content, pt) + (content.Length - 1) * space;
        }
        /// <summary>
        /// 计算单行文本的宽度
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fontFilePath"></param>
        /// <param name="space"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static float CalculateWidth(string content, string fontFilePath, float space, float pt)
        {
            //这里引用itextsharp的字体，是为了计算空格的大小，因为GDI+对空格的宽度统一为0
            FontFactory.Register(fontFilePath);
            BaseFont bfChinese = BaseFont.CreateFont(fontFilePath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return CalculateWidth(content, bfChinese, space, pt);
        }


        #endregion
        /// <summary>
        /// 计算垂直文字的高度
        /// </summary>
        /// <param name="strOutputContent">文本内容</param>
        /// <param name="bfChinese">itextsharp字体对象</param>
        /// <param name="witchBox">TextBox对象</param>
        /// <returns></returns>
        public static float CalculateVerticalTextWidth(string strOutputContent, TextBox witchBox, string fontPath)
        {
            //这里引用itextsharp的字体，是为了计算空格的大小，因为GDI+对空格的宽度统一为0
            FontFactory.Register(fontPath);
            BaseFont bfChinese = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();
            privateFonts.AddFontFile(fontPath);

            System.Drawing.Font font = new System.Drawing.Font(privateFonts.Families[0], witchBox.pt, FontStyle.Regular, GraphicsUnit.Pixel);

            float kongGe = bfChinese.GetWidthPointKerned("r", witchBox.pt); //设置空格的宽度=字符"r"的宽度
            float marginLeft = kongGe / 2; //绘制英文的偏移量，因为旋转后的英文会向左边移动一点距离，这里设置为空格宽度的一半距离
            float cellHeight = 0; //两行之间的空隙距离
            if (witchBox.leading != null && witchBox.leading > 0.1f && witchBox.leading > witchBox.pt) //如果存在间距就计算两行之间的距离
            {
                cellHeight = witchBox.leading - witchBox.pt;
            }
            float defaultCellHeight = 0; //旋转字符之间的字间距，给一个默认值
            bool isRotate = false; //如果是旋转字符，那个就没有行距

            float startY = 0;
            for (int i = 0; i < strOutputContent.Length; i++)
            {
                //解决字母间距过大的问题
                if (IsRotateChar(strOutputContent[i]))
                {
                    //竖向绘制字母
                    float Cut = 0;
                    float standardWidth = bfChinese.GetWidthPointKerned("e", witchBox.pt);
                    if (i < strOutputContent.Length - 1)
                    {
                        if (!IsRotateChar(strOutputContent[i + 1]))
                        {
                            Cut = standardWidth;
                            isRotate = false; //如果下一个字符是非旋转字符，就有行距
                        }
                        else
                        {
                            isRotate = true; //如果下一个字符是旋转字符，那个就没有行距
                        }
                        //defaultCellHeight = GetRotateSpecialSpace(strOutputContent[i], strOutputContent[i + 1]); //旋转字符根据特殊情况，返回特殊间距
                    }
                    float width = bfChinese.GetWidthPointKerned(strOutputContent[i].ToString(), witchBox.pt);
                    float lessWidth = kongGe + 0.9f; //设置最小宽度标准，如果字符的宽度小于这个标准值就特殊处理
                    if (strOutputContent[i] == ' ') //如果是空格就采用空格的宽度绘制
                    {
                        width = kongGe;
                    }
                    else if (width < bfChinese.GetWidthPointKerned(" ", witchBox.pt)) //如果小于空格大小，就两倍的宽度
                    {
                        width += width;
                    }
                    else if (width < lessWidth)   //字符宽度大于空格，但小于标准值的，就用一个“e”字母的宽度来计算
                    {
                        width = standardWidth;
                    }
                    startY += width + Cut + (isRotate ? defaultCellHeight : cellHeight);
                }
                else
                {
                    float cutY = 0;
                    if (i < strOutputContent.Length - 1)
                    {
                        if (IsRotateChar(strOutputContent[i + 1]))
                        {
                            cutY = kongGe;
                        }
                    }
                    startY += font.GetHeight() + cutY + cellHeight;
                }
            }
            return startY;
        }


        #region 用于保存图片的参数指数
        /*
         *  使用例子：
         *  ImageCodecInfo jgpEncoder = PaintTools.GetEncoderByFormat(ImageFormat.Jpeg);
         *  EncoderParameters encoderParameters = PaintTools.GetEncoderParametersByNumber(100);
         *  bitMapCut.Save(AppDomain.CurrentDomain.BaseDirectory + savePicPath + "_.jpg", jgpEncoder, encoderParameters);
         */

        /// <summary>
        /// 设置图片品质
        /// </summary>
        /// <param name="compressionRate">100是最好品质</param>
        /// <returns></returns>
        public static EncoderParameters GetEncoderParametersByNumber(int compressionRate = 100)
        {
            System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionRate);//这里设品质
            encoderParameters.Param[0] = encoderParameter;
            return encoderParameters;
        }


        /// <summary>
        /// 根据图片格式返回编码
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoderByFormat(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion

        /// <summary>
        /// 根据用户设置返回文本居中模式
        /// </summary>
        /// <param name="textAlign"></param>
        /// <returns></returns>
        public static TextAlignEnum GetTextAlignEnum(string textAlign)
        {
            TextAlignEnum align;
            if (Enum.TryParse(textAlign, out align)==false)
            {
                align = TextAlignEnum.left;
            }
            return align;
        }

    }

}
