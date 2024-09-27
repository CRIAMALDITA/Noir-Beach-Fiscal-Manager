using RestaurantData.TablesDataClasses;
using SalesRestaurantSystem.WindowsHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers.ListviewHandlers;
using SalesRestaurantSystem.WindowsHandlers.Listview_Handlers;
using System.Data;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public class SalesMakerWindowHandler : TransactionsWindowHandler<ProductData>
    {
        private SearchClientsHandler _searchClientsHandler;

        public SalesMakerWindowHandler(Window win) : base(win)
        {
            _searchClientsHandler = new SearchClientsHandler();
        }
        public override void SetCustomerInformationFields(TextBox idField, Button button, TextBox nameField)
        {
            base.SetCustomerInformationFields(idField, button, nameField);
            _searchClientsHandler.SetSearchField(idField);
            _searchClientsHandler.SetSearchButton(button);
            _searchClientsHandler.SetNameField(nameField);
        }

        public override void SetTransactionInformationFields(TextBox dateField, ComboBox type)
        {
            base.SetTransactionInformationFields(dateField, type);
            type.Items.Clear();
            type.ItemsSource = new List<string>()
            {
                "A",
                "B",
                "C",
                "D"
            };
        }

        public override void SearchCustomer()
        {
           _searchClientsHandler.Search(_idField.Text);
        }

        public override void SetCatalogInformationFields(ListView elementsList, TextBox searchField, Button searchBtn)
        {
            base.SetCatalogInformationFields(elementsList, searchField, searchBtn);
            ProductsListViewHandler productList = new ProductsListViewHandler(_window);
            productList.SetListViewer(elementsList, null);
            productList.SetListData(new List<ProductData>());
            productList.SetItemButtonAction((data, index) =>
            {
                DataRow row = data.Row;
                ProductData item = DataManager.Instance.Product.GetByIdAsync(Convert.ToInt32(row["IdProduct"])).Result;
                ProductData[] values = { item };
                var product = _catalogList.Items.First(x => x.IdProduct == item.IdProduct);
                if (product.Stock <= 0) return;
                product.Stock--;
                AddToCart(values);
               _catalogList.RefreshListView();
            });
            productList.SetListData(DataManager.Instance.Product.GetAllAsync().Result);
            _catalogList = productList;
        }

        public override void SetTransactionResumeFields(ListView cartList, TextBox subTotal, TextBox off, TextBox total, TextBox paysWith, TextBox change, Button makeSale)
        {
            base.SetTransactionResumeFields(cartList, subTotal, off, total, paysWith, change, makeSale);

            ProductsCartPanelHandler productsCart = new ProductsCartPanelHandler(_window);
            productsCart.SetListView(cartList);
            productsCart.SetFields(_subTotalField, _totalField, _paysWitchField, _changeField, _offField);
            productsCart.CartListViewHandler.SetItemButtonAction((data, index) =>
            {
                DataRow row = data.Row;
                ProductData item = DataManager.Instance.Product.GetByNameAsync(row["Name"].ToString()).Result;
                ProductData[] values = { item };
                RemoveFromCart(values);
                var product = _catalogList.Items.First(x => x.IdProduct == item.IdProduct);
                int productIndex = _catalogList.Items.IndexOf(product);
                product.Stock++;
                _catalogList.RefreshListView();
            });
            _cartList = productsCart;
        }

        public override void SearchInCatalog(string filter)
        {
            _catalogList.Search("Name", filter);
        }

        public override void MakeTransaction()
        {
            throw new NotImplementedException();
        }

    }
}
