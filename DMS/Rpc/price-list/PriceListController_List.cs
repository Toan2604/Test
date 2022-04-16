using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public partial class PriceListController : RpcController
    {
        [Route(PriceListRoute.FilterListOrganization), HttpPost]
        public async Task<List<PriceList_OrganizationDTO>> FilterListOrganization([FromBody] PriceList_OrganizationFilterDTO PriceList_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = PriceList_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = PriceList_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = PriceList_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = PriceList_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = PriceList_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = PriceList_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = PriceList_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = PriceList_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = PriceList_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);

            List<PriceList_OrganizationDTO> PriceList_OrganizationDTOs = Organizations
                .Select(x => new PriceList_OrganizationDTO(x)).ToList();
            return PriceList_OrganizationDTOs;
        }

        [Route(PriceListRoute.FilterListItem), HttpPost]
        public async Task<List<PriceList_ItemDTO>> FilterListItem([FromBody] PriceList_ItemFilterDTO PriceList_ItemFilterDTO)
        {
            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = PriceList_ItemFilterDTO.Id;
            ItemFilter.ProductId = PriceList_ItemFilterDTO.ProductId;
            ItemFilter.Code = PriceList_ItemFilterDTO.Code;
            ItemFilter.Name = PriceList_ItemFilterDTO.Name;
            ItemFilter.ScanCode = PriceList_ItemFilterDTO.ScanCode;
            ItemFilter.SalePrice = PriceList_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = PriceList_ItemFilterDTO.RetailPrice;
            ItemFilter.Search = PriceList_ItemFilterDTO.Search;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Item> Items = await ItemService.List(ItemFilter);
            List<PriceList_ItemDTO> PriceList_ItemDTOs = Items
                .Select(x => new PriceList_ItemDTO(x)).ToList();
            return PriceList_ItemDTOs;
        }

        [Route(PriceListRoute.FilterListRequestState), HttpPost]
        public async Task<List<PriceList_RequestStateDTO>> FilterListRequestState([FromBody] PriceList_RequestStateFilterDTO PriceList_RequestStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = PriceList_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = PriceList_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = PriceList_RequestStateFilterDTO.Name;

            List<RequestState> RequestStatees = await RequestStateService.List(RequestStateFilter);
            List<PriceList_RequestStateDTO> PriceList_RequestStateDTOs = RequestStatees
                .Select(x => new PriceList_RequestStateDTO(x)).ToList();
            return PriceList_RequestStateDTOs;
        }

        [Route(PriceListRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<PriceList_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] PriceList_StoreGroupingFilterDTO PriceList_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = PriceList_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = PriceList_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = PriceList_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = PriceList_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = PriceList_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = PriceList_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<PriceList_StoreGroupingDTO> PriceList_StoreGroupingDTOs = StoreGroupings
                .Select(x => new PriceList_StoreGroupingDTO(x)).ToList();
            return PriceList_StoreGroupingDTOs;
        }
        [Route(PriceListRoute.FilterListStore), HttpPost]
        public async Task<List<PriceList_StoreDTO>> FilterListStore([FromBody] PriceList_StoreFilterDTO PriceList_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = PriceList_StoreFilterDTO.Id;
            StoreFilter.Code = PriceList_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PriceList_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PriceList_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = PriceList_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PriceList_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PriceList_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PriceList_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PriceList_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PriceList_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PriceList_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PriceList_StoreFilterDTO.WardId;
            StoreFilter.Address = PriceList_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = PriceList_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PriceList_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PriceList_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PriceList_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PriceList_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PriceList_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PriceList_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PriceList_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.WorkflowDefinitionId = PriceList_StoreFilterDTO.WorkflowDefinitionId;
            StoreFilter.StoreStatusId = PriceList_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PriceList_StoreDTO> PriceList_StoreDTOs = Stores
                .Select(x => new PriceList_StoreDTO(x)).ToList();
            return PriceList_StoreDTOs;
        }
        [Route(PriceListRoute.FilterListStoreType), HttpPost]
        public async Task<List<PriceList_StoreTypeDTO>> FilterListStoreType([FromBody] PriceList_StoreTypeFilterDTO PriceList_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = PriceList_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = PriceList_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = PriceList_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<PriceList_StoreTypeDTO> PriceList_StoreTypeDTOs = StoreTypes
                .Select(x => new PriceList_StoreTypeDTO(x)).ToList();
            return PriceList_StoreTypeDTOs;
        }

        [Route(PriceListRoute.FilterListPriceListType), HttpPost]
        public async Task<List<PriceList_PriceListTypeDTO>> FilterListPriceListType([FromBody] PriceList_PriceListTypeFilterDTO PriceList_PriceListTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PriceListTypeFilter PriceListTypeFilter = new PriceListTypeFilter();
            PriceListTypeFilter.Skip = 0;
            PriceListTypeFilter.Take = int.MaxValue;
            PriceListTypeFilter.Take = 20;
            PriceListTypeFilter.OrderBy = PriceListTypeOrder.Id;
            PriceListTypeFilter.OrderType = OrderType.ASC;
            PriceListTypeFilter.Selects = PriceListTypeSelect.ALL;

            List<PriceListType> PriceListTypes = await PriceListTypeService.List(PriceListTypeFilter);
            List<PriceList_PriceListTypeDTO> PriceList_PriceListTypeDTOs = PriceListTypes
                .Select(x => new PriceList_PriceListTypeDTO(x)).ToList();
            return PriceList_PriceListTypeDTOs;
        }
        [Route(PriceListRoute.FilterListSalesOrderType), HttpPost]
        public async Task<List<PriceList_SalesOrderTypeDTO>> FilterListSalesOrderType([FromBody] PriceList_SalesOrderTypeFilterDTO PriceList_SalesOrderTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SalesOrderTypeFilter SalesOrderTypeFilter = new SalesOrderTypeFilter();
            SalesOrderTypeFilter.Skip = 0;
            SalesOrderTypeFilter.Take = 20;
            SalesOrderTypeFilter.OrderBy = SalesOrderTypeOrder.Id;
            SalesOrderTypeFilter.OrderType = OrderType.ASC;
            SalesOrderTypeFilter.Selects = SalesOrderTypeSelect.ALL;
            SalesOrderTypeFilter.Id = PriceList_SalesOrderTypeFilterDTO.Id;
            SalesOrderTypeFilter.Code = PriceList_SalesOrderTypeFilterDTO.Code;
            SalesOrderTypeFilter.Name = PriceList_SalesOrderTypeFilterDTO.Name;

            List<SalesOrderType> SalesOrderTypes = await SalesOrderTypeService.List(SalesOrderTypeFilter);
            List<PriceList_SalesOrderTypeDTO> PriceList_SalesOrderTypeDTOs = SalesOrderTypes
                .Select(x => new PriceList_SalesOrderTypeDTO(x)).ToList();
            return PriceList_SalesOrderTypeDTOs;
        }
        [Route(PriceListRoute.FilterListStatus), HttpPost]
        public async Task<List<PriceList_StatusDTO>> FilterListStatus()
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
            List<PriceList_StatusDTO> PriceList_StatusDTOs = Statuses
                .Select(x => new PriceList_StatusDTO(x)).ToList();
            return PriceList_StatusDTOs;
        }

        [Route(PriceListRoute.SingleListAppUser), HttpPost]
        public async Task<List<PriceList_AppUserDTO>> SingleListAppUser([FromBody] PriceList_AppUserFilterDTO PriceList_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = PriceList_AppUserFilterDTO.Id;
            AppUserFilter.Username = PriceList_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = PriceList_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = PriceList_AppUserFilterDTO.Address;
            AppUserFilter.Email = PriceList_AppUserFilterDTO.Email;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<PriceList_AppUserDTO> PriceList_AppUserDTOs = AppUsers
                .Select(x => new PriceList_AppUserDTO(x)).ToList();
            return PriceList_AppUserDTOs;
        }

        [Route(PriceListRoute.SingleListOrganization), HttpPost]
        public async Task<List<PriceList_OrganizationDTO>> SingleListOrganization([FromBody] PriceList_OrganizationFilterDTO PriceList_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = PriceList_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = PriceList_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = PriceList_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = PriceList_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = PriceList_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = PriceList_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = PriceList_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = PriceList_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = PriceList_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = PriceList_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);

            List<PriceList_OrganizationDTO> PriceList_OrganizationDTOs = Organizations
                .Select(x => new PriceList_OrganizationDTO(x)).ToList();
            return PriceList_OrganizationDTOs;
        }

        [Route(PriceListRoute.SingleListItem), HttpPost]
        public async Task<List<PriceList_ItemDTO>> SingleListItem([FromBody] PriceList_ItemFilterDTO PriceList_ItemFilterDTO)
        {
            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = PriceList_ItemFilterDTO.Id;
            ItemFilter.ProductId = PriceList_ItemFilterDTO.ProductId;
            ItemFilter.Code = PriceList_ItemFilterDTO.Code;
            ItemFilter.Name = PriceList_ItemFilterDTO.Name;
            ItemFilter.ScanCode = PriceList_ItemFilterDTO.ScanCode;
            ItemFilter.SalePrice = PriceList_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = PriceList_ItemFilterDTO.RetailPrice;
            ItemFilter.StatusId = PriceList_ItemFilterDTO.StatusId;
            ItemFilter.Search = PriceList_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<PriceList_ItemDTO> PriceList_ItemDTOs = Items
                .Select(x => new PriceList_ItemDTO(x)).ToList();
            return PriceList_ItemDTOs;
        }
        [Route(PriceListRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<PriceList_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] PriceList_StoreGroupingFilterDTO PriceList_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = PriceList_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = PriceList_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = PriceList_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = PriceList_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = PriceList_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = PriceList_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = PriceList_StoreGroupingFilterDTO.StatusId;

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<PriceList_StoreGroupingDTO> PriceList_StoreGroupingDTOs = StoreGroupings
                .Select(x => new PriceList_StoreGroupingDTO(x)).ToList();
            return PriceList_StoreGroupingDTOs;
        }
        [Route(PriceListRoute.SingleListStore), HttpPost]
        public async Task<List<PriceList_StoreDTO>> SingleListStore([FromBody] PriceList_StoreFilterDTO PriceList_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = PriceList_StoreFilterDTO.Id;
            StoreFilter.Code = PriceList_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PriceList_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PriceList_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = PriceList_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PriceList_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PriceList_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PriceList_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PriceList_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PriceList_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PriceList_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PriceList_StoreFilterDTO.WardId;
            StoreFilter.Address = PriceList_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = PriceList_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PriceList_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PriceList_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PriceList_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PriceList_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PriceList_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PriceList_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PriceList_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = PriceList_StoreFilterDTO.StatusId;
            StoreFilter.WorkflowDefinitionId = PriceList_StoreFilterDTO.WorkflowDefinitionId;
            StoreFilter.StoreStatusId = PriceList_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PriceList_StoreDTO> PriceList_StoreDTOs = Stores
                .Select(x => new PriceList_StoreDTO(x)).ToList();
            return PriceList_StoreDTOs;
        }
        [Route(PriceListRoute.SingleListStoreType), HttpPost]
        public async Task<List<PriceList_StoreTypeDTO>> SingleListStoreType([FromBody] PriceList_StoreTypeFilterDTO PriceList_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = PriceList_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = PriceList_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = PriceList_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = PriceList_StoreTypeFilterDTO.StatusId;

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<PriceList_StoreTypeDTO> PriceList_StoreTypeDTOs = StoreTypes
                .Select(x => new PriceList_StoreTypeDTO(x)).ToList();
            return PriceList_StoreTypeDTOs;
        }

        [Route(PriceListRoute.SingleListPriceListType), HttpPost]
        public async Task<List<PriceList_PriceListTypeDTO>> SingleListPriceListType([FromBody] PriceList_PriceListTypeFilterDTO PriceList_PriceListTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PriceListTypeFilter PriceListTypeFilter = new PriceListTypeFilter();
            PriceListTypeFilter.Skip = 0;
            PriceListTypeFilter.Take = int.MaxValue;
            PriceListTypeFilter.Take = 20;
            PriceListTypeFilter.OrderBy = PriceListTypeOrder.Id;
            PriceListTypeFilter.OrderType = OrderType.ASC;
            PriceListTypeFilter.Selects = PriceListTypeSelect.ALL;

            List<PriceListType> PriceListTypes = await PriceListTypeService.List(PriceListTypeFilter);
            List<PriceList_PriceListTypeDTO> PriceList_PriceListTypeDTOs = PriceListTypes
                .Select(x => new PriceList_PriceListTypeDTO(x)).ToList();
            return PriceList_PriceListTypeDTOs;
        }
        [Route(PriceListRoute.SingleListSalesOrderType), HttpPost]
        public async Task<List<PriceList_SalesOrderTypeDTO>> SingleListSalesOrderType([FromBody] PriceList_SalesOrderTypeFilterDTO PriceList_SalesOrderTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            SalesOrderTypeFilter SalesOrderTypeFilter = new SalesOrderTypeFilter();
            SalesOrderTypeFilter.Skip = 0;
            SalesOrderTypeFilter.Take = 20;
            SalesOrderTypeFilter.OrderBy = SalesOrderTypeOrder.Id;
            SalesOrderTypeFilter.OrderType = OrderType.ASC;
            SalesOrderTypeFilter.Selects = SalesOrderTypeSelect.ALL;
            SalesOrderTypeFilter.Id = PriceList_SalesOrderTypeFilterDTO.Id;
            SalesOrderTypeFilter.Code = PriceList_SalesOrderTypeFilterDTO.Code;
            SalesOrderTypeFilter.Name = PriceList_SalesOrderTypeFilterDTO.Name;

            List<SalesOrderType> SalesOrderTypes = await SalesOrderTypeService.List(SalesOrderTypeFilter);
            List<PriceList_SalesOrderTypeDTO> PriceList_SalesOrderTypeDTOs = SalesOrderTypes
                .Select(x => new PriceList_SalesOrderTypeDTO(x)).ToList();
            return PriceList_SalesOrderTypeDTOs;
        }
        [Route(PriceListRoute.SingleListStatus), HttpPost]
        public async Task<List<PriceList_StatusDTO>> SingleListStatus()
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
            List<PriceList_StatusDTO> PriceList_StatusDTOs = Statuses
                .Select(x => new PriceList_StatusDTO(x)).ToList();
            return PriceList_StatusDTOs;
        }

        [Route(PriceListRoute.SingleListProvince), HttpPost]
        public async Task<List<PriceList_ProvinceDTO>> SingleListProvince([FromBody] PriceList_ProvinceFilterDTO PriceList_ProvinceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = PriceList_ProvinceFilterDTO.Skip;
            ProvinceFilter.Take = PriceList_ProvinceFilterDTO.Take;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = PriceList_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = PriceList_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<PriceList_ProvinceDTO> PriceList_ProvinceDTOs = Provinces
                .Select(x => new PriceList_ProvinceDTO(x)).ToList();
            return PriceList_ProvinceDTOs;
        }

        [Route(PriceListRoute.SingleListProductType), HttpPost]
        public async Task<List<PriceList_ProductTypeDTO>> SingleListProductType([FromBody] PriceList_ProductTypeFilterDTO PriceList_ProductTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = PriceList_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = PriceList_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = PriceList_ProductTypeFilterDTO.Name;
            ProductTypeFilter.Description = PriceList_ProductTypeFilterDTO.Description;
            ProductTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (ProductTypeFilter.Id == null) ProductTypeFilter.Id = new IdFilter();
            ProductTypeFilter.Id.In = await FilterProductType(ProductTypeService, CurrentContext);
            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<PriceList_ProductTypeDTO> PriceList_ProductTypeDTOs = ProductTypes
                .Select(x => new PriceList_ProductTypeDTO(x)).ToList();
            return PriceList_ProductTypeDTOs;
        }

        [Route(PriceListRoute.SingleListProductGrouping), HttpPost]
        public async Task<List<PriceList_ProductGroupingDTO>> SingleListProductGrouping([FromBody] PriceList_ProductGroupingFilterDTO PriceList_ProductGroupingFilterDTO)
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

            if (ProductGroupingFilter.Id == null) ProductGroupingFilter.Id = new IdFilter();
            ProductGroupingFilter.Id.In = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<PriceList_ProductGroupingDTO> PriceList_ProductGroupingDTOs = ProductGroupings
                .Select(x => new PriceList_ProductGroupingDTO(x)).ToList();
            return PriceList_ProductGroupingDTOs;
        }

        [Route(PriceListRoute.SingleListRequestState), HttpPost]
        public async Task<List<PriceList_RequestStateDTO>> SingleListRequestState([FromBody] PriceList_RequestStateFilterDTO PriceList_RequestStateFilterDTO)
        {
            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = PriceList_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = PriceList_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = PriceList_RequestStateFilterDTO.Name;

            List<RequestState> RequestStates = await RequestStateService.List(RequestStateFilter);
            List<PriceList_RequestStateDTO> PriceList_RequestStateDTOs = RequestStates
                .Select(x => new PriceList_RequestStateDTO(x)).ToList();
            return PriceList_RequestStateDTOs;
        }

        [Route(PriceListRoute.CountItem), HttpPost]
        public async Task<long> CountItem([FromBody] PriceList_ItemFilterDTO PriceList_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Id = PriceList_ItemFilterDTO.Id;
            ItemFilter.ProductId = PriceList_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = PriceList_ItemFilterDTO.ProductTypeId;
            ItemFilter.ProductGroupingId = PriceList_ItemFilterDTO.ProductGroupingId;
            ItemFilter.Code = PriceList_ItemFilterDTO.Code;
            ItemFilter.Name = PriceList_ItemFilterDTO.Name;
            ItemFilter.ScanCode = PriceList_ItemFilterDTO.ScanCode;
            ItemFilter.OtherName = PriceList_ItemFilterDTO.OtherName;
            ItemFilter.SalePrice = PriceList_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = PriceList_ItemFilterDTO.RetailPrice;
            ItemFilter.Search = PriceList_ItemFilterDTO.Search;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await ItemService.Count(ItemFilter);
        }

        [Route(PriceListRoute.ListItem), HttpPost]
        public async Task<List<PriceList_ItemDTO>> ListItem([FromBody] PriceList_ItemFilterDTO PriceList_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = PriceList_ItemFilterDTO.Skip;
            ItemFilter.Take = PriceList_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = PriceList_ItemFilterDTO.Id;
            ItemFilter.ProductId = PriceList_ItemFilterDTO.ProductId;
            ItemFilter.ProductTypeId = PriceList_ItemFilterDTO.ProductTypeId;
            ItemFilter.ProductGroupingId = PriceList_ItemFilterDTO.ProductGroupingId;
            ItemFilter.Code = PriceList_ItemFilterDTO.Code;
            ItemFilter.Name = PriceList_ItemFilterDTO.Name;
            ItemFilter.ScanCode = PriceList_ItemFilterDTO.ScanCode;
            ItemFilter.OtherName = PriceList_ItemFilterDTO.OtherName;
            ItemFilter.SalePrice = PriceList_ItemFilterDTO.SalePrice;
            ItemFilter.RetailPrice = PriceList_ItemFilterDTO.RetailPrice;
            ItemFilter.Search = PriceList_ItemFilterDTO.Search;
            ItemFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Item> Items = await ItemService.List(ItemFilter);
            List<PriceList_ItemDTO> PriceList_ItemDTOs = Items
                .Select(x => new PriceList_ItemDTO(x)).ToList();
            return PriceList_ItemDTOs;
        }

        [Route(PriceListRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] PriceList_StoreFilterDTO PriceList_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = PriceList_StoreFilterDTO.Id;
            StoreFilter.Code = PriceList_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PriceList_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PriceList_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = PriceList_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PriceList_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PriceList_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PriceList_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PriceList_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PriceList_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PriceList_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PriceList_StoreFilterDTO.WardId;
            StoreFilter.Address = PriceList_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = PriceList_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PriceList_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PriceList_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PriceList_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PriceList_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PriceList_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PriceList_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PriceList_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.WorkflowDefinitionId = PriceList_StoreFilterDTO.WorkflowDefinitionId;
            StoreFilter.StoreStatusId = PriceList_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            return await StoreService.Count(StoreFilter);
        }

        [Route(PriceListRoute.ListStore), HttpPost]
        public async Task<List<PriceList_StoreDTO>> ListStore([FromBody] PriceList_StoreFilterDTO PriceList_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = PriceList_StoreFilterDTO.Skip;
            StoreFilter.Take = PriceList_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = PriceList_StoreFilterDTO.Id;
            StoreFilter.Code = PriceList_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = PriceList_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = PriceList_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = PriceList_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = PriceList_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = PriceList_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = PriceList_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = PriceList_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = PriceList_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = PriceList_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = PriceList_StoreFilterDTO.WardId;
            StoreFilter.Address = PriceList_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = PriceList_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = PriceList_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = PriceList_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = PriceList_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = PriceList_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = PriceList_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = PriceList_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = PriceList_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.WorkflowDefinitionId = PriceList_StoreFilterDTO.WorkflowDefinitionId;
            StoreFilter.StoreStatusId = PriceList_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PriceList_StoreDTO> PriceList_StoreDTOs = Stores
                .Select(x => new PriceList_StoreDTO(x)).ToList();
            return PriceList_StoreDTOs;
        }

        [Route(PriceListRoute.CountPriceListItemHistory), HttpPost]
        public async Task<long> CountPriceListItemHistory([FromBody] PriceList_PriceListItemHistoryFilterDTO PriceList_PriceListItemHistoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PriceListItemHistoryFilter PriceListItemHistoryFilter = new PriceListItemHistoryFilter
            {
                ItemId = PriceList_PriceListItemHistoryFilterDTO.ItemId,
                PriceListId = PriceList_PriceListItemHistoryFilterDTO.PriceListId,
                ModifierId = PriceList_PriceListItemHistoryFilterDTO.ModifierId,
                UpdatedAt = PriceList_PriceListItemHistoryFilterDTO.UpdatedAt,
            };

            return await PriceListItemHistoryService.Count(PriceListItemHistoryFilter);
        }

        [Route(PriceListRoute.ListPriceListItemHistory), HttpPost]
        public async Task<List<PriceList_PriceListItemHistoryDTO>> ListPriceListItemHistory([FromBody] PriceList_PriceListItemHistoryFilterDTO PriceList_PriceListItemHistoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PriceListItemHistoryFilter PriceListItemHistoryFilter = new PriceListItemHistoryFilter
            {
                Skip = PriceList_PriceListItemHistoryFilterDTO.Skip,
                Take = PriceList_PriceListItemHistoryFilterDTO.Take,
                Selects = PriceListItemHistorySelect.ALL,
                OrderBy = PriceListItemHistoryOrder.UpdatedAt,
                OrderType = OrderType.DESC,
                ItemId = PriceList_PriceListItemHistoryFilterDTO.ItemId,
                PriceListId = PriceList_PriceListItemHistoryFilterDTO.PriceListId,
                ModifierId = PriceList_PriceListItemHistoryFilterDTO.ModifierId,
                UpdatedAt = PriceList_PriceListItemHistoryFilterDTO.UpdatedAt,
            };

            List<PriceListItemHistory> PriceListItemHistories = await PriceListItemHistoryService.List(PriceListItemHistoryFilter);
            List<PriceList_PriceListItemHistoryDTO> PriceList_PriceListItemHistoryDTOs = PriceListItemHistories
                .Select(x => new PriceList_PriceListItemHistoryDTO(x))
                .ToList();
            return PriceList_PriceListItemHistoryDTOs;
        }
    }
}

