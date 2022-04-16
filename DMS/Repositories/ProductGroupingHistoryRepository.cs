using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IProductGroupingHistoryRepository
    {
        Task<bool> BulkMerge(List<ProductGroupingHistory> ProductGroupingHistories);
    }
    public class ProductGroupingHistoryRepository : IProductGroupingHistoryRepository
    {
        private DataContext DataContext;
        public ProductGroupingHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> BulkMerge(List<ProductGroupingHistory> ProductGroupingHistories)
        {
            List<ProductGroupingHistoryDAO> ProductGroupingHistoryDAOs = new List<ProductGroupingHistoryDAO>();
            foreach (ProductGroupingHistory ProductGroupingHistory in ProductGroupingHistories)
            {
                ProductGroupingHistoryDAO ProductGroupingHistoryDAO = new ProductGroupingHistoryDAO
                {
                    ProductGroupingId = ProductGroupingHistory.ProductGroupingId,
                    Code = ProductGroupingHistory.Code,
                    Name = ProductGroupingHistory.Name,
                    AppUserId = ProductGroupingHistory.AppUserId,
                    StatusId = ProductGroupingHistory.StatusId,
                    CreatedAt = ProductGroupingHistory.CreatedAt == StaticParams.DateTimeMin ? StaticParams.DateTimeNow : ProductGroupingHistory.CreatedAt
                };
                ProductGroupingHistoryDAOs.Add(ProductGroupingHistoryDAO);
            }
            await DataContext.BulkMergeAsync(ProductGroupingHistoryDAOs);

            return true;
        }
    }
}
