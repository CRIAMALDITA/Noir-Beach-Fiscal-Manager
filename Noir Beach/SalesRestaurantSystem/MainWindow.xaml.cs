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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Point_of_sale_for_Restaurant;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using SalesRestaurantSystem;

namespace SalesRestaurantSystem
{

    public partial class Login_Window : Window
    {
        private bool Debug = false;
        public Login_Window()
        {
            if (!Debug)
            {
                //Check SQl Server Connection.
                bool _connectionSuccefully = false;
                LoadingWindow _sqlServerConnect = new LoadingWindow("Connecting on Data Base", Task.Run<string>(async () =>
                {
                    string _msg = string.Empty;
                    _msg = await DataManager.Instance.CheckConnection().ConfigureAwait(false);
                    return _msg;
                }), false, _successfully =>
                {
                    if (!_successfully) Application.Current.Shutdown();
                    else _connectionSuccefully = true;
                });


                while (!_connectionSuccefully) { }



                //Check If there's any user.
                bool anyUser = false;

                LoadingWindow _anyUserExist = new LoadingWindow("Checking if any user exist", Task.Run<string>(async () =>
                {
                    bool _result = false;
                    _result = await DataManager.Instance.InitialCheck().ConfigureAwait(false);
                    return _result ? "I_Users founded!" : "W_Users dont found... Admin user default was created.";
                }), false, _successfully =>
                {
                    if (!_successfully)
                    {
                        MessageBoxResult _result = MessageBox.ShowEmergentMessage("E_Database Error, something goes wrong.");
                        if (_result == MessageBoxResult.OK)
                        {
                            Application.Current.Shutdown();
                        }
                    }
                    else anyUser = true;
                });

                while (!anyUser) { }
            }

            InitializeComponent();
            WindowUtilities.CenterWindowOnScreen(this);
        }

        private async void OnClickLogin_BTN(object sender, RoutedEventArgs e)
        {
            string id = IDLoginField.Text;
            string pass = PasswordLoginField.Password;
            UserData finded = await DataManager.Instance.User.LogInUser(id, pass).ConfigureAwait(false);

            if (Debug) LogIn(new UserData()
            {
                FullName = "Norca",
                Document = "69-420-666",
                IdPermission = (await DataManager.Instance.Permission.GetByNameAsync("ADMINISTRATOR").ConfigureAwait(false)).IdPermission

            });
                if (finded != null) LogIn(finded);
                else MessageBox.ShowEmergentMessage("W_Login failed. Please check your credentials.");
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return && e.Key != Key.Enter)
                return;
            e.Handled = true;
            OnClickLogin_BTN(sender, null);
        }

        private void LogIn(UserData userData)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = new SystemWindow(userData);
                window.Show();
                Close();
            });
        }
    }
}
