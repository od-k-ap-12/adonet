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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace adonet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void IntroButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new IntroWindow().ShowDialog();
            this.Show();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new AuthWindow().ShowDialog();
            this.Show();
        }
    }
}
/*
 * ADO NET Вступ
 * ADO.NET - технологія доступу до даних яка вводить єдиний інтерфейс для роботи з різними джерелами даних (з різними СУБД)
 * -Сама БД: ПКМ Project - Add New Item - Service based Database
 * Параметри підключення. Зазвичай, їх закладають в окремий файл з конфігурацією (appsettings.json)
 * Драйвер підключення (конектори) - додаткові бібліотеки (NuGet), які містять інструменти
 * для з'єднання з відповідною СУБД
 * SQL Server -- System.Data.SqlClient
 */
