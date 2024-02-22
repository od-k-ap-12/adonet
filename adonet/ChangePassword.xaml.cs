using adonet.DAL.DTO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private readonly User _user;
        public CrudActions SelectedAction { get; private set; }
        public ChangePassword(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Password == RepeatPasswordTextBox.Password)
            {
                SelectedAction = CrudActions.Update;
                _user.PasswordHash = App.md5(RepeatPasswordTextBox.Password);
                MessageBox.Show("Password changed");
                DialogResult = true;
            }
        }
    }
}
