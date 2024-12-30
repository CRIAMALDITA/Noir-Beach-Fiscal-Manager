using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesRestaurantSystem
{
    internal class CategoryListViewHandler : ListViewPanelController<CategoryData>
    {
        public CategoryListViewHandler(Window win) : base(win){}

        public override void RemoveItemToList(CategoryData value)
        {
            var element = Items.First(x => x.IdCategory == value.IdCategory);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<CategoryData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("CategoryName", typeof(string));
            table.Columns.Add("CategoryState", typeof(string));
            table.Columns.Add("CreationDate", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdCategory"] = item.IdCategory;
                row["CategoryName"] = item.CategoryName;
                row["CategoryState"] = item.CategoryState;
                row["CreationDate"] = item.CreationDate;
                table.Rows.Add(row);
            }

            return table;
        }
        public override CategoryData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var userData = new CategoryData
            {
                IdCategory = Convert.ToInt32(row["IdCategory"]),
                CategoryName = row["CategoryName"].ToString(),
                CreationDate = Convert.ToDateTime(row["CreationDate"])
            };
            return userData;
        }

        public override void Search(string type, string filter)
        {
            List<CategoryData> list = new List<CategoryData>();
            list = Items.ToList();
            if(filter != "")
            {
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "ID": if (!item.IdCategory.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Name": if (!item.CategoryName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "State": if (!((item.CategoryState ? "Active" : "Inactive").ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", "")))) list.Remove(item); break;                   
                        case "CreationDate": if (!item.CreationDate.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                    }
                }
            }
            SearchedItems.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                int index = Items.IndexOf(list[i]);
                SearchedItems.Add(index);
            };
            if (SearchedItems.Count > 0) FilterSetted = true;
            RefreshListView();
        }

        public override void DobleClick()
        {
        }
    }
}
