using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        protected TextBox _paysWitchField;
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
            onBackPressed?.Invoke();
        }
        public void ShowUI()
        {
            _backButtonController.AddListener(GoBack);
            _backButtonController.ShowButton(true);
            _grid.Visibility = Visibility.Visible;
            _catalogList.RefreshListView();
            _cartList.CartListViewHandler.RefreshListView();
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
        public virtual void SetTransactionResumeFields(ListView cartList, TextBox subTotal, TextBox off, TextBox total, TextBox paysWith, TextBox change, Button makeSale)
        {
            _subTotalField = subTotal;
            _offField = off;
            _totalField = total;
            _paysWitchField = paysWith;
            _changeField = change;
            _makeTransactionButton = makeSale;
            _makeTransactionButton.Click += (o, e) => MakeTransaction();
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
    }
}
