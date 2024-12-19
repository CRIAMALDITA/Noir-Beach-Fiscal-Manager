using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System.Diagnostics;



namespace SalesRestaurantSystem.WindowsHandlers
{
    public class ClientsDataHandlerWindow : DataHandlerWindow<ClientData>
    {
        private TextBox _idField;
        private TextBox _nameField;
        private TextBox _emailField;
        private TextBox _telephoneField;
        private ComboBox _createAccount;
        private TextBox _clientBalance;
        private ComboBox _clientStatus;
        public bool categoriesAreEmpty = false;

        public ClientsDataHandlerWindow(Window currentWindow) : base(currentWindow)
        {
            MainListViewPanel = new ClientsListViewHandler(currentWindow);
            RemovedListViewPanel = new ClientsListViewHandler(currentWindow);
            currentListOpen = MainListViewPanel;

        }
        public async override Task<string> GetPKByName(Type dataType, string name)
        {
            if (dataType == typeof(CategoryController))
            {
                UserData user = await DataManager.Instance.User.GetByNameAsync(name).ConfigureAwait(false);
                return user.IdUser.ToString(); ;
            }
            return "";
        }

        public override void SetCategories(ComboBox box)
        {
            _categorySearch = box;
            _categorySearch.ItemsSource = null;
            _categorySearch.Items.Clear();
            List<string> categories = new List<string>()
            {
                "ID","Full Name","Email", "Telephone", "Account", "Balance", "Creation Date"
            };
            _categorySearch.ItemsSource = categories;
            _currentCategorySelected = "ID";
            _categorySearch.SelectedIndex = 0;  
            _categorySearch.SelectionChanged += (o, e) => InsertCategory();
        }


        public override void InsertCategory()
        {
            var selected = _categorySearch.SelectedItem.ToString();
            switch (selected)
            {
                case "ID": selected = "ID"; break;
                case "Full Name": selected = "Full Name"; break;
                case "Email": selected = "Email"; break;
                case "Telephone": selected = "Telephone"; break;
                case "Account": selected = "Account"; break;
                case "Balance": selected = "Balance"; break;
                case "Creation Date": selected = "CreationDate"; break;
            }
            _currentCategorySelected = selected;
            var textSearch = _searchField.Text;
            Search(_currentCategorySelected, textSearch);
        }

        public override void SetFields(Control[] fields)
        {
            _fields = fields.ToList();
            _idField = _fields.First(x => x.Tag.ToString() == "Document") as TextBox;
            _nameField = _fields.First(x => x.Tag.ToString() == "FullName") as TextBox;
            _emailField = _fields.First(x => x.Tag.ToString() == "Email") as TextBox;
            _telephoneField = _fields.First(x => x.Tag.ToString() == "Telephone") as TextBox;
            _createAccount = _fields.First(x => x.Tag.ToString() == "Account") as ComboBox;
            _clientStatus = _fields.First(x => x.Tag.ToString() == "ClientState") as ComboBox;
            _clientBalance = _fields.First(x => x.Tag.ToString() == "Balance") as TextBox;

            _createAccount.ItemsSource = null;
            _createAccount.Items.Clear();
            _createAccount.ItemsSource = new List<string>() { "Yes", "No" };
            _createAccount.SelectedIndex = 0;

            _clientStatus.ItemsSource = null;
            _clientStatus.Items.Clear();
            _clientStatus.ItemsSource = new List<string>() { "Active", "Inactive" };
            _clientStatus.SelectedIndex = 0;

            BuildCategoryCombBox();
        }


        public void BuildCategoryCombBox()
        {
            //User Role ComboBox Building
            _categorySearch.ItemsSource = null;
            _categorySearch.Items.Clear();
            List<string> categoryNames = new List<string>();
            var data = DataManager.Instance.Category.GetAllAsync().Result;
            if (data.Count <= 0)
            {
                categoriesAreEmpty = true;
                return;
            }
            data.ForEach(x => categoryNames.Add(x.CategoryName));
            _categorySearch.ItemsSource = categoryNames;
            _categorySearch.SelectedIndex = 0;
            categoriesAreEmpty = false;
        }

