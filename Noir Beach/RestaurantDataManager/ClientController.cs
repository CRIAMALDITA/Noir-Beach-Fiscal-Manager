using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager
{
    public class ClientController : DataController<ClientData>
    {
        public ClientRepository ClientRepository;
        public ClientController(QueryDataContext context) : base(context)
        {
            ClientRepository = new(context);
            Repository = ClientRepository;
        }
    }
}
