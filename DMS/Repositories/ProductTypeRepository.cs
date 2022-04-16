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
    public interface IProductTypeRepository
    {
        Task<int> Count(ProductTypeFilter ProductTypeFilter);
        Task<int> CountAll(ProductTypeFilter ProductTypeFilter);
        Task<List<ProductType>> List(ProductTypeFilter ProductTypeFilter);
        Task<ProductType> Get(long Id);
        Task<bool> BulkMerge(List<ProductType> ProductTypes);
    }
    public class ProductTypeRepository : IProductTypeRepository
    {
        private DataContext DataContext;
        public ProductTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProductTypeDAO> DynamicFilter(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedTime);
            return query;
        }

        private IQueryable<ProductTypeDAO> OrFilter(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProductTypeDAO> initQuery = query.Where(q => false);
            foreach (ProductTypeFilter ProductTypeFilter in filter.OrFilter)
            {
                IQueryable<ProductTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProductTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, ProductTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, ProductTypeFilter.Name);
                queryable = queryable.Where(q => q.Description, ProductTypeFilter.Description);
                queryable = queryable.Where(q => q.StatusId, ProductTypeFilter.StatusId);
                queryable = queryable.Where(q => q.UpdatedAt, ProductTypeFilter.UpdatedTime);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProductTypeDAO> DynamicOrder(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProductTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProductTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProductTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProductTypeOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case ProductTypeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ProductTypeOrder.UpdatedTime:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProductTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProductTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProductTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProductTypeOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case ProductTypeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ProductTypeOrder.UpdatedTime:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ProductType>> DynamicSelect(IQueryable<ProductTypeDAO> query, ProductTypeFilter filter)
        {
            List<ProductType> ProductTypes = await query.Select(q => new ProductType()
            {
                Id = filter.Selects.Contains(ProductTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProductTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProductTypeSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(ProductTypeSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(ProductTypeSelect.Status) ? q.StatusId : default(long),
                UpdatedAt = filter.Selects.Contains(ProductTypeSelect.UpdatedTime) ? q.UpdatedAt : default(DateTime),
                Status = filter.Selects.Contains(ProductTypeSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Used = q.Used,
            }).ToListWithNoLockAsync();
            return ProductTypes;
        }

        public async Task<int> Count(ProductTypeFilter filter)
        {
            IQueryable<ProductTypeDAO> ProductTypes = DataContext.ProductType;
            ProductTypes = DynamicFilter(ProductTypes, filter);
            ProductTypes = OrFilter(ProductTypes, filter);
            return await ProductTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ProductTypeFilter filter)
        {
            IQueryable<ProductTypeDAO> ProductTypes = DataContext.ProductType;
            ProductTypes = DynamicFilter(ProductTypes, filter);
            return await ProductTypes.CountWithNoLockAsync();
        }

        public async Task<List<ProductType>> List(ProductTypeFilter filter)
        {
            if (filter == null) return new List<ProductType>();
            IQueryable<ProductTypeDAO> ProductTypeDAOs = DataContext.ProductType.AsNoTracking();
            ProductTypeDAOs = DynamicFilter(ProductTypeDAOs, filter);
            ProductTypeDAOs = OrFilter(ProductTypeDAOs, filter);
            ProductTypeDAOs = DynamicOrder(ProductTypeDAOs, filter);
            List<ProductType> ProductTypes = await DynamicSelect(ProductTypeDAOs, filter);
            return ProductTypes;
        }

        public async Task<ProductType> Get(long Id)
        {
            ProductType ProductType = await DataContext.ProductType.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new ProductType()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StatusId = x.StatusId,
                    UpdatedAt = x.UpdatedAt,
                    Used = x.Used,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).FirstOrDefaultWithNoLockAsync();

            if (ProductType == null)
                return null;

            return ProductType;
        }
        public async Task<bool> BulkMerge(List<ProductType> ProductTypes)
        {
            List<ProductTypeDAO> ProductTypeDAOs = new List<ProductTypeDAO>();
            foreach (var ProductType in ProductTypes)
            {
                ProductTypeDAO ProductTypeDAO = new ProductTypeDAO();
                ProductTypeDAO.Id = ProductType.Id;
                ProductTypeDAO.CreatedAt = ProductType.CreatedAt;
                ProductTypeDAO.UpdatedAt = ProductType.UpdatedAt;
                ProductTypeDAO.DeletedAt = ProductType.DeletedAt;
                ProductTypeDAO.Id = ProductType.Id;
                ProductTypeDAO.Code = ProductType.Code;
                ProductTypeDAO.Name = ProductType.Name;
                ProductTypeDAO.StatusId = ProductType.StatusId;
                ProductTypeDAO.Description = ProductType.Description;
                ProductTypeDAO.Used = ProductType.Used;
                ProductTypeDAO.RowId = ProductType.RowId;
                ProductTypeDAOs.Add(ProductTypeDAO);
            }
            await DataContext.BulkMergeAsync(ProductTypeDAOs);
            return true;
        }
    }
}
