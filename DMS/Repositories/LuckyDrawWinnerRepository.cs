using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using DMS.Entities;
using DMS.Models;
using DMS.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.Repositories
{
    public interface ILuckyDrawWinnerRepository
    {
        Task<int> CountAll(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<int> Count(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<List<LuckyDrawWinner>> List(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<List<LuckyDrawWinner>> List(List<long> Ids);
        Task<LuckyDrawWinner> Get(long Id);
        Task<bool> Create(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> Update(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> Delete(LuckyDrawWinner LuckyDrawWinner);
        Task<bool> BulkMerge(List<LuckyDrawWinner> LuckyDrawWinners);
        Task<bool> BulkDelete(List<LuckyDrawWinner> LuckyDrawWinners);
    }
    public class LuckyDrawWinnerRepository : ILuckyDrawWinnerRepository
    {
        private DataContext DataContext;
        public LuckyDrawWinnerRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<LuckyDrawWinnerDAO>> DynamicFilter(IQueryable<LuckyDrawWinnerDAO> query, LuckyDrawWinnerFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Time, filter.Time);
            query = query.Where(q => q.LuckyDrawId, filter.LuckyDrawId);
            query = query.Where(q => q.LuckyDrawRegistrationId, filter.LuckyDrawRegistrationId);
            query = query.Where(q => q.LuckyDrawStructureId, filter.LuckyDrawStructureId);
            query = query.Where(q => q.LuckyDrawNumberId, filter.LuckyDrawNumberId);
            query = query.Where(q => q.LuckyDrawRegistration.AppUserId, filter.AppUserId);
            query = query.Where(q => q.LuckyDrawRegistration.StoreId, filter.StoreId);
            query = query.Where(q => q.LuckyDrawRegistration.IsDrawnByStore, filter.IsDrawnByStore);
            if (filter.Used.HasValue)
            {
                if (filter.Used.Value) // giải thưởng đã được quay
                {
                    query = query.Where(x => x.LuckyDrawNumberId.HasValue);
                }
                else if (!filter.Used.Value)
                {
                    query = query.Where(x => !x.LuckyDrawNumberId.HasValue);
                }
            }

            return query;
        }

        private async Task<IQueryable<LuckyDrawWinnerDAO>> OrFilter(IQueryable<LuckyDrawWinnerDAO> query, LuckyDrawWinnerFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyDrawWinnerDAO> initQuery = query.Where(q => false);
            foreach (LuckyDrawWinnerFilter LuckyDrawWinnerFilter in filter.OrFilter)
            {
                IQueryable<LuckyDrawWinnerDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyDrawWinnerFilter.Id);
                queryable = queryable.Where(q => q.Time, LuckyDrawWinnerFilter.Time);
                queryable = queryable.Where(q => q.LuckyDrawId, LuckyDrawWinnerFilter.LuckyDrawId);
                queryable = queryable.Where(q => q.LuckyDrawRegistrationId, LuckyDrawWinnerFilter.LuckyDrawRegistrationId);
                queryable = queryable.Where(q => q.LuckyDrawStructureId, LuckyDrawWinnerFilter.LuckyDrawStructureId);
                queryable = queryable.Where(q => q.LuckyDrawNumberId, LuckyDrawWinnerFilter.LuckyDrawNumberId);
                queryable = queryable.Where(q => q.LuckyDrawRegistration.AppUserId, filter.AppUserId);
                queryable = queryable.Where(q => q.LuckyDrawRegistration.StoreId, filter.StoreId);
                queryable = queryable.Where(q => q.LuckyDrawRegistration.IsDrawnByStore, filter.IsDrawnByStore);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<LuckyDrawWinnerDAO> DynamicOrder(IQueryable<LuckyDrawWinnerDAO> query, LuckyDrawWinnerFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawWinnerOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDraw:
                            query = query.OrderBy(q => q.LuckyDrawId);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDrawStructure:
                            query = query.OrderBy(q => q.LuckyDrawStructureId);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDrawRegistration:
                            query = query.OrderBy(q => q.LuckyDrawRegistrationId);
                            break;
                        case LuckyDrawWinnerOrder.Time:
                            query = query.OrderBy(q => q.Time);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDrawNumber:
                            query = query.OrderBy(q => q.LuckyDrawNumberId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawWinnerOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDraw:
                            query = query.OrderByDescending(q => q.LuckyDrawId);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDrawStructure:
                            query = query.OrderByDescending(q => q.LuckyDrawStructureId);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDrawRegistration:
                            query = query.OrderByDescending(q => q.LuckyDrawRegistrationId);
                            break;
                        case LuckyDrawWinnerOrder.Time:
                            query = query.OrderByDescending(q => q.Time);
                            break;
                        case LuckyDrawWinnerOrder.LuckyDrawNumber:
                            query = query.OrderByDescending(q => q.LuckyDrawNumberId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyDrawWinner>> DynamicSelect(IQueryable<LuckyDrawWinnerDAO> query, LuckyDrawWinnerFilter filter)
        {
            List<LuckyDrawWinner> LuckyDrawWinners = await query.Select(q => new LuckyDrawWinner()
            {
                Id = filter.Selects.Contains(LuckyDrawWinnerSelect.Id) ? q.Id : default(long),
                LuckyDrawId = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDraw) ? q.LuckyDrawId : default(long),
                LuckyDrawStructureId = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDrawStructure) ? q.LuckyDrawStructureId : default(long),
                LuckyDrawRegistrationId = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDrawRegistration) ? q.LuckyDrawRegistrationId : default(long),
                Time = filter.Selects.Contains(LuckyDrawWinnerSelect.Time) ? q.Time : default(DateTime),
                LuckyDrawNumberId = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDrawNumber) ? q.LuckyDrawNumberId : null,
                LuckyDraw = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDraw) && q.LuckyDraw != null ? new LuckyDraw
                {
                    Id = q.LuckyDraw.Id,
                    Code = q.LuckyDraw.Code,
                    Name = q.LuckyDraw.Name,
                    LuckyDrawTypeId = q.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = q.LuckyDraw.OrganizationId,
                    RevenuePerTurn = q.LuckyDraw.RevenuePerTurn,
                    StartAt = q.LuckyDraw.StartAt,
                    EndAt = q.LuckyDraw.EndAt,
                    AvatarImageId = q.LuckyDraw.AvatarImageId,
                    ImageId = q.LuckyDraw.ImageId,
                    Description = q.LuckyDraw.Description,
                    StatusId = q.LuckyDraw.StatusId,
                    Used = q.LuckyDraw.Used,
                } : null,
                LuckyDrawRegistration = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDrawRegistration) && q.LuckyDrawRegistration != null ? new LuckyDrawRegistration
                {
                    Id = q.LuckyDrawRegistration.Id,
                    LuckyDrawId = q.LuckyDrawRegistration.LuckyDrawId,
                    AppUserId = q.LuckyDrawRegistration.AppUserId,
                    StoreId = q.LuckyDrawRegistration.StoreId,
                    Revenue = q.LuckyDrawRegistration.Revenue,
                    TurnCounter = q.LuckyDrawRegistration.TurnCounter,
                    IsDrawnByStore = q.LuckyDrawRegistration.IsDrawnByStore,
                    Time = q.LuckyDrawRegistration.Time,
                } : null,
                LuckyDrawStructure = filter.Selects.Contains(LuckyDrawWinnerSelect.LuckyDrawStructure) && q.LuckyDrawStructure != null ? new LuckyDrawStructure
                {
                    Id = q.LuckyDrawStructure.Id,
                    LuckyDrawId = q.LuckyDrawStructure.LuckyDrawId,
                    Name = q.LuckyDrawStructure.Name,
                    Value = q.LuckyDrawStructure.Value,
                    Quantity = q.LuckyDrawStructure.Quantity,
                } : null,
                RowId = q.RowId,
            }).ToListWithNoLockAsync();
            return LuckyDrawWinners;
        }

        public async Task<int> CountAll(LuckyDrawWinnerFilter filter)
        {
            IQueryable<LuckyDrawWinnerDAO> LuckyDrawWinnerDAOs = DataContext.LuckyDrawWinner.AsNoTracking();
            LuckyDrawWinnerDAOs = await DynamicFilter(LuckyDrawWinnerDAOs, filter);
            return await LuckyDrawWinnerDAOs.CountWithNoLockAsync();
        }

        public async Task<int> Count(LuckyDrawWinnerFilter filter)
        {
            IQueryable<LuckyDrawWinnerDAO> LuckyDrawWinnerDAOs = DataContext.LuckyDrawWinner.AsNoTracking();
            LuckyDrawWinnerDAOs = await DynamicFilter(LuckyDrawWinnerDAOs, filter);
            LuckyDrawWinnerDAOs = await OrFilter(LuckyDrawWinnerDAOs, filter);
            return await LuckyDrawWinnerDAOs.CountWithNoLockAsync();
        }

        public async Task<List<LuckyDrawWinner>> List(LuckyDrawWinnerFilter filter)
        {
            if (filter == null) return new List<LuckyDrawWinner>();
            IQueryable<LuckyDrawWinnerDAO> LuckyDrawWinnerDAOs = DataContext.LuckyDrawWinner.AsNoTracking();
            LuckyDrawWinnerDAOs = await DynamicFilter(LuckyDrawWinnerDAOs, filter);
            LuckyDrawWinnerDAOs = await OrFilter(LuckyDrawWinnerDAOs, filter);
            LuckyDrawWinnerDAOs = DynamicOrder(LuckyDrawWinnerDAOs, filter);
            List<LuckyDrawWinner> LuckyDrawWinners = await DynamicSelect(LuckyDrawWinnerDAOs, filter);
            return LuckyDrawWinners;
        }

        public async Task<List<LuckyDrawWinner>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<LuckyDrawWinnerDAO> query = DataContext.LuckyDrawWinner.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<LuckyDrawWinner> LuckyDrawWinners = await query.AsNoTracking()
            .Select(x => new LuckyDrawWinner()
            {
                RowId = x.RowId,
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                LuckyDrawStructureId = x.LuckyDrawStructureId,
                LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                Time = x.Time,
                LuckyDrawNumberId = x.LuckyDrawNumberId,
                LuckyDraw = x.LuckyDraw == null ? null : new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    Code = x.LuckyDraw.Code,
                    Name = x.LuckyDraw.Name,
                    LuckyDrawTypeId = x.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = x.LuckyDraw.OrganizationId,
                    RevenuePerTurn = x.LuckyDraw.RevenuePerTurn,
                    StartAt = x.LuckyDraw.StartAt,
                    EndAt = x.LuckyDraw.EndAt,
                    AvatarImageId = x.LuckyDraw.AvatarImageId,
                    ImageId = x.LuckyDraw.ImageId,
                    Description = x.LuckyDraw.Description,
                    StatusId = x.LuckyDraw.StatusId,
                    Used = x.LuckyDraw.Used,
                },
                LuckyDrawRegistration = x.LuckyDrawRegistration == null ? null : new LuckyDrawRegistration
                {
                    Id = x.LuckyDrawRegistration.Id,
                    LuckyDrawId = x.LuckyDrawRegistration.LuckyDrawId,
                    AppUserId = x.LuckyDrawRegistration.AppUserId,
                    StoreId = x.LuckyDrawRegistration.StoreId,
                    Revenue = x.LuckyDrawRegistration.Revenue,
                    TurnCounter = x.LuckyDrawRegistration.TurnCounter,
                    IsDrawnByStore = x.LuckyDrawRegistration.IsDrawnByStore,
                    Time = x.LuckyDrawRegistration.Time,
                    Store = new Store
                    {
                        Id = x.LuckyDrawRegistration.Store.Id,
                        Code = x.LuckyDrawRegistration.Store.Code,
                        CodeDraft = x.LuckyDrawRegistration.Store.CodeDraft,
                        Name = x.LuckyDrawRegistration.Store.Name
                    },
                    AppUser = new AppUser
                    {
                        Username = x.LuckyDrawRegistration.AppUser.Username,
                        DisplayName = x.LuckyDrawRegistration.AppUser.DisplayName,
                    },
                },
                LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                {
                    Id = x.LuckyDrawStructure.Id,
                    LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                    Name = x.LuckyDrawStructure.Name,
                    Value = x.LuckyDrawStructure.Value,
                    Quantity = x.LuckyDrawStructure.Quantity,
                },
            }).ToListWithNoLockAsync();


            return LuckyDrawWinners;
        }

        public async Task<LuckyDrawWinner> Get(long Id)
        {
            LuckyDrawWinner LuckyDrawWinner = await DataContext.LuckyDrawWinner.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new LuckyDrawWinner()
            {
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                LuckyDrawStructureId = x.LuckyDrawStructureId,
                LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                Time = x.Time,
                LuckyDrawNumberId = x.LuckyDrawNumberId,
                LuckyDraw = x.LuckyDraw == null ? null : new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    Code = x.LuckyDraw.Code,
                    Name = x.LuckyDraw.Name,
                    LuckyDrawTypeId = x.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = x.LuckyDraw.OrganizationId,
                    RevenuePerTurn = x.LuckyDraw.RevenuePerTurn,
                    StartAt = x.LuckyDraw.StartAt,
                    EndAt = x.LuckyDraw.EndAt,
                    AvatarImageId = x.LuckyDraw.AvatarImageId,
                    ImageId = x.LuckyDraw.ImageId,
                    Description = x.LuckyDraw.Description,
                    StatusId = x.LuckyDraw.StatusId,
                    Used = x.LuckyDraw.Used,
                },
                LuckyDrawRegistration = x.LuckyDrawRegistration == null ? null : new LuckyDrawRegistration
                {
                    Id = x.LuckyDrawRegistration.Id,
                    LuckyDrawId = x.LuckyDrawRegistration.LuckyDrawId,
                    AppUserId = x.LuckyDrawRegistration.AppUserId,
                    StoreId = x.LuckyDrawRegistration.StoreId,
                    Revenue = x.LuckyDrawRegistration.Revenue,
                    TurnCounter = x.LuckyDrawRegistration.TurnCounter,
                    IsDrawnByStore = x.LuckyDrawRegistration.IsDrawnByStore,
                    Time = x.LuckyDrawRegistration.Time,
                },
                LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                {
                    Id = x.LuckyDrawStructure.Id,
                    LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                    Name = x.LuckyDrawStructure.Name,
                    Value = x.LuckyDrawStructure.Value,
                    Quantity = x.LuckyDrawStructure.Quantity,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (LuckyDrawWinner == null)
                return null;

            return LuckyDrawWinner;
        }

        public async Task<bool> Create(LuckyDrawWinner LuckyDrawWinner)
        {
            LuckyDrawWinnerDAO LuckyDrawWinnerDAO = new LuckyDrawWinnerDAO();
            LuckyDrawWinnerDAO.Id = LuckyDrawWinner.Id;
            LuckyDrawWinnerDAO.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
            LuckyDrawWinnerDAO.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
            LuckyDrawWinnerDAO.LuckyDrawRegistrationId = LuckyDrawWinner.LuckyDrawRegistrationId;
            LuckyDrawWinnerDAO.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
            LuckyDrawWinnerDAO.Time = LuckyDrawWinner.Time;
            LuckyDrawWinnerDAO.RowId = Guid.NewGuid();
            DataContext.LuckyDrawWinner.Add(LuckyDrawWinnerDAO);
            await DataContext.SaveChangesAsync();
            LuckyDrawWinner.Id = LuckyDrawWinnerDAO.Id;
            return true;
        }

        public async Task<bool> Update(LuckyDrawWinner LuckyDrawWinner)
        {
            LuckyDrawWinnerDAO LuckyDrawWinnerDAO = DataContext.LuckyDrawWinner
                .Where(x => x.Id == LuckyDrawWinner.Id)
                .FirstOrDefault();
            if (LuckyDrawWinnerDAO == null)
                return false;
            LuckyDrawWinnerDAO.Id = LuckyDrawWinner.Id;
            LuckyDrawWinnerDAO.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
            LuckyDrawWinnerDAO.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
            LuckyDrawWinnerDAO.LuckyDrawRegistrationId = LuckyDrawWinner.LuckyDrawRegistrationId;
            LuckyDrawWinnerDAO.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
            LuckyDrawWinnerDAO.Time = LuckyDrawWinner.Time;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(LuckyDrawWinner LuckyDrawWinner)
        {
            await DataContext.LuckyDrawWinner
                .Where(x => x.Id == LuckyDrawWinner.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<LuckyDrawWinner> LuckyDrawWinners)
        {
            IdFilter IdFilter = new IdFilter { In = LuckyDrawWinners.Select(x => x.Id).ToList() };
            List<LuckyDrawWinnerDAO> LuckyDrawWinnerDAOs = new List<LuckyDrawWinnerDAO>();
            List<LuckyDrawWinnerDAO> DbLuckyDrawWinnerDAOs = await DataContext.LuckyDrawWinner
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (LuckyDrawWinner LuckyDrawWinner in LuckyDrawWinners)
            {
                LuckyDrawWinnerDAO LuckyDrawWinnerDAO = DbLuckyDrawWinnerDAOs
                        .Where(x => x.Id == LuckyDrawWinner.Id)
                        .FirstOrDefault();
                if (LuckyDrawWinnerDAO == null)
                {
                    LuckyDrawWinnerDAO = new LuckyDrawWinnerDAO();
                    LuckyDrawWinnerDAO.RowId = Guid.NewGuid();
                    LuckyDrawWinner.RowId = LuckyDrawWinnerDAO.RowId;
                }
                LuckyDrawWinnerDAO.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
                LuckyDrawWinnerDAO.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
                LuckyDrawWinnerDAO.LuckyDrawRegistrationId = LuckyDrawWinner.LuckyDrawRegistrationId;
                LuckyDrawWinnerDAO.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
                LuckyDrawWinnerDAO.Time = LuckyDrawWinner.Time;
                LuckyDrawWinnerDAOs.Add(LuckyDrawWinnerDAO);
            }
            await DataContext.BulkMergeAsync(LuckyDrawWinnerDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<LuckyDrawWinner> LuckyDrawWinners)
        {
            List<long> Ids = LuckyDrawWinners.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DataContext.LuckyDrawWinner
                .Where(x => x.Id, IdFilter)
                .DeleteFromQueryAsync();
            return true;
        }
    }
}
