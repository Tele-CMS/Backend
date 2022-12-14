using System;
using System.IO;
#if NET45
#else
using Microsoft.Extensions.Configuration;
#endif

namespace HC.NLog.Targets.ElasticSearch
{
    internal static class StringExtensions
    {
        public static object ToSystemType(this string field, Type type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return Convert.ToBoolean(field);
                case "System.Double":
                    return Convert.ToDouble(field);
                case "System.DateTime":
                    return Convert.ToDateTime(field);
                case "System.Int32":
                    return Convert.ToInt32(field);
                case "System.Int64":
                    return Convert.ToInt64(field);
                default:
                    return field;
            }
        }

        public static string GetConnectionString(this string name)
        {
            var value = GetEnvironmentVariable(name);
            if (!string.IsNullOrEmpty(value))
                return value;
#if NET45
            var connectionString = ConfigurationManager.ConnectionStrings[name];
            return connectionString?.ConnectionString;
#else
            IConfigurationRoot configuration;
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
            return configuration["ElasticsearchUrl"];
#endif

        }

        private static string GetEnvironmentVariable(this string name)
        {
            return string.IsNullOrEmpty(name) ? null : Environment.GetEnvironmentVariable(name);
        }
    }
}