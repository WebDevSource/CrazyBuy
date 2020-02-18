using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace CrazyBuy.Common
{
    public class Utils
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static string GetConfiguration(string configurationKey)
        {
            string result = "";

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();

                result = Configuration[configurationKey];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MD5_Encode(string data)
        {
            byte[] byKey = System.Text.Encoding.Default.GetBytes(GetConfiguration("MD5_key").Substring(5, 8));
            byte[] byIV = System.Text.Encoding.Default.GetBytes(GetConfiguration("MD5_key").Substring(5, 8));
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            cst.Write(System.Text.Encoding.Default.GetBytes(data), 0, System.Text.Encoding.Default.GetByteCount(data));
            //sw.Write(data);   
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MD5_Decode(string data)
        {
            //把金鑰轉成二進位制陣列
            byte[] byKey = System.Text.Encoding.Default.GetBytes(GetConfiguration("MD5_key").Substring(5, 8));
            byte[] byIV = System.Text.Encoding.Default.GetBytes(GetConfiguration("MD5_key").Substring(5, 8));
            byte[] byEnc;
            try
            {
                //base64解碼
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            byte[] tmp = new byte[ms.Length];
            cst.Read(tmp, 0, tmp.Length);
            string result = System.Text.Encoding.Default.GetString(tmp);
            return result.Replace("\0", "");
        }

    }

}
