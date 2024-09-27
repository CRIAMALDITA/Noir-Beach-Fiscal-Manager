using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;

namespace RestaurantData
{
    internal class TableData
    {
        public int Id { get; set; }
        //public ClientData Client { get; set; } = ClientData.NoClient();
        public int Persons = 0;
    }
}
