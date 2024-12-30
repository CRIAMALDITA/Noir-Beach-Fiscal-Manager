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
    public class PurchaseDataHandlerWindow : HistoryHandlerWindow<PurchaseData>
    {
        private ComboBox _categoryBox;

        public bool categoriesAreEmpty = false;

        public PurchaseDataHandlerWindow (Window currentWindow) : base(currentWindow)
        {
            MainListViewPanel = new PurchaseListViewHandler(currentWindow);
            RemovedListViewPanel = new PurchaseListViewHandler(currentWindow);
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
                "DB ID","User","Supplier", "ID Num","Total", "Creation Date"
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
                case "DB ID": selected = "IdPurchase"; break;
                case "User": selected = "UserName"; break;
                case "Supplier": selected = "Supplier"; break;
                case "ID Num": selected = "IdNum"; break;
                case "Total": selected = "Total"; break;
                case "Creation Date": selected = "CreationDate"; break;
            }
            _currentCategorySelected = selected;
            var textSearch = _searchField.Text;
            Search(_currentCategorySelected, textSearch);
        }

        public override void SetFields(Control[] fields)
        {
            _fields = fields.ToList();
            _categoryBox = _fields.First(x => x.Tag.ToString() == "Category") as ComboBox;
        }


        public void BuildCategoryCombBox()
        {
            _categoryBox.ItemsSource = null;
            _categoryBox.Items.Clear();
            List<string> categoryNames = new List<string>();
            var data = DataManager.Instance.Category.GetAllAsync().Result;
            if (data.Count <= 0)
            {
                categoriesAreEmpty = true;
                return;
            }
            data.ForEach(x => categoryNames.Add(x.CategoryName));
            _categoryBox.ItemsSource = categoryNames;
            _categoryBox.SelectedIndex = 0;
            categoriesAreEmpty = false;
        }

        public override bool SetParameters()
        {
            return true;
        }
        public override void ShowUI()
        {
            base.ShowUI();

            if (DataManager.Instance.Category.GetAllAsync().Result.Count <= 0)
            {
                var result = MessageBox.ShowEmergentMessage("E_At the moment there is no category created, Purchases cannot be created until there is at least one category.");
                if (result == MessageBoxResult.OK)
                {
                    GoBack();
                    return;
                }
            }
            if (categoriesAreEmpty) BuildCategoryCombBox();
            List<PurchaseData> Purchases = new List<PurchaseData>();
            Task.Run(async () =>
            {
                Purchases = await DataManager.Instance.Purchase.GetAllAsync().ConfigureAwait(false);
            }).Wait();
            var PurchasesInactive = Purchases.Where(x => !x.PurchaseState).ToList();
            var PurchasesActive = Purchases.Where(x => x.PurchaseState).ToList();
            MainListViewPanel.SetListData(PurchasesActive);
            RemovedListViewPanel.SetListData(PurchasesInactive);
        }

        public override void Search(string category, string filter)
        {
            currentListOpen.Search(category, filter);
        }
    }
}
