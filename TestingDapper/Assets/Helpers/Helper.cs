using System.Configuration;

namespace Dapper
{
    public static class Helper
    {
        public static string ConnectionString() => ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public static string ConnectionString(int timeout) =>
            ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{timeout}", timeout.ToString());

    }
}
