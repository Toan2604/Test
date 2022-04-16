using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public partial class PromotionCodeController : RpcController
    {
        [Route(PromotionCodeRoute.FilterListOrganization), HttpPost]
        public async Task<List<PromotionCode_OrganizationDTO>> FilterListOrganization([FromBody] PromotionCode_OrganizationFilterDTO PromotionCode_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = PromotionCode_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = PromotionCode_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = PromotionCode_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = PromotionCode_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = PromotionCode_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = PromotionCode_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = PromotionCode_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = PromotionCode_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = PromotionCode_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<PromotionCode_OrganizationDTO> PromotionCode_OrganizationDTOs = Organizations
                .Select(x => new PromotionCode_OrganizationDTO(x)).ToList();
            return PromotionCode_OrganizationDTOs;
        }
        [Route(PromotionCodeRoute.FilterListPromotionDiscountType), HttpPost]
        public async Task<List<PromotionCode_PromotionDiscountTypeDTO>> FilterListPromotionDiscountType([FromBody] PromotionCode_PromotionDiscountTypeFilterDTO PromotionCode_PromotionDiscountTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionDiscountTypeFilter PromotionDiscountTypeFilter = new PromotionDiscountTypeFilter();
            PromotionDiscountTypeFilter.Skip = 0;
            PromotionDiscountTypeFilter.Take = int.MaxValue;
            PromotionDiscountTypeFilter.Take = 20;
            PromotionDiscountTypeFilter.OrderBy = PromotionDiscountTypeOrder.Id;
            PromotionDiscountTypeFilter.OrderType = OrderType.ASC;
            PromotionDiscountTypeFilter.Selects = PromotionDiscountTypeSelect.ALL;

            List<PromotionDiscountType> PromotionDiscountTypes = await PromotionDiscountTypeService.List(PromotionDiscountTypeFilter);
            List<PromotionCode_PromotionDiscountTypeDTO> PromotionCode_PromotionDiscountTypeDTOs = PromotionDiscountTypes
                .Select(x => new PromotionCode_PromotionDiscountTypeDTO(x)).ToList();
            return PromotionCode_PromotionDiscountTypeDTOs;
        }
        [Route(PromotionCodeRoute.FilterListPromotionProductAppliedType), HttpPost]
        public async Task<List<PromotionCode_PromotionProductAppliedTypeDTO>> FilterListPromotionProductAppliedType([FromBody] PromotionCode_PromotionProductAppliedTypeFilterDTO PromotionCode_PromotionProductAppliedTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter = new PromotionProductAppliedTypeFilter();
            PromotionProductAppliedTypeFilter.Skip = 0;
            PromotionProductAppliedTypeFilter.Take = int.MaxValue;
            PromotionProductAppliedTypeFilter.Take = 20;
            PromotionProductAppliedTypeFilter.OrderBy = PromotionProductAppliedTypeOrder.Id;
            PromotionProductAppliedTypeFilter.OrderType = OrderType.ASC;
            PromotionProductAppliedTypeFilter.Selects = PromotionProductAppliedTypeSelect.ALL;

            List<PromotionProductAppliedType> PromotionProductAppliedTypes = await PromotionProductAppliedTypeService.List(PromotionProductAppliedTypeFilter);
            List<PromotionCode_PromotionProductAppliedTypeDTO> PromotionCode_PromotionProductAppliedTypeDTOs = PromotionProductAppliedTypes
                .Select(x => new PromotionCode_PromotionProductAppliedTypeDTO(x)).ToList();
            return PromotionCode_PromotionProductAppliedTypeDTOs;
        }
        [Route(PromotionCodeRoute.FilterListPromotionType), HttpPost]
        public async Task<List<PromotionCode_PromotionTypeDTO>> FilterListPromotionType([FromBody] PromotionCode_PromotionTypeFilterDTO PromotionCode_PromotionTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionTypeFilter PromotionTypeFilter = new PromotionTypeFilter();
            PromotionTypeFilter.Skip = 0;
            PromotionTypeFilter.Take = int.MaxValue;
            PromotionTypeFilter.Take = 20;
            PromotionTypeFilter.OrderBy = PromotionTypeOrder.Id;
            PromotionTypeFilter.OrderType = OrderType.ASC;
            PromotionTypeFilter.Selects = PromotionTypeSelect.ALL;

            List<PromotionType> PromotionTypes = await PromotionTypeService.List(PromotionTypeFilter);
            List<PromotionCode_PromotionTypeDTO> PromotionCode_PromotionTypeDTOs = PromotionTypes
                .Select(x => new PromotionCode_PromotionTypeDTO(x)).ToList();
            return PromotionCode_PromotionTypeDTOs;
        }
        [Route(PromotionCodeRoute.FilterListStatus), HttpPost]
        public async Task<List<PromotionCode_StatusDTO>> FilterListStatus([FromBody] PromotionCode_StatusFilterDTO PromotionCode_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<PromotionCode_StatusDTO> PromotionCode_StatusDTOs = Statuses
                .Select(x => new PromotionCode_StatusDTO(x)).ToList();
            return PromotionCode_StatusDTOs;
        }
        [Route(PromotionCodeRoute.FilterListProduct), HttpPost]
        public async Task<List<PromotionCode_ProductDTO>> FilterListProduct([FromBody] PromotionCode_ProductFilterDTO PromotionCode_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Skip = 0;
            ProductFilter.Take = 20;
            ProductFilter.OrderBy = ProductOrder.Id;
            ProductFilter.OrderType = OrderType.ASC;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = PromotionCode_ProductFilterDTO.Id;
            ProductFilter.Code = PromotionCode_ProductFilterDTO.Code;
            ProductFilter.SupplierCode = PromotionCode_ProductFilterDTO.SupplierCode;
            ProductFilter.Name = PromotionCode_ProductFilterDTO.Name;
            ProductFilter.Description = PromotionCode_ProductFilterDTO.Description;
            ProductFilter.ScanCode = PromotionCode_ProductFilterDTO.ScanCode;
            ProductFilter.ERPCode = PromotionCode_ProductFilterDTO.ERPCode;
            ProductFilter.ProductTypeId = PromotionCode_ProductFilterDTO.ProductTypeId;
            ProductFilter.SupplierId = PromotionCode_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = PromotionCode_ProductFilterDTO.BrandId;
            ProductFilter.UnitOfMeasureId = PromotionCode_ProductFilterDTO.UnitOfMeasureId;
            ProductFilter.UnitOfMeasureGroupingId = PromotionCode_ProductFilterDTO.UnitOfMeasureGroupingId;
            ProductFilter.SalePrice = PromotionCode_ProductFilterDTO.SalePrice;
            ProductFilter.RetailPrice = PromotionCode_ProductFilterDTO.RetailPrice;
            ProductFilter.TaxTypeId = PromotionCode_ProductFilterDTO.TaxTypeId;
            ProductFilter.StatusId = PromotionCode_ProductFilterDTO.StatusId;
            ProductFilter.OtherName = PromotionCode_ProductFilterDTO.OtherName;
            ProductFilter.TechnicalName = PromotionCode_ProductFilterDTO.TechnicalName;
            ProductFilter.Note = PromotionCode_ProductFilterDTO.Note;
            ProductFilter.UsedVariationId = PromotionCode_ProductFilterDTO.UsedVariationId;
            ProductFilter.Search = PromotionCode_ProductFilterDTO.Search;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Product> Products = await ProductService.List(ProductFilter);
            List<PromotionCode_ProductDTO> PromotionCode_ProductDTOs = Products
                .Select(x => new PromotionCode_ProductDTO(x)).ToList();
            return PromotionCode_ProductDTOs;
        }
        [Route(PromotionCodeRoute.FilterListStore), HttpPost]
        public async Task<List<PromotionCode_StoreDTO>> FilterListStore([FromBody] PromotionCode_StoreFilterDTO PromotionCode_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = PromotionCode_StoreFilterDTO.Id;
            StoreFilter.Code = PromotionCode_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PromotionCode_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PromotionCode_StoreFilterDTO.Name;
            StoreFilter.UnsignName = PromotionCode_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = PromotionCode_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PromotionCode_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PromotionCode_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PromotionCode_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PromotionCode_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PromotionCode_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PromotionCode_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PromotionCode_StoreFilterDTO.WardId;
            StoreFilter.Address = PromotionCode_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = PromotionCode_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = PromotionCode_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PromotionCode_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PromotionCode_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PromotionCode_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PromotionCode_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PromotionCode_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PromotionCode_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PromotionCode_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = PromotionCode_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = PromotionCode_StoreFilterDTO.StoreStatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PromotionCode_StoreDTO> PromotionCode_StoreDTOs = Stores
                .Select(x => new PromotionCode_StoreDTO(x)).ToList();
            return PromotionCode_StoreDTOs;
        }

        [Route(PromotionCodeRoute.SingleListOrganization), HttpPost]
        public async Task<List<PromotionCode_OrganizationDTO>> SingleListOrganization([FromBody] PromotionCode_OrganizationFilterDTO PromotionCode_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = PromotionCode_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = PromotionCode_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = PromotionCode_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = PromotionCode_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = PromotionCode_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = PromotionCode_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = PromotionCode_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = PromotionCode_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = PromotionCode_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = PromotionCode_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<PromotionCode_OrganizationDTO> PromotionCode_OrganizationDTOs = Organizations
                .Select(x => new PromotionCode_OrganizationDTO(x)).ToList();
            return PromotionCode_OrganizationDTOs;
        }
        [Route(PromotionCodeRoute.SingleListPromotionDiscountType), HttpPost]
        public async Task<List<PromotionCode_PromotionDiscountTypeDTO>> SingleListPromotionDiscountType([FromBody] PromotionCode_PromotionDiscountTypeFilterDTO PromotionCode_PromotionDiscountTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionDiscountTypeFilter PromotionDiscountTypeFilter = new PromotionDiscountTypeFilter();
            PromotionDiscountTypeFilter.Skip = 0;
            PromotionDiscountTypeFilter.Take = int.MaxValue;
            PromotionDiscountTypeFilter.Take = 20;
            PromotionDiscountTypeFilter.OrderBy = PromotionDiscountTypeOrder.Id;
            PromotionDiscountTypeFilter.OrderType = OrderType.ASC;
            PromotionDiscountTypeFilter.Selects = PromotionDiscountTypeSelect.ALL;

            List<PromotionDiscountType> PromotionDiscountTypes = await PromotionDiscountTypeService.List(PromotionDiscountTypeFilter);
            List<PromotionCode_PromotionDiscountTypeDTO> PromotionCode_PromotionDiscountTypeDTOs = PromotionDiscountTypes
                .Select(x => new PromotionCode_PromotionDiscountTypeDTO(x)).ToList();
            return PromotionCode_PromotionDiscountTypeDTOs;
        }
        [Route(PromotionCodeRoute.SingleListPromotionProductAppliedType), HttpPost]
        public async Task<List<PromotionCode_PromotionProductAppliedTypeDTO>> SingleListPromotionProductAppliedType([FromBody] PromotionCode_PromotionProductAppliedTypeFilterDTO PromotionCode_PromotionProductAppliedTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter = new PromotionProductAppliedTypeFilter();
            PromotionProductAppliedTypeFilter.Skip = 0;
            PromotionProductAppliedTypeFilter.Take = int.MaxValue;
            PromotionProductAppliedTypeFilter.Take = 20;
            PromotionProductAppliedTypeFilter.OrderBy = PromotionProductAppliedTypeOrder.Id;
            PromotionProductAppliedTypeFilter.OrderType = OrderType.ASC;
            PromotionProductAppliedTypeFilter.Selects = PromotionProductAppliedTypeSelect.ALL;

            List<PromotionProductAppliedType> PromotionProductAppliedTypes = await PromotionProductAppliedTypeService.List(PromotionProductAppliedTypeFilter);
            List<PromotionCode_PromotionProductAppliedTypeDTO> PromotionCode_PromotionProductAppliedTypeDTOs = PromotionProductAppliedTypes
                .Select(x => new PromotionCode_PromotionProductAppliedTypeDTO(x)).ToList();
            return PromotionCode_PromotionProductAppliedTypeDTOs;
        }
        [Route(PromotionCodeRoute.SingleListPromotionType), HttpPost]
        public async Task<List<PromotionCode_PromotionTypeDTO>> SingleListPromotionType([FromBody] PromotionCode_PromotionTypeFilterDTO PromotionCode_PromotionTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionTypeFilter PromotionTypeFilter = new PromotionTypeFilter();
            PromotionTypeFilter.Skip = 0;
            PromotionTypeFilter.Take = int.MaxValue;
            PromotionTypeFilter.Take = 20;
            PromotionTypeFilter.OrderBy = PromotionTypeOrder.Id;
            PromotionTypeFilter.OrderType = OrderType.ASC;
            PromotionTypeFilter.Selects = PromotionTypeSelect.ALL;

            List<PromotionType> PromotionTypes = await PromotionTypeService.List(PromotionTypeFilter);
            List<PromotionCode_PromotionTypeDTO> PromotionCode_PromotionTypeDTOs = PromotionTypes
                .Select(x => new PromotionCode_PromotionTypeDTO(x)).ToList();
            return PromotionCode_PromotionTypeDTOs;
        }
        [Route(PromotionCodeRoute.SingleListStatus), HttpPost]
        public async Task<List<PromotionCode_StatusDTO>> SingleListStatus([FromBody] PromotionCode_StatusFilterDTO PromotionCode_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<PromotionCode_StatusDTO> PromotionCode_StatusDTOs = Statuses
                .Select(x => new PromotionCode_StatusDTO(x)).ToList();
            return PromotionCode_StatusDTOs;
        }
        [Route(PromotionCodeRoute.SingleListSupplier), HttpPost]
        public async Task<List<PromotionCode_SupplierDTO>> SingleListSupplier([FromBody] PromotionCode_SupplierFilterDTO PromotionCode_SupplierFilterDTO)
        {
            SupplierFilter SupplierFilter = new SupplierFilter();
            SupplierFilter.Skip = 0;
            SupplierFilter.Take = 20;
            SupplierFilter.OrderBy = SupplierOrder.Id;
            SupplierFilter.OrderType = OrderType.ASC;
            SupplierFilter.Selects = SupplierSelect.ALL;
            SupplierFilter.Id = PromotionCode_SupplierFilterDTO.Id;
            SupplierFilter.Code = PromotionCode_SupplierFilterDTO.Code;
            SupplierFilter.Name = PromotionCode_SupplierFilterDTO.Name;
            SupplierFilter.TaxCode = PromotionCode_SupplierFilterDTO.TaxCode;
            SupplierFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Supplier> Suppliers = await SupplierService.List(SupplierFilter);
            List<PromotionCode_SupplierDTO> PromotionCode_SupplierDTOs = Suppliers
                .Select(x => new PromotionCode_SupplierDTO(x)).ToList();
            return PromotionCode_SupplierDTOs;
        }
        [Route(PromotionCodeRoute.SingleListProduct), HttpPost]
        public async Task<List<PromotionCode_ProductDTO>> SingleListProduct([FromBody] PromotionCode_ProductFilterDTO PromotionCode_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Skip = 0;
            ProductFilter.Take = 20;
            ProductFilter.OrderBy = ProductOrder.Id;
            ProductFilter.OrderType = OrderType.ASC;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = PromotionCode_ProductFilterDTO.Id;
            ProductFilter.Code = PromotionCode_ProductFilterDTO.Code;
            ProductFilter.SupplierCode = PromotionCode_ProductFilterDTO.SupplierCode;
            ProductFilter.Name = PromotionCode_ProductFilterDTO.Name;
            ProductFilter.Description = PromotionCode_ProductFilterDTO.Description;
            ProductFilter.ScanCode = PromotionCode_ProductFilterDTO.ScanCode;
            ProductFilter.ERPCode = PromotionCode_ProductFilterDTO.ERPCode;
            ProductFilter.ProductTypeId = PromotionCode_ProductFilterDTO.ProductTypeId;
            ProductFilter.SupplierId = PromotionCode_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = PromotionCode_ProductFilterDTO.BrandId;
            ProductFilter.UnitOfMeasureId = PromotionCode_ProductFilterDTO.UnitOfMeasureId;
            ProductFilter.UnitOfMeasureGroupingId = PromotionCode_ProductFilterDTO.UnitOfMeasureGroupingId;
            ProductFilter.SalePrice = PromotionCode_ProductFilterDTO.SalePrice;
            ProductFilter.RetailPrice = PromotionCode_ProductFilterDTO.RetailPrice;
            ProductFilter.TaxTypeId = PromotionCode_ProductFilterDTO.TaxTypeId;
            ProductFilter.StatusId = PromotionCode_ProductFilterDTO.StatusId;
            ProductFilter.OtherName = PromotionCode_ProductFilterDTO.OtherName;
            ProductFilter.TechnicalName = PromotionCode_ProductFilterDTO.TechnicalName;
            ProductFilter.Note = PromotionCode_ProductFilterDTO.Note;
            ProductFilter.UsedVariationId = PromotionCode_ProductFilterDTO.UsedVariationId;
            ProductFilter.Search = PromotionCode_ProductFilterDTO.Search;

            List<Product> Products = await ProductService.List(ProductFilter);
            List<PromotionCode_ProductDTO> PromotionCode_ProductDTOs = Products
                .Select(x => new PromotionCode_ProductDTO(x)).ToList();
            return PromotionCode_ProductDTOs;
        }
        [Route(PromotionCodeRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<PromotionCode_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] PromotionCode_StoreGroupingFilterDTO PromotionCode_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = PromotionCode_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = PromotionCode_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = PromotionCode_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = PromotionCode_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = PromotionCode_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = PromotionCode_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = PromotionCode_StoreGroupingFilterDTO.StatusId;

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<PromotionCode_StoreGroupingDTO> PromotionCode_StoreGroupingDTOs = StoreGroupings
                .Select(x => new PromotionCode_StoreGroupingDTO(x)).ToList();
            return PromotionCode_StoreGroupingDTOs;
        }
        [Route(PromotionCodeRoute.SingleListStore), HttpPost]
        public async Task<List<PromotionCode_StoreDTO>> SingleListStore([FromBody] PromotionCode_StoreFilterDTO PromotionCode_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = PromotionCode_StoreFilterDTO.Id;
            StoreFilter.Code = PromotionCode_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PromotionCode_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PromotionCode_StoreFilterDTO.Name;
            StoreFilter.UnsignName = PromotionCode_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = PromotionCode_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PromotionCode_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PromotionCode_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PromotionCode_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PromotionCode_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PromotionCode_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PromotionCode_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PromotionCode_StoreFilterDTO.WardId;
            StoreFilter.Address = PromotionCode_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = PromotionCode_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = PromotionCode_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PromotionCode_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PromotionCode_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PromotionCode_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PromotionCode_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PromotionCode_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PromotionCode_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PromotionCode_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = PromotionCode_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = PromotionCode_StoreFilterDTO.StatusId;
            StoreFilter.StoreStatusId = PromotionCode_StoreFilterDTO.StoreStatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PromotionCode_StoreDTO> PromotionCode_StoreDTOs = Stores
                .Select(x => new PromotionCode_StoreDTO(x)).ToList();
            return PromotionCode_StoreDTOs;
        }
        [Route(PromotionCodeRoute.SingleListStoreType), HttpPost]
        public async Task<List<PromotionCode_StoreTypeDTO>> SingleListStoreType([FromBody] PromotionCode_StoreTypeFilterDTO PromotionCode_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = PromotionCode_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = PromotionCode_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = PromotionCode_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = PromotionCode_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = PromotionCode_StoreTypeFilterDTO.StatusId;

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<PromotionCode_StoreTypeDTO> PromotionCode_StoreTypeDTOs = StoreTypes
                .Select(x => new PromotionCode_StoreTypeDTO(x)).ToList();
            return PromotionCode_StoreTypeDTOs;
        }
        [Route(PromotionCodeRoute.CountOrganization), HttpPost]
        public async Task<long> CountOrganization([FromBody] PromotionCode_OrganizationFilterDTO PromotionCode_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Id = PromotionCode_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = PromotionCode_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = PromotionCode_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = PromotionCode_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = PromotionCode_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = PromotionCode_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = PromotionCode_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = PromotionCode_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = PromotionCode_OrganizationFilterDTO.Address;

            return await OrganizationService.Count(OrganizationFilter);
        }

        [Route(PromotionCodeRoute.ListOrganization), HttpPost]
        public async Task<List<PromotionCode_OrganizationDTO>> ListOrganization([FromBody] PromotionCode_OrganizationFilterDTO PromotionCode_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = PromotionCode_OrganizationFilterDTO.Skip;
            OrganizationFilter.Take = PromotionCode_OrganizationFilterDTO.Take;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = PromotionCode_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = PromotionCode_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = PromotionCode_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = PromotionCode_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = PromotionCode_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = PromotionCode_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = PromotionCode_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = PromotionCode_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = PromotionCode_OrganizationFilterDTO.Address;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<PromotionCode_OrganizationDTO> PromotionCode_OrganizationDTOs = Organizations
                .Select(x => new PromotionCode_OrganizationDTO(x)).ToList();
            return PromotionCode_OrganizationDTOs;
        }
        [Route(PromotionCodeRoute.CountProduct), HttpPost]
        public async Task<long> CountProduct([FromBody] PromotionCode_ProductFilterDTO PromotionCode_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Id = PromotionCode_ProductFilterDTO.Id;
            ProductFilter.Code = PromotionCode_ProductFilterDTO.Code;
            ProductFilter.SupplierCode = PromotionCode_ProductFilterDTO.SupplierCode;
            ProductFilter.Name = PromotionCode_ProductFilterDTO.Name;
            ProductFilter.Description = PromotionCode_ProductFilterDTO.Description;
            ProductFilter.ScanCode = PromotionCode_ProductFilterDTO.ScanCode;
            ProductFilter.ERPCode = PromotionCode_ProductFilterDTO.ERPCode;
            ProductFilter.ProductTypeId = PromotionCode_ProductFilterDTO.ProductTypeId;
            ProductFilter.SupplierId = PromotionCode_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = PromotionCode_ProductFilterDTO.BrandId;
            ProductFilter.UnitOfMeasureId = PromotionCode_ProductFilterDTO.UnitOfMeasureId;
            ProductFilter.UnitOfMeasureGroupingId = PromotionCode_ProductFilterDTO.UnitOfMeasureGroupingId;
            ProductFilter.SalePrice = PromotionCode_ProductFilterDTO.SalePrice;
            ProductFilter.RetailPrice = PromotionCode_ProductFilterDTO.RetailPrice;
            ProductFilter.TaxTypeId = PromotionCode_ProductFilterDTO.TaxTypeId;
            ProductFilter.StatusId = PromotionCode_ProductFilterDTO.StatusId;
            ProductFilter.OtherName = PromotionCode_ProductFilterDTO.OtherName;
            ProductFilter.TechnicalName = PromotionCode_ProductFilterDTO.TechnicalName;
            ProductFilter.Note = PromotionCode_ProductFilterDTO.Note;
            ProductFilter.UsedVariationId = PromotionCode_ProductFilterDTO.UsedVariationId;
            ProductFilter.Search = PromotionCode_ProductFilterDTO.Search;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await ProductService.Count(ProductFilter);
        }

        [Route(PromotionCodeRoute.ListProduct), HttpPost]
        public async Task<List<PromotionCode_ProductDTO>> ListProduct([FromBody] PromotionCode_ProductFilterDTO PromotionCode_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Skip = PromotionCode_ProductFilterDTO.Skip;
            ProductFilter.Take = PromotionCode_ProductFilterDTO.Take;
            ProductFilter.OrderBy = ProductOrder.Id;
            ProductFilter.OrderType = OrderType.ASC;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = PromotionCode_ProductFilterDTO.Id;
            ProductFilter.Code = PromotionCode_ProductFilterDTO.Code;
            ProductFilter.SupplierCode = PromotionCode_ProductFilterDTO.SupplierCode;
            ProductFilter.Name = PromotionCode_ProductFilterDTO.Name;
            ProductFilter.Description = PromotionCode_ProductFilterDTO.Description;
            ProductFilter.ScanCode = PromotionCode_ProductFilterDTO.ScanCode;
            ProductFilter.ERPCode = PromotionCode_ProductFilterDTO.ERPCode;
            ProductFilter.ProductTypeId = PromotionCode_ProductFilterDTO.ProductTypeId;
            ProductFilter.SupplierId = PromotionCode_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = PromotionCode_ProductFilterDTO.BrandId;
            ProductFilter.UnitOfMeasureId = PromotionCode_ProductFilterDTO.UnitOfMeasureId;
            ProductFilter.UnitOfMeasureGroupingId = PromotionCode_ProductFilterDTO.UnitOfMeasureGroupingId;
            ProductFilter.SalePrice = PromotionCode_ProductFilterDTO.SalePrice;
            ProductFilter.RetailPrice = PromotionCode_ProductFilterDTO.RetailPrice;
            ProductFilter.TaxTypeId = PromotionCode_ProductFilterDTO.TaxTypeId;
            ProductFilter.StatusId = PromotionCode_ProductFilterDTO.StatusId;
            ProductFilter.OtherName = PromotionCode_ProductFilterDTO.OtherName;
            ProductFilter.TechnicalName = PromotionCode_ProductFilterDTO.TechnicalName;
            ProductFilter.Note = PromotionCode_ProductFilterDTO.Note;
            ProductFilter.UsedVariationId = PromotionCode_ProductFilterDTO.UsedVariationId;
            ProductFilter.Search = PromotionCode_ProductFilterDTO.Search;
            ProductFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Product> Products = await ProductService.List(ProductFilter);
            List<PromotionCode_ProductDTO> PromotionCode_ProductDTOs = Products
                .Select(x => new PromotionCode_ProductDTO(x)).ToList();
            return PromotionCode_ProductDTOs;
        }
        [Route(PromotionCodeRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] PromotionCode_StoreFilterDTO PromotionCode_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = PromotionCode_StoreFilterDTO.Id;
            StoreFilter.Code = PromotionCode_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PromotionCode_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PromotionCode_StoreFilterDTO.Name;
            StoreFilter.UnsignName = PromotionCode_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = PromotionCode_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PromotionCode_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PromotionCode_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PromotionCode_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PromotionCode_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PromotionCode_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PromotionCode_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PromotionCode_StoreFilterDTO.WardId;
            StoreFilter.Address = PromotionCode_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = PromotionCode_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = PromotionCode_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PromotionCode_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PromotionCode_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PromotionCode_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PromotionCode_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PromotionCode_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PromotionCode_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PromotionCode_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = PromotionCode_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = PromotionCode_StoreFilterDTO.StoreStatusId;

            return await StoreService.Count(StoreFilter);
        }

        [Route(PromotionCodeRoute.ListStore), HttpPost]
        public async Task<List<PromotionCode_StoreDTO>> ListStore([FromBody] PromotionCode_StoreFilterDTO PromotionCode_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = PromotionCode_StoreFilterDTO.Skip;
            StoreFilter.Take = PromotionCode_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = PromotionCode_StoreFilterDTO.Id;
            StoreFilter.Code = PromotionCode_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PromotionCode_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PromotionCode_StoreFilterDTO.Name;
            StoreFilter.UnsignName = PromotionCode_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = PromotionCode_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PromotionCode_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PromotionCode_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PromotionCode_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PromotionCode_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PromotionCode_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PromotionCode_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PromotionCode_StoreFilterDTO.WardId;
            StoreFilter.Address = PromotionCode_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = PromotionCode_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = PromotionCode_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PromotionCode_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PromotionCode_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PromotionCode_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PromotionCode_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PromotionCode_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PromotionCode_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PromotionCode_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = PromotionCode_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = PromotionCode_StoreFilterDTO.StoreStatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PromotionCode_StoreDTO> PromotionCode_StoreDTOs = Stores
                .Select(x => new PromotionCode_StoreDTO(x)).ToList();
            return PromotionCode_StoreDTOs;
        }
    }
}

