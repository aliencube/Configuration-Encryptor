using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Aliencube.ConfigurationEncryptor.ConsoleApp
{
    /// <summary>
    /// This represents the cryption service entity.
    /// </summary>
    internal class CryptionService
    {
        private readonly Parameter _param;
        private readonly NameValueCollection _settings;

        /// <summary>
        /// Initialises a new instance of the CryptionService class.
        /// </summary>
        /// <param name="param">Parameter instance.</param>
        /// <param name="settings">AppSettings instance.</param>
        public CryptionService(Parameter param, NameValueCollection settings)
        {
            this._param = param;
            this._settings = settings;
        }

        private string _encryptionProvider;

        /// <summary>
        /// Gets the encryption provider.
        /// </summary>
        public string EncryptionProvider
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._encryptionProvider))
                    this._encryptionProvider = this._settings["EncryptionProvider"];

                return this._encryptionProvider;
            }
        }

        /// <summary>
        /// Executes encryption or decryption.
        /// </summary>
        /// <returns>Returns the configuration path encrypted/decrypted.</returns>
        public string Execute()
        {
            string filepath;
            switch (this._param.Direction)
            {
                case Direction.Encrypt:
                    filepath = this.EncryptSections(this._param.Filename, this._param.Sections);
                    break;

                case Direction.Decrypt:
                    filepath = this.DecryptSections(this._param.Filename, this._param.Sections);
                    break;

                default:
                    throw new InvalidOperationException("Must set the direction - Encrypt or Decrypt");
            }

            return filepath;
        }

        /// <summary>
        /// Encrypts configuration sections.
        /// </summary>
        /// <param name="filename">App.config/Web.config filename.</param>
        /// <param name="sections">Sections enum value.</param>
        /// <returns>Returns the configuration file path.</returns>
        private string EncryptSections(string filename, Sections sections)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(filename);
            if (sections.HasFlag(Sections.ConnectionStrings))
            {
                var connectionStringsSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;
                if (IsSectionAvailable(connectionStringsSection, Direction.Encrypt))
                {
                    connectionStringsSection.SectionInformation.ProtectSection(this.EncryptionProvider);
                    connectionStringsSection.SectionInformation.ForceSave = true;
                }
            }

            if (sections.HasFlag(Sections.AppSettings))
            {
                var appSettingsSection = configuration.GetSection("appSettings") as AppSettingsSection;
                if (IsSectionAvailable(appSettingsSection, Direction.Encrypt))
                {
                    appSettingsSection.SectionInformation.ProtectSection(this.EncryptionProvider);
                    appSettingsSection.SectionInformation.ForceSave = true;
                }
            }
            configuration.Save();
            return configuration.FilePath;
        }

        /// <summary>
        /// Decrypts configuration sections.
        /// </summary>
        /// <param name="filename">App.config/Web.config filename.</param>
        /// <param name="sections">Sections enum value.</param>
        /// <returns>Returns the configuration file path.</returns>
        private string DecryptSections(string filename, Sections sections)
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

        /// <summary>
        /// Checks whether the configuration section is available or not.
        /// </summary>
        /// <param name="section">ConfigurationSection instance.</param>
        /// <param name="direction">Direction to encrypt or decrypt.</param>
        /// <returns>Returns <c>True</c>, if the configuration section is available; otherwise returns <c>False</c>.</returns>
        private bool IsSectionAvailable(ConfigurationSection section, Direction direction)
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
}