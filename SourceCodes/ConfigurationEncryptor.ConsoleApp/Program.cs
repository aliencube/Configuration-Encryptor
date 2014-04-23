using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationEncryptor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public static void EncryptConnectionString(bool encrypt, string fileName)
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
}
