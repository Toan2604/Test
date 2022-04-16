using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using DMS.Services.MPromotion;
using DMS.Services.MPromotionCombo;
using DMS.Services.MPromotionDirectSalesOrder;
using DMS.Services.MPromotionDiscountType;
using DMS.Services.MPromotionPolicy;
using DMS.Services.MPromotionProduct;
using DMS.Services.MPromotionProductGrouping;
using DMS.Services.MPromotionProductType;
using DMS.Services.MPromotionSamePrice;
using DMS.Services.MPromotionStore;
using DMS.Services.MPromotionStoreGrouping;
using DMS.Services.MPromotionStoreType;
using DMS.Services.MPromotionType;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public partial class PromotionController : RpcController
    {
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IPromotionTypeService PromotionTypeService;
        private IStatusService StatusService;
        private IPromotionComboService PromotionComboService;
        private IPromotionPolicyService PromotionPolicyService;
        private IPromotionDirectSalesOrderService PromotionDirectSalesOrderService;
        private IPromotionDiscountTypeService PromotionDiscountTypeService;
        private IPromotionProductGroupingService PromotionProductGroupingService;
        private IProductGroupingService ProductGroupingService;
        private IPromotionProductTypeService PromotionProductTypeService;
        private IProductTypeService ProductTypeService;
        private IPromotionProductService PromotionProductService;
        private IProductService ProductService;
        private IPromotionSamePriceService PromotionSamePriceService;
        private IStoreGroupingService StoreGroupingService;
        private IPromotionStoreGroupingService PromotionStoreGroupingService;
        private IStoreService StoreService;
        private IStoreTypeService StoreTypeService;
        private IPromotionStoreTypeService PromotionStoreTypeService;
        private IPromotionStoreService PromotionStoreService;
        private IPromotionService PromotionService;
        private ICurrentContext CurrentContext;
        public PromotionController(
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IPromotionTypeService PromotionTypeService,
            IStatusService StatusService,
            IPromotionComboService PromotionComboService,
            IPromotionPolicyService PromotionPolicyService,
            IPromotionDirectSalesOrderService PromotionDirectSalesOrderService,
            IPromotionDiscountTypeService PromotionDiscountTypeService,
            IPromotionProductGroupingService PromotionProductGroupingService,
            IProductGroupingService ProductGroupingService,
            IPromotionProductTypeService PromotionProductTypeService,
            IProductTypeService ProductTypeService,
            IPromotionProductService PromotionProductService,
            IProductService ProductService,
            IPromotionSamePriceService PromotionSamePriceService,
            IStoreGroupingService StoreGroupingService,
            IPromotionStoreGroupingService PromotionStoreGroupingService,
            IStoreService StoreService,
            IStoreTypeService StoreTypeService,
            IPromotionStoreTypeService PromotionStoreTypeService,
            IPromotionStoreService PromotionStoreService,
            IPromotionService PromotionService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.PromotionTypeService = PromotionTypeService;
            this.StatusService = StatusService;
            this.PromotionComboService = PromotionComboService;
            this.PromotionPolicyService = PromotionPolicyService;
            this.PromotionDirectSalesOrderService = PromotionDirectSalesOrderService;
            this.PromotionDiscountTypeService = PromotionDiscountTypeService;
            this.PromotionProductGroupingService = PromotionProductGroupingService;
            this.ProductGroupingService = ProductGroupingService;
            this.PromotionProductTypeService = PromotionProductTypeService;
            this.ProductTypeService = ProductTypeService;
            this.PromotionProductService = PromotionProductService;
            this.ProductService = ProductService;
            this.PromotionSamePriceService = PromotionSamePriceService;
            this.StoreGroupingService = StoreGroupingService;
            this.PromotionStoreGroupingService = PromotionStoreGroupingService;
            this.StoreService = StoreService;
            this.StoreTypeService = StoreTypeService;
            this.PromotionStoreTypeService = PromotionStoreTypeService;
            this.PromotionStoreService = PromotionStoreService;
            this.PromotionService = PromotionService;
            this.CurrentContext = CurrentContext;
        }

        [Route(PromotionRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Promotion_PromotionFilterDTO Promotion_PromotionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionFilter PromotionFilter = ConvertFilterDTOToFilterEntity(Promotion_PromotionFilterDTO);
            PromotionFilter = await PromotionService.ToFilter(PromotionFilter);
            int count = await PromotionService.Count(PromotionFilter);
            return count;
        }

        [Route(PromotionRoute.List), HttpPost]
        public async Task<ActionResult<List<Promotion_PromotionDTO>>> List([FromBody] Promotion_PromotionFilterDTO Promotion_PromotionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionFilter PromotionFilter = ConvertFilterDTOToFilterEntity(Promotion_PromotionFilterDTO);
            PromotionFilter = await PromotionService.ToFilter(PromotionFilter);
            List<Promotion> Promotions = await PromotionService.List(PromotionFilter);
            List<Promotion_PromotionDTO> Promotion_PromotionDTOs = Promotions
                .Select(c => new Promotion_PromotionDTO(c)).ToList();
            return Promotion_PromotionDTOs;
        }

        [Route(PromotionRoute.Get), HttpPost]
        public async Task<ActionResult<Promotion_PromotionDTO>> Get([FromBody] Promotion_PromotionDTO Promotion_PromotionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionDTO.Id))
                return Forbid();

            Promotion Promotion = await PromotionService.Get(Promotion_PromotionDTO.Id);
            return new Promotion_PromotionDTO(Promotion);
        }

        [Route(PromotionRoute.GetMpping), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> GetMpping([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = await PromotionPolicyService.GetMapping(Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicyId, Promotion_PromotionPromotionPolicyMappingDTO.PromotionId);
            return new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
        }

        [Route(PromotionRoute.CreateDraft), HttpPost]
        public async Task<ActionResult<Promotion_PromotionDTO>> CreateDraft()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Promotion Promotion = await PromotionService.CreateDraft();
            Promotion_PromotionDTO Promotion_PromotionDTO = new Promotion_PromotionDTO(Promotion);
            if (Promotion.IsValidated)
                return Promotion_PromotionDTO;
            else
                return BadRequest(Promotion_PromotionDTO);
        }

        [Route(PromotionRoute.Create), HttpPost]
        public async Task<ActionResult<Promotion_PromotionDTO>> Create([FromBody] Promotion_PromotionDTO Promotion_PromotionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionDTO.Id))
                return Forbid();

            Promotion Promotion = ConvertDTOToEntity(Promotion_PromotionDTO);
            Promotion = await PromotionService.Create(Promotion);
            Promotion_PromotionDTO = new Promotion_PromotionDTO(Promotion);
            if (Promotion.IsValidated)
                return Promotion_PromotionDTO;
            else
                return BadRequest(Promotion_PromotionDTO);
        }

        [Route(PromotionRoute.Update), HttpPost]
        public async Task<ActionResult<Promotion_PromotionDTO>> Update([FromBody] Promotion_PromotionDTO Promotion_PromotionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionDTO.Id))
                return Forbid();

            Promotion Promotion = ConvertDTOToEntity(Promotion_PromotionDTO);
            Promotion = await PromotionService.Update(Promotion);
            Promotion_PromotionDTO = new Promotion_PromotionDTO(Promotion);
            if (Promotion.IsValidated)
                return Promotion_PromotionDTO;
            else
                return BadRequest(Promotion_PromotionDTO);
        }

        [Route(PromotionRoute.UpdateDirectSalesOrder), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> UpdateDirectSalesOrder([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
            PromotionPromotionPolicyMapping.Note = Promotion_PromotionPromotionPolicyMappingDTO.Note;
            PromotionPromotionPolicyMapping.PromotionPolicy = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy == null ? null : new PromotionPolicy
            {
                Id = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Id,
                Code = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Code,
                Name = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Name,
            };
            PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.PromotionDirectSalesOrders?
                .Select(x => new PromotionDirectSalesOrder
                {
                    Id = x.Id,
                    PromotionPolicyId = x.PromotionPolicyId,
                    Note = x.Note,
                    FromValue = x.FromValue,
                    ToValue = x.ToValue,
                    PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                    PromotionId = x.PromotionId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountValue = x.DiscountValue,
                    Price = x.Price,
                    PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                    {
                        Id = x.PromotionDiscountType.Id,
                        Code = x.PromotionDiscountType.Code,
                        Name = x.PromotionDiscountType.Name,
                    },
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    PromotionDirectSalesOrderItemMappings = x.PromotionDirectSalesOrderItemMappings?.Select(x => new PromotionDirectSalesOrderItemMapping
                    {
                        PromotionDirectSalesOrderId = x.PromotionDirectSalesOrderId,
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ItemImageMappings = x.Item.ItemImageMappings.Select(i => new ItemImageMapping
                            {
                                ItemId = i.ItemId,
                                ImageId = i.ImageId,
                                Image = i.Image == null ? null : new Image
                                {
                                    Id = i.Image.Id,
                                    Url = i.Image.Url,
                                    ThumbnailUrl = i.Image.ThumbnailUrl,
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList();
            PromotionPromotionPolicyMapping = await PromotionService.UpdateDirectSalesOrder(PromotionPromotionPolicyMapping);
            Promotion_PromotionPromotionPolicyMappingDTO = new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
            if (PromotionPromotionPolicyMapping.IsValidated)
                return Promotion_PromotionPromotionPolicyMappingDTO;
            else
                return BadRequest(Promotion_PromotionPromotionPolicyMappingDTO);
        }

        [Route(PromotionRoute.UpdateProduct), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> UpdateProduct([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
            PromotionPromotionPolicyMapping.Note = Promotion_PromotionPromotionPolicyMappingDTO.Note;
            PromotionPromotionPolicyMapping.PromotionPolicy = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy == null ? null : new PromotionPolicy
            {
                Id = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Id,
                Code = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Code,
                Name = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Name,
            };
            PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProducts = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.PromotionProducts?
                .Select(x => new PromotionProduct
                {
                    Id = x.Id,
                    PromotionPolicyId = x.PromotionPolicyId,
                    ProductId = x.ProductId,
                    Note = x.Note,
                    FromValue = x.FromValue,
                    ToValue = x.ToValue,
                    PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                    PromotionId = x.PromotionId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountValue = x.DiscountValue,
                    Price = x.Price,
                    Product = x.Product == null ? null : new Product
                    {
                        Id = x.Product.Id,
                        Code = x.Product.Code,
                        Name = x.Product.Name,
                    },
                    PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                    {
                        Id = x.PromotionDiscountType.Id,
                        Code = x.PromotionDiscountType.Code,
                        Name = x.PromotionDiscountType.Name,
                    },
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    PromotionProductItemMappings = x.PromotionProductItemMappings?.Select(x => new PromotionProductItemMapping
                    {
                        PromotionProductId = x.PromotionProductId,
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ItemImageMappings = x.Item.ItemImageMappings.Select(i => new ItemImageMapping
                            {
                                ItemId = i.ItemId,
                                ImageId = i.ImageId,
                                Image = i.Image == null ? null : new Image
                                {
                                    Id = i.Image.Id,
                                    Url = i.Image.Url,
                                    ThumbnailUrl = i.Image.ThumbnailUrl,
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList();
            PromotionPromotionPolicyMapping = await PromotionService.UpdateProduct(PromotionPromotionPolicyMapping);
            Promotion_PromotionPromotionPolicyMappingDTO = new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
            if (PromotionPromotionPolicyMapping.IsValidated)
                return Promotion_PromotionPromotionPolicyMappingDTO;
            else
                return BadRequest(Promotion_PromotionPromotionPolicyMappingDTO);
        }

        [Route(PromotionRoute.UpdateProductGrouping), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> UpdateProductGrouping([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
            PromotionPromotionPolicyMapping.Note = Promotion_PromotionPromotionPolicyMappingDTO.Note;
            PromotionPromotionPolicyMapping.PromotionPolicy = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy == null ? null : new PromotionPolicy
            {
                Id = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Id,
                Code = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Code,
                Name = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Name,
            };
            PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductGroupings = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.PromotionProductGroupings?
                .Select(x => new PromotionProductGrouping
                {
                    Id = x.Id,
                    PromotionPolicyId = x.PromotionPolicyId,
                    ProductGroupingId = x.ProductGroupingId,
                    Note = x.Note,
                    FromValue = x.FromValue,
                    ToValue = x.ToValue,
                    PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                    PromotionId = x.PromotionId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountValue = x.DiscountValue,
                    Price = x.Price,
                    ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                    {
                        Id = x.ProductGrouping.Id,
                        Code = x.ProductGrouping.Code,
                        Name = x.ProductGrouping.Name,
                    },
                    PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                    {
                        Id = x.PromotionDiscountType.Id,
                        Code = x.PromotionDiscountType.Code,
                        Name = x.PromotionDiscountType.Name,
                    },
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    PromotionProductGroupingItemMappings = x.PromotionProductGroupingItemMappings?.Select(x => new PromotionProductGroupingItemMapping
                    {
                        PromotionProductGroupingId = x.PromotionProductGroupingId,
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ItemImageMappings = x.Item.ItemImageMappings.Select(i => new ItemImageMapping
                            {
                                ItemId = i.ItemId,
                                ImageId = i.ImageId,
                                Image = i.Image == null ? null : new Image
                                {
                                    Id = i.Image.Id,
                                    Url = i.Image.Url,
                                    ThumbnailUrl = i.Image.ThumbnailUrl,
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList();
            PromotionPromotionPolicyMapping = await PromotionService.UpdateProductGrouping(PromotionPromotionPolicyMapping);
            Promotion_PromotionPromotionPolicyMappingDTO = new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
            if (PromotionPromotionPolicyMapping.IsValidated)
                return Promotion_PromotionPromotionPolicyMappingDTO;
            else
                return BadRequest(Promotion_PromotionPromotionPolicyMappingDTO);
        }

        [Route(PromotionRoute.UpdateProductType), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> UpdateProductType([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
            PromotionPromotionPolicyMapping.Note = Promotion_PromotionPromotionPolicyMappingDTO.Note;
            PromotionPromotionPolicyMapping.PromotionPolicy = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy == null ? null : new PromotionPolicy
            {
                Id = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Id,
                Code = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Code,
                Name = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Name,
            };
            PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductTypes = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.PromotionProductTypes?
                .Select(x => new PromotionProductType
                {
                    Id = x.Id,
                    PromotionPolicyId = x.PromotionPolicyId,
                    ProductTypeId = x.ProductTypeId,
                    Note = x.Note,
                    FromValue = x.FromValue,
                    ToValue = x.ToValue,
                    PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                    PromotionId = x.PromotionId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountValue = x.DiscountValue,
                    Price = x.Price,
                    ProductType = x.ProductType == null ? null : new ProductType
                    {
                        Id = x.ProductType.Id,
                        Code = x.ProductType.Code,
                        Name = x.ProductType.Name,
                    },
                    PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                    {
                        Id = x.PromotionDiscountType.Id,
                        Code = x.PromotionDiscountType.Code,
                        Name = x.PromotionDiscountType.Name,
                    },
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    PromotionProductTypeItemMappings = x.PromotionProductTypeItemMappings?.Select(x => new PromotionProductTypeItemMapping
                    {
                        PromotionProductTypeId = x.PromotionProductTypeId,
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ItemImageMappings = x.Item.ItemImageMappings.Select(i => new ItemImageMapping
                            {
                                ItemId = i.ItemId,
                                ImageId = i.ImageId,
                                Image = i.Image == null ? null : new Image
                                {
                                    Id = i.Image.Id,
                                    Url = i.Image.Url,
                                    ThumbnailUrl = i.Image.ThumbnailUrl,
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList();
            PromotionPromotionPolicyMapping = await PromotionService.UpdateProductType(PromotionPromotionPolicyMapping);
            Promotion_PromotionPromotionPolicyMappingDTO = new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
            if (PromotionPromotionPolicyMapping.IsValidated)
                return Promotion_PromotionPromotionPolicyMappingDTO;
            else
                return BadRequest(Promotion_PromotionPromotionPolicyMappingDTO);
        }

        [Route(PromotionRoute.UpdateCombo), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> UpdateCombo([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
            PromotionPromotionPolicyMapping.Note = Promotion_PromotionPromotionPolicyMappingDTO.Note;
            PromotionPromotionPolicyMapping.PromotionPolicy = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy == null ? null : new PromotionPolicy
            {
                Id = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Id,
                Code = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Code,
                Name = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Name,
            };
            PromotionPromotionPolicyMapping.PromotionPolicy.PromotionCombos = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.PromotionCombos?
                .Select(x => new PromotionCombo
                {
                    Id = x.Id,
                    PromotionPolicyId = x.PromotionPolicyId,
                    Name = x.Name,
                    Note = x.Note,
                    PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                    PromotionId = x.PromotionId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountValue = x.DiscountValue,
                    Price = x.Price,
                    PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                    {
                        Id = x.PromotionDiscountType.Id,
                        Code = x.PromotionDiscountType.Code,
                        Name = x.PromotionDiscountType.Name,
                    },
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    PromotionComboInItemMappings = x.PromotionComboInItemMappings?.Select(x => new PromotionComboInItemMapping
                    {
                        PromotionComboId = x.PromotionComboId,
                        ItemId = x.ItemId,
                        From = x.From,
                        To = x.To,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ItemImageMappings = x.Item.ItemImageMappings.Select(i => new ItemImageMapping
                            {
                                ItemId = i.ItemId,
                                ImageId = i.ImageId,
                                Image = i.Image == null ? null : new Image
                                {
                                    Id = i.Image.Id,
                                    Url = i.Image.Url,
                                    ThumbnailUrl = i.Image.ThumbnailUrl,
                                }
                            }).ToList()
                        }
                    }).ToList(),
                    PromotionComboOutItemMappings = x.PromotionComboOutItemMappings?.Select(x => new PromotionComboOutItemMapping
                    {
                        PromotionComboId = x.PromotionComboId,
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ItemImageMappings = x.Item.ItemImageMappings.Select(i => new ItemImageMapping
                            {
                                ItemId = i.ItemId,
                                ImageId = i.ImageId,
                                Image = i.Image == null ? null : new Image
                                {
                                    Id = i.Image.Id,
                                    Url = i.Image.Url,
                                    ThumbnailUrl = i.Image.ThumbnailUrl,
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList();
            PromotionPromotionPolicyMapping = await PromotionService.UpdateCombo(PromotionPromotionPolicyMapping);
            Promotion_PromotionPromotionPolicyMappingDTO = new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
            if (PromotionPromotionPolicyMapping.IsValidated)
                return Promotion_PromotionPromotionPolicyMappingDTO;
            else
                return BadRequest(Promotion_PromotionPromotionPolicyMappingDTO);
        }

        [Route(PromotionRoute.UpdateSamePrice), HttpPost]
        public async Task<ActionResult<Promotion_PromotionPromotionPolicyMappingDTO>> UpdateSamePrice([FromBody] Promotion_PromotionPromotionPolicyMappingDTO Promotion_PromotionPromotionPolicyMappingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionPromotionPolicyMappingDTO.PromotionId))
                return Forbid();

            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
            PromotionPromotionPolicyMapping.Note = Promotion_PromotionPromotionPolicyMappingDTO.Note;
            PromotionPromotionPolicyMapping.PromotionPolicy = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy == null ? null : new PromotionPolicy
            {
                Id = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Id,
                Code = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Code,
                Name = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.Name,
            };
            PromotionPromotionPolicyMapping.PromotionPolicy.PromotionSamePrices = Promotion_PromotionPromotionPolicyMappingDTO.PromotionPolicy.PromotionSamePrices?
                .Select(x => new PromotionSamePrice
                {
                    Id = x.Id,
                    PromotionPolicyId = x.PromotionPolicyId,
                    Note = x.Note,
                    Price = x.Price,
                    PromotionId = x.PromotionId,
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                }).ToList();
            PromotionPromotionPolicyMapping = await PromotionService.UpdateSamePrice(PromotionPromotionPolicyMapping);
            Promotion_PromotionPromotionPolicyMappingDTO = new Promotion_PromotionPromotionPolicyMappingDTO(PromotionPromotionPolicyMapping);
            if (PromotionPromotionPolicyMapping.IsValidated)
                return Promotion_PromotionPromotionPolicyMappingDTO;
            else
                return BadRequest(Promotion_PromotionPromotionPolicyMappingDTO);
        }

        [Route(PromotionRoute.Delete), HttpPost]
        public async Task<ActionResult<Promotion_PromotionDTO>> Delete([FromBody] Promotion_PromotionDTO Promotion_PromotionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Promotion_PromotionDTO.Id))
                return Forbid();

            Promotion Promotion = ConvertDTOToEntity(Promotion_PromotionDTO);
            Promotion = await PromotionService.Delete(Promotion);
            Promotion_PromotionDTO = new Promotion_PromotionDTO(Promotion);
            if (Promotion.IsValidated)
                return Promotion_PromotionDTO;
            else
                return BadRequest(Promotion_PromotionDTO);
        }

        [Route(PromotionRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionFilter PromotionFilter = new PromotionFilter();
            PromotionFilter = await PromotionService.ToFilter(PromotionFilter);
            PromotionFilter.Id = new IdFilter { In = Ids };
            PromotionFilter.Selects = PromotionSelect.Id;
            PromotionFilter.Skip = 0;
            PromotionFilter.Take = int.MaxValue;

            List<Promotion> Promotions = await PromotionService.List(PromotionFilter);
            Promotions = await PromotionService.BulkDelete(Promotions);
            if (Promotions.Any(x => !x.IsValidated))
                return BadRequest(Promotions.Where(x => !x.IsValidated));
            return true;
        }

        [Route(PromotionRoute.ImportStore), HttpPost]
        public async Task<ActionResult<List<Promotion_PromotionStoreMappingDTO>>> ImportStore([FromForm] long PromotionId, [FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(PromotionId))
                return Forbid();

            Promotion Promotion = await PromotionService.Get(PromotionId);
            if (Promotion == null)
                Promotion = new Promotion();
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest("Định dạng file không hợp lệ");

            StoreFilter StoreFilter = new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.ALL
            };

            List<Store> Stores = await StoreService.List(StoreFilter);
            StringBuilder errorContent = new StringBuilder();
            Promotion.PromotionStoreMappings = new List<PromotionStoreMapping>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Daily_Apdung"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 1;
                int SttColumnn = 0 + StartColumn;
                int StoreCodeColumn = 1 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string stt = worksheet.Cells[i + StartRow, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;

                    string StoreCodeValue = worksheet.Cells[i + StartRow, StoreCodeColumn].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(StoreCodeValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Chưa nhập mã đại lý");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(StoreCodeValue) && i == worksheet.Dimension.End.Row)
                        break;

                    var Store = Stores.Where(x => x.Code == StoreCodeValue).FirstOrDefault();
                    if (Store == null)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Mã đại lý không tồn tại");
                        continue;
                    }
                    PromotionStoreMapping PromotionStoreMapping = Promotion.PromotionStoreMappings.Where(x => x.StoreId == Store.Id).FirstOrDefault();
                    if (PromotionStoreMapping == null)
                    {
                        PromotionStoreMapping = new PromotionStoreMapping();
                        PromotionStoreMapping.PromotionId = Promotion.Id;
                        PromotionStoreMapping.StoreId = Store.Id;
                        PromotionStoreMapping.Store = Store;
                        Promotion.PromotionStoreMappings.Add(PromotionStoreMapping);
                    }
                }
                if (errorContent.Length > 0)
                    return BadRequest(errorContent.ToString());
            }

            List<Promotion_PromotionStoreMappingDTO> Promotion_PromotionStoreMappingDTOs = Promotion.PromotionStoreMappings
                 .Select(c => new Promotion_PromotionStoreMappingDTO(c)).ToList();
            return Promotion_PromotionStoreMappingDTOs;
        }

        [Route(PromotionRoute.ExportStore), HttpPost]
        public async Task<ActionResult> ExportStore([FromBody] Promotion_PromotionDTO Promotion_PromotionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long PromotionId = Promotion_PromotionDTO?.Id ?? 0;
            Promotion Promotion = await PromotionService.Get(PromotionId);
            if (Promotion == null)
                return null;

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                var PromotionStoreMappingHeader = new List<string>()
                {
                    "STT",
                    "Mã đại lý",
                    "Tên đại lý"
                };
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < Promotion.PromotionStoreMappings.Count; i++)
                {
                    var PromotionStoreMapping = Promotion.PromotionStoreMappings[i];
                    data.Add(new Object[]
                    {
                        i+1,
                        PromotionStoreMapping.Store?.Code,
                        PromotionStoreMapping.Store?.Name,
                    });
                }
                excel.GenerateWorksheet("Daily_Apdung", PromotionStoreMappingHeader, data);

                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", $"{Promotion.Code}_Store.xlsx");
        }

        [Route(PromotionRoute.ExportTemplateStore), HttpPost]
        public async Task<ActionResult> ExportTemplateStore()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var appUser = await AppUserService.Get(CurrentContext.UserId);
            var StoreFilter = new StoreFilter
            {
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name,
                Skip = 0,
                Take = int.MaxValue,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                OrganizationId = new IdFilter { Equal = appUser.OrganizationId }
            };
            var Stores = await StoreService.List(StoreFilter);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string tempPath = "Templates/Promotion_Store.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(tempPath);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            using (var xlPackage = new ExcelPackage(input))
            {
                var worksheet = xlPackage.Workbook.Worksheets["Daily"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow = 2;
                int numberCell = 1;
                for (var i = 0; i < Stores.Count; i++)
                {
                    Store Store = Stores[i];
                    worksheet.Cells[startRow + i, numberCell].Value = Store.Code;
                    worksheet.Cells[startRow + i, numberCell + 1].Value = Store.Name;
                }
                xlPackage.SaveAs(output);
            }
            return File(output.ToArray(), "application/octet-stream", "Template_Promotion_Store.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            PromotionFilter PromotionFilter = new PromotionFilter();
            PromotionFilter = await PromotionService.ToFilter(PromotionFilter);
            if (Id == 0)
            {

            }
            else
            {
                PromotionFilter.Id = new IdFilter { Equal = Id };
                int count = await PromotionService.Count(PromotionFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Promotion ConvertDTOToEntity(Promotion_PromotionDTO Promotion_PromotionDTO)
        {
            Promotion_PromotionDTO.TrimString();
            Promotion Promotion = new Promotion();
            Promotion.Id = Promotion_PromotionDTO.Id;
            Promotion.Code = Promotion_PromotionDTO.Code;
            Promotion.Name = Promotion_PromotionDTO.Name;
            Promotion.StartDate = Promotion_PromotionDTO.StartDate;
            Promotion.EndDate = Promotion_PromotionDTO.EndDate;
            Promotion.OrganizationId = Promotion_PromotionDTO.OrganizationId;
            Promotion.PromotionTypeId = Promotion_PromotionDTO.PromotionTypeId;
            Promotion.Note = Promotion_PromotionDTO.Note;
            Promotion.Priority = Promotion_PromotionDTO.Priority;
            Promotion.StatusId = Promotion_PromotionDTO.StatusId;
            Promotion.Organization = Promotion_PromotionDTO.Organization == null ? null : new Organization
            {
                Id = Promotion_PromotionDTO.Organization.Id,
                Code = Promotion_PromotionDTO.Organization.Code,
                Name = Promotion_PromotionDTO.Organization.Name,
                ParentId = Promotion_PromotionDTO.Organization.ParentId,
                Path = Promotion_PromotionDTO.Organization.Path,
                Level = Promotion_PromotionDTO.Organization.Level,
                StatusId = Promotion_PromotionDTO.Organization.StatusId,
                Phone = Promotion_PromotionDTO.Organization.Phone,
                Email = Promotion_PromotionDTO.Organization.Email,
                Address = Promotion_PromotionDTO.Organization.Address,
            };
            Promotion.PromotionType = Promotion_PromotionDTO.PromotionType == null ? null : new PromotionType
            {
                Id = Promotion_PromotionDTO.PromotionType.Id,
                Code = Promotion_PromotionDTO.PromotionType.Code,
                Name = Promotion_PromotionDTO.PromotionType.Name,
            };
            Promotion.Status = Promotion_PromotionDTO.Status == null ? null : new Status
            {
                Id = Promotion_PromotionDTO.Status.Id,
                Code = Promotion_PromotionDTO.Status.Code,
                Name = Promotion_PromotionDTO.Status.Name,
            };
            Promotion.PromotionPromotionPolicyMappings = Promotion_PromotionDTO.PromotionPromotionPolicyMappings?
                .Select(x => new PromotionPromotionPolicyMapping
                {
                    PromotionPolicyId = x.PromotionPolicyId,
                    Note = x.Note,
                    StatusId = x.StatusId,
                    PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).ToList();
            Promotion.PromotionStoreGroupingMappings = Promotion_PromotionDTO.PromotionStoreGroupingMappings?
                .Select(x => new PromotionStoreGroupingMapping
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
            Promotion.PromotionStoreMappings = Promotion_PromotionDTO.PromotionStoreMappings?
                .Select(x => new PromotionStoreMapping
                {
                    StoreId = x.StoreId,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
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
                        StatusId = x.Store.StatusId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                    },
                }).ToList();
            Promotion.PromotionStoreTypeMappings = Promotion_PromotionDTO.PromotionStoreTypeMappings?
                .Select(x => new PromotionStoreTypeMapping
                {
                    StoreTypeId = x.StoreTypeId,
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        Used = x.StoreType.Used,
                    },
                }).ToList();
            Promotion.BaseLanguage = CurrentContext.Language;
            return Promotion;
        }

        private PromotionFilter ConvertFilterDTOToFilterEntity(Promotion_PromotionFilterDTO Promotion_PromotionFilterDTO)
        {
            PromotionFilter PromotionFilter = new PromotionFilter();
            PromotionFilter.Selects = PromotionSelect.ALL;
            PromotionFilter.Skip = Promotion_PromotionFilterDTO.Skip;
            PromotionFilter.Take = Promotion_PromotionFilterDTO.Take;
            PromotionFilter.OrderBy = Promotion_PromotionFilterDTO.OrderBy;
            PromotionFilter.OrderType = Promotion_PromotionFilterDTO.OrderType;

            PromotionFilter.Id = Promotion_PromotionFilterDTO.Id;
            PromotionFilter.Code = Promotion_PromotionFilterDTO.Code;
            PromotionFilter.Name = Promotion_PromotionFilterDTO.Name;
            PromotionFilter.StartDate = Promotion_PromotionFilterDTO.StartDate;
            PromotionFilter.EndDate = Promotion_PromotionFilterDTO.EndDate;
            PromotionFilter.OrganizationId = Promotion_PromotionFilterDTO.OrganizationId;
            PromotionFilter.PromotionTypeId = Promotion_PromotionFilterDTO.PromotionTypeId;
            PromotionFilter.Note = Promotion_PromotionFilterDTO.Note;
            PromotionFilter.Priority = Promotion_PromotionFilterDTO.Priority;
            PromotionFilter.StatusId = Promotion_PromotionFilterDTO.StatusId;
            PromotionFilter.CreatedAt = Promotion_PromotionFilterDTO.CreatedAt;
            PromotionFilter.UpdatedAt = Promotion_PromotionFilterDTO.UpdatedAt;
            return PromotionFilter;
        }
    }
}

