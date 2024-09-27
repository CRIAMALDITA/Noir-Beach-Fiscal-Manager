using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("PRODUCT_DATA")]
    public class ProductData
    {
        [SQLKey(KeyType.PK, typeof(ProductData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdProduct { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public bool ProductState { get; set; }
        [SQLKey(KeyType.FK, typeof(CategoryData))] public int IdCategory { get; set; }
        [ForeignKey("IdCategory")] public virtual CategoryData CategoryData { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
