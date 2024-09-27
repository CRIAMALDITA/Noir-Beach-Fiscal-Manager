using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("PRODUCT_CATEGORY")]
    public class CategoryData
    {
        [SQLKey(KeyType.PK, typeof(CategoryData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdCategory { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool CategoryState { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
