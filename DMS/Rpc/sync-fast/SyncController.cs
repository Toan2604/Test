using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Repositories;
using DMS.Services.MInventory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Rpc.sync_fast
{
    public class SyncController : RpcController
    {
        public IUOW UOW { get; }
        public ICurrentContext CurrentContext { get; }
        public IRabbitManager RabbitManager { get; }
        public IInventoryService InventoryService { get; }

        public SyncController(
            IUOW UOW, 
            ICurrentContext CurrentContext, 
            IRabbitManager RabbitManager,
            IInventoryService InventoryService)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.InventoryService = InventoryService;
        }

        #region Store
        [Route(SyncRoute.BulkMergeStore), HttpPost]
        public async Task<ActionResult<bool>> BulkMerge([FromBody] List<Sync_StoreDTO> Sync_StoreDTOs)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<Store> Stores = Sync_StoreDTOs
                .Select(s => ConvertDTOToEntity(s))
                .ToList();
            List<long> Ids = Stores.Where(s => s.Id != 0).Select(s => s.Id).Distinct().ToList();
            List<Store> ExistedStores = await UOW.StoreRepository.List(Ids);
            Stores.ForEach(async s => s.RowId = s.Id == 0 ? Guid.NewGuid() : ExistedStores.Where(x => x.Id == s.Id).FirstOrDefault().RowId);
            var result = await UOW.StoreRepository.BulkMerge(Stores);
            if (result) RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreSync.Code);
            return result;
        }
        [Route(SyncRoute.CreateStore), HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] Sync_StoreDTO Sync_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Store Store = ConvertDTOToEntity(Sync_StoreDTO);
            var result = await UOW.StoreRepository.Create(Store);
            if (result) RabbitManager.PublishSingle(Store, RoutingKeyEnum.StoreSync.Code);
            return result;
        }

        [Route(SyncRoute.UpdateStore), HttpPost]
        public async Task<ActionResult<bool>> Update([FromBody] Sync_StoreDTO Sync_StoreDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Store Store = ConvertDTOToEntity(Sync_StoreDTO);
            var result = await UOW.StoreRepository.Update(Store);
            if (result) RabbitManager.PublishSingle(Store, RoutingKeyEnum.StoreSync.Code);
            return result;
        }
        private Store ConvertDTOToEntity(Sync_StoreDTO Sync_StoreDTO)
        {
            Store Store = new Store();
            Store.Id = Sync_StoreDTO.Id;
            Store.Code = Sync_StoreDTO.Code;
            Store.CodeDraft = Sync_StoreDTO.CodeDraft;
            Store.Name = Sync_StoreDTO.Name;
            Store.ParentStoreId = Sync_StoreDTO.ParentStoreId;
            Store.OrganizationId = Sync_StoreDTO.OrganizationId;
            Store.StoreTypeId = Sync_StoreDTO.StoreTypeId;
            Store.Telephone = Sync_StoreDTO.Telephone;
            Store.ProvinceId = Sync_StoreDTO.ProvinceId;
            Store.DistrictId = Sync_StoreDTO.DistrictId;
            Store.WardId = Sync_StoreDTO.WardId;
            Store.Address = Sync_StoreDTO.Address;
            Store.DeliveryAddress = Sync_StoreDTO.DeliveryAddress;
            Store.UnsignName = Sync_StoreDTO.Name.ChangeToEnglishChar();
            Store.UnsignAddress = Sync_StoreDTO.Address.ChangeToEnglishChar();
            Store.Latitude = Sync_StoreDTO.Latitude;
            Store.Longitude = Sync_StoreDTO.Longitude;
            Store.DeliveryLatitude = Sync_StoreDTO.DeliveryLatitude;
            Store.DeliveryLongitude = Sync_StoreDTO.DeliveryLongitude;
            Store.OwnerName = Sync_StoreDTO.OwnerName;
            Store.OwnerPhone = Sync_StoreDTO.OwnerPhone;
            Store.OwnerEmail = Sync_StoreDTO.OwnerEmail;
            Store.TaxCode = Sync_StoreDTO.TaxCode;
            Store.LegalEntity = Sync_StoreDTO.LegalEntity;
            Store.StatusId = Sync_StoreDTO.StatusId;
            Store.StoreScoutingId = Sync_StoreDTO.StoreScoutingId;
            Store.StoreStatusId = Sync_StoreDTO.StoreStatusId;
            Store.AppUserId = Sync_StoreDTO.AppUserId;
            Store.CreatorId = Sync_StoreDTO.CreatorId;
            Store.Organization = Sync_StoreDTO.Organization == null ? null : new Organization
            {
                Id = Sync_StoreDTO.Organization.Id,
                Code = Sync_StoreDTO.Organization.Code,
                Name = Sync_StoreDTO.Organization.Name,
                ParentId = Sync_StoreDTO.Organization.ParentId,
                Path = Sync_StoreDTO.Organization.Path,
                Level = Sync_StoreDTO.Organization.Level,
                StatusId = Sync_StoreDTO.Organization.StatusId,
                Phone = Sync_StoreDTO.Organization.Phone,
                Address = Sync_StoreDTO.Organization.Address,
                Email = Sync_StoreDTO.Organization.Email,
            };
            Store.StoreType = Sync_StoreDTO.StoreType == null ? null : new StoreType
            {
                Id = Sync_StoreDTO.StoreType.Id,
                Code = Sync_StoreDTO.StoreType.Code,
                Name = Sync_StoreDTO.StoreType.Name,
                StatusId = Sync_StoreDTO.StoreType.StatusId,

            };
            Store.StoreStoreGroupingMappings = Sync_StoreDTO.StoreStoreGroupingMappings?.Select(x => new StoreStoreGroupingMapping
            {
                StoreGroupingId = x.StoreGroupingId,
                StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                {
                    Id = x.StoreGrouping.Id,
                    Code = x.StoreGrouping.Code,
                    Name = x.StoreGrouping.Name,
                    ParentId = x.StoreGrouping.ParentId,
                    Path = x.StoreGrouping.Path,
                    Level = x.StoreGrouping.Level,
                    StatusId = x.StoreGrouping.StatusId,
                },

            }).ToList();
            Store.BaseLanguage = CurrentContext.Language;
            return Store;
        }
        #endregion

        #region Store Grouping
        [Route(SyncRoute.BulkMergeStoreGrouping), HttpPost]
        public async Task<ActionResult<bool>> BulkMerge([FromBody] List<Sync_StoreGroupingDTO> Sync_StoreGroupingDTOs)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            List<StoreGrouping> StoreGroupings = Sync_StoreGroupingDTOs.Select(s => ConvertDTOToEntity(s)).ToList();
            List<long> Ids = StoreGroupings.Where(s => s.Id != 0).Select(s => s.Id).Distinct().ToList();
            List<StoreGrouping> ExistedStoreGroupings = await UOW.StoreGroupingRepository.List(Ids);
            StoreGroupings.ForEach(async s => s.RowId = s.Id == 0 ? Guid.NewGuid() : ExistedStoreGroupings.Where(x => x.Id == s.Id).FirstOrDefault().RowId);
            var result = await UOW.StoreGroupingRepository.BulkMerge(StoreGroupings);
            if (result) RabbitManager.PublishList(StoreGroupings, RoutingKeyEnum.StoreGroupingSync.Code);
            return result;
        }
        [Route(SyncRoute.CreateStoreGrouping), HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] Sync_StoreGroupingDTO Sync_StoreGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGrouping StoreGrouping = ConvertDTOToEntity(Sync_StoreGroupingDTO);
            var result = await UOW.StoreGroupingRepository.Create(StoreGrouping);
            if (result) RabbitManager.PublishSingle(StoreGrouping, RoutingKeyEnum.StoreGroupingSync.Code);
            return result;
        }
        [Route(SyncRoute.UpdateStoreGrouping), HttpPost]
        public async Task<ActionResult<bool>> Update([FromBody] Sync_StoreGroupingDTO Sync_StoreGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGrouping StoreGrouping = ConvertDTOToEntity(Sync_StoreGroupingDTO);
            var result = await UOW.StoreGroupingRepository.Update(StoreGrouping);
            if (result) RabbitManager.PublishSingle(StoreGrouping, RoutingKeyEnum.StoreGroupingSync.Code);
            return result;
        }
        private StoreGrouping ConvertDTOToEntity(Sync_StoreGroupingDTO Sync_StoreGroupingDTO)
        {
            Sync_StoreGroupingDTO.TrimString();
            StoreGrouping StoreGrouping = new StoreGrouping();
            StoreGrouping.Id = Sync_StoreGroupingDTO.Id;
            StoreGrouping.Code = Sync_StoreGroupingDTO.Code;
            StoreGrouping.Name = Sync_StoreGroupingDTO.Name;
            StoreGrouping.ParentId = Sync_StoreGroupingDTO.ParentId;
            StoreGrouping.Path = Sync_StoreGroupingDTO.Path;
            StoreGrouping.Level = Sync_StoreGroupingDTO.Level;
            StoreGrouping.StatusId = Sync_StoreGroupingDTO.StatusId;
            StoreGrouping.BaseLanguage = CurrentContext.Language;
            return StoreGrouping;
        }
        #endregion

        #region Store Type
        [Route(SyncRoute.BulkMergeStoreType), HttpPost]
        public async Task<ActionResult<bool>> BulkMerge([FromBody] List<Sync_StoreTypeDTO> Sync_StoreTypeDTOs)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            List<StoreType> StoreTypes = Sync_StoreTypeDTOs.Select(s => ConvertDTOToEntity(s)).ToList();
            List<long> Ids = StoreTypes.Where(s => s.Id != 0).Select(s => s.Id).Distinct().ToList();
            List<StoreType> ExistedStoreTypes = await UOW.StoreTypeRepository.List(Ids);
            StoreTypes.ForEach(async s => s.RowId = s.Id == 0 ? Guid.NewGuid() : ExistedStoreTypes.Where(x => x.Id == s.Id).FirstOrDefault().RowId);
            var result = await UOW.StoreTypeRepository.BulkMerge(StoreTypes);
            if (result) RabbitManager.PublishList(StoreTypes, RoutingKeyEnum.StoreTypeSync.Code);
            return result;
        }
        [Route(SyncRoute.CreateStoreType), HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] Sync_StoreTypeDTO Sync_StoreTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreType StoreType = ConvertDTOToEntity(Sync_StoreTypeDTO);
            var result = await UOW.StoreTypeRepository.Create(StoreType);
            if (result) RabbitManager.PublishSingle(StoreType, RoutingKeyEnum.StoreTypeSync.Code);
            return result;
        }
        [Route(SyncRoute.UpdateStoreType), HttpPost]
        public async Task<ActionResult<bool>> Update([FromBody] Sync_StoreTypeDTO Sync_StoreTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreType StoreType = ConvertDTOToEntity(Sync_StoreTypeDTO);
            var result = await UOW.StoreTypeRepository.Update(StoreType);
            if (result) RabbitManager.PublishSingle(StoreType, RoutingKeyEnum.StoreTypeSync.Code);
            return result;
        }
        private StoreType ConvertDTOToEntity(Sync_StoreTypeDTO Sync_StoreTypeDTO)
        {
            StoreType StoreType = new StoreType();
            StoreType.Id = Sync_StoreTypeDTO.Id;
            StoreType.Code = Sync_StoreTypeDTO.Code;
            StoreType.Name = Sync_StoreTypeDTO.Name;
            StoreType.StatusId = Sync_StoreTypeDTO.StatusId;
            StoreType.ColorId = Sync_StoreTypeDTO.ColorId;
            StoreType.BaseLanguage = CurrentContext.Language;
            return StoreType;
        }
        #endregion

        #region  Inventory
        [Route(SyncRoute.ListInventory), HttpPost]
        public async Task<ActionResult<List<Sync_InventoryDTO>>> ListInventory([FromBody] Sync_InventoryFilterDTO Sync_InventoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            InventoryFilter InventoryFilter = new InventoryFilter();
            InventoryFilter.Take = int.MaxValue;
            InventoryFilter.Skip = 0;
            InventoryFilter.OrderBy = InventoryOrder.Id;
            InventoryFilter.OrderType = OrderType.ASC;
            InventoryFilter.Selects = InventorySelect.ALL;
            List<Inventory> Inventories = await UOW.InventoryRepository.List(InventoryFilter);
            List<Sync_InventoryDTO> Sync_InventoryDTOs = Inventories.Select(x => new Sync_InventoryDTO(x)).ToList();
            return Sync_InventoryDTOs;
        }
        [Route(SyncRoute.CreateInventory), HttpPost]
        public async Task<ActionResult<Sync_InventoryDTO>> Create([FromBody] Sync_InventoryDTO Sync_InventoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Inventory Inventory = ConvertDTOToEntity(Sync_InventoryDTO);
            Inventory = await InventoryService.Create(Inventory);
            Sync_InventoryDTO = new Sync_InventoryDTO(Inventory);
            if (Inventory.IsValidated)
                return Sync_InventoryDTO;
            else
                return BadRequest(Sync_InventoryDTO);
        }
        [Route(SyncRoute.UpdateInventory), HttpPost]
        public async Task<ActionResult<Sync_InventoryDTO>> Update([FromBody] Sync_InventoryDTO Sync_InventoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Inventory Inventory = ConvertDTOToEntity(Sync_InventoryDTO);
            Inventory = await InventoryService.Update(Inventory);
            Sync_InventoryDTO = new Sync_InventoryDTO(Inventory);
            if (Inventory.IsValidated)
                return Sync_InventoryDTO;
            else
                return BadRequest(Sync_InventoryDTO);
        }
        private Inventory ConvertDTOToEntity(Sync_InventoryDTO Sync_InventoryDTO)
        {
            Sync_InventoryDTO.TrimString();
            Inventory Inventory = new Inventory();
            Inventory.Id = Sync_InventoryDTO.Id;
            Inventory.WarehouseId = Sync_InventoryDTO.WarehouseId;
            Inventory.ItemId = Sync_InventoryDTO.ItemId;
            Inventory.SaleStock = Sync_InventoryDTO.SaleStock;
            Inventory.AccountingStock = Sync_InventoryDTO.AccountingStock;
            Inventory.BaseLanguage = CurrentContext.Language;
            return Inventory;
        }
        #endregion
        #region Warehouse 
        [Route(SyncRoute.CreateWarehouse), HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] Sync_WarehouseDTO Sync_WarehouseDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Warehouse Warehouse = ConvertDTOToEntity(Sync_WarehouseDTO);
            var result = await UOW.WarehouseRepository.Create(Warehouse);
            return result;
        }

        [Route(SyncRoute.UpdateWarehouse), HttpPost]
        public async Task<ActionResult<bool>> Update([FromBody] Sync_WarehouseDTO Sync_WarehouseDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Warehouse Warehouse = ConvertDTOToEntity(Sync_WarehouseDTO);
            var result = await UOW.WarehouseRepository.Update(Warehouse);
            return result;
        }
        private Warehouse ConvertDTOToEntity(Sync_WarehouseDTO Sync_WarehouseDTO)
        {
            Sync_WarehouseDTO.TrimString();
            Warehouse Warehouse = new Warehouse();
            Warehouse.Id = Sync_WarehouseDTO.Id;
            Warehouse.Code = Sync_WarehouseDTO.Code;
            Warehouse.Name = Sync_WarehouseDTO.Name;
            Warehouse.Address = Sync_WarehouseDTO.Address;
            Warehouse.ProvinceId = Sync_WarehouseDTO.ProvinceId;
            Warehouse.DistrictId = Sync_WarehouseDTO.DistrictId;
            Warehouse.WardId = Sync_WarehouseDTO.WardId;
            Warehouse.StatusId = Sync_WarehouseDTO.StatusId;
            Warehouse.Inventories = Sync_WarehouseDTO.Inventories?
                .Select(x => new Inventory
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    SaleStock = x.SaleStock,
                    WarehouseId = x.WarehouseId,
                    AccountingStock = x.AccountingStock
                }).ToList();
            Warehouse.BaseLanguage = CurrentContext.Language;
            return Warehouse;
        }
        #endregion

        #region Price List
        [Route(SyncRoute.CreatePriceList), HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] Sync_PriceListDTO Sync_PriceListDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PriceList PriceList = ConvertDTOToEntity(Sync_PriceListDTO);
            var result = await UOW.PriceListRepository.Create(PriceList);
            return result;
        }

        [Route(SyncRoute.UpdatePriceList), HttpPost]
        public async Task<ActionResult<bool>> Update([FromBody] Sync_PriceListDTO Sync_PriceListDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PriceList PriceList = ConvertDTOToEntity(Sync_PriceListDTO);
            var result = await UOW.PriceListRepository.Update(PriceList);
            return result;
        }
        private PriceList ConvertDTOToEntity(Sync_PriceListDTO Sync_PriceListDTO)
        {
            Sync_PriceListDTO.TrimString();
            PriceList PriceList = new PriceList();
            PriceList.Id = Sync_PriceListDTO.Id;
            PriceList.Code = Sync_PriceListDTO.Code;
            PriceList.Name = Sync_PriceListDTO.Name;
            PriceList.CreatorId = Sync_PriceListDTO.CreatorId;
            PriceList.StartDate = Sync_PriceListDTO.StartDate;
            PriceList.EndDate = Sync_PriceListDTO.EndDate;
            PriceList.StatusId = Sync_PriceListDTO.StatusId;
            PriceList.OrganizationId = Sync_PriceListDTO.OrganizationId;
            PriceList.PriceListTypeId = Sync_PriceListDTO.PriceListTypeId;
            PriceList.SalesOrderTypeId = Sync_PriceListDTO.SalesOrderTypeId;
            PriceList.RequestStateId = Sync_PriceListDTO.RequestStateId;
            PriceList.PriceListItemMappings = Sync_PriceListDTO.PriceListItemMappings?.Select(x => new PriceListItemMapping
            {
                ItemId = x.ItemId,
                PriceListId = x.PriceListId,
                Price = x.Price,
                PriceListItemHistories = x.PriceListItemHistories?.Select(x => new PriceListItemHistory
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    PriceListId = x.PriceListId,
                    ModifierId = x.ModifierId,
                    NewPrice = x.NewPrice,
                    OldPrice = x.OldPrice,
                    UpdatedAt = x.UpdatedAt,
                }).ToList()
            }).ToList();
            PriceList.BaseLanguage = CurrentContext.Language;
            return PriceList;
        }
        #endregion

        #region Direct Sales Order
        [Route(SyncRoute.CreateDirectSalesOrder), HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] Sync_DirectSalesOrderDTO Sync_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(Sync_DirectSalesOrderDTO);
            var result = await UOW.DirectSalesOrderRepository.Create(DirectSalesOrder);
            if (result) RabbitManager.PublishSingle(DirectSalesOrder, RoutingKeyEnum.DirectSalesOrderSync.Code);
            return result;
        }

        [Route(SyncRoute.UpdateDirectSalesOrder), HttpPost]
        public async Task<ActionResult<bool>> Update([FromBody] Sync_DirectSalesOrderDTO Sync_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(Sync_DirectSalesOrderDTO);
            var result = await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
            if (result) RabbitManager.PublishSingle(DirectSalesOrder, RoutingKeyEnum.DirectSalesOrderSync.Code);
            return result;
        }
        private DirectSalesOrder ConvertDTOToEntity(Sync_DirectSalesOrderDTO Sync_DirectSalesOrderDTO)
        {
            Sync_DirectSalesOrderDTO.TrimString();
            DirectSalesOrder DirectSalesOrder = new DirectSalesOrder();
            DirectSalesOrder.Id = Sync_DirectSalesOrderDTO.Id;
            DirectSalesOrder.Code = Sync_DirectSalesOrderDTO.Code;
            DirectSalesOrder.BuyerStoreId = Sync_DirectSalesOrderDTO.BuyerStoreId;
            DirectSalesOrder.PhoneNumber = Sync_DirectSalesOrderDTO.PhoneNumber;
            DirectSalesOrder.StoreAddress = Sync_DirectSalesOrderDTO.StoreAddress;
            DirectSalesOrder.DeliveryAddress = Sync_DirectSalesOrderDTO.DeliveryAddress;
            DirectSalesOrder.SaleEmployeeId = Sync_DirectSalesOrderDTO.SaleEmployeeId;
            DirectSalesOrder.OrganizationId = Sync_DirectSalesOrderDTO.OrganizationId;
            DirectSalesOrder.OrderDate = Sync_DirectSalesOrderDTO.OrderDate;
            DirectSalesOrder.DeliveryDate = Sync_DirectSalesOrderDTO.DeliveryDate;
            DirectSalesOrder.RequestStateId = Sync_DirectSalesOrderDTO.RequestStateId;
            DirectSalesOrder.EditedPriceStatusId = Sync_DirectSalesOrderDTO.EditedPriceStatusId;
            DirectSalesOrder.Note = Sync_DirectSalesOrderDTO.Note;
            DirectSalesOrder.SubTotal = Sync_DirectSalesOrderDTO.SubTotal;
            DirectSalesOrder.GeneralDiscountPercentage = Sync_DirectSalesOrderDTO.GeneralDiscountPercentage;
            DirectSalesOrder.GeneralDiscountAmount = Sync_DirectSalesOrderDTO.GeneralDiscountAmount;
            DirectSalesOrder.TotalTaxAmount = Sync_DirectSalesOrderDTO.TotalTaxAmount;
            DirectSalesOrder.TotalAfterTax = Sync_DirectSalesOrderDTO.TotalAfterTax;
            DirectSalesOrder.PromotionCode = Sync_DirectSalesOrderDTO.PromotionCode;
            DirectSalesOrder.PromotionValue = Sync_DirectSalesOrderDTO.PromotionValue;
            DirectSalesOrder.Total = Sync_DirectSalesOrderDTO.Total;
            DirectSalesOrder.DirectSalesOrderContents = Sync_DirectSalesOrderDTO.DirectSalesOrderContents?
                .Select(x => new DirectSalesOrderContent
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    Quantity = x.Quantity,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    RequestedQuantity = x.RequestedQuantity,
                    PrimaryPrice = x.PrimaryPrice,
                    SalePrice = x.SalePrice,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountAmount = x.DiscountAmount,
                    GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    Amount = x.Amount,
                    TaxPercentage = x.TaxPercentage,
                    TaxAmount = x.TaxAmount,
                    Factor = x.Factor
                }).ToList();
            DirectSalesOrder.DirectSalesOrderPromotions = Sync_DirectSalesOrderDTO.DirectSalesOrderPromotions?
                .Select(x => new DirectSalesOrderPromotion
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    Quantity = x.Quantity,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    RequestedQuantity = x.RequestedQuantity,
                    Note = x.Note,
                    Factor = x.Factor
                }).ToList();
            DirectSalesOrder.BaseLanguage = CurrentContext.Language;
            return DirectSalesOrder;
        }
        #endregion
    }
}
