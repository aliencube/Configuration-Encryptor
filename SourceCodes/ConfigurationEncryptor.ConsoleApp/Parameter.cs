using System;
using System.Linq;

namespace Aliencube.ConfigurationEncryptor.ConsoleApp
{
    /// <summary>
    /// This represents the parameter entity.
    /// </summary>
    internal class Parameter
    {
        /// <summary>
        /// Initialises a new instance of the Parameter class.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public Parameter(string[] args)
        {
            this.SetParameter(args);
        }

        /// <summary>
        /// Gets the direction for encryption or decryption.
        /// </summary>
        public Direction Direction { get; private set; }

        /// <summary>
        /// Gets the filename of App.config or Web.config.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets the configuration sections.
        /// </summary>
        public Sections Sections { get; private set; }

        /// <summary>
        /// Sets the parameters from the list of arguments.
        /// </summary>
        /// <param name="args">List of arguments.</param>
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
}