using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using SalesRestaurantSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Point_of_sale_for_Restaurant
{
    public partial class PurchaseDetailsWindow : Window
    {
        private PurchaseData _purchaseData;

        public PurchaseDetailsWindow(PurchaseData data)
        {
            InitializeComponent();
            Show();
            WindowUtilities.CenterWindowOnScreen(this);
            _purchaseData = data;
            Date.Text = _purchaseData.CreationDate.ToString();
            Supplier.Text = _purchaseData.Supplier.FiscalName;
            ID.Text = _purchaseData.Supplier.Document;
            Total.Text = _purchaseData.Total.ToString();
            IdType.Text = _purchaseData.IdentificationType.ToString();
            User.Text = _purchaseData.User.FullName.ToString();
            PurchaseDetailsListViewHandler productList = new(this);
            productList.SetListViewer(ListViewer, null);
            List<PurchaseDetailsData> details = DataManager.Instance.PurchaseDetails.GetAllAsync().Result.Where(x => x.IdPurchase == _purchaseData.IdPurchase).ToList();
            productList.AddItemsToList(details);
        }
    }
}
