using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SalesRestaurantSystem.WindowsHandlers.ListviewHandlers
{
    public abstract class CartListViewHandler<T> : ListViewPanelController<CartElementData> where T : class
    {
        public CartListViewHandler(Window win) : base(win){}

        public override DataTable CreateDataTableFromGeneric(List<CartElementData> items)
        {
            var table = new DataTable();

            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Amount", typeof(string));
            table.Columns.Add("SubTotal", typeof(string));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["Name"] = item.Name;
                row["Price"] = item.Price.ToString("C");
                row["Amount"] = item.Amount.ToString();
                row["SubTotal"] = item.Subtotal.ToString("C");
                table.Rows.Add(row);
            }

            return table;
        }

        public override void AddItemToList(CartElementData value)
        {
            if (Items.Any(x => x.Name == value.Name))
            {
                Items.First(x => x.Name == value.Name).Amount++;
                var item = Items.First(x => x.Name == value.Name);
                item.Subtotal = item.Price * item.Amount;
                RefreshListView();
            }
            else base.AddItemToList(value);
        }
        public override CartElementData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];

            decimal price = 0;
            decimal subTotal = 0;
            decimal.TryParse(row["Price"].ToString().TrimStart('$'), out price);
            decimal.TryParse(row["Subtotal"].ToString().TrimStart('$'), out subTotal);
            var userData = new CartElementData
            {
                Name = row["Name"].ToString(),
                Price = price,
                Amount = Convert.ToInt32(row["Amount"]),
                Subtotal = subTotal,
           };
            return userData;
        }

        public abstract List<T> CreateGenericsFormCartElements();

        public override void RemoveItemToList(CartElementData value)
        {
            if (!Items.Any(x => x.Name == value.Name)) return;
            if (Items.First(x => x.Name == value.Name).Amount > 1)
            {
                Items.First(x => x.Name == value.Name).Amount--;
                RefreshListView();
                return;
            }
            if (Items.FirstOrDefault(value) != null) Items.Remove(Items.First(x => x.Name == value.Name));
            OnItemRemoved?.Invoke();
            RefreshListView();

        }

        public override void Search(string type, string filter){}
    }

    public class CartElementData
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int Amount { get; set; } = 0;
        public decimal Subtotal { get; set; } = 0;
    }
}
