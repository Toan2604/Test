using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProduct
{
    public interface IItemService : IServiceScoped
    {
        Task<int> Count(ItemFilter ItemFilter);
        Task<List<Item>> List(ItemFilter ItemFilter);
        Task<List<Item>> MobileList(ItemFilter ItemFilter);
        Task<Item> Get(long Id);
        Task<Item> MobileGet(long Id);
        ItemFilter ToFilter(ItemFilter ItemFilter);
        Task<Item> GetItemByVariation(long ProductId, List<long> VariationIds, long StoreId);
    }

    public class ItemService : BaseService, IItemService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageService ImageService;
        private IItemValidator ItemValidator;

        public ItemService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IImageService ImageService,
            IItemValidator ItemValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ImageService = ImageService;
            this.ItemValidator = ItemValidator;
        }
        public async Task<int> Count(ItemFilter ItemFilter)
        {
            try
            {
                int result = await UOW.ItemRepository.Count(ItemFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemService));
            }
            return 0;
        }

        public async Task<List<Item>> List(ItemFilter ItemFilter)
        {
            try
            {
                List<Item> Items = await UOW.ItemRepository.List(ItemFilter);

                var Ids = Items.Select(x => x.Id).ToList();
                InventoryFilter InventoryFilter = new InventoryFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    ItemId = new IdFilter { In = Ids },
                    Selects = InventorySelect.SaleStock | InventorySelect.Item
                };

                var inventories = await UOW.InventoryRepository.List(InventoryFilter);
                var list = inventories.GroupBy(x => x.ItemId).Select(x => new { ItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

                foreach (var item in Items)
                {
                    item.SaleStock = list.Where(i => i.ItemId == item.Id).Select(i => i.SaleStock).FirstOrDefault();
                    item.HasInventory = item.SaleStock > 0;
                }
                return Items;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemService));
            }
            return null;
        }

        public async Task<List<Item>> MobileList(ItemFilter ItemFilter)
        {
            try
            {
                List<Item> Items;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Items = await UOW.EsItemRepository.List(ItemFilter);
                else
                    Items = await UOW.ItemRepository.List(ItemFilter);

                var Ids = Items.Select(x => x.Id).ToList();
                InventoryFilter InventoryFilter = new InventoryFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    ItemId = new IdFilter { In = Ids },
                    Selects = InventorySelect.SaleStock | InventorySelect.Item
                };

                var inventories = await UOW.InventoryRepository.List(InventoryFilter);
                var list = inventories.GroupBy(x => x.ItemId).Select(x => new { ItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

                foreach (var item in Items)
                {
                    item.SaleStock = list.Where(i => i.ItemId == item.Id).Select(i => i.SaleStock).FirstOrDefault();
                    item.HasInventory = item.SaleStock > 0;
                }
                return Items;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemService));
            }
            return null;
        }

        public async Task<Item> Get(long Id)
        {
            var appUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            Item Item = await UOW.ItemRepository.Get(Id);
            if (Item == null)
                return null;

            WarehouseFilter warehouseFilter = new WarehouseFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                OrderBy = WarehouseOrder.Code,
                OrderType = OrderType.ASC,
                Selects = WarehouseSelect.Id | WarehouseSelect.Code | WarehouseSelect.Name,
                OrganizationId = new IdFilter { Equal = appUser.OrganizationId }
            };
            List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(warehouseFilter);
            var WarehouseIds = Warehouses.Select(x => x.Id).ToList();
            InventoryFilter InventoryFilter = new InventoryFilter
            {
                ItemId = new IdFilter { Equal = Item.Id },
                Skip = 0,
                Take = int.MaxValue,
                Selects = InventorySelect.ALL,
                WarehouseId = new IdFilter { In = WarehouseIds }
            };
            List<Inventory> Inventories = await UOW.InventoryRepository.List(InventoryFilter);
            var InventoryIds = Inventories.Select(x => x.Id).ToList();

            #region lấy thời gian cập nhật tồn kho gần nhất
            InventoryHistoryFilter InventoryHistoryFilter = new InventoryHistoryFilter
            {
                InventoryId = new IdFilter { In = InventoryIds },
                OrderBy = InventoryHistoryOrder.UpdateTime,
                OrderType = OrderType.DESC,
                Skip = 0,
                Take = int.MaxValue,
                Selects = InventoryHistorySelect.UpdateTime,
            };
            List<InventoryHistory> InventoryHistories = await UOW.InventoryHistoryRepository.List(InventoryHistoryFilter);
            InventoryHistory InventoryHistory = InventoryHistories.FirstOrDefault();
            if (InventoryHistory != null)
            {
                Item.LastUpdateInventory = InventoryHistory.UpdateTime;
            }
            #endregion

            Item.Inventories = new List<Inventory>();
            foreach (Warehouse Warehouse in Warehouses)
            {
                Inventory Inventory = Inventories.Where(i => i.WarehouseId == Warehouse.Id && i.ItemId == Id).FirstOrDefault();
                if (Inventory == null)
                {
                    Inventory = new Inventory
                    {
                        Warehouse = Warehouse,
                        ItemId = Id,
                        SaleStock = 0,
                        AccountingStock = 0,
                        WarehouseId = Warehouse.Id,
                    };
                }
                Item.Inventories.Add(Inventory);
            }
            Item.HasInventory = Item.Inventories.Select(i => i.SaleStock).Sum() > 0;
            return Item;
        }

        public async Task<Item> MobileGet(long Id)
        {
            var appUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);

            Item Item;
            var SystemConfig = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfig.USE_ELASTICSEARCH)
                Item = await UOW.EsItemRepository.Get(Id);
            else
                Item = await UOW.ItemRepository.Get(Id);

            if (Item == null)
                return null;

            WarehouseFilter warehouseFilter = new WarehouseFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                OrderBy = WarehouseOrder.Code,
                OrderType = OrderType.ASC,
                Selects = WarehouseSelect.Id | WarehouseSelect.Code | WarehouseSelect.Name,
                OrganizationId = new IdFilter { Equal = appUser.OrganizationId }
            };
            List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(warehouseFilter);
            var WarehouseIds = Warehouses.Select(x => x.Id).ToList();
            InventoryFilter InventoryFilter = new InventoryFilter
            {
                ItemId = new IdFilter { Equal = Item.Id },
                Skip = 0,
                Take = int.MaxValue,
                Selects = InventorySelect.ALL,
                WarehouseId = new IdFilter { In = WarehouseIds }
            };
            List<Inventory> Inventories = await UOW.InventoryRepository.List(InventoryFilter);
            var InventoryIds = Inventories.Select(x => x.Id).ToList();

            #region lấy thời gian cập nhật tồn kho gần nhất
            InventoryHistoryFilter InventoryHistoryFilter = new InventoryHistoryFilter
            {
                InventoryId = new IdFilter { In = InventoryIds },
                OrderBy = InventoryHistoryOrder.UpdateTime,
                OrderType = OrderType.DESC,
                Skip = 0,
                Take = int.MaxValue,
                Selects = InventoryHistorySelect.UpdateTime,
            };
            List<InventoryHistory> InventoryHistories = await UOW.InventoryHistoryRepository.List(InventoryHistoryFilter);
            InventoryHistory InventoryHistory = InventoryHistories.FirstOrDefault();
            if (InventoryHistory != null)
            {
                Item.LastUpdateInventory = InventoryHistory.UpdateTime;
            }
            #endregion

            Item.Inventories = new List<Inventory>();
            foreach (Warehouse Warehouse in Warehouses)
            {
                Inventory Inventory = Inventories.Where(i => i.WarehouseId == Warehouse.Id && i.ItemId == Id).FirstOrDefault();
                if (Inventory == null)
                {
                    Inventory = new Inventory
                    {
                        Warehouse = Warehouse,
                        ItemId = Id,
                        SaleStock = 0,
                        AccountingStock = 0,
                        WarehouseId = Warehouse.Id,
                    };
                }
                Item.Inventories.Add(Inventory);
            }
            Item.HasInventory = Item.Inventories.Select(i => i.SaleStock).Sum() > 0;
            return Item;
        }

        public ItemFilter ToFilter(ItemFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ItemFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ItemFilter subFilter = new ItemFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ProductTypeId))
                        subFilter.ProductTypeId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ProductGroupingId))
                        subFilter.ProductGroupingId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.SalePrice))
                        subFilter.SalePrice = FilterPermissionDefinition.DecimalFilter;
                }
            }
            return filter;
        }

        public async Task<Item> GetItemByVariation(long ProductId, List<long> VariationIds, long StoreId)
        {
            var Store = new Store();
            if (StoreId > 0)
                Store = await UOW.StoreRepository.Get(StoreId);
            var AppUserId = CurrentContext.UserId;
            var AppUser = await UOW.AppUserRepository.GetSimple(AppUserId);
            Product Product;
            Product = await UOW.ProductRepository.Get(ProductId);
            if (Product == null)
                return null;
            ItemFilter ItemFilter = new ItemFilter
            {
                ProductId = new IdFilter
                {
                    Equal = ProductId
                },
                Selects = ItemSelect.ALL,
                Skip = 0,
                Take = int.MaxValue,
            };
            List<Item> Items;
            Items = await UOW.ItemRepository.List(ItemFilter); // list ra toàn bộ item theo product Id

            if (VariationIds.Count > 0)
            {
                Items = await FilterByVariationIds(Items, VariationIds, ProductId);
            } // nếu chọn variation để filter item
            if (Store == null) Store = new Store();
            Items = await CheckSalesStock(Items, AppUser.OrganizationId); // check tồn kho
            Items = await ApplyPrice(Items, Store.Id); // áp giá
            Item Result = Items.FirstOrDefault();
            if (Result == null)
            {
                return null;
            } // ko tìm thấy Item nào
            Result.Product = Product;
            Result.Product.VariationGroupings = Result.Product.VariationGroupings.Where(x => x.Variations.Count > 0).ToList(); // remove VariationGrouping doest not have Variations
            return Result;
        } // lấy ra product và một item theo ItemFilter
        private async Task<List<Item>> CheckSalesStock(List<Item> Items, long OrganizationId)
        {
            List<long> ItemIds = Items.Select(x => x.Id).ToList();
            List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WarehouseSelect.Id,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                OrganizationId = new IdFilter { Equal = OrganizationId }
            });
            var WarehouseIds = Warehouses.Select(x => x.Id).ToList(); // lay kho theo store

            InventoryFilter InventoryFilter = new InventoryFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                ItemId = new IdFilter { In = ItemIds },
                WarehouseId = new IdFilter { In = WarehouseIds },
                Selects = InventorySelect.SaleStock | InventorySelect.Item | InventorySelect.Warehouse
            };

            var inventories = await UOW.InventoryRepository.List(InventoryFilter);
            var list = inventories.GroupBy(x => x.ItemId).Select(x => new { ItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

            foreach (var item in Items)
            {
                item.SaleStock = list.Where(i => i.ItemId == item.Id).Select(i => i.SaleStock).FirstOrDefault();
                item.HasInventory = item.SaleStock > 0;
                item.Inventories = new List<Inventory>();
                item.Inventories.AddRange(inventories.Where(x => x.ItemId == item.Id).ToList());
            }
            return Items;
        } // check ton kho cho ListItem dua vao OrganizationId
        private async Task<List<Item>> FilterByVariationIds(List<Item> Items, List<long> VariationIds, long ProductId)
        {
            List<VariationGrouping> variationGroupings = await UOW.VariationGroupingRepository.List(
                   new VariationGroupingFilter()
                   {
                       ProductId = new IdFilter
                       {
                           Equal = ProductId
                       },
                       Selects = VariationGroupingSelect.ALL,
                       Skip = 0,
                       Take = int.MaxValue,
                   }); // lấy ra VariationGrouping theo productId
            List<long> VariationGroupingIds = variationGroupings.Select(x => x.Id).ToList(); // lấy ra Ids của VariationGrouping
            List<Variation> Variations = await UOW.VariationRepository.List(
                 new VariationFilter()
                 {
                     Id = new IdFilter
                     {
                         In = VariationIds
                     },
                     VariationGroupingId = new IdFilter
                     {
                         In = VariationGroupingIds
                     },
                     Selects = VariationSelect.Code,
                     Skip = 0,
                     Take = int.MaxValue,
                 }
            ); // lấy ra toàn bộ variation theo variationIds và VariationGroupingIds
            List<string> VariationCodes = Variations.Select(x => x.Code).ToList();
            if (Items != null && Items.Any() && VariationCodes != null && VariationCodes.Any())
            {
                foreach (string VariationCode in VariationCodes)
                {
                    Items = Items.Where(x => x.Code.Split("-").Contains(VariationCode)).ToList();
                }
            };
            return Items;
        }

        private async Task<List<Item>> ApplyPrice(List<Item> Items, long StoreId)
        {
            var Store = new Store();
            
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };

            var Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
            if (Store.Organization != null) Organizations = Organizations.Where(x => x.Path.StartsWith(Store.Organization.Path) || Store.Organization.Path.StartsWith(x.Path)).ToList();
            var OrganizationIds = Organizations.Select(x => x.Id).ToList();

            var ItemIds = Items.Select(x => x.Id).Distinct().ToList();
            Dictionary<long, decimal> result = new Dictionary<long, decimal>();

            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();

            if (StoreId != 0)
            {
                Store = await UOW.StoreRepository.Get(StoreId);
                var parentIds = Store.Organization.Path.Split('.');
                OrganizationIds = new List<long>();
                foreach (var parentId in parentIds)
                {
                    if (!string.IsNullOrEmpty(parentId))
                    {
                        OrganizationIds.Add(parentId.ParseLong());
                    }
                };
                PriceListItemMappingFilter PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.ALLSTORE.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };

                var PriceListItemMappingAllStore = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);
                PriceListItemMappings.AddRange(PriceListItemMappingAllStore);

                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STOREGROUPING.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    StoreGroupingId = new IdFilter { In = Store.StoreStoreGroupingMappings?.Select(x => x.StoreGroupingId).ToList() },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingStoreGrouping = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);

                var StoreTypeIdFilter = new IdFilter();
                if (Store.StoreTypeId != 0) StoreTypeIdFilter.Equal = Store.StoreTypeId;
                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STORETYPE.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    StoreTypeId = StoreTypeIdFilter,
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingStoreType = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);

                var StoreIdFilter = new IdFilter();
                if (Store.Id != 0) StoreIdFilter.Equal = Store.Id;
                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.DETAILS.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    StoreId = StoreIdFilter,
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingStoreDetail = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);
                PriceListItemMappings.AddRange(PriceListItemMappingStoreGrouping);
                PriceListItemMappings.AddRange(PriceListItemMappingStoreType);
                PriceListItemMappings.AddRange(PriceListItemMappingStoreDetail);
            }
            //Áp giá theo cấu hình
            //Ưu tiên lấy giá thấp hơn
            if (SystemConfiguration.PRIORITY_USE_PRICE_LIST == 0)
            {
                foreach (var ItemId in ItemIds)
                {
                    result.Add(ItemId, decimal.MaxValue);
                }
                foreach (var ItemId in ItemIds)
                {
                    foreach (var OrganizationId in OrganizationIds)
                    {
                        decimal targetPrice = decimal.MaxValue;
                        targetPrice = PriceListItemMappings.Where(x => x.ItemId == ItemId && x.PriceList.OrganizationId == OrganizationId)
                            .Select(x => x.Price)
                            .DefaultIfEmpty(decimal.MaxValue)
                            .Min();
                        if (targetPrice < result[ItemId])
                        {
                            result[ItemId] = targetPrice;
                        }
                    }
                }

                foreach (var ItemId in ItemIds)
                {
                    if (result[ItemId] == decimal.MaxValue)
                    {
                        result[ItemId] = Items.Where(x => x.Id == ItemId).Select(x => x.SalePrice.GetValueOrDefault(0)).FirstOrDefault();
                    }
                }
            }
            //Ưu tiên lấy giá cao hơn
            else if (SystemConfiguration.PRIORITY_USE_PRICE_LIST == 1)
            {
                foreach (var ItemId in ItemIds)
                {
                    result.Add(ItemId, decimal.MinValue);
                }
                foreach (var ItemId in ItemIds)
                {
                    foreach (var OrganizationId in OrganizationIds)
                    {
                        decimal targetPrice = decimal.MinValue;
                        targetPrice = PriceListItemMappings.Where(x => x.ItemId == ItemId && x.PriceList.OrganizationId == OrganizationId)
                            .Select(x => x.Price)
                            .DefaultIfEmpty(decimal.MinValue)
                            .Max();
                        if (targetPrice > result[ItemId])
                        {
                            result[ItemId] = targetPrice;
                        }
                    }
                }

                foreach (var ItemId in ItemIds)
                {
                    if (result[ItemId] == decimal.MinValue)
                    {
                        result[ItemId] = Items.Where(x => x.Id == ItemId).Select(x => x.SalePrice.GetValueOrDefault(0)).FirstOrDefault();
                    }
                }
            }

            foreach (var item in Items)
            {
                item.SalePrice = result[item.Id] * (1 + item.Product.TaxType.Percentage / 100);
            }
            // thống nhất BE sẽ tính giá cho tất cả API
            return Items;
        }
    }
}
