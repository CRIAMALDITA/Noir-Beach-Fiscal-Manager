using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers;
using SalesRestaurantSystem.WindowsHandlers.ListviewHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public abstract class TransactionsWindowHandler<T> : IUIPanel, IUITransactionsHandler<T> where T : class
    {
        //Panel
        protected Window _window;
        protected Grid _grid;
        protected BackButtonController _backButtonController;
        public Action onBackPressed;

        //Transaction Info
        protected TextBox _dateField;
        protected ComboBox _transactionType;

        //Customer Info
        protected TextBox _idField;
        protected Button _searchCustomerButton;
        protected TextBox _nameField;

        //Catalog
        protected ListViewPanelController<T> _catalogList;
        protected TextBox _searchField;
        protected Button _searchElementButton;

        //Transaction Resume
        protected CartPanelHanlder<T> _cartList;
        protected TextBox _subTotalField;
        protected TextBox _offField;
        protected TextBox _totalField;
        protected TextBox _paysWithField;
        protected TextBox _changeField;
        protected Button _makeTransactionButton;

        public TransactionsWindowHandler(Window win)
        {
            _window = win;
        }
        
        //Panel
        public void SetPanel(Grid grid, BackButtonController backButton)
        {
            _grid = grid;
            _backButtonController = backButton;
        }
        public void GoBack()
        {
            HideUI();
            _backButtonController.ShowButton(false);
            onBackPressed?.Invoke();
        }
        public void ShowUI()
        {
            _backButtonController.AddListener(GoBack);
            _backButtonController.ShowButton(true);
            _grid.Visibility = Visibility.Visible;
            _catalogList.RefreshListView();
            _cartList.CartListViewHandler.RefreshListView();
            UpdateCatalog();
        }
        public void HideUI()
        {
            _grid.Visibility = Visibility.Hidden;
        }
        public void OnBackButtonPressed(Action action)
        {
            onBackPressed = action;
        }

        //Transaction Behaviour
        public virtual void SetTransactionInformationFields(TextBox dateField, ComboBox type)
        {
            _dateField = dateField;
            _transactionType = type;
            _dateField.Text = DateTime.Now.ToString();
        }
        public virtual void SetCustomerInformationFields(TextBox idField, Button button, TextBox nameField)
        {
            _idField = idField;
            _searchCustomerButton = button;
            _nameField = nameField;
        }
        public virtual void SetCatalogInformationFields(ListView elementsList, TextBox searchField, Button searchBtn)
        {
            _searchField = searchField;
            _searchElementButton = searchBtn;
            _searchElementButton.Click += (o, e) => SearchInCatalog(_searchField.Text);
        }
        public abstract void UpdateCatalog();
        public virtual void SetTransactionResumeFields(ListView cartList, TextBox subTotal, TextBox off, TextBox total, TextBox paysWith, TextBox change, Button makeSale)
        {
            _subTotalField = subTotal;
            _offField = off;
            _totalField = total;
            _paysWithField = paysWith;
            _changeField = change;
            _makeTransactionButton = makeSale;
            _makeTransactionButton.Click += (o, e) => MakeTransaction();
        }

        public virtual TransactionInformation GetTransactionInformationFields()
        {
            return new TransactionInformation()
            {
                Date = _dateField.Text,
                Type = _transactionType.SelectedValue.ToString(),
            };
        }
        public virtual CustomerInformation GetCustomerInformationField()
        {
            return new CustomerInformation()
            {
                Id = _idField.Text,
                Name = _nameField.Text,
            };
        }
        public virtual CartInformation GetCartInformationFields()
        {
            return new CartInformation()
            {
                values = _cartList.CartListViewHandler.Items.ToList()
            };
        }
        public virtual TransactionResume GetTransactionResumeFields()
        {
            return new TransactionResume()
            {
                SubTotal = _subTotalField.Text,
                Off = _offField.Text,
                Total = _totalField.Text,
                PaysWith = _paysWithField.Text,
                Change = _changeField.Text
            };
        }
        public virtual SellMakerData GetSaleData()
        {
            return new SellMakerData()
            {
                Transaction = GetTransactionInformationFields(),
                Customer = GetCustomerInformationField(),
                Cart = GetCartInformationFields(),
                Resume = GetTransactionResumeFields(),
            };
        }

        public abstract void SearchInCatalog(string filter);
        public abstract void SearchCustomer();
        public void AddToCart(T[] items)
        {
            _cartList.AddElements(items);
        }
        public bool RemoveFromCart(T[] items)
        {
            return _cartList.RemoveElements(items);
        }
        public abstract void MakeTransaction();

        public void ClearTransaction()
        {
            _cartList.CartListViewHandler.Items.Clear();
            _changeField.Text = string.Empty;
            _dateField.Text = DateTime.Today.ToString();
            _nameField.Text = string.Empty;
            _paysWithField.Text = string.Empty;
            _idField.Text = string.Empty;
            _totalField.Text = string.Empty;
            _searchField.Text = string.Empty;
            _idField.Text = string.Empty;
            _subTotalField.Text = string.Empty;
            _offField.Text = string.Empty;
            _transactionType.SelectedIndex = 0;        
        }
        public struct TransactionInformation 
        {
            public string Date;
            public string Type;
        }
        public struct CustomerInformation 
        {
            public string Id;
            public string Name;
        }
        public struct CartInformation
        {
            public List<CartElementData> values;
        }
        public struct TransactionResume 
        {
            public string SubTotal;
            public string Off;
            public string PaysWith;
            public string Change;
            public string Total;
        }
        public struct SellMakerData()
        {
            public TransactionInformation Transaction;
            public CustomerInformation Customer;
            public CartInformation Cart;
            public TransactionResume Resume;
        }
    }
}
