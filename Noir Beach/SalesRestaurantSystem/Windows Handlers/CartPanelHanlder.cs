using SalesRestaurantSystem.WindowsHandlers.ListviewHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public abstract class CartPanelHanlder<T> where T : class
    {
        public Window _window;
        
        private TextBox _subTotalField;
        private TextBox _paysWithField;
        private TextBox _changeField;
        private TextBox _offField;
        private TextBox _totalField;

        public Action<CartData> OnSendData;
        public Action OnElementAdded;
        public Action OnElementRemoved;

        public CartListViewHandler<T> CartListViewHandler { get; set; }

        public CartPanelHanlder(Window win)
        {
            _window = win;
        }

        public abstract void SetListView(ListView list);
        public abstract void AddElements(T[] items);
        public abstract bool RemoveElements(T[] items);
        public abstract List<CartElementData> GenericToCartElements(T[] items);

        public void SetFields(TextBox subTotalField, TextBox totalField, TextBox paysWithField, TextBox changeField, TextBox offField)
        {;
            _totalField = totalField;
        }

        public void UpdatePrices()
        {
            decimal price = 0;

            for (int i = 0; i < CartListViewHandler.Items.Count; i++)
                price += CartListViewHandler.Items[i].Price * CartListViewHandler.Items[i].Amount;

            _totalField.Text = price.ToString("C", new System.Globalization.CultureInfo("en-US"));
        }

        public void SendCartData()
        {
            CartData newData = new CartData()
            {
                Elements = CartListViewHandler.CreateGenericsFormCartElements(),
                SubTotal = Convert.ToDecimal(_subTotalField.Text),
                Off = Convert.ToDecimal(_offField),
                Total = Convert.ToDecimal(_totalField.Text),
                PaysWith = Convert.ToDecimal(_paysWithField.Text),
                Change = Convert.ToDecimal(_changeField.Text)
            };
            OnSendData?.Invoke(newData);
        }

        public struct CartData
        {
            public List<T> Elements;
            public decimal SubTotal;
            public decimal Off;
            public decimal Total;
            public decimal PaysWith;
            public decimal Change;
        }
    }
}
