using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IBrandHistoryRepository
    {
        Task<bool> BulkMerge(List<BrandHistory> BrandHistories);
        Task<bool> Create(BrandHistory BrandHistory);
    }
    public class BrandHistoryRepository : IBrandHistoryRepository
    {
        private DataContext DataContext;
        public BrandHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(BrandHistory BrandHistory)
        {
            try
            {
                BrandHistoryDAO Old = await DataContext.BrandHistory
                .Where(x => x.BrandId == BrandHistory.BrandId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultWithNoLockAsync();
                if (Old == null || (Old.StatusId != BrandHistory.StatusId || Old.Used != BrandHistory.Used))
                {
                    BrandHistoryDAO BrandHistoryDAO = new BrandHistoryDAO();
                    BrandHistoryDAO.Id = BrandHistory.Id;
                    BrandHistoryDAO.BrandId = BrandHistory.BrandId;
                    BrandHistoryDAO.StatusId = BrandHistory.StatusId;
                    BrandHistoryDAO.AppUserId = BrandHistory.AppUserId;
                    BrandHistoryDAO.Used = BrandHistory.Used;
                    BrandHistoryDAO.CreatedAt = StaticParams.DateTimeNow;
                    DataContext.BrandHistory.Add(BrandHistoryDAO);
                    await DataContext.SaveChangesAsync();
                    BrandHistory.Id = BrandHistoryDAO.Id;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> BulkMerge(List<BrandHistory> BrandHistories)
        {
            List<BrandHistoryDAO> BrandHistoryDAOs = new List<BrandHistoryDAO>();
            foreach (BrandHistory BrandHistory in BrandHistories)
            {
                BrandHistoryDAO BrandHistoryDAO = new BrandHistoryDAO
                {
                    BrandId = BrandHistory.BrandId,
                    AppUserId = BrandHistory.AppUserId,
                    StatusId = BrandHistory.StatusId,
                    Used = BrandHistory.Used,
                    CreatedAt = BrandHistory.CreatedAt == StaticParams.DateTimeMin ? StaticParams.DateTimeNow : BrandHistory.CreatedAt
                };
                BrandHistoryDAOs.Add(BrandHistoryDAO);
            }
            await DataContext.BulkMergeAsync(BrandHistoryDAOs);

            foreach (BrandHistory BrandHistory in BrandHistories)
            {
                BrandHistory.Id = BrandHistoryDAOs.Where(x => x.BrandId == BrandHistory.BrandId).Select(x => x.Id).FirstOrDefault();
            }
                return true;
        }
    }
}
