using adonet.EFContext;
using adonet.EFCrudViews;
using adonet.Models;
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
    /// Interaction logic for EfCrudWindow.xaml
    /// </summary>
    public partial class EfCrudWindow : Window
    {
        public EfCrudWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DepartmentsListView.ItemsSource = null;
            App.EfDataContext.Departments.Load();
            DepartmentsListView.ItemsSource =
                App.EfDataContext
                .Departments
                .Local
                .ToObservableCollection();
            ProductsListView.ItemsSource = null;
            App.EfDataContext.Products.Load();
            ProductsListView.ItemsSource =
                App.EfDataContext
                .Products
                .Local
                .ToObservableCollection();

        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if(sender is ListViewItem item && item.Content is Department department)
            {

                EfDepartmentCrudWindow dialog = new(DepartmentModel.FromEntity(department));
                dialog.ShowDialog();
                if(dialog.Action==CrudActions.Update)
                {
                    department.Name = dialog.model.Name;
                    department.InternationalName = dialog.model.InternationalName;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    department.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
            }
            else if (sender is ListViewItem item2 && item2.Content is Product product)
            {

                EfProductCrudWindow dialog = new(ProductModel.FromEntity(product));
                dialog.ShowDialog();
                if (dialog.Action == CrudActions.Update)
                {
                    product.Name = dialog.model.Name;
                    product.Price = Convert.ToDouble(dialog.model.Price);
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    product.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
            }
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
