//using CourseManager.Common.ItextSharp.PdfBase;

using CourseManager.Common.ItextSharp.PdfBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace  CourseManager.Common
{
    /// <summary>
    /// 缩略图帮助类
    /// </summary>
    public class ThumbnailHelper
    {
        /// <summary>
        /// 生成缩略图，可能有空白，等比缩放
        /// </summary>
        /// <param name="sourceImagePath"> 源图的路径(含文件名及扩展名) </param>
        /// <param name="desImagePath"> 生成的缩略图所保存的路径(含文件名及扩展名)注意：扩展名一定要与生成的缩略图格式相对应 </param>
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值) </param>
        /// <param name="height"> 欲生成的缩略图 "画布" 的高度(像素值) </param>
        public static void GenerateImage(string sourceImagePath, string desImagePath, int width, int height)
        {
            using (Image imageFrom = Image.FromFile(sourceImagePath))
            {
                // 源图宽度及高度
                int imageFromWidth = imageFrom.Width;
                int imageFromHeight = imageFrom.Height;

                // 生成的缩略图实际宽度及高度
                int bitmapWidth = width;
                int bitmapHeight = height;

                // 生成的缩略图在上述"画布"上的位置
                int X = 0;
                int Y = 0;

                // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置
                if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
                {
                    bitmapHeight = imageFromHeight * width / imageFromWidth;
                    Y = (height - bitmapHeight) / 2;
                }
                else
                {
                    bitmapWidth = imageFromWidth * height / imageFromHeight;
                    X = (width - bitmapWidth) / 2;
                }
                Bitmap bmp = new Bitmap(width, height);
                // 创建画布
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // 清除整个绘图面并以透明背景色填充
                    g.Clear(Color.White);
                    // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。
                    g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
                    bmp.Save(desImagePath, GetImageFormat(desImagePath));
                }
            }

        }

        /// <summary>
        /// 生成缩略图，不留空白，等比缩放，边沿部分可能被裁切掉
        /// </summary>
        /// <param name="sourceImagePath"></param>
        /// <param name="desImagePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fileType">image/video</param>
        public static Tuple<int, int> GenerateImage2(string sourceImagePath, string desImagePath, int width, int height, string fileType = "image")
        {
            Bitmap imageFrom = new Bitmap(sourceImagePath);
            //Rotate180FlipNone   指定不进行翻转的 180 度旋转。
            //  Rotate180FlipX  指定后接水平翻转的 180 度旋转。
            //  Rotate180FlipXY 指定后接水平翻转和垂直翻转的 180 度旋转。
            //  Rotate180FlipY  指定后接垂直翻转的 180 度旋转。
            //  Rotate270FlipNone   指定不进行翻转的 270 度旋转。
            //  Rotate270FlipX  指定后接水平翻转的 270 度旋转。
            //  Rotate270FlipXY 指定后接水平翻转和垂直翻转的 270 度旋转。
            //  Rotate270FlipY  指定后接垂直翻转的 270 度旋转。
            //  Rotate90FlipNone    指定不进行翻转的 90 度旋转。
            //  Rotate90FlipX   指定后接水平翻转的 90 度旋转。
            //  Rotate90FlipXY  指定后接水平翻转和垂直翻转的 90 度旋转。
            //  Rotate90FlipY   指定后接垂直翻转的 90 度旋转。
            //  RotateNoneFlipNone  指定不进行旋转和翻转。
            //  RotateNoneFlipX 指定没有后跟水平翻转的旋转。
            //  RotateNoneFlipXY    指定没有后跟水平和垂直翻转的旋转。
            //  RotateNoneFlipY 指定没有后跟垂直翻转的旋转。
            // 源图宽度及高度
            int imgSourceW = imageFrom.Width;
            int imgSourceH = imageFrom.Height;
            EXIFextractor er = new EXIFextractor(ref imageFrom, "\n");
            if (er.properties["Orientation"] != null && er.properties["Orientation"].ToString() == "6" || fileType == "video")//Orientation值为6即顺时针90
            {
                imgSourceW = imageFrom.Height;
                imgSourceH = imageFrom.Width;
                imageFrom.RotateFlip(RotateFlipType.Rotate90FlipNone);//Rotate270FlipNone 
            }
            Tuple<int, int> wh = Tuple.Create(imgSourceW, imgSourceH);
            // 生成的缩略图在上述"画布"上的位置
            int X = 0;
            int Y = 0;

            int cutW = 0, cutH = 0;
            float whPerSour = (float)imgSourceW / imgSourceH;
            float whPerDesc = (float)width / height;
            if (whPerSour >= whPerDesc)
            {
                cutH = imgSourceH;
                cutW = imgSourceH * width / height;
                X = (imgSourceW - cutW) / 2;
            }
            else
            {
                cutW = imgSourceW;
                cutH = imgSourceW * height / width;
                Y = (imgSourceH - cutH) / 2;
            }
            Bitmap bitmapCut = new Bitmap(width, height);

            if (Path.GetExtension(sourceImagePath).ToUpper().Contains("PNG"))
            {
                PaintTools.CutPhoto2(ref bitmapCut, imageFrom, new Rectangle(0, 0, width, height), X, Y, cutW, cutH);
            }
            else
            {
                PaintTools.CutPhoto(ref bitmapCut, imageFrom, new Rectangle(0, 0, width, height), X, Y, cutW, cutH);
            }
            bitmapCut.Save(desImagePath, PaintTools.GetEncoderByFormat(GetImageFormat(desImagePath)), PaintTools.GetEncoderParametersByNumber(95));
            imageFrom.Dispose();
            bitmapCut.Dispose();
            return wh;
        }

        public static void GenerateCompressImage(string sourceImagePath, string desImagePath, int compressionRate = 100)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(desImagePath));
            if (dirInfo.Exists == false)
            {
                dirInfo.Create();
            }
            
            if (new List<string> { ".PNG", ".GIF" }.Contains( Path.GetExtension(sourceImagePath).ToUpper()))
            {
                new FileInfo(sourceImagePath).CopyTo(desImagePath);
            }
            else {
                using (Bitmap imageFrom = new Bitmap(sourceImagePath))
                {
                    imageFrom.Save(desImagePath, PaintTools.GetEncoderByFormat(GetImageFormat(desImagePath)), PaintTools.GetEncoderParametersByNumber(compressionRate));
                }
            }
               
        }

        public static ImageFormat GetImageFormat(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToUpper();
            switch (ext)
            {
                case ".GIF":
                    return ImageFormat.Gif;
                case ".BMP":
                    return ImageFormat.Bmp;
                case ".PNG":
                    return ImageFormat.Png;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        public static bool IsImage(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToUpper();
            switch (ext)
            {
                case ".GIF":
                case ".BMP":
                case ".PNG":
                case ".JPG":
                case ".JPEG":
                    return true;
                default:
                    return false;
            }
        }


        private Image _srcImage;

        private string _srcFileName;

        public bool SetImage(string fileName)
        {
            _srcFileName = WebHelper.GetMapPath(fileName);
            try
            {
                _srcImage = Image.FromFile(_srcFileName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        public Image GetImage(int width, int height)
        {
            Image.GetThumbnailImageAbort callb = ThumbnailCallback;
            Image img = _srcImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
            return img;
        }

        public void SaveThumbnailImage(int width, int height)
        {
            string extension = Path.GetExtension(_srcFileName);
            if (extension != null)
                switch (extension.ToLower())
                {
                    case ".png":
                        SaveImage(width, height, ImageFormat.Png);
                        break;
                    case ".gif":
                        SaveImage(width, height, ImageFormat.Gif);
                        break;
                    default:
                        SaveImage(width, height, ImageFormat.Jpeg);
                        break;
                }
        }

        public void SaveImage(int width, int height, ImageFormat imgformat)
        {
            if ((Equals(imgformat, ImageFormat.Gif) || (_srcImage.Width <= width)) && (_srcImage.Height <= height))
                return;
            Image.GetThumbnailImageAbort callb = ThumbnailCallback;
            Image img = _srcImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
            _srcImage.Dispose();
            img.Save(_srcFileName, imgformat);
            img.Dispose();
        }



        public static void MakeSquareImage(Image image, string newFileName, int newSize)
        {
            int width = image.Width;
            int height = image.Height;

            var b = new Bitmap(newSize, newSize);

            try
            {
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;

                g.Clear(Color.Transparent);
                g.DrawImage(image, new Rectangle(0, 0, newSize, newSize),
                        width < height
                            ? new Rectangle(0, (height - width) / 2, width, width)
                            : new Rectangle((width - height) / 2, 0, height, height), GraphicsUnit.Pixel);

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetImageFormat(newFileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                b.Dispose();
            }
        }
        /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bmp">原始图Bitmap</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>输出Bitmap</returns>
        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;
            PixelFormat pf;
            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }
            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();
            tmp.Dispose();
            return dst;
        }
        public static void MakeSquareImage(string fileName, string newFileName, int newSize)
        {
            MakeSquareImage(Image.FromFile(fileName), newFileName, newSize);
        }

        public static void MakeRemoteSquareImage(string url, string newFileName, int newSize)
        {
            Stream stream = GetRemoteImage(url);
            if (stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeSquareImage(original, newFileName, newSize);
        }

        public static void MakeThumbnailImage(Image original, string newFileName, int maxWidth, int maxHeight)
        {
            Size newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
            Image displayImage = new Bitmap(original, newSize);

            try
            {
                displayImage.Save(newFileName, original.RawFormat);
            }
            finally
            {
                original.Dispose();
            }
        }

        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            MakeThumbnailImage(Image.FromFile(fileName), newFileName, maxWidth, maxHeight);
        }

        public static void MakeRemoteThumbnailImage(string url, string newFileName, int maxWidth, int maxHeight)
        {
            Stream stream = GetRemoteImage(url);
            if (stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeThumbnailImage(original, newFileName, maxWidth, maxHeight);
        }

        private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)100));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] codecInfo = ImageCodecInfo.GetImageEncoders();
            return codecInfo.FirstOrDefault(ici => ici.MimeType == mimeType);
        }

        private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            var MAX_WIDTH = (decimal)maxWidth;
            var MAX_HEIGHT = (decimal)maxHeight;
            decimal aspectRatio = MAX_WIDTH / MAX_HEIGHT;
            var originalWidth = (decimal)width;
            var originalHeight = (decimal)height;
            int newWidth, newHeight;
            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;

                if (originalWidth / originalHeight > aspectRatio)
                {
                    factor = originalWidth / MAX_WIDTH;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            return new Size(newWidth, newHeight);
        }

        private static Stream GetRemoteImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentLength = 0;
            request.Timeout = 20000;
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }
    }
}
