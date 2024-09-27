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

namespace SalesRestaurantSystem
{

    public partial class SearchClientsWindow: Window
    {
        public ListView ElementsList;
        public TextBox IDField;

        public Button CancelBTN;
        public Button AcceptBTN;

        public SearchClientsWindow()
        {
            InitializeComponent();
            ElementsList = SearchEntities_List;
            IDField = SearchEntities_ID;
            CancelBTN = SearchEntities_Cancel;
            AcceptBTN = SearchEntities_Accept;
        }
    }
}
