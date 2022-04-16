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
    public interface IShowingOrderRepository
    {
        Task<int> Count(ShowingOrderFilter ShowingOrderFilter);
        Task<int> CountAll(ShowingOrderFilter ShowingOrderFilter);
        Task<List<ShowingOrder>> List(ShowingOrderFilter ShowingOrderFilter);
        Task<List<ShowingOrder>> List(List<long> Ids);
        Task<ShowingOrder> Get(long Id);
        Task<bool> Create(ShowingOrder ShowingOrder);
        Task<bool> Update(ShowingOrder ShowingOrder);
        Task<bool> Delete(ShowingOrder ShowingOrder);
        Task<bool> BulkMerge(List<ShowingOrder> ShowingOrders);
        Task<bool> BulkDelete(List<ShowingOrder> ShowingOrders);
    }
    public class ShowingOrderRepository : IShowingOrderRepository
    {
        private DataContext DataContext;
        public ShowingOrderRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ShowingOrderDAO> DynamicFilter(IQueryable<ShowingOrderDAO> query, ShowingOrderFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            query = query.Where(q => q.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            if (filter.ShowingItemId != null && filter.ShowingItemId.HasValue)
            {
                if (filter.ShowingItemId.Equal.HasValue)
                {
                    query = from q in query
                            join c in DataContext.ShowingOrderContent on q.Id equals c.ShowingOrderId
                            where c.ShowingItemId == filter.ShowingItemId.Equal.Value
                            select q;
                }

                if (filter.ShowingItemId.NotEqual.HasValue)
                {
                    query = from q in query
                            join c in DataContext.ShowingOrderContent on q.Id equals c.ShowingOrderId
                            where c.ShowingItemId != filter.ShowingItemId.Equal.Value
                            select q;
                }

                if (filter.ShowingItemId.In != null)
                {
                    query = from q in query
                            join c in DataContext.ShowingOrderContent on q.Id equals c.ShowingOrderId
                            where filter.ShowingItemId.In.Contains(c.ShowingItemId)
                            select q;
                }

                if (filter.ShowingItemId.NotIn != null)
                {
                    query = from q in query
                            join c in DataContext.ShowingOrderContent on q.Id equals c.ShowingOrderId
                            where !filter.ShowingItemId.In.Contains(c.ShowingItemId)
                            select q;
                }
                query = query.Distinct();
            }
            query = query.Where(q => q.Date, filter.Date);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.Total, filter.Total);
            query = query.Where(q => q.RowId, filter.RowId);
            return query;
        }

        private IQueryable<ShowingOrderDAO> OrFilter(IQueryable<ShowingOrderDAO> query, ShowingOrderFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ShowingOrderDAO> initQuery = query.Where(q => false);
            foreach (ShowingOrderFilter ShowingOrderFilter in filter.OrFilter)
            {
                IQueryable<ShowingOrderDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.AppUserId, filter.AppUserId);
                queryable = queryable.Where(q => q.OrganizationId, filter.OrganizationId);
                queryable = queryable.Where(q => q.StoreId, filter.StoreId);
                queryable = queryable.Where(q => q.Date, filter.Date);
                queryable = queryable.Where(q => q.StatusId, filter.StatusId);
                queryable = queryable.Where(q => q.Total, filter.Total);
                queryable = queryable.Where(q => q.RowId, filter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ShowingOrderDAO> DynamicOrder(IQueryable<ShowingOrderDAO> query, ShowingOrderFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ShowingOrderOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ShowingOrderOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ShowingOrderOrder.AppUser:
                            query = query.OrderBy(q => q.AppUserId);
                            break;
                        case ShowingOrderOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case ShowingOrderOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case ShowingOrderOrder.Date:
                            query = query.OrderBy(q => q.Date);
                            break;
                        case ShowingOrderOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ShowingOrderOrder.Total:
                            query = query.OrderBy(q => q.Total);
                            break;
                        case ShowingOrderOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ShowingOrderOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ShowingOrderOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ShowingOrderOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUserId);
                            break;
                        case ShowingOrderOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case ShowingOrderOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case ShowingOrderOrder.Date:
                            query = query.OrderByDescending(q => q.Date);
                            break;
                        case ShowingOrderOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ShowingOrderOrder.Total:
                            query = query.OrderByDescending(q => q.Total);
                            break;
                        case ShowingOrderOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ShowingOrder>> DynamicSelect(IQueryable<ShowingOrderDAO> query, ShowingOrderFilter filter)
        {
            List<ShowingOrder> ShowingOrders = await query.Select(q => new ShowingOrder()
            {
                Id = filter.Selects.Contains(ShowingOrderSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ShowingOrderSelect.Code) ? q.Code : default(string),
                AppUserId = filter.Selects.Contains(ShowingOrderSelect.AppUser) ? q.AppUserId : default(long),
                OrganizationId = filter.Selects.Contains(ShowingOrderSelect.Organization) ? q.OrganizationId : default(long),
                StoreId = filter.Selects.Contains(ShowingOrderSelect.Store) ? q.StoreId : default(long),
                Date = filter.Selects.Contains(ShowingOrderSelect.Date) ? q.Date : default(DateTime),
                StatusId = filter.Selects.Contains(ShowingOrderSelect.Status) ? q.StatusId : default(long),
                Total = filter.Selects.Contains(ShowingOrderSelect.Total) ? q.Total : default(decimal),
                RowId = filter.Selects.Contains(ShowingOrderSelect.Row) ? q.RowId : default(Guid),
                AppUser = filter.Selects.Contains(ShowingOrderSelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                    Address = q.AppUser.Address,
                    Email = q.AppUser.Email,
                    Phone = q.AppUser.Phone,
                    SexId = q.AppUser.SexId,
                    Birthday = q.AppUser.Birthday,
                    Avatar = q.AppUser.Avatar,
                    PositionId = q.AppUser.PositionId,
                    Department = q.AppUser.Department,
                    OrganizationId = q.AppUser.OrganizationId,
                    ProvinceId = q.AppUser.ProvinceId,
                    StatusId = q.AppUser.StatusId,
                    RowId = q.AppUser.RowId,
                } : null,
                Organization = filter.Selects.Contains(ShowingOrderSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Email = q.Organization.Email,
                    Address = q.Organization.Address,
                    RowId = q.Organization.RowId,
                } : null,
                Status = filter.Selects.Contains(ShowingOrderSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Store = filter.Selects.Contains(ShowingOrderSelect.Store) && q.Store != null ? new Store
                {
                    Id = q.Store.Id,
                    Code = q.Store.Code,
                    CodeDraft = q.Store.CodeDraft,
                    Name = q.Store.Name,
                    UnsignName = q.Store.UnsignName,
                    ParentStoreId = q.Store.ParentStoreId,
                    OrganizationId = q.Store.OrganizationId,
                    StoreTypeId = q.Store.StoreTypeId,
                    Telephone = q.Store.Telephone,
                    ProvinceId = q.Store.ProvinceId,
                    DistrictId = q.Store.DistrictId,
                    WardId = q.Store.WardId,
                    Address = q.Store.Address,
                    UnsignAddress = q.Store.UnsignAddress,
                    DeliveryAddress = q.Store.DeliveryAddress,
                    Latitude = q.Store.Latitude,
                    Longitude = q.Store.Longitude,
                    DeliveryLatitude = q.Store.DeliveryLatitude,
                    DeliveryLongitude = q.Store.DeliveryLongitude,
                    OwnerName = q.Store.OwnerName,
                    OwnerPhone = q.Store.OwnerPhone,
                    OwnerEmail = q.Store.OwnerEmail,
                    TaxCode = q.Store.TaxCode,
                    LegalEntity = q.Store.LegalEntity,
                    CreatorId = q.Store.CreatorId,
                    AppUserId = q.Store.AppUserId,
                    StatusId = q.Store.StatusId,
                    RowId = q.Store.RowId,
                    Used = q.Store.Used,
                    StoreScoutingId = q.Store.StoreScoutingId,
                    StoreStatusId = q.Store.StoreStatusId,
                    StoreStatus = q.Store.StoreStatus == null ? null : new StoreStatus
                    {
                        Id = q.Store.StoreStatus.Id,
                        Code = q.Store.StoreStatus.Code,
                        Name = q.Store.StoreStatus.Name,
                    }
                } : null,
            }).ToListWithNoLockAsync();
            return ShowingOrders;
        }

        public async Task<int> Count(ShowingOrderFilter filter)
        {
            IQueryable<ShowingOrderDAO> ShowingOrders = DataContext.ShowingOrder.AsNoTracking();
            ShowingOrders = DynamicFilter(ShowingOrders, filter);
            ShowingOrders = OrFilter(ShowingOrders, filter);
            return await ShowingOrders.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ShowingOrderFilter filter)
        {
            IQueryable<ShowingOrderDAO> ShowingOrders = DataContext.ShowingOrder.AsNoTracking();
            ShowingOrders = DynamicFilter(ShowingOrders, filter);
            return await ShowingOrders.CountWithNoLockAsync();
        }

        public async Task<List<ShowingOrder>> List(ShowingOrderFilter filter)
        {
            if (filter == null) return new List<ShowingOrder>();
            IQueryable<ShowingOrderDAO> ShowingOrderDAOs = DataContext.ShowingOrder.AsNoTracking();
            ShowingOrderDAOs = DynamicFilter(ShowingOrderDAOs, filter);
            ShowingOrderDAOs = OrFilter(ShowingOrderDAOs, filter);
            ShowingOrderDAOs = DynamicOrder(ShowingOrderDAOs, filter);
            List<ShowingOrder> ShowingOrders = await DynamicSelect(ShowingOrderDAOs, filter);
            return ShowingOrders;
        }

        public async Task<List<ShowingOrder>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
             .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.ShowingOrder
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;

            List<ShowingOrder> ShowingOrders = await query.AsNoTracking()
            .Select(x => new ShowingOrder()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                AppUserId = x.AppUserId,
                OrganizationId = x.OrganizationId,
                StoreId = x.StoreId,
                Date = x.Date,
                StatusId = x.StatusId,
                Total = x.Total,
                RowId = x.RowId,
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                    Address = x.AppUser.Address,
                    Email = x.AppUser.Email,
                    Phone = x.AppUser.Phone,
                    SexId = x.AppUser.SexId,
                    Birthday = x.AppUser.Birthday,
                    Avatar = x.AppUser.Avatar,
                    PositionId = x.AppUser.PositionId,
                    Department = x.AppUser.Department,
                    OrganizationId = x.AppUser.OrganizationId,
                    ProvinceId = x.AppUser.ProvinceId,
                    StatusId = x.AppUser.StatusId,
                    RowId = x.AppUser.RowId,
                },
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                    RowId = x.Organization.RowId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    UnsignName = x.Store.UnsignName,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    UnsignAddress = x.Store.UnsignAddress,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    CreatorId = x.Store.CreatorId,
                    AppUserId = x.Store.AppUserId,
                    StatusId = x.Store.StatusId,
                    RowId = x.Store.RowId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                },
            }).ToListWithNoLockAsync();

            var ShowingOrderContent = from s in DataContext.ShowingOrderContent
                                      join tt in tempTableQuery.Query on s.ShowingOrderId equals tt.Column1
                                      select s;


            List<ShowingOrderContent> ShowingOrderContents = await ShowingOrderContent.AsNoTracking()
                .Select(x => new ShowingOrderContent
                {
                    Id = x.Id,
                    ShowingOrderId = x.ShowingOrderId,
                    ShowingItemId = x.ShowingItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    SalePrice = x.SalePrice,
                    Quantity = x.Quantity,
                    Amount = x.Amount,
                    ShowingItem = new ShowingItem
                    {
                        Id = x.ShowingItem.Id,
                        Code = x.ShowingItem.Code,
                        Name = x.ShowingItem.Name,
                        ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                        UnitOfMeasureId = x.ShowingItem.UnitOfMeasureId,
                        SalePrice = x.ShowingItem.SalePrice,
                        Description = x.ShowingItem.Description,
                        StatusId = x.ShowingItem.StatusId,
                        Used = x.ShowingItem.Used,
                        RowId = x.ShowingItem.RowId,
                        ShowingCategory = x.ShowingItem.ShowingCategory == null ? null : new ShowingCategory
                        {
                            Id = x.ShowingItem.ShowingCategory.Id,
                            Code = x.ShowingItem.ShowingCategory.Code,
                            Name = x.ShowingItem.ShowingCategory.Name,
                        }
                    },
                    UnitOfMeasure = new UnitOfMeasure
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
            foreach (ShowingOrder ShowingOrder in ShowingOrders)
            {
                ShowingOrder.ShowingOrderContents = ShowingOrderContents
                    .Where(x => x.ShowingOrderId == ShowingOrder.Id)
                    .ToList();
            }

            return ShowingOrders;
        }

        public async Task<ShowingOrder> Get(long Id)
        {
            ShowingOrder ShowingOrder = await DataContext.ShowingOrder.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new ShowingOrder()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                AppUserId = x.AppUserId,
                OrganizationId = x.OrganizationId,
                StoreId = x.StoreId,
                Date = x.Date,
                StatusId = x.StatusId,
                Total = x.Total,
                RowId = x.RowId,
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                    Address = x.AppUser.Address,
                    Email = x.AppUser.Email,
                    Phone = x.AppUser.Phone,
                    SexId = x.AppUser.SexId,
                    Birthday = x.AppUser.Birthday,
                    Avatar = x.AppUser.Avatar,
                    PositionId = x.AppUser.PositionId,
                    Department = x.AppUser.Department,
                    OrganizationId = x.AppUser.OrganizationId,
                    ProvinceId = x.AppUser.ProvinceId,
                    StatusId = x.AppUser.StatusId,
                    RowId = x.AppUser.RowId,
                },
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                    RowId = x.Organization.RowId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    UnsignName = x.Store.UnsignName,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    UnsignAddress = x.Store.UnsignAddress,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    CreatorId = x.Store.CreatorId,
                    AppUserId = x.Store.AppUserId,
                    StatusId = x.Store.StatusId,
                    RowId = x.Store.RowId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ShowingOrder == null)
                return null;
            ShowingOrder.ShowingOrderContents = await DataContext.ShowingOrderContent.AsNoTracking()
                .Where(x => x.ShowingOrderId == ShowingOrder.Id)
                .Select(x => new ShowingOrderContent
                {
                    Id = x.Id,
                    ShowingOrderId = x.ShowingOrderId,
                    ShowingItemId = x.ShowingItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    SalePrice = x.SalePrice,
                    Quantity = x.Quantity,
                    Amount = x.Amount,
                    ShowingItem = new ShowingItem
                    {
                        Id = x.ShowingItem.Id,
                        Code = x.ShowingItem.Code,
                        Name = x.ShowingItem.Name,
                        ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                        UnitOfMeasureId = x.ShowingItem.UnitOfMeasureId,
                        SalePrice = x.ShowingItem.SalePrice,
                        Description = x.ShowingItem.Description,
                        StatusId = x.ShowingItem.StatusId,
                        Used = x.ShowingItem.Used,
                        RowId = x.ShowingItem.RowId,
                        ShowingCategory = x.ShowingItem.ShowingCategory == null ? null : new ShowingCategory
                        {
                            Id = x.ShowingItem.ShowingCategory.Id,
                            Code = x.ShowingItem.ShowingCategory.Code,
                            Name = x.ShowingItem.ShowingCategory.Name,
                        }
                    },
                    UnitOfMeasure = new UnitOfMeasure
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

            return ShowingOrder;
        }
        public async Task<bool> Create(ShowingOrder ShowingOrder)
        {
            ShowingOrderDAO ShowingOrderDAO = new ShowingOrderDAO();
            ShowingOrderDAO.Id = ShowingOrder.Id;
            ShowingOrderDAO.Code = ShowingOrder.Code;
            ShowingOrderDAO.AppUserId = ShowingOrder.AppUserId;
            ShowingOrderDAO.OrganizationId = ShowingOrder.OrganizationId;
            ShowingOrderDAO.StoreId = ShowingOrder.StoreId;
            ShowingOrderDAO.Date = ShowingOrder.Date;
            ShowingOrderDAO.StatusId = ShowingOrder.StatusId;
            ShowingOrderDAO.Total = ShowingOrder.Total;
            ShowingOrderDAO.RowId = ShowingOrder.RowId;
            ShowingOrderDAO.CreatedAt = StaticParams.DateTimeNow;
            ShowingOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ShowingOrder.Add(ShowingOrderDAO);
            await DataContext.SaveChangesAsync();
            ShowingOrder.Id = ShowingOrderDAO.Id;
            await SaveReference(ShowingOrder);
            return true;
        }

        public async Task<bool> Update(ShowingOrder ShowingOrder)
        {
            ShowingOrderDAO ShowingOrderDAO = DataContext.ShowingOrder.Where(x => x.Id == ShowingOrder.Id).FirstOrDefault();
            if (ShowingOrderDAO == null)
                return false;
            ShowingOrderDAO.Id = ShowingOrder.Id;
            ShowingOrderDAO.Code = ShowingOrder.Code;
            ShowingOrderDAO.AppUserId = ShowingOrder.AppUserId;
            ShowingOrderDAO.OrganizationId = ShowingOrder.OrganizationId;
            ShowingOrderDAO.StoreId = ShowingOrder.StoreId;
            ShowingOrderDAO.Date = ShowingOrder.Date;
            ShowingOrderDAO.StatusId = ShowingOrder.StatusId;
            ShowingOrderDAO.Total = ShowingOrder.Total;
            ShowingOrderDAO.RowId = ShowingOrder.RowId;
            ShowingOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(ShowingOrder);
            return true;
        }

        public async Task<bool> Delete(ShowingOrder ShowingOrder)
        {
            await DataContext.ShowingOrder.Where(x => x.Id == ShowingOrder.Id).UpdateFromQueryAsync(x => new ShowingOrderDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<ShowingOrder> ShowingOrders)
        {
            List<ShowingOrderDAO> ShowingOrderDAOs = new List<ShowingOrderDAO>();
            foreach (ShowingOrder ShowingOrder in ShowingOrders)
            {
                ShowingOrderDAO ShowingOrderDAO = new ShowingOrderDAO();
                ShowingOrderDAO.Id = ShowingOrder.Id;
                ShowingOrderDAO.Code = ShowingOrder.Code;
                ShowingOrderDAO.AppUserId = ShowingOrder.AppUserId;
                ShowingOrderDAO.OrganizationId = ShowingOrder.OrganizationId;
                ShowingOrderDAO.StoreId = ShowingOrder.StoreId;
                ShowingOrderDAO.Date = ShowingOrder.Date;
                ShowingOrderDAO.StatusId = ShowingOrder.StatusId;
                ShowingOrderDAO.Total = ShowingOrder.Total;
                ShowingOrderDAO.RowId = ShowingOrder.RowId;
                ShowingOrderDAO.CreatedAt = StaticParams.DateTimeNow;
                ShowingOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
                ShowingOrderDAOs.Add(ShowingOrderDAO);
            }
            await DataContext.BulkMergeAsync(ShowingOrderDAOs);

            foreach (var ShowingOrderDAO in ShowingOrderDAOs)
            {
                if (ShowingOrderDAO.Id < 1000000)
                {
                    var Code = (ShowingOrderDAO.Id + 1000000).ToString();
                    ShowingOrderDAO.Code = Code.Substring(Code.Length - 6);
                }
                else
                {
                    ShowingOrderDAO.Code = ShowingOrderDAO.Id.ToString();
                }
            }
            await DataContext.BulkMergeAsync(ShowingOrderDAOs);

            var Ids = ShowingOrderDAOs.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            // before
            //await DataContext.ShowingOrderContent
            //    .Where(x => x.ShowingOrderId, IdFilter)
            //    .DeleteFromQueryAsync();
            // after
            await DataContext.ShowingOrderContent
                .Where(x => IdFilter.In.Contains(x.ShowingOrderId))
                .DeleteFromQueryAsync();

            List<ShowingOrderContentDAO> ShowingOrderContentDAOs = new List<ShowingOrderContentDAO>();
            foreach (var ShowingOrder in ShowingOrders)
            {
                ShowingOrder.Id = ShowingOrderDAOs.Where(x => x.RowId == ShowingOrder.RowId).Select(x => x.Id).FirstOrDefault();
                if (ShowingOrder.ShowingOrderContents != null)
                {
                    var listContent = ShowingOrder.ShowingOrderContents?.Select(x => new ShowingOrderContentDAO
                    {
                        Amount = x.Amount,
                        Quantity = x.Quantity,
                        SalePrice = x.SalePrice,
                        ShowingItemId = x.ShowingItemId,
                        ShowingOrderId = ShowingOrder.Id,
                        UnitOfMeasureId = x.UnitOfMeasureId,
                    }).ToList();
                    ShowingOrderContentDAOs.AddRange(listContent);
                }
            }
            await DataContext.BulkMergeAsync(ShowingOrderContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ShowingOrder> ShowingOrders)
        {
            List<long> Ids = ShowingOrders.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DataContext.ShowingOrder
                .Where(x => IdFilter.In.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ShowingOrderDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(ShowingOrder ShowingOrder)
        {
            await DataContext.ShowingOrderContent
                .Where(x => x.ShowingOrderId == ShowingOrder.Id)
                .DeleteFromQueryAsync();
            List<ShowingOrderContentDAO> ShowingOrderContentDAOs = new List<ShowingOrderContentDAO>();
            if (ShowingOrder.ShowingOrderContents != null)
            {
                foreach (ShowingOrderContent ShowingOrderContent in ShowingOrder.ShowingOrderContents)
                {
                    ShowingOrderContentDAO ShowingOrderContentDAO = new ShowingOrderContentDAO();
                    ShowingOrderContentDAO.Id = ShowingOrderContent.Id;
                    ShowingOrderContentDAO.ShowingOrderId = ShowingOrder.Id;
                    ShowingOrderContentDAO.ShowingItemId = ShowingOrderContent.ShowingItemId;
                    ShowingOrderContentDAO.UnitOfMeasureId = ShowingOrderContent.UnitOfMeasureId;
                    ShowingOrderContentDAO.SalePrice = ShowingOrderContent.SalePrice;
                    ShowingOrderContentDAO.Quantity = ShowingOrderContent.Quantity;
                    ShowingOrderContentDAO.Amount = ShowingOrderContent.Amount;
                    ShowingOrderContentDAOs.Add(ShowingOrderContentDAO);
                }
                await DataContext.ShowingOrderContent.BulkMergeAsync(ShowingOrderContentDAOs);
            }
        }

    }
}
