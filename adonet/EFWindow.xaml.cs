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
            DateTime start = new DateTime(2023, 3, 11);

            DateTime randomDate = start
                .AddDays(0)
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
            DateTime date = new(2023, 03, 11);
            var query = App.EfDataContext.Products
                .GroupJoin(
                    App.EfDataContext.Sales,
                    p => p.Id,
                    s => s.ProductId,
                    (product, sales) => new
                    {
                        Name = product.Name,
                        Pcs = sales.Where(s=>s.SaleDt.Date == date.Date).Sum(s => s.Quantity)
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

        private void Linq2Button_Click(object sender, RoutedEventArgs e)
        {
            /* Д.З. Скласти EF(LINQ) запити на одержання наступних даних:
            * - статистику продажів по менеджерах за "сьогодні" - за день, як сьогодні, тільки за звітний рік БД (2023)
            * 
            * Також модифікувати алгоритм визначення дати, щоб вона
            * відповідала поточній даті, додати відомості про дату до виводу статистики
            */


            ResultLabel.Content = "";
            DateTime date = new DateTime(2023, DateTime.Now.Month, DateTime.Now.Day); 
            var query = App.EfDataContext.Managers
                .GroupJoin(
                    App.EfDataContext.Sales,
                    p => p.Id,
                    s => s.ManagerId,
                    (manager, sales) => new
                    {
                        Name = manager.Surname,
                        Pcs = sales.Where(s => s.SaleDt.Date == date.Date).Sum(s => s.Quantity)
                    }
                );
            ResultLabel.Content += date.Day.ToString()+"."+date.Month.ToString()+"."+date.Year.ToString() + "\n";
            foreach (var item in query)
            {
                ResultLabel.Content += $"{item.Name} {item.Pcs}\n";
            }
        }

        private void NavButton1_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";
            foreach(var man in
            App.EfDataContext.Managers.
                Include(m => m.MainDepartment)
                .Take(5)){
                ResultLabel.Content += $"{man.Surname} {man.MainDepartment.Name}\n";
            }
            ResultLabel.Content += "\n";
            foreach(var str in
            App.EfDataContext
                .Departments
                .Include(d=>d.MainWorkers)
                .Select(d => $"{d.Name} {d.MainWorkers.Count}"))
            {
                ResultLabel.Content += $"{str}\n";
            }

        }

        private void NavButton2_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";
            foreach(var str in
            App.EfDataContext
                .Managers
                .Include(m=> m.SecondaryDepartment)
                .Select(m=>$"{m.Surname} {(m.SecondaryDepartment==null?"--":m.SecondaryDepartment.Name)}")
                /*.Take(15)*/)
            {
                ResultLabel.Content += $"{str}\n";
            }
        }
        private void NavHwButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";
            /* Д.З. "Навігаційні властивості"
             * Запит на "відділ за сумісництвом" - додати нумерацію рядків результату
             *  (бажано у складі запиту)
             * Додати виведення інверсної навігаційної властивості - кількості
             *  працівників за сумісництвом, що працюють у відділах
             *  
             * * Скласти SQL запити, аналогічні EF-запитам
             */
            var query = App.EfDataContext
                 .Managers
                 .Include(m => m.SecondaryDepartment)
                 .Select(m => $"{m.Surname} {(m.SecondaryDepartment == null ? "--" : m.SecondaryDepartment.Name)}")
                 .ToList();
            foreach (var item in query.Select((item, i) => $"{i + 1}. {item}"))
            {
                ResultLabel.Content += $"{item}\n";
            }

            ResultLabel.Content += $"--------------------------------------\n";

            var query2 = App.EfDataContext
                .Departments
                .Include(d => d.SecondaryWorkers)
                .Select(d => $"{d.Name} {d.SecondaryWorkers.Count()}")
                .ToList();
            foreach (var item in query2.Select((item, i) => $"{i + 1}. {item}"))
            {
                ResultLabel.Content += $"{item}\n";
            }
        }

        private void ChiefButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";
            foreach (Manager man in
            App.EfDataContext
                .Managers
                .Include(m => m.Chief))
            {
                ResultLabel.Content += $"{man.Surname}--{man.Chief?.Surname ?? "no chief"}\n";
            }

        }

        private void SubordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";
            foreach (Manager man in
            App.EfDataContext
                .Managers
                .Include(m => m.Subordinates))
            {
                ResultLabel.Content += $"{man.Surname}--{String.Join(",",man.Subordinates.Select(m=>m.Surname))}\n";
            }

        }

        private void SalesProdButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new(2023, 03, 11);
            ResultLabel.Content = "";
            foreach (Product p in
            App.EfDataContext
                .Products
                .Include(p => p.Sales))
            {
                ResultLabel.Content += $"{p.Name}--{p.Sales.Where(s=>s.SaleDt.Date==date.Date).Count()}\n";
            }
        }

        private void Linq3Button_Click(object sender, RoutedEventArgs e)
        {
             /* Д.З. Скласти EF(LINQ) запити на одержання наступних даних:
             * - статистику продажів по товарах за "сьогодні" 
             *    Назва товару -- кількість чеків, шт -- кількість товарів, шт. -- обіг, грн.
             *    
             * Тренуватись у налаштуванні зв'язків через контекст даних
             */
            DateTime date = new DateTime(2023, DateTime.Now.Month, DateTime.Now.Day);
            ResultLabel.Content = "";
            foreach (Product p in
            App.EfDataContext
                .Products
                .Include(p => p.Sales))
            {
                ResultLabel.Content += $"{p.Name}--к-сть чеків: {p.Sales.Where(s => s.SaleDt.Date == date.Date).Count()}--к-сть товарів: {p.Sales.Where(s => s.SaleDt.Date == date.Date).Sum(s => s.Quantity)}--обіг,грн: {p.Price*p.Sales.Where(s => s.SaleDt.Date == date.Date).Sum(s => s.Quantity)}\n\n";
            }
        }

    }
}

