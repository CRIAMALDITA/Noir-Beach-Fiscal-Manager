using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Point_of_sale_for_Restaurant.SystemWindow;
using RestaurantDataManager;
using System.Security.Cryptography;
using RestaurantData;
using System.Windows.Input;
using System.Reflection;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SalesRestaurantSystem
{
    public abstract class HistoryHandlerWindow<T> : Window, IUIPanel, IUISearchBar<T>, IUIDataHandler<T> where T : class
    {
        protected Window _currentWindow;

        protected Grid _mainWindowGrid;


        public BackButtonController BackButton;
        public Action OnBackPressed;

        //Search Bar
        protected TextBox _searchField;
        protected ComboBox _categorySearch;
        protected Button _searchButton;
        protected string _currentCategorySelected;

        //Data Viewer
        public ListViewPanelController<T> MainListViewPanel;
        public ListViewPanelController<T> RemovedListViewPanel;
        public ListViewPanelController<T> currentListOpen;


        //DataHandler
        protected T _currentItemGenerated;
        protected List<Control> _fields;
        protected Button _addItemBtn;
        protected Button _removeItemBtn;
        protected Button _trueRemoveItemBtn;
        protected Button _removeHistoryBtn;
        protected Button _backHistoryBtn;
        protected Button _recoveryItemBtn;
        protected Button _exportItemBtn;

        public HistoryHandlerWindow(Window currentWindow)
        {
            this._currentWindow = currentWindow;
        }
        public virtual void SetPanel(Grid grid, BackButtonController backBtn)
        {
            _mainWindowGrid = grid;
            BackButton = backBtn;
        }
        public virtual void SetSearchField(TextBox searchField)
        {
            _searchField = searchField;
        }
        public virtual void SetSearchButton(Button button)
        {
            _searchButton = button;
            _searchField.KeyDown += (o, e) =>
            {
                if (_searchField.IsSelectionActive && e.Key != System.Windows.Input.Key.Return && e.Key != System.Windows.Input.Key.Enter)
                    return;
                e.Handled = true;
                Search(_currentCategorySelected, _searchField.Text);
            };
            _searchButton.Click += (o, e) => Search(_currentCategorySelected, _searchField.Text);
        }
        public abstract void SetCategories(ComboBox box);

        public abstract void SetFields(Control[] fields);
        public virtual void SetMainListView(ListView mainListView, Grid bottom) 
        {
            MainListViewPanel.SetListViewer(mainListView, bottom);
        }
        public void SetRecoveryItem(Button btn)
        {
            _recoveryItemBtn = btn;
            _recoveryItemBtn.Click += (o, e) => RecoveryItems(RemovedListViewPanel.SelectedItems.ToArray());
        }

        public void SetRemoveHistory(Button btn, Button back)
        {
            _removeHistoryBtn = btn;
            _backHistoryBtn = back;
            _removeHistoryBtn.Click += (sender, o) =>
            {
                ShowRemoveHistory(true);
            };
            _backHistoryBtn.Click += (sender, o) =>
            {
                ShowRemoveHistory(false);
            };
        }
        public virtual void SetRemovedListView(ListView listView, Grid bottom)
        {
            RemovedListViewPanel.SetListViewer(listView, bottom);
        }

        public virtual void SetRemoveItem(Button btn, Button trueRmvBtn)
        {
            _removeItemBtn = btn;
            _trueRemoveItemBtn = trueRmvBtn;
            _trueRemoveItemBtn.Click += (o, e) => TrueRemoveData(RemovedListViewPanel.SelectedItems.ToArray());
            _removeItemBtn.Click += (o, e) => RemoveData(MainListViewPanel.SelectedItems.ToArray());
        }
        public virtual void SetExportItem(Button btn)
        {
            _exportItemBtn = btn;
        }
        public abstract bool SetParameters();

        public virtual void ShowUI()
        {
            WindowUtilities.ShowInterface(_mainWindowGrid);
            BackButton.AddListener(GoBack);
            BackButton.ShowButton(true);
        }
        public virtual void HideUI()
        {
            WindowUtilities.ShowInterface(_mainWindowGrid, false);
            BackButton.ShowButton(false);
        }
        public virtual void ShowRemoveHistory(bool show)
        {
            if (show)
            {
                MainListViewPanel.HideUI();
                RemovedListViewPanel.ShowUI();
                currentListOpen = RemovedListViewPanel;
            }
            else
            {
                RemovedListViewPanel.HideUI();
                MainListViewPanel.ShowUI();
                currentListOpen = MainListViewPanel;
            }

        }

        public abstract void InsertCategory();
        public abstract void Search(string category, string filter);
        
        public virtual void RemoveData(T[] items)
        {
            if (items.Length == 0)
            {
                MessageBox.ShowEmergentMessage($"W_Warning: Please select an element before remove.");
                return;
            }

            LoadingWindow removeBar = new LoadingWindow("Removing", Task.Run<bool>(async () =>
            {
                foreach (T item in items)
                {
                    var currentProperty = typeof(T).GetProperties().Where(x => x.Name.Contains("State")).First();
                    currentProperty.SetValue(item, false);
                    bool removeSuccessfullyawait = await DataManager.Instance.GenericController<T>().UpdateAsync(item).ConfigureAwait(false);
                    
                    if (!removeSuccessfullyawait)
                    {
                        MessageBox.ShowEmergentMessage($"E_Error: Data handler couldn't remove Item \'{item}\'");
                        return false;
                    }
                }
                return true;
            }), false, complete =>
            {
                if (complete)
                {
                    MainListViewPanel.RemoveItemsToList(items.ToList());
                    RemovedListViewPanel.AddItemsToList(items.ToList());
                    MessageBox.ShowEmergentMessage($"I_Items removed successfullly!");
                }
            });
        }
        public virtual void ExcelExport() { }

        public virtual void RefreshPanel()
        {
            this.RefreshWindow();
        }

        public abstract Task<string> GetPKByName(Type dataType, string name);


        public void TrueRemoveData(T[] items)
        {
            if (items.Length == 0)
            {
                MessageBox.ShowEmergentMessage($"W_Warning: Please select an element before remove.");
                return;
            }
            var result = MessageBox.ShowConfirmMessage("W_Warning: You are removing this file PERMANENTLY, do you want to continue?");
            if (result == MessageBoxResult.No) return;
            else
            {
                ConfirmPasswordMessageBox confirm = new ConfirmPasswordMessageBox();
                if (confirm.ShowDialog() == false) return;
            }

            LoadingWindow removeBar = new LoadingWindow("Removing", Task.Run<bool>(async () =>
            {
                foreach (T item in items)
                {
                    var properties = typeof(T).GetProperties();
                    int id = -1;
                    foreach (var propertie in properties)
                    {
                        var attribute = propertie.GetCustomAttribute<SQLKey>();
                        if (attribute != null && attribute.KeyType == KeyType.PK)
                        {
                            id = Convert.ToInt32(propertie.GetValue(item));
                        }
                    }
                    bool removeSuccessfullyawait = false;
                    if (id != -1) removeSuccessfullyawait = await DataManager.Instance.GenericController<T>().DeleteAsync(id);
                    else
                    {
                        MessageBox.ShowEmergentMessage($"E_Error: Data handler couldn't find Primary Key Item \'{item}\'");
                        return false;
                    }
                    if (!removeSuccessfullyawait)
                    {
                        MessageBox.ShowEmergentMessage($"E_Error: Data handler couldn't remove Item \'{id}\'");
                        return false;
                    }
                }
                return true;
            }), false, complete =>
            {
                if (complete)
                {
                    RemovedListViewPanel.RemoveItemsToList(items.ToList());
                    MessageBox.ShowEmergentMessage($"I_Items removed successfullly!");
                }
            });
        }

        public void RecoveryItems(T[] items)
        {
            if(items.Length == 0)
            {
                MessageBox.ShowEmergentMessage($"W_Warning: Please select an element before recovery.");
                return;
            }
            List<T> newMainDAta = new List<T>();
            List<T> newRemoveData = new List<T>();
            LoadingWindow removeBar = new LoadingWindow("Recovering", Task.Run<bool>(async () =>
            {
                foreach (T item in items)
                {
                    var currentProperty = typeof(T).GetProperties().Where(x => x.Name.Contains("State")).First();
                    currentProperty.SetValue(item, true);
                    bool removeSuccessfullyawait = await DataManager.Instance.GenericController<T>().UpdateAsync(item).ConfigureAwait(false);

                    if (!removeSuccessfullyawait)
                    {
                        MessageBox.ShowEmergentMessage($"E_Error: Data handler couldn't recovery Item \'{item}\'");
                        return false;
                    }
                }
                return true;
            }), false, complete =>
            {
                if (complete)
                {
                    MainListViewPanel.AddItemsToList(items.ToList());
                    RemovedListViewPanel.RemoveItemsToList(items.ToList());
                    MessageBox.ShowEmergentMessage($"I_Items recovered successfullly!");
                }
            });
        }

        public virtual void GoBack()
        {
            HideUI();
            OnBackPressed?.Invoke();
        }

        public void OnBackButtonPressed(Action action)
        {
            OnBackPressed = action;
        }
    }
}