using adonet.EFContext;
using adonet.EFCrudViews;
using adonet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
        private ICollectionView departmentsView;
        private readonly Predicate<Object> departmentsFilter = obj => (obj as Department)?.DeleteDt == null;
        private Task? dbTask;
        public EfCrudWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            LoadManagerData();
        }
        private void LoadManagerData()
        {
            ManagersListView.ItemsSource = null;
            App.EfDataContext.Managers.Load();
            ManagersListView.ItemsSource =
                App.EfDataContext
                .Managers
                .Local
                .ToObservableCollection();
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

            departmentsView = CollectionViewSource.GetDefaultView(DepartmentsListView.ItemsSource);
            departmentsView.Filter = obj => (obj as Department)?.DeleteDt == null;

        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if(sender is System.Windows.Controls.ListViewItem item && item.Content is Department department)
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
            else if (sender is System.Windows.Controls.ListViewItem item2 && item2.Content is Product product)
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
            Department department = new()
            {
                Id = Guid.NewGuid(),
            };
            EfDepartmentCrudWindow dialog = new(DepartmentModel.FromEntity(department));
            dialog.ShowDialog();
            if (dialog.Action == CrudActions.Update)
            {
                department.Name = dialog.model.Name;
                department.InternationalName = dialog.model.InternationalName;
                App.EfDataContext.Add(department);
                App.EfDataContext.SaveChanges();
                LoadData();
            }
        }

        private void AllDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            departmentsView.Filter=departmentsView.Filter == null ? obj => (obj as Department)?.DeleteDt == null : null;
            if (departmentsView.Filter == null) { AllDepartmentButton.Content = "Hide"; }
            else { AllDepartmentButton.Content = "All"; }
        }

        private void AddManagerButton_Click(object sender, RoutedEventArgs e)
        {
 /*           Manager entity = new() { Id = Guid.NewGuid() };
            ManagerModel model = new(entity)
            {
                Departments = App.EfDataContext.
                    Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new IdName { Id = d.Id, Name = d.Name })
                    .ToList(),
                *//*                    MainDep= new IdName { Id = manager.MainDepartment.Id, Name = manager.MainDepartment.Name }*//*
                Chiefs = App.EfDataContext
                    .Chiefs
                    .Select(m => new IdName
                    {
                        Id = m.Id,
                        Name = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}."
                    })
                    .ToList(),
            };
            EfManagerCrudWindow dialog = new(model);
            dialog.ShowDialog();*/
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if(sender is System.Windows.Controls.ListViewItem item && item.Content is Manager manager)
            {
                EfManagerCrudWindow dialog = new(new ManagerModel(manager)
                {
                    Departments = App.EfDataContext.
                    Departments
                    .OrderBy(d => d.Name)
                    .Select(d=>new IdName { Id=d.Id,Name=d.Name})
                    .ToList(),
/*                    MainDep= new IdName { Id = manager.MainDepartment.Id, Name = manager.MainDepartment.Name }*/
                    Chiefs=App.EfDataContext
                    .Chiefs
                    .Select(m=>new IdName
                    {
                        Id=m.Id,
                        Name=$"{m.Surname} {m.Name[0]}. {m.Secname[0]}."
                    })
                    .ToList(),
                });

                dialog.ShowDialog();

                if (dialog.Action == CrudActions.Update)
                {
                    manager.Surname = dialog.model.Surname;
                    manager.Name= dialog.model.Name;
                    manager.Secname = dialog.model.Secname;
                    manager.IdMainDep = dialog.model.MainDep.Id;
                    manager.IdSecDep = dialog.model.SecDep?.Id;
                    manager.IdChief=dialog.model.Chief?.Id;
                    dbTask = App.EfDataContext
                        .SaveChangesAsync()
                        .ContinueWith(_ => Dispatcher.Invoke(LoadManagerData));
                }
                else if(dialog.Action == CrudActions.Delete)
                {
                    manager.DeleteDt = DateTime.Now;
                }
                if (dialog.Action != CrudActions.None)
                {
                    dbTask = App.EfDataContext
                        .SaveChangesAsync()
                        .ContinueWith(_ => Dispatcher.Invoke(LoadManagerData));

                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            dbTask?.Wait();
        }
    }
}
