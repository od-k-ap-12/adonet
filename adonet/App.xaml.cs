using adonet.EFContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace adonet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EFContext.EfContext EfDataContext { get;} = new();
        public static void LogError(string message, [CallerMemberName] string callerName="undefined")
        {
            System.IO.File.AppendAllText("logs.txt", $"{DateTime.Now}[{callerName}]{message}");
        }
        private static SqlConnection? _msConnection;
        public static SqlConnection MsSqlConnection
        {
            get
            {
                if(_msConnection == null)
                {
                    _msConnection = new(
                        JsonSerializer.Deserialize<JsonElement>(
                            System.IO.File.ReadAllText("appsettings.json")
                        )
                        .GetProperty("ConnectionStrings")
                        .GetProperty("LocalMS")
                        .GetString()!
                    );
                    _msConnection.Open();

                }
                return _msConnection;
            }
        }
        public static String md5(String input)
        {
            return Convert.ToHexString(
                System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(input)));
        }

    }
}
