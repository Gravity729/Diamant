using Diamant.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Products : Window
    {
        PawnshopContext db = new PawnshopContext();
        List<Product> products;
        public Products()
        {
            InitializeComponent();
            products = db.Products.Include(w => w.Employee).Include(w => w.Client).ToList();
            lvProduct.ItemsSource = products;

            if (App.UserRole == 1)
            {

                employees.Visibility = Visibility.Collapsed;
            }
            if (App.UserRole == 2)
            {

                add.Visibility = Visibility.Collapsed;
                edit.Visibility = Visibility.Collapsed;
                del.Visibility = Visibility.Collapsed;
            }
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ListViewItem listViewItem)
            {
                Product currentProducts = null;
                using (PawnshopContext db = new PawnshopContext())
                {
                    var selectedItem = listViewItem.Content as dynamic;

                    decimal products = Convert.ToDecimal(selectedItem.ProductId);

                    currentProducts = db.Products.FirstOrDefault(b => b.ProductId == products);

                    DetailProduct DetailProductWindow = new(currentProducts);
                    DetailProductWindow.Show();
                    Close();
                }
            }
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            Home wHomeWindow = new();
            wHomeWindow.Show();
            Close();
        }

        private void Client_OnClick(object sender, RoutedEventArgs e)
        {
            Clients ClientsWindow = new();
            ClientsWindow.Show();
            Close();
        }

        private void Product_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {
            Employees EmployeesWindow = new();
            EmployeesWindow.Show();
            Close();
        }

        private void addProduct_OnClick(object sender, RoutedEventArgs e)
        {
            NewProduct wNewProduct = new();
            wNewProduct.Show();
            Close();
        }

        private void editProduct_OnClick(object sender, RoutedEventArgs e)
        {
            if (lvProduct.SelectedItem != null)
            {
                Product selectedProduct = lvProduct.SelectedItem as Product;
                EditProduct wEditProduct = new EditProduct(selectedProduct);
                wEditProduct.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для редактирования");
            }
        }

        private void deleteProduct_OnClick(object sender, RoutedEventArgs e)
        {
            if (lvProduct.SelectedItem != null)
            {
                Product deleteProduct = (Product)lvProduct.SelectedItem;
                var full = deleteProduct.NameProduct + " от клиента " + deleteProduct.Client.ShortNameClient + " за " + deleteProduct.AssessedValue.ToString("C2");
                MessageBoxResult result = System.Windows.MessageBox.Show($"Вы уверены, что хотите удалить товар: {full}?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    db.Entry(deleteProduct).Reload();
                    db.Products.Remove(deleteProduct);
                    db.SaveChanges();

                    products = db.Products
                        .Include(w => w.Client)
                        .Include(w => w.Employee)
                        .ToList();

                    lvProduct.ItemsSource = products;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Пожалуйста, выберите тренировку для удаления");
            }
        }



        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                lvProduct.ItemsSource = products;
            }
            else
            {
                var filteredProducts = products.Where(p =>
                    p.NameProduct.ToLower().Contains(searchText) ||
                    p.DueDate.ToString().Contains(searchText) ||
                    p.StatusProduct.ToLower().Contains(searchText) ||
                    p.Employee.LName.ToLower().Contains(searchText) ||
                    p.Client.LName.ToLower().Contains(searchText)).ToList();

                lvProduct.ItemsSource = filteredProducts;
            }
        }

        private void returnedProduct_OnClick(object sender, RoutedEventArgs e)
        {
            ReturnedProduct wReturnedItem = new();
            wReturnedItem.Show();
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
