using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
        //將密碼用MD5加密
        public static string ConverToMD5(string Password)
        {
            MD5 md5 = MD5.Create();//建立一個MD5
            byte[] source = Encoding.Default.GetBytes(Password);//將字串轉為Byte[]
            byte[] crypto = md5.ComputeHash(source);//進行MD5加密
            string password_md5 = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串
            return password_md5;
        }
    }

}
