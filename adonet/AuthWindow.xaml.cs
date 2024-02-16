using adonet.DAL.DAO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace adonet
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }
        private String? GetInputError()
        {
            if (String.IsNullOrEmpty(RegName.Text))
            {
                return "Fill Name box";
            }
            if (String.IsNullOrEmpty(RegLogin.Text))
            {
                return "Fill Login box";
            }
            if (String.IsNullOrEmpty(RegPassword.Password))
            {
                return "Fill Password box";
            }
            if (RegPassword.Password != RegRepeat.Password)
            {
                return "Passwords mismatch";
            }
            return null;
        }
        private String md5(String input)
        {
            return Convert.ToHexString(
                System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(input)));
        }
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = GetInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage,
                    "Виконання зупинене",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
           /* using var cmd = new SqlCommand(
                $"INSERT INTO Users VALUES( NEWID(), @name, @login, '{md5(RegPassword.Password)}' )",
                App.MsSqlConnection);
            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
            {
                Value = RegName.Text
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = RegLogin.Text
            });*/
            try
            {
                if(UserDao.AddUser(new(){
                    Name = RegName.Text,
                    Login = RegLogin.Text,
                    PasswordHash=md5(RegPassword.Password),
                    Birthdate = DateTime.Now,
                }))
                {
                    MessageBox.Show("Insert OK");
                }
                else
                {
                    MessageBox.Show("Insert fails");
                }
/*                cmd.Prepare();  // підготовка запиту - компіляція без параметрів
                cmd.ExecuteNonQuery();  // виконання - передача даних у скомпільований запит
                MessageBox.Show("Insert OK");*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserDao.GetUserByCredentials(LogLogin.Text, md5(LogPassword.Password))==null)
            {
                MessageBox.Show("Login or Password isn't correct");
            }
            else
            {
                MessageBox.Show("Information is correct");
            }
        }
    }
}
