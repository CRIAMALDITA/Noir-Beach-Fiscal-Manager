using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Point_of_sale_for_Restaurant.SystemWindow;
using System.Windows.Controls;
using System.Windows;
using RestaurantData;
using System.Data;
using System.Reflection;
using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;

namespace SalesRestaurantSystem
{
    public abstract class ListViewPanelController<T> : IUIDataViewer<T> where T : class
    {
        protected ListView _listView;
        protected Window _window;
        protected Grid _bottonOptions;
        public ObservableCollection<T> Items { get; private set; } = new ObservableCollection<T>();
        public List<int> SearchedItems = new();
        public List<T> SelectedItems = new();

        public Action OnItemAdded;
        public Action OnItemRemoved;
        public ICommand OnItemButtonPressedCommand { get; private set; }


        private Action<DataRowView, int> _itemListAction;

        private List<ListButtonElement> _buttons = new List<ListButtonElement>();

        protected string _categorySearch;

        public ListViewPanelController(Window win)
        {
            _window = win;
        }

        public void SetListViewer(ListView content, Grid bottomOptions)
        {
            _listView = content;
            _bottonOptions = bottomOptions;
            _listView.SelectionChanged += OnItemSelected;
            _listView.DataContext = this;
            OnItemButtonPressedCommand = new RelayCommand(OnItemButtonPressed);
            Debug.WriteLine(_listView.DataContext);

            Items = new ObservableCollection<T>();

            Items.CollectionChanged += (o, e) => RefreshListView();
            _listView.MouseEnter += (o, e) => RefreshListView();
            for (int i = 0; i < Items.Count; i++)
            {
                int index = i;
                SearchedItems.Add(index);
            }
            RefreshListView();

        }
        public void RefreshListView()
        {
            _window.Dispatcher.Invoke(new Action(() =>
            {
                _listView.ItemsSource = null;
                _listView.Items.Clear();
                var visibleItems = new List<T>();
                if (SearchedItems.Count == 0) visibleItems = Items.ToList();
                else
                    for (int i = 0; i < SearchedItems.Count; i++)
                    {
                        visibleItems.Add(Items[SearchedItems[i]]);
                    }
                _listView.ItemsSource = CreateDataTableFromGeneric(visibleItems.ToList()).DefaultView;
                WindowUtilities.RefreshWindow(_window);
            }));
        }
        public void SetListData(List<T> values)
        {
            foreach (var item in values)
            {
                Items.Add(item);
            }; 
            OnItemAdded?.Invoke();
        }
        public void OnItemSelected(object sender, EventArgs e)
        {
            _window.Dispatcher.Invoke(() =>
            {
                SelectedItems.Clear();
                foreach (var selectedItem in _listView.SelectedItems)
                {
                    var instance = Activator.CreateInstance<T>();
                    DataTable table = null;
                    if (selectedItem is DataRowView rowView)
                    {
                        DataRow row = rowView.Row;

                        table = row.Table.Clone();
                        table.ImportRow(row);
                    }
                    var element = CreateGenericFromDataTable(table);
                    var elementToAdd = new Dictionary<string, string>();
                    SelectedItems.Add(element);
                }
            });
        }
        public void AddItemsToList(List<T> items)
        {
            items.ForEach(AddItemToList);
            OnItemAdded?.Invoke();
        }
        public virtual void AddItemToList(T value)
        {
            if (Items.FirstOrDefault(value) != null) Items.Add(value);
            OnItemAdded?.Invoke();
        }
        public void RemoveItemsToList(List<T> items)
        {
            items.ForEach(RemoveItemToList);
            OnItemRemoved?.Invoke();
        }
        public abstract void RemoveItemToList(T value);
        public void ShowUI()
        {
            _listView.Visibility = Visibility.Visible;
            _bottonOptions.Visibility = Visibility.Visible;
        }
        public void HideUI()
        {
            _listView.Visibility = Visibility.Hidden;
            _bottonOptions.Visibility = Visibility.Hidden;
        }
        public abstract DataTable CreateDataTableFromGeneric(List<T> items);
        public abstract T CreateGenericFromDataTable(DataTable table);
        public abstract void Search(string type, string filter);
        public void GoBack(){}
        public void OnBackButtonPressed(Action action){}
        public void SetItemButtonAction(Action<DataRowView, int> action)
        {
            _itemListAction += action;
        }

        public void OnItemButtonPressed(object parameter) 
        {
            if (parameter is DataRowView rowView)
            {
                int index = 0;
                for(int i = 0; i < _listView.Items.Count; i++)
                {
                    if (_listView.Items[0] == parameter)
                    {
                        index = i; 
                        break;
                    }
                }
                _itemListAction?.Invoke(rowView, index);
            }
        }

        //unused
        public void SetPanel(Grid grid, BackButtonController backBtn) { }

        public ListViewItem GetRowElementData(int index)
        {
            var item = _listView.Items[index];
            return _listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
        }

        public struct ListButtonElement
        {
            public T Value { get; set; }
            public Button Button { get; set; }
            public bool Setted = false;

            public ListButtonElement(){}
        }
    }
}
