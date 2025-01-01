using RestaurantDataManager;
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

namespace SalesRestaurantSystem
{
    public partial class ConfirmPasswordMessageBox : Window
    {
        public ConfirmPasswordMessageBox()
        {
            InitializeComponent();
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            string Password = passwordBox.Password;
            DialogResult = DataManager.Instance.User.CurrentUserLogged.UserPassword == Password;
            if(DialogResult == true)
            {
                Close();
            }
            else
            {
                MessageBox.ShowEmergentMessage("E_Incorrect Password.");
                Close();
            }
        }
        private void Confirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return && e.Key != Key.Enter)
                return;
            e.Handled = true;
            OnOkButtonClick(sender, null);
        }
    }
}
