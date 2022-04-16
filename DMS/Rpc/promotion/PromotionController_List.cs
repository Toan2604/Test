using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public partial class PromotionController : RpcController
    {
        [Route(PromotionRoute.FilterListOrganization), HttpPost]
        public async Task<List<Promotion_OrganizationDTO>> FilterListOrganization([FromBody] Promotion_OrganizationFilterDTO Promotion_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Promotion_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Promotion_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Promotion_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Promotion_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Promotion_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Promotion_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = Promotion_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = Promotion_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = Promotion_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = Promotion_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Promotion_OrganizationDTO> Promotion_OrganizationDTOs = Organizations
                .Select(x => new Promotion_OrganizationDTO(x)).ToList();
            return Promotion_OrganizationDTOs;
        }
        [Route(PromotionRoute.FilterListPromotionType), HttpPost]
        public async Task<List<Promotion_PromotionTypeDTO>> FilterListPromotionType([FromBody] Promotion_PromotionTypeFilterDTO Promotion_PromotionTypeFilterDTO)
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
            List<Promotion_PromotionTypeDTO> Promotion_PromotionTypeDTOs = PromotionTypes
                .Select(x => new Promotion_PromotionTypeDTO(x)).ToList();
            return Promotion_PromotionTypeDTOs;
        }
        [Route(PromotionRoute.FilterListStatus), HttpPost]
        public async Task<List<Promotion_StatusDTO>> FilterListStatus([FromBody] Promotion_StatusFilterDTO Promotion_StatusFilterDTO)
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
            List<Promotion_StatusDTO> Promotion_StatusDTOs = Statuses
                .Select(x => new Promotion_StatusDTO(x)).ToList();
            return Promotion_StatusDTOs;
        }
        [Route(PromotionRoute.FilterListPromotionPolicy), HttpPost]
        public async Task<List<Promotion_PromotionPolicyDTO>> FilterListPromotionPolicy([FromBody] Promotion_PromotionPolicyFilterDTO Promotion_PromotionPolicyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionPolicyFilter PromotionPolicyFilter = new PromotionPolicyFilter();
            PromotionPolicyFilter.Skip = 0;
            PromotionPolicyFilter.Take = int.MaxValue;
            PromotionPolicyFilter.Take = 20;
            PromotionPolicyFilter.OrderBy = PromotionPolicyOrder.Id;
            PromotionPolicyFilter.OrderType = OrderType.ASC;
            PromotionPolicyFilter.Selects = PromotionPolicySelect.ALL;

            List<PromotionPolicy> PromotionPolicies = await PromotionPolicyService.List(PromotionPolicyFilter);
            List<Promotion_PromotionPolicyDTO> Promotion_PromotionPolicyDTOs = PromotionPolicies
                .Select(x => new Promotion_PromotionPolicyDTO(x)).ToList();
            return Promotion_PromotionPolicyDTOs;
        }
        [Route(PromotionRoute.FilterListPromotionDiscountType), HttpPost]
        public async Task<List<Promotion_PromotionDiscountTypeDTO>> FilterListPromotionDiscountType([FromBody] Promotion_PromotionDiscountTypeFilterDTO Promotion_PromotionDiscountTypeFilterDTO)
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
            List<Promotion_PromotionDiscountTypeDTO> Promotion_PromotionDiscountTypeDTOs = PromotionDiscountTypes
                .Select(x => new Promotion_PromotionDiscountTypeDTO(x)).ToList();
            return Promotion_PromotionDiscountTypeDTOs;
        }
        [Route(PromotionRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<Promotion_ProductGroupingDTO>> FilterListProductGrouping([FromBody] Promotion_ProductGroupingFilterDTO Promotion_ProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.ALL;
            ProductGroupingFilter.Id = Promotion_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = Promotion_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = Promotion_ProductGroupingFilterDTO.Name;
            ProductGroupingFilter.Description = Promotion_ProductGroupingFilterDTO.Description;
            ProductGroupingFilter.ParentId = Promotion_ProductGroupingFilterDTO.ParentId;
            ProductGroupingFilter.Path = Promotion_ProductGroupingFilterDTO.Path;
            ProductGroupingFilter.Level = Promotion_ProductGroupingFilterDTO.Level;

            if (ProductGroupingFilter.Id == null) ProductGroupingFilter.Id = new IdFilter();
            ProductGroupingFilter.Id.In = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<Promotion_ProductGroupingDTO> Promotion_ProductGroupingDTOs = ProductGroupings
                .Select(x => new Promotion_ProductGroupingDTO(x)).ToList();
            return Promotion_ProductGroupingDTOs;
        }
        [Route(PromotionRoute.FilterListProductType), HttpPost]
        public async Task<List<Promotion_ProductTypeDTO>> FilterListProductType([FromBody] Promotion_ProductTypeFilterDTO Promotion_ProductTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = Promotion_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = Promotion_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = Promotion_ProductTypeFilterDTO.Name;
            ProductTypeFilter.Description = Promotion_ProductTypeFilterDTO.Description;
            ProductTypeFilter.StatusId = Promotion_ProductTypeFilterDTO.StatusId;

            if (ProductTypeFilter.Id == null) ProductTypeFilter.Id = new IdFilter();
            ProductTypeFilter.Id.In = await FilterProductType(ProductTypeService, CurrentContext);
            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<Promotion_ProductTypeDTO> Promotion_ProductTypeDTOs = ProductTypes
                .Select(x => new Promotion_ProductTypeDTO(x)).ToList();
            return Promotion_ProductTypeDTOs;
        }
        [Route(PromotionRoute.FilterListProduct), HttpPost]
        public async Task<List<Promotion_ProductDTO>> FilterListProduct([FromBody] Promotion_ProductFilterDTO Promotion_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Skip = 0;
            ProductFilter.Take = 20;
            ProductFilter.OrderBy = ProductOrder.Id;
            ProductFilter.OrderType = OrderType.ASC;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = Promotion_ProductFilterDTO.Id;
            ProductFilter.Code = Promotion_ProductFilterDTO.Code;
            ProductFilter.SupplierCode = Promotion_ProductFilterDTO.SupplierCode;
            ProductFilter.Name = Promotion_ProductFilterDTO.Name;
            ProductFilter.Description = Promotion_ProductFilterDTO.Description;
            ProductFilter.ScanCode = Promotion_ProductFilterDTO.ScanCode;
            ProductFilter.ERPCode = Promotion_ProductFilterDTO.ERPCode;
            ProductFilter.ProductTypeId = Promotion_ProductFilterDTO.ProductTypeId;
            ProductFilter.SupplierId = Promotion_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = Promotion_ProductFilterDTO.BrandId;
            ProductFilter.UnitOfMeasureId = Promotion_ProductFilterDTO.UnitOfMeasureId;
            ProductFilter.UnitOfMeasureGroupingId = Promotion_ProductFilterDTO.UnitOfMeasureGroupingId;
            ProductFilter.SalePrice = Promotion_ProductFilterDTO.SalePrice;
            ProductFilter.RetailPrice = Promotion_ProductFilterDTO.RetailPrice;
            ProductFilter.TaxTypeId = Promotion_ProductFilterDTO.TaxTypeId;
            ProductFilter.StatusId = Promotion_ProductFilterDTO.StatusId;
            ProductFilter.OtherName = Promotion_ProductFilterDTO.OtherName;
            ProductFilter.TechnicalName = Promotion_ProductFilterDTO.TechnicalName;
            ProductFilter.Note = Promotion_ProductFilterDTO.Note;
            ProductFilter.UsedVariationId = Promotion_ProductFilterDTO.UsedVariationId;

            List<Product> Products = await ProductService.List(ProductFilter);
            List<Promotion_ProductDTO> Promotion_ProductDTOs = Products
                .Select(x => new Promotion_ProductDTO(x)).ToList();
            return Promotion_ProductDTOs;
        }
        [Route(PromotionRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<Promotion_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] Promotion_StoreGroupingFilterDTO Promotion_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = Promotion_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = Promotion_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Promotion_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = Promotion_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = Promotion_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = Promotion_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = Promotion_StoreGroupingFilterDTO.StatusId;

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<Promotion_StoreGroupingDTO> Promotion_StoreGroupingDTOs = StoreGroupings
                .Select(x => new Promotion_StoreGroupingDTO(x)).ToList();
            return Promotion_StoreGroupingDTOs;
        }
        [Route(PromotionRoute.FilterListStore), HttpPost]
        public async Task<List<Promotion_StoreDTO>> FilterListStore([FromBody] Promotion_StoreFilterDTO Promotion_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = Promotion_StoreFilterDTO.Id;
            StoreFilter.Code = Promotion_StoreFilterDTO.Code;
            StoreFilter.Name = Promotion_StoreFilterDTO.Name;
            StoreFilter.UnsignName = Promotion_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = Promotion_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Promotion_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Promotion_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Promotion_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = Promotion_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Promotion_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Promotion_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Promotion_StoreFilterDTO.WardId;
            StoreFilter.Address = Promotion_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = Promotion_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = Promotion_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Promotion_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Promotion_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = Promotion_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = Promotion_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = Promotion_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Promotion_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Promotion_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = Promotion_StoreFilterDTO.StatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Promotion_StoreDTO> Promotion_StoreDTOs = Stores
                .Select(x => new Promotion_StoreDTO(x)).ToList();
            return Promotion_StoreDTOs;
        }
        [Route(PromotionRoute.FilterListStoreType), HttpPost]
        public async Task<List<Promotion_StoreTypeDTO>> FilterListStoreType([FromBody] Promotion_StoreTypeFilterDTO Promotion_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = Promotion_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Promotion_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Promotion_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = Promotion_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = Promotion_StoreTypeFilterDTO.StatusId;

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<Promotion_StoreTypeDTO> Promotion_StoreTypeDTOs = StoreTypes
                .Select(x => new Promotion_StoreTypeDTO(x)).ToList();
            return Promotion_StoreTypeDTOs;
        }

        [Route(PromotionRoute.SingleListOrganization), HttpPost]
        public async Task<List<Promotion_OrganizationDTO>> SingleListOrganization([FromBody] Promotion_OrganizationFilterDTO Promotion_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Promotion_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Promotion_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Promotion_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Promotion_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Promotion_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Promotion_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = Promotion_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = Promotion_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = Promotion_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = Promotion_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Promotion_OrganizationDTO> Promotion_OrganizationDTOs = Organizations
                .Select(x => new Promotion_OrganizationDTO(x)).ToList();
            return Promotion_OrganizationDTOs;
        }
        [Route(PromotionRoute.SingleListPromotionType), HttpPost]
        public async Task<List<Promotion_PromotionTypeDTO>> SingleListPromotionType([FromBody] Promotion_PromotionTypeFilterDTO Promotion_PromotionTypeFilterDTO)
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
            List<Promotion_PromotionTypeDTO> Promotion_PromotionTypeDTOs = PromotionTypes
                .Select(x => new Promotion_PromotionTypeDTO(x)).ToList();
            return Promotion_PromotionTypeDTOs;
        }
        [Route(PromotionRoute.SingleListStatus), HttpPost]
        public async Task<List<Promotion_StatusDTO>> SingleListStatus([FromBody] Promotion_StatusFilterDTO Promotion_StatusFilterDTO)
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
            List<Promotion_StatusDTO> Promotion_StatusDTOs = Statuses
                .Select(x => new Promotion_StatusDTO(x)).ToList();
            return Promotion_StatusDTOs;
        }
        [Route(PromotionRoute.SingleListPromotionPolicy), HttpPost]
        public async Task<List<Promotion_PromotionPolicyDTO>> SingleListPromotionPolicy([FromBody] Promotion_PromotionPolicyFilterDTO Promotion_PromotionPolicyFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionPolicyFilter PromotionPolicyFilter = new PromotionPolicyFilter();
            PromotionPolicyFilter.Skip = 0;
            PromotionPolicyFilter.Take = int.MaxValue;
            PromotionPolicyFilter.Take = 20;
            PromotionPolicyFilter.OrderBy = PromotionPolicyOrder.Id;
            PromotionPolicyFilter.OrderType = OrderType.ASC;
            PromotionPolicyFilter.Selects = PromotionPolicySelect.ALL;

            List<PromotionPolicy> PromotionPolicies = await PromotionPolicyService.List(PromotionPolicyFilter);
            List<Promotion_PromotionPolicyDTO> Promotion_PromotionPolicyDTOs = PromotionPolicies
                .Select(x => new Promotion_PromotionPolicyDTO(x)).ToList();
            return Promotion_PromotionPolicyDTOs;
        }
        [Route(PromotionRoute.SingleListPromotionDiscountType), HttpPost]
        public async Task<List<Promotion_PromotionDiscountTypeDTO>> SingleListPromotionDiscountType([FromBody] Promotion_PromotionDiscountTypeFilterDTO Promotion_PromotionDiscountTypeFilterDTO)
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
            List<Promotion_PromotionDiscountTypeDTO> Promotion_PromotionDiscountTypeDTOs = PromotionDiscountTypes
                .Select(x => new Promotion_PromotionDiscountTypeDTO(x)).ToList();
            return Promotion_PromotionDiscountTypeDTOs;
        }
        [Route(PromotionRoute.SingleListProductGrouping), HttpPost]
        public async Task<List<Promotion_ProductGroupingDTO>> SingleListProductGrouping([FromBody] Promotion_ProductGroupingFilterDTO Promotion_ProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.ALL;
            ProductGroupingFilter.Id = Promotion_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = Promotion_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = Promotion_ProductGroupingFilterDTO.Name;
            ProductGroupingFilter.Description = Promotion_ProductGroupingFilterDTO.Description;
            ProductGroupingFilter.ParentId = Promotion_ProductGroupingFilterDTO.ParentId;
            ProductGroupingFilter.Path = Promotion_ProductGroupingFilterDTO.Path;
            ProductGroupingFilter.Level = Promotion_ProductGroupingFilterDTO.Level;

            if (ProductGroupingFilter.Id == null) ProductGroupingFilter.Id = new IdFilter();
            ProductGroupingFilter.Id.In = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<Promotion_ProductGroupingDTO> Promotion_ProductGroupingDTOs = ProductGroupings
                .Select(x => new Promotion_ProductGroupingDTO(x)).ToList();
            return Promotion_ProductGroupingDTOs;
        }
        [Route(PromotionRoute.SingleListProductType), HttpPost]
        public async Task<List<Promotion_ProductTypeDTO>> SingleListProductType([FromBody] Promotion_ProductTypeFilterDTO Promotion_ProductTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = Promotion_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = Promotion_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = Promotion_ProductTypeFilterDTO.Name;
            ProductTypeFilter.Description = Promotion_ProductTypeFilterDTO.Description;
            ProductTypeFilter.StatusId = Promotion_ProductTypeFilterDTO.StatusId;

            if (ProductTypeFilter.Id == null) ProductTypeFilter.Id = new IdFilter();
            ProductTypeFilter.Id.In = await FilterProductType(ProductTypeService, CurrentContext);
            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<Promotion_ProductTypeDTO> Promotion_ProductTypeDTOs = ProductTypes
                .Select(x => new Promotion_ProductTypeDTO(x)).ToList();
            return Promotion_ProductTypeDTOs;
        }
        [Route(PromotionRoute.SingleListProduct), HttpPost]
        public async Task<List<Promotion_ProductDTO>> SingleListProduct([FromBody] Promotion_ProductFilterDTO Promotion_ProductFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Skip = 0;
            ProductFilter.Take = 20;
            ProductFilter.OrderBy = ProductOrder.Id;
            ProductFilter.OrderType = OrderType.ASC;
            ProductFilter.Selects = ProductSelect.ALL;
            ProductFilter.Id = Promotion_ProductFilterDTO.Id;
            ProductFilter.Code = Promotion_ProductFilterDTO.Code;
            ProductFilter.SupplierCode = Promotion_ProductFilterDTO.SupplierCode;
            ProductFilter.Name = Promotion_ProductFilterDTO.Name;
            ProductFilter.Description = Promotion_ProductFilterDTO.Description;
            ProductFilter.ScanCode = Promotion_ProductFilterDTO.ScanCode;
            ProductFilter.ERPCode = Promotion_ProductFilterDTO.ERPCode;
            ProductFilter.ProductTypeId = Promotion_ProductFilterDTO.ProductTypeId;
            ProductFilter.SupplierId = Promotion_ProductFilterDTO.SupplierId;
            ProductFilter.BrandId = Promotion_ProductFilterDTO.BrandId;
            ProductFilter.UnitOfMeasureId = Promotion_ProductFilterDTO.UnitOfMeasureId;
            ProductFilter.UnitOfMeasureGroupingId = Promotion_ProductFilterDTO.UnitOfMeasureGroupingId;
            ProductFilter.SalePrice = Promotion_ProductFilterDTO.SalePrice;
            ProductFilter.RetailPrice = Promotion_ProductFilterDTO.RetailPrice;
            ProductFilter.TaxTypeId = Promotion_ProductFilterDTO.TaxTypeId;
            ProductFilter.StatusId = Promotion_ProductFilterDTO.StatusId;
            ProductFilter.OtherName = Promotion_ProductFilterDTO.OtherName;
            ProductFilter.TechnicalName = Promotion_ProductFilterDTO.TechnicalName;
            ProductFilter.Note = Promotion_ProductFilterDTO.Note;
            ProductFilter.UsedVariationId = Promotion_ProductFilterDTO.UsedVariationId;

            List<Product> Products = await ProductService.List(ProductFilter);
            List<Promotion_ProductDTO> Promotion_ProductDTOs = Products
                .Select(x => new Promotion_ProductDTO(x)).ToList();
            return Promotion_ProductDTOs;
        }
        [Route(PromotionRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<Promotion_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] Promotion_StoreGroupingFilterDTO Promotion_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = Promotion_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = Promotion_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Promotion_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = Promotion_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = Promotion_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = Promotion_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = Promotion_StoreGroupingFilterDTO.StatusId;

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<Promotion_StoreGroupingDTO> Promotion_StoreGroupingDTOs = StoreGroupings
                .Select(x => new Promotion_StoreGroupingDTO(x)).ToList();
            return Promotion_StoreGroupingDTOs;
        }
        [Route(PromotionRoute.SingleListStore), HttpPost]
        public async Task<List<Promotion_StoreDTO>> SingleListStore([FromBody] Promotion_StoreFilterDTO Promotion_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = Promotion_StoreFilterDTO.Id;
            StoreFilter.Code = Promotion_StoreFilterDTO.Code;
            StoreFilter.Name = Promotion_StoreFilterDTO.Name;
            StoreFilter.UnsignName = Promotion_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = Promotion_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Promotion_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Promotion_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Promotion_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = Promotion_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Promotion_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Promotion_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Promotion_StoreFilterDTO.WardId;
            StoreFilter.Address = Promotion_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = Promotion_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = Promotion_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Promotion_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Promotion_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = Promotion_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = Promotion_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = Promotion_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Promotion_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Promotion_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = Promotion_StoreFilterDTO.StatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Promotion_StoreDTO> Promotion_StoreDTOs = Stores
                .Select(x => new Promotion_StoreDTO(x)).ToList();
            return Promotion_StoreDTOs;
        }
        [Route(PromotionRoute.SingleListStoreType), HttpPost]
        public async Task<List<Promotion_StoreTypeDTO>> SingleListStoreType([FromBody] Promotion_StoreTypeFilterDTO Promotion_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = Promotion_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Promotion_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Promotion_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = Promotion_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = Promotion_StoreTypeFilterDTO.StatusId;

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<Promotion_StoreTypeDTO> Promotion_StoreTypeDTOs = StoreTypes
                .Select(x => new Promotion_StoreTypeDTO(x)).ToList();
            return Promotion_StoreTypeDTOs;
        }

        [Route(PromotionRoute.CountStoreGrouping), HttpPost]
        public async Task<long> CountStoreGrouping([FromBody] Promotion_StoreGroupingFilterDTO Promotion_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Id = Promotion_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = Promotion_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Promotion_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = Promotion_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = Promotion_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = Promotion_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = Promotion_StoreGroupingFilterDTO.StatusId;

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            return await StoreGroupingService.Count(StoreGroupingFilter);
        }

        [Route(PromotionRoute.ListStoreGrouping), HttpPost]
        public async Task<List<Promotion_StoreGroupingDTO>> ListStoreGrouping([FromBody] Promotion_StoreGroupingFilterDTO Promotion_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = Promotion_StoreGroupingFilterDTO.Skip;
            StoreGroupingFilter.Take = Promotion_StoreGroupingFilterDTO.Take;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = Promotion_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = Promotion_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Promotion_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = Promotion_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = Promotion_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = Promotion_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = Promotion_StoreGroupingFilterDTO.StatusId;

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<Promotion_StoreGroupingDTO> Promotion_StoreGroupingDTOs = StoreGroupings
                .Select(x => new Promotion_StoreGroupingDTO(x)).ToList();
            return Promotion_StoreGroupingDTOs;
        }
        [Route(PromotionRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] Promotion_StoreFilterDTO Promotion_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = Promotion_StoreFilterDTO.Id;
            StoreFilter.Code = Promotion_StoreFilterDTO.Code;
            StoreFilter.Name = Promotion_StoreFilterDTO.Name;
            StoreFilter.UnsignName = Promotion_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = Promotion_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Promotion_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Promotion_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Promotion_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = Promotion_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Promotion_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Promotion_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Promotion_StoreFilterDTO.WardId;
            StoreFilter.Address = Promotion_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = Promotion_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = Promotion_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Promotion_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Promotion_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = Promotion_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = Promotion_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = Promotion_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Promotion_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Promotion_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = Promotion_StoreFilterDTO.StatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            return await StoreService.Count(StoreFilter);
        }

        [Route(PromotionRoute.ListStore), HttpPost]
        public async Task<List<Promotion_StoreDTO>> ListStore([FromBody] Promotion_StoreFilterDTO Promotion_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = Promotion_StoreFilterDTO.Skip;
            StoreFilter.Take = Promotion_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = Promotion_StoreFilterDTO.Id;
            StoreFilter.Code = Promotion_StoreFilterDTO.Code;
            StoreFilter.Name = Promotion_StoreFilterDTO.Name;
            StoreFilter.UnsignName = Promotion_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = Promotion_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Promotion_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Promotion_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Promotion_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = Promotion_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Promotion_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Promotion_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Promotion_StoreFilterDTO.WardId;
            StoreFilter.Address = Promotion_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = Promotion_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = Promotion_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Promotion_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Promotion_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = Promotion_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = Promotion_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = Promotion_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Promotion_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Promotion_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = Promotion_StoreFilterDTO.StatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Promotion_StoreDTO> Promotion_StoreDTOs = Stores
                .Select(x => new Promotion_StoreDTO(x)).ToList();
            return Promotion_StoreDTOs;
        }
        [Route(PromotionRoute.CountStoreType), HttpPost]
        public async Task<long> CountStoreType([FromBody] Promotion_StoreTypeFilterDTO Promotion_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Id = Promotion_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Promotion_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Promotion_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = Promotion_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = Promotion_StoreTypeFilterDTO.StatusId;

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            return await StoreTypeService.Count(StoreTypeFilter);
        }

        [Route(PromotionRoute.ListStoreType), HttpPost]
        public async Task<List<Promotion_StoreTypeDTO>> ListStoreType([FromBody] Promotion_StoreTypeFilterDTO Promotion_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = Promotion_StoreTypeFilterDTO.Skip;
            StoreTypeFilter.Take = Promotion_StoreTypeFilterDTO.Take;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = Promotion_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Promotion_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Promotion_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = Promotion_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = Promotion_StoreTypeFilterDTO.StatusId;

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<Promotion_StoreTypeDTO> Promotion_StoreTypeDTOs = StoreTypes
                .Select(x => new Promotion_StoreTypeDTO(x)).ToList();
            return Promotion_StoreTypeDTOs;
        }
    }
}