        public override bool SetParameters()
        {
            _currentItemGenerated = new ClientData();

            string id = string.Empty;
            string fullName = string.Empty;
            string email = string.Empty;
            string telephone = string.Empty;
            bool account = false;
            string balance = string.Empty;
            DateTime creationDate = DateTime.Now;


            id = _idField.Text;
            fullName = _nameField.Text;
            email = _emailField.Text;
            telephone = _telephoneField.Text;
            balance = _clientBalance.Text;

            string checkResult = FieldsCheck(id, fullName, email, telephone, balance);
            if (checkResult != string.Empty)
            {
                MessageBox.ShowEmergentMessage(checkResult);
                return false;
            }
            account = _createAccount.SelectedItem.ToString() == "Active" ? true : false;


            _currentItemGenerated = new ClientData()
            {
                Document = id,
                FullName = fullName,
                Email = email,
                Telephone = telephone,
                Account = account,
                AccountBalance = decimal.Parse(balance),
                CreationDate = creationDate,
            };
            return true;
        }
        public override void ShowUI()
        {
            base.ShowUI();
            if (categoriesAreEmpty) BuildCategoryCombBox();
            List<ClientData> clients = new List<ClientData>();
            Task.Run(async () =>
            {
                clients = await DataManager.Instance.Client.GetAllAsync().ConfigureAwait(false);
            }).Wait();
            var clientsInactive = clients.Where(x => !x.ClientState).ToList();
            var clientsActive = clients.Where(x => x.ClientState).ToList();
            MainListViewPanel.SetListData(clientsActive);
            RemovedListViewPanel.SetListData(clientsInactive);
        }



        private string FieldsCheck(string id, string fullName, string email, string telephone, string clientBalance)
        {
            string idResult = IntCheck(id, "Id");
            if (idResult != string.Empty) return idResult;
            
            string fullNameResult = NameCheck(fullName);
            if (fullNameResult != string.Empty) return fullNameResult;

            string emailResult = EmailCheck(email);
            if (emailResult != string.Empty) return emailResult;

            string phoneResult = PhoneNumberCheck(telephone);
            if (phoneResult != string.Empty) return phoneResult;

            string balanceResult = PriceCheck(clientBalance, "Client Balance");
            if (balanceResult != string.Empty) return balanceResult;
            return string.Empty;
        }
        private string NameCheck(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "E_Name cannot be empty.";

            if (!name.All(c => char.IsLetter(c) || char.IsSeparator(c))) return "E_Name must contain only letters.";

            return string.Empty;
        }
        private string IDCheck(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return "E_ID cannot be empty.";

            if (!id.All(char.IsDigit)) return "E_ID must contain only numbers.";

            return string.Empty;
        }
        private string EmailCheck(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return "E_Email cannot be empty.";

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email) return "E_Email is not a valid email format.";
            }
            catch
            {
                return "E_Email is not a valid email format.";
            }

            return string.Empty;
        }
        private string PhoneNumberCheck(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return "E_Phone number cannot be empty.";

            if (!phoneNumber.All(char.IsDigit))
                return "E_Phone number must contain only numbers.";

            if (phoneNumber.Length < 8 || phoneNumber.Length > 15)
                return "E_Phone number must be between 8 and 15 digits.";

            return string.Empty;
        }
        private string PriceCheck(string Price, string field)
        {
            if (string.IsNullOrWhiteSpace(Price))
                return $"E_{field} cannot be empty.";

            if (!decimal.TryParse(Price, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out _))
                return $"E_{field} must be a valid decimal number.";

            return string.Empty;
        }
        private string IntCheck(string id, string field)
        {
            if (string.IsNullOrWhiteSpace(id))
                return $"E_{field} cannot be empty.";

            if (!int.TryParse(id, out _))
                return $"E_{field} must be a valid number.";

            return string.Empty;
        }
        public override void Search(string category, string filter)
        {
            currentListOpen.Search(category, filter);
        }
    }
}
