﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class SellDetailsController : DataController<SellDetailsData>
    {
        public SellDetailsRepository SellDetailsRepository;

        public SellDetailsController(QueryDataContext context) : base(context)
        {
            SellDetailsRepository = new(context);
            Repository = SellDetailsRepository;
        }

        public override async Task<List<SellDetailsData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            return true;
        }
    }
}