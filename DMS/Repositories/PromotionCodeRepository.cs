using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IPromotionCodeRepository
    {
        Task<int> Count(PromotionCodeFilter PromotionCodeFilter);
        Task<int> CountAll(PromotionCodeFilter PromotionCodeFilter);
        Task<List<PromotionCode>> List(PromotionCodeFilter PromotionCodeFilter);
        Task<PromotionCode> Get(long Id);
        Task<bool> Create(PromotionCode PromotionCode);
        Task<bool> Update(PromotionCode PromotionCode);
        Task<bool> Delete(PromotionCode PromotionCode);
        Task<bool> BulkMerge(List<PromotionCode> PromotionCodes);
        Task<bool> BulkDelete(List<PromotionCode> PromotionCodes);
        Task<bool> Used(List<long> Ids);
    }
    public class PromotionCodeRepository : IPromotionCodeRepository
    {
        private DataContext DataContext;
        public PromotionCodeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionCodeDAO> DynamicFilter(IQueryable<PromotionCodeDAO> query, PromotionCodeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.PromotionDiscountTypeId, filter.PromotionDiscountTypeId);
            query = query.Where(q => q.Value, filter.Value);
            query = query.Where(q => q.MaxValue, filter.MaxValue);
            query = query.Where(q => q.PromotionTypeId, filter.PromotionTypeId);
            query = query.Where(q => q.PromotionProductAppliedTypeId, filter.PromotionProductAppliedTypeId);
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
            query = query.Where(q => q.StartDate, filter.StartDate);
            query = query.Where(q => q.EndDate == null).Union(query.Where(q => q.EndDate, filter.EndDate));
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<PromotionCodeDAO> OrFilter(IQueryable<PromotionCodeDAO> query, PromotionCodeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionCodeDAO> initQuery = query.Where(q => false);
            foreach (PromotionCodeFilter PromotionCodeFilter in filter.OrFilter)
            {
                IQueryable<PromotionCodeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionCodeFilter.Id);
                queryable = queryable.Where(q => q.Code, PromotionCodeFilter.Code);
                queryable = queryable.Where(q => q.Name, PromotionCodeFilter.Name);
                queryable = queryable.Where(q => q.Quantity, PromotionCodeFilter.Quantity);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionCodeFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.Value, PromotionCodeFilter.Value);
                queryable = queryable.Where(q => q.MaxValue, PromotionCodeFilter.MaxValue);
                queryable = queryable.Where(q => q.PromotionTypeId, PromotionCodeFilter.PromotionTypeId);
                queryable = queryable.Where(q => q.PromotionProductAppliedTypeId, PromotionCodeFilter.PromotionProductAppliedTypeId);
                queryable = queryable.Where(q => q.OrganizationId, PromotionCodeFilter.OrganizationId);
                queryable = queryable.Where(q => q.StartDate, PromotionCodeFilter.StartDate);
                queryable = queryable.Where(q => q.EndDate, PromotionCodeFilter.EndDate);
                queryable = queryable.Where(q => q.StatusId, PromotionCodeFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionCodeDAO> DynamicOrder(IQueryable<PromotionCodeDAO> query, PromotionCodeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionCodeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionCodeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PromotionCodeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case PromotionCodeOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case PromotionCodeOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionCodeOrder.Value:
                            query = query.OrderBy(q => q.Value);
                            break;
                        case PromotionCodeOrder.MaxValue:
                            query = query.OrderBy(q => q.MaxValue);
                            break;
                        case PromotionCodeOrder.PromotionType:
                            query = query.OrderBy(q => q.PromotionTypeId);
                            break;
                        case PromotionCodeOrder.PromotionProductAppliedType:
                            query = query.OrderBy(q => q.PromotionProductAppliedTypeId);
                            break;
                        case PromotionCodeOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case PromotionCodeOrder.StartDate:
                            query = query.OrderBy(q => q.StartDate);
                            break;
                        case PromotionCodeOrder.EndDate:
                            query = query.OrderBy(q => q.EndDate);
                            break;
                        case PromotionCodeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionCodeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionCodeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PromotionCodeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case PromotionCodeOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case PromotionCodeOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionCodeOrder.Value:
                            query = query.OrderByDescending(q => q.Value);
                            break;
                        case PromotionCodeOrder.MaxValue:
                            query = query.OrderByDescending(q => q.MaxValue);
                            break;
                        case PromotionCodeOrder.PromotionType:
                            query = query.OrderByDescending(q => q.PromotionTypeId);
                            break;
                        case PromotionCodeOrder.PromotionProductAppliedType:
                            query = query.OrderByDescending(q => q.PromotionProductAppliedTypeId);
                            break;
                        case PromotionCodeOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case PromotionCodeOrder.StartDate:
                            query = query.OrderByDescending(q => q.StartDate);
                            break;
                        case PromotionCodeOrder.EndDate:
                            query = query.OrderByDescending(q => q.EndDate);
                            break;
                        case PromotionCodeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionCode>> DynamicSelect(IQueryable<PromotionCodeDAO> query, PromotionCodeFilter filter)
        {
            List<PromotionCode> PromotionCodes = await query.Select(q => new PromotionCode()
            {
                Id = filter.Selects.Contains(PromotionCodeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PromotionCodeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PromotionCodeSelect.Name) ? q.Name : default(string),
                Quantity = filter.Selects.Contains(PromotionCodeSelect.Quantity) ? q.Quantity : default(long?),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionCodeSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                Value = filter.Selects.Contains(PromotionCodeSelect.Value) ? q.Value : default(decimal),
                MaxValue = filter.Selects.Contains(PromotionCodeSelect.MaxValue) ? q.MaxValue : default(decimal?),
                PromotionTypeId = filter.Selects.Contains(PromotionCodeSelect.PromotionType) ? q.PromotionTypeId : default(long),
                PromotionProductAppliedTypeId = filter.Selects.Contains(PromotionCodeSelect.PromotionProductAppliedType) ? q.PromotionProductAppliedTypeId : default(long),
                OrganizationId = filter.Selects.Contains(PromotionCodeSelect.Organization) ? q.OrganizationId : default(long),
                StartDate = filter.Selects.Contains(PromotionCodeSelect.StartDate) ? q.StartDate : default(DateTime),
                EndDate = filter.Selects.Contains(PromotionCodeSelect.EndDate) ? q.EndDate : default(DateTime?),
                StatusId = filter.Selects.Contains(PromotionCodeSelect.Status) ? q.StatusId : default(long),
                Organization = filter.Selects.Contains(PromotionCodeSelect.Organization) && q.Organization != null ? new Organization
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
                PromotionDiscountType = filter.Selects.Contains(PromotionCodeSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionProductAppliedType = filter.Selects.Contains(PromotionCodeSelect.PromotionProductAppliedType) && q.PromotionProductAppliedType != null ? new PromotionProductAppliedType
                {
                    Id = q.PromotionProductAppliedType.Id,
                    Code = q.PromotionProductAppliedType.Code,
                    Name = q.PromotionProductAppliedType.Name,
                } : null,
                PromotionType = filter.Selects.Contains(PromotionCodeSelect.PromotionType) && q.PromotionType != null ? new PromotionType
                {
                    Id = q.PromotionType.Id,
                    Code = q.PromotionType.Code,
                    Name = q.PromotionType.Name,
                } : null,
                Status = filter.Selects.Contains(PromotionCodeSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                Used = q.Used,
            }).ToListWithNoLockAsync();
            return PromotionCodes;
        }

        public async Task<int> Count(PromotionCodeFilter filter)
        {
            IQueryable<PromotionCodeDAO> PromotionCodes = DataContext.PromotionCode.AsNoTracking();
            PromotionCodes = DynamicFilter(PromotionCodes, filter);
            PromotionCodes = OrFilter(PromotionCodes, filter);
            return await PromotionCodes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionCodeFilter filter)
        {
            IQueryable<PromotionCodeDAO> PromotionCodes = DataContext.PromotionCode.AsNoTracking();
            PromotionCodes = DynamicFilter(PromotionCodes, filter);
            return await PromotionCodes.CountWithNoLockAsync();
        }
        public async Task<List<PromotionCode>> List(PromotionCodeFilter filter)
        {
            if (filter == null) return new List<PromotionCode>();
            IQueryable<PromotionCodeDAO> PromotionCodeDAOs = DataContext.PromotionCode.AsNoTracking();
            PromotionCodeDAOs = DynamicFilter(PromotionCodeDAOs, filter);
            PromotionCodeDAOs = OrFilter(PromotionCodeDAOs, filter);
            PromotionCodeDAOs = DynamicOrder(PromotionCodeDAOs, filter);
            List<PromotionCode> PromotionCodes = await DynamicSelect(PromotionCodeDAOs, filter);
            return PromotionCodes;
        }

        public async Task<PromotionCode> Get(long Id)
        {
            PromotionCode PromotionCode = await DataContext.PromotionCode.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionCode()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Quantity = x.Quantity,
                PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                Value = x.Value,
                MaxValue = x.MaxValue,
                PromotionTypeId = x.PromotionTypeId,
                PromotionProductAppliedTypeId = x.PromotionProductAppliedTypeId,
                OrganizationId = x.OrganizationId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StatusId = x.StatusId,
                Used = x.Used,
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
                PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                {
                    Id = x.PromotionDiscountType.Id,
                    Code = x.PromotionDiscountType.Code,
                    Name = x.PromotionDiscountType.Name,
                },
                PromotionProductAppliedType = x.PromotionProductAppliedType == null ? null : new PromotionProductAppliedType
                {
                    Id = x.PromotionProductAppliedType.Id,
                    Code = x.PromotionProductAppliedType.Code,
                    Name = x.PromotionProductAppliedType.Name,
                },
                PromotionType = x.PromotionType == null ? null : new PromotionType
                {
                    Id = x.PromotionType.Id,
                    Code = x.PromotionType.Code,
                    Name = x.PromotionType.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionCode == null)
                return null;
            PromotionCode.PromotionCodeHistories = await DataContext.PromotionCodeHistory.AsNoTracking()
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .Select(x => new PromotionCodeHistory
                {
                    Id = x.Id,
                    PromotionCodeId = x.PromotionCodeId,
                    AppliedAt = x.AppliedAt,
                    RowId = x.RowId,
                }).ToListWithNoLockAsync();
            PromotionCode.PromotionCodeOrganizationMappings = await DataContext.PromotionCodeOrganizationMapping.AsNoTracking()
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .Where(x => x.Organization.DeletedAt == null)
                .Select(x => new PromotionCodeOrganizationMapping
                {
                    PromotionCodeId = x.PromotionCodeId,
                    OrganizationId = x.OrganizationId,
                    Organization = new Organization
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
                }).ToListWithNoLockAsync();
            PromotionCode.PromotionCodeProductMappings = await DataContext.PromotionCodeProductMapping.AsNoTracking()
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .Where(x => x.Product.DeletedAt == null)
                .Select(x => new PromotionCodeProductMapping
                {
                    PromotionCodeId = x.PromotionCodeId,
                    ProductId = x.ProductId,
                    Product = new Product
                    {
                        Id = x.Product.Id,
                        Code = x.Product.Code,
                        Name = x.Product.Name,
                        Description = x.Product.Description,
                        ScanCode = x.Product.ScanCode,
                        ERPCode = x.Product.ERPCode,
                        ProductTypeId = x.Product.ProductTypeId,
                        BrandId = x.Product.BrandId,
                        UnitOfMeasureId = x.Product.UnitOfMeasureId,
                        UnitOfMeasureGroupingId = x.Product.UnitOfMeasureGroupingId,
                        SalePrice = x.Product.SalePrice,
                        RetailPrice = x.Product.RetailPrice,
                        TaxTypeId = x.Product.TaxTypeId,
                        StatusId = x.Product.StatusId,
                        OtherName = x.Product.OtherName,
                        TechnicalName = x.Product.TechnicalName,
                        Note = x.Product.Note,
                        IsNew = x.Product.IsNew,
                        UsedVariationId = x.Product.UsedVariationId,
                        Used = x.Product.Used,
                        RowId = x.Product.RowId,
                    },
                }).ToListWithNoLockAsync();
            PromotionCode.PromotionCodeStoreMappings = await DataContext.PromotionCodeStoreMapping.AsNoTracking()
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .Where(x => x.Store.DeletedAt == null)
                .Select(x => new PromotionCodeStoreMapping
                {
                    PromotionCodeId = x.PromotionCodeId,
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
                        AppUserId = x.Store.AppUserId,
                        StatusId = x.Store.StatusId,
                        RowId = x.Store.RowId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                    },
                }).ToListWithNoLockAsync();

            return PromotionCode;
        }
        public async Task<bool> Create(PromotionCode PromotionCode)
        {
            PromotionCodeDAO PromotionCodeDAO = new PromotionCodeDAO();
            PromotionCodeDAO.Id = PromotionCode.Id;
            PromotionCodeDAO.Code = PromotionCode.Code;
            PromotionCodeDAO.Name = PromotionCode.Name;
            PromotionCodeDAO.Quantity = PromotionCode.Quantity;
            PromotionCodeDAO.PromotionDiscountTypeId = PromotionCode.PromotionDiscountTypeId;
            PromotionCodeDAO.Value = PromotionCode.Value;
            PromotionCodeDAO.MaxValue = PromotionCode.MaxValue;
            PromotionCodeDAO.PromotionTypeId = PromotionCode.PromotionTypeId;
            PromotionCodeDAO.PromotionProductAppliedTypeId = PromotionCode.PromotionProductAppliedTypeId;
            PromotionCodeDAO.OrganizationId = PromotionCode.OrganizationId;
            PromotionCodeDAO.StartDate = PromotionCode.StartDate;
            PromotionCodeDAO.EndDate = PromotionCode.EndDate;
            PromotionCodeDAO.StatusId = PromotionCode.StatusId;
            PromotionCodeDAO.CreatedAt = StaticParams.DateTimeNow;
            PromotionCodeDAO.UpdatedAt = StaticParams.DateTimeNow;
            PromotionCodeDAO.Used = false;
            DataContext.PromotionCode.Add(PromotionCodeDAO);
            await DataContext.SaveChangesAsync();
            PromotionCode.Id = PromotionCodeDAO.Id;
            await SaveReference(PromotionCode);
            return true;
        }

        public async Task<bool> Update(PromotionCode PromotionCode)
        {
            PromotionCodeDAO PromotionCodeDAO = DataContext.PromotionCode.Where(x => x.Id == PromotionCode.Id).FirstOrDefault();
            if (PromotionCodeDAO == null)
                return false;
            PromotionCodeDAO.Id = PromotionCode.Id;
            PromotionCodeDAO.Code = PromotionCode.Code;
            PromotionCodeDAO.Name = PromotionCode.Name;
            PromotionCodeDAO.Quantity = PromotionCode.Quantity;
            PromotionCodeDAO.PromotionDiscountTypeId = PromotionCode.PromotionDiscountTypeId;
            PromotionCodeDAO.Value = PromotionCode.Value;
            PromotionCodeDAO.MaxValue = PromotionCode.MaxValue;
            PromotionCodeDAO.PromotionTypeId = PromotionCode.PromotionTypeId;
            PromotionCodeDAO.PromotionProductAppliedTypeId = PromotionCode.PromotionProductAppliedTypeId;
            PromotionCodeDAO.OrganizationId = PromotionCode.OrganizationId;
            PromotionCodeDAO.StartDate = PromotionCode.StartDate;
            PromotionCodeDAO.EndDate = PromotionCode.EndDate;
            PromotionCodeDAO.StatusId = PromotionCode.StatusId;
            PromotionCodeDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionCode);
            return true;
        }

        public async Task<bool> Delete(PromotionCode PromotionCode)
        {
            await DataContext.PromotionCode.Where(x => x.Id == PromotionCode.Id).UpdateFromQueryAsync(x => new PromotionCodeDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionCode> PromotionCodes)
        {
            List<PromotionCodeDAO> PromotionCodeDAOs = new List<PromotionCodeDAO>();
            foreach (PromotionCode PromotionCode in PromotionCodes)
            {
                PromotionCodeDAO PromotionCodeDAO = new PromotionCodeDAO();
                PromotionCodeDAO.Id = PromotionCode.Id;
                PromotionCodeDAO.Code = PromotionCode.Code;
                PromotionCodeDAO.Name = PromotionCode.Name;
                PromotionCodeDAO.Quantity = PromotionCode.Quantity;
                PromotionCodeDAO.PromotionDiscountTypeId = PromotionCode.PromotionDiscountTypeId;
                PromotionCodeDAO.Value = PromotionCode.Value;
                PromotionCodeDAO.MaxValue = PromotionCode.MaxValue;
                PromotionCodeDAO.PromotionTypeId = PromotionCode.PromotionTypeId;
                PromotionCodeDAO.PromotionProductAppliedTypeId = PromotionCode.PromotionProductAppliedTypeId;
                PromotionCodeDAO.OrganizationId = PromotionCode.OrganizationId;
                PromotionCodeDAO.StartDate = PromotionCode.StartDate;
                PromotionCodeDAO.EndDate = PromotionCode.EndDate;
                PromotionCodeDAO.StatusId = PromotionCode.StatusId;
                PromotionCodeDAO.CreatedAt = StaticParams.DateTimeNow;
                PromotionCodeDAO.UpdatedAt = StaticParams.DateTimeNow;
                PromotionCodeDAO.Used = false;
                PromotionCodeDAOs.Add(PromotionCodeDAO);
            }
            await DataContext.BulkMergeAsync(PromotionCodeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionCode> PromotionCodes)
        {
            List<long> Ids = PromotionCodes.Select(x => x.Id).ToList();
            await DataContext.PromotionCode
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new PromotionCodeDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(PromotionCode PromotionCode)
        {
            await DataContext.PromotionCodeOrganizationMapping
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .DeleteFromQueryAsync();
            List<PromotionCodeOrganizationMappingDAO> PromotionCodeOrganizationMappingDAOs = new List<PromotionCodeOrganizationMappingDAO>();
            if (PromotionCode.PromotionCodeOrganizationMappings != null)
            {
                foreach (PromotionCodeOrganizationMapping PromotionCodeOrganizationMapping in PromotionCode.PromotionCodeOrganizationMappings)
                {
                    PromotionCodeOrganizationMappingDAO PromotionCodeOrganizationMappingDAO = new PromotionCodeOrganizationMappingDAO();
                    PromotionCodeOrganizationMappingDAO.PromotionCodeId = PromotionCode.Id;
                    PromotionCodeOrganizationMappingDAO.OrganizationId = PromotionCodeOrganizationMapping.OrganizationId;
                    PromotionCodeOrganizationMappingDAOs.Add(PromotionCodeOrganizationMappingDAO);
                }
                await DataContext.PromotionCodeOrganizationMapping.BulkMergeAsync(PromotionCodeOrganizationMappingDAOs);
            }
            await DataContext.PromotionCodeProductMapping
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .DeleteFromQueryAsync();
            List<PromotionCodeProductMappingDAO> PromotionCodeProductMappingDAOs = new List<PromotionCodeProductMappingDAO>();
            if (PromotionCode.PromotionCodeProductMappings != null)
            {
                foreach (PromotionCodeProductMapping PromotionCodeProductMapping in PromotionCode.PromotionCodeProductMappings)
                {
                    PromotionCodeProductMappingDAO PromotionCodeProductMappingDAO = new PromotionCodeProductMappingDAO();
                    PromotionCodeProductMappingDAO.PromotionCodeId = PromotionCode.Id;
                    PromotionCodeProductMappingDAO.ProductId = PromotionCodeProductMapping.ProductId;
                    PromotionCodeProductMappingDAOs.Add(PromotionCodeProductMappingDAO);
                }
                await DataContext.PromotionCodeProductMapping.BulkMergeAsync(PromotionCodeProductMappingDAOs);
            }
            await DataContext.PromotionCodeStoreMapping
                .Where(x => x.PromotionCodeId == PromotionCode.Id)
                .DeleteFromQueryAsync();
            List<PromotionCodeStoreMappingDAO> PromotionCodeStoreMappingDAOs = new List<PromotionCodeStoreMappingDAO>();
            if (PromotionCode.PromotionCodeStoreMappings != null)
            {
                foreach (PromotionCodeStoreMapping PromotionCodeStoreMapping in PromotionCode.PromotionCodeStoreMappings)
                {
                    PromotionCodeStoreMappingDAO PromotionCodeStoreMappingDAO = new PromotionCodeStoreMappingDAO();
                    PromotionCodeStoreMappingDAO.PromotionCodeId = PromotionCode.Id;
                    PromotionCodeStoreMappingDAO.StoreId = PromotionCodeStoreMapping.StoreId;
                    PromotionCodeStoreMappingDAOs.Add(PromotionCodeStoreMappingDAO);
                }
                await DataContext.PromotionCodeStoreMapping.BulkMergeAsync(PromotionCodeStoreMappingDAOs);
            }
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.PromotionCode
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new PromotionCodeDAO { Used = true });
            return true;
        }

    }
}
