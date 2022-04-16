using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE.Enums;

namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public partial class WebDirectSalesOrderController : SimpleController
    {
        [Route(WebDirectSalesOrderRoute.SingleListAppUser), HttpPost]
        public async Task<List<WebDirectSalesOrder_AppUserDTO>> SingleListAppUser([FromBody] WebDirectSalesOrder_AppUserFilterDTO WebDirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = WebDirectSalesOrder_AppUserFilterDTO.Skip;
            AppUserFilter.Take = WebDirectSalesOrder_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Username = WebDirectSalesOrder_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WebDirectSalesOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WebDirectSalesOrder_AppUserFilterDTO.Address;
            AppUserFilter.Email = WebDirectSalesOrder_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WebDirectSalesOrder_AppUserFilterDTO.Phone;
            AppUserFilter.SexId = WebDirectSalesOrder_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WebDirectSalesOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.Department = WebDirectSalesOrder_AppUserFilterDTO.Department;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            AppUserFilter = await ChangeAppUserFilterByCurrentAccount(AppUserFilter);
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WebDirectSalesOrder_AppUserDTO> WebDirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new WebDirectSalesOrder_AppUserDTO(x)).ToList();
            return WebDirectSalesOrder_AppUserDTOs;
        }
        //[Route(WebDirectSalesOrderRoute.ListAppUser), HttpPost]
        //public async Task<List<WebDirectSalesOrder_AppUserDTO>> ListAppUser([FromBody] WebDirectSalesOrder_AppUserFilterDTO WebDirectSalesOrder_AppUserFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);
        //    AppUserFilter AppUserFilter = new AppUserFilter
        //    {
        //        Skip = WebDirectSalesOrder_AppUserFilterDTO.Skip,
        //        Take = WebDirectSalesOrder_AppUserFilterDTO.Take,
        //        OrderBy = AppUserOrder.Id,
        //        OrderType = OrderType.ASC,
        //        Selects = AppUserSelect.ALL,
        //        Username = WebDirectSalesOrder_AppUserFilterDTO.Username,
        //        DisplayName = WebDirectSalesOrder_AppUserFilterDTO.DisplayName,
        //        StatusId = WebDirectSalesOrder_AppUserFilterDTO.StatusId
        //    };
        //    AppUserFilter = await ChangeAppUserFilterByCurrentAccount(AppUserFilter);
        //    List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
        //    List<WebDirectSalesOrder_AppUserDTO> WebDirectSalesOrder_AppUserDTOs = AppUsers
        //        .Select(x => new WebDirectSalesOrder_AppUserDTO(x)).ToList();
        //    return WebDirectSalesOrder_AppUserDTOs;
        //}
        //[Route(WebDirectSalesOrderRoute.CountAppUser), HttpPost]
        //public async Task<int> CountAppUser([FromBody] WebDirectSalesOrder_AppUserFilterDTO WebDirectSalesOrder_AppUserFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);
        //    AppUserFilter AppUserFilter = new AppUserFilter
        //    {
        //        Skip = WebDirectSalesOrder_AppUserFilterDTO.Skip,
        //        Take = WebDirectSalesOrder_AppUserFilterDTO.Take,
        //        OrderBy = AppUserOrder.Id,
        //        OrderType = OrderType.ASC,
        //        Selects = AppUserSelect.ALL,
        //        Username = WebDirectSalesOrder_AppUserFilterDTO.Username,
        //        DisplayName = WebDirectSalesOrder_AppUserFilterDTO.DisplayName,
        //        StatusId = WebDirectSalesOrder_AppUserFilterDTO.StatusId
        //    };
        //    AppUserFilter = await ChangeAppUserFilterByCurrentAccount(AppUserFilter);
        //    int Count = await AppUserService.Count(AppUserFilter);
        //    return Count;
        //}
        //[Route(WebDirectSalesOrderRoute.ListErpApprovalState), HttpPost]
        //public async Task<List<WebDirectSalesOrder_ErpApprovalStateDTO>> ListErpApprovalState([FromBody] WebDirectSalesOrder_ErpApprovalStateFilterDTO WebDirectSalesOrder_ErpApprovalStateFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);
        //    ErpApprovalStateFilter ErpApprovalStateFilter = new ErpApprovalStateFilter();
        //    ErpApprovalStateFilter.Skip = 0;
        //    ErpApprovalStateFilter.Take = 20;
        //    ErpApprovalStateFilter.OrderBy = ErpApprovalStateOrder.Id;
        //    ErpApprovalStateFilter.OrderType = OrderType.ASC;
        //    ErpApprovalStateFilter.Selects = ErpApprovalStateSelect.ALL;
        //    ErpApprovalStateFilter.Id = WebDirectSalesOrder_ErpApprovalStateFilterDTO.Id;
        //    ErpApprovalStateFilter.Code = WebDirectSalesOrder_ErpApprovalStateFilterDTO.Code;
        //    ErpApprovalStateFilter.Name = WebDirectSalesOrder_ErpApprovalStateFilterDTO.Name;
        //    List<ErpApprovalState> ErpApprovalStates = await ErpApprovalStateService.List(ErpApprovalStateFilter);
        //    List<WebDirectSalesOrder_ErpApprovalStateDTO> WebDirectSalesOrder_ErpApprovalStateDTOs = ErpApprovalStates
        //        .Select(x => new WebDirectSalesOrder_ErpApprovalStateDTO(x)).ToList();
        //    return WebDirectSalesOrder_ErpApprovalStateDTOs;
        //}
        [Route(WebDirectSalesOrderRoute.FilterListStoreApprovalState), HttpPost]
        public async Task<List<WebDirectSalesOrder_StoreApprovalStateDTO>> FilterListStoreApprovalState([FromBody] WebDirectSalesOrder_StoreApprovalStateFilterDTO WebDirectSalesOrder_StoreApprovalStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreApprovalStateFilter StoreApprovalStateFilter = new StoreApprovalStateFilter();
            StoreApprovalStateFilter.Skip = 0;
            StoreApprovalStateFilter.Take = int.MaxValue;
            StoreApprovalStateFilter.OrderBy = StoreApprovalStateOrder.Id;
            StoreApprovalStateFilter.OrderType = OrderType.ASC;
            StoreApprovalStateFilter.Selects = StoreApprovalStateSelect.ALL;
            StoreApprovalStateFilter.Id = WebDirectSalesOrder_StoreApprovalStateFilterDTO.Id;
            StoreApprovalStateFilter.Code = WebDirectSalesOrder_StoreApprovalStateFilterDTO.Code;
            StoreApprovalStateFilter.Name = WebDirectSalesOrder_StoreApprovalStateFilterDTO.Name;
            List<StoreApprovalState> StoreApprovalStates = await StoreApprovalStateService.List(StoreApprovalStateFilter);
            List<WebDirectSalesOrder_StoreApprovalStateDTO> WebDirectSalesOrder_StoreApprovalStateDTOs = StoreApprovalStates
                .Select(x => new WebDirectSalesOrder_StoreApprovalStateDTO(x)).ToList();
            return WebDirectSalesOrder_StoreApprovalStateDTOs;
        }
        [Route(WebDirectSalesOrderRoute.FilterListGeneralApprovalState), HttpPost]
        public async Task<List<WebDirectSalesOrder_GeneralApprovalStateDTO>> FilterListGeneralApprovalState([FromBody] WebDirectSalesOrder_GeneralApprovalStateFilterDTO WebDirectSalesOrder_GeneralApprovalStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            GeneralApprovalStateFilter GeneralApprovalStateFilter = new GeneralApprovalStateFilter();
            GeneralApprovalStateFilter.Skip = 0;
            GeneralApprovalStateFilter.Take = int.MaxValue;
            GeneralApprovalStateFilter.OrderBy = GeneralApprovalStateOrder.Id;
            GeneralApprovalStateFilter.OrderType = OrderType.ASC;
            GeneralApprovalStateFilter.Selects = GeneralApprovalStateSelect.ALL;
            GeneralApprovalStateFilter.Id = new IdFilter { NotEqual = GeneralApprovalStateEnum.NEW.Id };
            GeneralApprovalStateFilter.Code = WebDirectSalesOrder_GeneralApprovalStateFilterDTO.Code;
            GeneralApprovalStateFilter.Name = WebDirectSalesOrder_GeneralApprovalStateFilterDTO.Name;
            List<GeneralApprovalState> GeneralApprovalStates = await GeneralApprovalStateService.List(GeneralApprovalStateFilter);
            List<WebDirectSalesOrder_GeneralApprovalStateDTO> WebDirectSalesOrder_GeneralApprovalStateDTOs = GeneralApprovalStates
                .Select(x => new WebDirectSalesOrder_GeneralApprovalStateDTO(x)).ToList();
            return WebDirectSalesOrder_GeneralApprovalStateDTOs;
        }
        [Route(WebDirectSalesOrderRoute.ListItem), HttpPost]
        public async Task<List<WebDirectSalesOrder_ItemDTO>> ListItem([FromBody] WebDirectSalesOrder_ItemFilterDTO WebDirectSalesOrder_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ItemFilter ItemFilter = new ItemFilter
            {
                Skip = WebDirectSalesOrder_ItemFilterDTO.Skip,
                Take = WebDirectSalesOrder_ItemFilterDTO.Take,
                OrderBy = ItemOrder.Id,
                OrderType = OrderType.DESC,
                Selects = ItemSelect.ALL,
                Id = WebDirectSalesOrder_ItemFilterDTO.Id,
                Code = WebDirectSalesOrder_ItemFilterDTO.Code,
                Name = WebDirectSalesOrder_ItemFilterDTO.Name,
                CategoryId = WebDirectSalesOrder_ItemFilterDTO.CategoryId,
                OtherName = WebDirectSalesOrder_ItemFilterDTO.OtherName,
                ProductGroupingId = WebDirectSalesOrder_ItemFilterDTO.ProductGroupingId,
                ProductId = WebDirectSalesOrder_ItemFilterDTO.ProductId,
                ProductTypeId = WebDirectSalesOrder_ItemFilterDTO.ProductTypeId,
                RetailPrice = WebDirectSalesOrder_ItemFilterDTO.RetailPrice,
                SalePrice = WebDirectSalesOrder_ItemFilterDTO.SalePrice,
                ScanCode = WebDirectSalesOrder_ItemFilterDTO.ScanCode,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                SupplierId = WebDirectSalesOrder_ItemFilterDTO.SupplierId,
                Search = WebDirectSalesOrder_ItemFilterDTO.Search,
            };
            List<Item> Items = await ItemService.ListByStore(ItemFilter);
            if (Items == null)
                return null;
            List<WebDirectSalesOrder_ItemDTO> WebDirectSalesOrder_ItemDTOs = Items.Select(x => new WebDirectSalesOrder_ItemDTO(x)).ToList();
            return WebDirectSalesOrder_ItemDTOs;
        }
        [Route(WebDirectSalesOrderRoute.CountItem), HttpPost]
        public async Task<long> CountItem([FromBody] WebDirectSalesOrder_ItemFilterDTO WebDirectSalesOrder_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ItemFilter ItemFilter = new ItemFilter
            {
                Skip = WebDirectSalesOrder_ItemFilterDTO.Skip,
                Take = WebDirectSalesOrder_ItemFilterDTO.Take,
                OrderBy = ItemOrder.Id,
                OrderType = OrderType.DESC,
                Selects = ItemSelect.ALL,
                Id = WebDirectSalesOrder_ItemFilterDTO.Id,
                Code = WebDirectSalesOrder_ItemFilterDTO.Code,
                Name = WebDirectSalesOrder_ItemFilterDTO.Name,
                CategoryId = WebDirectSalesOrder_ItemFilterDTO.CategoryId,
                OtherName = WebDirectSalesOrder_ItemFilterDTO.OtherName,
                ProductGroupingId = WebDirectSalesOrder_ItemFilterDTO.ProductGroupingId,
                ProductId = WebDirectSalesOrder_ItemFilterDTO.ProductId,
                ProductTypeId = WebDirectSalesOrder_ItemFilterDTO.ProductTypeId,
                RetailPrice = WebDirectSalesOrder_ItemFilterDTO.RetailPrice,
                SalePrice = WebDirectSalesOrder_ItemFilterDTO.SalePrice,
                ScanCode = WebDirectSalesOrder_ItemFilterDTO.ScanCode,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                SupplierId = WebDirectSalesOrder_ItemFilterDTO.SupplierId,
                Search = WebDirectSalesOrder_ItemFilterDTO.Search,
            };
            long count = await ItemService.CountByStore(ItemFilter);
            return count;
        }
        [Route(WebDirectSalesOrderRoute.SingleListUnitOfMeasure), HttpPost]
        public async Task<List<WebDirectSalesOrder_UnitOfMeasureDTO>> SingleListUnitOfMeasure([FromBody] WebDirectSalesOrder_UnitOfMeasureFilterDTO WebDirectSalesOrder_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            //TODO 
            List<Product> Products = await ProductService.List(new ProductFilter
            {
                Id = WebDirectSalesOrder_UnitOfMeasureFilterDTO.ProductId,
                Selects = ProductSelect.Id,
            });
            long ProductId = Products.Select(p => p.Id).FirstOrDefault();
            Product Product = await ProductService.Get(ProductId);
            List<WebDirectSalesOrder_UnitOfMeasureDTO> WebDirectSalesOrder_UnitOfMeasureDTOs = new List<WebDirectSalesOrder_UnitOfMeasureDTO>();
            if (Product.UnitOfMeasureGrouping != null)
            {
                WebDirectSalesOrder_UnitOfMeasureDTOs = Product.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents.Select(x => new WebDirectSalesOrder_UnitOfMeasureDTO(x)).ToList();
            }
            WebDirectSalesOrder_UnitOfMeasureDTO WebDirectSalesOrder_UnitOfMeasureDTO = new WebDirectSalesOrder_UnitOfMeasureDTO
            {
                Id = Product.UnitOfMeasure.Id,
                Code = Product.UnitOfMeasure.Code,
                Name = Product.UnitOfMeasure.Name,
                Description = Product.UnitOfMeasure.Description,
                StatusId = StatusEnum.ACTIVE.Id,
                Factor = 1,
            };
            WebDirectSalesOrder_UnitOfMeasureDTOs.Add(WebDirectSalesOrder_UnitOfMeasureDTO);
            WebDirectSalesOrder_UnitOfMeasureDTOs = WebDirectSalesOrder_UnitOfMeasureDTOs.Distinct().ToList();
            return WebDirectSalesOrder_UnitOfMeasureDTOs;
        }
        [Route(WebDirectSalesOrderRoute.FilterListCategory), HttpPost]
        public async Task<List<WebDirectSalesOrder_CategoryDTO>> FilterListCategory([FromBody] WebDirectSalesOrder_CategoryFilterDTO WebDirectSalesOrder_CategoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            CategoryFilter CategoryFilter = new CategoryFilter();
            CategoryFilter.Skip = 0;
            CategoryFilter.Take = int.MaxValue;
            CategoryFilter.OrderBy = CategoryOrder.Id;
            CategoryFilter.OrderType = OrderType.ASC;
            CategoryFilter.Selects = CategorySelect.ALL;
            CategoryFilter.Id = WebDirectSalesOrder_CategoryFilterDTO.Id;
            CategoryFilter.Code = WebDirectSalesOrder_CategoryFilterDTO.Code;
            CategoryFilter.Name = WebDirectSalesOrder_CategoryFilterDTO.Name;
            CategoryFilter.ParentId = WebDirectSalesOrder_CategoryFilterDTO.ParentId;
            CategoryFilter.Path = WebDirectSalesOrder_CategoryFilterDTO.Path;
            CategoryFilter.Level = WebDirectSalesOrder_CategoryFilterDTO.Level;
            CategoryFilter.StatusId = WebDirectSalesOrder_CategoryFilterDTO.StatusId;
            CategoryFilter.ImageId = WebDirectSalesOrder_CategoryFilterDTO.ImageId;
            CategoryFilter.RowId = WebDirectSalesOrder_CategoryFilterDTO.RowId;

            List<Category> Categories = await CategoryService.List(CategoryFilter);
            List<WebDirectSalesOrder_CategoryDTO> WebDirectSalesOrder_CategoryDTOs = Categories
                .Select(x => new WebDirectSalesOrder_CategoryDTO(x)).ToList();
            return WebDirectSalesOrder_CategoryDTOs;
        }
        private async Task<AppUserFilter> ChangeAppUserFilterByCurrentAccount(AppUserFilter AppUserFilter)
        {
            long StoreUserId = CurrentContext.StoreUserId;
            List<StoreUser> StoreUsers = await StoreUserService.List(new StoreUserFilter
            {
                Id = new IdFilter { Equal = StoreUserId },
                Selects = StoreUserSelect.Id | StoreUserSelect.Store
            });
            StoreUser StoreUser = StoreUsers.FirstOrDefault();
            List<Store> Stores = await StoreService.List(new StoreFilter
            {
                Id = new IdFilter { Equal = StoreUser.Store.Id },
                Selects = StoreSelect.Id | StoreSelect.StoreAppUserMappings | StoreSelect.Organization
            }); // lay ra pham vi di tuyen + org cua cua hang
            Store Store = Stores.FirstOrDefault();
            if (Store.StoreAppUserMappings.Count == 0)
            {
                AppUserFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
            } // neu khong co pham vi di tuyen, filter AU theo Id OU
            else
            {
                List<long> AppUserIds = Store.StoreAppUserMappings.Select(x => x.AppUserId).ToList();
                AppUserFilter.Id = new IdFilter
                {
                    In = AppUserIds
                };
            } // neu co pham vi di tuyen, lay toan bo appUser theo pham vi di tuyen
            return AppUserFilter;
        }
    }
}
