using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("SUPPLIER")]
    public class SupplierData
    {
        [SQLKey(KeyType.PK, typeof(ClientData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdSupplier { get; set; }
        public string Document { get; set; } = string.Empty;
        public string FiscalName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public bool SupplierState { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
