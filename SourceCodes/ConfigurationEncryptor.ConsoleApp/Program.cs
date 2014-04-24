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
            switch (parameter.Direction)
            {
                case Direction.Encrypt:
                    EncryptSections(parameter.Filename, parameter.Sections);
                    break;
                case Direction.Decrypt:
                    DecryptSections(parameter.Filename, parameter.Sections);
                    break;
                default:
                    throw new InvalidOperationException("Must set the direction - Encrypt or Decrypt");
            }
        }

        private static void EncryptSections(string filename, ConsoleApp.Sections sections)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(filename);
        }

        private static void DecryptSections(string filename, ConsoleApp.Sections sections)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(filename);
        }

        private static void EncryptConnectionString(bool encrypt, string fileName)
        {
            Configuration configuration = null;
            try
            {
                // Open the configuration file and retrieve the connectionStrings section.
                configuration = ConfigurationManager.OpenExeConfiguration(fileName);
                ConnectionStringsSection configSection =
                configuration.GetSection("connectionStrings") as ConnectionStringsSection;
                if ((!(configSection.ElementInformation.IsLocked)) &&
                    (!(configSection.SectionInformation.IsLocked)))
                {
                    if (encrypt && !configSection.SectionInformation.IsProtected)
                    {
                        //this line will encrypt the file
                        configSection.SectionInformation.ProtectSection
                            ("DataProtectionConfigurationProvider");
                    }

                    if (!encrypt &&
                    configSection.SectionInformation.IsProtected)//encrypt is true so encrypt
                    {
                        //this line will decrypt the file.
                        configSection.SectionInformation.UnprotectSection();
                    }
                    //re-save the configuration file section
                    configSection.SectionInformation.ForceSave = true;
                    // Save the current configuration

                    configuration.Save();
                    Process.Start("notepad.exe", configuration.FilePath);
                    //configFile.FilePath
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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