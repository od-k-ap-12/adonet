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
    /// Interaction logic for EfProductCrudWindow.xaml
    /// </summary>
    public partial class EfProductCrudWindow : Window
    {
        public ProductModel model { get; init; }
        public CrudActions Action { get; private set; }
        public EfProductCrudWindow(ProductModel model)
        {
            InitializeComponent();
            this.model = model;
            this.DataContext = this;
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
    }
}
