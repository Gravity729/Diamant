using Diamant.Models;
using Diamant.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diamant
{
    public partial class Authorization : Window
    {
        Employee authEmployee = new();
        public Authorization()
        {
            InitializeComponent();
        }

        private void Authorization_OnClick(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();

            string error;
            byte result;

            result = Employee.CheckAuthorization(login, pass, ref authEmployee, out error);

            Home wHomeWindow;

            switch (result)
            {
                case 0:
                    System.Windows.MessageBox.Show(error);
                    break;
                case 1:
                    App.UserRole = 1;

                    using (PawnshopContext context = new())
                    {
                        authEmployee = context.Employees.FirstOrDefault(e => e.LoginE == login);
                    }

                    App.currentEmployee = authEmployee;
                    System.Windows.MessageBox.Show("Добро пожаловать, сотрудник!");
                    wHomeWindow = new();
                    wHomeWindow.Show();
                    Close();
                    break;
                case 2:
                    App.UserRole = 2;
                    App.currentEmployee = authEmployee;
                    System.Windows.MessageBox.Show("Добро пожаловать, администратор!");
                    wHomeWindow = new();
                    wHomeWindow.Show();
                    Close();
                    break;
                case 3:
                    textBoxLogin.ToolTip = error;
                    System.Windows.MessageBox.Show(error);
                    textBoxLogin.Background = Brushes.LightCoral;
                    break;
                case 4:
                    passBox.ToolTip = error;
                    System.Windows.MessageBox.Show(error);
                    passBox.Background = Brushes.LightCoral;
                    break;
            }
        }
    }
}