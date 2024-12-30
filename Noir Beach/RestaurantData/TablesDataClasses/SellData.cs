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
    [Table("SELL")]
    public class SellData
    {
        [SQLKey(KeyType.PK, typeof(SellData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdSell { get; set; }
        [SQLKey(KeyType.FK, typeof(UserData))] public int IdUser { get; set; }
        [ForeignKey("IdUser")] public virtual UserData User { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string ClientIdentification { get; set; }
        public string ClientName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Exchange { get; set; }
        public decimal Total { get; set; }
        public bool SaleState { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int InvoiceNumber { get; set; }
    }
}
