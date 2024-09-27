using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData
{
    internal class MenuData
    {
        public string Name { get; set; } = string.Empty;
        public List<MenuCategoryData> CategoryData = new();
    }

    internal class MenuCategoryData
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<MenuItemData> Items = new(); 
    }

    internal class MenuItemData
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public required MenuCategoryData CategoryData { get; set; }
        public float Price { get; set; } = 0;
        public float Discount { get; set; } = 0;
    }
}
