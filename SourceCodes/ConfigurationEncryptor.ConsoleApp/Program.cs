using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace Aliencube.ConfigurationEncryptor.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parameter = new Parameter(args);
            string filepath;
            switch (parameter.Direction)
            {
                case Direction.Encrypt:
                    filepath = EncryptSections(parameter.Filename, parameter.Sections);
                    break;
                case Direction.Decrypt:
                    filepath = DecryptSections(parameter.Filename, parameter.Sections);
                    break;
                default:
                    throw new InvalidOperationException("Must set the direction - Encrypt or Decrypt");
            }

            Console.WriteLine(ConfigurationManager.ConnectionStrings["ConsoleAppContext"].ConnectionString);
            Console.WriteLine(ConfigurationManager.AppSettings["ConsoleAppKey"]);

            Process.Start("notepad.exe", filepath);
        }

        private static string EncryptSections(string filename, ConsoleApp.Sections sections)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(filename);
            if (sections.HasFlag(Sections.ConnectionStrings))
            {
                var connectionStringsSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;
                if (IsSectionAvailable(connectionStringsSection, Direction.Encrypt))
                {
                    connectionStringsSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                    connectionStringsSection.SectionInformation.ForceSave = true;
                }
            }

            if (sections.HasFlag(Sections.AppSettings))
            {
                var appSettingsSection = configuration.GetSection("appSettings") as AppSettingsSection;
                if (IsSectionAvailable(appSettingsSection, Direction.Encrypt))
                {
                    appSettingsSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                    appSettingsSection.SectionInformation.ForceSave = true;
                }
            }
            configuration.Save();
            return configuration.FilePath;
        }

        private static string DecryptSections(string filename, ConsoleApp.Sections sections)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(filename);
            if (sections.HasFlag(Sections.ConnectionStrings))
            {
                var connectionStringsSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;
                if (IsSectionAvailable(connectionStringsSection, Direction.Decrypt))
                {
                    connectionStringsSection.SectionInformation.UnprotectSection();
                    connectionStringsSection.SectionInformation.ForceSave = true;
                }
            }

            if (sections.HasFlag(Sections.AppSettings))
            {
                var appSettingsSection = configuration.GetSection("appSettings") as AppSettingsSection;
                if (IsSectionAvailable(appSettingsSection, Direction.Decrypt))
                {
                    appSettingsSection.SectionInformation.UnprotectSection();
                    appSettingsSection.SectionInformation.ForceSave = true;
                }
            }
            configuration.Save();
            return configuration.FilePath;
        }

        private static bool IsSectionAvailable(ConfigurationSection section, Direction direction)
        {
            if (section == null || section.ElementInformation.IsLocked || section.SectionInformation.IsLocked)
                return false;

            if (direction == Direction.Encrypt)
                return !section.SectionInformation.IsProtected;


            if (direction == Direction.Decrypt)
                return section.SectionInformation.IsProtected;

            return false;
        }
    }

    internal class Parameter
    {
        public Parameter(string[] args)
        {
            this.SetParameter(args);
        }

        public Direction Direction { get; private set; }
        public string Filename { get; private set; }
        public Sections Sections { get; private set; }

        private void SetParameter(string[] args)
        {
            this.Direction = Direction.None;
            var direction = args.FirstOrDefault(p => p.ToLower() == "/e" || p.ToLower() == "/d");
            if (!String.IsNullOrWhiteSpace(direction))
            {
                switch (direction)
                {
                    case "/e":
                        this.Direction = Direction.Encrypt;
                        break;
                    case "/d":
                        this.Direction = Direction.Decrypt;
                        break;
                }
            }

            var filename = args.FirstOrDefault(p => p.ToLower().StartsWith("/c:"));
            if (!String.IsNullOrWhiteSpace(filename))
                this.Filename = filename.Replace("/c:", "");

            this.Sections = Sections.None;
            foreach (var arg in args.Where(p => !p.ToLower().StartsWith("/")))
            {
                ConsoleApp.Sections result;
                this.Sections |= Enum.TryParse(arg, true, out result) ? result : Sections.None;
            }
        }
    }

    enum Direction
    {
        None = 0,
        Encrypt = 1,
        Decrypt = 2
    }

    [Flags]
    enum Sections
    {
        None = 0,
        AppSettings = 1 << 0,
        ConnectionStrings = 1 << 1,
    }
}