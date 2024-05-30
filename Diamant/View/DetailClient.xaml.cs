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
    public partial class DetailClient : Window
    {
        public DetailClient(Client currentClients)
        {
            InitializeComponent();
            DataContext = currentClients;
            using (var db = new PawnshopContext())
            {
                lname.Content = currentClients.LName;
                fname.Content = currentClients.FName;
                pname.Content = string.IsNullOrEmpty(currentClients.PName) ? "---" : currentClients.PName;
                bdate.Content = currentClients.BDate.ToString();
                phone.Content = currentClients.Phone.ToString();
                spassport.Content = currentClients.SPassport.ToString();
                npassport.Content = currentClients.NPassport.ToString();

            }
        }

        private void back_OnClick(object sender, RoutedEventArgs e)
        {
            Clients ClientsWindow = new();
            ClientsWindow.Show();
            Close();
        }
    }
}
