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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace adonet
{
    /// <summary>
    /// Interaction logic for PracticeWindow.xaml
    /// </summary>
    public partial class PracticeWindow : Window
    {
        public PracticeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            QuestLabel1.Content = "";
            DateTime today = new DateTime(2023, DateTime.Now.Month, DateTime.Now.Day);

            var query1 = App.EfDataContext
                .Sales
                .Where(s => s.SaleDt.Date == today.Date)
                .Select(s => s.SaleDt.TimeOfDay)
                .Min();
            QuestLabel1.Content = query1;

            var query2 = App.EfDataContext
                .Sales
                .Where(s => s.SaleDt.Date == today.Date)
                .Select(s => s.SaleDt.TimeOfDay)
                .Max();
             QuestLabel2.Content = query2;

            var query3 = App.EfDataContext
                .Sales
                .Where(s=>s.SaleDt.Date == today.Date)
                .Select(s=>s.Quantity)
                .Max();
            QuestLabel3.Content = query3;

            var query4 = App.EfDataContext
                .Sales
                .Where(s=>s.SaleDt.Date==today.Date)
                .Select(s=>s.Quantity)
                .Average();
            QuestLabel4.Content = query4;

            var query5 = App.EfDataContext
                .Sales
                .Include(s => s.Product)
                .Where(s => s.SaleDt.Date == today.Date)
                .Select(s => s.Quantity * s.Product.Price)
                .Average();
            QuestLabel5.Content = Math.Floor(query5) + " грн";

            DateTime? today2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var query6 = App.EfDataContext
                .Sales
                .Where(s => s.DeleteDt.Value.Date == today2.Value.Date)
                .Count();
            var query6_2= App.EfDataContext
                .Sales
                .Include(s=>s.Product)
                .Where(s => s.DeleteDt.Value.Date == today2.Value.Date)
                .Select(s=>s.Quantity*s.Product.Price)
                .Sum();
            QuestLabel6.Content = "Повернуті чеки: " + query6 +" на суму " + Math.Floor(query6_2) + " грн";

            var query7 = App.EfDataContext.Managers
               .GroupJoin(
                   App.EfDataContext.Sales,
                   p => p.Id,
                   s => s.ManagerId,
                   (manager, sales) => new
                   {
                       Name = manager.Surname+" "+manager.Name+" "+manager.Secname,
                       Pcs = sales.Where(s => s.SaleDt.Date == today.Date).Sum(s => s.Quantity)
                   }
               ).ToList();

            foreach (var manager in query7)
            {
                if (manager.Pcs ==query7.Max(s=>s.Pcs))
                {
                    QuestLabel7.Content = "Найкращий продавець: " + manager.Name + " - " + manager.Pcs + " шт";
                }
            }
        }
    }
}
