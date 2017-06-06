using CourseManager.Common.Extensions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CourseManager.Common.ThirdParty.FFmpeg
{
    /// <summary>
    /// 视频处理类
    /// </summary>
    public class FFmpegHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoPath">视频文件物理路径</param>
        /// <returns></returns>
        public static FFmpegHelper GetInstance(string videoPath)
        {
            return new FFmpegHelper(videoPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoPath">视频文件物理路径</param>
        public FFmpegHelper(string videoPath)
        {
            _videoPath = videoPath;
        }

        #region 属性

        readonly string _ffmpegPath=WebHelper.GetMapPath(@"\Configs\ffmpeg\ffmpeg.exe");

        string _videoPath;

        StringBuilder _outMsg = new StringBuilder();

        #endregion
        /// <summary>
        /// 获取视频的某帧处的图片
        /// </summary>
        /// <param name="imageSaveFullPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="duration">指定帧处图片</param>
        public void GetVideoFirstImage(string imageSaveFullPath,int width,int height,int duration=4)//980x490
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(imageSaveFullPath));
            if (dirInfo.Exists == false)
            {
                dirInfo.Create();
            }
            string args = string.Format("-i \"{0}\" -ss {1} -vframes 1 -r 1 -ac 1 -ab 2 -s {2}*{3} -f image2 \"{4}\"", _videoPath, duration, width, height, imageSaveFullPath.Replace("\\","/"));
            using (Process p = new Process())
            {
                p.StartInfo.FileName = _ffmpegPath;//要调用外部程序的绝对路径
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(_videoPath);
                p.StartInfo.Arguments = args;//参数(这里就是FFMPEG的参数了)
                p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序线程(一定为FALSE,详细的请看MSDN)
                p.StartInfo.CreateNoWindow = false;//不创建窗口
                p.Start();//启动线程
                //Thread.Sleep(3000);
                p.WaitForExit();
            }
        }

        /// <summary>
        /// 获取视频的信息
        /// </summary>
        /// <returns></returns>
        public VideoInfo GetVideoInfo()
        {
            VideoInfo info = new VideoInfo();
            GetOutPutMsg();
            string wh = "";//
            if (_outMsg.Length > 0)
            {
                string data = _outMsg.ToString();
                wh = data.Match("(\\d{3,4})x(\\d{3,4})");
                if (wh.IsNotNullAndNotWhiteSpace())
                {
                    int[] intWH = wh.Split(new[] { 'x' }).Select(o => int.Parse(o)).ToArray();
                    info.Width = intWH[0];
                    info.Height = intWH[1];
                }
                string dura = data.Match("Duration: \\d{2}:\\d{2}:\\d{2}");
                if (dura.IsNotNullAndNotEmpty())
                {
                    info.Duration = dura.Replace("Duration:", "").Trim();
                }
                string bitrate = data.Match("bitrate: \\d{1,5} kb\\/s");
                if (bitrate.IsNotNullAndNotEmpty())
                {
                    info.Bitrate = bitrate.ToLower().Replace("bitrate:", "").Trim();
                }
            }
            return info;

        }


        private void GetOutPutMsg()
        {
            string args = string.Format("-i \"{0}\"", _videoPath);
            using (Process p = new Process())
            {
                p.StartInfo.FileName = _ffmpegPath;//要调用外部程序的绝对路径
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(_videoPath);
                p.StartInfo.Arguments = args;//参数(这里就是FFMPEG的参数了)
                p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序线程(一定为FALSE,详细的请看MSDN)
                p.StartInfo.CreateNoWindow = false;//不创建窗口
                p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的
                p.ErrorDataReceived += new DataReceivedEventHandler((obj, output) =>
                {
                    _outMsg.AppendLine(output.Data);
                });//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN
                p.Start();//启动线程
                //Thread.Sleep(3000);
                p.BeginErrorReadLine();//开始异步读取
                p.WaitForExit();
            }
        }

    }
}
