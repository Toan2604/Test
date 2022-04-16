using DMS.Entities;
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
    public interface IStoreCheckingRepository
    {
        Task<int> Count(StoreCheckingFilter StoreCheckingFilter);
        Task<int> CountAll(StoreCheckingFilter StoreCheckingFilter);
        Task<List<StoreChecking>> List(StoreCheckingFilter StoreCheckingFilter);
        Task<List<StoreChecking>> List(List<long> Ids);
        Task<StoreChecking> Get(long Id);
        Task<bool> Create(StoreChecking StoreChecking);
        Task<bool> Update(StoreChecking StoreChecking);
    }
    public class StoreCheckingRepository : IStoreCheckingRepository
    {
        private DataContext DataContext;
        public StoreCheckingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<StoreCheckingDAO>> DynamicFilter(IQueryable<StoreCheckingDAO> query, StoreCheckingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            if (filter.StoreId != null)
            {
                if (filter.StoreId.In != null)
                {
                    query = query.Where(q => q.StoreId, filter.StoreId);
                }
                else
                {
                    query = query.Where(q => q.StoreId, filter.StoreId);
                }
            }
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = await DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO =await DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
            }
            query = query.Where(q => q.SaleEmployeeId, filter.SaleEmployeeId);
            query = query.Where(q => q.Longitude, filter.Longitude);
            query = query.Where(q => q.Latitude, filter.Latitude);
            query = query.Where(q => q.CheckInAt, filter.CheckInAt);
            query = query.Where(q => q.CheckOutAt, filter.CheckOutAt);
            query = query.Where(q => q.IndirectSalesOrderCounter, filter.CountIndirectSalesOrder);
            query = query.Where(q => q.ImageCounter, filter.CountImage);
            return query;
        }

        private async Task<IQueryable<StoreCheckingDAO>> OrFilter(IQueryable<StoreCheckingDAO> query, StoreCheckingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreCheckingDAO> initQuery = query.Where(q => false);
            foreach (StoreCheckingFilter StoreCheckingFilter in filter.OrFilter)
            {
                IQueryable<StoreCheckingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreCheckingFilter.Id);
                queryable = queryable.Where(q => q.StoreId, StoreCheckingFilter.StoreId);
                queryable = queryable.Where(q => q.SaleEmployeeId, StoreCheckingFilter.SaleEmployeeId);
                if (StoreCheckingFilter.OrganizationId != null)
                {
                    if (StoreCheckingFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = await DataContext.Organization
                            .Where(o => o.Id == StoreCheckingFilter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreCheckingFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = await DataContext.Organization
                            .Where(o => o.Id == StoreCheckingFilter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreCheckingFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreCheckingFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                    if (StoreCheckingFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreCheckingFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.Longitude, StoreCheckingFilter.Longitude);
                queryable = queryable.Where(q => q.Latitude, StoreCheckingFilter.Latitude);
                queryable = queryable.Where(q => q.CheckInAt, StoreCheckingFilter.CheckInAt);
                queryable = queryable.Where(q => q.CheckOutAt, StoreCheckingFilter.CheckOutAt);
                queryable = queryable.Where(q => q.IndirectSalesOrderCounter, StoreCheckingFilter.CountIndirectSalesOrder);
                queryable = queryable.Where(q => q.ImageCounter, StoreCheckingFilter.CountImage);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreCheckingDAO> DynamicOrder(IQueryable<StoreCheckingDAO> query, StoreCheckingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreCheckingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreCheckingOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case StoreCheckingOrder.SaleEmployee:
                            query = query.OrderBy(q => q.SaleEmployeeId);
                            break;
                        case StoreCheckingOrder.Longitude:
                            query = query.OrderBy(q => q.Longitude);
                            break;
                        case StoreCheckingOrder.Latitude:
                            query = query.OrderBy(q => q.Latitude);
                            break;
                        case StoreCheckingOrder.CheckInAt:
                            query = query.OrderBy(q => q.CheckInAt);
                            break;
                        case StoreCheckingOrder.CheckOutAt:
                            query = query.OrderBy(q => q.CheckOutAt);
                            break;
                        case StoreCheckingOrder.CountIndirectSalesOrder:
                            query = query.OrderBy(q => q.IndirectSalesOrderCounter);
                            break;
                        case StoreCheckingOrder.CountImage:
                            query = query.OrderBy(q => q.ImageCounter);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreCheckingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreCheckingOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case StoreCheckingOrder.SaleEmployee:
                            query = query.OrderByDescending(q => q.SaleEmployeeId);
                            break;
                        case StoreCheckingOrder.Longitude:
                            query = query.OrderByDescending(q => q.Longitude);
                            break;
                        case StoreCheckingOrder.Latitude:
                            query = query.OrderByDescending(q => q.Latitude);
                            break;
                        case StoreCheckingOrder.CheckInAt:
                            query = query.OrderByDescending(q => q.CheckInAt);
                            break;
                        case StoreCheckingOrder.CheckOutAt:
                            query = query.OrderByDescending(q => q.CheckOutAt);
                            break;
                        case StoreCheckingOrder.CountIndirectSalesOrder:
                            query = query.OrderByDescending(q => q.IndirectSalesOrderCounter);
                            break;
                        case StoreCheckingOrder.CountImage:
                            query = query.OrderByDescending(q => q.ImageCounter);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreChecking>> DynamicSelect(IQueryable<StoreCheckingDAO> query, StoreCheckingFilter filter)
        {
            List<StoreChecking> StoreCheckings = await query.Select(q => new StoreChecking()
            {
                Id = filter.Selects.Contains(StoreCheckingSelect.Id) ? q.Id : default(long),
                StoreId = filter.Selects.Contains(StoreCheckingSelect.Store) ? q.StoreId : default(long),
                SaleEmployeeId = filter.Selects.Contains(StoreCheckingSelect.SaleEmployee) ? q.SaleEmployeeId : default(long),
                Longitude = filter.Selects.Contains(StoreCheckingSelect.Longitude) ? q.Longitude : default(decimal?),
                Latitude = filter.Selects.Contains(StoreCheckingSelect.Latitude) ? q.Latitude : default(decimal?),
                CheckOutLongitude = filter.Selects.Contains(StoreCheckingSelect.CheckOutLongitude) ? q.CheckOutLongitude : default(decimal?),
                CheckOutLatitude = filter.Selects.Contains(StoreCheckingSelect.CheckOutLatitude) ? q.CheckOutLatitude : default(decimal?),
                CheckInAt = filter.Selects.Contains(StoreCheckingSelect.CheckInAt) ? q.CheckInAt : default(DateTime?),
                CheckOutAt = filter.Selects.Contains(StoreCheckingSelect.CheckOutAt) ? q.CheckOutAt : default(DateTime?),
                CheckInDistance = filter.Selects.Contains(StoreCheckingSelect.CheckInDistance) ? q.CheckInDistance : default(long?),
                CheckOutDistance = filter.Selects.Contains(StoreCheckingSelect.CheckOutDistance) ? q.CheckOutDistance : default(long?),
                CountIndirectSalesOrder = filter.Selects.Contains(StoreCheckingSelect.CountIndirectSalesOrder) ? q.IndirectSalesOrderCounter : default(long?),
                ImageCounter = filter.Selects.Contains(StoreCheckingSelect.CountImage) ? q.ImageCounter : default(long?),
                SaleEmployee = filter.Selects.Contains(StoreCheckingSelect.SaleEmployee) && q.SaleEmployee == null ? null : new AppUser
                {
                    Id = q.SaleEmployee.Id,
                    Username = q.SaleEmployee.Username,
                    DisplayName = q.SaleEmployee.DisplayName,
                    Email = q.SaleEmployee.Email,
                    Phone = q.SaleEmployee.Phone,
                    Address = q.SaleEmployee.Address,
                    Department = q.SaleEmployee.Department,
                    PositionId = q.SaleEmployee.PositionId,
                    RowId = q.SaleEmployee.RowId,
                    SexId = q.SaleEmployee.SexId,
                    StatusId = q.SaleEmployee.StatusId,
                    OrganizationId = q.SaleEmployee.OrganizationId,
                    Organization = q.SaleEmployee.Organization == null ? null : new Organization
                    {
                        Id = q.SaleEmployee.Organization.Id,
                        Code = q.SaleEmployee.Organization.Code,
                        Name = q.SaleEmployee.Organization.Name,
                    },
                },
                Organization = filter.Selects.Contains(StoreSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Address = q.Organization.Address,
                    Email = q.Organization.Email,
                } : null,
                Store = filter.Selects.Contains(StoreCheckingSelect.Store) && q.Store == null ? null : new Store
                {
                    Id = q.Store.Id,
                    Code = q.Store.Code,
                    Name = q.Store.Name,
                    ParentStoreId = q.Store.ParentStoreId,
                    OrganizationId = q.Store.OrganizationId,
                    StoreTypeId = q.Store.StoreTypeId,
                    Telephone = q.Store.Telephone,
                    ProvinceId = q.Store.ProvinceId,
                    DistrictId = q.Store.DistrictId,
                    WardId = q.Store.WardId,
                    Address = q.Store.Address,
                    DeliveryAddress = q.Store.DeliveryAddress,
                    Latitude = q.Store.Latitude,
                    Longitude = q.Store.Longitude,
                    OwnerName = q.Store.OwnerName,
                    OwnerPhone = q.Store.OwnerPhone,
                    OwnerEmail = q.Store.OwnerEmail,
                    TaxCode = q.Store.TaxCode,
                    LegalEntity = q.Store.LegalEntity,
                    StatusId = q.Store.StatusId,
                },
                Planned = q.Planned,
                IsOpenedStore = q.IsOpenedStore,
                DeviceName = q.DeviceName,
            }).ToListWithNoLockAsync();
            return StoreCheckings;
        }

        public async Task<int> Count(StoreCheckingFilter filter)
        {
            IQueryable<StoreCheckingDAO> StoreCheckings = DataContext.StoreChecking.AsNoTracking();
            StoreCheckings = await DynamicFilter(StoreCheckings, filter);
            StoreCheckings = await OrFilter(StoreCheckings, filter);
            return await StoreCheckings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreCheckingFilter filter)
        {
            IQueryable<StoreCheckingDAO> StoreCheckings = DataContext.StoreChecking.AsNoTracking();
            StoreCheckings = await DynamicFilter(StoreCheckings, filter);
            return await StoreCheckings.CountWithNoLockAsync();
        }

        public async Task<List<StoreChecking>> List(StoreCheckingFilter filter)
        {
            if (filter == null) return new List<StoreChecking>();
            IQueryable<StoreCheckingDAO> StoreCheckingDAOs = DataContext.StoreChecking.AsNoTracking();
            StoreCheckingDAOs = await DynamicFilter(StoreCheckingDAOs, filter);
            StoreCheckingDAOs = await OrFilter(StoreCheckingDAOs, filter);
            StoreCheckingDAOs = DynamicOrder(StoreCheckingDAOs, filter);
            List<StoreChecking> StoreCheckings = await DynamicSelect(StoreCheckingDAOs, filter);
            return StoreCheckings;
        }

        public async Task<List<StoreChecking>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
           .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.StoreChecking
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;

            List<StoreChecking> StoreCheckings = await query.AsNoTracking()
            .Select(x => new StoreChecking()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                SaleEmployeeId = x.SaleEmployeeId,
                OrganizationId = x.OrganizationId,
                DeviceName = x.DeviceName,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                CheckOutLongitude = x.CheckOutLongitude,
                CheckOutLatitude = x.CheckOutLatitude,
                CheckInAt = x.CheckInAt,
                CheckOutAt = x.CheckOutAt,
                CheckInDistance = x.CheckInDistance,
                CheckOutDistance = x.CheckOutDistance,
                CountIndirectSalesOrder = x.IndirectSalesOrderCounter,
                ImageCounter = x.ImageCounter,
                IsOpenedStore = x.IsOpenedStore,
                Planned = x.Planned,
                SaleEmployee = x.SaleEmployee == null ? null : new AppUser
                {
                    Id = x.SaleEmployee.Id,
                    Username = x.SaleEmployee.Username,
                    DisplayName = x.SaleEmployee.DisplayName,
                    Email = x.SaleEmployee.Email,
                    Phone = x.SaleEmployee.Phone,
                    Address = x.SaleEmployee.Address,
                    Department = x.SaleEmployee.Department,
                    PositionId = x.SaleEmployee.PositionId,
                    RowId = x.SaleEmployee.RowId,
                    SexId = x.SaleEmployee.SexId,
                    StatusId = x.SaleEmployee.StatusId,
                    OrganizationId = x.SaleEmployee.OrganizationId,
                    Organization = x.SaleEmployee.Organization == null ? null : new Organization
                    {
                        Id = x.SaleEmployee.Organization.Id,
                        Code = x.SaleEmployee.Organization.Code,
                        Name = x.SaleEmployee.Organization.Name,
                    },
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
                    Address = x.Organization.Address,
                    Email = x.Organization.Email,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    StatusId = x.Store.StatusId,
                }
            }).ToListWithNoLockAsync();
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<StoreCheckingImageMapping> StoreCheckingImageMappings = await DataContext.StoreCheckingImageMapping
                .Where(x => x.StoreCheckingId, IdFilter)
                .Select(x => new StoreCheckingImageMapping
                {
                    ImageId = x.ImageId,
                    StoreCheckingId = x.StoreCheckingId,
                    AlbumId = x.AlbumId,
                    StoreId = x.StoreId,
                    SaleEmployeeId = x.SaleEmployeeId,
                    ShootingAt = x.ShootingAt,
                    Distance = x.Distance,
                    Album = new Album
                    {
                        Id = x.Album.Id,
                        Name = x.Album.Name,
                    },
                    SaleEmployee = new AppUser
                    {
                        Id = x.SaleEmployee.Id,
                        Username = x.SaleEmployee.Username,
                        DisplayName = x.SaleEmployee.DisplayName,
                        Email = x.SaleEmployee.Email,
                        Phone = x.SaleEmployee.Phone,
                    },
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                    Store = new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        Name = x.Store.Name,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                    },
                }).ToListWithNoLockAsync();
            foreach (StoreChecking StoreChecking in StoreCheckings)
            {
                StoreChecking.StoreCheckingImageMappings = StoreCheckingImageMappings
                    .Where(x => x.StoreCheckingId == StoreChecking.Id).ToList();
            }
            return StoreCheckings;
        }

        public async Task<StoreChecking> Get(long Id)
        {
            StoreChecking StoreChecking = await DataContext.StoreChecking.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new StoreChecking()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                SaleEmployeeId = x.SaleEmployeeId,
                OrganizationId = x.OrganizationId,
                DeviceName = x.DeviceName,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                CheckOutLongitude = x.CheckOutLongitude,
                CheckOutLatitude = x.CheckOutLatitude,
                CheckInAt = x.CheckInAt,
                CheckOutAt = x.CheckOutAt,
                CheckInDistance = x.CheckInDistance,
                CheckOutDistance = x.CheckOutDistance,
                CountIndirectSalesOrder = x.IndirectSalesOrderCounter,
                ImageCounter = x.ImageCounter,
                IsOpenedStore = x.IsOpenedStore,
                Planned = x.Planned,
                SaleEmployee = x.SaleEmployee == null ? null : new AppUser
                {
                    Id = x.SaleEmployee.Id,
                    Username = x.SaleEmployee.Username,
                    DisplayName = x.SaleEmployee.DisplayName,
                    Email = x.SaleEmployee.Email,
                    Phone = x.SaleEmployee.Phone,
                    Address = x.SaleEmployee.Address,
                    Department = x.SaleEmployee.Department,
                    PositionId = x.SaleEmployee.PositionId,
                    RowId = x.SaleEmployee.RowId,
                    SexId = x.SaleEmployee.SexId,
                    StatusId = x.SaleEmployee.StatusId,
                    OrganizationId = x.SaleEmployee.OrganizationId,
                    Organization = x.SaleEmployee.Organization == null ? null : new Organization
                    {
                        Id = x.SaleEmployee.Organization.Id,
                        Code = x.SaleEmployee.Organization.Code,
                        Name = x.SaleEmployee.Organization.Name,
                    },
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
                    Address = x.Organization.Address,
                    Email = x.Organization.Email,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    StatusId = x.Store.StatusId,
                    ParentStore = new Store
                    {
                        Id = x.Store.ParentStore.Id,
                        Code = x.Store.ParentStore.Code,
                        Name = x.Store.ParentStore.Name,
                    }
                }
            }).FirstOrDefaultWithNoLockAsync();

            if (StoreChecking == null)
                return null;
            StoreChecking.StoreCheckingImageMappings = await DataContext.StoreCheckingImageMapping.AsNoTracking()
                .Where(x => x.StoreCheckingId == StoreChecking.Id)
                .Where(x => x.Image.DeletedAt == null)
                .Select(x => new StoreCheckingImageMapping
                {
                    ImageId = x.ImageId,
                    StoreCheckingId = x.StoreCheckingId,
                    AlbumId = x.AlbumId,
                    StoreId = x.StoreId,
                    SaleEmployeeId = x.SaleEmployeeId,
                    ShootingAt = x.ShootingAt,
                    Distance = x.Distance,
                    Album = new Album
                    {
                        Id = x.Album.Id,
                        Name = x.Album.Name,
                    },
                    SaleEmployee = new AppUser
                    {
                        Id = x.SaleEmployee.Id,
                        Username = x.SaleEmployee.Username,
                        DisplayName = x.SaleEmployee.DisplayName,
                        Email = x.SaleEmployee.Email,
                        Phone = x.SaleEmployee.Phone,
                    },
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                    Store = new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        Name = x.Store.Name,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                    },
                }).ToListWithNoLockAsync();

            return StoreChecking;
        }
        public async Task<bool> Create(StoreChecking StoreChecking)
        {
            StoreCheckingDAO StoreCheckingDAO = new StoreCheckingDAO();
            StoreCheckingDAO.Id = StoreChecking.Id;
            StoreCheckingDAO.StoreId = StoreChecking.StoreId;
            StoreCheckingDAO.OrganizationId = StoreChecking.OrganizationId;
            StoreCheckingDAO.SaleEmployeeId = StoreChecking.SaleEmployeeId;
            StoreCheckingDAO.DeviceName = StoreChecking.DeviceName;
            StoreCheckingDAO.Longitude = StoreChecking.Longitude;
            StoreCheckingDAO.Latitude = StoreChecking.Latitude;
            StoreCheckingDAO.CheckOutLongitude = StoreChecking.CheckOutLongitude;
            StoreCheckingDAO.CheckOutLatitude = StoreChecking.CheckOutLatitude;
            StoreCheckingDAO.CheckInAt = StoreChecking.CheckInAt;
            StoreCheckingDAO.CheckOutAt = StoreChecking.CheckOutAt;
            StoreCheckingDAO.CheckInDistance = StoreChecking.CheckInDistance;
            StoreCheckingDAO.CheckOutDistance = StoreChecking.CheckOutDistance;
            StoreCheckingDAO.IndirectSalesOrderCounter = StoreChecking.CountIndirectSalesOrder;
            StoreCheckingDAO.ImageCounter = StoreChecking.ImageCounter;
            StoreCheckingDAO.Planned = StoreChecking.Planned;
            StoreCheckingDAO.IsOpenedStore = StoreChecking.IsOpenedStore;
            DataContext.StoreChecking.Add(StoreCheckingDAO);
            await DataContext.SaveChangesAsync();
            StoreChecking.Id = StoreCheckingDAO.Id;
            await SaveReference(StoreChecking);
            return true;
        }

        public async Task<bool> Update(StoreChecking StoreChecking)
        {
            StoreCheckingDAO StoreCheckingDAO = DataContext.StoreChecking.Where(x => x.Id == StoreChecking.Id).FirstOrDefault();
            if (StoreCheckingDAO == null)
                return false;
            StoreCheckingDAO.Id = StoreChecking.Id;
            StoreCheckingDAO.StoreId = StoreChecking.StoreId;
            StoreCheckingDAO.SaleEmployeeId = StoreChecking.SaleEmployeeId;
            StoreCheckingDAO.Longitude = StoreChecking.Longitude;
            StoreCheckingDAO.Latitude = StoreChecking.Latitude;
            StoreCheckingDAO.CheckOutLongitude = StoreChecking.CheckOutLongitude;
            StoreCheckingDAO.CheckOutLatitude = StoreChecking.CheckOutLatitude;
            StoreCheckingDAO.CheckInAt = StoreChecking.CheckInAt;
            StoreCheckingDAO.CheckOutAt = StoreChecking.CheckOutAt;
            StoreCheckingDAO.CheckInDistance = StoreChecking.CheckInDistance;
            StoreCheckingDAO.CheckOutDistance = StoreChecking.CheckOutDistance;
            StoreCheckingDAO.IndirectSalesOrderCounter = StoreChecking.CountIndirectSalesOrder;
            StoreCheckingDAO.ImageCounter = StoreChecking.ImageCounter;
            StoreCheckingDAO.IsOpenedStore = StoreChecking.IsOpenedStore;
            await DataContext.SaveChangesAsync();
            await SaveReference(StoreChecking);
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreChecking> StoreCheckings)
        {
            List<StoreCheckingDAO> StoreCheckingDAOs = new List<StoreCheckingDAO>();
            foreach (StoreChecking StoreChecking in StoreCheckings)
            {
                StoreCheckingDAO StoreCheckingDAO = new StoreCheckingDAO();
                StoreCheckingDAO.Id = StoreChecking.Id;
                StoreCheckingDAO.StoreId = StoreChecking.StoreId;
                StoreCheckingDAO.SaleEmployeeId = StoreChecking.SaleEmployeeId;
                StoreCheckingDAO.Longitude = StoreChecking.Longitude;
                StoreCheckingDAO.Latitude = StoreChecking.Latitude;
                StoreCheckingDAO.CheckOutLongitude = StoreChecking.CheckOutLongitude;
                StoreCheckingDAO.CheckOutLatitude = StoreChecking.CheckOutLatitude;
                StoreCheckingDAO.CheckInAt = StoreChecking.CheckInAt;
                StoreCheckingDAO.CheckOutAt = StoreChecking.CheckOutAt;
                StoreCheckingDAO.CheckInDistance = StoreChecking.CheckInDistance;
                StoreCheckingDAO.CheckOutDistance = StoreChecking.CheckOutDistance;
                StoreCheckingDAO.IndirectSalesOrderCounter = StoreChecking.CountIndirectSalesOrder;
                StoreCheckingDAO.ImageCounter = StoreChecking.ImageCounter;
                StoreCheckingDAO.IsOpenedStore = StoreChecking.IsOpenedStore;
                StoreCheckingDAOs.Add(StoreCheckingDAO);
            }
            await DataContext.BulkMergeAsync(StoreCheckingDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreChecking> StoreCheckings)
        {
            List<long> Ids = StoreCheckings.Select(x => x.Id).ToList();
            await DataContext.StoreChecking
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(StoreChecking StoreChecking)
        {
            if (StoreChecking.StoreCheckingImageMappings == null) StoreChecking.StoreCheckingImageMappings = new List<StoreCheckingImageMapping>();
            var LocalStoreCheckingImageMappings = await DataContext.StoreCheckingImageMapping
                .Where(x => x.StoreCheckingId == StoreChecking.Id)
                .Select(x => new StoreCheckingImageMapping
                {
                    ImageId = x.ImageId,
                    StoreCheckingId = x.StoreCheckingId,
                    Key = x.ImageId.ToString(),
                    Hash = x.ImageId.ToString()
                })
                .ToListWithNoLockAsync();
            foreach (var mapping in StoreChecking.StoreCheckingImageMappings)
            {
                mapping.Key = mapping.ImageId.ToString();
                mapping.Hash = mapping.ImageId.ToString();
            }

            var result = LocalStoreCheckingImageMappings.Split(StoreChecking.StoreCheckingImageMappings);
            var AddList = result.Item1.Select(x => x.Remote).ToList();
            var DeleteList = result.Item3.Select(x => x.Local).ToList();

            if (AddList.Any())
            {
                List<StoreCheckingImageMappingDAO> StoreCheckingImageMappingDAOs = new List<StoreCheckingImageMappingDAO>();
                foreach (StoreCheckingImageMapping StoreCheckingImageMapping in AddList)
                {
                    StoreCheckingImageMappingDAO StoreCheckingImageMappingDAO = new StoreCheckingImageMappingDAO();
                    StoreCheckingImageMappingDAO.ImageId = StoreCheckingImageMapping.ImageId;
                    StoreCheckingImageMappingDAO.StoreCheckingId = StoreChecking.Id;
                    StoreCheckingImageMappingDAO.AlbumId = StoreCheckingImageMapping.AlbumId;
                    StoreCheckingImageMappingDAO.StoreId = StoreChecking.StoreId;
                    StoreCheckingImageMappingDAO.SaleEmployeeId = StoreChecking.SaleEmployeeId;
                    StoreCheckingImageMappingDAO.ShootingAt = StoreCheckingImageMapping.ShootingAt;
                    StoreCheckingImageMappingDAO.Distance = StoreCheckingImageMapping.Distance;
                    StoreCheckingImageMappingDAOs.Add(StoreCheckingImageMappingDAO);
                }
                await DataContext.BulkMergeAsync(StoreCheckingImageMappingDAOs);
            }
            if (DeleteList.Any())
            {
                var DeletedIds = DeleteList.Select(x => x.ImageId).ToList();
                await DataContext.StoreCheckingImageMapping.WhereBulkContains(DeletedIds, x => x.ImageId).Where(x => x.StoreCheckingId == StoreChecking.Id).DeleteFromQueryAsync();
            }
        }
    }
}
