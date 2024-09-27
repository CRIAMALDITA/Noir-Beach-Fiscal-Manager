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
        {
            _subTotalField = subTotalField;
            _totalField = totalField;
            _paysWithField = paysWithField;
            _changeField = changeField;
            _offField = offField;

            KeyEventHandler action = (sender, e) =>
            {
                if(e.Key == Key.Enter)
                {
                    UpdatePrices();
                }
            };

            _offField.KeyDown += action;
            _paysWithField.KeyDown += action;

            _offField.LostFocus += (o, e) => UpdatePrices();
            _paysWithField.LostFocus += (o, e) => UpdatePrices();
        }

        public void UpdatePrices()
        {
            decimal price = 0;

            for (int i = 0; i < CartListViewHandler.Items.Count; i++)
                price += CartListViewHandler.Items[i].Price * CartListViewHandler.Items[i].Amount;

            _subTotalField.Text = price.ToString("C", new System.Globalization.CultureInfo("en-US"));
            decimal off = 0;
            try
            {
                string text = _offField.Text;
                if (decimal.TryParse(text.TrimEnd('%'), out off))
                {
                    _offField.Text = (off / 100).ToString("P");
                }
                else throw new Exception();
            }
            catch
            {
                off = 0;
                _offField.Text = 0.ToString("P");
            }
            decimal total = off > 0 ? price - price * off / 100 : price;
            _totalField.Text = total.ToString("C", new System.Globalization.CultureInfo("en-US"));

            decimal change = 0;
            try
            {
                decimal pays = 0;
                decimal.TryParse(_paysWithField.Text.TrimStart('$'), out pays);
                change = total - pays;
                change = change < 0 ? change * -1 : change;
                _paysWithField.Text = decimal.Parse(_paysWithField.Text).ToString("C", new System.Globalization.CultureInfo("en-US"));
                _changeField.Text = change.ToString("C", new System.Globalization.CultureInfo("en-US"));
            }
            catch
            {
                off = 0;
                _changeField.Text = 0.ToString("C", new System.Globalization.CultureInfo("en-US"));
            }

            _changeField.Text = (change >= 0 ? change : 0).ToString("C", new System.Globalization.CultureInfo("en-US"));
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
