using adonet.DAL.DAO;
using adonet.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CrudWindow.xaml
    /// </summary>
    public partial class CrudWindow : Window
    {
        public ObservableCollection<User> Users { get; set; }
        public CrudWindow()
        {
            Users = new();
            this.DataContext = this;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (User user in UserDao.GetAll())
            {
                Users.Add(user);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListViewItem item) //item=sender as ListViewItem
            {
                if(item.Content is User user)
                {
                    UserWindow dialog = new(user);
                    dialog.ShowDialog();
                    MessageBox.Show(dialog.SelectedAction.ToString());
                    User UpdatedUser = new User(Guid.Parse(dialog.IdTextBox.Text),dialog.NameTextBox.Text,dialog.LoginTextBox.Text,dialog.DkTextBox.Text,dialog.BirthdateDatePicker.SelectedDate);
                    switch(dialog.SelectedAction)
                    {
                        case CrudActions.Update: UpdateUser(UpdatedUser); break;
                        case CrudActions.Delete: DeleteUser(user); break;
                    }
                }

            }
        }
        private void UpdateUser(User user)
        {
            if(UserDao.UpdateUser(user))
            {
                MessageBox.Show("Successfully updated");
                foreach (var u in Users)
                {
                    if (u.Id == user.Id)
                    {
                        Users.Remove(u);
                        break;
                    }
                }
                Users.Add(user);
            }
            else
            {
                MessageBox.Show("Update failed");
            }
        }
        private void DeleteUser(User user)
        {
            {
                if (UserDao.DeleteUser(user))
                {
                    MessageBox.Show("Successfully deleted");
                    Users.Remove(user);
                }
                else
                {
                    MessageBox.Show("Delete failed");
                }
            }
        }
    }
}
