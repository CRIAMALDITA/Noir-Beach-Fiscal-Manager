using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RestaurantData;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers;

namespace SalesRestaurantSystem
{
    public class SearchSupplierHandler : SearchEntityHandler<SupplierData>
    {
        public List<SupplierData> SupplierData { get; set; }

        public SearchEntitiesWindow SearchEntitiesWindow { get; set; }


        public SearchSupplierHandler(){}

        public override SupplierData Search(string id)
        {
            SupplierData data = new SupplierData();
            try
            {
                var entities = id == "" ? DataManager.Instance.Supplier.GetAllAsync().Result : 
                    DataManager.Instance.Supplier.GetAllAsync().Result.Where(x => x.Document.ToString().Contains(id)).ToList();

                SupplierData = entities;
                switch (entities.Count())
                {
                    case 0: MessageBox.ShowEmergentMessage($"E_Cannot find any client with ID: {id}. Please set a valid ID"); break;
                    case 1: data = entities[0]; break;
                    default: data = OpenSearchWindow(entities, id); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ShowEmergentMessage($"E_Error to find entities. \nDetails:\n {ex.Message}");
                return null;
            }
            _idField.Text = data == null ? "" : data.Document.ToString();
            _nameField.Text = data == null ? "" : data.FiscalName.ToString();
            return data;
        }

        public SupplierData OpenSearchWindow(List<SupplierData> clientsFinded, string id)
        {
            SupplierData supplierData = null;
            SearchEntitiesWindow = new SearchEntitiesWindow();
            SearchEntitiesWindow.IDField.Text = id;
            GridView gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "ID",
                DisplayMemberBinding = new System.Windows.Data.Binding("ID"),
                Width = 100
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Fiscal Name",
                DisplayMemberBinding = new System.Windows.Data.Binding("Fiscal Name"),
                Width = 150
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Tel",
                DisplayMemberBinding = new System.Windows.Data.Binding("Telephone"),
                Width = 100
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Balance",
                DisplayMemberBinding = new System.Windows.Data.Binding(""),
                Width = 80
            });
            ObservableCollection<SupplierData> suppliers = new ObservableCollection<SupplierData>();
            clientsFinded.ForEach(suppliers.Add);
            SearchEntitiesWindow.ElementsList.View = gridView;
            SearchEntitiesWindow.ElementsList.ItemsSource = suppliers;
            SearchEntitiesWindow.CancelBTN.Click += (o, e) =>
            {
                EntityFinded = false;
                SearchEntitiesWindow.Close();
            };
            SearchEntitiesWindow.AcceptBTN.Click += (o, e) => 
            { 
                supplierData = SearchEntitiesWindow.ElementsList.SelectedItem as SupplierData;
                SearchEntitiesWindow.Close();
            };
            SearchEntitiesWindow.IDField.TextChanged += (o, e) =>
            {
                List<SupplierData> newData = new List<SupplierData>();
                newData = clientsFinded.Where(x => x.Document.Contains(SearchEntitiesWindow.IDField.Text)).ToList();
                suppliers.Clear();
                newData.ForEach(suppliers.Add);
            };
            SearchEntitiesWindow.ShowDialog();
            EntityFinded = supplierData != null;
            return supplierData;
        }
    }
}
