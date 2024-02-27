using adonet.EFContext;
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
            DisplayStatistics();
        }
        private void DisplayStatistics()
        {
            ManagersCountLabel.Content = App.EfDataContext.Managers.Count();
            ProductsCountLabel.Content = App.EfDataContext.Products.Count();
            DepartmentsCountLabel.Content = App.EfDataContext.Departments.Count();
            SalesCountLabel.Content = App.EfDataContext.Sales.Count();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(var dep in App.EfDataContext.Departments)
            {
                ResultLabel.Content += $"{dep.Name}\n";
            }
            var query2 = App.EfDataContext
                .Departments
                .Select(d => d.Name);
            ResultLabel.Content += query2.Min();
            var query3 = App.EfDataContext
                .Departments
                .OrderBy(d => d.Id)
                .AsEnumerable()
                .Select(d => new { Card = d.Id.ToString()[..5] + " " + d.Name })
                .OrderBy(a => a.Card)
                .First();
            ResultLabel.Content += "\n"+query3.Card + "\n";
        }

        private void CheapButton_Click(object sender, RoutedEventArgs e)
        {
            var query4 = App.EfDataContext
                .Products
                .OrderBy(d => d.Price)
                .Select(p => p.Name + " " + p.Price)
                .First();
            ResultLabel.Content += query4.ToString();
            var query5 = App.EfDataContext
                .Products
                .Select(p => p.Price);
            ResultLabel.Content += "\n";
            ResultLabel.Content += "Average price: "+ query5.Average().ToString();
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            SalesCountLabel.Content = "Updating...";
            Task.Run(AddSales);
        }
        private async Task AddSales()
        {
            DateTime start = new DateTime(2023, 1, 1);

            DateTime randomDate = start
                .AddDays(App.Random.Next(365))
                .AddHours(App.Random.Next(9, 20))
                .AddMinutes(App.Random.Next(0, 59))
                .AddSeconds(App.Random.Next(0, 59));
            for (int i = 0; i < 10000; i++)
            {
                App.EfDataContext.Sales.Add(new EFContext.Sale()
                {
                    Id = Guid.NewGuid(),
                    ManagerId = App.EfDataContext.Managers.OrderBy(r => Guid.NewGuid()).First().Id,
                    ProductId = App.EfDataContext.Products.OrderBy(r => Guid.NewGuid()).First().Id,
                    Quantity = App.Random.Next(1, 10),
                    SaleDt = randomDate,
                });
            }
            await App.EfDataContext.SaveChangesAsync();
            Dispatcher.Invoke(DisplayStatistics);
        }

        private void ProductSalesButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new(2023, 02, 23);
            var query = App.EfDataContext.Products
                .GroupJoin(
                    App.EfDataContext.Sales,
                    p => p.Id,
                    s => s.ProductId,
                    (product, sales) => new
                    {
                        Name = product.Name,
                        Pcs = sales.Sum(s => s.Quantity)
                    }
                );
            foreach( var item in query )
            {
                ResultLabel.Content += $"{item.Name}\t{item.Pcs}\n";
            }
        }

        private void LinqButton_Click(object sender, RoutedEventArgs e)
        {
            /* Д.З. Скласти EF(LINQ) запити на одержання наступних даних:
            * - самий молодий менеджер
            * - товар з найкоротшою назвою
            * - випадковий товар (за натиском кнопки дані оновлюються)
            * - випадковий менеджер
            * Також додати інформацію (вивести) про випадкову дату (у межах 2023 року)
            *  та час у межах 9:00 до 20:00
            */
            ResultLabel.Content = string.Empty;
            var query1 = App.EfDataContext
                .Managers
                .OrderBy(m => m.Age)
                .Select(m => m.Name + " " + m.Age)
                .First();
            ResultLabel.Content += "youngest manager: " + query1 + " \n";

            var query2 = App.EfDataContext
              .Products
              .OrderBy(p=>p.Name)
              .Select(p => p.Name)
              .First();
            ResultLabel.Content += "shortest name: " + query2 +" \n";

            var query3 = App.EfDataContext
                .Managers
                .OrderBy(m => Guid.NewGuid())
                .Select(m => m.Name)
                .First();
            ResultLabel.Content += "random manager: " + query3 + " \n";

            var query4 = App.EfDataContext
                .Products
                .OrderBy(p => Guid.NewGuid())
                .Select(p => p.Name)
                .First();
            ResultLabel.Content += "random product: " + query4 + " \n";

            DateTime start = new DateTime(2023, 1, 1);

            DateTime randomDate = start
                .AddDays(App.Random.Next(365))
                .AddHours(App.Random.Next(9, 20))
                .AddMinutes(App.Random.Next(0, 59))
                .AddSeconds(App.Random.Next(0, 59));
            ResultLabel.Content += "random date: " + randomDate + " \n";
        }
    }
}

