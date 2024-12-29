using RestaurantDataManager;
using System.Data;
using System.Windows.Controls;

namespace SalesRestaurantSystem
{

     public interface IUIPanel
     {
         void SetPanel(Grid grid, BackButtonController backButton);
         void ShowUI();
         void HideUI();
         void GoBack();
         void OnBackButtonPressed(Action action);
     }

    public interface IUIDataWindow<T> where T : class
    {
        void SetFields(Control[] fields);
        void SetParameters();
    }

    public interface IUIMultiWindowPanel
     {
         void SetOptions(Button[] button);
         void SetWindows(IUIPanel[] grids);
         void OnSelectOption(int option);
         void EnableWindow(IUIPanel grid);
         void RefreshPanel();
     }

     public interface IUISearchBar<T> where T : class
     {
         void SetCategories(ComboBox box);
         void SetSearchField(TextBox searchField);
         void SetSearchButton(Button button);
         void InsertCategory();
         void Search(string category, string filter);
     }

     public interface IUISearchEntity<T> where T : class
     {
         void SetSearchField(TextBox searchField);
         void SetNameField(TextBox nameField);
         void SetSearchButton(Button button);
         T Search(string id);
     }

     public interface IUIDataGetterByID<T> where T : class
     {
         void SetIDField(TextBox id);
         void SetSearchButton(Button button);
         void OpenSearchWindow();
         void SetParameters(); 
     }

     public interface IUIPriceHandler
     {
         void SetFields(TextBox price, TextBox paysWith, TextBox change, TextBox off);
         void SetButton(Button button);
         void SendFinalPrice();
     }

    public interface IUIDataHandler<T> where T : class
    {
        void SetFields(Control[] fields);
        void SetRemoveItem(Button btn, Button trueRmbBtn);
        void SetRemoveHistory(Button btn, Button back);
        void SetExportItem(Button btn);
        void SetRecoveryItem(Button btn);
        void ShowRemoveHistory(bool show);
        Task<string> GetPKByName(Type dataType, string name);
        void TrueRemoveData(T[] items);
        void RecoveryItems(T[] items);
        void RemoveData(T[] items);
        void ExcelExport();
    }

    public interface IUIElementsHandler<T> where T : class
    {
        void SetAddItem(Button btn);
        void AddData();
        void ClearFields();

    }

    public interface IUITransactionsHandler<T> where T : class
    {
        void SetTransactionInformationFields(TextBox dateField, ComboBox type);
        void SetCustomerInformationFields(TextBox idField, Button button, TextBox nameField);
        void SetCatalogInformationFields(ListView elementsList, TextBox searchField, Button searchBtn);
        void SetTransactionResumeFields(ListView cartList, TextBox subTotal, TextBox off, TextBox total, TextBox paysWith, TextBox change, Button makeSale);
        void SearchInCatalog(string filter);
        void UpdateCatalog();
        void SearchCustomer();
        void AddToCart(T[] items);
        bool RemoveFromCart(T[] items);
        void MakeTransaction();
        void ClearTransaction();
    }

    public interface IUIDataViewer<T> : IUIPanel where T : class
    {
        void SetListViewer(ListView content, Grid bottomOptions);
        void SetListData(List<T> values);
        void Search(string type, string filter);
        void DobleClick();
        void AddItemToList(T value);
        void RemoveItemToList(T value);
        ListViewItem GetRowElementData(int index);
        void OnItemSelected(object sender, EventArgs e);
        void SetItemButtonAction(Action<DataRowView, int> action);
        void OnItemButtonPressed(object parameter);
        DataTable CreateDataTableFromGeneric(List<T> dict);
        T CreateGenericFromDataTable(DataTable table);
    }
}
