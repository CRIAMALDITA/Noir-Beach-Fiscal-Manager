using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantDataManager
{
    public class BackButtonController 
    {
        private Button _button;
        private Action _action;
        public BackButtonController(Button button)
        {
            _button = button;
            _button.Click += (o, e) => InvokeCallBack();
        }

        public void ShowButton(bool show)
        {
            if(show) _button.Visibility = Visibility.Visible;
            else _button.Visibility = Visibility.Hidden;
        }

        public void AddListener(Action action)
        {
            _action = action;
        }

        public void InvokeCallBack()
        {
            _action?.Invoke();
        }
    }
}
