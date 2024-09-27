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
    public class ProductDataHandlerWindow : DataHandlerWindow<ProductData>
    {
        //controlPanelElements
        private TextBox _codeField;
        private TextBox _nameField;
        private TextBox _descriptionField;
        private ComboBox _categoryBox;
        private TextBox _stockField;
        private TextBox _purchaseField;
        private TextBox _sellField;
        private ComboBox _productState;

        public bool categoriesAreEmpty = false;

        public ProductDataHandlerWindow (Window currentWindow) : base(currentWindow)
        {
            MainListViewPanel = new ProductsListViewHandler(currentWindow);
            RemovedListViewPanel = new ProductsListViewHandler(currentWindow);
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
                "ID","Code","Product", "State", "Creation Date"
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
                case "ID": selected = "IdProduct"; break;
                case "Code": selected = "Code"; break;
                case "Product": selected = "ProductName"; break;
                case "State": selected = "ProductState"; break;
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
            _codeField = _fields.First(x => x.Tag.ToString() == "Code") as TextBox;
            _nameField = _fields.First(x => x.Tag.ToString() == "ProductName") as TextBox;
            _descriptionField = _fields.First(x => x.Tag.ToString() == "ProductDescription") as TextBox;
            _stockField = _fields.First(x => x.Tag.ToString() == "Stock") as TextBox;
            _purchaseField = _fields.First(x => x.Tag.ToString() == "PurchasePrice") as TextBox;
            _sellField = _fields.First(x => x.Tag.ToString() == "SellPrice") as TextBox;
            _categoryBox = _fields.First(x => x.Tag.ToString() == "Category") as ComboBox;
            _productState = _fields.First(x => x.Tag.ToString() == "ProductState") as ComboBox;

            //User State ComboBox building
            _productState.ItemsSource = null;
            _productState.Items.Clear();
            _productState.ItemsSource = new List<string>() { "Active", "Inactive" };
            _productState.SelectedIndex = 0;

            BuildCategoryCombBox();

        }


        public void BuildCategoryCombBox()
        {
            //User Role ComboBox Building
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
            _currentItemGenerated = new ProductData();

            string code = string.Empty;
            string productName = string.Empty;
            string productDescription = string.Empty;
            int idCategory = 0;
            string purchase = string.Empty;
            string sell = string.Empty;
            string stock = string.Empty;
            bool productState = false;
            DateTime creationDate = DateTime.Now;


            code = _codeField.Text;
            productName = _nameField.Text;
            productDescription = _descriptionField.Text;
            purchase = _purchaseField.Text;
            sell = _sellField.Text;
            stock = _stockField.Text;

            string checkResult = FieldsCheck(code, purchase, sell);
            if (checkResult != string.Empty)
            {
                MessageBox.ShowEmergentMessage(checkResult);
                return false;
            }
            var categoryName = _categoryBox.SelectedItem.ToString();
            idCategory = DataManager.Instance.Category.GetByNameAsync(categoryName).Result.IdCategory;

            productState = _productState.SelectedItem.ToString() == "Active" ? true : false;


            _currentItemGenerated = new ProductData()
            {
                Code = code,
                ProductName = productName,
                ProductDescription = productDescription,
                IdCategory = idCategory,
                Stock = int.Parse(stock),
                SellPrice = decimal.Parse(sell),
                PurchasePrice = decimal.Parse(purchase),
                ProductState = productState,
                CreationDate = creationDate,
            };
            return true;
        }
        public override void ShowUI()
        {
            base.ShowUI();

            if (DataManager.Instance.Category.GetAllAsync().Result.Count <= 0)
            {
                var result = MessageBox.ShowEmergentMessage("E_At the moment there is no category created, products cannot be created until there is at least one category.");
                if (result == MessageBoxResult.OK)
                {
                    GoBack();
                    return;
                }
            }
            if (categoriesAreEmpty) BuildCategoryCombBox();
            List<ProductData> products = new List<ProductData>();
            Task.Run(async () =>
            {
                products = await DataManager.Instance.Product.GetAllAsync().ConfigureAwait(false);
            }).Wait();
            var productsInactive = products.Where(x => !x.ProductState).ToList();
            var productsActive = products.Where(x => x.ProductState).ToList();
            MainListViewPanel.SetListData(productsActive);
            RemovedListViewPanel.SetListData(productsInactive);
        }



        private string FieldsCheck(string code, string purchase, string sell)
        {
            string codeResult = IntCheck(code, "Code");
            if (codeResult != string.Empty) return codeResult;

            string purchaseResult = PriceCheck(purchase, "Purchase");
            if (purchaseResult != string.Empty) return purchaseResult;

            string sellResult = PriceCheck(sell, "Sell");
            if (sellResult != string.Empty) return sellResult;
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
