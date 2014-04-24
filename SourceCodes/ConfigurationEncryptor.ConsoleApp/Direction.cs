namespace Aliencube.ConfigurationEncryptor.ConsoleApp
{
    /// <summary>
    /// This determines the direction for cryption.
    /// </summary>
    internal enum Direction
    {
        /// <summary>
        /// Identifies that no direction is given.
        /// </summary>
        None = 0,

        /// <summary>
        /// Identifies encryption.
        /// </summary>
        Encrypt = 1,

        /// <summary>
        /// Identifies decription.
        /// </summary>
        Decrypt = 2
    }
}