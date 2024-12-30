using RestaurantData.TablesDataClasses;
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
           
            if(subTotalField is not null) _subTotalField = subTotalField;
            
            if (paysWithField is not null)
            { 
                _paysWithField = paysWithField;
                _paysWithField.KeyDown += (o, e) => {
                    if (e.Key == Key.Enter) UpdatePrices();
                };
            }
           
            if(changeField is not null) _changeField = changeField;
            
            if (offField is not null) 
            { 
                _offField = offField;
                _offField.KeyDown += (o, e) => {
                    if (e.Key == Key.Enter) UpdatePrices(); };
            }

        }


        public void UpdatePrices()
        {
            decimal price = 0;

            for (int i = 0; i < CartListViewHandler.Items.Count; i++)
                price += CartListViewHandler.Items[i].Price * CartListViewHandler.Items[i].Amount;

            _subTotalField.Text = price.ToString("C", new System.Globalization.CultureInfo("en-US"));
            string offfield = _offField.Text.Replace("%", "").Trim();
            _offField.Text = _offField.Text.Contains('%') ? _offField.Text : _offField.Text + "%";
            decimal off = Convert.ToDecimal(offfield == "" ? "0" : offfield);
            if (_subTotalField is not null) _totalField.Text = (off > 0 ? (price - price * off / 100) : price).ToString("C", new System.Globalization.CultureInfo("en-US"));

            if(_paysWithField is not null && _changeField is not null)
            {
                _paysWithField.Text = Convert.ToDecimal(_paysWithField.Text == "" ? "0" : _paysWithField.Text.Replace("$", "").Trim()).ToString("C", new System.Globalization.CultureInfo("en-US"));
                decimal total = Convert.ToDecimal(_totalField.Text == "" ? "0" : _totalField.Text.Replace("$", ""));
                decimal pays = Convert.ToDecimal(_paysWithField.Text == "" ? "0" : _paysWithField.Text.Replace("$", ""));
                _changeField.Text = (pays - total).ToString("C", new System.Globalization.CultureInfo("en-US"));
            }
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
