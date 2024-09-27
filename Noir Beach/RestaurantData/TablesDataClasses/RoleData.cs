using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("ROL")]
    public class RoleData
    {
        [Key] public int IdRol { get; set; }
        public string RolName { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
