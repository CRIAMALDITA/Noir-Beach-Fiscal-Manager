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
    public class SearchClientsHandler : SearchEntityHandler<ClientData>
    {
        public List<ClientData> ClientData { get; set; }

        public SearchClientsWindow SearchEntitiesWindow { get; set; }


        public SearchClientsHandler(){}

        public override ClientData Search(string id)
        {
            ClientData data = new ClientData();
            try
            {
                var entities = id == "" ? DataManager.Instance.Client.GetAllAsync().Result : 
                    DataManager.Instance.Client.GetAllAsync().Result.Where(x => x.Document.ToString().Contains(id)).ToList();

                ClientData = entities;
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
            _nameField.Text = data == null ? "" : data.FullName.ToString();
            return data;
        }

        public ClientData OpenSearchWindow(List<ClientData> clientsFinded, string id)
        {
            ClientData clientData = null;
            SearchEntitiesWindow = new SearchClientsWindow();
            SearchEntitiesWindow.IDField.Text = id;
            GridView gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "ID",
                DisplayMemberBinding = new System.Windows.Data.Binding("Document"),
                Width = 100
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                DisplayMemberBinding = new System.Windows.Data.Binding("FullName"),
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
                DisplayMemberBinding = new System.Windows.Data.Binding("AccountBalance"),
                Width = 80
            });
            ObservableCollection<ClientData> clients = new ObservableCollection<ClientData>();
            clientsFinded.ForEach(clients.Add);
            SearchEntitiesWindow.ElementsList.View = gridView;
            SearchEntitiesWindow.ElementsList.ItemsSource = clients;
            SearchEntitiesWindow.CancelBTN.Click += (o, e) =>
            {
                EntityFinded = false;
                SearchEntitiesWindow.Close();
            };
            SearchEntitiesWindow.AcceptBTN.Click += (o, e) => 
            { 
                clientData = SearchEntitiesWindow.ElementsList.SelectedItem as ClientData;
                SearchEntitiesWindow.Close();
            };
            SearchEntitiesWindow.IDField.TextChanged += (o, e) =>
            {
                List<ClientData> newData = new List<ClientData>();
                newData = clientsFinded.Where(x => x.Document.Contains(SearchEntitiesWindow.IDField.Text)).ToList();
                clients.Clear();
                newData.ForEach(clients.Add);
            };
            SearchEntitiesWindow.ShowDialog();
            EntityFinded = clientData != null;
            return clientData;
        }
    }
}
