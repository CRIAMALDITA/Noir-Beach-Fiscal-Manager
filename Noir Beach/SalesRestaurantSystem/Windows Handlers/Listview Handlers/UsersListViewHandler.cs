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
    internal class UsersListViewHandler : ListViewPanelController<UserData>
    {
        public UsersListViewHandler(Window win) : base(win){}

        public override void RemoveItemToList(UserData value)
        {
            var element = Items.First(x => x.IdUser == value.IdUser);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<UserData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdUser", typeof(int));
            table.Columns.Add("Document", typeof(string));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Role", typeof(string));
            table.Columns.Add("Permission", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdUser"] = item.IdUser;
                row["Document"] = item.Document;
                row["FullName"] = item.FullName;
                row["Email"] = item.Email;
                row["Role"] = DataManager.Instance.Role.GetByIdAsync(item.IdRol).Result.RolName;
                row["Permission"] = DataManager.Instance.Permission.GetByIdAsync(item.IdPermission).Result.PermissionName;
                row["Date"] = item.CreationDate;
                table.Rows.Add(row);
            }

            return table;
        }
        public override UserData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var userData = new UserData
            {
                IdUser = Convert.ToInt32(row["IdUser"]),
                Document = row["Document"].ToString(),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                RoleData = DataManager.Instance.Role.GetByNameAsync(row["Role"].ToString()).Result,
                UserPassword = DataManager.Instance.User.GetByIdAsync(Convert.ToInt32(row["IdUser"])).Result.UserPassword,
                PermissionData = DataManager.Instance.Permission.GetByNameAsync(row["Permission"].ToString()).Result,
                IdRol = DataManager.Instance.Role.GetByNameAsync(row["Role"].ToString()).Result.IdRol,
                IdPermission = DataManager.Instance.Permission.GetByNameAsync(row["Permission"].ToString()).Result.IdPermission,
                CreationDate = Convert.ToDateTime(row["Date"]),
                UserState = DataManager.Instance.User.GetByIdAsync(Convert.ToInt32(row["IdUser"])).Result.UserState
            };

            return userData;
        }

        public override void Search(string type, string filter)
        {
            List<UserData> list = new List<UserData>();
            list = Items.ToList();
            if(filter != "")
            {
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "IdUser": if (!item.IdUser.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "ID": if (!item.Document.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "FullName": if (!item.FullName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Email": if (!item.Email.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "IdRole": if (!item.IdRol.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "IdPermission": if (!item.IdPermission.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "CreationDate": if (!item.CreationDate.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                    }
                }
            }
            SearchedItems.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                SearchedItems.Add(index);
            };
            RefreshListView();
        }
    }
}
