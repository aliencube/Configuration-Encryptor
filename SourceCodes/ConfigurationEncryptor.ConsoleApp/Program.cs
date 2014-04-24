using System;
using System.Configuration;
using System.Diagnostics;

namespace Aliencube.ConfigurationEncryptor.ConsoleApp
{
    /// <summary>
    /// This represents the main program entity.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Performs the main program entity.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        private static void Main(string[] args)
        {
            var param = new Parameter(args);
            var settings = ConfigurationManager.AppSettings;

            var service = new CryptionService(param, settings);
            var filepath = service.Execute();

            if ((ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection) != null && !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ConnectionStringName"]))
                Console.WriteLine(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionStringName"]].ConnectionString);

            if ((ConfigurationManager.GetSection("appSettings") as AppSettingsSection) != null && !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AppSettingKey"]))
                Console.WriteLine(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AppSettingKey"]]);

            Process.Start("notepad.exe", filepath);
        }
    }
}