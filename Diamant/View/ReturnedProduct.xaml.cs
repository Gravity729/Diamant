using Diamant.Models;
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

namespace Diamant.View
{
    public partial class ReturnedProduct : Window
    {
        PawnshopContext db = new PawnshopContext();
        List<ReturnedItem> returnedItems;
        List<object> anonymousReturnedItems;

        public ReturnedProduct()
        {
            InitializeComponent();

            returnedItems = db.ReturnedItems.ToList();
            anonymousReturnedItems = db.ReturnedItems
                .Select(item => new
                {
                    ProductId = item.ProductId,
                    NameProduct = item.NameProduct,
                    DueDate = item.DueDate,
                    DeletionDate = item.DeletionDate,
                    StatusProduct = item.StatusProduct,
                    ShortNameClient = db.Clients.FirstOrDefault(c => c.ClientId == item.ClientId).ShortNameClient,
                    ShortNameEmployee = db.Employees.FirstOrDefault(e => e.EmployeeId == item.EmployeeId).ShortNameEmployee
                })
                .ToList()
                .Cast<object>()
                .ToList();

            lvReturnedProduct.ItemsSource = anonymousReturnedItems;
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ListViewItem listViewItem)
            {
                ReturnedItem currentreturnedItems = null;
                using (PawnshopContext db = new PawnshopContext())
                {
                    var selectedItem = listViewItem.Content as dynamic;

                    decimal productId = Convert.ToDecimal(selectedItem.ProductId);

                    currentreturnedItems = db.ReturnedItems.FirstOrDefault(b => b.ProductId == productId);

                    DetailReturnedItem DetailProductWindow = new(currentreturnedItems);
                    DetailProductWindow.Show();
                    Close();
                }
            }
        }
        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                lvReturnedProduct.ItemsSource = anonymousReturnedItems;
            }
            else
            {
                var filteredProducts = returnedItems
                    .Where(p =>
                        p.NameProduct.ToLower().Contains(searchText) ||
                        p.DueDate.ToString().Contains(searchText) ||
                        p.DeletionDate.ToString().Contains(searchText) ||
                        p.StatusProduct.ToLower().Contains(searchText) ||
                        db.Clients.Any(c => c.ClientId == p.ClientId && c.LName.ToLower().Contains(searchText)) ||
                        db.Employees.Any(emp => emp.EmployeeId == p.EmployeeId && emp.LName.ToLower().Contains(searchText))
                    )
                    .Select(p => new
                    {
                        ProductId = p.ProductId,
                        NameProduct = p.NameProduct,
                        DueDate = p.DueDate,
                        DeletionDate = p.DeletionDate,
                        StatusProduct = p.StatusProduct,
                        ShortNameClient = db.Clients.FirstOrDefault(c => c.ClientId == p.ClientId)?.ShortNameClient,
                        ShortNameEmployee = db.Employees.FirstOrDefault(e => e.EmployeeId == p.EmployeeId)?.ShortNameEmployee
                    })
                    .ToList();

                lvReturnedProduct.ItemsSource = filteredProducts;
            }
        }

        private void back_OnClick(object sender, RoutedEventArgs e)
        {
            Products ProductsWindow = new();
            ProductsWindow.Show();
            Close();
        }
    }
}
