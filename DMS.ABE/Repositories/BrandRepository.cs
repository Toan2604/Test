using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IBrandRepository
    {
        Task<int> Count(BrandFilter BrandFilter);
        Task<int> CountAll(BrandFilter BrandFilter);
        Task<List<Brand>> List(BrandFilter BrandFilter);
        Task<Brand> Get(long Id);
        Task<bool> BulkMerge(List<Brand> Brands);
    }
    public class BrandRepository : IBrandRepository
    {
        private DataContext DataContext;
        public BrandRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<BrandDAO> DynamicFilter(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.UpdatedAt, filter.UpdateTime);
            return query;
        }

        private IQueryable<BrandDAO> OrFilter(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<BrandDAO> initQuery = query.Where(q => false);
            foreach (BrandFilter BrandFilter in filter.OrFilter)
            {
                IQueryable<BrandDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, BrandFilter.Id);
                queryable = queryable.Where(q => q.Code, BrandFilter.Code);
                queryable = queryable.Where(q => q.Name, BrandFilter.Name);
                queryable = queryable.Where(q => q.Description, BrandFilter.Description);
                queryable = queryable.Where(q => q.StatusId, BrandFilter.StatusId);
                queryable = queryable.Where(q => q.UpdatedAt, BrandFilter.UpdateTime);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<BrandDAO> DynamicOrder(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case BrandOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case BrandOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case BrandOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case BrandOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case BrandOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case BrandOrder.UpdateTime:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case BrandOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case BrandOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case BrandOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case BrandOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case BrandOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case BrandOrder.UpdateTime:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Brand>> DynamicSelect(IQueryable<BrandDAO> query, BrandFilter filter)
        {
            List<Brand> Brands = await query.Select(q => new Brand()
            {
                Id = filter.Selects.Contains(BrandSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(BrandSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(BrandSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(BrandSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(BrandSelect.Status) ? q.StatusId : default(long),
                UpdateTime = filter.Selects.Contains(BrandSelect.UpdateTime) ? q.UpdatedAt : default(DateTime),
                Status = filter.Selects.Contains(BrandSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Used = q.Used,
            }).ToListAsync();
            return Brands;
        }

        public async Task<int> Count(BrandFilter filter)
        {
            IQueryable<BrandDAO> Brands = DataContext.Brand;
            Brands = DynamicFilter(Brands, filter);
            Brands = OrFilter(Brands, filter);
            return await Brands.CountAsync();
        }
        public async Task<int> CountAll(BrandFilter filter)
        {
            IQueryable<BrandDAO> Brands = DataContext.Brand;
            Brands = DynamicFilter(Brands, filter);
            return await Brands.CountAsync();
        }
        public async Task<List<Brand>> List(BrandFilter filter)
        {
            if (filter == null) return new List<Brand>();
            IQueryable<BrandDAO> BrandDAOs = DataContext.Brand.AsNoTracking();
            BrandDAOs = DynamicFilter(BrandDAOs, filter);
            BrandDAOs = OrFilter(BrandDAOs, filter);
            BrandDAOs = DynamicOrder(BrandDAOs, filter);
            List<Brand> Brands = await DynamicSelect(BrandDAOs, filter);
            return Brands;
        }

        public async Task<Brand> Get(long Id)
        {
            Brand Brand = await DataContext.Brand.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Brand()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StatusId = x.StatusId,
                    Used = x.Used,
                    UpdateTime = x.UpdatedAt,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).FirstOrDefaultAsync();

            if (Brand == null)
                return null;

            return Brand;
        }

        public async Task<bool> BulkMerge(List<Brand> Brands)
        {
            List<BrandDAO> BrandDAOs = Brands.Select(x => new BrandDAO
            {
                Code = x.Code,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Used = x.Used,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                RowId = x.RowId,
                StatusId = x.StatusId,
            }).ToList();
            await DataContext.BulkMergeAsync(BrandDAOs);
            return true;
        }
    }
}
