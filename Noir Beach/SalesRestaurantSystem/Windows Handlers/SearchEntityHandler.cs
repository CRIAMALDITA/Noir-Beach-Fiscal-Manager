using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public abstract class SearchEntityHandler<T> : IUISearchEntity<T> where T : class
    {

        protected TextBox _nameField;
        protected TextBox _idField;
        protected Button _searchBtn;

        public void SetSearchField(TextBox searchField)
        {
            _idField = searchField;
        }

        public void SetNameField(TextBox nameField)
        {
            _nameField = nameField;
        }

        public void SetSearchButton(Button button)
        {
            _searchBtn = button;
            _searchBtn.Click += (s, e) => Search(_idField.Text);
        }

        public abstract T Search(string id);

    }
}
