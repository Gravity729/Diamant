using Diamant.Models;
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
    public partial class Employees : Window
    {
        PawnshopContext db = new PawnshopContext();
        List<Employee> employees;
        public ObservableCollection<Employee> FilteredEmployeesData { get; set; } = new ObservableCollection<Employee>();
        public Employees()
        {
            InitializeComponent();
            employees = db.Employees.ToList();
            FilteredEmployeesData = new ObservableCollection<Employee>(employees);

            employees = db.Employees.ToList();
            lvEmployee.ItemsSource = FilteredEmployeesData;
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ListViewItem listViewItem)
            {
                Employee currentEmployees = null;
                using (PawnshopContext db = new PawnshopContext())
                {
                    var selectedItem = listViewItem.Content as dynamic;

                    decimal employees = Convert.ToDecimal(selectedItem.EmployeeId);

                    currentEmployees = db.Employees.FirstOrDefault(b => b.EmployeeId == employees);

                    DetailEmployee DetailEmployeeWindow = new(currentEmployees);
                    DetailEmployeeWindow.Show();
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
            Products ProductsWindow = new();
            ProductsWindow.Show();
            Close();
        }

        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void addEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            NewEmployee wNewEmployee = new();
            wNewEmployee.Show();
            Close();
        }

        private void editEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            if (lvEmployee.SelectedItem != null)
            {
                Employee selectedEmployee = lvEmployee.SelectedItem as Employee;
                EditEmployee wEditEmployee = new EditEmployee(selectedEmployee);
                wEditEmployee.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для редактирования");
            }
        }

        private void deleteEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            if (lvEmployee.SelectedItem != null)
            {
                Employee deleteEmployee = (Employee)lvEmployee.SelectedItem;
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {deleteEmployee.FullNameEmployee}?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    db.Entry(deleteEmployee).Reload();
                    db.Employees.Remove(deleteEmployee);
                    db.SaveChanges();
                    employees = db.Employees.ToList();
                    lvEmployee.ItemsSource = employees;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления");
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredEmployeesData.Clear();
                foreach (var item in employees)
                {
                    FilteredEmployeesData.Add(item);
                }
            }
            else
            {
                FilteredEmployeesData.Clear();
                foreach (var item in employees)
                {
                    if (item.LName.ToLower().Contains(searchText) ||
                        item.FName.ToLower().Contains(searchText) ||
                        item.PName.ToLower().Contains(searchText) ||
                        item.Phone.ToLower().Contains(searchText) ||
                        item.BDate.ToString().Contains(searchText))
                    {
                        FilteredEmployeesData.Add(item);
                    }
                }
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Authorization wAuthorization = new();
            wAuthorization.Show();
            Close();
        }
    }
}
