using iText.StyledXmlParser.Jsoup.Select;
using RestaurantData;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

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
            type.SelectedIndex = 0;
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


            SellMakerData selldata = GetSaleData();

            SellData sell = GetSellDataFromFields(selldata);
            try 
            {
                await DataManager.Instance.Sell.AddAsync(sell);
            }
            catch(Exception ex)
            {
                MessageBox.ShowEmergentMessage("E_Cannot add Sell on SellData SQL.");
            }

            LoadingWindow addingSells = new LoadingWindow("Making Sale...",
            Task.Run<bool>(async () =>
            {
                try
                {
                    List<SellDetailsData> list = GetSellDetailsDatasFromFields(selldata);
                    for (int i = 0; i < list.Count; i++)
                    {
                        SellDetailsData item = list[i];
                        await DataManager.Instance.SellDetails.AddAsync(item);
                        ProductData product = await DataManager.Instance.Product.GetByIdAsync(item.IdProduct);
                        product.Stock = product.Stock - item.ProductCount;
                        await DataManager.Instance.Product.UpdateAsync(product);
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    MessageBox.ShowEmergentMessage("E_Cannot add SellDetails on SellDetailsData SQL.");
                    return false;
                }
            }),
            result =>
            {
                if (result)
                {
                    UpdateCatalog();
                    ClearTransaction();
                    MakeReceipt(selldata);
                    MessageBox.ShowEmergentMessage("I_Sale Completed!");
                }
            });
        }

        private bool VerifyFields()
        {
            SellMakerData selldata = GetSaleData();
            if (_searchClientsHandler.EntityFinded == false)
            {
                MessageBox.ShowEmergentMessage("E_Client is not selected");
                return false;
            }
            if (_transactionType.SelectedIndex == -1)
            {
                MessageBox.ShowEmergentMessage("E_Transaction type is not selected");
                return false;
            }
            if (selldata.Cart.values.Count == 0)
            {
                MessageBox.ShowEmergentMessage("E_Cart is Empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(selldata.Resume.PaysWith))
            {
                MessageBox.ShowEmergentMessage("E_Payment is empty");
                return false;
            }
            decimal paysWith = Convert.ToDecimal(selldata.Resume.PaysWith.Replace("$", "").Trim());

            decimal subTotal = Convert.ToDecimal(selldata.Resume.SubTotal.Replace("$", "").Trim());
            decimal total = Convert.ToDecimal(selldata.Resume.Total.Replace("$", "").Trim());

            if (paysWith < total)
            {
                MessageBox.ShowEmergentMessage("E_Payment is not enough");
                return false;
            }

            return true;
        }
        private SellData GetSellDataFromFields(SellMakerData selldata)
        {
            decimal paysWith = Convert.ToDecimal(selldata.Resume.PaysWith.Replace("$", "").Trim());
            decimal subTotal = Convert.ToDecimal(selldata.Resume.SubTotal.Replace("$", "").Trim());
            decimal total = Convert.ToDecimal(selldata.Resume.Total.Replace("$", "").Trim());
            SellData sell = new SellData();
            var data = DataManager.Instance.Sell.GetAllAsync().Result;

            sell.IdUser = DataManager.Instance.User.CurrentUserLogged.IdUser;
            sell.Total = total;
            sell.SubTotal = subTotal;
            sell.Exchange = (total - paysWith) * - 1;
            sell.IdentificationNumber = DataManager.Instance.User.CurrentUserLogged.Document;
            sell.IdentificationType = selldata.Transaction.Type;
            sell.ClientIdentification = selldata.Customer.Id;
            sell.ClientName = selldata.Customer.Name;
            sell.CreationDate = Convert.ToDateTime(selldata.Transaction.Date);
            sell.InvoiceNumber = data.Count;
            return sell;
        }
        private List<SellDetailsData> GetSellDetailsDatasFromFields(SellMakerData selldata)
        {
            List<SellDetailsData> list = new List<SellDetailsData>();
            for (int i = 0; i < selldata.Cart.values.Count; i++)
            {
                SellDetailsData item = new SellDetailsData();
                var result = DataManager.Instance.Sell.GetAllAsync().Result;
                item.IdSell = result.OrderByDescending(s => s.IdSell).FirstOrDefault().IdSell;
                item.SubTotal = selldata.Cart.values[i].Subtotal;
                item.ProductCount = selldata.Cart.values[i].Amount;
                item.SellPrice = selldata.Cart.values[i].Price;
                item.IdProduct = DataManager.Instance.Product.GetByNameAsync(selldata.Cart.values[i].Name).Result.IdProduct;
                item.CreationDate = Convert.ToDateTime(selldata.Transaction.Date);
                list.Add(item);
            }
            return list;
        }
        private void MakeReceipt(SellMakerData selldata)
        {
            var bussiness = DataManager.Instance.Bussiness.LoadData();

            List<ReceiptData.ReceiptElements> elements = new();

            for(int i = 0; i < selldata.Cart.values.Count; i++)
            {
                elements.Add(new ReceiptData.ReceiptElements()
                {
                    Count = selldata.Cart.values[i].Amount.ToString(),
                    Name = selldata.Cart.values[i].Name,
                    Price = selldata.Cart.values[i].Price.ToString(),
                    SubTotal = selldata.Cart.values[i].Subtotal.ToString(),
                });
            }

            ReceiptData receiptData = new ReceiptData()
            {
                Top = new ReceiptData.ReceiptTop()
                {
                    Id = bussiness.Tin,
                    CompanyName = bussiness.BusinessName,
                    Address = bussiness.Address
                },
                Client = new ReceiptData.ReceiptClientData()
                {
                    ClientName = selldata.Customer.Name,
                    ClientId = selldata.Customer.Id,
                    ClientAddress = DataManager.Instance.Client.GetByNameAsync(selldata.Customer.Name).Result.Telephone,
                    ClientMail = DataManager.Instance.Client.GetByNameAsync(selldata.Customer.Name).Result.Email,
                },
                Invoice = new ReceiptData.ReceiptInvoiceData()
                {
                    CreationDate = selldata.Transaction.Date,
                    InvoiceNum = DataManager.Instance.Sell.GetAllAsync().Result.Count().ToString(),
                    POS = "0",
                    Discount = selldata.Resume.Change,
                    Total = selldata.Resume.Total,
                    UserName = DataManager.Instance.User.CurrentUserLogged.FullName,
                    Elements = elements
                },
                Pay = new ReceiptData.PayData()
                {
                    PaysWith = selldata.Resume.PaysWith,
                    Change = selldata.Resume.Change,
                    PayType = "cash",
                    Total = selldata.Resume.Total,
                },
                Messagge = "THX U FOR UR PURCHASE :D"
            };

            ReceiptController.MakeReceipt(receiptData);
        }

    }
}
