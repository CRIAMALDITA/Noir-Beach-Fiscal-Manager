using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("PERMISSION")]
    public class PermissionData
    {
        [Key] public int IdPermission { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
