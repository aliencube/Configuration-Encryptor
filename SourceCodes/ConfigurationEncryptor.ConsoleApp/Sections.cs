using System;

namespace Aliencube.ConfigurationEncryptor.ConsoleApp
{
    /// <summary>
    /// This determines the configuration sections.
    /// </summary>
    [Flags]
    internal enum Sections
    {
        /// <summary>
        /// Identifies that no section is given.
        /// </summary>
        None = 0,

        /// <summary>
        /// Identifies the "appSettings" section.
        /// </summary>
        AppSettings = 1 << 0,

        /// <summary>
        /// Identifies the "connectionStrings" sections.
        /// </summary>
        ConnectionStrings = 1 << 1,
    }
}