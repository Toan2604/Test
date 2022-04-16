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
    public interface IShowingItemRepository
    {
        Task<int> Count(ShowingItemFilter ShowingItemFilter);
        Task<int> CountAll(ShowingItemFilter ShowingItemFilter);
        Task<List<ShowingItem>> List(ShowingItemFilter ShowingItemFilter);
        Task<List<ShowingItem>> List(List<long> Ids);
        Task<ShowingItem> Get(long Id);
        Task<bool> Create(ShowingItem ShowingItem);
        Task<bool> Update(ShowingItem ShowingItem);
        Task<bool> Delete(ShowingItem ShowingItem);
        Task<bool> BulkMerge(List<ShowingItem> ShowingItems);
        Task<bool> BulkDelete(List<ShowingItem> ShowingItems);
        Task<bool> Used(List<long> Ids);
    }
    public class ShowingItemRepository : IShowingItemRepository
    {
        private DataContext DataContext;
        public ShowingItemRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ShowingItemDAO> DynamicFilter(IQueryable<ShowingItemDAO> query, ShowingItemFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.ShowingCategoryId, filter.ShowingCategoryId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.SalePrice, filter.SalePrice);
            query = query.Where(q => q.ERPCode, filter.ERPCode);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.RowId, filter.RowId);
            if (filter.Search != null)
            {
                List<string> Tokens = filter.Search.Split(" ").Select(x => x.ToLower()).ToList();
                var queryForCode = query;
                var queryForName = query;
                foreach (string Token in Tokens)
                {
                    if (string.IsNullOrWhiteSpace(Token))
                        continue;
                    queryForCode = queryForCode.Where(x => x.Code.ToLower().Contains(Token));
                    queryForName = queryForName.Where(x => x.Name.ToLower().Contains(Token));
                }
                query = queryForCode.Union(queryForName);
                query = query.Distinct();
            }
            return query;
        }

        private IQueryable<ShowingItemDAO> OrFilter(IQueryable<ShowingItemDAO> query, ShowingItemFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ShowingItemDAO> initQuery = query.Where(q => false);
            foreach (ShowingItemFilter ShowingItemFilter in filter.OrFilter)
            {
                IQueryable<ShowingItemDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                queryable = queryable.Where(q => q.ShowingCategoryId, filter.ShowingCategoryId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.SalePrice, filter.SalePrice);
                queryable = queryable.Where(q => q.ERPCode, filter.ERPCode);
                queryable = queryable.Where(q => q.Description, filter.Description);
                queryable = queryable.Where(q => q.StatusId, filter.StatusId);
                queryable = queryable.Where(q => q.RowId, filter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ShowingItemDAO> DynamicOrder(IQueryable<ShowingItemDAO> query, ShowingItemFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ShowingItemOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ShowingItemOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ShowingItemOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ShowingItemOrder.ShowingCategory:
                            query = query.OrderBy(q => q.ShowingCategoryId);
                            break;
                        case ShowingItemOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case ShowingItemOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case ShowingItemOrder.ERPCode:
                            query = query.OrderBy(q => q.ERPCode);
                            break;
                        case ShowingItemOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case ShowingItemOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ShowingItemOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                        case ShowingItemOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ShowingItemOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ShowingItemOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ShowingItemOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ShowingItemOrder.ShowingCategory:
                            query = query.OrderByDescending(q => q.ShowingCategoryId);
                            break;
                        case ShowingItemOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case ShowingItemOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case ShowingItemOrder.ERPCode:
                            query = query.OrderByDescending(q => q.ERPCode);
                            break;
                        case ShowingItemOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case ShowingItemOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ShowingItemOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                        case ShowingItemOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ShowingItem>> DynamicSelect(IQueryable<ShowingItemDAO> query, ShowingItemFilter filter)
        {
            List<ShowingItem> ShowingItems = await query.Select(q => new ShowingItem()
            {
                Id = filter.Selects.Contains(ShowingItemSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ShowingItemSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ShowingItemSelect.Name) ? q.Name : default(string),
                ShowingCategoryId = filter.Selects.Contains(ShowingItemSelect.ShowingCategory) ? q.ShowingCategoryId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(ShowingItemSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                SalePrice = filter.Selects.Contains(ShowingItemSelect.SalePrice) ? q.SalePrice : default(decimal),
                ERPCode = filter.Selects.Contains(ShowingItemSelect.ERPCode) ? q.ERPCode : default(string),
                Description = filter.Selects.Contains(ShowingItemSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(ShowingItemSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(ShowingItemSelect.Used) ? q.Used : default(bool),
                RowId = filter.Selects.Contains(ShowingItemSelect.Row) ? q.RowId : default(Guid),
                ShowingCategory = filter.Selects.Contains(ShowingItemSelect.ShowingCategory) && q.ShowingCategory != null ? new ShowingCategory
                {
                    Id = q.ShowingCategory.Id,
                    Code = q.ShowingCategory.Code,
                    Name = q.ShowingCategory.Name,
                    ParentId = q.ShowingCategory.ParentId,
                    Path = q.ShowingCategory.Path,
                    Level = q.ShowingCategory.Level,
                    StatusId = q.ShowingCategory.StatusId,
                    ImageId = q.ShowingCategory.ImageId,
                    RowId = q.ShowingCategory.RowId,
                    Used = q.ShowingCategory.Used,
                } : null,
                Status = filter.Selects.Contains(ShowingItemSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(ShowingItemSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                    Used = q.UnitOfMeasure.Used,
                    RowId = q.UnitOfMeasure.RowId,
                } : null,
            }).ToListWithNoLockAsync();

            var Ids = ShowingItems.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<ShowingItemImageMappingDAO> ShowingItemImageMappings = await DataContext.ShowingItemImageMapping.Include(x => x.Image)
                .Where(x => x.ShowingItemId, IdFilter).ToListWithNoLockAsync();
            foreach (var ShowingItem in ShowingItems)
            {
                ShowingItem.ShowingItemImageMappings = new List<ShowingItemImageMapping>();
                var ShowingItemImageMappingDAO = ShowingItemImageMappings.Where(x => x.ShowingItemId == ShowingItem.Id).FirstOrDefault();
                if (ShowingItemImageMappingDAO != null)
                {
                    ShowingItemImageMapping ShowingItemImageMapping = new ShowingItemImageMapping
                    {
                        ImageId = ShowingItemImageMappingDAO.ImageId,
                        ShowingItemId = ShowingItemImageMappingDAO.ShowingItemId,
                        Image = ShowingItemImageMappingDAO.Image == null ? null : new Image
                        {
                            Id = ShowingItemImageMappingDAO.Image.Id,
                            Name = ShowingItemImageMappingDAO.Image.Name,
                            Url = ShowingItemImageMappingDAO.Image.Url,
                            ThumbnailUrl = ShowingItemImageMappingDAO.Image.ThumbnailUrl
                        }
                    };
                    ShowingItem.ShowingItemImageMappings.Add(ShowingItemImageMapping);
                }
            }
            return ShowingItems;
        }

        public async Task<int> Count(ShowingItemFilter filter)
        {
            IQueryable<ShowingItemDAO> ShowingItems = DataContext.ShowingItem.AsNoTracking();
            ShowingItems = DynamicFilter(ShowingItems, filter);
            ShowingItems = OrFilter(ShowingItems, filter);
            return await ShowingItems.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ShowingItemFilter filter)
        {
            IQueryable<ShowingItemDAO> ShowingItems = DataContext.ShowingItem.AsNoTracking();
            ShowingItems = DynamicFilter(ShowingItems, filter);
            return await ShowingItems.CountWithNoLockAsync();
        }

        public async Task<List<ShowingItem>> List(ShowingItemFilter filter)
        {
            if (filter == null) return new List<ShowingItem>();
            IQueryable<ShowingItemDAO> ShowingItemDAOs = DataContext.ShowingItem.AsNoTracking();
            ShowingItemDAOs = DynamicFilter(ShowingItemDAOs, filter);
            ShowingItemDAOs = OrFilter(ShowingItemDAOs, filter);
            ShowingItemDAOs = DynamicOrder(ShowingItemDAOs, filter);
            List<ShowingItem> ShowingItems = await DynamicSelect(ShowingItemDAOs, filter);
            return ShowingItems;
        }

        public async Task<List<ShowingItem>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
            .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.ShowingItem
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;

            List<ShowingItem> ShowingItems = await query.AsNoTracking()
           .Select(x => new ShowingItem()
           {
               CreatedAt = x.CreatedAt,
               UpdatedAt = x.UpdatedAt,
               DeletedAt = x.DeletedAt,
               Id = x.Id,
               Code = x.Code,
               Name = x.Name,
               ShowingCategoryId = x.ShowingCategoryId,
               UnitOfMeasureId = x.UnitOfMeasureId,
               SalePrice = x.SalePrice,
               ERPCode = x.ERPCode,
               Description = x.Description,
               StatusId = x.StatusId,
               Used = x.Used,
               RowId = x.RowId,
               ShowingCategory = x.ShowingCategory == null ? null : new ShowingCategory
               {
                   Id = x.ShowingCategory.Id,
                   Code = x.ShowingCategory.Code,
                   Name = x.ShowingCategory.Name,
                   ParentId = x.ShowingCategory.ParentId,
                   Path = x.ShowingCategory.Path,
                   Level = x.ShowingCategory.Level,
                   StatusId = x.ShowingCategory.StatusId,
                   ImageId = x.ShowingCategory.ImageId,
                   RowId = x.ShowingCategory.RowId,
                   Used = x.ShowingCategory.Used,
               },
               Status = x.Status == null ? null : new Status
               {
                   Id = x.Status.Id,
                   Code = x.Status.Code,
                   Name = x.Status.Name,
               },
               UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
               {
                   Id = x.UnitOfMeasure.Id,
                   Code = x.UnitOfMeasure.Code,
                   Name = x.UnitOfMeasure.Name,
                   Description = x.UnitOfMeasure.Description,
                   StatusId = x.UnitOfMeasure.StatusId,
                   Used = x.UnitOfMeasure.Used,
                   RowId = x.UnitOfMeasure.RowId,
               },
           }).ToListWithNoLockAsync();


            return ShowingItems;
        }

        public async Task<ShowingItem> Get(long Id)
        {
            ShowingItem ShowingItem = await DataContext.ShowingItem.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new ShowingItem()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                ShowingCategoryId = x.ShowingCategoryId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                SalePrice = x.SalePrice,
                ERPCode = x.ERPCode,
                Description = x.Description,
                StatusId = x.StatusId,
                Used = x.Used,
                RowId = x.RowId,
                ShowingCategory = x.ShowingCategory == null ? null : new ShowingCategory
                {
                    Id = x.ShowingCategory.Id,
                    Code = x.ShowingCategory.Code,
                    Name = x.ShowingCategory.Name,
                    ParentId = x.ShowingCategory.ParentId,
                    Path = x.ShowingCategory.Path,
                    Level = x.ShowingCategory.Level,
                    StatusId = x.ShowingCategory.StatusId,
                    ImageId = x.ShowingCategory.ImageId,
                    RowId = x.ShowingCategory.RowId,
                    Used = x.ShowingCategory.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    Used = x.UnitOfMeasure.Used,
                    RowId = x.UnitOfMeasure.RowId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ShowingItem == null)
                return null;

            ShowingItem.ShowingItemImageMappings = await DataContext.ShowingItemImageMapping.AsNoTracking()
              .Where(x => x.ShowingItemId == ShowingItem.Id)
              .Select(x => new ShowingItemImageMapping
              {
                  ShowingItemId = x.ShowingItemId,
                  ImageId = x.ImageId,
                  Image = new Image
                  {
                      Id = x.Image.Id,
                      Name = x.Image.Name,
                      Url = x.Image.Url,
                      ThumbnailUrl = x.Image.ThumbnailUrl,
                  },
              }).ToListWithNoLockAsync();
            return ShowingItem;
        }
        public async Task<bool> Create(ShowingItem ShowingItem)
        {
            ShowingItemDAO ShowingItemDAO = new ShowingItemDAO();
            ShowingItemDAO.Id = ShowingItem.Id;
            ShowingItemDAO.Code = ShowingItem.Code;
            ShowingItemDAO.Name = ShowingItem.Name;
            ShowingItemDAO.ShowingCategoryId = ShowingItem.ShowingCategoryId;
            ShowingItemDAO.UnitOfMeasureId = ShowingItem.UnitOfMeasureId;
            ShowingItemDAO.SalePrice = ShowingItem.SalePrice;
            ShowingItemDAO.ERPCode = ShowingItem.ERPCode;
            ShowingItemDAO.Description = ShowingItem.Description;
            ShowingItemDAO.StatusId = ShowingItem.StatusId;
            ShowingItemDAO.Used = ShowingItem.Used;
            ShowingItemDAO.RowId = ShowingItem.RowId;
            ShowingItemDAO.CreatedAt = StaticParams.DateTimeNow;
            ShowingItemDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ShowingItem.Add(ShowingItemDAO);
            await DataContext.SaveChangesAsync();
            ShowingItem.Id = ShowingItemDAO.Id;
            await SaveReference(ShowingItem);
            return true;
        }

        public async Task<bool> Update(ShowingItem ShowingItem)
        {
            ShowingItemDAO ShowingItemDAO = DataContext.ShowingItem.Where(x => x.Id == ShowingItem.Id).FirstOrDefault();
            if (ShowingItemDAO == null)
                return false;
            ShowingItemDAO.Id = ShowingItem.Id;
            ShowingItemDAO.Code = ShowingItem.Code;
            ShowingItemDAO.Name = ShowingItem.Name;
            ShowingItemDAO.ShowingCategoryId = ShowingItem.ShowingCategoryId;
            ShowingItemDAO.UnitOfMeasureId = ShowingItem.UnitOfMeasureId;
            ShowingItemDAO.SalePrice = ShowingItem.SalePrice;
            ShowingItemDAO.ERPCode = ShowingItem.ERPCode;
            ShowingItemDAO.Description = ShowingItem.Description;
            ShowingItemDAO.StatusId = ShowingItem.StatusId;
            ShowingItemDAO.Used = ShowingItem.Used;
            ShowingItemDAO.RowId = ShowingItem.RowId;
            ShowingItemDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(ShowingItem);
            return true;
        }

        public async Task<bool> Delete(ShowingItem ShowingItem)
        {
            await DataContext.ShowingItem.Where(x => x.Id == ShowingItem.Id).UpdateFromQueryAsync(x => new ShowingItemDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<ShowingItem> ShowingItems)
        {
            List<ShowingItemDAO> ShowingItemDAOs = new List<ShowingItemDAO>();
            foreach (ShowingItem ShowingItem in ShowingItems)
            {
                ShowingItemDAO ShowingItemDAO = new ShowingItemDAO();
                ShowingItemDAO.Id = ShowingItem.Id;
                ShowingItemDAO.Code = ShowingItem.Code;
                ShowingItemDAO.Name = ShowingItem.Name;
                ShowingItemDAO.ShowingCategoryId = ShowingItem.ShowingCategoryId;
                ShowingItemDAO.UnitOfMeasureId = ShowingItem.UnitOfMeasureId;
                ShowingItemDAO.SalePrice = ShowingItem.SalePrice;
                ShowingItemDAO.ERPCode = ShowingItem.ERPCode;
                ShowingItemDAO.Description = ShowingItem.Description;
                ShowingItemDAO.StatusId = ShowingItem.StatusId;
                ShowingItemDAO.Used = ShowingItem.Used;
                ShowingItemDAO.RowId = ShowingItem.RowId;
                ShowingItemDAO.CreatedAt = StaticParams.DateTimeNow;
                ShowingItemDAO.UpdatedAt = StaticParams.DateTimeNow;
                ShowingItemDAOs.Add(ShowingItemDAO);
            }
            await DataContext.BulkMergeAsync(ShowingItemDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ShowingItem> ShowingItems)
        {
            List<long> Ids = ShowingItems.Select(x => x.Id).ToList();
            await DataContext.ShowingItem
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ShowingItemDAO
                {
                    DeletedAt = StaticParams.DateTimeNow,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

        private async Task SaveReference(ShowingItem ShowingItem)
        {
            await DataContext.ShowingItemImageMapping
               .Where(x => x.ShowingItemId == ShowingItem.Id)
               .DeleteFromQueryAsync();
            List<ShowingItemImageMappingDAO> ShowingItemImageMappingDAOs = new List<ShowingItemImageMappingDAO>();
            if (ShowingItem.ShowingItemImageMappings != null)
            {
                foreach (ShowingItemImageMapping ShowingItemImageMapping in ShowingItem.ShowingItemImageMappings)
                {
                    ShowingItemImageMappingDAO ShowingItemImageMappingDAO = new ShowingItemImageMappingDAO()
                    {
                        ShowingItemId = ShowingItem.Id,
                        ImageId = ShowingItemImageMapping.ImageId,
                    };
                    ShowingItemImageMappingDAOs.Add(ShowingItemImageMappingDAO);
                }
                await DataContext.ShowingItemImageMapping.BulkMergeAsync(ShowingItemImageMappingDAOs);
            }
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.ShowingItem
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ShowingItemDAO { Used = true });
            return true;
        }

    }
}
