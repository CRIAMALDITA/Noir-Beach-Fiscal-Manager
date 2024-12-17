using RestaurantDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SalesRestaurantSystem
{
    public class MultiWindowHandlerWindow : Window, IUIPanel, IUIMultiWindowPanel
    {

        private Window _currentWindow;
        private Grid _mainGrid;
        private Grid _optionsGrid;
        private IUIPanel[] _windows;
        private IUIPanel _currentGridOpen;
        private Button[] _options;

        public Action onBackButtonPressed;

        private BackButtonController _backButtonController;
        

        public MultiWindowHandlerWindow(Window currentWindow, Grid mainGrid)
        {
            this._currentWindow = currentWindow;
            _mainGrid = mainGrid;
        }
        public void SetOptions(Button[] options) 
        { 
            _options = options;
            for (int i = 0; i < _options.Length; i++)
            {
                int index = i;
                _options[i].Click += (a,b) => OnSelectOption(index);
            }
        }
        public virtual void SetPanel(Grid grid, BackButtonController backBtn)
        {
            _optionsGrid = grid;
            _backButtonController =backBtn;
            _backButtonController.AddListener(GoBack);
        }
        public void SetWindows(IUIPanel[] grids)
        {
            _windows = grids;
            foreach (var item in _windows)
            {
                item.OnBackButtonPressed(ResetbackButton);
            }
        }

        private void ResetbackButton()
        {
            _backButtonController.ShowButton(true);
            _backButtonController.AddListener(GoBack);
        }

        public void OnSelectOption(int option)
        {
            EnableWindow(_windows[option]);
        }

        public void RefreshPanel()
        {
            this.RefreshWindow();
        }
        public void HideUI()
        {
            _mainGrid.Visibility = Visibility.Hidden;
        }
        public void ShowUI()
        {
            _backButtonController.ShowButton(true);
            ResetbackButton();
            _mainGrid.Visibility = Visibility.Visible;
        }

        public void EnableWindow(IUIPanel grid)
        {
            if(_currentGridOpen != null) _currentGridOpen.HideUI();
            _currentGridOpen = grid;
            _currentGridOpen.ShowUI();
        }
        public void GoBack()
        {
            _currentGridOpen?.HideUI();
            HideUI();
            _backButtonController.ShowButton(false);
            onBackButtonPressed?.Invoke();
        }

        public virtual void OnBackButtonPressed(Action action)
        {
            onBackButtonPressed += action;
        }
    }
}
