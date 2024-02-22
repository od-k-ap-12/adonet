using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for EFWindow.xaml
    /// </summary>
    public partial class EFWindow : Window
    {
        public EFWindow()
        {
            App.EfDataContext.Database.Migrate();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ManagersCountLabel.Content = App.EfDataContext.Managers.Count();
            ProductsCountLabel.Content = App.EfDataContext.Products.Count();
            DepartmentsCountLabel.Content = App.EfDataContext.Departments.Count();
            SalesCountLabel.Content = App.EfDataContext.Sales.Count();
        }
    }
}

