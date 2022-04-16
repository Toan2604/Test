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
    public interface IProblemRepository
    {
        Task<int> Count(ProblemFilter ProblemFilter);
        Task<int> CountAll(ProblemFilter ProblemFilter);
        Task<List<Problem>> List(ProblemFilter ProblemFilter);
        Task<List<Problem>> List(List<long> Ids);
        Task<Problem> Get(long Id);
        Task<bool> Create(Problem Problem);
        Task<bool> Update(Problem Problem);
        Task<bool> Delete(Problem Problem);
        Task<bool> BulkMerge(List<Problem> Problems);
        Task<bool> BulkDelete(List<Problem> Problems);
    }
    public class ProblemRepository : IProblemRepository
    {
        private DataContext DataContext;
        public ProblemRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProblemDAO> DynamicFilter(IQueryable<ProblemDAO> query, ProblemFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.StoreCheckingId, filter.StoreCheckingId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.CreatorId, filter.AppUserId);
            query = query.Where(q => q.ProblemTypeId, filter.ProblemTypeId);
            query = query.Where(q => q.NoteAt, filter.NoteAt);
            query = query.Where(q => q.CompletedAt, filter.CompletedAt);
            query = query.Where(q => q.Content, filter.Content);
            query = query.Where(q => q.ProblemStatusId, filter.ProblemStatusId);
            query = query.Where(q => q.Store.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.Store.DistrictId, filter.DistrictId);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLock();
                    query = query.Where(q => q.Creator.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLock();
                    query = query.Where(q => !q.Creator.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.Creator.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.Creator.OrganizationId, IdFilter);
                }
            }
            return query;
        }

        private IQueryable<ProblemDAO> OrFilter(IQueryable<ProblemDAO> query, ProblemFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProblemDAO> initQuery = query.Where(q => false);
            foreach (ProblemFilter ProblemFilter in filter.OrFilter)
            {
                IQueryable<ProblemDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProblemFilter.Id);
                queryable = queryable.Where(q => q.StoreCheckingId, ProblemFilter.StoreCheckingId);
                queryable = queryable.Where(q => q.Code, ProblemFilter.Code);
                queryable = queryable.Where(q => q.StoreId, ProblemFilter.StoreId);
                queryable = queryable.Where(q => q.CreatorId, ProblemFilter.AppUserId);
                queryable = queryable.Where(q => q.ProblemTypeId, ProblemFilter.ProblemTypeId);
                queryable = queryable.Where(q => q.NoteAt, ProblemFilter.NoteAt);
                queryable = queryable.Where(q => q.CompletedAt, ProblemFilter.CompletedAt);
                queryable = queryable.Where(q => q.Content, ProblemFilter.Content);
                queryable = queryable.Where(q => q.ProblemStatusId, ProblemFilter.ProblemStatusId);
                if (ProblemFilter.OrganizationId != null)
                {
                    if (ProblemFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == ProblemFilter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLock();
                        queryable = queryable.Where(q => q.Creator.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (ProblemFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == ProblemFilter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLock();
                        queryable = queryable.Where(q => !q.Creator.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (ProblemFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => ProblemFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.Creator.OrganizationId, IdFilter);
                    }
                    if (ProblemFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => ProblemFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.Creator.OrganizationId, IdFilter);
                    }
                }
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProblemDAO> DynamicOrder(IQueryable<ProblemDAO> query, ProblemFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProblemOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProblemOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProblemOrder.StoreChecking:
                            query = query.OrderBy(q => q.StoreCheckingId);
                            break;
                        case ProblemOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case ProblemOrder.Creator:
                            query = query.OrderBy(q => q.CreatorId);
                            break;
                        case ProblemOrder.ProblemType:
                            query = query.OrderBy(q => q.ProblemTypeId);
                            break;
                        case ProblemOrder.NoteAt:
                            query = query.OrderBy(q => q.NoteAt);
                            break;
                        case ProblemOrder.CompletedAt:
                            query = query.OrderBy(q => q.CompletedAt);
                            break;
                        case ProblemOrder.Content:
                            query = query.OrderBy(q => q.Content);
                            break;
                        case ProblemOrder.ProblemStatus:
                            query = query.OrderBy(q => q.ProblemStatusId);
                            break;
                        case ProblemOrder.Organization:
                            query = query.OrderBy(q => q.Creator.OrganizationId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProblemOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProblemOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProblemOrder.StoreChecking:
                            query = query.OrderByDescending(q => q.StoreCheckingId);
                            break;
                        case ProblemOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case ProblemOrder.Creator:
                            query = query.OrderByDescending(q => q.CreatorId);
                            break;
                        case ProblemOrder.ProblemType:
                            query = query.OrderByDescending(q => q.ProblemTypeId);
                            break;
                        case ProblemOrder.NoteAt:
                            query = query.OrderByDescending(q => q.NoteAt);
                            break;
                        case ProblemOrder.CompletedAt:
                            query = query.OrderByDescending(q => q.CompletedAt);
                            break;
                        case ProblemOrder.Content:
                            query = query.OrderByDescending(q => q.Content);
                            break;
                        case ProblemOrder.ProblemStatus:
                            query = query.OrderByDescending(q => q.ProblemStatusId);
                            break;
                        case ProblemOrder.Organization:
                            query = query.OrderByDescending(q => q.Creator.OrganizationId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Problem>> DynamicSelect(IQueryable<ProblemDAO> query, ProblemFilter filter)
        {
            List<Problem> Problems = await query.Select(q => new Problem()
            {
                Id = filter.Selects.Contains(ProblemSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProblemSelect.Code) ? q.Code : default(string),
                StoreCheckingId = filter.Selects.Contains(ProblemSelect.StoreChecking) ? q.StoreCheckingId : null,
                StoreId = filter.Selects.Contains(ProblemSelect.Store) ? q.StoreId : default(long),
                CreatorId = filter.Selects.Contains(ProblemSelect.Creator) ? q.CreatorId : default(long),
                ProblemTypeId = filter.Selects.Contains(ProblemSelect.ProblemType) ? q.ProblemTypeId : default(long),
                NoteAt = filter.Selects.Contains(ProblemSelect.NoteAt) ? q.NoteAt : default(DateTime),
                CompletedAt = filter.Selects.Contains(ProblemSelect.CompletedAt) ? q.CompletedAt : null,
                Content = filter.Selects.Contains(ProblemSelect.Content) ? q.Content : default(string),
                ProblemStatusId = filter.Selects.Contains(ProblemSelect.ProblemStatus) ? q.ProblemStatusId : default(long),
                Creator = filter.Selects.Contains(ProblemSelect.Creator) && q.Creator != null ? new AppUser
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
                        Name = q.Creator.Organization.Name,
                        Email = q.Creator.Organization.Email,
                        Level = q.Creator.Organization.Level,
                        Phone = q.Creator.Organization.Phone,
                        ParentId = q.Creator.Organization.ParentId,
                        StatusId = q.Creator.Organization.StatusId,
                        Address = q.Creator.Organization.Address,
                    }
                } : null,
                ProblemStatus = filter.Selects.Contains(ProblemSelect.ProblemStatus) && q.ProblemStatus != null ? new ProblemStatus
                {
                    Id = q.ProblemStatus.Id,
                    Code = q.ProblemStatus.Code,
                    Name = q.ProblemStatus.Name,
                } : null,
                ProblemType = filter.Selects.Contains(ProblemSelect.ProblemType) && q.ProblemType != null ? new ProblemType
                {
                    Id = q.ProblemType.Id,
                    Code = q.ProblemType.Code,
                    Name = q.ProblemType.Name,
                } : null,
                Store = filter.Selects.Contains(ProblemSelect.Store) && q.Store != null ? new Store
                {
                    Id = q.Store.Id,
                    Code = q.Store.Code,
                    CodeDraft = q.Store.CodeDraft,
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
                    DeliveryLatitude = q.Store.DeliveryLatitude,
                    DeliveryLongitude = q.Store.DeliveryLongitude,
                    OwnerName = q.Store.OwnerName,
                    OwnerPhone = q.Store.OwnerPhone,
                    OwnerEmail = q.Store.OwnerEmail,
                    TaxCode = q.Store.TaxCode,
                    LegalEntity = q.Store.LegalEntity,
                    StatusId = q.Store.StatusId,
                } : null,
                StoreChecking = filter.Selects.Contains(ProblemSelect.StoreChecking) && q.StoreChecking != null ? new StoreChecking
                {
                    Id = q.StoreChecking.Id,
                    StoreId = q.StoreChecking.StoreId,
                    SaleEmployeeId = q.StoreChecking.SaleEmployeeId,
                    Longitude = q.StoreChecking.Longitude,
                    Latitude = q.StoreChecking.Latitude,
                    CheckInAt = q.StoreChecking.CheckInAt,
                    CheckOutAt = q.StoreChecking.CheckOutAt,
                } : null,
                RowId = q.RowId,
            }).ToListWithNoLockAsync();
            return Problems;
        }

        public async Task<int> Count(ProblemFilter filter)
        {
            IQueryable<ProblemDAO> Problems = DataContext.Problem.AsNoTracking();
            Problems = DynamicFilter(Problems, filter);
            Problems = OrFilter(Problems, filter);
            return await Problems.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ProblemFilter filter)
        {
            IQueryable<ProblemDAO> Problems = DataContext.Problem.AsNoTracking();
            Problems = DynamicFilter(Problems, filter);
            return await Problems.CountWithNoLockAsync();
        }

        public async Task<List<Problem>> List(ProblemFilter filter)
        {
            if (filter == null) return new List<Problem>();
            IQueryable<ProblemDAO> ProblemDAOs = DataContext.Problem.AsNoTracking();
            ProblemDAOs = DynamicFilter(ProblemDAOs, filter);
            ProblemDAOs = OrFilter(ProblemDAOs, filter);
            ProblemDAOs = DynamicOrder(ProblemDAOs, filter);
            List<Problem> Problems = await DynamicSelect(ProblemDAOs, filter);
            return Problems;
        }

        public async Task<List<Problem>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
          
            List<Problem> Problems = await DataContext.Problem.AsNoTracking()
                .Where(x => x.Id, IdFilter)
                .Select(x => new Problem()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ProblemStatusId = x.ProblemStatusId,
                    ProblemTypeId = x.ProblemTypeId,
                    CreatorId = x.CreatorId,
                    NoteAt = x.NoteAt,
                    CompletedAt = x.CompletedAt,
                    Content = x.Content,
                    StoreCheckingId = x.StoreCheckingId,
                    StoreId = x.StoreId,
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        OrganizationId = x.Creator.OrganizationId,
                        DisplayName = x.Creator.DisplayName,
                        Address = x.Creator.Address,
                        Email = x.Creator.Email,
                        Phone = x.Creator.Phone,
                    },
                    ProblemStatus = x.ProblemStatus == null ? null : new ProblemStatus
                    {
                        Id = x.ProblemStatus.Id,
                        Code = x.ProblemStatus.Code,
                        Name = x.ProblemStatus.Name,
                    },
                    ProblemType = x.ProblemType == null ? null : new ProblemType
                    {
                        Id = x.ProblemType.Id,
                        Code = x.ProblemType.Code,
                        Name = x.ProblemType.Name,
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
                    },
                    StoreChecking = x.StoreChecking == null ? null : new StoreChecking
                    {
                        Id = x.StoreChecking.Id,
                        StoreId = x.StoreChecking.StoreId,
                        SaleEmployeeId = x.StoreChecking.SaleEmployeeId,
                        OrganizationId = x.StoreChecking.OrganizationId,
                        DeviceName = x.StoreChecking.DeviceName,
                        Longitude = x.StoreChecking.Longitude,
                        Latitude = x.StoreChecking.Latitude,
                        CheckOutLongitude = x.StoreChecking.CheckOutLongitude,
                        CheckOutLatitude = x.StoreChecking.CheckOutLatitude,
                        CheckInAt = x.StoreChecking.CheckInAt,
                        CheckOutAt = x.StoreChecking.CheckOutAt,
                        CheckInDistance = x.StoreChecking.CheckInDistance,
                        CheckOutDistance = x.StoreChecking.CheckOutDistance,
                        CountIndirectSalesOrder = x.StoreChecking.IndirectSalesOrderCounter,
                        ImageCounter = x.StoreChecking.ImageCounter,
                        IsOpenedStore = x.StoreChecking.IsOpenedStore,
                        Planned = x.StoreChecking.Planned,
                    },
                }).ToListWithNoLockAsync();
            List<ProblemImageMapping> ProblemImageMappings = await DataContext.ProblemImageMapping.AsNoTracking()
               .Where(x => x.ProblemId, IdFilter)
               .Select(x => new ProblemImageMapping()
               {
                   ProblemId = x.ProblemId,
                   ImageId = x.ImageId,
                   Image = new Image
                   {
                       Id = x.Image.Id,
                       Name = x.Image.Name,
                       Url = x.Image.Url,
                       ThumbnailUrl = x.Image.ThumbnailUrl,
                   },
               }).ToListWithNoLockAsync();
            foreach (Problem Problem in Problems)
            {
                Problem.ProblemImageMappings = ProblemImageMappings
                    .Where(x => x.ProblemId == Problem.Id)
                    .ToList();
            }
            return Problems;
        }

        public async Task<Problem> Get(long Id)
        {
            Problem Problem = await DataContext.Problem.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new Problem()
            {
                Id = x.Id,
                Code = x.Code,
                StoreCheckingId = x.StoreCheckingId,
                StoreId = x.StoreId,
                CreatorId = x.CreatorId,
                ProblemTypeId = x.ProblemTypeId,
                NoteAt = x.NoteAt,
                CompletedAt = x.CompletedAt,
                Content = x.Content,
                ProblemStatusId = x.ProblemStatusId,
                RowId = x.RowId,
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
                    Organization = x.Creator.Organization == null ? null : new Organization
                    {
                        Id = x.Creator.Organization.Id,
                        Code = x.Creator.Organization.Code,
                        Name = x.Creator.Organization.Name,
                        Email = x.Creator.Organization.Email,
                        Level = x.Creator.Organization.Level,
                        Phone = x.Creator.Organization.Phone,
                        ParentId = x.Creator.Organization.ParentId,
                        StatusId = x.Creator.Organization.StatusId,
                        Address = x.Creator.Organization.Address,
                    }
                },
                ProblemStatus = x.ProblemStatus == null ? null : new ProblemStatus
                {
                    Id = x.ProblemStatus.Id,
                    Code = x.ProblemStatus.Code,
                    Name = x.ProblemStatus.Name,
                },
                ProblemType = x.ProblemType == null ? null : new ProblemType
                {
                    Id = x.ProblemType.Id,
                    Code = x.ProblemType.Code,
                    Name = x.ProblemType.Name,
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
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    StatusId = x.Store.StatusId,
                },
                StoreChecking = x.StoreChecking == null ? null : new StoreChecking
                {
                    Id = x.StoreChecking.Id,
                    StoreId = x.StoreChecking.StoreId,
                    SaleEmployeeId = x.StoreChecking.SaleEmployeeId,
                    Longitude = x.StoreChecking.Longitude,
                    Latitude = x.StoreChecking.Latitude,
                    CheckInAt = x.StoreChecking.CheckInAt,
                    CheckOutAt = x.StoreChecking.CheckOutAt,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Problem == null)
                return null;
            Problem.ProblemImageMappings = await DataContext.ProblemImageMapping.AsNoTracking()
                .Where(x => x.ProblemId == Problem.Id)
                .Where(x => x.Image.DeletedAt == null)
                .Select(x => new ProblemImageMapping
                {
                    ProblemId = x.ProblemId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                }).ToListWithNoLockAsync();
            Problem.ProblemHistories = await DataContext.ProblemHistory.AsNoTracking()
                .Where(x => x.ProblemId == Problem.Id)
                .Select(x => new ProblemHistory
                {
                    Id = x.Id,
                    ModifierId = x.ModifierId,
                    ProblemId = x.ProblemId,
                    ProblemStatusId = x.ProblemStatusId,
                    Time = x.Time,
                    ProblemStatus = x.ProblemStatus == null ? null : new ProblemStatus
                    {
                        Id = x.ProblemStatus.Id,
                        Code = x.ProblemStatus.Code,
                        Name = x.ProblemStatus.Name,
                    },
                    Modifier = x.Modifier == null ? null : new AppUser
                    {
                        Id = x.Modifier.Id,
                        Username = x.Modifier.Username,
                        DisplayName = x.Modifier.DisplayName,
                    }
                }).ToListWithNoLockAsync();
            return Problem;
        }
        public async Task<bool> Create(Problem Problem)
        {
            ProblemDAO ProblemDAO = new ProblemDAO();
            ProblemDAO.Id = Problem.Id;
            ProblemDAO.Code = Problem.Code;
            ProblemDAO.StoreCheckingId = Problem.StoreCheckingId;
            ProblemDAO.StoreId = Problem.StoreId;
            ProblemDAO.CreatorId = Problem.CreatorId;
            ProblemDAO.ProblemTypeId = Problem.ProblemTypeId;
            ProblemDAO.NoteAt = StaticParams.DateTimeNow;
            ProblemDAO.CompletedAt = Problem.CompletedAt;
            ProblemDAO.Content = Problem.Content;
            ProblemDAO.ProblemStatusId = Enums.ProblemStatusEnum.WAITING.Id;
            ProblemDAO.RowId = Guid.NewGuid();
            DataContext.Problem.Add(ProblemDAO);
            await DataContext.SaveChangesAsync();
            Problem.Id = ProblemDAO.Id;
            await SaveReference(Problem);
            return true;
        }

        public async Task<bool> Update(Problem Problem)
        {
            ProblemDAO ProblemDAO = DataContext.Problem.Where(x => x.Id == Problem.Id).FirstOrDefault();
            if (ProblemDAO == null)
                return false;
            ProblemDAO.Id = Problem.Id;
            ProblemDAO.Code = Problem.Code;
            ProblemDAO.StoreCheckingId = Problem.StoreCheckingId;
            ProblemDAO.StoreId = Problem.StoreId;
            ProblemDAO.CreatorId = Problem.CreatorId;
            ProblemDAO.ProblemTypeId = Problem.ProblemTypeId;
            ProblemDAO.CompletedAt = Problem.CompletedAt;
            ProblemDAO.Content = Problem.Content;
            ProblemDAO.ProblemStatusId = Problem.ProblemStatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(Problem);
            return true;
        }

        public async Task<bool> Delete(Problem Problem)
        {
            await DataContext.ProblemHistory.Where(x => x.ProblemId == Problem.Id).DeleteFromQueryAsync();
            await DataContext.ProblemImageMapping.Where(x => x.ProblemId == Problem.Id).DeleteFromQueryAsync();
            await DataContext.Problem.Where(x => x.Id == Problem.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<Problem> Problems)
        {
            List<ProblemDAO> ProblemDAOs = new List<ProblemDAO>();
            foreach (Problem Problem in Problems)
            {
                ProblemDAO ProblemDAO = new ProblemDAO();
                ProblemDAO.Id = Problem.Id;
                ProblemDAO.Code = Problem.Code;
                ProblemDAO.StoreCheckingId = Problem.StoreCheckingId;
                ProblemDAO.StoreId = Problem.StoreId;
                ProblemDAO.CreatorId = Problem.CreatorId;
                ProblemDAO.ProblemTypeId = Problem.ProblemTypeId;
                ProblemDAO.NoteAt = Problem.NoteAt;
                ProblemDAO.CompletedAt = Problem.CompletedAt;
                ProblemDAO.Content = Problem.Content;
                ProblemDAO.ProblemStatusId = Problem.ProblemStatusId;
                ProblemDAOs.Add(ProblemDAO);
            }
            await DataContext.BulkMergeAsync(ProblemDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<Problem> Problems)
        {
            List<long> Ids = Problems.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DataContext.ProblemHistory
                .Where(x => Ids.Contains(x.ProblemId))
                .DeleteFromQueryAsync();
            await DataContext.ProblemImageMapping
                .Where(x => Ids.Contains(x.ProblemId))
                .DeleteFromQueryAsync();
            await DataContext.Problem
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(Problem Problem)
        {
            await DataContext.ProblemImageMapping
                .Where(x => x.ProblemId == Problem.Id)
                .DeleteFromQueryAsync();
            List<ProblemImageMappingDAO> ProblemImageMappingDAOs = new List<ProblemImageMappingDAO>();
            if (Problem.ProblemImageMappings != null)
            {
                foreach (ProblemImageMapping ProblemImageMapping in Problem.ProblemImageMappings)
                {
                    ProblemImageMappingDAO ProblemImageMappingDAO = new ProblemImageMappingDAO();
                    ProblemImageMappingDAO.ProblemId = Problem.Id;
                    ProblemImageMappingDAO.ImageId = ProblemImageMapping.ImageId;
                    ProblemImageMappingDAOs.Add(ProblemImageMappingDAO);
                }
                await DataContext.ProblemImageMapping.BulkMergeAsync(ProblemImageMappingDAOs);
            }

            await DataContext.ProblemHistory.Where(x => x.ProblemId == Problem.Id).DeleteFromQueryAsync();

            List<ProblemHistoryDAO> ProblemHistoryDAOs = new List<ProblemHistoryDAO>();
            if (Problem.ProblemHistories != null)
            {
                foreach (ProblemHistory ProblemHistory in Problem.ProblemHistories)
                {
                    ProblemHistoryDAO ProblemHistoryDAO = new ProblemHistoryDAO();
                    ProblemHistoryDAO.ProblemId = Problem.Id;
                    ProblemHistoryDAO.ModifierId = ProblemHistory.ModifierId;
                    ProblemHistoryDAO.ProblemStatusId = ProblemHistory.ProblemStatusId;
                    ProblemHistoryDAO.Time = ProblemHistory.Time;
                    ProblemHistoryDAOs.Add(ProblemHistoryDAO);
                }
                await DataContext.ProblemHistory.BulkMergeAsync(ProblemHistoryDAOs);
            }
        }

    }
}
