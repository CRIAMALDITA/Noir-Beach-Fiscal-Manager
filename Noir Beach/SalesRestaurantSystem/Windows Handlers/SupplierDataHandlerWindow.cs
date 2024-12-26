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
    public class SuppliersDataHandlerWindow : DataHandlerWindow<SupplierData>
    {
        private TextBox _idField;
        private TextBox _nameField;
        private TextBox _emailField;
        private TextBox _telephoneField;
        private ComboBox _supplierstatus;
        public bool categoriesAreEmpty = false;

        public SuppliersDataHandlerWindow(Window currentWindow) : base(currentWindow)
        {
            MainListViewPanel = new SuppliersListViewHandler(currentWindow);
            RemovedListViewPanel = new SuppliersListViewHandler(currentWindow);
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
                "ID","Fiscal Name","Email", "Telephone", "Creation Date"
            };
            _categorySearch.ItemsSource = categories;
            _currentCategorySelected = "ID";
            _categorySearch.SelectedIndex = 0;  
            _categorySearch.SelectionChanged += (o, e) => InsertCategory();
            categoriesAreEmpty = false;
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
            _nameField = _fields.First(x => x.Tag.ToString() == "FiscalName") as TextBox;
            _emailField = _fields.First(x => x.Tag.ToString() == "Email") as TextBox;
            _telephoneField = _fields.First(x => x.Tag.ToString() == "Telephone") as TextBox;
            _supplierstatus = _fields.First(x => x.Tag.ToString() == "SupplierState") as ComboBox;

            _supplierstatus.ItemsSource = null;
            _supplierstatus.Items.Clear();
            _supplierstatus.ItemsSource = new List<string>() { "Active", "Inactive" };
            _supplierstatus.SelectedIndex = 0;

            if(categoriesAreEmpty) BuildCategoryCombBox();
        }


        public void BuildCategoryCombBox()
        {
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
            _currentItemGenerated = new SupplierData();

            string id = string.Empty;
            string fullName = string.Empty;
            string email = string.Empty;
            string telephone = string.Empty;
            bool status = false;
            DateTime creationDate = DateTime.Now;


            id = _idField.Text;
            fullName = _nameField.Text;
            email = _emailField.Text;
            telephone = _telephoneField.Text;

            status = _supplierstatus.SelectedItem.ToString() == "Active" ? true : false;

            string checkResult = FieldsCheck(id, fullName, email, telephone);
            if (checkResult != string.Empty)
            {
                MessageBox.ShowEmergentMessage(checkResult);
                return false;
            }


            _currentItemGenerated = new SupplierData()
            {
                Document = id,
                FiscalName = fullName,
                Email = email,
                Telephone = telephone,
                SupplierState = status,
                CreationDate = creationDate,
            };
            return true;
        }
        public override void ShowUI()
        {
            base.ShowUI();
            if (categoriesAreEmpty) BuildCategoryCombBox();
            List<SupplierData> Suppliers = new List<SupplierData>();
            Suppliers = DataManager.Instance.Supplier.GetAllAsync().Result;
            var SuppliersInactive = Suppliers.Where(x => !x.SupplierState).ToList();
            var SuppliersActive = Suppliers.Where(x => x.SupplierState).ToList();
            MainListViewPanel.SetListData(SuppliersActive);
            RemovedListViewPanel.SetListData(SuppliersInactive);
        }



        private string FieldsCheck(string id, string fullName, string email, string telephone)
        {
            string idResult = IntCheck(id, "Id");
            if (idResult != string.Empty) return idResult;
            
            string fullNameResult = NameCheck(fullName);
            if (fullNameResult != string.Empty) return fullNameResult;

            string emailResult = EmailCheck(email);
            if (emailResult != string.Empty) return emailResult;

            string phoneResult = PhoneNumberCheck(telephone);
            if (phoneResult != string.Empty) return phoneResult;

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
