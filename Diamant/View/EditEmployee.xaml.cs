using Diamant.Models;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Shapes;

namespace Diamant.View
{
    public partial class EditEmployee : Window
    {
        private Employee employee;
        PawnshopContext db = new();
        public EditEmployee(Employee selectedEmployee)
        {
            InitializeComponent();
            employee = selectedEmployee;
            LoadEmployeeData();
        }
        private void LoadEmployeeData()
        {
            LName.Text = employee.LName;
            FName.Text = employee.FName;
            PName.Text = employee.PName;
            BDate.Text = employee.BDate.ToString();
            PassportSeries.Text = employee.SPassport.ToString();
            PassportNumber.Text = employee.NPassport.ToString();
            Phone.Text = employee.Phone.ToString();
            Login.Text = employee.LoginE.ToString();
            Pass.Text = employee.PasswordE.ToString();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            List<string> errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(LName.Text))
            {
                errorMessages.Add("Фамилия не может быть пустой");
            }

            if (string.IsNullOrWhiteSpace(FName.Text))
            {
                errorMessages.Add("Имя не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(BDate.Text) || !DateTime.TryParse(BDate.Text, out _))
            {
                errorMessages.Add("Дата рождения должна быть в формате дд.мм.гггг");
            }
            else
            {
                DateTime birthDate = DateTime.Parse(BDate.Text);
                if (birthDate.AddYears(18) > DateTime.Now)
                {
                    errorMessages.Add("Сотруднику должно быть не менее 18 лет для регистрации.");
                }
            }

            if (string.IsNullOrWhiteSpace(PassportSeries.Text) || !Regex.IsMatch(PassportSeries.Text, @"^\d{4}$"))
            {
                errorMessages.Add("Серия паспорта должна состоять из 4 цифр");
            }

            if (string.IsNullOrWhiteSpace(PassportNumber.Text) || !Regex.IsMatch(PassportNumber.Text, @"^\d{6}$"))
            {
                errorMessages.Add("Номер паспорта должен состоять из 6 цифр");
            }

            if (string.IsNullOrWhiteSpace(Phone.Text) || !Regex.IsMatch(Phone.Text, @"^\d{11}$"))
            {
                errorMessages.Add("Номер телефона должен состоять из 11 цифр и быть в формате 81234567890");
            }

            if (string.IsNullOrWhiteSpace(Login.Text) || Login.Text.Length < 5)
            {
                errorMessages.Add("Логин не может быть пустым, он должен состоять минимум из 5 символов");
            }

            var existingEmployeeWithLogin = db.Employees.FirstOrDefault(emp => emp.LoginE == Login.Text && emp.EmployeeId != employee.EmployeeId);
            if (existingEmployeeWithLogin != null)
            {
                errorMessages.Add("Логин уже используется другим сотрудником");
            }

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^])[A-Za-z\d!@#$%^]{6,}$";
            if (string.IsNullOrWhiteSpace(Pass.Text) || !Regex.IsMatch(Pass.Text, passwordPattern))
            {
                errorMessages.Add("Пароль должен содержать минимум 6 символов, минимум 1 прописную букву, минимум 1 цифру и минимум один символ из набора: ! @ # $ % ^");
            }

            var existingEmployeeWithPhone = db.Employees.FirstOrDefault(emp => emp.Phone == Phone.Text && emp.EmployeeId != employee.EmployeeId);
            if (existingEmployeeWithPhone != null)
            {
                errorMessages.Add("Номер телефона уже используется другим сотрудником");
            }

            var existingEmployeeWithPassport = db.Employees.FirstOrDefault(emp => emp.SPassport == PassportSeries.Text && emp.NPassport == PassportNumber.Text && emp.EmployeeId != employee.EmployeeId);
            if (existingEmployeeWithPassport != null)
            {
                errorMessages.Add("Серия и номер паспорта уже используются другим сотрудником");
            }
            if (errorMessages.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте следующие ошибки:\n" + string.Join("\n", errorMessages));
                return;
            }

            employee.LName = LName.Text.Trim();
            employee.FName = FName.Text.Trim();
            employee.PName = PName.Text.Trim();
            employee.BDate = DateOnly.Parse(BDate.Text.Trim());
            employee.SPassport = PassportSeries.Text.Trim();
            employee.NPassport = PassportNumber.Text.Trim();
            employee.Phone = Phone.Text.Trim();
            employee.LoginE = Login.Text.Trim();
            employee.PasswordE = Pass.Text.Trim();

            db.Employees.Update(employee);
            db.SaveChanges();

            MessageBox.Show("Данные сотрудника успешно обновлены :)");
            Employees EmployeesWindow = new();
            EmployeesWindow.Show();
            Close();
        }

        private void BtCansel_Click(object sender, RoutedEventArgs e)
        {
            Employees EmployeesWindow = new();
            EmployeesWindow.Show();
            Close();
        }
    }
}
