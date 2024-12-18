using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RestaurantData.TablesDataClasses
{
    [Table("PURCHASE")]
    public class PurhcaseData
    {
        [SQLKey(KeyType.PK, typeof(CategoryData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdPurchase { get; set; }
        [SQLKey(KeyType.FK, typeof(RoleData))] public int IdUser { get; set; }
        [ForeignKey("IdUser")] public virtual UserData User { get; set; }
        [SQLKey(KeyType.FK, typeof(RoleData))] public int IdSupplier { get; set; }
        /*[ForeignKey("IdUser")] public virtual UserData Supplier { get; set; }*/
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string ClientIdentification { get; set; }
        public string ClientName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Exchange { get; set; }
        public decimal Total { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int InvoiceNumber { get; set; }
    }
}
