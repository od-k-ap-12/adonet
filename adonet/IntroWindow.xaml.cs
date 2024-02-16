using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace adonet
{
    /// <summary>
    /// Interaction logic for IntroWindow.xaml
    /// </summary>
    public partial class IntroWindow : Window
    {
        bool isConnected = false;
        private readonly string _msConnectionString;
        SqlConnection? msConnection;
        public IntroWindow()
        {
            InitializeComponent();
            var config=JsonSerializer.Deserialize<JsonElement>(
            System.IO.File.ReadAllText("appsettings.json"));
            var connectionStrings = config.GetProperty("ConnectionStrings");
            _msConnectionString = connectionStrings.GetProperty("LocalMS").GetString()!;
        }

        private void ConnectMSButton_Click(object sender, RoutedEventArgs e)
        {
            msConnection = new(_msConnectionString);
            try
            {
                msConnection.Open();
                MSConnectionStatusLabel.Content = "Connected";
                ConnectMSButton.IsEnabled = false;
                DisconnectMSButton.IsEnabled = true;
            }
            catch(Exception ex)
            {
                MSConnectionStatusLabel.Content = ex.Message;
            }
        }

        private void CreateMSButton_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new(); //інструмент передачі команди
            cmd.Connection = msConnection;
            cmd.CommandText = @"
            CREATE TABLE Users(
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Name NVARCHAR(64) NOT NULL,
                Login NVARCHAR(64) NOT NULL,
                PasswordHash CHAR(32) NOT NULL,
            )";
            try
            {
                cmd.ExecuteNonQuery();
                MSCreateStatusLabel.Content = "Executed";
            }
            catch(Exception ex)
            {
                MSCreateStatusLabel.Content = ex.Message;
            }
        }
/*        private String md5(String input)
        {
            using var hasher = System.Security.Cryptography.MD5.Create();
            return Convert.ToHexString(hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)));
        }*/
        private String md5(String input)
        {
            return Convert.ToHexString(
                System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(input)));
        }

        private String? GetInputError()
        {
            if (String.IsNullOrEmpty(UserNameTextBox.Text))
            {
                return "Fill Name box";
            }
            if (String.IsNullOrEmpty(UserLoginTextBox.Text))
            {
                return "Fill Login box";

            }
            if (String.IsNullOrEmpty(UserPasswordTextBox.Password))
            {
                return "Fill Password box";
            }
            return null;
        }

        private void InsertMsButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage=GetInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            using var cmd = new SqlCommand($"INSERT INTO Users VALUES(NEWID(), @name, @login,'{md5(UserPasswordTextBox.Password)}',@birthdate)", msConnection);
            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
            {
                Value=UserNameTextBox.Text
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = UserLoginTextBox.Text
            });
            cmd.Parameters.Add(new SqlParameter("@birthdate", System.Data.SqlDbType.Date, 64)
            {
                Value = !string.IsNullOrEmpty(UserBirthdateTextBox.Text) ? UserBirthdateTextBox.Text : (object)DBNull.Value
            });
            try
            {
                cmd.Prepare(); //підготовка запиту - компіляція без параметрів
                cmd.ExecuteNonQuery(); //виконання - передача даних у скомпільований запит
                InsertStatusLabel.Content = "Insert OK";
            }
            catch(Exception ex)
            {
                InsertStatusLabel.Content=ex.Message;
            }
        }

        private void DropMSButton_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new();
            cmd.Connection = msConnection;
            cmd.CommandText = @"
            DROP TABLE dbo.Users;";
            try
            {
                cmd.ExecuteNonQuery();
                MSCreateStatusLabel.Content = "Dropped";
            }
            catch (Exception ex)
            {
                MSCreateStatusLabel.Content = ex.Message;
            }
        }

        private void DisconnectMSButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                msConnection.Close();
                MSConnectionStatusLabel.Content = "Disconnected";
                DisconnectMSButton.IsEnabled = false;
                ConnectMSButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MSConnectionStatusLabel.Content = ex.Message;
            }
        }

        private void AddBirthdateMSButton_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new();
            cmd.Connection = msConnection;
            cmd.CommandText = @"
            ALTER TABLE dbo.Users 
            ADD Birthdate DATETIME;";
            try
            {
                cmd.ExecuteNonQuery();
                MSCreateStatusLabel.Content = "Added";
            }
            catch (Exception ex)
            {
                MSCreateStatusLabel.Content = ex.Message;
            }
        }

        private void SelectMSButton_Click(object sender, RoutedEventArgs e)
        {
            if(msConnection == null || msConnection.State == System.Data.ConnectionState.Closed)
            {
                MessageBox.Show("Необхідно встановити підключення","Виконання зупинене", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            using SqlCommand cmd = new("SELECT * FROM Users", msConnection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                SelectMsTextBlock.Text = "";
                while (reader.Read())
                {
                    var id = reader.GetGuid("Id").ToString();
                    var name= reader.GetString("Name");
                    var login=reader.GetString("Login");
                    var hash = reader.GetString("PasswordHash");
                    var birthdate = "";
                    if (!reader.IsDBNull("Birthdate"))
                    {
                        birthdate = (reader.GetDateTime("Birthdate").ToString("dd/MM/yyyy"));
                    }
                    SelectMsTextBlock.Text += $"{id[..5]},{name},{login},{hash[..5]},{birthdate}\n";
                }
            }
            catch(Exception ex)
            {
                SelectMsTextBlock.Text= ex.Message;
            }
        }
    }
}
