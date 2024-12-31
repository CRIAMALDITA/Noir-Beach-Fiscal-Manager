using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SalesRestaurantSystem
{
    static class WindowUtilities
    {
        public static void CenterWindowOnScreen(Window win)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = win.Width;
            double windowHeight = win.Height;
            win.Left = (screenWidth / 2) - (windowWidth / 2);
            win.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public static void RefreshWindow<T>(this T win) where T : Window
        {
            win.Dispatcher.Invoke(() => 
            {
                if (win != null)
                {
                    win.InvalidateVisual();
                    win.UpdateLayout();
                }
            });
        }

        public static void ShowInterface(Grid ui, bool show = true)
        {
            ui.Visibility = show ? Visibility.Visible : Visibility.Hidden;
        }
    }

    public static class WindowExtensions
    {
        public static void RefreshWindow(this Window win)
        {
            if (win != null)
            {
                win.InvalidateVisual();
                win.UpdateLayout();
            }
        }
    }
}
