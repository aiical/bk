using System;

namespace CourseManager.Common
{
    public sealed class IdentityCreator
    {
        /*
         * var guid = Guid.NewGuid();         
         * guid.ToString("D")   10244798-9a34-4245-b1ef-9143f9b1e68a
         * guid.ToString("N")   102447989a344245b1ef9143f9b1e68a
         * guid.ToString("B")   {10244798-9a34-4245-b1ef-9143f9b1e68a}
         * guid.ToString("P")   (10244798-9a34-4245-b1ef-9143f9b1e68a)
         * 不区另大小写
         */
        /// <summary>
        /// 获取一个Guid，作为主键，推荐方式
        /// </summary>
        public static string NextIdentity
        {
            get
            {
                return Guid.NewGuid().ToString("N").Substring(new Random().Next(0, 17), 15) + DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            }
        }
        /// <summary>
        /// 创建一个Guid
        /// </summary>
        public static string NewGuid
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }

        /// <summary>
        /// 获取时间戳 DateTime.Now - 1970
        /// </summary>
        public static int TimeStamp
        {
            get
            {
                return ((Int32)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            }
        }
        /// <summary>
        /// 获取一个订单号
        /// </summary>
        /// <returns></returns>
        public static string CreateOrderNo()
        {
            string randomstr = "";
            int count = 4;
            Random[] rnds = new Random[count];
            for (int i = 0; i < count; i++)
            {
                rnds[i] = new Random(unchecked((int)(DateTime.Now.Ticks >> i)));
                randomstr += rnds[i].Next(100).ToString().PadLeft(2, '0');
            }
            randomstr = randomstr.Substring(new Random().Next(0, 4), 4);
            randomstr = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + randomstr;
            return randomstr;
        }
    }

}
