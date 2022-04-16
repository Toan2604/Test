using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public partial class IndirectSalesOrderController : RpcController
    {

        [Route(IndirectSalesOrderRoute.FilterListStore), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreDTO>> FilterListStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = IndirectSalesOrder_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = IndirectSalesOrder_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = IndirectSalesOrder_StoreFilterDTO.StatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await IndirectSalesOrderService.ListStore(StoreFilter);
            List<IndirectSalesOrder_StoreDTO> IndirectSalesOrder_StoreDTOs = Stores
                .Select(x => new IndirectSalesOrder_StoreDTO(x)).ToList();
            return IndirectSalesOrder_StoreDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreStatusDTO>> FilterListStoreStatus([FromBody] IndirectSalesOrder_StoreStatusFilterDTO IndirectSalesOrder_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = IndirectSalesOrder_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = IndirectSalesOrder_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = IndirectSalesOrder_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<IndirectSalesOrder_StoreStatusDTO> IndirectSalesOrder_StoreStatusDTOs = StoreStatuses
                .Select(x => new IndirectSalesOrder_StoreStatusDTO(x)).ToList();
            return IndirectSalesOrder_StoreStatusDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListAppUser), HttpPost]
        public async Task<List<IndirectSalesOrder_AppUserDTO>> FilterListAppUser([FromBody] IndirectSalesOrder_AppUserFilterDTO IndirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = IndirectSalesOrder_AppUserFilterDTO.Id;
            AppUserFilter.Username = IndirectSalesOrder_AppUserFilterDTO.Username;
            AppUserFilter.Password = IndirectSalesOrder_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = IndirectSalesOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = IndirectSalesOrder_AppUserFilterDTO.Address;
            AppUserFilter.Email = IndirectSalesOrder_AppUserFilterDTO.Email;
            AppUserFilter.Phone = IndirectSalesOrder_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = IndirectSalesOrder_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = IndirectSalesOrder_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = IndirectSalesOrder_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = IndirectSalesOrder_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = IndirectSalesOrder_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = IndirectSalesOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = IndirectSalesOrder_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<IndirectSalesOrder_AppUserDTO> IndirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new IndirectSalesOrder_AppUserDTO(x)).ToList();
            return IndirectSalesOrder_AppUserDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListOrganization), HttpPost]
        public async Task<List<IndirectSalesOrder_OrganizationDTO>> FilterListOrganization([FromBody] IndirectSalesOrder_OrganizationFilterDTO IndirectSalesOrder_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = IndirectSalesOrder_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = IndirectSalesOrder_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = IndirectSalesOrder_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = IndirectSalesOrder_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = IndirectSalesOrder_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = IndirectSalesOrder_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = null;
            OrganizationFilter.Phone = IndirectSalesOrder_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = IndirectSalesOrder_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = IndirectSalesOrder_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<IndirectSalesOrder_OrganizationDTO> IndirectSalesOrder_OrganizationDTOs = Organizations
                .Select(x => new IndirectSalesOrder_OrganizationDTO(x)).ToList();
            return IndirectSalesOrder_OrganizationDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListUnitOfMeasure), HttpPost]
        public async Task<List<IndirectSalesOrder_UnitOfMeasureDTO>> FilterListUnitOfMeasure([FromBody] IndirectSalesOrder_UnitOfMeasureFilterDTO IndirectSalesOrder_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter();
            UnitOfMeasureFilter.Skip = 0;
            UnitOfMeasureFilter.Take = 20;
            UnitOfMeasureFilter.OrderBy = UnitOfMeasureOrder.Id;
            UnitOfMeasureFilter.OrderType = OrderType.ASC;
            UnitOfMeasureFilter.Selects = UnitOfMeasureSelect.ALL;
            UnitOfMeasureFilter.Id = IndirectSalesOrder_UnitOfMeasureFilterDTO.Id;
            UnitOfMeasureFilter.Code = IndirectSalesOrder_UnitOfMeasureFilterDTO.Code;
            UnitOfMeasureFilter.Name = IndirectSalesOrder_UnitOfMeasureFilterDTO.Name;
            UnitOfMeasureFilter.Description = IndirectSalesOrder_UnitOfMeasureFilterDTO.Description;
            UnitOfMeasureFilter.StatusId = IndirectSalesOrder_UnitOfMeasureFilterDTO.StatusId;

            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);
            List<IndirectSalesOrder_UnitOfMeasureDTO> IndirectSalesOrder_UnitOfMeasureDTOs = UnitOfMeasures
                .Select(x => new IndirectSalesOrder_UnitOfMeasureDTO(x)).ToList();
            return IndirectSalesOrder_UnitOfMeasureDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListItem), HttpPost]
        public async Task<List<IndirectSalesOrder_ItemDTO>> FilterListItem([FromBody] IndirectSalesOrder_ItemFilterDTO IndirectSalesOrder_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = IndirectSalesOrder_ItemFilterDTO.Id;
            ItemFilter.ProductId = IndirectSalesOrder_ItemFilterDTO.ProductId;
            ItemFilter.Code = IndirectSalesOrder_ItemFilterDTO.Code;
            ItemFilter.Name = IndirectSalesOrder_ItemFilterDTO.Name;
            ItemFilter.ScanCode = IndirectSalesOrder_ItemFilterDTO.ScanCode;
            ItemFilter.SalePrice = IndirectSalesOrder_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = IndirectSalesOrder_ItemFilterDTO.RetailPrice;
            ItemFilter.StatusId = IndirectSalesOrder_ItemFilterDTO.StatusId;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<IndirectSalesOrder_ItemDTO> IndirectSalesOrder_ItemDTOs = Items
                .Select(x => new IndirectSalesOrder_ItemDTO(x)).ToList();
            return IndirectSalesOrder_ItemDTOs;
        }
        [Route(IndirectSalesOrderRoute.FilterListEditedPriceStatus), HttpPost]
        public async Task<List<IndirectSalesOrder_EditedPriceStatusDTO>> FilterListEditedPriceStatus([FromBody] IndirectSalesOrder_EditedPriceStatusFilterDTO IndirectSalesOrder_EditedPriceStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            EditedPriceStatusFilter EditedPriceStatusFilter = new EditedPriceStatusFilter();
            EditedPriceStatusFilter.Skip = 0;
            EditedPriceStatusFilter.Take = 20;
            EditedPriceStatusFilter.OrderBy = EditedPriceStatusOrder.Id;
            EditedPriceStatusFilter.OrderType = OrderType.ASC;
            EditedPriceStatusFilter.Selects = EditedPriceStatusSelect.ALL;
            EditedPriceStatusFilter.Id = IndirectSalesOrder_EditedPriceStatusFilterDTO.Id;
            EditedPriceStatusFilter.Code = IndirectSalesOrder_EditedPriceStatusFilterDTO.Code;
            EditedPriceStatusFilter.Name = IndirectSalesOrder_EditedPriceStatusFilterDTO.Name;

            List<EditedPriceStatus> EditedPriceStatuses = await EditedPriceStatusService.List(EditedPriceStatusFilter);
            List<IndirectSalesOrder_EditedPriceStatusDTO> IndirectSalesOrder_EditedPriceStatusDTOs = EditedPriceStatuses
                .Select(x => new IndirectSalesOrder_EditedPriceStatusDTO(x)).ToList();
            return IndirectSalesOrder_EditedPriceStatusDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListRequestState), HttpPost]
        public async Task<List<IndirectSalesOrder_RequestStateDTO>> FilterListRequestState([FromBody] IndirectSalesOrder_RequestStateFilterDTO IndirectSalesOrder_RequestStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = IndirectSalesOrder_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = IndirectSalesOrder_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = IndirectSalesOrder_RequestStateFilterDTO.Name;

            List<RequestState> RequestStatees = await RequestStateService.List(RequestStateFilter);
            List<IndirectSalesOrder_RequestStateDTO> IndirectSalesOrder_RequestStateDTOs = RequestStatees
                .Select(x => new IndirectSalesOrder_RequestStateDTO(x)).ToList();
            return IndirectSalesOrder_RequestStateDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListBrand), HttpPost]
        public async Task<List<IndirectSalesOrder_BrandDTO>> FilterListBrand([FromBody] IndirectSalesOrder_BrandFilterDTO IndirectSalesOrder_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = 0;
            BrandFilter.Take = 20;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.ALL;
            BrandFilter.Id = IndirectSalesOrder_BrandFilterDTO.Id;
            BrandFilter.Code = IndirectSalesOrder_BrandFilterDTO.Code;
            BrandFilter.Name = IndirectSalesOrder_BrandFilterDTO.Name;
            BrandFilter.Description = IndirectSalesOrder_BrandFilterDTO.Description;
            BrandFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            BrandFilter.UpdateTime = IndirectSalesOrder_BrandFilterDTO.UpdateTime;

            List<Brand> Brands = await BrandService.List(BrandFilter);
            List<IndirectSalesOrder_BrandDTO> IndirectSalesOrder_BrandDTOs = Brands
                .Select(x => new IndirectSalesOrder_BrandDTO(x)).ToList();
            return IndirectSalesOrder_BrandDTOs;
        }

        [Route(IndirectSalesOrderRoute.FilterListCategory), HttpPost]
        public async Task<List<IndirectSalesOrder_CategoryDTO>> FilterListCategory([FromBody] IndirectSalesOrder_CategoryFilterDTO IndirectSalesOrder_CategoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            CategoryFilter CategoryFilter = new CategoryFilter();
            CategoryFilter.Skip = 0;
            CategoryFilter.Take = int.MaxValue;
            CategoryFilter.OrderBy = CategoryOrder.Id;
            CategoryFilter.OrderType = OrderType.ASC;
            CategoryFilter.Selects = CategorySelect.ALL;
            CategoryFilter.Id = IndirectSalesOrder_CategoryFilterDTO.Id;
            CategoryFilter.Code = IndirectSalesOrder_CategoryFilterDTO.Code;
            CategoryFilter.Name = IndirectSalesOrder_CategoryFilterDTO.Name;
            CategoryFilter.ParentId = IndirectSalesOrder_CategoryFilterDTO.ParentId;
            CategoryFilter.Path = IndirectSalesOrder_CategoryFilterDTO.Path;
            CategoryFilter.Level = IndirectSalesOrder_CategoryFilterDTO.Level;
            CategoryFilter.StatusId = IndirectSalesOrder_CategoryFilterDTO.StatusId;
            CategoryFilter.ImageId = IndirectSalesOrder_CategoryFilterDTO.ImageId;
            CategoryFilter.RowId = IndirectSalesOrder_CategoryFilterDTO.RowId;

            List<Category> Categories = await CategoryService.List(CategoryFilter);
            List<IndirectSalesOrder_CategoryDTO> IndirectSalesOrder_CategoryDTOs = Categories
                .Select(x => new IndirectSalesOrder_CategoryDTO(x)).ToList();
            return IndirectSalesOrder_CategoryDTOs;
        }


        [Route(IndirectSalesOrderRoute.SingleListStore), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreDTO>> SingleListStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = IndirectSalesOrder_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = IndirectSalesOrder_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<IndirectSalesOrder_StoreDTO> IndirectSalesOrder_StoreDTOs = Stores
                .Select(x => new IndirectSalesOrder_StoreDTO(x)).ToList();
            return IndirectSalesOrder_StoreDTOs;
        }
        [Route(IndirectSalesOrderRoute.SingleListAppUser), HttpPost]
        public async Task<List<IndirectSalesOrder_AppUserDTO>> SingleListAppUser([FromBody] IndirectSalesOrder_AppUserFilterDTO IndirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = IndirectSalesOrder_AppUserFilterDTO.Id;
            AppUserFilter.Username = IndirectSalesOrder_AppUserFilterDTO.Username;
            AppUserFilter.Password = IndirectSalesOrder_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = IndirectSalesOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = IndirectSalesOrder_AppUserFilterDTO.Address;
            AppUserFilter.Email = IndirectSalesOrder_AppUserFilterDTO.Email;
            AppUserFilter.Phone = IndirectSalesOrder_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = IndirectSalesOrder_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = IndirectSalesOrder_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = IndirectSalesOrder_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = IndirectSalesOrder_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            AppUserFilter.Birthday = IndirectSalesOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = IndirectSalesOrder_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<IndirectSalesOrder_AppUserDTO> IndirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new IndirectSalesOrder_AppUserDTO(x)).ToList();
            return IndirectSalesOrder_AppUserDTOs;
        }

        [Route(IndirectSalesOrderRoute.SingleListOrganization), HttpPost]
        public async Task<List<IndirectSalesOrder_OrganizationDTO>> SingleListOrganization([FromBody] IndirectSalesOrder_OrganizationFilterDTO IndirectSalesOrder_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = IndirectSalesOrder_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = IndirectSalesOrder_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = IndirectSalesOrder_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = IndirectSalesOrder_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = IndirectSalesOrder_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = IndirectSalesOrder_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = IndirectSalesOrder_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = IndirectSalesOrder_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = IndirectSalesOrder_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = IndirectSalesOrder_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<IndirectSalesOrder_OrganizationDTO> IndirectSalesOrder_OrganizationDTOs = Organizations
                .Select(x => new IndirectSalesOrder_OrganizationDTO(x)).ToList();
            return IndirectSalesOrder_OrganizationDTOs;
        }

        [Route(IndirectSalesOrderRoute.SingleListUnitOfMeasure), HttpPost]
        public async Task<List<IndirectSalesOrder_UnitOfMeasureDTO>> SingleListUnitOfMeasure([FromBody] IndirectSalesOrder_UnitOfMeasureFilterDTO IndirectSalesOrder_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            //TODO cần optimize lại phần này, sử dụng itemId thay vì productId

            List<Product> Products = await ProductService.List(new ProductFilter
            {
                Id = IndirectSalesOrder_UnitOfMeasureFilterDTO.ProductId,
                Selects = ProductSelect.Id,
            });
            long ProductId = Products.Select(p => p.Id).FirstOrDefault();
            Product Product = await ProductService.Get(ProductId);

            List<IndirectSalesOrder_UnitOfMeasureDTO> IndirectSalesOrder_UnitOfMeasureDTOs = new List<IndirectSalesOrder_UnitOfMeasureDTO>();
            if (Product.UnitOfMeasureGrouping != null)
            {
                IndirectSalesOrder_UnitOfMeasureDTOs = Product.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents.Select(x => new IndirectSalesOrder_UnitOfMeasureDTO(x)).ToList();
            }
            IndirectSalesOrder_UnitOfMeasureDTO IndirectSalesOrder_UnitOfMeasureDTO = new IndirectSalesOrder_UnitOfMeasureDTO
            {
                Id = Product.UnitOfMeasure.Id,
                Code = Product.UnitOfMeasure.Code,
                Name = Product.UnitOfMeasure.Name,
                Description = Product.UnitOfMeasure.Description,
                StatusId = StatusEnum.ACTIVE.Id,
                Factor = 1,
            };
            IndirectSalesOrder_UnitOfMeasureDTOs.Add(IndirectSalesOrder_UnitOfMeasureDTO);
            IndirectSalesOrder_UnitOfMeasureDTOs = IndirectSalesOrder_UnitOfMeasureDTOs.Distinct().ToList();
            return IndirectSalesOrder_UnitOfMeasureDTOs;
        }

        [Route(IndirectSalesOrderRoute.SingleListSupplier), HttpPost]
        public async Task<List<IndirectSalesOrder_SupplierDTO>> SingleListSupplier([FromBody] IndirectSalesOrder_SupplierFilterDTO Product_SupplierFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SupplierFilter SupplierFilter = new SupplierFilter();
            SupplierFilter.Skip = 0;
            SupplierFilter.Take = 20;
            SupplierFilter.OrderBy = SupplierOrder.Id;
            SupplierFilter.OrderType = OrderType.ASC;
            SupplierFilter.Selects = SupplierSelect.ALL;
            SupplierFilter.Id = Product_SupplierFilterDTO.Id;
            SupplierFilter.Code = Product_SupplierFilterDTO.Code;
            SupplierFilter.Name = Product_SupplierFilterDTO.Name;
            SupplierFilter.TaxCode = Product_SupplierFilterDTO.TaxCode;
            SupplierFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Supplier> Suppliers = await SupplierService.List(SupplierFilter);
            List<IndirectSalesOrder_SupplierDTO> IndirectSalesOrder_SupplierDTOs = Suppliers
                .Select(x => new IndirectSalesOrder_SupplierDTO(x)).ToList();
            return IndirectSalesOrder_SupplierDTOs;
        }
        [Route(IndirectSalesOrderRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] IndirectSalesOrder_StoreGroupingFilterDTO IndirectSalesOrder_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = IndirectSalesOrder_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = IndirectSalesOrder_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = IndirectSalesOrder_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = IndirectSalesOrder_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<IndirectSalesOrder_StoreGroupingDTO> IndirectSalesOrder_StoreGroupingDTOs = StoreGroupings
                .Select(x => new IndirectSalesOrder_StoreGroupingDTO(x)).ToList();
            return IndirectSalesOrder_StoreGroupingDTOs;
        }
        [Route(IndirectSalesOrderRoute.SingleListStoreType), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreTypeDTO>> SingleListStoreType([FromBody] IndirectSalesOrder_StoreTypeFilterDTO IndirectSalesOrder_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = IndirectSalesOrder_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = IndirectSalesOrder_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = IndirectSalesOrder_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<IndirectSalesOrder_StoreTypeDTO> IndirectSalesOrder_StoreTypeDTOs = StoreTypes
                .Select(x => new IndirectSalesOrder_StoreTypeDTO(x)).ToList();
            return IndirectSalesOrder_StoreTypeDTOs;
        }
        [Route(IndirectSalesOrderRoute.SingleListItem), HttpPost]
        public async Task<List<IndirectSalesOrder_ItemDTO>> SingleListItem([FromBody] IndirectSalesOrder_ItemFilterDTO IndirectSalesOrder_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = IndirectSalesOrder_ItemFilterDTO.Id;
            ItemFilter.ProductId = IndirectSalesOrder_ItemFilterDTO.ProductId;
            ItemFilter.Code = IndirectSalesOrder_ItemFilterDTO.Code;
            ItemFilter.Name = IndirectSalesOrder_ItemFilterDTO.Name;
            ItemFilter.ScanCode = IndirectSalesOrder_ItemFilterDTO.ScanCode;
            ItemFilter.SalePrice = IndirectSalesOrder_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = IndirectSalesOrder_ItemFilterDTO.RetailPrice;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Item> Items = await ItemService.List(ItemFilter);
            List<IndirectSalesOrder_ItemDTO> IndirectSalesOrder_ItemDTOs = Items
                .Select(x => new IndirectSalesOrder_ItemDTO(x)).ToList();
            return IndirectSalesOrder_ItemDTOs;
        }

        [Route(IndirectSalesOrderRoute.SingleListProductGrouping), HttpPost]
        public async Task<List<IndirectSalesOrder_ProductGroupingDTO>> SingleListProductGrouping([FromBody] IndirectSalesOrder_ProductGroupingFilterDTO IndirectSalesOrder_ProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            List<ProductGrouping> IndirectSalesOrderGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<IndirectSalesOrder_ProductGroupingDTO> IndirectSalesOrder_ProductGroupingDTOs = IndirectSalesOrderGroupings
                .Select(x => new IndirectSalesOrder_ProductGroupingDTO(x)).ToList();
            return IndirectSalesOrder_ProductGroupingDTOs;
        }
        [Route(IndirectSalesOrderRoute.SingleListProductType), HttpPost]
        public async Task<List<IndirectSalesOrder_ProductTypeDTO>> SingleListProductType([FromBody] IndirectSalesOrder_ProductTypeFilterDTO IndirectSalesOrder_ProductTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = IndirectSalesOrder_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = IndirectSalesOrder_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = IndirectSalesOrder_ProductTypeFilterDTO.Name;
            ProductTypeFilter.Description = IndirectSalesOrder_ProductTypeFilterDTO.Description;
            ProductTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<IndirectSalesOrder_ProductTypeDTO> IndirectSalesOrder_ProductTypeDTOs = ProductTypes
                .Select(x => new IndirectSalesOrder_ProductTypeDTO(x)).ToList();
            return IndirectSalesOrder_ProductTypeDTOs;
        }

        [Route(IndirectSalesOrderRoute.SingleListEditedPriceStatus), HttpPost]
        public async Task<List<IndirectSalesOrder_EditedPriceStatusDTO>> SingleListEditedPriceStatus([FromBody] IndirectSalesOrder_EditedPriceStatusFilterDTO IndirectSalesOrder_EditedPriceStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            EditedPriceStatusFilter EditedPriceStatusFilter = new EditedPriceStatusFilter();
            EditedPriceStatusFilter.Skip = 0;
            EditedPriceStatusFilter.Take = 20;
            EditedPriceStatusFilter.OrderBy = EditedPriceStatusOrder.Id;
            EditedPriceStatusFilter.OrderType = OrderType.ASC;
            EditedPriceStatusFilter.Selects = EditedPriceStatusSelect.ALL;
            EditedPriceStatusFilter.Id = IndirectSalesOrder_EditedPriceStatusFilterDTO.Id;
            EditedPriceStatusFilter.Code = IndirectSalesOrder_EditedPriceStatusFilterDTO.Code;
            EditedPriceStatusFilter.Name = IndirectSalesOrder_EditedPriceStatusFilterDTO.Name;

            List<EditedPriceStatus> EditedPriceStatuses = await EditedPriceStatusService.List(EditedPriceStatusFilter);
            List<IndirectSalesOrder_EditedPriceStatusDTO> IndirectSalesOrder_EditedPriceStatusDTOs = EditedPriceStatuses
                .Select(x => new IndirectSalesOrder_EditedPriceStatusDTO(x)).ToList();
            return IndirectSalesOrder_EditedPriceStatusDTOs;
        }
        [Route(IndirectSalesOrderRoute.SingleListRequestState), HttpPost]
        public async Task<List<IndirectSalesOrder_RequestStateDTO>> SingleListRequestState([FromBody] IndirectSalesOrder_RequestStateFilterDTO IndirectSalesOrder_RequestStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = IndirectSalesOrder_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = IndirectSalesOrder_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = IndirectSalesOrder_RequestStateFilterDTO.Name;

            List<RequestState> RequestStatees = await RequestStateService.List(RequestStateFilter);
            List<IndirectSalesOrder_RequestStateDTO> IndirectSalesOrder_RequestStateDTOs = RequestStatees
                .Select(x => new IndirectSalesOrder_RequestStateDTO(x)).ToList();
            return IndirectSalesOrder_RequestStateDTOs;
        }

        [Route(IndirectSalesOrderRoute.SingleListTaxType), HttpPost]
        public async Task<List<IndirectSalesOrder_TaxTypeDTO>> SingleListTaxType([FromBody] IndirectSalesOrder_TaxTypeFilterDTO IndirectSalesOrder_TaxTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            TaxTypeFilter TaxTypeFilter = new TaxTypeFilter();
            TaxTypeFilter.Skip = 0;
            TaxTypeFilter.Take = 20;
            TaxTypeFilter.OrderBy = TaxTypeOrder.Id;
            TaxTypeFilter.OrderType = OrderType.ASC;
            TaxTypeFilter.Selects = TaxTypeSelect.ALL;
            TaxTypeFilter.Id = IndirectSalesOrder_TaxTypeFilterDTO.Id;
            TaxTypeFilter.Code = IndirectSalesOrder_TaxTypeFilterDTO.Code;
            TaxTypeFilter.Name = IndirectSalesOrder_TaxTypeFilterDTO.Name;
            TaxTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<TaxType> TaxTypes = await TaxTypeService.List(TaxTypeFilter);
            List<IndirectSalesOrder_TaxTypeDTO> IndirectSalesOrder_TaxTypeDTOs = TaxTypes
                .Select(x => new IndirectSalesOrder_TaxTypeDTO(x)).ToList();
            return IndirectSalesOrder_TaxTypeDTOs;
        }

        [Route(IndirectSalesOrderRoute.CountBuyerStore), HttpPost]
        public async Task<long> CountBuyerStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = IndirectSalesOrder_StoreFilterDTO.StoreStatusId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            if (IndirectSalesOrder_StoreFilterDTO.SaleEmployeeId.HasValue && IndirectSalesOrder_StoreFilterDTO.SaleEmployeeId.Equal.HasValue)
            {
                int count = await IndirectSalesOrderService.CountInScoped(StoreFilter, IndirectSalesOrder_StoreFilterDTO.SaleEmployeeId.Equal.Value);
                return count;
            }
            return 0;
        }

        [Route(IndirectSalesOrderRoute.ListBuyerStore), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreDTO>> ListBuyerStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = IndirectSalesOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = IndirectSalesOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = IndirectSalesOrder_StoreFilterDTO.StoreStatusId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (IndirectSalesOrder_StoreFilterDTO.SaleEmployeeId.HasValue && IndirectSalesOrder_StoreFilterDTO.SaleEmployeeId.Equal.HasValue)
            {
                List<Store> Stores = await IndirectSalesOrderService.ListInScoped(StoreFilter, IndirectSalesOrder_StoreFilterDTO.SaleEmployeeId.Equal.Value);
                List<IndirectSalesOrder_StoreDTO> IndirectSalesOrder_StoreDTOs = Stores
                    .Select(x => new IndirectSalesOrder_StoreDTO(x)).ToList();
                return IndirectSalesOrder_StoreDTOs;
            }
            return new List<IndirectSalesOrder_StoreDTO>();

        }

        [Route(IndirectSalesOrderRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            return await StoreService.Count(StoreFilter);
        }

        [Route(IndirectSalesOrderRoute.ListStore), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreDTO>> ListStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = IndirectSalesOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = IndirectSalesOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreFilter.Id.NotEqual.HasValue)
                StoreFilter.Id.In.Remove(StoreFilter.Id.NotEqual.Value);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<IndirectSalesOrder_StoreDTO> IndirectSalesOrder_StoreDTOs = Stores
                .Select(x => new IndirectSalesOrder_StoreDTO(x)).ToList();
            return IndirectSalesOrder_StoreDTOs;
        }

        [Route(IndirectSalesOrderRoute.CountSellerStore), HttpPost]
        public async Task<long> CountSellerStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreService.Count(StoreFilter);
        }

        [Route(IndirectSalesOrderRoute.ListSellerStore), HttpPost]
        public async Task<List<IndirectSalesOrder_StoreDTO>> ListSellerStore([FromBody] IndirectSalesOrder_StoreFilterDTO IndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = IndirectSalesOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = IndirectSalesOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = IndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = IndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = IndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = IndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = IndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = IndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = IndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = IndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = IndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = IndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = IndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = IndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = IndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = IndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = IndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = IndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = IndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = IndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = IndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<IndirectSalesOrder_StoreDTO> IndirectSalesOrder_StoreDTOs = Stores
                .Select(x => new IndirectSalesOrder_StoreDTO(x)).ToList();
            return IndirectSalesOrder_StoreDTOs;
        }

        [Route(IndirectSalesOrderRoute.CountItem), HttpPost]
        public async Task<long> CountItem([FromBody] IndirectSalesOrder_ItemFilterDTO IndirectSalesOrder_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Id = IndirectSalesOrder_ItemFilterDTO.Id;
            ItemFilter.Code = IndirectSalesOrder_ItemFilterDTO.Code;
            ItemFilter.Name = IndirectSalesOrder_ItemFilterDTO.Name;
            ItemFilter.OtherName = IndirectSalesOrder_ItemFilterDTO.OtherName;
            ItemFilter.ProductGroupingId = IndirectSalesOrder_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = IndirectSalesOrder_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = IndirectSalesOrder_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = IndirectSalesOrder_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = IndirectSalesOrder_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = IndirectSalesOrder_ItemFilterDTO.ScanCode;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = IndirectSalesOrder_ItemFilterDTO.SupplierId;
            ItemFilter.Search = IndirectSalesOrder_ItemFilterDTO.Search;

            ItemFilter = ItemService.ToFilter(ItemFilter);

            return await ItemService.Count(ItemFilter);
        }

        [Route(IndirectSalesOrderRoute.ListItem), HttpPost]
        public async Task<List<IndirectSalesOrder_ItemDTO>> ListItem([FromBody] IndirectSalesOrder_ItemFilterDTO IndirectSalesOrder_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = IndirectSalesOrder_ItemFilterDTO.Skip;
            ItemFilter.Take = IndirectSalesOrder_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = IndirectSalesOrder_ItemFilterDTO.Id;
            ItemFilter.Code = IndirectSalesOrder_ItemFilterDTO.Code;
            ItemFilter.Name = IndirectSalesOrder_ItemFilterDTO.Name;
            ItemFilter.OtherName = IndirectSalesOrder_ItemFilterDTO.OtherName;
            ItemFilter.ProductGroupingId = IndirectSalesOrder_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductId = IndirectSalesOrder_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = IndirectSalesOrder_ItemFilterDTO.ProductTypeId;
            ItemFilter.RetailPrice = IndirectSalesOrder_ItemFilterDTO.RetailPrice;
            ItemFilter.SalePrice = IndirectSalesOrder_ItemFilterDTO.SalePrice;
            ItemFilter.ScanCode = IndirectSalesOrder_ItemFilterDTO.ScanCode;
            ItemFilter.CategoryId = IndirectSalesOrder_ItemFilterDTO.CategoryId;
            ItemFilter.BrandId = IndirectSalesOrder_ItemFilterDTO.BrandId;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ItemFilter.SupplierId = IndirectSalesOrder_ItemFilterDTO.SupplierId;
            ItemFilter.Search = IndirectSalesOrder_ItemFilterDTO.Search;
            ItemFilter = ItemService.ToFilter(ItemFilter);

            if (IndirectSalesOrder_ItemFilterDTO.SalesEmployeeId == null && IndirectSalesOrder_ItemFilterDTO.SalesEmployeeId.HasValue == false)
            {
                return new List<IndirectSalesOrder_ItemDTO>();
            }
            else if (IndirectSalesOrder_ItemFilterDTO.StoreId == null && IndirectSalesOrder_ItemFilterDTO.StoreId.HasValue == false)
            {
                return new List<IndirectSalesOrder_ItemDTO>();
            }
            else
            {
                List<Item> Items = await IndirectSalesOrderService.ListItem(ItemFilter, IndirectSalesOrder_ItemFilterDTO.SalesEmployeeId.Equal, IndirectSalesOrder_ItemFilterDTO.StoreId.Equal);
                List<IndirectSalesOrder_ItemDTO> IndirectSalesOrder_ItemDTOs = Items
                    .Select(x => new IndirectSalesOrder_ItemDTO(x)).ToList();
                return IndirectSalesOrder_ItemDTOs;
            }
        }
    }
}

