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
    public interface ILuckyDrawRepository
    {
        Task<int> CountAll(LuckyDrawFilter LuckyDrawFilter);
        Task<int> Count(LuckyDrawFilter LuckyDrawFilter);
        Task<List<LuckyDraw>> List(LuckyDrawFilter LuckyDrawFilter);
        Task<List<LuckyDraw>> List(List<long> Ids);
        Task<LuckyDraw> Get(long Id);
        Task<bool> Create(LuckyDraw LuckyDraw);
        Task<bool> Update(LuckyDraw LuckyDraw);
        Task<bool> Delete(LuckyDraw LuckyDraw);
        Task<bool> BulkMerge(List<LuckyDraw> LuckyDraws);
        Task<bool> BulkDelete(List<LuckyDraw> LuckyDraws);
        Task<bool> Used(List<long> Ids);
    }
    public class LuckyDrawRepository : ILuckyDrawRepository
    {
        private DataContext DataContext;
        public LuckyDrawRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<LuckyDrawDAO>> DynamicFilter(IQueryable<LuckyDrawDAO> query, LuckyDrawFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.RevenuePerTurn, filter.RevenuePerTurn);
            query = query.Where(q => q.StartAt, filter.StartAt);
            query = query.Where(q => q.EndAt, filter.EndAt);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.Used, filter.Used);
            query = query.Where(q => q.AvatarImageId, filter.AvatarImageId);
            query = query.Where(q => q.ImageId, filter.ImageId);
            query = query.Where(q => q.LuckyDrawTypeId, filter.LuckyDrawTypeId);
            if (filter.OrganizationId != null && filter.OrganizationId.HasValue)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = await DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                    query = query.Where(q => OrganizationDAO.Path.StartsWith(q.Organization.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = await DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                    query = query.Where(q => OrganizationDAO.Path.StartsWith(q.Organization.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await  DataContext.Organization
                        .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                        .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
            }
            if (filter.IsInAction != null)
            {
                if (filter.IsInAction.Value == true)
                {
                    var LuckyDrawIds = DataContext.LuckyDrawWinner.Select(x => x.LuckyDrawId).ToList();
                    var Ids = LuckyDrawIds.Distinct();
                    List<long> ListId = new List<long>();
                    List<LuckyDrawStructureDAO> LuckyDrawStructureDAOs = DataContext.LuckyDrawStructure.ToList();
                    foreach (var Id in Ids)
                    {
                        var count = LuckyDrawIds.Count(x => x == Id);
                        var total = LuckyDrawStructureDAOs.Where(x => x.LuckyDrawId == Id).Sum(x => x.Quantity);
                        if (count >= total) ListId.Add(Id);
                    }
                    IdFilter IdFilter = new IdFilter { NotIn = ListId };
                    query = query.Where(q => q.Id, IdFilter);

                }
            }
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private async Task<IQueryable<LuckyDrawDAO>> OrFilter(IQueryable<LuckyDrawDAO> query, LuckyDrawFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyDrawDAO> initQuery = query.Where(q => false);
            foreach (LuckyDrawFilter LuckyDrawFilter in filter.OrFilter)
            {
                IQueryable<LuckyDrawDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyDrawFilter.Id);
                queryable = queryable.Where(q => q.Code, LuckyDrawFilter.Code);
                queryable = queryable.Where(q => q.Name, LuckyDrawFilter.Name);
                queryable = queryable.Where(q => q.RevenuePerTurn, LuckyDrawFilter.RevenuePerTurn);
                queryable = queryable.Where(q => q.StartAt, LuckyDrawFilter.StartAt);
                queryable = queryable.Where(q => q.EndAt, LuckyDrawFilter.EndAt);
                queryable = queryable.Where(q => q.Description, LuckyDrawFilter.Description);
                queryable = queryable.Where(q => q.Used, LuckyDrawFilter.Used);
                queryable = queryable.Where(q => q.AvatarImageId, LuckyDrawFilter.AvatarImageId);
                queryable = queryable.Where(q => q.ImageId, LuckyDrawFilter.ImageId);
                queryable = queryable.Where(q => q.LuckyDrawTypeId, LuckyDrawFilter.LuckyDrawTypeId);
                if (LuckyDrawFilter.OrganizationId != null && LuckyDrawFilter.OrganizationId.HasValue)
                {
                    if (LuckyDrawFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = await DataContext.Organization
                            .Where(o => o.Id == LuckyDrawFilter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (LuckyDrawFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = await DataContext.Organization
                            .Where(o => o.Id == LuckyDrawFilter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (LuckyDrawFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => LuckyDrawFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                    if (LuckyDrawFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs =await DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => LuckyDrawFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.StatusId, LuckyDrawFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<LuckyDrawDAO> DynamicOrder(IQueryable<LuckyDrawDAO> query, LuckyDrawFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyDrawOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case LuckyDrawOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case LuckyDrawOrder.LuckyDrawType:
                            query = query.OrderBy(q => q.LuckyDrawTypeId);
                            break;
                        case LuckyDrawOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case LuckyDrawOrder.RevenuePerTurn:
                            query = query.OrderBy(q => q.RevenuePerTurn);
                            break;
                        case LuckyDrawOrder.StartAt:
                            query = query.OrderBy(q => q.StartAt);
                            break;
                        case LuckyDrawOrder.EndAt:
                            query = query.OrderBy(q => q.EndAt);
                            break;
                        case LuckyDrawOrder.AvatarImage:
                            query = query.OrderBy(q => q.AvatarImageId);
                            break;
                        case LuckyDrawOrder.Image:
                            query = query.OrderBy(q => q.ImageId);
                            break;
                        case LuckyDrawOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case LuckyDrawOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case LuckyDrawOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyDrawOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case LuckyDrawOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case LuckyDrawOrder.LuckyDrawType:
                            query = query.OrderByDescending(q => q.LuckyDrawTypeId);
                            break;
                        case LuckyDrawOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case LuckyDrawOrder.RevenuePerTurn:
                            query = query.OrderByDescending(q => q.RevenuePerTurn);
                            break;
                        case LuckyDrawOrder.StartAt:
                            query = query.OrderByDescending(q => q.StartAt);
                            break;
                        case LuckyDrawOrder.EndAt:
                            query = query.OrderByDescending(q => q.EndAt);
                            break;
                        case LuckyDrawOrder.AvatarImage:
                            query = query.OrderByDescending(q => q.AvatarImageId);
                            break;
                        case LuckyDrawOrder.Image:
                            query = query.OrderByDescending(q => q.ImageId);
                            break;
                        case LuckyDrawOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case LuckyDrawOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case LuckyDrawOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyDraw>> DynamicSelect(IQueryable<LuckyDrawDAO> query, LuckyDrawFilter filter)
        {
            List<LuckyDraw> LuckyDraws = await query.Select(q => new LuckyDraw()
            {
                Id = filter.Selects.Contains(LuckyDrawSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(LuckyDrawSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(LuckyDrawSelect.Name) ? q.Name : default(string),
                LuckyDrawTypeId = filter.Selects.Contains(LuckyDrawSelect.LuckyDrawType) ? q.LuckyDrawTypeId : default(long),
                OrganizationId = filter.Selects.Contains(LuckyDrawSelect.Organization) ? q.OrganizationId : default(long),
                RevenuePerTurn = filter.Selects.Contains(LuckyDrawSelect.RevenuePerTurn) ? q.RevenuePerTurn : default(decimal),
                StartAt = filter.Selects.Contains(LuckyDrawSelect.StartAt) ? q.StartAt : default(DateTime),
                EndAt = filter.Selects.Contains(LuckyDrawSelect.EndAt) ? q.EndAt : default(DateTime),
                AvatarImageId = filter.Selects.Contains(LuckyDrawSelect.AvatarImage) ? q.AvatarImageId : default(long),
                ImageId = filter.Selects.Contains(LuckyDrawSelect.Image) ? q.ImageId : default(long),
                Description = filter.Selects.Contains(LuckyDrawSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(LuckyDrawSelect.Status) ? q.StatusId : default(long),
                Used = filter.Selects.Contains(LuckyDrawSelect.Used) ? q.Used : default(bool),
                AvatarImage = filter.Selects.Contains(LuckyDrawSelect.AvatarImage) && q.AvatarImage != null ? new Image
                {
                    Id = q.AvatarImage.Id,
                    Name = q.AvatarImage.Name,
                    Url = q.AvatarImage.Url,
                    ThumbnailUrl = q.AvatarImage.ThumbnailUrl,
                } : null,
                Image = filter.Selects.Contains(LuckyDrawSelect.Image) && q.Image != null ? new Image
                {
                    Id = q.Image.Id,
                    Name = q.Image.Name,
                    Url = q.Image.Url,
                    ThumbnailUrl = q.Image.ThumbnailUrl,
                } : null,
                LuckyDrawType = filter.Selects.Contains(LuckyDrawSelect.LuckyDrawType) && q.LuckyDrawType != null ? new LuckyDrawType
                {
                    Id = q.LuckyDrawType.Id,
                    Code = q.LuckyDrawType.Code,
                    Name = q.LuckyDrawType.Name,
                } : null,
                Organization = filter.Selects.Contains(LuckyDrawSelect.Organization) && q.Organization != null ? new Organization
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
                    IsDisplay = q.Organization.IsDisplay,
                } : null,
                Status = filter.Selects.Contains(LuckyDrawSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = q.RowId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
            }).ToListWithNoLockAsync();
            if (filter.Selects.Contains(LuckyDrawSelect.LuckyDrawStructures))
            {
                IdFilter IdFilter = new IdFilter { In = LuckyDraws.Select(x => x.Id).ToList() };
                var LuckyDrawStructures = await DataContext.LuckyDrawStructure
                    .Where(x => x.LuckyDrawId, IdFilter)
                    .Select(x => new LuckyDrawStructure
                    {
                        Id = x.Id,
                        LuckyDrawId = x.LuckyDrawId,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        Value = x.Value,
                    })
                    .AsNoTracking().ToListWithNoLockAsync();
                foreach (var LuckyDraw in LuckyDraws)
                {
                    LuckyDraw.LuckyDrawStructures = LuckyDrawStructures.Where(x => x.LuckyDrawId == LuckyDraw.Id).ToList();
                }
            }
            return LuckyDraws;
        }

        public async Task<int> CountAll(LuckyDrawFilter filter)
        {
            IQueryable<LuckyDrawDAO> LuckyDrawDAOs = DataContext.LuckyDraw.AsNoTracking();
            LuckyDrawDAOs = await DynamicFilter(LuckyDrawDAOs, filter);
            return await LuckyDrawDAOs.CountWithNoLockAsync();
        }

        public async Task<int> Count(LuckyDrawFilter filter)
        {
            IQueryable<LuckyDrawDAO> LuckyDrawDAOs = DataContext.LuckyDraw.AsNoTracking();
            LuckyDrawDAOs = await DynamicFilter(LuckyDrawDAOs, filter);
            LuckyDrawDAOs = await OrFilter(LuckyDrawDAOs, filter);
            return await LuckyDrawDAOs.CountWithNoLockAsync();
        }

        public async Task<List<LuckyDraw>> List(LuckyDrawFilter filter)
        {
            if (filter == null) return new List<LuckyDraw>();
            IQueryable<LuckyDrawDAO> LuckyDrawDAOs = DataContext.LuckyDraw.AsNoTracking();
            LuckyDrawDAOs = await DynamicFilter(LuckyDrawDAOs, filter);
            LuckyDrawDAOs = await OrFilter(LuckyDrawDAOs, filter);
            LuckyDrawDAOs = DynamicOrder(LuckyDrawDAOs, filter);
            List<LuckyDraw> LuckyDraws = await DynamicSelect(LuckyDrawDAOs, filter);
            return LuckyDraws;
        }

        public async Task<List<LuckyDraw>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<LuckyDrawDAO> query = DataContext.LuckyDraw.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<LuckyDraw> LuckyDraws = await query.AsNoTracking()
            .Select(x => new LuckyDraw()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                LuckyDrawTypeId = x.LuckyDrawTypeId,
                OrganizationId = x.OrganizationId,
                RevenuePerTurn = x.RevenuePerTurn,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                AvatarImageId = x.AvatarImageId,
                ImageId = x.ImageId,
                Description = x.Description,
                StatusId = x.StatusId,
                Used = x.Used,
                AvatarImage = x.AvatarImage == null ? null : new Image
                {
                    Id = x.AvatarImage.Id,
                    Name = x.AvatarImage.Name,
                    Url = x.AvatarImage.Url,
                    ThumbnailUrl = x.AvatarImage.ThumbnailUrl,
                },
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                },
                LuckyDrawType = x.LuckyDrawType == null ? null : new LuckyDrawType
                {
                    Id = x.LuckyDrawType.Id,
                    Code = x.LuckyDrawType.Code,
                    Name = x.LuckyDrawType.Name,
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
                    IsDisplay = x.Organization.IsDisplay,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();

            var LuckyDrawStoreGroupingMappingQuery = DataContext.LuckyDrawStoreGroupingMapping.AsNoTracking()
                .Where(x => x.LuckyDrawId, IdFilter);
            List<LuckyDrawStoreGroupingMapping> LuckyDrawStoreGroupingMappings = await LuckyDrawStoreGroupingMappingQuery
                .Where(x => x.StoreGrouping.DeletedAt == null)
                .Select(x => new LuckyDrawStoreGroupingMapping
                {
                    LuckyDrawId = x.LuckyDrawId,
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                        Used = x.StoreGrouping.Used,
                    },
                }).ToListWithNoLockAsync();

            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDraw.LuckyDrawStoreGroupingMappings = LuckyDrawStoreGroupingMappings
                    .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                    .ToList();
            }

            var LuckyDrawStoreMappingQuery = DataContext.LuckyDrawStoreMapping.AsNoTracking()
                .Where(x => x.LuckyDrawId, IdFilter);
            List<LuckyDrawStoreMapping> LuckyDrawStoreMappings = await LuckyDrawStoreMappingQuery
                .Where(x => x.Store.DeletedAt == null)
                .Select(x => new LuckyDrawStoreMapping
                {
                    LuckyDrawId = x.LuckyDrawId,
                    StoreId = x.StoreId,
                    Store = new Store
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
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                        IsStoreApprovalDirectSalesOrder = x.Store.IsStoreApprovalDirectSalesOrder,
                    },
                }).ToListWithNoLockAsync();

            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDraw.LuckyDrawStoreMappings = LuckyDrawStoreMappings
                    .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                    .ToList();
            }

            var LuckyDrawStoreTypeMappingQuery = DataContext.LuckyDrawStoreTypeMapping.AsNoTracking()
                .Where(x => x.LuckyDrawId, IdFilter);
            List<LuckyDrawStoreTypeMapping> LuckyDrawStoreTypeMappings = await LuckyDrawStoreTypeMappingQuery
                .Where(x => x.StoreType.DeletedAt == null)
                .Select(x => new LuckyDrawStoreTypeMapping
                {
                    LuckyDrawId = x.LuckyDrawId,
                    StoreTypeId = x.StoreTypeId,
                    StoreType = new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        Used = x.StoreType.Used,
                    },
                }).ToListWithNoLockAsync();

            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDraw.LuckyDrawStoreTypeMappings = LuckyDrawStoreTypeMappings
                    .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                    .ToList();
            }

            var LuckyDrawStructureQuery = DataContext.LuckyDrawStructure.AsNoTracking()
                .Where(x => x.LuckyDrawId, IdFilter);
            List<LuckyDrawStructure> LuckyDrawStructures = await LuckyDrawStructureQuery
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity,
                }).ToListWithNoLockAsync();

            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDraw.LuckyDrawStructures = LuckyDrawStructures
                    .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                    .ToList();
            }

            var LuckyDrawWinnerQuery = DataContext.LuckyDrawWinner.AsNoTracking()
                           .Where(x => x.LuckyDrawId, IdFilter);
            List<LuckyDrawWinner> LuckyDrawWinners = await LuckyDrawWinnerQuery
                .Select(x => new LuckyDrawWinner
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    LuckyDrawStructureId = x.LuckyDrawStructureId,
                    LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                    Time = x.Time,
                    LuckyDrawRegistration = new LuckyDrawRegistration
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
                    LuckyDrawStructure = new LuckyDrawStructure
                    {
                        Id = x.LuckyDrawStructure.Id,
                        LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                        Name = x.LuckyDrawStructure.Name,
                        Value = x.LuckyDrawStructure.Value,
                        Quantity = x.LuckyDrawStructure.Quantity,
                    },
                }).ToListWithNoLockAsync();

            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDraw.LuckyDrawWinners = LuckyDrawWinners
                    .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                    .ToList();
            }

            var LuckyDrawRegistrationQuery = DataContext.LuckyDrawRegistration.AsNoTracking()
                .Where(x => x.LuckyDrawId, IdFilter);
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await LuckyDrawRegistrationQuery
                .Select(x => new LuckyDrawRegistration
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    AppUserId = x.AppUserId,
                    StoreId = x.StoreId,
                    AppUser = new AppUser
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
                    },
                    Store = new Store
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
                        StoreStatusId = x.Store.StoreStatusId,
                    }
                }).ToListWithNoLockAsync();

            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDraw.LuckyDrawRegistrations = LuckyDrawRegistrations
                    .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                    .ToList();
            }

            return LuckyDraws;
        }

        public async Task<LuckyDraw> Get(long Id)
        {
            LuckyDraw LuckyDraw = await DataContext.LuckyDraw.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new LuckyDraw()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                LuckyDrawTypeId = x.LuckyDrawTypeId,
                OrganizationId = x.OrganizationId,
                RevenuePerTurn = x.RevenuePerTurn,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                AvatarImageId = x.AvatarImageId,
                ImageId = x.ImageId,
                Description = x.Description,
                StatusId = x.StatusId,
                Used = x.Used,
                AvatarImage = x.AvatarImage == null ? null : new Image
                {
                    Id = x.AvatarImage.Id,
                    Name = x.AvatarImage.Name,
                    Url = x.AvatarImage.Url,
                    ThumbnailUrl = x.AvatarImage.ThumbnailUrl,
                },
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                },
                LuckyDrawType = x.LuckyDrawType == null ? null : new LuckyDrawType
                {
                    Id = x.LuckyDrawType.Id,
                    Code = x.LuckyDrawType.Code,
                    Name = x.LuckyDrawType.Name,
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
                    IsDisplay = x.Organization.IsDisplay,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (LuckyDraw == null)
                return null;
            LuckyDraw.LuckyDrawStoreGroupingMappings = await DataContext.LuckyDrawStoreGroupingMapping.AsNoTracking()
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .Where(x => x.StoreGrouping.DeletedAt == null)
                .Select(x => new LuckyDrawStoreGroupingMapping
                {
                    LuckyDrawId = x.LuckyDrawId,
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                        Used = x.StoreGrouping.Used,
                    },
                }).ToListWithNoLockAsync();
            LuckyDraw.LuckyDrawStoreMappings = await DataContext.LuckyDrawStoreMapping.AsNoTracking()
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .Where(x => x.Store.DeletedAt == null)
                .Select(x => new LuckyDrawStoreMapping
                {
                    LuckyDrawId = x.LuckyDrawId,
                    StoreId = x.StoreId,
                    Store = new Store
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
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                        IsStoreApprovalDirectSalesOrder = x.Store.IsStoreApprovalDirectSalesOrder,
                        Province = new Province
                        {
                            Id = x.Store.Province.Id,
                            Code = x.Store.Province.Code,
                            Name = x.Store.Province.Name,
                        },
                        StoreType = new StoreType
                        {
                            Id = x.Store.StoreType.Id,
                            Code = x.Store.StoreType.Code,
                            Name = x.Store.StoreType.Name,
                            StatusId = x.Store.StoreType.StatusId,
                        },
                    },
                }).ToListWithNoLockAsync();
            LuckyDraw.LuckyDrawStoreTypeMappings = await DataContext.LuckyDrawStoreTypeMapping.AsNoTracking()
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .Where(x => x.StoreType.DeletedAt == null)
                .Select(x => new LuckyDrawStoreTypeMapping
                {
                    LuckyDrawId = x.LuckyDrawId,
                    StoreTypeId = x.StoreTypeId,
                    StoreType = new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        Used = x.StoreType.Used,
                    },
                }).ToListWithNoLockAsync();
            LuckyDraw.LuckyDrawStructures = await DataContext.LuckyDrawStructure.AsNoTracking()
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity,
                }).ToListWithNoLockAsync();
            LuckyDraw.LuckyDrawWinners = await DataContext.LuckyDrawWinner.AsNoTracking()
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .Select(x => new LuckyDrawWinner
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    LuckyDrawStructureId = x.LuckyDrawStructureId,
                    LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                    Time = x.Time,
                    LuckyDrawRegistration = new LuckyDrawRegistration
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
                    LuckyDrawStructure = new LuckyDrawStructure
                    {
                        Id = x.LuckyDrawStructure.Id,
                        LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                        Name = x.LuckyDrawStructure.Name,
                        Value = x.LuckyDrawStructure.Value,
                        Quantity = x.LuckyDrawStructure.Quantity,
                    },
                }).ToListWithNoLockAsync();
            LuckyDraw.LuckyDrawRegistrations = await DataContext.LuckyDrawRegistration.AsNoTracking()
            .Where(x => x.LuckyDrawId == LuckyDraw.Id)
            .Select(x => new LuckyDrawRegistration
            {
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                AppUserId = x.AppUserId,
                StoreId = x.StoreId,
                AppUser = new AppUser
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
                },
                Store = new Store
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
                    StoreStatusId = x.Store.StoreStatusId,
                }
            }).ToListWithNoLockAsync();

            return LuckyDraw;
        }

        public async Task<bool> Create(LuckyDraw LuckyDraw)
        {
            LuckyDrawDAO LuckyDrawDAO = new LuckyDrawDAO();
            LuckyDrawDAO.Id = LuckyDraw.Id;
            LuckyDrawDAO.Code = LuckyDraw.Code;
            LuckyDrawDAO.Name = LuckyDraw.Name;
            LuckyDrawDAO.LuckyDrawTypeId = LuckyDraw.LuckyDrawTypeId;
            LuckyDrawDAO.OrganizationId = LuckyDraw.OrganizationId;
            LuckyDrawDAO.RevenuePerTurn = LuckyDraw.RevenuePerTurn;
            LuckyDrawDAO.StartAt = LuckyDraw.StartAt;
            LuckyDrawDAO.EndAt = LuckyDraw.EndAt;
            LuckyDrawDAO.AvatarImageId = LuckyDraw.AvatarImageId;
            LuckyDrawDAO.ImageId = LuckyDraw.ImageId;
            LuckyDrawDAO.Description = LuckyDraw.Description;
            LuckyDrawDAO.StatusId = LuckyDraw.StatusId;
            LuckyDrawDAO.Used = LuckyDraw.Used;
            LuckyDrawDAO.RowId = Guid.NewGuid();
            LuckyDrawDAO.CreatedAt = StaticParams.DateTimeNow;
            LuckyDrawDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.LuckyDraw.Add(LuckyDrawDAO);
            await DataContext.SaveChangesAsync();
            LuckyDraw.Id = LuckyDrawDAO.Id;
            await SaveReference(LuckyDraw);
            return true;
        }

        public async Task<bool> Update(LuckyDraw LuckyDraw)
        {
            LuckyDrawDAO LuckyDrawDAO = DataContext.LuckyDraw
                .Where(x => x.Id == LuckyDraw.Id)
                .FirstOrDefault();
            if (LuckyDrawDAO == null)
                return false;
            LuckyDrawDAO.Id = LuckyDraw.Id;
            LuckyDrawDAO.Code = LuckyDraw.Code;
            LuckyDrawDAO.Name = LuckyDraw.Name;
            LuckyDrawDAO.LuckyDrawTypeId = LuckyDraw.LuckyDrawTypeId;
            LuckyDrawDAO.OrganizationId = LuckyDraw.OrganizationId;
            LuckyDrawDAO.RevenuePerTurn = LuckyDraw.RevenuePerTurn;
            LuckyDrawDAO.StartAt = LuckyDraw.StartAt;
            LuckyDrawDAO.EndAt = LuckyDraw.EndAt;
            LuckyDrawDAO.AvatarImageId = LuckyDraw.AvatarImageId;
            LuckyDrawDAO.ImageId = LuckyDraw.ImageId;
            LuckyDrawDAO.Description = LuckyDraw.Description;
            LuckyDrawDAO.StatusId = LuckyDraw.StatusId;
            LuckyDrawDAO.Used = LuckyDraw.Used;
            LuckyDrawDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            if (LuckyDraw.Used == false) await SaveReference(LuckyDraw);
            return true;
        }

        public async Task<bool> Delete(LuckyDraw LuckyDraw)
        {
            await DataContext.LuckyDraw
                .Where(x => x.Id == LuckyDraw.Id)
                .UpdateFromQueryAsync(x => new LuckyDrawDAO
                {
                    DeletedAt = StaticParams.DateTimeNow,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

        public async Task<bool> BulkMerge(List<LuckyDraw> LuckyDraws)
        {
            IdFilter IdFilter = new IdFilter { In = LuckyDraws.Select(x => x.Id).ToList() };
            List<LuckyDrawDAO> LuckyDrawDAOs = new List<LuckyDrawDAO>();
            List<LuckyDrawDAO> DbLuckyDrawDAOs = await DataContext.LuckyDraw
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (LuckyDraw LuckyDraw in LuckyDraws)
            {
                LuckyDrawDAO LuckyDrawDAO = DbLuckyDrawDAOs
                        .Where(x => x.Id == LuckyDraw.Id)
                        .FirstOrDefault();
                if (LuckyDrawDAO == null)
                {
                    LuckyDrawDAO = new LuckyDrawDAO();
                    LuckyDrawDAO.CreatedAt = StaticParams.DateTimeNow;
                    LuckyDrawDAO.RowId = Guid.NewGuid();
                    LuckyDraw.RowId = LuckyDrawDAO.RowId;
                }
                LuckyDrawDAO.Code = LuckyDraw.Code;
                LuckyDrawDAO.Name = LuckyDraw.Name;
                LuckyDrawDAO.LuckyDrawTypeId = LuckyDraw.LuckyDrawTypeId;
                LuckyDrawDAO.OrganizationId = LuckyDraw.OrganizationId;
                LuckyDrawDAO.RevenuePerTurn = LuckyDraw.RevenuePerTurn;
                LuckyDrawDAO.StartAt = LuckyDraw.StartAt;
                LuckyDrawDAO.EndAt = LuckyDraw.EndAt;
                LuckyDrawDAO.AvatarImageId = LuckyDraw.AvatarImageId;
                LuckyDrawDAO.ImageId = LuckyDraw.ImageId;
                LuckyDrawDAO.Description = LuckyDraw.Description;
                LuckyDrawDAO.StatusId = LuckyDraw.StatusId;
                LuckyDrawDAO.Used = LuckyDraw.Used;
                LuckyDrawDAO.UpdatedAt = StaticParams.DateTimeNow;
                LuckyDrawDAOs.Add(LuckyDrawDAO);
            }
            await DataContext.BulkMergeAsync(LuckyDrawDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<LuckyDraw> LuckyDraws)
        {
            List<long> Ids = LuckyDraws.Select(x => x.Id).ToList();
            await DataContext.LuckyDraw
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new LuckyDrawDAO
                {
                    DeletedAt = StaticParams.DateTimeNow,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

        private async Task SaveReference(LuckyDraw LuckyDraw)
        {
            await DataContext.LuckyDrawStoreGroupingMapping
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .DeleteFromQueryAsync();
            List<LuckyDrawStoreGroupingMappingDAO> LuckyDrawStoreGroupingMappingDAOs = new List<LuckyDrawStoreGroupingMappingDAO>();
            if (LuckyDraw.LuckyDrawStoreGroupingMappings != null)
            {
                foreach (LuckyDrawStoreGroupingMapping LuckyDrawStoreGroupingMapping in LuckyDraw.LuckyDrawStoreGroupingMappings)
                {
                    LuckyDrawStoreGroupingMappingDAO LuckyDrawStoreGroupingMappingDAO = new LuckyDrawStoreGroupingMappingDAO();
                    LuckyDrawStoreGroupingMappingDAO.LuckyDrawId = LuckyDraw.Id;
                    LuckyDrawStoreGroupingMappingDAO.StoreGroupingId = LuckyDrawStoreGroupingMapping.StoreGroupingId;
                    LuckyDrawStoreGroupingMappingDAOs.Add(LuckyDrawStoreGroupingMappingDAO);
                }
                await DataContext.LuckyDrawStoreGroupingMapping.BulkMergeAsync(LuckyDrawStoreGroupingMappingDAOs);
            }
            await DataContext.LuckyDrawStoreMapping
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .DeleteFromQueryAsync();
            List<LuckyDrawStoreMappingDAO> LuckyDrawStoreMappingDAOs = new List<LuckyDrawStoreMappingDAO>();
            if (LuckyDraw.LuckyDrawStoreMappings != null)
            {
                foreach (LuckyDrawStoreMapping LuckyDrawStoreMapping in LuckyDraw.LuckyDrawStoreMappings)
                {
                    LuckyDrawStoreMappingDAO LuckyDrawStoreMappingDAO = new LuckyDrawStoreMappingDAO();
                    LuckyDrawStoreMappingDAO.LuckyDrawId = LuckyDraw.Id;
                    LuckyDrawStoreMappingDAO.StoreId = LuckyDrawStoreMapping.StoreId;
                    LuckyDrawStoreMappingDAOs.Add(LuckyDrawStoreMappingDAO);
                }
                await DataContext.LuckyDrawStoreMapping.BulkMergeAsync(LuckyDrawStoreMappingDAOs);
            }
            await DataContext.LuckyDrawStoreTypeMapping
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .DeleteFromQueryAsync();
            List<LuckyDrawStoreTypeMappingDAO> LuckyDrawStoreTypeMappingDAOs = new List<LuckyDrawStoreTypeMappingDAO>();
            if (LuckyDraw.LuckyDrawStoreTypeMappings != null)
            {
                foreach (LuckyDrawStoreTypeMapping LuckyDrawStoreTypeMapping in LuckyDraw.LuckyDrawStoreTypeMappings)
                {
                    LuckyDrawStoreTypeMappingDAO LuckyDrawStoreTypeMappingDAO = new LuckyDrawStoreTypeMappingDAO();
                    LuckyDrawStoreTypeMappingDAO.LuckyDrawId = LuckyDraw.Id;
                    LuckyDrawStoreTypeMappingDAO.StoreTypeId = LuckyDrawStoreTypeMapping.StoreTypeId;
                    LuckyDrawStoreTypeMappingDAOs.Add(LuckyDrawStoreTypeMappingDAO);
                }
                await DataContext.LuckyDrawStoreTypeMapping.BulkMergeAsync(LuckyDrawStoreTypeMappingDAOs);
            }
            var LuckyDrawStructureIds = LuckyDraw.LuckyDrawStructures.Select(x => x.Id).ToList();
            await DataContext.LuckyDrawNumber
                .Where(x => LuckyDrawStructureIds.Contains(x.LuckyDrawStructureId))
                .DeleteFromQueryAsync();
            await DataContext.LuckyDrawStructure
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .DeleteFromQueryAsync();
            List<LuckyDrawStructureDAO> LuckyDrawStructureDAOs = new List<LuckyDrawStructureDAO>();
            if (LuckyDraw.LuckyDrawStructures != null)
            {
                foreach (LuckyDrawStructure LuckyDrawStructure in LuckyDraw.LuckyDrawStructures)
                {
                    LuckyDrawStructureDAO LuckyDrawStructureDAO = new LuckyDrawStructureDAO();
                    LuckyDrawStructureDAO.Id = LuckyDrawStructure.Id;
                    LuckyDrawStructureDAO.LuckyDrawId = LuckyDraw.Id;
                    LuckyDrawStructureDAO.Name = LuckyDrawStructure.Name;
                    LuckyDrawStructureDAO.Value = LuckyDrawStructure.Value;
                    LuckyDrawStructureDAO.Quantity = LuckyDrawStructure.Quantity;
                    LuckyDrawStructureDAOs.Add(LuckyDrawStructureDAO);
                }
                await DataContext.LuckyDrawStructure.BulkMergeAsync(LuckyDrawStructureDAOs);
            }
            List<LuckyDrawRegistrationDAO> LuckyDrawRegistrationDAOs = new List<LuckyDrawRegistrationDAO>();
            List<LuckyDrawRegistrationDAO> DbLuckyDrawRegistrationDAOs = await DataContext.LuckyDrawRegistration
                .Where(x => x.LuckyDrawId == LuckyDraw.Id)
                .ToListWithNoLockAsync();
            if (LuckyDraw.LuckyDrawRegistrations != null)
            {
                foreach (LuckyDrawRegistration LuckyDrawRegistration in LuckyDraw.LuckyDrawRegistrations)
                {
                    LuckyDrawRegistrationDAO LuckyDrawRegistrationDAO = DbLuckyDrawRegistrationDAOs
                        .Where(x => x.Id == LuckyDrawRegistration.Id).FirstOrDefault();
                    if (LuckyDrawRegistrationDAO == null)
                    {
                        LuckyDrawRegistrationDAO = new LuckyDrawRegistrationDAO();
                    }
                    LuckyDrawRegistrationDAO.LuckyDrawId = LuckyDraw.Id;
                    LuckyDrawRegistrationDAO.AppUserId = LuckyDrawRegistrationDAO.AppUserId;
                    LuckyDrawRegistrationDAO.StoreId = LuckyDrawRegistrationDAO.StoreId;
                    LuckyDrawRegistrationDAO.Revenue = LuckyDrawRegistrationDAO.Revenue;
                    LuckyDrawRegistrationDAO.TurnCounter = LuckyDrawRegistrationDAO.TurnCounter;
                    LuckyDrawRegistrationDAO.IsDrawnByStore = LuckyDrawRegistrationDAO.IsDrawnByStore;
                    LuckyDrawRegistrationDAO.Time = LuckyDrawRegistrationDAO.Time;
                    LuckyDrawRegistrationDAOs.Add(LuckyDrawRegistrationDAO);
                }
                await DataContext.LuckyDrawRegistration.BulkMergeAsync(LuckyDrawRegistrationDAOs);
            }


        }

        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.LuckyDraw
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new LuckyDrawDAO { Used = true });
            return true;
        }
    }
}
