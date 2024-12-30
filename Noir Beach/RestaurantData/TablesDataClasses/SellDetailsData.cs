using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("SELL_DETAILS")]
    public class SellDetailsData
    {
        [SQLKey(KeyType.PK, typeof(CategoryData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdSellDetails { get; set; }
        [SQLKey(KeyType.FK, typeof(RoleData))] public int IdSell { get; set; }
        [SQLKey(KeyType.FK, typeof(RoleData))] public int IdProduct { get; set; }
        [ForeignKey("IdSell")] public virtual SellData Sell { get; set; }
        [ForeignKey("IdProduct")] public virtual ProductData Product { get; set; }

        public decimal SellPrice { get; set; }
        public int ProductsCount { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}
