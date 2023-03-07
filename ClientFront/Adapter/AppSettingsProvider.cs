using Microsoft.Extensions.Configuration;
using System;

namespace Adapter
{
    /// <summary>
    /// The main purpose of this class load configuration json file.
    /// Hostname is url to receiver.
    /// AnalyticCookieUniqueidentifierName is name of cookie that injected in request to receiver and response to browser.
    /// If IsAnalyticEnabled params is disabled then all methods calls of adapter will be ignored.
    /// </summary>
    public static class AppSettingsProvider
    {
        private static readonly IConfiguration _configuration;

        static AppSettingsProvider()
        {
            _configuration = LoadConfiguration();
        }

        public static string HostName => _configuration.GetSection("HostName").Value;

        public static IConfiguration LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            return configuration;
        }
    }
}
