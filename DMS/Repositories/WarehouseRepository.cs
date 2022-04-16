using DMS.Entities;
using DMS.Enums;
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
using MongoDB.Driver;

namespace DMS.Repositories
{
    public interface IWarehouseRepository
    {
        Task<int> Count(WarehouseFilter WarehouseFilter);
        Task<int> CountAll(WarehouseFilter WarehouseFilter);
        Task<List<Warehouse>> List(WarehouseFilter WarehouseFilter);
        Task<List<Warehouse>> List(List<long> Ids);
        Task<Warehouse> Get(long Id);
        Task<bool> Create(Warehouse Warehouse);
        Task<bool> Update(Warehouse Warehouse);
        Task<bool> Delete(Warehouse Warehouse);
        Task<bool> BulkMerge(List<Warehouse> Warehouses);
        Task<bool> BulkDelete(List<Warehouse> Warehouses);
    }
    public class WarehouseRepository : IWarehouseRepository
    {
        private DataContext DataContext;
        public WarehouseRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<WarehouseDAO>> DynamicFilter(IQueryable<WarehouseDAO> query, WarehouseFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.DeletedAt == null);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Address, filter.Address);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value && o.StatusId == 1).FirstOrDefault();
                    var WarehouseOrganizationMappingQuery = DataContext.WarehouseOrganizationMapping.AsNoTracking();
                    WarehouseOrganizationMappingQuery = WarehouseOrganizationMappingQuery.Where(x => x.Organization.Path.StartsWith(OrganizationDAO.Path));
                    var WarehouseIds = await WarehouseOrganizationMappingQuery.Select(x => x.WarehouseId).ToListWithNoLockAsync();
                    query = query.Where(x => x.Id, new IdFilter { In = WarehouseIds });
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value && o.StatusId == 1).FirstOrDefault();
                    var WarehouseOrganizationMappingQuery = DataContext.WarehouseOrganizationMapping.AsNoTracking();
                    WarehouseOrganizationMappingQuery = WarehouseOrganizationMappingQuery.Where(x => x.Organization.Path.StartsWith(OrganizationDAO.Path));
                    var WarehouseIds = await WarehouseOrganizationMappingQuery.Select(x => x.WarehouseId).ToListWithNoLockAsync();
                    query = query.Where(x => x.Id, new IdFilter { NotIn = WarehouseIds });
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> OrganizationIds = Branches.Select(o => o.Id).Distinct().ToList();
                    var WarehouseOrganizationMappingQuery = DataContext.WarehouseOrganizationMapping.AsNoTracking();
                    WarehouseOrganizationMappingQuery = WarehouseOrganizationMappingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
                    var WarehouseIds = await WarehouseOrganizationMappingQuery.Select(x => x.WarehouseId).ToListWithNoLockAsync();
                    query = query.Where(x => x.Id, new IdFilter { In = WarehouseIds });
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> OrganizationIds = Branches.Select(o => o.Id).Distinct().ToList();
                    var WarehouseOrganizationMappingQuery = DataContext.WarehouseOrganizationMapping.AsNoTracking();
                    WarehouseOrganizationMappingQuery = WarehouseOrganizationMappingQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
                    var WarehouseIds = await WarehouseOrganizationMappingQuery.Select(x => x.WarehouseId).ToListWithNoLockAsync();
                    query = query.Where(x => x.Id, new IdFilter { NotIn = WarehouseIds });
                }
            }
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.WardId, filter.WardId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<WarehouseDAO> OrFilter(IQueryable<WarehouseDAO> query, WarehouseFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WarehouseDAO> initQuery = query.Where(q => false);
            foreach (WarehouseFilter WarehouseFilter in filter.OrFilter)
            {
                IQueryable<WarehouseDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WarehouseFilter.Id);
                queryable = queryable.Where(q => q.Code, WarehouseFilter.Code);
                queryable = queryable.Where(q => q.Name, WarehouseFilter.Name);
                queryable = queryable.Where(q => q.Address, WarehouseFilter.Address);
                if (WarehouseFilter.OrganizationId != null)
                {
                    if (WarehouseFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == WarehouseFilter.OrganizationId.Equal.Value && o.StatusId == 1).FirstOrDefault();
                        queryable = from q in queryable
                                    join om in DataContext.WarehouseOrganizationMapping on q.Id equals om.WarehouseId
                                    join o in DataContext.Organization on om.OrganizationId equals o.Id
                                    where o.Path.StartsWith(OrganizationDAO.Path)
                                    select q;
                    }
                    if (WarehouseFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == WarehouseFilter.OrganizationId.Equal.Value && o.StatusId == 1).FirstOrDefault();
                        queryable = from q in queryable
                                    join om in DataContext.WarehouseOrganizationMapping on q.Id equals om.WarehouseId
                                    join o in DataContext.Organization on om.OrganizationId equals o.Id
                                    where !o.Path.StartsWith(OrganizationDAO.Path)
                                    select q;
                    }
                    if (WarehouseFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => WarehouseFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join om in DataContext.WarehouseOrganizationMapping on q.Id equals om.WarehouseId
                                    join o in DataContext.Organization on om.OrganizationId equals o.Id
                                    where Ids.Contains(o.Id)
                                    select q;
                    }
                    if (WarehouseFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => WarehouseFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join om in DataContext.WarehouseOrganizationMapping on q.Id equals om.WarehouseId
                                    join o in DataContext.Organization on om.OrganizationId equals o.Id
                                    where !Ids.Contains(o.Id)
                                    select q;
                    }
                }
                queryable = queryable.Where(q => q.ProvinceId, WarehouseFilter.ProvinceId);
                queryable = queryable.Where(q => q.DistrictId, WarehouseFilter.DistrictId);
                queryable = queryable.Where(q => q.WardId, WarehouseFilter.WardId);
                queryable = queryable.Where(q => q.StatusId, WarehouseFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<WarehouseDAO> DynamicOrder(IQueryable<WarehouseDAO> query, WarehouseFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WarehouseOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WarehouseOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WarehouseOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WarehouseOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case WarehouseOrder.Province:
                            query = query.OrderBy(q => q.Province.Name);
                            break;
                        case WarehouseOrder.District:
                            query = query.OrderBy(q => q.District.Name);
                            break;
                        case WarehouseOrder.Ward:
                            query = query.OrderBy(q => q.Ward.Name);
                            break;
                        case WarehouseOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case WarehouseOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                        case WarehouseOrder.CreatedAt:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WarehouseOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WarehouseOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WarehouseOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WarehouseOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case WarehouseOrder.Province:
                            query = query.OrderByDescending(q => q.Province.Name);
                            break;
                        case WarehouseOrder.District:
                            query = query.OrderByDescending(q => q.District.Name);
                            break;
                        case WarehouseOrder.Ward:
                            query = query.OrderByDescending(q => q.Ward.Name);
                            break;
                        case WarehouseOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case WarehouseOrder.UpdatedAt:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                        case WarehouseOrder.CreatedAt:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Warehouse>> DynamicSelect(IQueryable<WarehouseDAO> query, WarehouseFilter filter)
        {
            List<Warehouse> Warehouses = await query.Select(q => new Warehouse()
            {
                Id = q.Id,
                Code = filter.Selects.Contains(WarehouseSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WarehouseSelect.Name) ? q.Name : default(string),
                Address = filter.Selects.Contains(WarehouseSelect.Address) ? q.Address : default(string),
                ProvinceId = filter.Selects.Contains(WarehouseSelect.Province) ? q.ProvinceId : default(long?),
                DistrictId = filter.Selects.Contains(WarehouseSelect.District) ? q.DistrictId : default(long?),
                WardId = filter.Selects.Contains(WarehouseSelect.Ward) ? q.WardId : default(long?),
                StatusId = filter.Selects.Contains(WarehouseSelect.Status) ? q.StatusId : default(long),
                CreatedAt = filter.Selects.Contains(WarehouseSelect.CreatedAt) ? q.CreatedAt : default(DateTime),
                UpdatedAt = filter.Selects.Contains(WarehouseSelect.UpdatedAt) ? q.UpdatedAt : default(DateTime),
                District = filter.Selects.Contains(WarehouseSelect.District) && q.District != null ? new District
                {
                    Id = q.District.Id,
                    Code = q.District.Code,
                    Name = q.District.Name,
                } : null,
                Province = filter.Selects.Contains(WarehouseSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                } : null,
                Status = filter.Selects.Contains(WarehouseSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Ward = filter.Selects.Contains(WarehouseSelect.Ward) && q.Ward != null ? new Ward
                {
                    Id = q.Ward.Id,
                    Code = q.Ward.Code,
                    Name = q.Ward.Name,
                } : null,
            }).AsNoTracking().ToListWithNoLockAsync();
            List<long> WarehouseIds = Warehouses.Select(x => x.Id).ToList();
            List<InventoryDAO> InventoryDAOs = await DataContext.Inventory.Where(i => i.WarehouseId, new IdFilter { In = WarehouseIds })
                .Select(x => new InventoryDAO { WarehouseId = x.WarehouseId, SaleStock = x.SaleStock }).ToListWithNoLockAsync();
            foreach (Warehouse Warehouse in Warehouses)
            {
                Warehouse.Used = InventoryDAOs.Where(x => x.WarehouseId == Warehouse.Id).Select(x => x.SaleStock).DefaultIfEmpty(0).Sum() > 0;
            }


            if (filter.Selects.Contains(WarehouseSelect.WarehouseOrganizationMappings))
            {
                List<WarehouseOrganizationMapping> WarehouseOrganizationMappings = await DataContext.WarehouseOrganizationMapping.AsNoTracking()
                    .Where(x => x.WarehouseId, new IdFilter { In = WarehouseIds })
                    .Select(p => new WarehouseOrganizationMapping
                    {
                        WarehouseId = p.WarehouseId,
                        OrganizationId = p.OrganizationId,
                        Organization = new Organization
                        {
                            Id = p.Organization.Id,
                            Code = p.Organization.Code,
                            Name = p.Organization.Name,
                            ParentId = p.Organization.ParentId,
                            Path = p.Organization.Path,
                            Level = p.Organization.Level,
                            StatusId = p.Organization.StatusId,
                        }
                    })
                    .ToListWithNoLockAsync();
                foreach (Warehouse Warehouse in Warehouses)
                {
                    Warehouse.WarehouseOrganizationMappings = WarehouseOrganizationMappings
                        .Where(x => x.WarehouseId == Warehouse.Id)
                        .ToList();
                }
            }
            return Warehouses;
        }

        public async Task<int> Count(WarehouseFilter filter)
        {
            IQueryable<WarehouseDAO> Warehouses = DataContext.Warehouse;
            Warehouses = await DynamicFilter(Warehouses, filter);
            Warehouses = OrFilter(Warehouses, filter);
            return await Warehouses.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(WarehouseFilter filter)
        {
            IQueryable<WarehouseDAO> Warehouses = DataContext.Warehouse;
            Warehouses = await DynamicFilter(Warehouses, filter);
            return await Warehouses.CountWithNoLockAsync();
        }

        public async Task<List<Warehouse>> List(WarehouseFilter filter)
        {
            if (filter == null) return new List<Warehouse>();
            List<Warehouse> Warehouses = new List<Warehouse>();
            IQueryable<WarehouseDAO> WarehouseDAOs = DataContext.Warehouse;
            WarehouseDAOs = await DynamicFilter(WarehouseDAOs, filter);
            WarehouseDAOs = OrFilter(WarehouseDAOs, filter);
            WarehouseDAOs = DynamicOrder(WarehouseDAOs, filter);
            Warehouses = await DynamicSelect(WarehouseDAOs, filter);
            return Warehouses;
        }

        public async Task<List<Warehouse>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext.BulkInsertValuesIntoTempTableAsync<long>(Ids);
            var query = from s in DataContext.Warehouse
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;
            List<Warehouse> Warehouses = await query.AsNoTracking()
            .Select(x => new Warehouse()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Address = x.Address,
                ProvinceId = x.ProvinceId,
                DistrictId = x.DistrictId,
                WardId = x.WardId,
                StatusId = x.StatusId,
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
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
                Ward = x.Ward == null ? null : new Ward
                {
                    Id = x.Ward.Id,
                    Code = x.Ward.Code,
                    Name = x.Ward.Name,
                    Priority = x.Ward.Priority,
                    DistrictId = x.Ward.DistrictId,
                    StatusId = x.Ward.StatusId,
                }
            }).ToListWithNoLockAsync();
            var InventoryQuery = DataContext.Inventory.AsNoTracking()
                .Where(x => x.WarehouseId, new IdFilter { In = Ids });
            List<Inventory> Inventories = await InventoryQuery
                .Select(x => new Inventory
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    WarehouseId = x.WarehouseId,
                    SaleStock = x.SaleStock,
                    AccountingStock = x.AccountingStock,
                    RowId = x.RowId,
                    CreatedAt = x.CreatedAt,                    
                    UpdatedAt = x.UpdatedAt,                    
                }).ToListWithNoLockAsync();
            foreach (Warehouse Warehouse in Warehouses)
            {
                Warehouse.Inventories = Inventories
                    .Where(x => x.WarehouseId == Warehouse.Id)
                    .ToList();
            }
            var WarehouseOrganizationMappingQuery = DataContext.WarehouseOrganizationMapping.AsNoTracking()
                .Where(x => x.WarehouseId, new IdFilter { In = Ids });
            List<WarehouseOrganizationMapping> WarehouseOrganizationMappings = await WarehouseOrganizationMappingQuery
                .Where(x => x.Organization.DeletedAt == null)
                .Select(x => new WarehouseOrganizationMapping
                {
                    WarehouseId = x.WarehouseId,
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
                        IsDisplay = x.Organization.IsDisplay,
                    },
                }).ToListWithNoLockAsync();

            foreach (Warehouse Warehouse in Warehouses)
            {
                Warehouse.WarehouseOrganizationMappings = WarehouseOrganizationMappings
                    .Where(x => x.WarehouseId == Warehouse.Id)
                    .ToList();
            }
            return Warehouses;
        }

        public async Task<Warehouse> Get(long Id)
        {
            Warehouse Warehouse = await DataContext.Warehouse
                .Where(x => x.Id == Id).AsNoTracking()
                .Select(x => new Warehouse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Address = x.Address,
                    ProvinceId = x.ProvinceId,
                    DistrictId = x.DistrictId,
                    WardId = x.WardId,
                    StatusId = x.StatusId,
                    RowId = x.RowId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    District = x.District == null ? null : new District
                    {
                        Id = x.District.Id,
                        Code = x.District.Code,
                        Name = x.District.Name,
                    },
                    Province = x.Province == null ? null : new Province
                    {
                        Id = x.Province.Id,
                        Code = x.Province.Code,
                        Name = x.Province.Name,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    Ward = x.Ward == null ? null : new Ward
                    {
                        Id = x.Ward.Id,
                        Code = x.Ward.Code,
                        Name = x.Ward.Name,
                    },
                }).AsNoTracking().FirstOrDefaultWithNoLockAsync();

            if (Warehouse == null)
                return null;
            List<Item> Items = await DataContext.Item.AsNoTracking()
                .Where(i => i.DeletedAt == null && i.StatusId == StatusEnum.ACTIVE.Id)
                .Where(i => i.Product.StatusId == StatusEnum.ACTIVE.Id && i.Product.DeletedAt == null)
                .Select(x => new Item
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Code = x.Code,
                    Name = x.Name,
                    ScanCode = x.ScanCode,
                    SalePrice = x.SalePrice,
                    RetailPrice = x.RetailPrice,
                    Product = new Product
                    {
                        Id = x.Product.Id,
                        Code = x.Product.Code,
                        Name = x.Product.Name,
                        ERPCode = x.Product.ERPCode,
                        UnitOfMeasureId = x.Product.UnitOfMeasureId,
                        UnitOfMeasure = new UnitOfMeasure
                        {
                            Id = x.Product.UnitOfMeasure.Id,
                            Code = x.Product.UnitOfMeasure.Code,
                            Name = x.Product.UnitOfMeasure.Name,
                        },
                    }
                }).ToListWithNoLockAsync();

            var warehouse_query = from inv in DataContext.Inventory
                                  join it in DataContext.Item on inv.ItemId equals it.Id
                                  join pr in DataContext.Product on it.ProductId equals pr.Id
                                  where (inv.WarehouseId == Warehouse.Id) &&
                                  inv.DeletedAt == null &&
                                  it.StatusId == StatusEnum.ACTIVE.Id &&
                                  pr.StatusId == StatusEnum.ACTIVE.Id &&
                                  it.DeletedAt == null &&
                                  pr.DeletedAt == null
                                  select new Inventory
                                  {
                                      Id = inv.Id,
                                      WarehouseId = inv.WarehouseId,
                                      ItemId = inv.ItemId,
                                      SaleStock = inv.SaleStock,
                                      AccountingStock = inv.AccountingStock
                                  };
            Warehouse.Inventories = await warehouse_query
                .ToListWithNoLockAsync();
            Warehouse.Used = Warehouse.Inventories.Select(i => i.SaleStock).DefaultIfEmpty(0).Sum() > 0;
            foreach (Item Item in Items)
            {
                Inventory Inventory = Warehouse.Inventories.Where(i => i.ItemId == Item.Id).FirstOrDefault();
                if (Inventory == null)
                {
                    Inventory = new Inventory
                    {
                        Id = 0,
                        WarehouseId = Warehouse.Id,
                        ItemId = Item.Id,
                        Item = Item,
                        SaleStock = 0,
                        AccountingStock = 0,
                    };
                    Warehouse.Inventories.Add(Inventory);
                }
                Inventory.Item = Item;
            }
            foreach (var Inventory in Warehouse.Inventories)
            {
                if (Inventory.Item == null)
                { }
            }
            Warehouse.Inventories = Warehouse.Inventories.OrderBy(i => i.ItemId).ToList();

            Warehouse.WarehouseOrganizationMappings = await DataContext.WarehouseOrganizationMapping.AsNoTracking()
            .Where(x => x.WarehouseId == Warehouse.Id)
            .Where(x => x.Organization.DeletedAt == null)
            .Select(x => new WarehouseOrganizationMapping
            {
                WarehouseId = x.WarehouseId,
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
                    IsDisplay = x.Organization.IsDisplay,
                },
            }).ToListWithNoLockAsync();
            return Warehouse;
        }
        public async Task<bool> Create(Warehouse Warehouse)
        {
            WarehouseDAO WarehouseDAO = new WarehouseDAO();
            WarehouseDAO.Id = Warehouse.Id;
            WarehouseDAO.Code = Warehouse.Code;
            WarehouseDAO.Name = Warehouse.Name;
            WarehouseDAO.Address = Warehouse.Address;
            WarehouseDAO.ProvinceId = Warehouse.ProvinceId;
            WarehouseDAO.DistrictId = Warehouse.DistrictId;
            WarehouseDAO.WardId = Warehouse.WardId;
            WarehouseDAO.StatusId = Warehouse.StatusId;
            WarehouseDAO.RowId = Warehouse.RowId;
            WarehouseDAO.CreatedAt = StaticParams.DateTimeNow;
            WarehouseDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.Warehouse.Add(WarehouseDAO);
            await DataContext.SaveChangesAsync();
            Warehouse.Id = WarehouseDAO.Id;
            Warehouse.RowId = WarehouseDAO.RowId;
            await SaveReference(Warehouse);

            return true;
        }

        public async Task<bool> Update(Warehouse Warehouse)
        {
            WarehouseDAO WarehouseDAO = DataContext.Warehouse.Where(x => x.Id == Warehouse.Id).FirstOrDefault();
            if (WarehouseDAO == null)
                return false;
            WarehouseDAO.Id = Warehouse.Id;
            WarehouseDAO.Code = Warehouse.Code;
            WarehouseDAO.Name = Warehouse.Name;
            WarehouseDAO.Address = Warehouse.Address;
            WarehouseDAO.ProvinceId = Warehouse.ProvinceId;
            WarehouseDAO.DistrictId = Warehouse.DistrictId;
            WarehouseDAO.WardId = Warehouse.WardId;
            WarehouseDAO.StatusId = Warehouse.StatusId;
            WarehouseDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(Warehouse);

            return true;
        }

        public async Task<bool> Delete(Warehouse Warehouse)
        {
            await DataContext.Warehouse.Where(x => x.Id == Warehouse.Id).UpdateFromQueryAsync(x => new WarehouseDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<Warehouse> Warehouses)
        {
            IdFilter IdFilter = new IdFilter { In = Warehouses.Select(x => x.Id).ToList() };
            List<WarehouseDAO> WarehouseDAOs = new List<WarehouseDAO>();
            List<WarehouseDAO> DbWarehouseDAOs = await DataContext.Warehouse
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (Warehouse Warehouse in Warehouses)
            {
                WarehouseDAO WarehouseDAO = DbWarehouseDAOs
                        .Where(x => x.Id == Warehouse.Id)
                        .FirstOrDefault();
                if (WarehouseDAO == null)
                {
                    WarehouseDAO = new WarehouseDAO();
                    WarehouseDAO.CreatedAt = StaticParams.DateTimeNow;
                    WarehouseDAO.RowId = Guid.NewGuid();
                    Warehouse.RowId = WarehouseDAO.RowId;
                }
                WarehouseDAO.Code = Warehouse.Code;
                WarehouseDAO.Name = Warehouse.Name;
                WarehouseDAO.Address = Warehouse.Address;
                WarehouseDAO.ProvinceId = Warehouse.ProvinceId;
                WarehouseDAO.DistrictId = Warehouse.DistrictId;
                WarehouseDAO.WardId = Warehouse.WardId;
                WarehouseDAO.StatusId = Warehouse.StatusId;
                WarehouseDAO.UpdatedAt = StaticParams.DateTimeNow;
                WarehouseDAOs.Add(WarehouseDAO);
            }
            await DataContext.BulkMergeAsync(WarehouseDAOs);  
            
            foreach (Warehouse Warehouse in Warehouses)
            {
                Warehouse.Id = WarehouseDAOs.FirstOrDefault(x => x.Code == Warehouse.Code).Id;
                await SaveReference(Warehouse);
            }
            //await RemoveFromCache(nameof(WarehouseRepository));
            return true;
        }

        public async Task<bool> BulkDelete(List<Warehouse> Warehouses)
        {
            List<long> Ids = Warehouses.Select(x => x.Id).ToList();
            await DataContext.Warehouse
                .Where(x => x.Id, new IdFilter { In = Ids })
                .UpdateFromQueryAsync(x => new WarehouseDAO { DeletedAt = StaticParams.DateTimeNow });

            return true;
        }

        private async Task SaveReference(Warehouse Warehouse)
        {
            if (Warehouse.Inventories == null) Warehouse.Inventories = new List<Inventory>();
            List<Inventory> LocalInventories = await DataContext.Inventory
                .Where(x => x.WarehouseId == Warehouse.Id)
                .Where(x => x.DeletedAt == null)
                .Select(x => new Inventory
                {
                    Id = x.Id,
                    Key = x.WarehouseId.ToString() + x.ItemId.ToString(),
                    Hash = x.SaleStock.ToString() + "." + x.AccountingStock.ToString(),
                })
                .ToListWithNoLockAsync();
            List<long> InventoryIds = LocalInventories.Select(x => x.Id).ToList();
            List<InventoryHistoryDAO> InventoryHistoryDAOs = await DataContext.InventoryHistory
                .WhereBulkContains(InventoryIds, x => x.InventoryId)
                .ToListWithNoLockAsync();
            foreach (var remote in Warehouse.Inventories)
            {
                remote.Key = remote.WarehouseId.ToString() + remote.ItemId.ToString();
                remote.Hash = remote.SaleStock.ToString() + "." + remote.AccountingStock.ToString();
            }
            var InventoryResult = LocalInventories.Split(Warehouse.Inventories);
            var NewInventories = InventoryResult.Item1.Select(x => x.Remote).ToList();
            var UpdatedInventories = InventoryResult.Item2.Select(x => x.Remote).ToList();
            var DeletedInventories = InventoryResult.Item3.Select(x => x.Local).ToList();
            NewInventories.AddRange(UpdatedInventories);

            if (NewInventories.Any())
            {
                List<InventoryDAO> InventoryDAOs = new List<InventoryDAO>();
                foreach (Inventory Inventory in NewInventories)
                {
                    InventoryDAO InventoryDAO = new InventoryDAO();
                    InventoryDAO.Id = Inventory.Id;
                    InventoryDAO.WarehouseId = Warehouse.Id;
                    InventoryDAO.ItemId = Inventory.ItemId;
                    InventoryDAO.SaleStock = Inventory.SaleStock;
                    InventoryDAO.AccountingStock = Inventory.AccountingStock;
                    InventoryDAO.RowId = Guid.NewGuid();
                    InventoryDAO.CreatedAt = StaticParams.DateTimeNow;
                    InventoryDAO.UpdatedAt = StaticParams.DateTimeNow;
                    InventoryDAO.DeletedAt = null;
                    InventoryDAOs.Add(InventoryDAO);
                    Inventory.RowId = InventoryDAO.RowId;
                }
                await DataContext.BulkMergeAsync(InventoryDAOs, options =>
                {
                    options.IgnoreOnMergeUpdateExpression = i => new { i.CreatedAt, i.RowId };
                });
                //lịch sử inventory
                foreach (Inventory Inventory in NewInventories)
                {
                    InventoryDAO InventoryDAO = InventoryDAOs
                       .Where(x => x.ItemId == Inventory.ItemId).FirstOrDefault();
                    if (InventoryDAO != null && Inventory.InventoryHistories != null)
                    { 
                        foreach (InventoryHistory inventoryHistory in Inventory.InventoryHistories)
                        {
                            InventoryHistoryDAO InventoryHistoryDAO = InventoryHistoryDAOs.Where(x => x.Id == inventoryHistory.Id && x.Id != 0).FirstOrDefault();
                            if (InventoryHistoryDAO == null)
                            {
                                InventoryHistoryDAO = new InventoryHistoryDAO
                                {
                                    InventoryId = InventoryDAO.Id,
                                    AppUserId = inventoryHistory.AppUserId,
                                    SaleStock = inventoryHistory.SaleStock,
                                    AccountingStock = inventoryHistory.AccountingStock,
                                    OldAccountingStock = inventoryHistory.OldAccountingStock,
                                    OldSaleStock = inventoryHistory.OldSaleStock,
                                    CreatedAt = StaticParams.DateTimeNow,
                                    UpdatedAt = StaticParams.DateTimeNow,
                                    DeletedAt = null,
                                };
                                InventoryHistoryDAOs.Add(InventoryHistoryDAO);
                            }
                        }
                        
                    }
                }
                await DataContext.InventoryHistory.BulkMergeAsync(InventoryHistoryDAOs);
            }
            if (DeletedInventories.Any())
            {
                var DeletedIds = DeletedInventories.Select(x => x.Id).ToList();
                await DataContext.Inventory.WhereBulkContains(DeletedIds, x => x.Id).UpdateFromQueryAsync(x => new InventoryDAO { DeletedAt = StaticParams.DateTimeNow });
            }
            await DataContext.WarehouseOrganizationMapping
            .Where(x => x.WarehouseId == Warehouse.Id)
            .DeleteFromQueryAsync();
            List<WarehouseOrganizationMappingDAO> WarehouseOrganizationMappingDAOs = new List<WarehouseOrganizationMappingDAO>();
            if (Warehouse.WarehouseOrganizationMappings != null)
            {
                foreach (WarehouseOrganizationMapping WarehouseOrganizationMapping in Warehouse.WarehouseOrganizationMappings)
                {
                    WarehouseOrganizationMappingDAO WarehouseOrganizationMappingDAO = new WarehouseOrganizationMappingDAO();
                    WarehouseOrganizationMappingDAO.WarehouseId = Warehouse.Id;
                    WarehouseOrganizationMappingDAO.OrganizationId = WarehouseOrganizationMapping.OrganizationId;
                    WarehouseOrganizationMappingDAOs.Add(WarehouseOrganizationMappingDAO);
                }
                await DataContext.WarehouseOrganizationMapping.BulkMergeAsync(WarehouseOrganizationMappingDAOs);
            }
        }
    }
}
