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
    public partial class Clients : Window
    {
        PawnshopContext db = new PawnshopContext();
        List<Client> clients;
        public ObservableCollection<Client> FilteredClientsData { get; set; } = new ObservableCollection<Client>();
        public Clients()
        {
            InitializeComponent();
            clients = db.Clients.ToList();
            FilteredClientsData = new ObservableCollection<Client>(clients);

            lvClient.ItemsSource = FilteredClientsData;

            if (App.UserRole == 1)
            {
                employees.Visibility = Visibility.Collapsed;
            }
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ListViewItem listViewItem)
            {
                Client currentClients = null;
                using (PawnshopContext db = new PawnshopContext())
                {
                    var selectedItem = listViewItem.Content as dynamic;

                    decimal clients = Convert.ToDecimal(selectedItem.ClientId);

                    currentClients = db.Clients.FirstOrDefault(b => b.ClientId == clients);

                    DetailClient wDetailClient = new(currentClients);
                    wDetailClient.Show();
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

        }

        private void Product_OnClick(object sender, RoutedEventArgs e)
        {
            Products wProducts = new();
            wProducts.Show();
            Close();
        }

        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {
            Employees wEmployees = new();
            wEmployees.Show();
            Close();
        }

        private void addClient_OnClick(object sender, RoutedEventArgs e)
        {
            NewClient wNewClient = new();
            wNewClient.Show();
            Close();
        }

        private void editClient_OnClick(object sender, RoutedEventArgs e)
        {
            if (lvClient.SelectedItem != null)
            {
                Client selectedClient = lvClient.SelectedItem as Client;
                //Hide();
                EditClient wEditClient = new EditClient(selectedClient);
                wEditClient.Show();
                //UpdateClientsList();
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования");
            }
        }

        private void deleteClient_OnClick(object sender, RoutedEventArgs e)
        {
            if (lvClient.SelectedItem != null)
            {
                Client deleteClient = (Client)lvClient.SelectedItem;
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {deleteClient.FullNameClient}?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    db.Entry(deleteClient).Reload();
                    db.Clients.Remove(deleteClient);
                    db.SaveChanges();
                    clients = db.Clients.ToList();
                    lvClient.ItemsSource = clients;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления");
            }
        }



        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredClientsData.Clear();
                foreach (var item in clients)
                {
                    FilteredClientsData.Add(item);
                }
            }
            else
            {
                FilteredClientsData.Clear();
                foreach (var item in clients)
                {
                    if (item.LName.ToLower().Contains(searchText) ||
                        item.FName.ToLower().Contains(searchText) ||
                        item.PName.ToLower().Contains(searchText) ||
                        item.Phone.ToLower().Contains(searchText) ||
                        item.BDate.ToString().Contains(searchText))
                    {
                        FilteredClientsData.Add(item);
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
