using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("PURCHASE_DETAILS")]
    public class PurchaseDetailsData
    {
        [SQLKey(KeyType.PK, typeof(CategoryData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdPurchaseDetails { get; set; }
        [SQLKey(KeyType.FK, typeof(PurchaseData))] public int IdPurchase { get; set; }
        [SQLKey(KeyType.FK, typeof(ProductData))] public int IdProduct { get; set; }
        [ForeignKey("IdPurchase")] public virtual PurchaseData Purchase { get; set; }
        [ForeignKey("IdProduct")] public virtual ProductData Product { get; set; }

        public decimal PurchasePrice { get; set; }
        public int ProductCount { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}
