﻿using System;
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
    public class PurchaseData
    {
        [SQLKey(KeyType.PK, typeof(CategoryData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdPurchase { get; set; }
        [SQLKey(KeyType.FK, typeof(UserData))] public int IdUser { get; set; }
        [ForeignKey("IdUser")] public virtual UserData User { get; set; }
        [SQLKey(KeyType.FK, typeof(SupplierData))] public int IdSupplier { get; set; }
        [ForeignKey("IdSupplier")] public virtual SupplierData Supplier { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public decimal Total { get; set; }
        public bool PurchaseState { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
