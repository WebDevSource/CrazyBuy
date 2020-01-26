using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;

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
	}
}
