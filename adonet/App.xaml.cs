using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

    }
}
