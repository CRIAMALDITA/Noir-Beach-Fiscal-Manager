using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SalesRestaurantSystem
{
    public class ToolBarController
    {

        private ToolBarOption _optionSelected;
        private List<ToolBarOption> _options;
        private Window _currentWindow;


        public ToolBarController(Window window, List<ToolBarOption> options)
        {
            _currentWindow = window;
            _options = options;

            foreach (ToolBarOption option in _options)
            {
                option.onClick += (o, e) => OnOptionSelected(option);
            }
        }

        public void OnOptionSelected(ToolBarOption option)
        {
            UnSelectCurrent();
            _optionSelected = option;
            _optionSelected.ClickButtonAnimation(() => _optionSelected.SelectOption(true));
        }

        public void UnSelectCurrent()
        {
            if(_optionSelected == null) return;
            _optionSelected.SelectOption(false);
            //_optionSelected = null;
        }
        public void BlockByIndex(int index, bool block)
        {
            _options[index].SetBlock(block);
        }
    }
    public class ToolBarOption
    {
        private Button _button;

        public Rectangle _shadow;
        public Rectangle _selected;
        public Image _blocked;

        public RoutedEventHandler onClick;
        public MouseEventHandler onMouseEnter;
        public MouseEventHandler onMouseLeave;

        private bool _isSelected;
        private bool _isBlocked;
        private Window _window;

        public ToolBarOption(Window window, Button button, Rectangle shadow, Rectangle selected, Image blocked, Action OnClicked)
        {
            _window = window;
            this._button = button;
            _shadow = shadow;
            _selected = selected;
            _blocked = blocked;

            _button.MouseEnter += (o, e) => FocusOption(true);
            _button.MouseLeave += (o, e) => FocusOption(false);

            _button.Click += (o, e) => onClick?.Invoke(o, e);
            _button.Click += (o, e) => OnClicked?.Invoke();
            _button.MouseEnter += (o, e) => onMouseEnter?.Invoke(o, e);
            _button.MouseLeave += (o, e) => onMouseLeave?.Invoke(o, e);
        }

        public void SelectOption(bool select)
        {
            _window.Dispatcher.Invoke(() =>
            {
                _isSelected = select;
                _selected.Fill = select ? Brushes.LightSlateGray : Brushes.Transparent;
            });
        }
        public void FocusOption(bool focus)
        {
            if (_isSelected) return;
            _window.Dispatcher.Invoke(() =>
            {
                _shadow.Fill = focus ? Brushes.LightSteelBlue : Brushes.Transparent;
            });
        }

        public void ClickButtonAnimation(Action onComplete)
        {
            if(_isSelected) return;
            FocusOption(false);
            Brush brush = null;
            _window.Dispatcher.Invoke(() =>
            {
                brush = _shadow.Fill;
                _shadow.Fill = Brushes.SlateGray;
            });
            Task.Run(() =>
            {
                Thread.Sleep(100);
                _window.Dispatcher.Invoke(() =>
                {
                    _shadow.Fill = brush;
                    onComplete?.Invoke();
                });
            });
        }

        public void SetBlock(bool block)
        {
            _isBlocked = block;
            _blocked.Visibility = _isBlocked ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
