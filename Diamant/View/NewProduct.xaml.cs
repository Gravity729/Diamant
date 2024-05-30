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
    public partial class NewProduct : Window
    {
        private PawnshopContext db = new();
        public NewProduct()
        {
            InitializeComponent();
            LoadClient();

            if (App.currentEmployee != null)
            {
                EmployeeFIO.Text = App.currentEmployee.FullNameEmployee;
            }
            else
            {
                MessageBox.Show("Ошибка: Сотрудник не найден!");
            }
        }
        private void LoadClient()
        {
            ClientFIO.ItemsSource = db.Clients.ToList();
            ClientFIO.DisplayMemberPath = "FullNameClient";
            ClientFIO.SelectedValuePath = "ClientId";
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            List<string> errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                errorMessages.Add("Наименование не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(ClientFIO.Text))
            {
                errorMessages.Add("Поле ФИО клиента не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(AssessedValue.Text) || !decimal.TryParse(AssessedValue.Text, out _))
            {
                errorMessages.Add("Оценочная стоимость не может быть пустой");
            }

            if (string.IsNullOrWhiteSpace(BailAmount.Text) || !decimal.TryParse(BailAmount.Text, out _))
            {
                errorMessages.Add("Выданная сумма не может быть пустой");
            }

            if (string.IsNullOrWhiteSpace(DueDate.Text) || !DateTime.TryParse(DueDate.Text, out _))
            {
                errorMessages.Add("Дата сдачи должна быть в формате дд.мм.гггг");
            }

            if (string.IsNullOrWhiteSpace(ShelfLife.Text) || !DateTime.TryParse(ShelfLife.Text, out _))
            {
                errorMessages.Add("Срок хранения должен быть в формате дд.мм.гггг");
            }
            if (errorMessages.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте следующие ошибки:\n" + string.Join("\n", errorMessages));
                return;
            }


            string name = Name.Text.Trim();
            string statusProduct = StatusProduct.Text.Trim();

            if (ClientFIO.SelectedItem != null &&
                int.TryParse(ClientFIO.SelectedValue.ToString(), out int clientId) &&
                decimal.TryParse(AssessedValue.Text.Trim(), out decimal assessedValue) &&
                decimal.TryParse(BailAmount.Text.Trim(), out decimal bailAmount) &&
                DateOnly.TryParse(DueDate.Text.Trim(), out DateOnly dueDate) &&
                DateOnly.TryParse(ShelfLife.Text.Trim(), out DateOnly shelfLife))
            {
                var newProduct = new Product
                {
                    NameProduct = name,
                    AssessedValue = assessedValue,
                    BailAmount = bailAmount,
                    DueDate = dueDate,
                    ShelfLife = shelfLife,
                    StatusProduct = statusProduct.ToString(),
                    ClientId = clientId,
                    EmployeeId = App.currentEmployee.EmployeeId
                };
                if (dueDate > shelfLife)
                {
                    errorMessages.Add("Дата хранения не может быть раньше даты сдачи товара");
                }
                if (errorMessages.Any())
                {
                    MessageBox.Show("Пожалуйста, исправьте следующие ошибки:\n" + string.Join("\n", errorMessages));
                    return;
                }
                db.Products.Add(newProduct);
                db.SaveChanges();
                MessageBox.Show("Товар успешно добавлен!");
                Products ProductsWindow = new();
                ProductsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные данные!");
            }
        }

        private void BtCansel_Click(object sender, RoutedEventArgs e)
        {
            Products ProductsWindow = new();
            ProductsWindow.Show();
            Close();
        }
    }
}
