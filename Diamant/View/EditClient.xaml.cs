using Diamant.Models;
using Microsoft.VisualBasic.Logging;
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
    public partial class EditClient : Window
    {
        private Client client;
        PawnshopContext db = new();
        public EditClient(Client selectedClient)
        {
            InitializeComponent();
            client = selectedClient;
            LoadClientData();
        }

        private void LoadClientData()
        {
            LName.Text = client.LName;
            FName.Text = client.FName;
            PName.Text = client.PName;
            BDate.Text = client.BDate.ToString();
            PassportSeries.Text = client.SPassport.ToString();
            PassportNumber.Text = client.NPassport.ToString();
            Phone.Text = client.Phone.ToString();
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

            var existingClientWithPhone = db.Clients.FirstOrDefault(cl => cl.Phone == Phone.Text && cl.ClientId != client.ClientId);
            if (existingClientWithPhone != null)
            {
                errorMessages.Add("Номер телефона уже используется другим клиентом");
            }

            var existingClientWithPassport = db.Clients.FirstOrDefault(cl => cl.SPassport == PassportSeries.Text && cl.NPassport == PassportNumber.Text && cl.ClientId != client.ClientId);
            if (existingClientWithPassport != null)
            {
                errorMessages.Add("Серия и номер паспорта уже используются другим клиентом");
            }

            if (errorMessages.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте следующие ошибки:\n" + string.Join("\n", errorMessages));
                return;
            }

            client.LName = LName.Text.Trim();
            client.FName = FName.Text.Trim();
            client.PName = PName.Text.Trim();
            client.BDate = DateOnly.Parse(BDate.Text.Trim());
            client.SPassport = PassportSeries.Text.Trim();
            client.NPassport = PassportNumber.Text.Trim();
            client.Phone = Phone.Text.Trim();

            db.Clients.Update(client);
            db.SaveChanges();

            MessageBox.Show("Данные клиента успешно обновлены :)");
            Clients ClientsWindow = new();
            ClientsWindow.Show();
            Close();
        }

        private void BtCansel_Click(object sender, RoutedEventArgs e)
        {
            Clients ClientsWindow = new();
            ClientsWindow.Show();
            Close();
        }
    }
}
