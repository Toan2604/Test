using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MWarehouse
{
    public interface IWarehouseService : IServiceScoped
    {
        Task<int> Count(WarehouseFilter WarehouseFilter);
        Task<List<Warehouse>> List(WarehouseFilter WarehouseFilter);
        Task<Warehouse> Get(long Id);
        Task<Warehouse> Create(Warehouse Warehouse);
        Task<Warehouse> Update(Warehouse Warehouse);
        Task<Warehouse> Delete(Warehouse Warehouse);
        Task<List<Warehouse>> BulkDelete(List<Warehouse> Warehouses);
        Task<List<Warehouse>> BulkMerge(List<Warehouse> Warehouses);
        Task<List<Warehouse>> Import(List<Warehouse> Warehouses);
        WarehouseFilter ToFilter(WarehouseFilter WarehouseFilter);
        Task CheckInventoryStateOrder(List<Inventory> Inventories);
    }

    public class WarehouseService : BaseService, IWarehouseService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRabbitManager RabbitManager;
        private IWarehouseValidator WarehouseValidator;

        public WarehouseService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            IWarehouseValidator WarehouseValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.WarehouseValidator = WarehouseValidator;
        }
        public async Task<int> Count(WarehouseFilter WarehouseFilter)
        {
            try
            {
                int result = await UOW.WarehouseRepository.Count(WarehouseFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return 0;
        }

        public async Task<List<Warehouse>> List(WarehouseFilter WarehouseFilter)
        {
            try
            {
                List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(WarehouseFilter);
                return Warehouses;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }
        public async Task<Warehouse> Get(long Id)
        {
            Warehouse Warehouse = await UOW.WarehouseRepository.Get(Id);
            if (Warehouse == null)
                return null;
            return Warehouse;
        }

        public async Task<Warehouse> Create(Warehouse Warehouse)
        {
            if (!await WarehouseValidator.Create(Warehouse))
                return Warehouse;

            try
            {
                Warehouse = await BuildData(Warehouse);

                await UOW.WarehouseRepository.Create(Warehouse);

                if (Warehouse.Inventories.Any(x => x.SaleStock > 0)) //nếu có kho mới có số lượng tồn > 0 thì cập nhật lại trạng thái đơn hàng
                {
                    var inventories = Warehouse.Inventories.Where(x => x.SaleStock > 0).ToList();
                    SyncOrder(inventories);
                }
                Warehouse = await UOW.WarehouseRepository.Get(Warehouse.Id);
                Sync(new List<Warehouse> { Warehouse });
                Logging.CreateAuditLog(Warehouse, new { }, nameof(WarehouseService));
                return Warehouse;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }

        public async Task<Warehouse> Update(Warehouse Warehouse)
        {
            if (!await WarehouseValidator.Update(Warehouse))
                return Warehouse;
            try
            {
                var oldData = await UOW.WarehouseRepository.Get(Warehouse.Id);
                Warehouse = await BuildData(Warehouse);

                await UOW.WarehouseRepository.Update(Warehouse);
                List<Inventory> Inventories = new List<Inventory>();
                var oldInventories = oldData.Inventories;
                var newInventories = Warehouse.Inventories;
                foreach (var inventory in oldInventories)
                {
                    var newInventory = newInventories.Where(x => x.ItemId == inventory.ItemId).FirstOrDefault();
                    if (newInventory != null && newInventory.SaleStock != inventory.SaleStock)
                        Inventories.Add(newInventory);
                }
                if (Inventories != null && Inventories.Any())
                    SyncOrder(Inventories);
                var newData = await UOW.WarehouseRepository.Get(Warehouse.Id);
                Sync(new List<Warehouse> { newData });
                Logging.CreateAuditLog(newData, oldData, nameof(WarehouseService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }

        public async Task<Warehouse> Delete(Warehouse Warehouse)
        {
            if (!await WarehouseValidator.Delete(Warehouse))
                return Warehouse;

            try
            {
                await UOW.WarehouseRepository.Delete(Warehouse);
                Warehouse = await UOW.WarehouseRepository.Get(Warehouse.Id);
                Sync(new List<Warehouse> { Warehouse });
                Logging.CreateAuditLog(new { }, Warehouse, nameof(WarehouseService));
                return Warehouse;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }


        public async Task<List<Warehouse>> BulkDelete(List<Warehouse> Warehouses)
        {
            if (!await WarehouseValidator.BulkDelete(Warehouses))
                return Warehouses;

            try
            {
                await UOW.WarehouseRepository.BulkDelete(Warehouses);

                Logging.CreateAuditLog(new { }, Warehouses, nameof(WarehouseService));
                return Warehouses;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }
        public async Task<List<Warehouse>> BulkMerge(List<Warehouse> Warehouses)
        {
            if (!await WarehouseValidator.BulkMerge(Warehouses))
                return Warehouses;

            try
            {
                var WarehouseIds = Warehouses.Select(x => x.Id).ToList();
                var oldInventories = await UOW.InventoryRepository.List(new InventoryFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = InventorySelect.ALL,
                    WarehouseId = new IdFilter { In = WarehouseIds }
                });
                for (int i = 0; i < Warehouses.Count; i++)
                {
                    Warehouses[i] = await BuildData(Warehouses[i]);
                }
                await UOW.WarehouseRepository.BulkMerge(Warehouses);
                List<long> Ids = Warehouses.Select(x => x.Id).ToList();
                Warehouses = await UOW.WarehouseRepository.List(Ids);
                Sync(Warehouses);
                var newInventories = Warehouses.SelectMany(x => x.Inventories).ToList();
                List<Inventory> Inventories = new List<Inventory>();
                Inventories = newInventories.Where(x => (oldInventories.Where(o => o.Id == x.Id).FirstOrDefault()?.SaleStock ?? 0) != x.SaleStock).ToList();
                if (Inventories != null && Inventories.Any())
                {
                    SyncOrder(Inventories);
                }
                Logging.CreateAuditLog(new { }, Warehouses, nameof(WarehouseService));
                return Warehouses;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }
        public async Task CheckInventoryStateOrder(List<Inventory> Inventories)
        {
            if (Inventories == null || !Inventories.Any()) return;
            List<long> ItemIds = Inventories.Select(x => x.ItemId).Distinct().ToList();
            List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(new DirectSalesOrderFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = DirectSalesOrderSelect.Id | DirectSalesOrderSelect.SaleEmployee | DirectSalesOrderSelect.BuyerStore 
                | DirectSalesOrderSelect.DirectSalesOrderContents | DirectSalesOrderSelect.DirectSalesOrderPromotions 
                | DirectSalesOrderSelect.InventoryCheckState | DirectSalesOrderSelect.StoreBalanceCheckState,
                GeneralApprovalStateId = new IdFilter
                {
                    NotIn = new List<long>
                        {
                            GeneralApprovalStateEnum.APPROVED.Id,
                            GeneralApprovalStateEnum.REJECTED.Id,
                            GeneralApprovalStateEnum.STORE_APPROVED.Id,
                            GeneralApprovalStateEnum.STORE_REJECTED.Id
                        }
                },
            });
            DirectSalesOrders = DirectSalesOrders.Where(x => x.DirectSalesOrderContents.Select(c => c.ItemId).Any(i => ItemIds.Contains(i) ||
                                                             x.DirectSalesOrderPromotions.Select(p => p.ItemId).Any(i => ItemIds.Contains(i)))).ToList();

            var ItemIdsInOrder = DirectSalesOrders.SelectMany(x => x.DirectSalesOrderContents).Select(x => x.ItemId).ToList();
            ItemIdsInOrder.AddRange(DirectSalesOrders.SelectMany(x => x.DirectSalesOrderPromotions).Select(x => x.ItemId).ToList());
            ItemIdsInOrder = ItemIdsInOrder.Distinct().ToList();
            var AppUserIds = DirectSalesOrders.Select(x => x.SaleEmployeeId).Distinct().ToList();
            var AppUsers = await UOW.AppUserRepository.List(AppUserIds);
            var Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = WarehouseSelect.Id,
                OrganizationId = new IdFilter { In = AppUsers.Select(x => x.OrganizationId).ToList() }
            });
            var WarehouseIds = Warehouses.Select(x => x.Id).Distinct().ToList();
            Warehouses = await UOW.WarehouseRepository.List(WarehouseIds);
            var WarehouseOrganizationMappings = Warehouses.SelectMany(x => x.WarehouseOrganizationMappings).ToList();
            var InventoriesInScoped = await UOW.InventoryRepository.List(new InventoryFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = InventorySelect.ALL,
                WarehouseId = new IdFilter { In = WarehouseIds },
                ItemId = new IdFilter { In = ItemIdsInOrder }
            });
            List<DirectSalesOrder> ChangeList = new List<DirectSalesOrder>();
            List<DirectSalesOrder> oldDirectSalesOrders = new List<DirectSalesOrder>();
            foreach (var DirectSalesOrder in DirectSalesOrders)
            {
                var oldDirectSalesOrder = Utils.Clone(DirectSalesOrder);
                oldDirectSalesOrders.Add(oldDirectSalesOrder);
                DirectSalesOrder.Key = DirectSalesOrder.Id.ToString();
                oldDirectSalesOrder.Key = oldDirectSalesOrder.Id.ToString();
                oldDirectSalesOrder.Hash = HashOrder(oldDirectSalesOrder);

                var ItemInScopedIds = new List<long>();
                var oldInventoryCheckStateId = DirectSalesOrder.InventoryCheckStateId;

                var AppUser = AppUsers.Where(x => x.Id == DirectSalesOrder.SaleEmployeeId).FirstOrDefault();
                if (DirectSalesOrder.DirectSalesOrderContents != null && DirectSalesOrder.DirectSalesOrderContents.Any())
                    ItemInScopedIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId));
                if (DirectSalesOrder.DirectSalesOrderPromotions != null && DirectSalesOrder.DirectSalesOrderPromotions.Any())
                    ItemInScopedIds.AddRange(DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.ItemId));
                ItemInScopedIds = ItemInScopedIds.Distinct().ToList();
                var WarehouseInScopedIds = WarehouseOrganizationMappings.Where(x => x.Organization.Path.StartsWith(AppUser.Organization.Path)).Select(x => x.WarehouseId).ToList();
                var InventoryInScoped = InventoriesInScoped.Where(x => WarehouseInScopedIds.Contains(x.WarehouseId)).ToList();
                DirectSalesOrder.InventoryCheckStateId = CheckStateEnum.PASS.Id;
                DirectSalesOrder.DirectSalesOrderContents?.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.PASS.Id);
                DirectSalesOrder.DirectSalesOrderPromotions?.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.PASS.Id);
                foreach (var ItemId in ItemInScopedIds)
                {
                    var contents = DirectSalesOrder.DirectSalesOrderContents?.Where(x => x.ItemId == ItemId).ToList();
                    var promotions = DirectSalesOrder.DirectSalesOrderPromotions?.Where(x => x.ItemId == ItemId).ToList();
                    var SaleQuantity = (contents?.Sum(x => (x.Quantity * x.Factor)) ?? 0) + (promotions?.Sum(x => (x.Quantity * x.Factor)) ?? 0);
                    var StockQuantity = InventoryInScoped?.Where(x => x.ItemId == ItemId).Sum(x => x.SaleStock) ?? 0;
                    if (SaleQuantity > StockQuantity)
                    {
                        DirectSalesOrder.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id;
                        if (contents != null) contents.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id);
                        if (promotions != null) promotions.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id);
                    }
                }
                DirectSalesOrder.Hash = HashOrder(DirectSalesOrder);
            }
            var result = oldDirectSalesOrders.Split(DirectSalesOrders);
            if (result.Item2 != null) ChangeList.AddRange(result.Item2.Select(x => x.Remote));
            if (ChangeList.Any()) await UOW.DirectSalesOrderRepository.BulkUpdateCheckState(ChangeList);
        }
        private string HashOrder(DirectSalesOrder DirectSalesOrder)
        {
            var sb = new StringBuilder(DirectSalesOrder.InventoryCheckStateId.ToString());
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                foreach (var content in DirectSalesOrder.DirectSalesOrderContents)
                {
                    sb.Append(content.InventoryCheckStateId.ToString());
                }
            }
            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                foreach (var promotion in DirectSalesOrder.DirectSalesOrderPromotions)
                {
                    sb.Append(promotion.InventoryCheckStateId.ToString());
                }
            }
            return sb.ToString();
        }
        public async Task<List<Warehouse>> Import(List<Warehouse> Warehouses)
        {
            if (!await WarehouseValidator.Import(Warehouses))
                return Warehouses;
            try
            {

                await UOW.WarehouseRepository.BulkMerge(Warehouses);


                Logging.CreateAuditLog(Warehouses, new { }, nameof(WarehouseService));
                return Warehouses;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WarehouseService));
            }
            return null;
        }

        public WarehouseFilter ToFilter(WarehouseFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<WarehouseFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                WarehouseFilter subFilter = new WarehouseFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterPermissionDefinition.IdFilter;
                }
            }
            return filter;
        }

        private async Task<Warehouse> BuildData(Warehouse Warehouse)
        {
            List<Item> items = await UOW.ItemRepository.List(new ItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ItemSelect.ALL
            });
            if (Warehouse.Inventories == null)
                Warehouse.Inventories = new List<Inventory>();
            foreach (Item item in items)
            {
                Inventory Inventory = Warehouse.Inventories.Where(i => i.ItemId == item.Id).FirstOrDefault();
                if (Inventory == null)
                {
                    Inventory = new Inventory();
                    Inventory.Id = 0;
                    Inventory.WarehouseId = Warehouse.Id;
                    Inventory.ItemId = item.Id;
                    Inventory.SaleStock = 0;
                    Inventory.AccountingStock = 0;
                    Warehouse.Inventories.Add(Inventory);
                }
            }
            Warehouse oldData = await UOW.WarehouseRepository.Get(Warehouse.Id); // get lai gia tri warehouse
            if (oldData != null)
            {
                foreach (Inventory inventory in Warehouse.Inventories)
                {
                    if (inventory.InventoryHistories == null)
                        inventory.InventoryHistories = new List<InventoryHistory>();
                    Inventory InventoryInDB = oldData.Inventories.Where(i => i.ItemId == inventory.ItemId).FirstOrDefault();
                    if (InventoryInDB == null)
                    {
                        InventoryHistory InventoryHistory = new InventoryHistory();
                        InventoryHistory.InventoryId = inventory.Id;
                        InventoryHistory.SaleStock = inventory.SaleStock;
                        InventoryHistory.AccountingStock = inventory.AccountingStock;
                        InventoryHistory.OldSaleStock = 0;
                        InventoryHistory.OldAccountingStock = 0;
                        InventoryHistory.AppUserId = CurrentContext.UserId;
                        inventory.InventoryHistories.Add(InventoryHistory);
                    }
                    else
                    {
                        inventory.Id = InventoryInDB.Id;
                        if (inventory.SaleStock != InventoryInDB.SaleStock || inventory.AccountingStock != InventoryInDB.AccountingStock)
                        {
                            InventoryHistory InventoryHistory = new InventoryHistory();
                            InventoryHistory.InventoryId = inventory.Id;
                            InventoryHistory.SaleStock = inventory.SaleStock;
                            InventoryHistory.AccountingStock = inventory.AccountingStock;
                            InventoryHistory.OldSaleStock = InventoryInDB.SaleStock;
                            InventoryHistory.OldAccountingStock = InventoryInDB.AccountingStock;
                            InventoryHistory.AppUserId = CurrentContext.UserId;
                            inventory.InventoryHistories.Add(InventoryHistory);
                        }
                    }
                }
            }
            return Warehouse;
        }
        private void SyncOrder(List<Inventory> Inventories)
        {
            RabbitManager.PublishList(Inventories, RoutingKeyEnum.WarehouseCheckOrder.Code);
        }
        private void Sync(List<Warehouse> Warehouses)
        {
            RabbitManager.PublishList(Warehouses, RoutingKeyEnum.WarehouseSync.Code);
        }
    }
}
