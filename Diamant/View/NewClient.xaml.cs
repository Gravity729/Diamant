using Diamant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class NewClient : Window
    {
        PawnshopContext db;
        public NewClient()
        {
            InitializeComponent();
            db = new PawnshopContext();
        }

        private void BtCansel_Click(object sender, RoutedEventArgs e)
        {
            Clients ClientsWindow = new();
            ClientsWindow.Show();
            Close();
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
                    errorMessages.Add("Клиенту должно быть не менее 18 лет для регистрации.");
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
            string lName = LName.Text.Trim();
            string fName = FName.Text.Trim();
            string pName = PName.Text.Trim();
            string phone = Phone.Text.Trim();

            if (db.Clients.Any(cl => cl.Phone == phone))
            {
                errorMessages.Add("Номер телефона уже используется другим сотрудником");
            }

            if (db.Clients.Any(cl => cl.SPassport == PassportSeries.ToString() && cl.NPassport == PassportNumber.ToString()))
            {
                errorMessages.Add("Серия и номер паспорта уже используются другим сотрудником");
            }

            if (errorMessages.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте следующие ошибки:\n" + string.Join("\n", errorMessages));
                return;
            }

            if (DateTime.TryParse(BDate.Text.Trim(), out DateTime bDate) && int.TryParse(PassportNumber.Text.Trim(), out int passportNumber) &&
                     int.TryParse(PassportSeries.Text.Trim(), out int passportSeries))
            {
                Client newClient = new Client
                {
                    LName = lName,
                    FName = fName,
                    PName = pName,
                    BDate = DateOnly.FromDateTime(bDate),
                    Phone = phone,
                    SPassport = passportSeries.ToString(),
                    NPassport = passportNumber.ToString()
                };

                db.Clients.Add(newClient);
                db.SaveChanges();
                MessageBox.Show("Запись добавлена успешно");

                Clients ClientsWindow = new();
                ClientsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные данные!");
            }
        }
    }
}
