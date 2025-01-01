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
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {

        public LoadingWindow(string _loadingName, Task<string> _task, Action<bool> _OnLoadComplete)
        {
            Initialize(_loadingName, _task, false, _OnLoadComplete);
        }
        public LoadingWindow(string _loadingName, Task<bool> _task, Action<bool> _OnLoadComplete)
        {
            Initialize(_loadingName, _task, false, _OnLoadComplete);
        }
        public LoadingWindow(string _loadingName, Task<string> _task, bool _hasEmergetMessage, Action<bool> _OnLoadComplete)
        {
            Initialize(_loadingName, _task, _hasEmergetMessage, _OnLoadComplete);
        }
        public LoadingWindow(string _loadingName, Task<bool> _task, bool _hasEmergetMessage, Action<bool> _OnLoadComplete)
        {
            Initialize(_loadingName, _task, _hasEmergetMessage, _OnLoadComplete);
        }

        public void Initialize(string _loadingName, Task<string> _task, bool _hasEmergetMessage, Action<bool> _OnLoadComplete)
        {
            InitializeComponent();
            Show();
            WindowUtilities.CenterWindowOnScreen(this);
            WindowName.Title = _loadingName;
            LoadingText.Text = $"{_loadingName}, please waite...";

            bool _loadComplete = false;
            string _msg = string.Empty;
            Task.Run(async () => 
            {
                _msg = await _task.ConfigureAwait(false);
                _loadComplete = true;
            }).ConfigureAwait(false);


            while (!_loadComplete) { }

            if (_hasEmergetMessage)
            {
                MessageBoxResult _result = MessageBox.ShowEmergentMessage(_msg);
                if (_result == MessageBoxResult.OK)
                {
                    string _unused = string.Empty;
                    _OnLoadComplete?.Invoke(!MessageBox.IsMessageError(_msg));
                    Close();
                }
            }
            else
            {
                _OnLoadComplete?.Invoke(!MessageBox.IsMessageError(_msg));
                Close();
            }
        }
        public void Initialize(string _loadingName, Task<bool> _task, bool _hasEmergetMessage, Action<bool> _OnLoadComplete)
        {
            InitializeComponent();
            WindowName.Title = _loadingName;
            LoadingText.Text = $"{_loadingName}, please waite...";

            bool _loadComplete = false;
            bool _result = false;
            Task.Run(async () =>
            {
                _result = await _task.ConfigureAwait(false);
                _loadComplete = true;
            }).ConfigureAwait(false);


            while (!_loadComplete) { }

            if (_hasEmergetMessage)
            {
                MessageBoxResult _msgResult = MessageBox.ShowEmergentMessage(_result ? $"I_Completed successfully!" : $"E_Something happened and the operation could not be completed.");
                if (_msgResult == MessageBoxResult.OK)
                {
                    string _unused = string.Empty;
                    _OnLoadComplete?.Invoke(_result);
                    Close();
                }
            }
            else
            {
                _OnLoadComplete?.Invoke(_result);
                Close();
            }
        }
    }
}
