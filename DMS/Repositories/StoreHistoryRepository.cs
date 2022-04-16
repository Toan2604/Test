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
    public interface IStoreHistoryRepository
    {
        Task<bool> Create(StoreHistory StoreHistory);
        Task<bool> BulkMerge(List<StoreHistory> StoreHistories);
    }
    public class StoreHistoryRepository : IStoreHistoryRepository
    {
        private DataContext DataContext;
        public StoreHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(StoreHistory StoreHistory)
        {
            try
            {
                StoreHistoryDAO Old = await DataContext.StoreHistory
                .Where(x => x.StoreId == StoreHistory.StoreId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
                if (Old == null || (Old.StatusId != StoreHistory.StatusId || Old.EstimatedRevenueId != StoreHistory.EstimatedRevenueId))
                {
                    StoreHistoryDAO StoreHistoryDAO = new StoreHistoryDAO();
                    StoreHistoryDAO.Id = StoreHistory.Id;
                    StoreHistoryDAO.StoreId = StoreHistory.StoreId;
                    StoreHistoryDAO.Code = StoreHistory.Code;
                    StoreHistoryDAO.Name = StoreHistory.Name;
                    StoreHistoryDAO.StatusId = StoreHistory.StatusId;
                    StoreHistoryDAO.StoreStatusId = StoreHistory.StoreStatusId;
                    StoreHistoryDAO.AppUserId = StoreHistory.AppUserId;
                    StoreHistoryDAO.EstimatedRevenueId = StoreHistory.EstimatedRevenueId;
                    StoreHistoryDAO.Latitude = StoreHistory.Latitude;
                    StoreHistoryDAO.Longitude = StoreHistory.Longitude;
                    StoreHistoryDAO.CreatedAt = StaticParams.DateTimeNow;
                    DataContext.StoreHistory.Add(StoreHistoryDAO);
                    await DataContext.SaveChangesAsync();
                    StoreHistory.Id = StoreHistoryDAO.Id;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> BulkMerge(List<StoreHistory> StoreHistories)
        {
            List<StoreHistoryDAO> StoreHistoryDAOs = new List<StoreHistoryDAO>();
            foreach (StoreHistory StoreHistory in StoreHistories)
            {
                StoreHistoryDAO StoreHistoryDAO = new StoreHistoryDAO();
                StoreHistoryDAO.StoreId = StoreHistory.StoreId;
                StoreHistoryDAO.Code = StoreHistory.Code;
                StoreHistoryDAO.Name = StoreHistory.Name;
                StoreHistoryDAO.StatusId = StoreHistory.StatusId;
                StoreHistoryDAO.AppUserId = StoreHistory.AppUserId;
                StoreHistoryDAO.EstimatedRevenueId = StoreHistory.EstimatedRevenueId;
                StoreHistoryDAO.Latitude = StoreHistory.Latitude;
                StoreHistoryDAO.Longitude = StoreHistory.Longitude;
                StoreHistoryDAO.StoreStatusId = StoreHistory.StoreStatusId;
                StoreHistoryDAO.CreatedAt = StoreHistory.CreatedAt == StaticParams.DateTimeMin ? StaticParams.DateTimeNow : StoreHistory.CreatedAt;
                StoreHistoryDAOs.Add(StoreHistoryDAO);
            }
            await DataContext.BulkMergeAsync(StoreHistoryDAOs);
            foreach (var StoreHistory in StoreHistories)
            {
                StoreHistory.Id = StoreHistoryDAOs.Where(x => x.StoreId == StoreHistory.StoreId).Select(x => x.Id).FirstOrDefault();
            }
            return true;
        }


    }
}
