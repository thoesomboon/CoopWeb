using System;
using System.Configuration;

namespace Coop.Configuration
{
    internal sealed class ApplicationConfig
    {
        private static readonly ApplicationConfig instance = new ApplicationConfig();

        static ApplicationConfig()
        {

        }

        private ApplicationConfig()
        {

        }

        internal static ApplicationConfig Instance
        {
            get
            {
                return instance;
            }
        }
    }
}