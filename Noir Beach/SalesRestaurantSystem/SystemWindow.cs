using SalesRestaurantSystem;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestaurantDataManager;
using RestaurantData.TablesDataClasses;
using SalesRestaurantSystem.WindowsHandlers;


namespace Point_of_sale_for_Restaurant
{
    public partial class SystemWindow : Window
    {
        private UserData USERDATA;

        public ToolBarController Interface_ToolBar;

        //Maintenance
        public MultiWindowHandlerWindow Maintenance_OptionsWindow;
        public UserDataHandlerWindow Maintenance_UserConfigWindow;
        public ProductDataHandlerWindow Maintenance_ProductConfigWindow;
        public CategoryDataHandlerWindow Maintenance_CategoryConfigWindow;
        public BusinessDetaillsWindowHandler Maintenance_BusinessConfigWindow;
        //Sell
        public SalesMakerWindowHandler Sales;

        //Purchase
        public PurchaseMakerWindowHandler Purchases;

        //Clients
        public ClientsDataHandlerWindow Clients;

        //Suppliers
        public SuppliersDataHandlerWindow Suppliers;

        public BackButtonController Interface_BackButton;

        public SystemWindow(UserData user)
        {
            //Window
            InitializeComponent();
            Show();

            //User Grid
            USERDATA = user;
            SetUserDetails(USERDATA.FullName, USERDATA.Document, USERDATA.PermissionData.PermissionName);
            Interface_BackButton = new BackButtonController(Interface_Back);

            //Toolbar Config ---------------------------------------------------------------------------------

            List<ToolBarOption> options = new List<ToolBarOption>();

            ToolBarOption MaintenanceOption = new ToolBarOption
                (
                    this, ToolBar_Maintenance,
                    ToolBar_Maintenance_Shadow,
                    ToolBar_Maintenance_Selected,
                    () => { ActiveUI(Maintenance_OptionsWindow); }
                );
            options.Add(MaintenanceOption);

            ToolBarOption SaleOption = new ToolBarOption
                (
                    this, ToolBar_Sale,
                    ToolBar_Sale_Shadow,
                    ToolBar_Sale_Selected,
                () => ActiveUI(Sales)
                );
            options.Add(SaleOption);

            ToolBarOption PurchaseOption = new ToolBarOption
                (
                    this, ToolBar_Purchase,
                    ToolBar_Purchase_Shadow,
                    ToolBar_Purchase_Selected,
                () => ActiveUI(Purchases)
                );
            options.Add(PurchaseOption);

            ToolBarOption ClientsOption = new ToolBarOption
                (
                    this, ToolBar_Clients,
                    ToolBar_Clients_Shadow,
                    ToolBar_Clients_Selected,
                () => ActiveUI(Clients)
                );
            options.Add(ClientsOption);

            ToolBarOption SuppliersOption = new ToolBarOption
                (
                    this, ToolBar_Suppliers,
                    ToolBar_Suppliers_Shadow,
                    ToolBar_Suppliers_Selected,
                    () => ActiveUI(Suppliers)
                );
            options.Add(SuppliersOption);

            ToolBarOption ReportsOption = new ToolBarOption
                (
                    this, ToolBar_Reports,
                    ToolBar_Reports_Shadow,
                    ToolBar_Reports_Selected, null
                //() => { ActiveUI(Interface_Reports); }
                );
            options.Add(ReportsOption);

            Interface_ToolBar = new ToolBarController(this, options);

            //------------------------------------------------------------------------------------------------


            //Maintenance UserConfig Grid --------------------------------------------------------------------
            Maintenance_UserConfigWindow = new UserDataHandlerWindow(this);
            Maintenance_UserConfigWindow.SetPanel(Interface_Maintenance_User, Interface_BackButton);
            Maintenance_UserConfigWindow.SetSearchField(Interface_Maintenance_User_SearchField);
            Maintenance_UserConfigWindow.SetSearchButton(Interface_Maintenance_User_Header_SearchButton);
            Maintenance_UserConfigWindow.SetAddItem(Interface_Maintenance_User_ControlPanel_AddBTN);
            Maintenance_UserConfigWindow.SetRemoveItem(Interface_Maintenance_User_MainBottom_RemoveBTN, Interface_Maintenance_User_RemovedBottom_RemoveBTN);
            Maintenance_UserConfigWindow.SetExportItem(Interface_Maintenance_User_MainBottom_ExportBTN);
            Maintenance_UserConfigWindow.SetMainListView(Interface_Maintenance_User_ListView, Interface_Maintenance_User_MainBottom);
            Maintenance_UserConfigWindow.SetRecoveryItem(Interface_Maintenance_User_RemovedBottom_RecoveryBTN);
            Maintenance_UserConfigWindow.SetRemovedListView(Interface_Maintenance_User_RemovedListView, Interface_Maintenance_User_RemovedBottom);
            Maintenance_UserConfigWindow.SetRemoveHistory(Interface_Maintenance_User_MainBottom_ViewRemovedElemets, Interface_Maintenance_User_RemovedBottom_ViewMainElemets);
            Maintenance_UserConfigWindow.SetCategories(Interface_Maintenance_User_CategoryBox);
            Maintenance_UserConfigWindow.SetFields(
            [
                Interface_Maintenance_User_ControlPanel_IDField,
                Interface_Maintenance_User_ControlPanel_NameField,
                Interface_Maintenance_User_ControlPanel_EmailField,
                Interface_Maintenance_User_ControlPanel_PasswordField,
                Interface_Maintenance_User_ControlPanel_ConfirmPasswordField,
                Interface_Maintenance_User_ControlPanel_RoleBox,
                Interface_Maintenance_User_ControlPanel_PermissionBox,
                Interface_Maintenance_User_ControlPanel_StatusBox
            ]);
            //------------------------------------------------------------------------------------------------

            //Maintenance ProductsConfig Grid-----------------------------------------------------------------
            Maintenance_ProductConfigWindow = new ProductDataHandlerWindow(this);
            Maintenance_ProductConfigWindow.SetPanel(Interface_Maintenance_Products, Interface_BackButton);
            Maintenance_ProductConfigWindow.SetSearchField(Interface_Maintenance_Products_SearchField);
            Maintenance_ProductConfigWindow.SetSearchButton(Interface_Maintenance_Products_Header_SearchButton);
            Maintenance_ProductConfigWindow.SetAddItem(Interface_Maintenance_Products_ControlPanel_AddBTN);
            Maintenance_ProductConfigWindow.SetRemoveItem(Interface_Maintenance_Products_MainBottom_RemoveBTN, Interface_Maintenance_Products_RemovedBottom_RemoveBTN);
            Maintenance_ProductConfigWindow.SetExportItem(Interface_Maintenance_Products_MainBottom_ExportBTN);
            Maintenance_ProductConfigWindow.SetMainListView(Interface_Maintenance_Products_ListView, Interface_Maintenance_Products_MainBottom);
            Maintenance_ProductConfigWindow.SetRecoveryItem(Interface_Maintenance_Products_RemovedBottom_RecoveryBTN);
            Maintenance_ProductConfigWindow.SetRemovedListView(Interface_Maintenance_Products_RemovedListView, Interface_Maintenance_Products_RemovedBottom);
            Maintenance_ProductConfigWindow.SetRemoveHistory(Interface_Maintenance_Products_MainBottom_ViewRemovedElemets, Interface_Maintenance_Products_RemovedBottom_ViewMainElemets);
            Maintenance_ProductConfigWindow.SetCategories(Interface_Maintenance_Products_CategoryBox);
            Maintenance_ProductConfigWindow.SetFields(
            [
                Interface_Maintenance_Products_ControlPanel_CodeField,
                Interface_Maintenance_Products_ControlPanel_NameField,
                Interface_Maintenance_Products_ControlPanel_DescriptionField,
                Interface_Maintenance_Products_ControlPanel_CategoryBox,
                Interface_Maintenance_Products_ControlPanel_SellPrice,
                Interface_Maintenance_Products_ControlPanel_PurchasePrice,
                Interface_Maintenance_Products_ControlPanel_Stock,
                Interface_Maintenance_Products_ControlPanel_StatusBox
            ]);
            //------------------------------------------------------------------------------------------------

            //Maintenance CategoryConfig Grid-----------------------------------------------------------------
            Maintenance_CategoryConfigWindow = new CategoryDataHandlerWindow(this);
            Maintenance_CategoryConfigWindow.SetPanel(Interface_Maintenance_Category, Interface_BackButton);
            Maintenance_CategoryConfigWindow.SetSearchField(Interface_Maintenance_Category_SearchField);
            Maintenance_CategoryConfigWindow.SetSearchButton(Interface_Maintenance_Category_Header_SearchButton);
            Maintenance_CategoryConfigWindow.SetAddItem(Interface_Maintenance_Category_ControlPanel_AddBTN);
            Maintenance_CategoryConfigWindow.SetRemoveItem(Interface_Maintenance_Category_MainBottom_RemoveBTN, Interface_Maintenance_Category_RemovedBottom_RemoveBTN);
            Maintenance_CategoryConfigWindow.SetExportItem(Interface_Maintenance_Category_MainBottom_ExportBTN);
            Maintenance_CategoryConfigWindow.SetMainListView(Interface_Maintenance_Category_ListView, Interface_Maintenance_Category_MainBottom);
            Maintenance_CategoryConfigWindow.SetRecoveryItem(Interface_Maintenance_Category_RemovedBottom_RecoveryBTN);
            Maintenance_CategoryConfigWindow.SetRemovedListView(Interface_Maintenance_Category_RemovedListView, Interface_Maintenance_Category_RemovedBottom);
            Maintenance_CategoryConfigWindow.SetRemoveHistory(Interface_Maintenance_Category_MainBottom_ViewRemovedElemets, Interface_Maintenance_Category_RemovedBottom_ViewMainElemets);
            Maintenance_CategoryConfigWindow.SetCategories(Interface_Maintenance_Category_CategoryBox);
            Maintenance_CategoryConfigWindow.SetFields(
            [
                Interface_Maintenance_Category_ControlPanel_NameField,
                Interface_Maintenance_Category_ControlPanel_StatusBox
            ]);
            //------------------------------------------------------------------------------------------------

            //Maintenance BusinessConfig Grid ----------------------------------------------------------------
            Maintenance_BusinessConfigWindow = new BusinessDetaillsWindowHandler(this);
            Maintenance_BusinessConfigWindow.SetPanel(Interface_Maintenance_Business, Interface_BackButton);
            Maintenance_BusinessConfigWindow.SetField
                (
                    Interface_Maintenance_Business_Name,
                    Interface_Maintenance_Business_TinField,
                    Interface_Maintenance_Business_AddressField,
                    Interface_Maintenance_Business_Image
                );
            Maintenance_BusinessConfigWindow.SetButtons(Interface_Maintenance_Business_SaveBtn, Interface_Maintenance_Business_Upload);
            //------------------------------------------------------------------------------------------------

            //Maintenance MultiWindow Grid -------------------------------------------------------------------
            Maintenance_OptionsWindow = new MultiWindowHandlerWindow(this, Interface_Maintenance);
            Maintenance_OptionsWindow.SetPanel(Interface_Maintenance_Options, Interface_BackButton);
            Maintenance_OptionsWindow.SetOptions(
            [
                Interface_Maintenance_Options_1,
                Interface_Maintenance_Options_2,
                Interface_Maintenance_Options_3,
                Interface_Maintenance_Options_4
            ]);
            Maintenance_OptionsWindow.SetWindows(
            [
                Maintenance_UserConfigWindow,
                Maintenance_ProductConfigWindow,
                Maintenance_CategoryConfigWindow,
                Maintenance_BusinessConfigWindow,
            ]);
            Maintenance_OptionsWindow.OnBackButtonPressed(Interface_ToolBar.UnSelectCurrent);
            //------------------------------------------------------------------------------------------------

            //Sales Grid -------------------------------------------------------------------------------------
            Sales = new SalesMakerWindowHandler(this);
            Sales.SetPanel(Interface_Sales, Interface_BackButton);
            Sales.SetTransactionInformationFields(Interface_Sale_DateField, Interface_Sale_TypeBox);
            Sales.SetCustomerInformationFields(Interface_Sale_Search_IDField, Interface_Sale_Search_SearchBTN, Interface_Sale_Search_FullName);
            Sales.SetCatalogInformationFields(Interface_Sale_Products_ProductList, Interface_Sale_Products_SearchField, Interface_Sale_Products_SearchBtn);
            Sales.SetTransactionResumeFields
                (
                    Interface_Sale_Resume_Listview,
                    Interface_Sale_Resume_SubTotalField,
                    Interface_Sale_Resume_OffField,
                    Interface_Sale_Resume_TotalField,
                    Interface_Sale_Resume_PaysWithField,
                    Interface_Sale_Resume_ChangeField,
                    Interface_Sale_Resume_MakeSale
                );
            Sales.OnBackButtonPressed(Interface_ToolBar.UnSelectCurrent);
            //-------------------------------------------------------------------------------------------------

            //Purchase Grid -----------------------------------------------------------------------------------
            Purchases = new PurchaseMakerWindowHandler(this);
            Purchases.SetPanel(Interface_Purchase, Interface_BackButton);
            Purchases.SetTransactionInformationFields(Interface_Purchase_DateField, Interface_Purchase_TypeBox);
            Purchases.SetCustomerInformationFields(Interface_Purchase_Search_IDField, Interface_Purchase_Search_SearchBTN, Interface_Purchase_Search_FullName);
            Purchases.SetCatalogInformationFields(Interface_Purchase_Products_ProductList, Interface_Purchase_Products_SearchField, Interface_Purchase_Products_SearchBtn);
            Purchases.SetTransactionResumeFields
                (
                    Interface_Purchase_Resume_Listview,
                    null,
                    null,
                    Interface_Purchase_Resume_TotalField,
                    null,
                    null,
                    Interface_Purchase_Resume_MakePurchase
                );
            Purchases.OnBackButtonPressed(Interface_ToolBar.UnSelectCurrent);
            //-------------------------------------------------------------------------------------------------

            //Clients Grid ------------------------------------------------------------------------------------
            Clients = new ClientsDataHandlerWindow(this);
            Clients.SetPanel(Interface_Clients, Interface_BackButton);
            Clients.SetSearchField(Interface_Clients_SearchField);
            Clients.SetSearchButton(Interface_Clients_Header_SearchButton);
            Clients.SetAddItem(Interface_Clients_ControlPanel_AddBTN);
            Clients.SetRemoveItem(Interface_Clients_MainBottom_RemoveBTN, Interface_Clients_RemovedBottom_RemoveBTN);
            Clients.SetExportItem(Interface_Clients_MainBottom_ExportBTN);
            Clients.SetMainListView(Interface_Clients_ListView, Interface_Clients_MainBottom);
            Clients.SetRecoveryItem(Interface_Clients_RemovedBottom_RecoveryBTN);
            Clients.SetRemovedListView(Interface_Clients_RemovedListView, Interface_Clients_RemovedBottom);
            Clients.SetRemoveHistory(Interface_Clients_MainBottom_ViewRemovedElemets, Interface_Clients_RemovedBottom_ViewMainElemets);
            Clients.SetCategories(Interface_Clients_CategoryBox);
            Clients.SetFields(
            [
                Interface_Clients_ControlPanel_NameField,
                Interface_Clients_ControlPanel_IDField,
                Interface_Clients_ControlPanel_EmailField,
                Interface_Clients_ControlPanel_TelephoneField,
                Interface_Clients_ControlPanel_BalancePrice,
                Interface_Clients_ControlPanel_AccountBox,
                Interface_Clients_ControlPanel_StatusBox


            ]);
            //-------------------------------------------------------------------------------------------------

            //Suppliers Grid ----------------------------------------------------------------------------------
            Suppliers = new SuppliersDataHandlerWindow(this);
            Suppliers.SetPanel(Interface_Supplier, Interface_BackButton);
            Suppliers.SetSearchField(Interface_Supplier_SearchField);
            Suppliers.SetSearchButton(Interface_Supplier_Header_SearchButton);
            Suppliers.SetAddItem(Interface_Supplier_ControlPanel_AddBTN);
            Suppliers.SetRemoveItem(Interface_Supplier_MainBottom_RemoveBTN, Interface_Supplier_RemovedBottom_RemoveBTN);
            Suppliers.SetExportItem(Interface_Supplier_MainBottom_ExportBTN);
            Suppliers.SetMainListView(Interface_Supplier_ListView, Interface_Supplier_MainBottom);
            Suppliers.SetRecoveryItem(Interface_Supplier_RemovedBottom_RecoveryBTN);
            Suppliers.SetRemovedListView(Interface_Supplier_RemovedListView, Interface_Supplier_RemovedBottom);
            Suppliers.SetRemoveHistory(Interface_Supplier_MainBottom_ViewRemovedElemets, Interface_Supplier_RemovedBottom_ViewMainElemets);
            Suppliers.SetCategories(Interface_Supplier_CategoryBox);
            Suppliers.SetFields(
            [
                Interface_Supplier_ControlPanel_FiscalNameField,
                Interface_Supplier_ControlPanel_IDField,
                Interface_Supplier_ControlPanel_EmailField,
                Interface_Supplier_ControlPanel_TelephoneField,
                Interface_Supplier_ControlPanel_StatusBox


            ]);
            //-------------------------------------------------------------------------------------------------

            //Reports Multi Window ----------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------
            //Sales History -----------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------
            //Purchase History --------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------
        }


        public void SetUserDetails(string _userName, string _id, string _permission)
        {
            UserBlock_UserName_Text.Text = _userName;
            UserBlock_DNI_Text.Text = _id;
            UserBlock_Permission_Text.Text = _permission;
        }

        public void ChangeUser_BTN()
        {
        }

        #region UI Methods

        private IUIPanel _currentUIActive;

        public void ActiveUI(IUIPanel currentUIActive)
        {
            if(_currentUIActive != null) _currentUIActive.GoBack();
            _currentUIActive = currentUIActive;
            _currentUIActive.ShowUI();
        }
        #endregion

        public class Item
        {
            public string Description { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
        }
    }
}