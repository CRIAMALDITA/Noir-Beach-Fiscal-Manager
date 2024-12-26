using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData.TablesDataClasses
{
    [Table("CLIENT")]
    public class ClientData
    {
        [SQLKey(KeyType.PK, typeof(ClientData))][Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdClient { get; set; }
        public string? Document { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Telephone { get; set; } = string.Empty;
        public bool Account { get; set; } = false;
        public decimal AccountBalance { get; set; } = 0;
        public bool ClientState { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool ClientState { get; set; } = false;
    }
}
