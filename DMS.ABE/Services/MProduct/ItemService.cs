using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using DMS.ABE.Services.MImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MProduct
{
    public interface IItemService : IServiceScoped
    {
        Task<long> Count(ItemFilter ItemFilter);
        Task<List<Item>> List(ItemFilter ItemFilter);
        Task<List<Item>> ListByStore(ItemFilter ItemFilter);
        Task<long> CountByStore(ItemFilter ItemFilter);
        Task<Item> Get(long Id);
        Task<Item> GetItemByVariation(long Id, List<long> VariationIds);
        ItemFilter ToFilter(ItemFilter ItemFilter);
        Task<Image> SaveImage(Image Image);
    }

    public class ItemService : BaseService, IItemService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageService ImageService;

        public ItemService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IImageService ImageService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ImageService = ImageService;
        }
        public async Task<long> Count(ItemFilter ItemFilter)
        {
            try
            {
                long result;
                result = await UOW.ItemRepository.Count(ItemFilter);

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
                List<Item> Items;
                Items = await UOW.ItemRepository.List(ItemFilter);
                return Items;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemService));
            }
            return null;
        }

        public async Task<List<Item>> ListByStore(ItemFilter ItemFilter)
        {
            try
            {
                Store Store = await GetStore();
                if (Store == null)
                    return null;

                List<Item> Items;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Items = await UOW.EsItemRepository.List(ItemFilter);
                else
                    Items = await UOW.ItemRepository.List(ItemFilter);

                Items = await CheckSalesStock(Items, Store.OrganizationId);
                Items = await ApplyPrice(Items, Store.Id); // ap gia theo priceList

                foreach (var Item in Items)
                {
                    if ((Item.ItemImageMappings == null || !Item.ItemImageMappings.Any()) && Item.Product.ProductImageMappings != null && Item.Product.ProductImageMappings.Any())
                    {
                        ProductImageMapping ProductImageMapping = Item.Product.ProductImageMappings.FirstOrDefault();
                        Item.ItemImageMappings = new List<ItemImageMapping> {
                            new ItemImageMapping
                            {
                                ImageId = ProductImageMapping.ImageId,
                                ItemId = Item.Id,
                                Image = ProductImageMapping.Image == null ? null : new Image
                                {
                                    Id = ProductImageMapping.Image.Id,
                                    Name = ProductImageMapping.Image.Name,
                                    Url = ProductImageMapping.Image.Url,
                                    ThumbnailUrl = ProductImageMapping.Image.ThumbnailUrl
                                }
                            }
                        };
                    }    
                }

                return Items;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemService));
            }
            return null;
        }
        public async Task<long> CountByStore(ItemFilter ItemFilter)
        {
            try
            {
                long count;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    count = await UOW.EsItemRepository.Count(ItemFilter);
                else
                    count = await UOW.ItemRepository.Count(ItemFilter);
                return count;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemService));
            }
            return 0;
        }

        public async Task<Item> Get(long Id)
        {
            Store Store = await GetStore(); // lay ra cua hang
            if (Store == null)
                return null;

            Item Item;
            Item = await UOW.ItemRepository.Get(Id); // lay ra item

            if (Item == null)
                return null;
            Product Product = Item.Product;
            Product.IsFavorite = false;
            int LikeCount = await CountFavorite(Product); // đếm số lượt thích sản phẩm
            if (LikeCount > 0)
            {
                Product.IsFavorite = true;
                Item.Product = Product;
            }
            List<Item> Items = await CheckSalesStock(new List<Item> { Item }, Store.OrganizationId); // check ton kho
            Items = await ApplyPrice(Items, Store.Id); // ap gia theo chinh sach gia
            Item = Items.FirstOrDefault();
            Item.Product.VariationGroupings = Item.Product.VariationGroupings.Where(x => x.Variations.Count > 0).ToList(); // remove VariationGrouping doest not have Variations
            return Item;
        }

        public async Task<Item> GetItemByVariation(long ProductId, List<long> VariationIds)
        {
            Store Store = await GetStore(); // lay ra cua hang
            if (Store == null)
                return null;
            
            Product Product;
            Product = await UOW.ProductRepository.Get(ProductId);

            if (Product == null)
                return null;
            Product.IsFavorite = false;
            int LikeCount = await CountFavorite(Product); // đếm số lượt thích sản phẩm
            if (LikeCount > 0)
                Product.IsFavorite = true;

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
            Items = await CheckSalesStock(Items, Store.OrganizationId); // check tồn kho
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

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/item/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            string thumbnailPath = $"/item/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path, thumbnailPath, 128, 128);
            return Image;
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
            var Store = await UOW.StoreRepository.Get(StoreId);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };

            var Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
            var OrganizationIds = Organizations
                .Where(x => x.Path.StartsWith(Store.Organization.Path) || Store.Organization.Path.StartsWith(x.Path))
                .Select(x => x.Id)
                .ToList();

            var ItemIds = Items.Select(x => x.Id).Distinct().ToList();
            Dictionary<long, decimal> result = new Dictionary<long, decimal>();

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

            var PriceListItemMappingAllStore = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);
            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();
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
            var PriceListItemMappingStoreGrouping = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);

            PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STORETYPE.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                StoreTypeId = new IdFilter { Equal = Store.StoreTypeId },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            };
            var PriceListItemMappingStoreType = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);

            PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.DETAILS.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                StoreId = new IdFilter { Equal = StoreId },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            var PriceListItemMappingStoreDetail = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreGrouping);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreType);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreDetail);

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
            return Items;
        }

        private async Task<int> CountFavorite(Product Product)
        {
            int CountFavorite = await UOW.StoreUserFavoriteProductMappingRepository.Count(new StoreUserFavoriteProductMappingFilter
            {
                FavoriteProductId = new IdFilter { Equal = Product.Id },
                StoreUserId = new IdFilter { Equal = CurrentContext.StoreUserId }
            });
            return CountFavorite;
        } // tra ve so luot like của sản phẩm

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
        } // check ton kho cho ListItem dua vao OrganizationId

        private async Task<Store> GetStore()
        {
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
            if (StoreUser == null)
            {
                return null;
            } // check storeUser co ton tai khong
            Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
            if (Store == null)
            {
                return null;
            } // check store tuong ung vs storeUser co ton tai khong
            return Store;
        }
    }
}
