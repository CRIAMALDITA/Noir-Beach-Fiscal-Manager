﻿using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public class ProductRepository : Repository<ProductData>
    {
        public ProductRepository(QueryDataContext context) : base(context) {}

        public async override Task<List<ProductData>> GetAllAsync()
        {
            var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<ProductData>().AsNoTracking().Include(p => p.CategoryData));
            var list = result.Result.ToList();
            _context.ReleaseContext(result.Context);
            return list;
        }

        public async override Task<ProductData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdProduct == id);
        }
    }
}