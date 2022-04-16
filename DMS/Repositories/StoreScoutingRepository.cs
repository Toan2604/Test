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
    public interface IStoreScoutingRepository
    {
        Task<int> Count(StoreScoutingFilter StoreScoutingFilter);
        Task<int> CountAll(StoreScoutingFilter StoreScoutingFilter);
        Task<List<StoreScouting>> List(StoreScoutingFilter StoreScoutingFilter);
        Task<List<StoreScouting>> List(List<long> Ids);
        Task<StoreScouting> Get(long Id);
        Task<bool> Create(StoreScouting StoreScouting);
        Task<bool> Update(StoreScouting StoreScouting);
        Task<bool> Delete(StoreScouting StoreScouting);
        Task<bool> BulkMerge(List<StoreScouting> StoreScoutings);
        Task<bool> BulkDelete(List<StoreScouting> StoreScoutings);
    }
    public class StoreScoutingRepository : IStoreScoutingRepository
    {
        private DataContext DataContext;
        public StoreScoutingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreScoutingDAO> DynamicFilter(IQueryable<StoreScoutingDAO> query, StoreScoutingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.OwnerPhone, filter.OwnerPhone);
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.WardId, filter.WardId);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
            }
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.Latitude, filter.Latitude);
            query = query.Where(q => q.Longitude, filter.Longitude);
            query = query.Where(q => q.CreatorId, filter.AppUserId);
            query = query.Where(q => q.StoreScoutingStatusId, filter.StoreScoutingStatusId);
            query = query.Where(q => q.StoreScoutingTypeId, filter.StoreScoutingTypeId);
            return query;
        }

        private IQueryable<StoreScoutingDAO> OrFilter(IQueryable<StoreScoutingDAO> query, StoreScoutingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreScoutingDAO> initQuery = query.Where(q => false);
            foreach (StoreScoutingFilter StoreScoutingFilter in filter.OrFilter)
            {
                IQueryable<StoreScoutingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreScoutingFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreScoutingFilter.Code);
                queryable = queryable.Where(q => q.Name, StoreScoutingFilter.Name);
                queryable = queryable.Where(q => q.OwnerPhone, StoreScoutingFilter.OwnerPhone);
                queryable = queryable.Where(q => q.ProvinceId, StoreScoutingFilter.ProvinceId);
                queryable = queryable.Where(q => q.DistrictId, StoreScoutingFilter.DistrictId);
                queryable = queryable.Where(q => q.WardId, StoreScoutingFilter.WardId);
                if (StoreScoutingFilter.OrganizationId != null)
                {
                    if (StoreScoutingFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == StoreScoutingFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreScoutingFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == StoreScoutingFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreScoutingFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreScoutingFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                    if (StoreScoutingFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreScoutingFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.Address, StoreScoutingFilter.Address);
                queryable = queryable.Where(q => q.Latitude, StoreScoutingFilter.Latitude);
                queryable = queryable.Where(q => q.Longitude, StoreScoutingFilter.Longitude);
                queryable = queryable.Where(q => q.CreatorId, StoreScoutingFilter.AppUserId);
                queryable = queryable.Where(q => q.StoreScoutingStatusId, StoreScoutingFilter.StoreScoutingStatusId);
                queryable = queryable.Where(q => q.StoreScoutingTypeId, StoreScoutingFilter.StoreScoutingTypeId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreScoutingDAO> DynamicOrder(IQueryable<StoreScoutingDAO> query, StoreScoutingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreScoutingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreScoutingOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreScoutingOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case StoreScoutingOrder.OwnerPhone:
                            query = query.OrderBy(q => q.OwnerPhone);
                            break;
                        case StoreScoutingOrder.Province:
                            query = query.OrderBy(q => q.ProvinceId);
                            break;
                        case StoreScoutingOrder.District:
                            query = query.OrderBy(q => q.DistrictId);
                            break;
                        case StoreScoutingOrder.Ward:
                            query = query.OrderBy(q => q.WardId);
                            break;
                        case StoreScoutingOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case StoreScoutingOrder.Latitude:
                            query = query.OrderBy(q => q.Latitude);
                            break;
                        case StoreScoutingOrder.Longitude:
                            query = query.OrderBy(q => q.Longitude);
                            break;
                        case StoreScoutingOrder.Creator:
                            query = query.OrderBy(q => q.Creator.DisplayName);
                            break;
                        case StoreScoutingOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case StoreScoutingOrder.StoreScoutingStatus:
                            query = query.OrderBy(q => q.StoreScoutingStatusId);
                            break;
                        case StoreScoutingOrder.StoreScoutingType:
                            query = query.OrderBy(q => q.StoreScoutingTypeId);
                            break;
                        case StoreScoutingOrder.CreatedAt:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreScoutingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreScoutingOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreScoutingOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case StoreScoutingOrder.OwnerPhone:
                            query = query.OrderByDescending(q => q.OwnerPhone);
                            break;
                        case StoreScoutingOrder.Province:
                            query = query.OrderByDescending(q => q.ProvinceId);
                            break;
                        case StoreScoutingOrder.District:
                            query = query.OrderByDescending(q => q.DistrictId);
                            break;
                        case StoreScoutingOrder.Ward:
                            query = query.OrderByDescending(q => q.WardId);
                            break;
                        case StoreScoutingOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case StoreScoutingOrder.Latitude:
                            query = query.OrderByDescending(q => q.Latitude);
                            break;
                        case StoreScoutingOrder.Longitude:
                            query = query.OrderByDescending(q => q.Longitude);
                            break;
                        case StoreScoutingOrder.Creator:
                            query = query.OrderByDescending(q => q.Creator.DisplayName);
                            break;
                        case StoreScoutingOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case StoreScoutingOrder.StoreScoutingStatus:
                            query = query.OrderByDescending(q => q.StoreScoutingStatusId);
                            break;
                        case StoreScoutingOrder.StoreScoutingType:
                            query = query.OrderByDescending(q => q.StoreScoutingTypeId);
                            break;
                        case StoreScoutingOrder.CreatedAt:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreScouting>> DynamicSelect(IQueryable<StoreScoutingDAO> query, StoreScoutingFilter filter)
        {
            List<StoreScouting> StoreScoutings = await query.Select(q => new StoreScouting()
            {
                Id = filter.Selects.Contains(StoreScoutingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreScoutingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreScoutingSelect.Name) ? q.Name : default(string),
                OwnerPhone = filter.Selects.Contains(StoreScoutingSelect.OwnerPhone) ? q.OwnerPhone : default(string),
                ProvinceId = filter.Selects.Contains(StoreScoutingSelect.Province) ? q.ProvinceId : default(long?),
                DistrictId = filter.Selects.Contains(StoreScoutingSelect.District) ? q.DistrictId : default(long?),
                WardId = filter.Selects.Contains(StoreScoutingSelect.Ward) ? q.WardId : default(long?),
                Address = filter.Selects.Contains(StoreScoutingSelect.Address) ? q.Address : default(string),
                Latitude = filter.Selects.Contains(StoreScoutingSelect.Latitude) ? q.Latitude : default(decimal),
                Longitude = filter.Selects.Contains(StoreScoutingSelect.Longitude) ? q.Longitude : default(decimal),
                CreatorId = filter.Selects.Contains(StoreScoutingSelect.Creator) ? q.CreatorId : default(long),
                OrganizationId = filter.Selects.Contains(StoreScoutingSelect.Organization) ? q.OrganizationId : default(long),
                StoreScoutingStatusId = filter.Selects.Contains(StoreScoutingSelect.StoreScoutingStatus) ? q.StoreScoutingStatusId : default(long),
                StoreScoutingTypeId = filter.Selects.Contains(StoreScoutingSelect.StoreScoutingType) ? q.StoreScoutingTypeId : default(long),
                Description = filter.Selects.Contains(StoreScoutingSelect.Description) ? q.Description : null,
                Creator = filter.Selects.Contains(StoreScoutingSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    DisplayName = q.Creator.DisplayName,
                    Address = q.Creator.Address,
                    Email = q.Creator.Email,
                    Phone = q.Creator.Phone,
                    PositionId = q.Creator.PositionId,
                    Department = q.Creator.Department,
                    OrganizationId = q.Creator.OrganizationId,
                    StatusId = q.Creator.StatusId,
                    Avatar = q.Creator.Avatar,
                    ProvinceId = q.Creator.ProvinceId,
                    SexId = q.Creator.SexId,
                    Birthday = q.Creator.Birthday,
                    Organization = q.Creator.Organization == null ? null : new Organization
                    {
                        Id = q.Creator.Organization.Id,
                        Code = q.Creator.Organization.Code,
                        Name = q.Creator.Organization.Name
                    }
                } : null,
                Organization = filter.Selects.Contains(StoreScoutingSelect.Organization) && q.Organization != null ? new Organization
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
                District = filter.Selects.Contains(StoreScoutingSelect.District) && q.District != null ? new District
                {
                    Id = q.District.Id,
                    Code = q.District.Code,
                    Name = q.District.Name,
                    Priority = q.District.Priority,
                    ProvinceId = q.District.ProvinceId,
                    StatusId = q.District.StatusId,
                } : null,
                Province = filter.Selects.Contains(StoreScoutingSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                    Priority = q.Province.Priority,
                    StatusId = q.Province.StatusId,
                } : null,
                StoreScoutingStatus = filter.Selects.Contains(StoreScoutingSelect.StoreScoutingStatus) && q.StoreScoutingStatus != null ? new StoreScoutingStatus
                {
                    Id = q.StoreScoutingStatus.Id,
                    Code = q.StoreScoutingStatus.Code,
                    Name = q.StoreScoutingStatus.Name,
                } : null,
                StoreScoutingType = filter.Selects.Contains(StoreScoutingSelect.StoreScoutingType) && q.StoreScoutingType != null ? new StoreScoutingType
                {
                    Id = q.StoreScoutingType.Id,
                    Code = q.StoreScoutingType.Code,
                    Name = q.StoreScoutingType.Name,
                } : null,
                Ward = filter.Selects.Contains(StoreScoutingSelect.Ward) && q.Ward != null ? new Ward
                {
                    Id = q.Ward.Id,
                    Code = q.Ward.Code,
                    Name = q.Ward.Name,
                    Priority = q.Ward.Priority,
                    DistrictId = q.Ward.DistrictId,
                    StatusId = q.Ward.StatusId,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                RowId = q.RowId,
            }).ToListWithNoLockAsync();
            return StoreScoutings;
        }

        public async Task<int> Count(StoreScoutingFilter filter)
        {
            IQueryable<StoreScoutingDAO> StoreScoutings = DataContext.StoreScouting.AsNoTracking();
            StoreScoutings = DynamicFilter(StoreScoutings, filter);
            StoreScoutings = OrFilter(StoreScoutings, filter);
            return await StoreScoutings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreScoutingFilter filter)
        {
            IQueryable<StoreScoutingDAO> StoreScoutings = DataContext.StoreScouting.AsNoTracking();
            StoreScoutings = DynamicFilter(StoreScoutings, filter);
            return await StoreScoutings.CountWithNoLockAsync();
        }

        public async Task<List<StoreScouting>> List(StoreScoutingFilter filter)
        {
            if (filter == null) return new List<StoreScouting>();
            IQueryable<StoreScoutingDAO> StoreScoutingDAOs = DataContext.StoreScouting.AsNoTracking();
            StoreScoutingDAOs = DynamicFilter(StoreScoutingDAOs, filter);
            StoreScoutingDAOs = OrFilter(StoreScoutingDAOs, filter);
            StoreScoutingDAOs = DynamicOrder(StoreScoutingDAOs, filter);
            List<StoreScouting> StoreScoutings = await DynamicSelect(StoreScoutingDAOs, filter);
            return StoreScoutings;
        }

        public async Task<List<StoreScouting>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            List<StoreScouting> StoreScoutings = await DataContext.StoreScouting.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new StoreScouting()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                OwnerPhone = x.OwnerPhone,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                Address = x.Address,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                CreatorId = x.CreatorId,
                OrganizationId = x.OrganizationId,
                StoreScoutingStatusId = x.StoreScoutingStatusId,
                StoreScoutingTypeId = x.StoreScoutingTypeId,
                RowId = x.RowId,
                Description = x.Description,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    Username = x.Creator.Username,
                    DisplayName = x.Creator.DisplayName,
                    Address = x.Creator.Address,
                    Email = x.Creator.Email,
                    Phone = x.Creator.Phone,
                    PositionId = x.Creator.PositionId,
                    Department = x.Creator.Department,
                    OrganizationId = x.Creator.OrganizationId,
                    StatusId = x.Creator.StatusId,
                    Avatar = x.Creator.Avatar,
                    ProvinceId = x.Creator.ProvinceId,
                    SexId = x.Creator.SexId,
                    Birthday = x.Creator.Birthday,
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
                District = x.District == null ? null : new District
                {
                    Id = x.District.Id,
                    Code = x.District.Code,
                    Name = x.District.Name,
                    Priority = x.District.Priority,
                    ProvinceId = x.District.ProvinceId,
                    StatusId = x.District.StatusId,
                },
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                },
                StoreScoutingStatus = x.StoreScoutingStatus == null ? null : new StoreScoutingStatus
                {
                    Id = x.StoreScoutingStatus.Id,
                    Code = x.StoreScoutingStatus.Code,
                    Name = x.StoreScoutingStatus.Name,
                },
                StoreScoutingType = x.StoreScoutingType == null ? null : new StoreScoutingType
                {
                    Id = x.StoreScoutingType.Id,
                    Code = x.StoreScoutingType.Code,
                    Name = x.StoreScoutingType.Name,
                },
                Ward = x.Ward == null ? null : new Ward
                {
                    Id = x.Ward.Id,
                    Code = x.Ward.Code,
                    Name = x.Ward.Name,
                    Priority = x.Ward.Priority,
                    DistrictId = x.Ward.DistrictId,
                    StatusId = x.Ward.StatusId,
                },
            }).ToListWithNoLockAsync();

            List<Store> Stores = await DataContext.Store
                .Where(x => x.StoreScoutingId.HasValue)
                .Where(x => x.StoreScoutingId, IdFilter)
                .Select(x => new Store
                {
                    Id = x.Id,
                    Code = x.Code,
                    CodeDraft = x.CodeDraft,
                    Name = x.Name,
                    ParentStoreId = x.ParentStoreId,
                    OrganizationId = x.OrganizationId,
                    StoreTypeId = x.StoreTypeId,
                    Telephone = x.Telephone,
                    ProvinceId = x.ProvinceId,
                    DistrictId = x.DistrictId,
                    WardId = x.WardId,
                    Address = x.Address,
                    DeliveryAddress = x.DeliveryAddress,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DeliveryLatitude = x.DeliveryLatitude,
                    DeliveryLongitude = x.DeliveryLongitude,
                    OwnerName = x.OwnerName,
                    OwnerPhone = x.OwnerPhone,
                    OwnerEmail = x.OwnerEmail,
                    TaxCode = x.TaxCode,
                    LegalEntity = x.LegalEntity,
                    StatusId = x.StatusId,
                    RowId = x.RowId,
                    Used = x.Used,
                }).ToListWithNoLockAsync();
            List<StoreScoutingImageMapping> StoreScoutingImageMappings = await DataContext.StoreScoutingImageMapping
                .Where(x => x.StoreScoutingId, IdFilter)
                .Select(x => new StoreScoutingImageMapping
                {
                    ImageId = x.ImageId,
                    StoreScoutingId = x.StoreScoutingId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                }).ToListWithNoLockAsync();
            foreach (StoreScouting StoreScouting in StoreScoutings)
            {
                StoreScouting.Store = Stores
                    .Where(x => x.StoreScoutingId == StoreScouting.Id)
                    .FirstOrDefault();
                StoreScouting.StoreScoutingImageMappings = StoreScoutingImageMappings
                    .Where(x => x.StoreScoutingId == StoreScouting.Id)
                    .ToList();
            }
            return StoreScoutings;
        }

        public async Task<StoreScouting> Get(long Id)
        {
            StoreScouting StoreScouting = await DataContext.StoreScouting.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new StoreScouting()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                OwnerPhone = x.OwnerPhone,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                Address = x.Address,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                CreatorId = x.CreatorId,
                OrganizationId = x.OrganizationId,
                StoreScoutingStatusId = x.StoreScoutingStatusId,
                StoreScoutingTypeId = x.StoreScoutingTypeId,
                RowId = x.RowId,
                Description = x.Description,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    Username = x.Creator.Username,
                    DisplayName = x.Creator.DisplayName,
                    Address = x.Creator.Address,
                    Email = x.Creator.Email,
                    Phone = x.Creator.Phone,
                    PositionId = x.Creator.PositionId,
                    Department = x.Creator.Department,
                    OrganizationId = x.Creator.OrganizationId,
                    StatusId = x.Creator.StatusId,
                    Avatar = x.Creator.Avatar,
                    ProvinceId = x.Creator.ProvinceId,
                    SexId = x.Creator.SexId,
                    Birthday = x.Creator.Birthday,
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
                District = x.District == null ? null : new District
                {
                    Id = x.District.Id,
                    Code = x.District.Code,
                    Name = x.District.Name,
                    Priority = x.District.Priority,
                    ProvinceId = x.District.ProvinceId,
                    StatusId = x.District.StatusId,
                },
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                },
                StoreScoutingStatus = x.StoreScoutingStatus == null ? null : new StoreScoutingStatus
                {
                    Id = x.StoreScoutingStatus.Id,
                    Code = x.StoreScoutingStatus.Code,
                    Name = x.StoreScoutingStatus.Name,
                },
                StoreScoutingType = x.StoreScoutingType == null ? null : new StoreScoutingType
                {
                    Id = x.StoreScoutingType.Id,
                    Code = x.StoreScoutingType.Code,
                    Name = x.StoreScoutingType.Name,
                },
                Ward = x.Ward == null ? null : new Ward
                {
                    Id = x.Ward.Id,
                    Code = x.Ward.Code,
                    Name = x.Ward.Name,
                    Priority = x.Ward.Priority,
                    DistrictId = x.Ward.DistrictId,
                    StatusId = x.Ward.StatusId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (StoreScouting == null)
                return null;

            StoreScouting.Store = await DataContext.Store.Where(x => x.StoreScoutingId.HasValue && x.StoreScoutingId == Id).Select(x => new Store
            {
                Id = x.Id,
                Code = x.Code,
                CodeDraft = x.CodeDraft,
                Name = x.Name,
                ParentStoreId = x.ParentStoreId,
                OrganizationId = x.OrganizationId,
                StoreTypeId = x.StoreTypeId,
                Telephone = x.Telephone,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                Address = x.Address,
                DeliveryAddress = x.DeliveryAddress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                DeliveryLatitude = x.DeliveryLatitude,
                DeliveryLongitude = x.DeliveryLongitude,
                OwnerName = x.OwnerName,
                OwnerPhone = x.OwnerPhone,
                OwnerEmail = x.OwnerEmail,
                TaxCode = x.TaxCode,
                LegalEntity = x.LegalEntity,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
            }).FirstOrDefaultWithNoLockAsync();
            if (StoreScouting.Store != null)
            {
                StoreScouting.StoreId = StoreScouting.Store.Id;
            }

            StoreScouting.StoreScoutingImageMappings = await DataContext.StoreScoutingImageMapping
                .Where(x => x.StoreScoutingId == Id).Select(x => new StoreScoutingImageMapping
                {
                    StoreScoutingId = x.StoreScoutingId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToListWithNoLockAsync();
            return StoreScouting;
        }
        public async Task<bool> Create(StoreScouting StoreScouting)
        {
            StoreScoutingDAO StoreScoutingDAO = new StoreScoutingDAO();
            StoreScoutingDAO.Id = StoreScouting.Id;
            StoreScoutingDAO.Code = "";
            StoreScoutingDAO.Name = StoreScouting.Name;
            StoreScoutingDAO.OwnerPhone = StoreScouting.OwnerPhone;
            StoreScoutingDAO.ProvinceId = StoreScouting.ProvinceId;
            StoreScoutingDAO.DistrictId = StoreScouting.DistrictId;
            StoreScoutingDAO.WardId = StoreScouting.WardId;
            StoreScoutingDAO.Address = StoreScouting.Address;
            StoreScoutingDAO.Latitude = StoreScouting.Latitude;
            StoreScoutingDAO.Longitude = StoreScouting.Longitude;
            StoreScoutingDAO.CreatorId = StoreScouting.CreatorId;
            StoreScoutingDAO.OrganizationId = StoreScouting.OrganizationId;
            StoreScoutingDAO.StoreScoutingStatusId = StoreScouting.StoreScoutingStatusId;
            StoreScoutingDAO.StoreScoutingTypeId = StoreScouting.StoreScoutingTypeId;
            StoreScoutingDAO.Description = StoreScouting.Description;
            StoreScoutingDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreScoutingDAO.UpdatedAt = StaticParams.DateTimeNow;
            StoreScoutingDAO.RowId = Guid.NewGuid();
            DataContext.StoreScouting.Add(StoreScoutingDAO);
            await DataContext.SaveChangesAsync();
            StoreScouting.Id = StoreScoutingDAO.Id;
            StoreScoutingDAO.Code = StoreScoutingDAO.Id.ToString();
            await SaveReference(StoreScouting);
            return true;
        }

        public async Task<bool> Update(StoreScouting StoreScouting)
        {
            StoreScoutingDAO StoreScoutingDAO = DataContext.StoreScouting.Where(x => x.Id == StoreScouting.Id).FirstOrDefault();
            if (StoreScoutingDAO == null)
                return false;
            StoreScoutingDAO.Id = StoreScouting.Id;
            StoreScoutingDAO.Name = StoreScouting.Name;
            StoreScoutingDAO.OwnerPhone = StoreScouting.OwnerPhone;
            StoreScoutingDAO.ProvinceId = StoreScouting.ProvinceId;
            StoreScoutingDAO.DistrictId = StoreScouting.DistrictId;
            StoreScoutingDAO.WardId = StoreScouting.WardId;
            StoreScoutingDAO.Address = StoreScouting.Address;
            StoreScoutingDAO.Latitude = StoreScouting.Latitude;
            StoreScoutingDAO.Longitude = StoreScouting.Longitude;
            StoreScoutingDAO.CreatorId = StoreScouting.CreatorId;
            StoreScoutingDAO.OrganizationId = StoreScouting.OrganizationId;
            StoreScoutingDAO.StoreScoutingStatusId = StoreScouting.StoreScoutingStatusId;
            StoreScoutingDAO.StoreScoutingTypeId = StoreScouting.StoreScoutingTypeId;
            StoreScoutingDAO.Description = StoreScouting.Description;
            StoreScoutingDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(StoreScouting);
            return true;
        }

        public async Task<bool> Delete(StoreScouting StoreScouting)
        {
            await DataContext.StoreScouting.Where(x => x.Id == StoreScouting.Id).UpdateFromQueryAsync(x => new StoreScoutingDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreScouting> StoreScoutings)
        {
            List<StoreScoutingDAO> StoreScoutingDAOs = new List<StoreScoutingDAO>();
            foreach (StoreScouting StoreScouting in StoreScoutings)
            {
                StoreScoutingDAO StoreScoutingDAO = new StoreScoutingDAO();
                StoreScoutingDAO.Id = StoreScouting.Id;
                StoreScoutingDAO.Code = StoreScouting.Code;
                StoreScoutingDAO.Name = StoreScouting.Name;
                StoreScoutingDAO.OwnerPhone = StoreScouting.OwnerPhone;
                StoreScoutingDAO.ProvinceId = StoreScouting.ProvinceId;
                StoreScoutingDAO.DistrictId = StoreScouting.DistrictId;
                StoreScoutingDAO.WardId = StoreScouting.WardId;
                StoreScoutingDAO.Address = StoreScouting.Address;
                StoreScoutingDAO.Latitude = StoreScouting.Latitude;
                StoreScoutingDAO.Longitude = StoreScouting.Longitude;
                StoreScoutingDAO.CreatorId = StoreScouting.CreatorId;
                StoreScoutingDAO.OrganizationId = StoreScouting.OrganizationId;
                StoreScoutingDAO.StoreScoutingStatusId = StoreScouting.StoreScoutingStatusId;
                StoreScoutingDAO.StoreScoutingTypeId = StoreScouting.StoreScoutingTypeId;
                StoreScoutingDAO.Description = StoreScouting.Description;
                StoreScoutingDAO.CreatedAt = StaticParams.DateTimeNow;
                StoreScoutingDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreScoutingDAOs.Add(StoreScoutingDAO);
            }
            await DataContext.BulkMergeAsync(StoreScoutingDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreScouting> StoreScoutings)
        {
            List<long> Ids = StoreScoutings.Select(x => x.Id).ToList();
            await DataContext.StoreScouting
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreScoutingDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(StoreScouting StoreScouting)
        {
            if (StoreScouting.StoreScoutingImageMappings == null) StoreScouting.StoreScoutingImageMappings = new List<StoreScoutingImageMapping>();
            var oldStoreScoutingImageMappings = await DataContext.StoreScoutingImageMapping
                .Where(x => x.StoreScoutingId == StoreScouting.Id)
                .Select(x => new StoreScoutingImageMapping
                {
                    ImageId = x.ImageId,
                    StoreScoutingId = x.StoreScoutingId,
                    Key = x.ImageId.ToString(),
                    Hash = x.ImageId.ToString()
                })
                .ToListWithNoLockAsync();
            foreach (var mapping in StoreScouting.StoreScoutingImageMappings)
            {
                mapping.Key = mapping.ImageId.ToString();
                mapping.Hash = mapping.ImageId.ToString();
            }

            var result = oldStoreScoutingImageMappings.Split(StoreScouting.StoreScoutingImageMappings);
            var AddList = result.Item1.Select(x => x.Remote).ToList();
            var DeleteList = result.Item3.Select(x => x.Local).ToList();

            if (DeleteList.Any())
            {
                var DeletedIds = DeleteList.Select(x => x.ImageId).ToList();
                await DataContext.StoreScoutingImageMapping.WhereBulkContains(DeletedIds, x => x.ImageId).Where(x => x.StoreScoutingId == StoreScouting.Id).DeleteFromQueryAsync();
            }
            if (AddList.Any())
            {
                foreach (StoreScoutingImageMapping StoreScoutingImageMapping in AddList)
                {
                    StoreScoutingImageMappingDAO StoreScoutingImageMappingDAO = new StoreScoutingImageMappingDAO();
                    StoreScoutingImageMappingDAO.StoreScoutingId = StoreScouting.Id;
                    StoreScoutingImageMappingDAO.ImageId = StoreScoutingImageMapping.ImageId;
                    DataContext.StoreScoutingImageMapping.Add(StoreScoutingImageMappingDAO);
                }
                await DataContext.SaveChangesAsync();
            }
        }

    }
}
