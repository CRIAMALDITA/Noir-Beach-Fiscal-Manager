using iText.StyledXmlParser.Jsoup.Select;
using MS.WindowsAPICodePack.Internal;
using RestaurantData;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public class PurchaseMakerWindowHandler : TransactionsWindowHandler<ProductData>
    {
        private SearchSupplierHandler _searchSuppliersHandler;

        public PurchaseMakerWindowHandler(Window win) : base(win)
        {
            _searchSuppliersHandler = new SearchSupplierHandler();
        }
        public override void SetCustomerInformationFields(TextBox idField, Button button, TextBox nameField)
        {
            base.SetCustomerInformationFields(idField, button, nameField);
            _searchSuppliersHandler.SetSearchField(idField);
            _searchSuppliersHandler.SetSearchButton(button);
            _searchSuppliersHandler.SetNameField(nameField);
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
            type.SelectedIndex = 0;
        }

        public override void SearchCustomer()
        {
            _searchSuppliersHandler.Search(_idField.Text);
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
                AddToCart(values);
                _catalogList.RefreshListView();
            });
            productList.SetListData(DataManager.Instance.Product.GetAllAsync().Result);
            _catalogList = productList;
        }

        public override void UpdateCatalog()
        {
            _catalogList.SetListData(DataManager.Instance.Product.GetAllAsync().Result);
        }

        public override void SetTransactionResumeFields(ListView cartList, TextBox subTotal, TextBox off, TextBox total, TextBox paysWith, TextBox change, Button makeSale)
        {
            base.SetTransactionResumeFields(cartList, subTotal, off, total, paysWith, change, makeSale);

            ProductsCartPanelHandler productsCart = new ProductsCartPanelHandler(_window);
            productsCart.SetListView(cartList);
            productsCart.SetFields(_subTotalField, _totalField, _paysWithField, _changeField, _offField);
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

        public async override void MakeTransaction() 
        {
            if(!VerifyFields()) return;


            TransactionMakerData Purchasedata = GetSaleData();


            PurchaseData Purchase = GetPurchaseDataFromFields(Purchasedata);
            try 
            {
                await DataManager.Instance.Purchase.AddAsync(Purchase);
            }
            catch(Exception ex)
            {
                MessageBox.ShowEmergentMessage("E_Cannot add Purchase on PurchaseData SQL.");
            }

            var data = await DataManager.Instance.Purchase.GetAllAsync();

            int purchaseId = data.Count == 0 ? 0 : data.OrderByDescending(s => s.IdPurchase).FirstOrDefault().IdPurchase;

            LoadingWindow addingPurchases = new LoadingWindow("Making Sale...",
            Task.Run<bool>(async () =>
            {
                try
                {
                    List<PurchaseDetailsData> list = GetPurchaseDetailsDatasFromFields(Purchasedata, purchaseId);
                    for (int i = 0; i < list.Count; i++)
                    {
                        PurchaseDetailsData item = list[i];
                        await DataManager.Instance.PurchaseDetails.AddAsync(item);
                        ProductData product = await DataManager.Instance.Product.GetByIdAsync(item.IdProduct);
                        product.Stock = product.Stock + item.ProductCount;
                        await DataManager.Instance.Product.UpdateAsync(product);
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    MessageBox.ShowEmergentMessage("E_Cannot add PurchaseDetails on PurchaseDetailsData SQL.");
                    return false;
                }
            }),
            result =>
            {
                if (result)
                {
                    UpdateCatalog();
                    ClearTransaction();
                    MessageBox.ShowEmergentMessage("I_Purchase Completed!");
                }
            });
        }

        private bool VerifyFields()
        {
            TransactionMakerData Purchasedata = GetSaleData();
            if (_searchSuppliersHandler.EntityFinded == false)
            {
                MessageBox.ShowEmergentMessage("E_Supplier is not selected");
                return false;
            }
            if (_transactionType.SelectedIndex == -1)
            {
                MessageBox.ShowEmergentMessage("E_Transaction type is not selected");
                return false;
            }
            if (Purchasedata.Cart.values.Count == 0)
            {
                MessageBox.ShowEmergentMessage("E_Cart is Empty");
                return false;
            }
            decimal total = Convert.ToDecimal(Purchasedata.Resume.Total.Replace("$", "").Trim());

            return true;
        }
        private PurchaseData GetPurchaseDataFromFields(TransactionMakerData Purchasedata)
        {
            decimal total = Convert.ToDecimal(Purchasedata.Resume.Total.Replace("$", "").Trim());
            PurchaseData Purchase = new PurchaseData();

            Purchase.IdUser = DataManager.Instance.User.CurrentUserLogged.IdUser;
            Purchase.Total = total;
            Purchase.IdSupplier = DataManager.Instance.Supplier.GetByNameAsync(Purchasedata.Customer.Name).Result.IdSupplier;
            Purchase.IdentificationNumber = DataManager.Instance.User.CurrentUserLogged.Document;
            Purchase.IdentificationType = Purchasedata.Transaction.Type;
            Purchase.IdentificationNumber = Purchasedata.Customer.Id;
            Purchase.CreationDate = Convert.ToDateTime(Purchasedata.Transaction.Date);
            Purchase.PurchaseState = true;
            return Purchase;
        }
        private List<PurchaseDetailsData> GetPurchaseDetailsDatasFromFields(TransactionMakerData Purchasedata, int id)
        {
            List<PurchaseDetailsData> list = new List<PurchaseDetailsData>();
            for (int i = 0; i < Purchasedata.Cart.values.Count; i++)
            {
                PurchaseDetailsData item = new PurchaseDetailsData();
                var result = DataManager.Instance.Purchase.GetAllAsync().Result;
                item.IdPurchase = id;
                item.SubTotal = Purchasedata.Cart.values[i].Subtotal;
                item.ProductCount = Purchasedata.Cart.values[i].Amount;
                item.PurchasePrice = Purchasedata.Cart.values[i].Price;
                item.IdProduct = DataManager.Instance.Product.GetByNameAsync(Purchasedata.Cart.values[i].Name).Result.IdProduct;
                item.CreationDate = Convert.ToDateTime(Purchasedata.Transaction.Date);
                list.Add(item);
            }
            return list;
        }
        private void MakeReceipt(TransactionMakerData Purchasedata){}

    }
}
