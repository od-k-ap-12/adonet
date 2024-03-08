using adonet.Models;
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

namespace adonet.EFCrudViews
{
    /// <summary>
    /// Interaction logic for EfManagerCrudWindow.xaml
    /// </summary>
    public partial class EfManagerCrudWindow : Window
    {
        public ManagerModel model { get; init; }
        public CrudActions Action { get; private set; }
        public EfManagerCrudWindow(ManagerModel model)
        {
            InitializeComponent();
            this.model = model;
            this.DataContext = this;
            Action = CrudActions.None;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.None;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.Update;
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.Delete;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainDepComboBox.SelectedItem =
                this.model.Departments.First(idName=>idName.Id==model.MainDep.Id);
            if (model.SecDep != null)
            {
                SecDepComboBox.SelectedItem =
                this.model.Departments.First(idName => idName.Id == model.SecDep.Id);
            }
            if (model.Chief != null)
            {
                ChiefComboBox.SelectedItem=model.Chiefs.First(m=>m.Id==model.Chief.Id);
            }
        }

        private void ClearSecDep_Click(object sender, RoutedEventArgs e)
        {
            SecDepComboBox.SelectedItem = null;
        }

        private void ClearChief_Click(object sender, RoutedEventArgs e)
        {
            SecDepComboBox.SelectedIndex= -1;
        }

        private void ChiefComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.Chief = ChiefComboBox.SelectedItem as IdName;
        }

        private void SecDepComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.SecDep=SecDepComboBox.SelectedItem as IdName;
        }

        private void MainDepComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.MainDep = (IdName)MainDepComboBox.SelectedItem;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
