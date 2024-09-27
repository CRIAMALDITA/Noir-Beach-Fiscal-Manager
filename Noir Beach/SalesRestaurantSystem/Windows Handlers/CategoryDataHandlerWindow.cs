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
    public class CategoryDataHandlerWindow : DataHandlerWindow<CategoryData>
    {
        //controlPanelElements
        private TextBox _categoryName;
        private ComboBox _categoryState;

        public CategoryDataHandlerWindow(Window currentWindow) : base(currentWindow)
        {
            MainListViewPanel = new CategoryListViewHandler(currentWindow);
            RemovedListViewPanel = new CategoryListViewHandler(currentWindow);
            currentListOpen = MainListViewPanel;
        }
        public async override Task<string> GetPKByName(Type dataType, string name)
        {
            return "";
        }

        public override void SetCategories(ComboBox box)
        {
            _categorySearch = box;
            _categorySearch.ItemsSource = null;
            _categorySearch.Items.Clear();
            List<string> categories = new List<string>()
            {
                "ID","Category", "State", "Creation Date"
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
                case "ID": selected = "IdCategory"; break;
                case "Category": selected = "CategoryName"; break;
                case "State": selected = "Categorystate"; break;
                case "Creation Date": selected = "CreationDate"; break;
            }
            _currentCategorySelected = selected;
            var textSearch = _searchField.Text;
            Search(_currentCategorySelected, textSearch);
        }

        public override void SetFields(Control[] fields)
        {
            _fields = fields.ToList();
            _categoryName = _fields.First(x => x.Tag.ToString() == "CategoryName") as TextBox;
            _categoryState = _fields.First(x => x.Tag.ToString() == "CategoryState") as ComboBox;

            //Category State ComboBox building
            _categoryState.ItemsSource = null;
            _categoryState.Items.Clear();
            _categoryState.ItemsSource = new List<string>() { "Active", "Inactive" };
            _categoryState.SelectedIndex = 0;


        }
        public override bool SetParameters()
        {
            _currentItemGenerated = new CategoryData();

            string categoryName = string.Empty;
            bool categoryState = false;
            DateTime creationDate = DateTime.Now;


            categoryName = _categoryName.Text;

            string checkResult = FieldsCheck(categoryName);
            if (checkResult != string.Empty)
            {
                MessageBox.ShowEmergentMessage(checkResult);
                return false;
            }
            categoryState = _categoryState.SelectedItem.ToString() == "Active" ? true : false;


            _currentItemGenerated = new CategoryData()
            {
                CategoryName = categoryName,
                CategoryState = categoryState,
                CreationDate = creationDate,
            };
            return true;
        }
        public override void ShowUI()
        {
            base.ShowUI();
            List<CategoryData> category = new List<CategoryData>();
            Task.Run(async () =>
            {
                category = await DataManager.Instance.Category.GetAllAsync().ConfigureAwait(false);
            }).Wait();
            var categoryInactive = category.Where(x => !x.CategoryState).ToList();
            var categoryActive = category.Where(x => x.CategoryState).ToList();
            MainListViewPanel.SetListData(categoryActive);
            RemovedListViewPanel.SetListData(categoryInactive);
        }



        private string FieldsCheck(string Name)
        {
            string nameResult = NameCheck(Name, "Name");
            if (nameResult != string.Empty) return nameResult;
            return string.Empty;
        }
        private string NameCheck(string name, string field)
        {
            if (string.IsNullOrWhiteSpace(name)) return "E_Name cannot be empty.";

            if (!name.All(char.IsLetter)) return "E_Name must contain only letters.";

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
