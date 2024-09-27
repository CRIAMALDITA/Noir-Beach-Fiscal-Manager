using RestaurantData;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SalesRestaurantSystem
{
    public class UserDataHandlerWindow : DataHandlerWindow<UserData>
    {
        //controlPanelElements
        private TextBox _idField;
        private TextBox _fullName;
        private TextBox _email;
        private PasswordBox _password;
        private PasswordBox _confirmPassword;
        private ComboBox _userState;
        private ComboBox _userRole;
        private ComboBox _userPermission;

        public UserDataHandlerWindow(Window currentWindow) : base(currentWindow)
        {
            MainListViewPanel = new UsersListViewHandler(currentWindow);
            RemovedListViewPanel = new UsersListViewHandler(currentWindow);
            currentListOpen = MainListViewPanel;
        }
        public async override Task<string> GetPKByName(Type dataType, string name)
        {
            if (dataType == typeof(UserData)) 
            {
                UserData user = await DataManager.Instance.User.GetByNameAsync(name).ConfigureAwait(false);
                return user.IdUser.ToString(); ;
            }
            if (dataType == typeof(RoleData)) 
            {
                RoleData role = await DataManager.Instance.Role.GetByNameAsync(name).ConfigureAwait(false);
                return role.IdRol.ToString(); 
            }
            if (dataType == typeof(PermissionData)) 
            {
                PermissionData permission = await DataManager.Instance.Permission.GetByNameAsync(name).ConfigureAwait(false);
                return permission.IdPermission.ToString(); 
            }
            return "";
        }

        public override void RemoveData(UserData[] items)
        {
            if(items.Length > 0)
            {
                var itemsLeft = MainListViewPanel.Items.Where(x => !items.Any(y => x.IdUser == y.IdUser && x.IdPermission == y.IdPermission)).ToArray();
                if (itemsLeft.Length == 0 || !itemsLeft.Any(x => x.IdPermission == DataManager.Instance.Permission.GetByNameAsync("ADMINISTRATOR").Result.IdPermission))
                {
                    var result = MessageBox.ShowEmergentMessage("E_Cannot remove all admin users, must left ONE admin user.");
                    if (result == MessageBoxResult.OK) return;
                }
            }
            base.RemoveData(items);
        }

        public override void SetCategories(ComboBox box)
        {
            _categorySearch = box;
            _categorySearch.ItemsSource = null;
            _categorySearch.Items.Clear();
            List<string> categories = new List<string>() 
            {
                "ID User","ID","Name", "Email", "Role", "Permission", "Creation Date"
            };
            _categorySearch.ItemsSource = categories;
            _currentCategorySelected = "ID";
            _categorySearch.SelectedIndex = 0;
            _categorySearch.SelectionChanged += (o, e) => InsertCategory();
        }


        public override void InsertCategory()
        {
            var selected =  _categorySearch.SelectedItem.ToString();
            switch (selected)
            {
                case "ID User": selected = "IdUser"; break;
                case "ID": selected = "Document"; break;
                case "Name": selected = "FullName"; break;
                case "Email": selected = "Email"; break;
                case "Role": selected = "IdRol"; break;
                case "Permission": selected = "IdPermission"; break;
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
            _fullName = _fields.First(x => x.Tag.ToString() == "FullName") as TextBox;
            _email = _fields.First(x => x.Tag.ToString() == "Email") as TextBox;
            _password = _fields.First(x => x.Tag.ToString() == "UserPassword") as PasswordBox;
            _confirmPassword = _fields.First(x => x.Tag.ToString() == "ConfirmUserPassword") as PasswordBox;
            _userState = _fields.First(x => x.Tag.ToString() == "UserState") as ComboBox;
            _userRole = _fields.First(x => x.Tag.ToString() == "IdRol") as ComboBox;
            _userPermission = _fields.First(x => x.Tag.ToString() == "IdPermission") as ComboBox;

            //User State ComboBox building
            _userState.ItemsSource = null;
            _userState.Items.Clear();
            _userState.ItemsSource = new List<string>(){ "Active", "Inactive"};
            _userState.SelectedIndex = 0;

            //User Role ComboBox Building
            _userRole.ItemsSource = null;
            _userRole.Items.Clear();
            List<string> rolesNames = new List<string>();
            DataManager.Instance.Role.GetAllAsync().Result.ForEach(x => rolesNames.Add(x.RolName));
            _userRole.ItemsSource = rolesNames;

            //User Role ComboBox Building
            _userPermission.ItemsSource = null;
            _userPermission.Items.Clear();
            List<string> permissionsName = new List<string>();
            DataManager.Instance.Permission.GetAllAsync().Result.ForEach(x => permissionsName.Add(x.PermissionName));
            _userPermission.ItemsSource = rolesNames;

        }
        public override bool SetParameters()
        {
            _currentItemGenerated = new UserData();

            string document = string.Empty;
            string fullName = string.Empty;
            string email =  string.Empty;
            string password = string.Empty;
            string confirmPassword = string.Empty;
            bool userState = false;
            int idRol = 0;
            int idPermission = 0;
            DateTime creationDate = DateTime.Now;


            document = _idField.Text;
            fullName = _fullName.Text;
            email = _email.Text;
            password = _password.Password;

            string checkResult = FieldsCheck(fullName, document, email, password);
            if(checkResult != string.Empty)
            {
                MessageBox.ShowEmergentMessage(checkResult);
                return false;
            }
            confirmPassword = _confirmPassword.Password;
            if(password != confirmPassword)
            {
                MessageBox.ShowEmergentMessage("E_Passwords don't Match.");
                return false;
            }
            var roleName = _userRole.SelectedItem.ToString();
            idRol = DataManager.Instance.Role.GetByNameAsync(roleName).Result.IdRol;
            var permissionName = _userPermission.SelectedItem.ToString();
            idPermission = DataManager.Instance.Permission.GetByNameAsync(permissionName).Result.IdPermission;
            userState = _userState.SelectedItem.ToString() == "Active" ? true : false;


            _currentItemGenerated = new UserData()
            {
                Document = document,
                FullName = fullName,
                Email = email,
                UserPassword = password,
                IdRol = idRol,
                IdPermission = idPermission,
                UserState = userState,
                CreationDate = creationDate
            };
            return true;
        }
        public override void ShowUI()
        {
            base.ShowUI();
            List<UserData> users = new List<UserData>();
            Task.Run(async () =>
            {
                users = await DataManager.Instance.User.GetAllAsync().ConfigureAwait(false);
            }).Wait();
            var usersInactive = users.Where(x => !x.UserState).ToList();
            var usersActive = users.Where(x => x.UserState).ToList();
            MainListViewPanel.SetListData(usersActive);
            RemovedListViewPanel.SetListData(usersInactive);
        } 



        private string FieldsCheck(string name, string id, string email, string password)
        {
            string nameResult = NameCheck(name);
            if(nameResult != string.Empty) return nameResult;
            
            string idResult = IDCheck(id);
            if (idResult != string.Empty) return idResult;
            
            string emailResult = EmailCheck(email);
            if (emailResult != string.Empty) return emailResult;
            
            string passwordResult = PasswordCheck(password);
            if (passwordResult != string.Empty) return passwordResult;

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
            
            if (!id.All(char.IsDigit) )return "E_ID must contain only numbers.";
            
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
        private string PasswordCheck(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return "E_Email cannot be empty.";
            
            if (password.Length <= 8) return "E_Password must be longer than 8 characters.";

            if (!password.Any(char.IsUpper)) return "E_Password must contain at least one uppercase letter.";

            if (!password.Any(char.IsLower)) return "E_Password must contain at least one lowercase letter.";

            if (!password.Any(char.IsDigit)) return "E_Password must contain at least one number.";

            return string.Empty;
        }

        public override void Search(string category, string filter)
        {
            currentListOpen.Search(category, filter);
        }

        public override void GoBack()
        {
            HideUI();
            base.GoBack();
        }
    }
}
