using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using DMS.Services.MStoreChecking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{

    public class MobileSyncController : SimpleController
    {
        private DataContext DataContext;
        private ICurrentContext CurrentContext;
        private IStoreCheckingService StoreCheckingService;
        public MobileSyncController(
            ICurrentContext CurrentContext,
            IStoreCheckingService StoreCheckingService,
            DataContext DataContext)
        {
            this.StoreCheckingService = StoreCheckingService;
            this.DataContext = DataContext;
        }

        [Route("rpc/dms/mobile-sync/pull"), HttpGet]
        public async Task<MobileSync_ChangeDTO> Pull([FromQuery] DateTime Timestamp)
        {
            MobileSync_ChangeDTO MobileSync_ChangeDTO = new MobileSync_ChangeDTO();
            MobileSync_ChangeDTO.Banner = await BuildBanner(Timestamp);
            MobileSync_ChangeDTO.Product = await BuildProduct(Timestamp);
            MobileSync_ChangeDTO.Item = await BuildItem(Timestamp);
            MobileSync_ChangeDTO.StoreInScoped = await BuildStoreInScoped(Timestamp);
            MobileSync_ChangeDTO.StorePlanned = await BuildStorePlanned(Timestamp);
            MobileSync_ChangeDTO.StoreUnplanned = await BuildStoreUnplanned(Timestamp);
            return MobileSync_ChangeDTO;
        }

        private async Task<MobileSync_BannerSyncDTO> BuildBanner(DateTime Timestamp)
        {
            MobileSync_BannerSyncDTO MobileSync_BannerSyncDTO = new MobileSync_BannerSyncDTO()
            {
                Created = new List<MobileSync_BannerDTO>(),
                Updated = new List<MobileSync_BannerDTO>(),
                Deleted = new List<MobileSync_BannerDTO>()
            };
            List<BannerDAO> BannerDAOs = await DataContext.Banner.Where(x => x.UpdatedAt >= Timestamp).ToListWithNoLockAsync();
            foreach (BannerDAO BannerDAO in BannerDAOs)
            {
                MobileSync_BannerDTO MobileSync_BannerDTO = new MobileSync_BannerDTO
                {
                    Id = BannerDAO.Id,
                    Code = BannerDAO.Code,
                    Content = BannerDAO.Content,
                    Priority = BannerDAO.Priority,
                    StatusId = BannerDAO.StatusId,
                    UpdatedAt = BannerDAO.UpdatedAt,
                    CreatedAt = BannerDAO.CreatedAt,
                    ImageId = BannerDAO.ImageId,
                    Title = BannerDAO.Title,
                };
                MobileSync_BannerDTO.Images = new List<MobileSync_ImageDTO>();
                if (BannerDAO.Image != null)
                {
                    MobileSync_BannerDTO.Images.Add(new MobileSync_ImageDTO
                    {
                        Id = BannerDAO.Image.Id,
                        Name = BannerDAO.Image.Name,
                        Url = BannerDAO.Image.Url,
                    });
                }

                if (BannerDAO.CreatedAt >= Timestamp)
                    MobileSync_BannerSyncDTO.Created.Add(MobileSync_BannerDTO);
                else if (BannerDAO.DeletedAt >= Timestamp)
                    MobileSync_BannerSyncDTO.Deleted.Add(MobileSync_BannerDTO);
                else
                    MobileSync_BannerSyncDTO.Updated.Add(MobileSync_BannerDTO);
            }
            return MobileSync_BannerSyncDTO;
        }

        private async Task<MobileSync_ProductSyncDTO> BuildProduct(DateTime Timestamp)
        {
            MobileSync_ProductSyncDTO MobileSync_ProductSyncDTO = new MobileSync_ProductSyncDTO()
            {
                Created = new List<MobileSync_ProductDTO>(),
                Updated = new List<MobileSync_ProductDTO>(),
                Deleted = new List<MobileSync_ProductDTO>()
            };
            List<ProductDAO> ProductDAOs = await DataContext.Product
                .Where(x => x.UpdatedAt >= Timestamp && x.StatusId == StatusEnum.ACTIVE.Id)
                .Include(x => x.ProductType)
                .Include(x => x.Brand)
                .Include(x => x.TaxType)
                .Include(x => x.UnitOfMeasure)
                .Include(x => x.UsedVariation)
                .ToListWithNoLockAsync();

            List<ProductImageMappingDAO> ProductImageMappings = await DataContext.ProductImageMapping
                .Where(x => x.Product.UpdatedAt >= Timestamp)
                .Include(x => x.Image)
                .ToListWithNoLockAsync();

            List<ProductProductGroupingMappingDAO> ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping
                .Where(x => x.Product.UpdatedAt >= Timestamp)
                .Include(x => x.ProductGrouping)
                .ToListWithNoLockAsync();

            List<ItemDAO> ItemDAOs = await DataContext.Item
                .Where(x => x.Product.UpdatedAt >= Timestamp)
                .ToListWithNoLockAsync();
            List<ItemImageMappingDAO> ItemImageMappingDAOs = await DataContext.ItemImageMapping
                .Where(x => x.Item.UpdatedAt >= Timestamp)
                .ToListWithNoLockAsync();

            List<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupings =
                await (from uomg in DataContext.UnitOfMeasureGrouping
                       join p in DataContext.Product on uomg.Id equals p.UnitOfMeasureGroupingId
                       where p.UpdatedAt >= Timestamp || p.Items.Any(i => i.UpdatedAt >= Timestamp)
                       select uomg)
                .Include(x => x.UnitOfMeasureGroupingContents)
                .ToListWithNoLockAsync();
            foreach (ProductDAO ProductDAO in ProductDAOs)
            {
                ProductDAO.ProductImageMappings = ProductImageMappings.Where(x => x.ProductId == ProductDAO.Id).ToList();
                ProductDAO.ProductProductGroupingMappings = ProductProductGroupingMappings
                    .Where(x => x.ProductId == ProductDAO.Id).ToList();
                ProductDAO.Items = ItemDAOs.Where(x => x.ProductId == x.ProductId).ToList();
                foreach (ItemDAO ItemDAO in ProductDAO.Items)
                {
                    ItemDAO.ItemImageMappings = ItemImageMappingDAOs.Where(x => x.ItemId == ItemDAO.Id).ToList();
                }
                ProductDAO.UnitOfMeasureGrouping = UnitOfMeasureGroupings.Where(x =>
                    ProductDAO.UnitOfMeasureGroupingId.HasValue &&
                    x.Id == ProductDAO.UnitOfMeasureGroupingId.Value)
                    .FirstOrDefault();

            }

            foreach (ProductDAO ProductDAO in ProductDAOs)
            {
                MobileSync_ProductDTO MobileSync_ProductDTO = new MobileSync_ProductDTO
                {
                    BrandId = ProductDAO.BrandId,
                    Code = ProductDAO.Code,
                    Description = ProductDAO.Description,
                    ERPCode = ProductDAO.ERPCode,
                    Id = ProductDAO.Id,
                    Name = ProductDAO.Name,
                    Note = ProductDAO.Note,
                    OtherName = ProductDAO.OtherName,
                    ProductTypeId = ProductDAO.ProductTypeId,
                    RetailPrice = ProductDAO.RetailPrice,
                    SalePrice = ProductDAO.SalePrice,
                    ScanCode = ProductDAO.ScanCode,
                    StatusId = ProductDAO.StatusId,
                    TaxTypeId = ProductDAO.TaxTypeId,
                    TechnicalName = ProductDAO.TechnicalName,
                    UnitOfMeasureGroupingId = ProductDAO.UnitOfMeasureGroupingId,
                    UnitOfMeasureId = ProductDAO.UnitOfMeasureId,
                    ProductProductGroupingMappings = ProductDAO.ProductProductGroupingMappings?.Select(x => new MobileSync_ProductProductGroupingMappingDTO
                    {
                        ProductGroupingId = x.ProductGroupingId,
                        ProductId = x.ProductId,
                        ProductGrouping = x.ProductGrouping == null ? null : new MobileSync_ProductGroupingDTO
                        {
                            Code = x.ProductGrouping.Code,
                            Description = x.ProductGrouping.Description,
                            Id = x.ProductGrouping.Id,
                            Name = x.ProductGrouping.Name,
                            ParentId = x.ProductGrouping.ParentId,
                            Path = x.ProductGrouping.Path,
                        },
                    }).ToList(),
                    ProductType = ProductDAO.ProductType == null ? null : new MobileSync_ProductTypeDTO
                    {
                        Code = ProductDAO.ProductType.Code,
                        Description = ProductDAO.ProductType.Description,
                        Id = ProductDAO.ProductType.Id,
                        Name = ProductDAO.ProductType.Name,
                        StatusId = ProductDAO.ProductType.StatusId,
                        UpdatedAt = ProductDAO.ProductType.UpdatedAt,
                    },
                    TaxType = ProductDAO.TaxType == null ? null : new MobileSync_TaxTypeDTO
                    {
                        Id = ProductDAO.TaxType.Id,
                        Code = ProductDAO.TaxType.Code,
                        Name = ProductDAO.TaxType.Name,
                    },
                    UnitOfMeasure = ProductDAO.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO
                    {
                        Id = ProductDAO.UnitOfMeasure.Id,
                        Code = ProductDAO.UnitOfMeasure.Code,
                        Name = ProductDAO.UnitOfMeasure.Name,
                    },
                    UnitOfMeasureGrouping = ProductDAO.UnitOfMeasureGrouping == null ? null : new MobileSync_UnitOfMeasureGroupingDTO
                    {
                        Id = ProductDAO.UnitOfMeasureGrouping.Id,
                        Code = ProductDAO.UnitOfMeasureGrouping.Code,
                        Name = ProductDAO.UnitOfMeasureGrouping.Name,
                        UnitOfMeasure = ProductDAO.UnitOfMeasureGrouping.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO
                        {
                            Id = ProductDAO.UnitOfMeasureGrouping.UnitOfMeasure.Id,
                            Code = ProductDAO.UnitOfMeasureGrouping.UnitOfMeasure.Code,
                            Name = ProductDAO.UnitOfMeasureGrouping.UnitOfMeasure.Name,
                        },
                        UnitOfMeasureGroupingContents = ProductDAO.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents?.Select(x => new MobileSync_UnitOfMeasureGroupingContentDTO
                        {
                            Factor = x.Factor,
                            Id = x.Id,
                            UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                            UnitOfMeasureId = x.UnitOfMeasureId,
                            UnitOfMeasure = x.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO
                            {
                                Id = x.UnitOfMeasure.Id,
                                Code = x.UnitOfMeasure.Code,
                                Name = x.UnitOfMeasure.Name,
                            }
                        }).ToList(),
                    },
                };
                if (ProductDAO.CreatedAt >= Timestamp)
                    MobileSync_ProductSyncDTO.Created.Add(MobileSync_ProductDTO);
                else if (ProductDAO.DeletedAt >= Timestamp)
                    MobileSync_ProductSyncDTO.Deleted.Add(MobileSync_ProductDTO);
                else
                    MobileSync_ProductSyncDTO.Updated.Add(MobileSync_ProductDTO);
            }
            return MobileSync_ProductSyncDTO;
        }

        private async Task<MobileSync_ItemSyncDTO> BuildItem(DateTime Timestamp)
        {
            MobileSync_ItemSyncDTO MobileSync_ItemSyncDTO = new MobileSync_ItemSyncDTO
            {
                Created = new List<MobileSync_ItemDTO>(),
                Updated = new List<MobileSync_ItemDTO>(),
                Deleted = new List<MobileSync_ItemDTO>()
            };

            List<ItemDAO> ItemDAOs = await DataContext.Item
                .Where(x => x.UpdatedAt >= Timestamp || x.Product.UpdatedAt >= Timestamp)
                .Include(x => x.Status)
                .ToListWithNoLockAsync();
            List<ItemImageMappingDAO> ItemImageMappingDAOs = await DataContext.ItemImageMapping
                .Where(x => x.Item.UpdatedAt >= Timestamp || x.Item.Product.UpdatedAt >= Timestamp)
                .Include(x => x.Image)
                .ToListWithNoLockAsync();

            List<ProductDAO> ProductDAOs = await DataContext.Product
                                                .Where(x => x.UpdatedAt >= Timestamp || x.Items.Any(i => i.UpdatedAt >= Timestamp))
                                                 .Include(x => x.UnitOfMeasure)
                                                 .Include(x => x.Brand)
                                                 .Include(x => x.ProductType)
                                                 .Include(x => x.UnitOfMeasure)
                                                 .ToListWithNoLockAsync();
            List<ProductImageMappingDAO> ProductImageMappingDAOs = await DataContext.ProductImageMapping
                                                .Where(x => x.Product.UpdatedAt >= Timestamp || x.Product.Items.Any(i => i.UpdatedAt >= Timestamp))
                                                .ToListWithNoLockAsync();
            List<ProductProductGroupingMappingDAO> ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping
               .Where(x => x.Product.UpdatedAt >= Timestamp || x.Product.Items.Any(i => i.UpdatedAt >= Timestamp))
               .Include(x => x.ProductGrouping)
               .ToListWithNoLockAsync();
            List<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs =
             await (from uomg in DataContext.UnitOfMeasureGrouping
                    join p in DataContext.Product on uomg.Id equals p.UnitOfMeasureGroupingId
                    where p.UpdatedAt >= Timestamp || p.Items.Any(i => i.UpdatedAt >= Timestamp)
                    select uomg)
                    .Include(x => x.UnitOfMeasure)
                    .Include(x => x.UnitOfMeasureGroupingContents).ThenInclude(y => y.UnitOfMeasure)
                    .ToListWithNoLockAsync();

            foreach (ItemDAO ItemDAO in ItemDAOs)
            {
                ItemDAO.ItemImageMappings = ItemImageMappingDAOs.Where(x => x.ItemId == ItemDAO.Id).ToList();
                ItemDAO.Product = ProductDAOs.Where(x => x.Id == ItemDAO.ProductId).FirstOrDefault();
                ItemDAO.Product.ProductImageMappings = ProductImageMappingDAOs.Where(x => x.ProductId == ItemDAO.ProductId).ToList();
                ItemDAO.Product.UnitOfMeasureGrouping = UnitOfMeasureGroupingDAOs
                    .Where(x => ItemDAO.Product.UnitOfMeasureGroupingId.HasValue && x.Id == ItemDAO.Product.UnitOfMeasureGroupingId.Value)
                    .FirstOrDefault();
                ItemDAO.Product.ProductProductGroupingMappings = ProductProductGroupingMappings.Where(x => x.ProductId == ItemDAO.ProductId).ToList();

            }

            foreach (ItemDAO ItemDAO in ItemDAOs)
            {
                MobileSync_ItemDTO MobileSync_ItemDTO = new MobileSync_ItemDTO
                {
                    Id = ItemDAO.Id,
                    Code = ItemDAO.Code,
                    Name = ItemDAO.Name,
                    ProductId = ItemDAO.ProductId,
                    RetailPrice = ItemDAO.RetailPrice,
                    SalePrice = ItemDAO.SalePrice,
                    ScanCode = ItemDAO.ScanCode,
                    StatusId = ItemDAO.StatusId,
                    Status = ItemDAO.Status == null ? null : new MobileSync_StatusDTO
                    {
                        Id = ItemDAO.Status.Id,
                        Code = ItemDAO.Status.Code,
                        Name = ItemDAO.Status.Name,
                    },
                    ItemImageMappings = ItemDAO.ItemImageMappings?.Select(x => new MobileSync_ItemImageMappingDTO
                    {
                        ImageId = x.ImageId,
                        ItemId = x.ItemId,
                        Image = x.Image == null ? null : new MobileSync_ImageDTO
                        {
                            Id = x.Image.Id,
                            Name = x.Image.Name,
                            Url = x.Image.Url,
                        }
                    }).ToList(),
                    Product = ItemDAO.Product == null ? null : new MobileSync_ProductDTO
                    {
                        BrandId = ItemDAO.Product.BrandId,
                        Code = ItemDAO.Product.Code,
                        Description = ItemDAO.Product.Description,
                        ERPCode = ItemDAO.Product.ERPCode,
                        Id = ItemDAO.Product.Id,
                        Name = ItemDAO.Product.Name,
                        Note = ItemDAO.Product.Note,
                        OtherName = ItemDAO.Product.OtherName,
                        ProductTypeId = ItemDAO.Product.ProductTypeId,
                        RetailPrice = ItemDAO.Product.RetailPrice,
                        SalePrice = ItemDAO.Product.SalePrice,
                        ScanCode = ItemDAO.Product.ScanCode,
                        StatusId = ItemDAO.Product.StatusId,
                        TaxTypeId = ItemDAO.Product.TaxTypeId,
                        TechnicalName = ItemDAO.Product.TechnicalName,
                        UnitOfMeasureGroupingId = ItemDAO.Product.UnitOfMeasureGroupingId,
                        UnitOfMeasureId = ItemDAO.Product.UnitOfMeasureId,
                        ProductProductGroupingMappings = ItemDAO.Product.ProductProductGroupingMappings?.Select(x => new MobileSync_ProductProductGroupingMappingDTO
                        {
                            ProductGroupingId = x.ProductGroupingId,
                            ProductId = x.ProductId,
                            ProductGrouping = x.ProductGrouping == null ? null : new MobileSync_ProductGroupingDTO
                            {
                                Code = x.ProductGrouping.Code,
                                Name = x.ProductGrouping.Name,
                                Id = x.ProductGrouping.Id,
                            }
                        }).ToList(),
                        UnitOfMeasure = ItemDAO.Product.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO
                        {
                            Id = ItemDAO.Product.UnitOfMeasure.Id,
                            Code = ItemDAO.Product.UnitOfMeasure.Code,
                            Name = ItemDAO.Product.UnitOfMeasure.Name,
                        },
                        UnitOfMeasureGrouping = ItemDAO.Product.UnitOfMeasureGrouping == null ? null : new MobileSync_UnitOfMeasureGroupingDTO
                        {
                            Id = ItemDAO.Product.UnitOfMeasureGrouping.Id,
                            Code = ItemDAO.Product.UnitOfMeasureGrouping.Code,
                            Name = ItemDAO.Product.UnitOfMeasureGrouping.Name,
                            UnitOfMeasureGroupingContents = ItemDAO.Product.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents
                            ?.Select(x => new MobileSync_UnitOfMeasureGroupingContentDTO
                            {
                                Id = x.Id,
                                Factor = x.Factor,
                                UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                                UnitOfMeasureId = x.UnitOfMeasureId,
                                UnitOfMeasure = x.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO
                                {
                                    Id = x.UnitOfMeasure.Id,
                                    Code = x.UnitOfMeasure.Code,
                                    Name = x.UnitOfMeasure.Name,
                                }
                            }).ToList(),
                        },
                        ProductType = ItemDAO.Product.ProductType == null ? null : new MobileSync_ProductTypeDTO
                        {
                            Id = ItemDAO.Product.ProductType.Id,
                            Code = ItemDAO.Product.ProductType.Code,
                            Name = ItemDAO.Product.ProductType.Name,
                        },
                        TaxType = ItemDAO.Product.TaxType == null ? null : new MobileSync_TaxTypeDTO
                        {
                            Id = ItemDAO.Product.TaxType.Id,
                            Code = ItemDAO.Product.TaxType.Code,
                            Name = ItemDAO.Product.TaxType.Name,
                        },
                    },
                };

                if (ItemDAO.CreatedAt >= Timestamp)
                    MobileSync_ItemSyncDTO.Created.Add(MobileSync_ItemDTO);
                else if (ItemDAO.DeletedAt >= Timestamp)
                    MobileSync_ItemSyncDTO.Deleted.Add(MobileSync_ItemDTO);
                else
                    MobileSync_ItemSyncDTO.Updated.Add(MobileSync_ItemDTO);
            }
            return MobileSync_ItemSyncDTO;
        }

        private async Task<MobileSync_StoreSyncDTO> BuildStoreInScoped(DateTime Timestamp)
        {
            List<Store> Stores = await StoreCheckingService.ListStoreInScope(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            }, null);
            List<MobileSync_StoreDTO> MobileSync_StoreDTOs = Stores.Select(x => new MobileSync_StoreDTO(x)).ToList();
            MobileSync_StoreSyncDTO MobileSync_StoreSyncDTO = new MobileSync_StoreSyncDTO
            {
                Created = MobileSync_StoreDTOs,
            };
            return MobileSync_StoreSyncDTO;
        }

        private async Task<MobileSync_StoreSyncDTO> BuildStorePlanned(DateTime Timestamp)
        {
            List<Store> Stores = await StoreCheckingService.ListStorePlanned(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            }, null);
            List<MobileSync_StoreDTO> MobileSync_StoreDTOs = Stores.Select(x => new MobileSync_StoreDTO(x)).ToList();
            MobileSync_StoreSyncDTO MobileSync_StoreSyncDTO = new MobileSync_StoreSyncDTO
            {
                Created = MobileSync_StoreDTOs,
            };
            return MobileSync_StoreSyncDTO;
        }
        private async Task<MobileSync_StoreSyncDTO> BuildStoreUnplanned(DateTime Timestamp)
        {
            List<Store> Stores = await StoreCheckingService.ListStorePlanned(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            }, null);
            List<MobileSync_StoreDTO> MobileSync_StoreDTOs = Stores.Select(x => new MobileSync_StoreDTO(x)).ToList();
            MobileSync_StoreSyncDTO MobileSync_StoreSyncDTO = new MobileSync_StoreSyncDTO
            {
                Created = MobileSync_StoreDTOs,
            };
            return MobileSync_StoreSyncDTO;
        }

        private async Task<MobileSync_StoreScoutingSyncDTO> BuildStoreScouting(DateTime Timestamp)
        {
            AppUserDAO AppUserDAO = await DataContext.AppUser.Where(x => x.Id == CurrentContext.UserId).FirstOrDefaultWithNoLockAsync();
            List<StoreScoutingDAO> StoreScoutingDAOs = await DataContext.StoreScouting
                .Where(x => x.UpdatedAt >= Timestamp)
                .ToListWithNoLockAsync();
            MobileSync_StoreScoutingSyncDTO MobileSync_StoreScoutingSyncDTO = new MobileSync_StoreScoutingSyncDTO()
            {

            };
            return MobileSync_StoreScoutingSyncDTO;
        }
    }
}
