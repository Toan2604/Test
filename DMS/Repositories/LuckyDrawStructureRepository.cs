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
    public interface ILuckyDrawStructureRepository
    {
        Task<int> CountAll(LuckyDrawStructureFilter LuckyDrawStructureFilter);
        Task<int> Count(LuckyDrawStructureFilter LuckyDrawStructureFilter);
        Task<List<LuckyDrawStructure>> List(LuckyDrawStructureFilter LuckyDrawStructureFilter);
        Task<List<LuckyDrawStructure>> List(List<long> Ids);
        Task<LuckyDrawStructure> Get(long Id);
        Task<bool> Create(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> Update(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> Delete(LuckyDrawStructure LuckyDrawStructure);
        Task<bool> BulkMerge(List<LuckyDrawStructure> LuckyDrawStructures);
        Task<bool> BulkDelete(List<LuckyDrawStructure> LuckyDrawStructures);
    }
    public class LuckyDrawStructureRepository : ILuckyDrawStructureRepository
    {
        private DataContext DataContext;
        public LuckyDrawStructureRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<LuckyDrawStructureDAO>> DynamicFilter(IQueryable<LuckyDrawStructureDAO> query, LuckyDrawStructureFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Value, filter.Value);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.LuckyDrawId, filter.LuckyDrawId);
            return query;
        }

        private async Task<IQueryable<LuckyDrawStructureDAO>> OrFilter(IQueryable<LuckyDrawStructureDAO> query, LuckyDrawStructureFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyDrawStructureDAO> initQuery = query.Where(q => false);
            foreach (LuckyDrawStructureFilter LuckyDrawStructureFilter in filter.OrFilter)
            {
                IQueryable<LuckyDrawStructureDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyDrawStructureFilter.Id);
                queryable = queryable.Where(q => q.Name, LuckyDrawStructureFilter.Name);
                queryable = queryable.Where(q => q.Value, LuckyDrawStructureFilter.Value);
                queryable = queryable.Where(q => q.Quantity, LuckyDrawStructureFilter.Quantity);
                queryable = queryable.Where(q => q.LuckyDrawId, LuckyDrawStructureFilter.LuckyDrawId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<LuckyDrawStructureDAO> DynamicOrder(IQueryable<LuckyDrawStructureDAO> query, LuckyDrawStructureFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawStructureOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyDrawStructureOrder.LuckyDraw:
                            query = query.OrderBy(q => q.LuckyDrawId);
                            break;
                        case LuckyDrawStructureOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case LuckyDrawStructureOrder.Value:
                            query = query.OrderBy(q => q.Value);
                            break;
                        case LuckyDrawStructureOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawStructureOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyDrawStructureOrder.LuckyDraw:
                            query = query.OrderByDescending(q => q.LuckyDrawId);
                            break;
                        case LuckyDrawStructureOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case LuckyDrawStructureOrder.Value:
                            query = query.OrderByDescending(q => q.Value);
                            break;
                        case LuckyDrawStructureOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyDrawStructure>> DynamicSelect(IQueryable<LuckyDrawStructureDAO> query, LuckyDrawStructureFilter filter)
        {
            List<LuckyDrawStructure> LuckyDrawStructures = query.Select(q => new LuckyDrawStructure()
            {
                Id = filter.Selects.Contains(LuckyDrawStructureSelect.Id) ? q.Id : default(long),
                LuckyDrawId = filter.Selects.Contains(LuckyDrawStructureSelect.LuckyDraw) ? q.LuckyDrawId : default(long),
                Name = filter.Selects.Contains(LuckyDrawStructureSelect.Name) ? q.Name : default(string),
                Value = filter.Selects.Contains(LuckyDrawStructureSelect.Value) ? q.Value : default(string),
                Quantity = filter.Selects.Contains(LuckyDrawStructureSelect.Quantity) ? q.Quantity : default(long),
                LuckyDraw = filter.Selects.Contains(LuckyDrawStructureSelect.LuckyDraw) && q.LuckyDraw != null ? new LuckyDraw
                {
                    Id = q.LuckyDraw.Id,
                    Code = q.LuckyDraw.Code,
                    Name = q.LuckyDraw.Name,
                    LuckyDrawTypeId = q.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = q.LuckyDraw.OrganizationId,
                    RevenuePerTurn = q.LuckyDraw.RevenuePerTurn,
                    StartAt = q.LuckyDraw.StartAt,
                    EndAt = q.LuckyDraw.EndAt,
                    ImageId = q.LuckyDraw.ImageId,
                    StatusId = q.LuckyDraw.StatusId,
                    Used = q.LuckyDraw.Used,
                } : null,
            }).ToList();
            return LuckyDrawStructures;
        }

        public async Task<int> CountAll(LuckyDrawStructureFilter filter)
        {
            IQueryable<LuckyDrawStructureDAO> LuckyDrawStructureDAOs = DataContext.LuckyDrawStructure.AsNoTracking();
            LuckyDrawStructureDAOs = await DynamicFilter(LuckyDrawStructureDAOs, filter);
            return LuckyDrawStructureDAOs.Count();
        }

        public async Task<int> Count(LuckyDrawStructureFilter filter)
        {
            IQueryable<LuckyDrawStructureDAO> LuckyDrawStructureDAOs = DataContext.LuckyDrawStructure.AsNoTracking();
            LuckyDrawStructureDAOs = await DynamicFilter(LuckyDrawStructureDAOs, filter);
            LuckyDrawStructureDAOs = await OrFilter(LuckyDrawStructureDAOs, filter);
            return LuckyDrawStructureDAOs.Count();
        }

        public async Task<List<LuckyDrawStructure>> List(LuckyDrawStructureFilter filter)
        {
            if (filter == null) return new List<LuckyDrawStructure>();
            IQueryable<LuckyDrawStructureDAO> LuckyDrawStructureDAOs = DataContext.LuckyDrawStructure.AsNoTracking();
            LuckyDrawStructureDAOs = await DynamicFilter(LuckyDrawStructureDAOs, filter);
            LuckyDrawStructureDAOs = await OrFilter(LuckyDrawStructureDAOs, filter);
            LuckyDrawStructureDAOs = DynamicOrder(LuckyDrawStructureDAOs, filter);
            List<LuckyDrawStructure> LuckyDrawStructures = await DynamicSelect(LuckyDrawStructureDAOs, filter);
            return LuckyDrawStructures;
        }

        public async Task<List<LuckyDrawStructure>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<LuckyDrawStructureDAO> query = DataContext.LuckyDrawStructure.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<LuckyDrawStructure> LuckyDrawStructures = query.AsNoTracking()
            .Select(x => new LuckyDrawStructure()
            {
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                Name = x.Name,
                Value = x.Value,
                Quantity = x.Quantity,
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
                    ImageId = x.LuckyDraw.ImageId,
                    StatusId = x.LuckyDraw.StatusId,
                    Used = x.LuckyDraw.Used,
                },
            }).ToList();
            

            return LuckyDrawStructures;
        }

        public async Task<LuckyDrawStructure> Get(long Id)
        {
            LuckyDrawStructure LuckyDrawStructure = DataContext.LuckyDrawStructure.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new LuckyDrawStructure()
            {
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                Name = x.Name,
                Value = x.Value,
                Quantity = x.Quantity,
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
                    ImageId = x.LuckyDraw.ImageId,
                    StatusId = x.LuckyDraw.StatusId,
                    Used = x.LuckyDraw.Used,
                },
            }).FirstOrDefault();

            if (LuckyDrawStructure == null)
                return null;

            return LuckyDrawStructure;
        }
        
        public async Task<bool> Create(LuckyDrawStructure LuckyDrawStructure)
        {
            LuckyDrawStructureDAO LuckyDrawStructureDAO = new LuckyDrawStructureDAO();
            LuckyDrawStructureDAO.Id = LuckyDrawStructure.Id;
            LuckyDrawStructureDAO.LuckyDrawId = LuckyDrawStructure.LuckyDrawId;
            LuckyDrawStructureDAO.Name = LuckyDrawStructure.Name;
            LuckyDrawStructureDAO.Value = LuckyDrawStructure.Value;
            LuckyDrawStructureDAO.Quantity = LuckyDrawStructure.Quantity;
            DataContext.LuckyDrawStructure.Add(LuckyDrawStructureDAO);
            DataContext.SaveChanges();
            LuckyDrawStructure.Id = LuckyDrawStructureDAO.Id;
            await SaveReference(LuckyDrawStructure);
            return true;
        }

        public async Task<bool> Update(LuckyDrawStructure LuckyDrawStructure)
        {
            LuckyDrawStructureDAO LuckyDrawStructureDAO = DataContext.LuckyDrawStructure
                .Where(x => x.Id == LuckyDrawStructure.Id)
                .FirstOrDefault();
            if (LuckyDrawStructureDAO == null)
                return false;
            LuckyDrawStructureDAO.Id = LuckyDrawStructure.Id;
            LuckyDrawStructureDAO.LuckyDrawId = LuckyDrawStructure.LuckyDrawId;
            LuckyDrawStructureDAO.Name = LuckyDrawStructure.Name;
            LuckyDrawStructureDAO.Value = LuckyDrawStructure.Value;
            LuckyDrawStructureDAO.Quantity = LuckyDrawStructure.Quantity;
            DataContext.SaveChanges();
            await SaveReference(LuckyDrawStructure);
            return true;
        }

        public async Task<bool> Delete(LuckyDrawStructure LuckyDrawStructure)
        {
            DataContext.LuckyDrawStructure
                .Where(x => x.Id == LuckyDrawStructure.Id)
                .DeleteFromQuery();
            return true;
        }
        
        public async Task<bool> BulkMerge(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            IdFilter IdFilter = new IdFilter { In = LuckyDrawStructures.Select(x => x.Id).ToList() };
            List<LuckyDrawStructureDAO> LuckyDrawStructureDAOs = new List<LuckyDrawStructureDAO>();
            List<LuckyDrawStructureDAO> DbLuckyDrawStructureDAOs = DataContext.LuckyDrawStructure
                .Where(x => x.Id, IdFilter)
                .ToList();
            foreach (LuckyDrawStructure LuckyDrawStructure in LuckyDrawStructures)
            {
                LuckyDrawStructureDAO LuckyDrawStructureDAO = DbLuckyDrawStructureDAOs
                        .Where(x => x.Id == LuckyDrawStructure.Id)
                        .FirstOrDefault();
                if (LuckyDrawStructureDAO == null)
                {
                    LuckyDrawStructureDAO = new LuckyDrawStructureDAO();
                }
                LuckyDrawStructureDAO.LuckyDrawId = LuckyDrawStructure.LuckyDrawId;
                LuckyDrawStructureDAO.Name = LuckyDrawStructure.Name;
                LuckyDrawStructureDAO.Value = LuckyDrawStructure.Value;
                LuckyDrawStructureDAO.Quantity = LuckyDrawStructure.Quantity;
                LuckyDrawStructureDAOs.Add(LuckyDrawStructureDAO);
            }
            DataContext.BulkMerge(LuckyDrawStructureDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            List<long> Ids = LuckyDrawStructures.Select(x => x.Id).ToList();
            DataContext.LuckyDrawNumber.Where(x => Ids.Contains(x.LuckyDrawStructureId)).DeleteFromQuery();
            DataContext.LuckyDrawStructure
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQuery();
            return true;
        }

        private async Task SaveReference(LuckyDrawStructure LuckyDrawStructure)
        {
        }
        
    }
}
