using Diamant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Diamant.View
{
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            if (App.UserRole == 1)
            {
                labelWelcome.Content = "Здравствуйте, " + App.currentEmployee.FName + " " + App.currentEmployee.PName + "!";
                employees.Visibility = Visibility.Collapsed;
            }
            else if (App.UserRole == 2)
            {
                labelWelcome.Content = "Привет, Админ!";
            }

        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Client_OnClick(object sender, RoutedEventArgs e)
        {
            Clients ClientsWindow = new();
            ClientsWindow.Show();
            Close();
        }

        private void Product_OnClick(object sender, RoutedEventArgs e)
        {
            Products ProductsWindow = new();
            ProductsWindow.Show();
            Close();
        }

        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {
            Employees EmployeesWindow = new();
            EmployeesWindow.Show();
            Close();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Authorization wAuthorization = new();
            wAuthorization.Show();
            Close();
        }
    }
}
