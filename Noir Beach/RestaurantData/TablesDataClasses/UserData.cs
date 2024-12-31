using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("USERDATA")]
    public class UserData
    {
        [SQLKey(KeyType.PK, typeof(UserData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdUser { get; set; }
        [Required][MaxLength(50)] public string Document { get; set; } = string.Empty;
        [Required][MaxLength(50)] public string FullName { get; set; } = string.Empty;
        [Required][MaxLength(50)] public string Email { get; set; } = string.Empty;
        [Required][MaxLength(50)] public string UserPassword { get; set; } = string.Empty;
        [Required] public bool UserState { get; set; }
        [Required][SQLKey(KeyType.FK, typeof(RoleData))] public int IdRol { get; set; }
        [Required][SQLKey(KeyType.FK, typeof(PermissionData))] public int IdPermission { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [ForeignKey("IdRol")] public virtual RoleData RoleData { get; set; }
        [ForeignKey("IdPermission")] public virtual PermissionData PermissionData { get; set; }
    }
}
