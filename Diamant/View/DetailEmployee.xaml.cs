using Diamant.Models;
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

namespace Diamant.View
{
    public partial class DetailEmployee : Window
    {
        public DetailEmployee(Employee currentEmployees)
        {
            InitializeComponent();
            DataContext = currentEmployees;
            using (var db = new PawnshopContext())
            {
                lname.Content = currentEmployees.LName;
                fname.Content = currentEmployees.FName;
                pname.Content = string.IsNullOrEmpty(currentEmployees.PName) ? "---" : currentEmployees.PName;
                bdate.Content = currentEmployees.BDate.ToString();
                phone.Content = currentEmployees.Phone.ToString();
                spassport.Content = currentEmployees.SPassport.ToString();
                npassport.Content = currentEmployees.NPassport.ToString();
                login.Content = currentEmployees.LoginE.ToString();
                pass.Content = currentEmployees.PasswordE.ToString();
            }
        }

        private void back_OnClick(object sender, RoutedEventArgs e)
        {
            Employees EmployeesWindow = new();
            EmployeesWindow.Show();
            Close();
        }
    }
}
