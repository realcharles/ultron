using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVC.Core
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public class GUtil
    {
        public static string GetMd5Hash(string input)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }
        public static string GenerateUniqueId(int num)
         {
             string randomResult = string.Empty;
             string readyStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
             char[] rtn = new char[num];
             Guid gid = Guid.NewGuid();
             var ba = gid.ToByteArray();
             for (var i = 0; i < num; i++)
             {
                 rtn[i] = readyStr[((ba[i] + ba[num + i]) % 35)];
             }
             foreach (char r in rtn)
             {
                 randomResult += r;
             }
             return randomResult;
         }
        /// <summary>
        /// 将币种代码转换为币种符合
        /// </summary>
        /// <param name="bzdm"></param>
        /// <returns></returns>
        public static string BZ2FH(string bzdm)
        {
            if (string.IsNullOrEmpty(bzdm))
                return "";
            switch (bzdm.ToUpper())
            {
                case "USD":
                    return "$ ";
                case "EUR":
                    return "€ ";
                case "RMB":
                case "CNY":
                    return "￥ ";
                case "CAD":
                    return "C$ ";
                case "AUD":
                    return "$A ";
                case "GBP":
                    return "￡ ";
                case "HKD":
                    return "HK$ ";
                default:
                    return bzdm + " ";
            }
        }

        /// <summary>
        /// 根据挂单方式加载可以执行的方法
        /// </summary>
        /// <param name="listingType"></param>
        /// <returns></returns>
        public static string GetNextStatus(string listingType)
        {
            IList<object> list = new List<object>();
            switch (listingType)
            {
                case "FixedPriceItem":
                case "StoresFixedPrice":
                case "PersonalOffer":
                case "Fixed":
                    {
                        list.Add(new { text = "修改价格&库存", value = "UpdatePriceOrKC" });
                        list.Add(new { text = "下架", value = "OffStore" });
                        list.Add(new { text = "查看日志", value = "ShowLog" });
                    }
                    break;
                case "Chinese":
                case "Dutch":
                case "Live":
                case "Auction":
                    {
                        list.Add(new { text = "下架", value = "OffStore" });
                        list.Add(new { text = "查看日志", value = "ShowLog" });
                    }
                    break;
            }
            if (list.Count == 0)
                return "";
            return list.toJSON();
        }
    }
}
