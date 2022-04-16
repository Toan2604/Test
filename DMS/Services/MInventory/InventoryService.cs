using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MInventory
{
    public interface IInventoryService : IServiceScoped
    {
        Task<int> Count(InventoryFilter InventoryFilter);
        Task<List<Inventory>> List(InventoryFilter InventoryFilter);
        Task<Inventory> Get(long Id);
        InventoryFilter ToFilter(InventoryFilter InventoryFilter);
        Task<Inventory> Create(Inventory Inventory);
        Task<Inventory> Update(Inventory Inventory);
    }
    public class InventoryService : BaseService, IInventoryService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IInventoryValidator InventoryValidator;

        public InventoryService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IInventoryValidator InventoryValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.InventoryValidator = InventoryValidator;
        }
        public async Task<int> Count(InventoryFilter InventoryFilter)
        {
            try
            {
                int result = await UOW.InventoryRepository.Count(InventoryFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(InventoryService));
            }
            return 0;
        }

        public async Task<List<Inventory>> List(InventoryFilter InventoryFilter)
        {
            try
            {
                List<Inventory> Inventorys = await UOW.InventoryRepository.List(InventoryFilter);
                return Inventorys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(InventoryService));
            }
            return null;
        }
        public async Task<Inventory> Get(long Id)
        {
            Inventory Inventory = await UOW.InventoryRepository.Get(Id);
            if (Inventory == null)
                return null;
            return Inventory;
        }

        public InventoryFilter ToFilter(InventoryFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<InventoryFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                InventoryFilter subFilter = new InventoryFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }

        #region Chỉ dùng cho đồng bộ FAST
        public async Task<Inventory> Create(Inventory Inventory)
        {
            if (!await InventoryValidator.Create(Inventory))
                return Inventory;

            try
            {
                await UOW.InventoryRepository.Create(Inventory);
                Inventory = await UOW.InventoryRepository.Get(Inventory.Id);
                Logging.CreateAuditLog(Inventory, new { }, nameof(InventoryService));
                return Inventory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(InventoryService));
            }
            return null;
        }

        public async Task<Inventory> Update(Inventory Inventory)
        {
            if (!await InventoryValidator.Update(Inventory))
                return Inventory;
            try
            {
                var oldData = await UOW.InventoryRepository.Get(Inventory.Id);

                await UOW.InventoryRepository.Update(Inventory);

                Inventory = await UOW.InventoryRepository.Get(Inventory.Id);
                if (oldData.SaleStock != Inventory.SaleStock)
                {
                    await CheckOrderState(Inventory);
                }
                Logging.CreateAuditLog(Inventory, oldData, nameof(InventoryService));
                return Inventory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(InventoryService));
            }
            return null;
        }

        private async Task CheckOrderState(Inventory Inventory)
        {
            List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(new DirectSalesOrderFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = DirectSalesOrderSelect.ALL,
                RequestStateId = new IdFilter { NotEqual = RequestStateEnum.APPROVED.Id },
                //Id = new IdFilter { In = DirectSalesOrderIds }                
            });
            DirectSalesOrders = DirectSalesOrders.Where(x => x.DirectSalesOrderContents.Select(x => x.ItemId).Contains(Inventory.ItemId) ||
                                                            x.DirectSalesOrderPromotions.Select(x => x.ItemId).Contains(Inventory.ItemId)).ToList();
            var DirectSalesOrderContents = DirectSalesOrders.Where(x => x.DirectSalesOrderContents != null).SelectMany(x => x.DirectSalesOrderContents).ToList();
            var DirectSalesOrderPromotions = DirectSalesOrders.Where(x => x.DirectSalesOrderPromotions != null).SelectMany(x => x.DirectSalesOrderPromotions).ToList();
            List<long> ItemIds = new List<long>();
            ItemIds.AddRange(DirectSalesOrderContents?.Select(x => x.ItemId));
            ItemIds.AddRange(DirectSalesOrderPromotions?.Select(x => x.ItemId));
            ItemIds = ItemIds.Distinct().ToList();
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
            var Inventories = await UOW.InventoryRepository.List(new InventoryFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = InventorySelect.ALL,
                WarehouseId = new IdFilter { In = WarehouseIds },
                ItemId = new IdFilter { In = ItemIds }
            });
            foreach (var DirectSalesOrder in DirectSalesOrders)
            {
                DirectSalesOrder.InventoryCheckStateId = CheckStateEnum.PASS.Id;
                var AppUser = AppUsers.Where(x => x.Id == DirectSalesOrder.SaleEmployeeId).FirstOrDefault();
                var ItemInScopedIds = new List<long>();
                if (DirectSalesOrder.DirectSalesOrderContents != null && DirectSalesOrder.DirectSalesOrderContents.Any())
                    ItemInScopedIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId));
                if (DirectSalesOrder.DirectSalesOrderPromotions != null && DirectSalesOrder.DirectSalesOrderPromotions.Any())
                    ItemInScopedIds.AddRange(DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.ItemId));
                ItemInScopedIds = ItemInScopedIds.Distinct().ToList();
                var WarehouseInScopedIds = Warehouses.Where(x => x.WarehouseOrganizationMappings.Select(x => x.OrganizationId).Contains(AppUser.OrganizationId)).Select(x => x.Id).ToList();
                var InventoryInScoped = Inventories.Where(x => WarehouseInScopedIds.Contains(x.WarehouseId)).ToList();
                foreach (var ItemId in ItemInScopedIds)
                {
                    var SaleQuantity = (DirectSalesOrder.DirectSalesOrderContents?.Where(x => x.ItemId == ItemId).Sum(x => x.Quantity) ?? 0)
                                    + (DirectSalesOrder.DirectSalesOrderPromotions?.Where(x => x.ItemId == ItemId).Sum(x => x.Quantity) ?? 0);
                    var StockQuantity = InventoryInScoped?.Where(x => x.ItemId == ItemId).Sum(x => x.SaleStock) ?? 0;
                    if (SaleQuantity > StockQuantity)
                    {
                        DirectSalesOrder.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id;
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
