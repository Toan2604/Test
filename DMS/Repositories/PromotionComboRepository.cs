using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IPromotionComboRepository
    {
        Task<int> Count(PromotionComboFilter PromotionComboFilter);
        Task<int> CountAll(PromotionComboFilter PromotionComboFilter);
        Task<List<PromotionCombo>> List(PromotionComboFilter PromotionComboFilter);
        Task<PromotionCombo> Get(long Id);
        Task<bool> Create(PromotionCombo PromotionCombo);
        Task<bool> Update(PromotionCombo PromotionCombo);
        Task<bool> Delete(PromotionCombo PromotionCombo);
        Task<bool> BulkMerge(List<PromotionCombo> PromotionCombos);
        Task<bool> BulkDelete(List<PromotionCombo> PromotionCombos);
    }
    public class PromotionComboRepository : IPromotionComboRepository
    {
        private DataContext DataContext;
        public PromotionComboRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionComboDAO> DynamicFilter(IQueryable<PromotionComboDAO> query, PromotionComboFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.PromotionPolicyId, filter.PromotionPolicyId);
            query = query.Where(q => q.PromotionId, filter.PromotionId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.PromotionDiscountTypeId, filter.PromotionDiscountTypeId);
            query = query.Where(q => q.DiscountPercentage, filter.DiscountPercentage);
            query = query.Where(q => q.DiscountValue, filter.DiscountValue);
            query = query.Where(q => q.Price, filter.Price);
            return query;
        }

        private IQueryable<PromotionComboDAO> OrFilter(IQueryable<PromotionComboDAO> query, PromotionComboFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionComboDAO> initQuery = query.Where(q => false);
            foreach (PromotionComboFilter PromotionComboFilter in filter.OrFilter)
            {
                IQueryable<PromotionComboDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionComboFilter.Id);
                queryable = queryable.Where(q => q.Name, PromotionComboFilter.Name);
                queryable = queryable.Where(q => q.PromotionPolicyId, PromotionComboFilter.PromotionPolicyId);
                queryable = queryable.Where(q => q.PromotionId, PromotionComboFilter.PromotionId);
                queryable = queryable.Where(q => q.Note, PromotionComboFilter.Note);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionComboFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.DiscountPercentage, PromotionComboFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountValue, PromotionComboFilter.DiscountValue);
                queryable = queryable.Where(q => q.Price, PromotionComboFilter.Price);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionComboDAO> DynamicOrder(IQueryable<PromotionComboDAO> query, PromotionComboFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionComboOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionComboOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case PromotionComboOrder.PromotionPolicy:
                            query = query.OrderBy(q => q.PromotionPolicyId);
                            break;
                        case PromotionComboOrder.Promotion:
                            query = query.OrderBy(q => q.PromotionId);
                            break;
                        case PromotionComboOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionComboOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionComboOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case PromotionComboOrder.DiscountValue:
                            query = query.OrderBy(q => q.DiscountValue);
                            break;
                        case PromotionComboOrder.Price:
                            query = query.OrderBy(q => q.Price);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionComboOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionComboOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case PromotionComboOrder.PromotionPolicy:
                            query = query.OrderByDescending(q => q.PromotionPolicyId);
                            break;
                        case PromotionComboOrder.Promotion:
                            query = query.OrderByDescending(q => q.PromotionId);
                            break;
                        case PromotionComboOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionComboOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionComboOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case PromotionComboOrder.DiscountValue:
                            query = query.OrderByDescending(q => q.DiscountValue);
                            break;
                        case PromotionComboOrder.Price:
                            query = query.OrderByDescending(q => q.Price);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionCombo>> DynamicSelect(IQueryable<PromotionComboDAO> query, PromotionComboFilter filter)
        {
            List<PromotionCombo> PromotionCombos = await query.Select(q => new PromotionCombo()
            {
                Id = filter.Selects.Contains(PromotionComboSelect.Id) ? q.Id : default(long),
                Name = filter.Selects.Contains(PromotionComboSelect.Name) ? q.Name : default(string),
                PromotionPolicyId = filter.Selects.Contains(PromotionComboSelect.PromotionPolicy) ? q.PromotionPolicyId : default(long),
                PromotionId = filter.Selects.Contains(PromotionComboSelect.Promotion) ? q.PromotionId : default(long),
                Note = filter.Selects.Contains(PromotionComboSelect.Note) ? q.Note : default(string),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionComboSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                DiscountPercentage = filter.Selects.Contains(PromotionComboSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountValue = filter.Selects.Contains(PromotionComboSelect.DiscountValue) ? q.DiscountValue : default(decimal?),
                Price = filter.Selects.Contains(PromotionComboSelect.Price) ? q.Price : default(decimal?),
                Promotion = filter.Selects.Contains(PromotionComboSelect.Promotion) && q.Promotion != null ? new Promotion
                {
                    Id = q.Promotion.Id,
                    Code = q.Promotion.Code,
                    Name = q.Promotion.Name,
                    StartDate = q.Promotion.StartDate,
                    EndDate = q.Promotion.EndDate,
                    OrganizationId = q.Promotion.OrganizationId,
                    PromotionTypeId = q.Promotion.PromotionTypeId,
                    Note = q.Promotion.Note,
                    Priority = q.Promotion.Priority,
                    StatusId = q.Promotion.StatusId,
                } : null,
                PromotionDiscountType = filter.Selects.Contains(PromotionComboSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionPolicy = filter.Selects.Contains(PromotionComboSelect.PromotionPolicy) && q.PromotionPolicy != null ? new PromotionPolicy
                {
                    Id = q.PromotionPolicy.Id,
                    Code = q.PromotionPolicy.Code,
                    Name = q.PromotionPolicy.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionCombos;
        }

        public async Task<int> Count(PromotionComboFilter filter)
        {
            IQueryable<PromotionComboDAO> PromotionCombos = DataContext.PromotionCombo.AsNoTracking();
            PromotionCombos = DynamicFilter(PromotionCombos, filter);
            PromotionCombos = OrFilter(PromotionCombos, filter);
            return await PromotionCombos.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionComboFilter filter)
        {
            IQueryable<PromotionComboDAO> PromotionCombos = DataContext.PromotionCombo.AsNoTracking();
            PromotionCombos = DynamicFilter(PromotionCombos, filter);
            return await PromotionCombos.CountWithNoLockAsync();
        }

        public async Task<List<PromotionCombo>> List(PromotionComboFilter filter)
        {
            if (filter == null) return new List<PromotionCombo>();
            IQueryable<PromotionComboDAO> PromotionComboDAOs = DataContext.PromotionCombo.AsNoTracking();
            PromotionComboDAOs = DynamicFilter(PromotionComboDAOs, filter);
            PromotionComboDAOs = OrFilter(PromotionComboDAOs, filter);
            PromotionComboDAOs = DynamicOrder(PromotionComboDAOs, filter);
            List<PromotionCombo> PromotionCombos = await DynamicSelect(PromotionComboDAOs, filter);
            return PromotionCombos;
        }

        public async Task<PromotionCombo> Get(long Id)
        {
            PromotionCombo PromotionCombo = await DataContext.PromotionCombo.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionCombo()
            {
                Id = x.Id,
                Name = x.Name,
                PromotionPolicyId = x.PromotionPolicyId,
                PromotionId = x.PromotionId,
                Note = x.Note,
                PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                DiscountPercentage = x.DiscountPercentage,
                DiscountValue = x.DiscountValue,
                Price = x.Price,
                Promotion = x.Promotion == null ? null : new Promotion
                {
                    Id = x.Promotion.Id,
                    Code = x.Promotion.Code,
                    Name = x.Promotion.Name,
                    StartDate = x.Promotion.StartDate,
                    EndDate = x.Promotion.EndDate,
                    OrganizationId = x.Promotion.OrganizationId,
                    PromotionTypeId = x.Promotion.PromotionTypeId,
                    Note = x.Promotion.Note,
                    Priority = x.Promotion.Priority,
                    StatusId = x.Promotion.StatusId,
                },
                PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                {
                    Id = x.PromotionDiscountType.Id,
                    Code = x.PromotionDiscountType.Code,
                    Name = x.PromotionDiscountType.Name,
                },
                PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                {
                    Id = x.PromotionPolicy.Id,
                    Code = x.PromotionPolicy.Code,
                    Name = x.PromotionPolicy.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionCombo == null)
                return null;

            return PromotionCombo;
        }
        public async Task<bool> Create(PromotionCombo PromotionCombo)
        {
            PromotionComboDAO PromotionComboDAO = new PromotionComboDAO();
            PromotionComboDAO.Id = PromotionCombo.Id;
            PromotionComboDAO.Name = PromotionCombo.Name;
            PromotionComboDAO.PromotionPolicyId = PromotionCombo.PromotionPolicyId;
            PromotionComboDAO.PromotionId = PromotionCombo.PromotionId;
            PromotionComboDAO.Note = PromotionCombo.Note;
            PromotionComboDAO.PromotionDiscountTypeId = PromotionCombo.PromotionDiscountTypeId;
            PromotionComboDAO.DiscountPercentage = PromotionCombo.DiscountPercentage;
            PromotionComboDAO.DiscountValue = PromotionCombo.DiscountValue;
            PromotionComboDAO.Price = PromotionCombo.Price;
            DataContext.PromotionCombo.Add(PromotionComboDAO);
            await DataContext.SaveChangesAsync();
            PromotionCombo.Id = PromotionComboDAO.Id;
            await SaveReference(PromotionCombo);
            return true;
        }

        public async Task<bool> Update(PromotionCombo PromotionCombo)
        {
            PromotionComboDAO PromotionComboDAO = DataContext.PromotionCombo.Where(x => x.Id == PromotionCombo.Id).FirstOrDefault();
            if (PromotionComboDAO == null)
                return false;
            PromotionComboDAO.Id = PromotionCombo.Id;
            PromotionComboDAO.Name = PromotionCombo.Name;
            PromotionComboDAO.PromotionPolicyId = PromotionCombo.PromotionPolicyId;
            PromotionComboDAO.PromotionId = PromotionCombo.PromotionId;
            PromotionComboDAO.Note = PromotionCombo.Note;
            PromotionComboDAO.PromotionDiscountTypeId = PromotionCombo.PromotionDiscountTypeId;
            PromotionComboDAO.DiscountPercentage = PromotionCombo.DiscountPercentage;
            PromotionComboDAO.DiscountValue = PromotionCombo.DiscountValue;
            PromotionComboDAO.Price = PromotionCombo.Price;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionCombo);
            return true;
        }

        public async Task<bool> Delete(PromotionCombo PromotionCombo)
        {
            await DataContext.PromotionCombo.Where(x => x.Id == PromotionCombo.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionCombo> PromotionCombos)
        {
            List<PromotionComboDAO> PromotionComboDAOs = new List<PromotionComboDAO>();
            foreach (PromotionCombo PromotionCombo in PromotionCombos)
            {
                PromotionComboDAO PromotionComboDAO = new PromotionComboDAO();
                PromotionComboDAO.Id = PromotionCombo.Id;
                PromotionComboDAO.Name = PromotionCombo.Name;
                PromotionComboDAO.PromotionPolicyId = PromotionCombo.PromotionPolicyId;
                PromotionComboDAO.PromotionId = PromotionCombo.PromotionId;
                PromotionComboDAO.Note = PromotionCombo.Note;
                PromotionComboDAO.PromotionDiscountTypeId = PromotionCombo.PromotionDiscountTypeId;
                PromotionComboDAO.DiscountPercentage = PromotionCombo.DiscountPercentage;
                PromotionComboDAO.DiscountValue = PromotionCombo.DiscountValue;
                PromotionComboDAO.Price = PromotionCombo.Price;
                PromotionComboDAOs.Add(PromotionComboDAO);
            }
            await DataContext.BulkMergeAsync(PromotionComboDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionCombo> PromotionCombos)
        {
            List<long> Ids = PromotionCombos.Select(x => x.Id).ToList();
            await DataContext.PromotionCombo
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionCombo PromotionCombo)
        {
        }

    }
}
