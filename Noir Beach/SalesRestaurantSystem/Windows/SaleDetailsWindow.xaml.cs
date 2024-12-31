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
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;

namespace Point_of_sale_for_Restaurant
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SaleDetailsWindow : Window
    {
        private SellData sellData { get; set; }
        public SaleDetailsWindow(SellData data)
        {
            InitializeComponent();
            Show();
            WindowUtilities.CenterWindowOnScreen(this);

            sellData = data;
            Date.Text = data.CreationDate.ToString();
            FullName.Text = data.ClientName;
            ID.Text = data.ClientIdentification;
            User.Text = data.User.FullName;
            IDType.Text = data.IdentificationType;
            Change.Text = data.Exchange.ToString();
            Total.Text = data.Total.ToString();
            PaidWith.Text = (data.Total + data.Exchange).ToString();
            SellDetailsListViewHandler list = new SellDetailsListViewHandler(this);
            list.SetListViewer(ListView, null);
            List<SellDetailsData> details = DataManager.Instance.SellDetails.GetAllAsync().Result.Where(x => x.IdSell == sellData.IdSell).ToList();
            list.AddItemsToList(details);

        }
    }
}
