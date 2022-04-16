using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public partial class KpiProductGroupingController : RpcController
    {
        [Route(KpiProductGroupingRoute.CountAppUser), HttpPost]
        public async Task<int> CountAppUser([FromBody] KpiProductGrouping_AppUserFilterDTO KpiProductGrouping_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Id = KpiProductGrouping_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiProductGrouping_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiProductGrouping_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = KpiProductGrouping_AppUserFilterDTO.Email;
            AppUserFilter.Phone = KpiProductGrouping_AppUserFilterDTO.Phone;
            AppUserFilter.OrganizationId = KpiProductGrouping_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }
            return await KpiProductGroupingService.CountAppUser(AppUserFilter, KpiProductGrouping_AppUserFilterDTO.KpiYearId, KpiProductGrouping_AppUserFilterDTO.KpiPeriodId, KpiProductGrouping_AppUserFilterDTO.KpiProductGroupingTypeId); ;
        }

        [Route(KpiProductGroupingRoute.ListAppUser), HttpPost]
        public async Task<List<KpiProductGrouping_AppUserDTO>> ListAppUser([FromBody] KpiProductGrouping_AppUserFilterDTO KpiProductGrouping_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = KpiProductGrouping_AppUserFilterDTO.Skip;
            AppUserFilter.Take = KpiProductGrouping_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = KpiProductGrouping_AppUserFilterDTO.Id;
            AppUserFilter.Username = KpiProductGrouping_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiProductGrouping_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = KpiProductGrouping_AppUserFilterDTO.Email;
            AppUserFilter.OrganizationId = KpiProductGrouping_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Phone = KpiProductGrouping_AppUserFilterDTO.Phone;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }

            List<AppUser> AppUsers = await KpiProductGroupingService.ListAppUser(AppUserFilter, KpiProductGrouping_AppUserFilterDTO.KpiYearId, KpiProductGrouping_AppUserFilterDTO.KpiPeriodId, KpiProductGrouping_AppUserFilterDTO.KpiProductGroupingTypeId);
            List<KpiProductGrouping_AppUserDTO> KpiProductGrouping_AppUserDTOs = AppUsers
                .Select(x => new KpiProductGrouping_AppUserDTO(x)).ToList();
            return KpiProductGrouping_AppUserDTOs;
        }

        [Route(KpiProductGroupingRoute.ListItem), HttpPost]
        public async Task<List<KpiProductGrouping_ItemDTO>> ListItem([FromBody] KpiProductGrouping_ItemFilterDTO KpiProductGrouping_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = KpiProductGrouping_ItemFilterDTO.Skip;
            ItemFilter.Take = KpiProductGrouping_ItemFilterDTO.Take;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = KpiProductGrouping_ItemFilterDTO.Id;
            ItemFilter.Code = KpiProductGrouping_ItemFilterDTO.Code;
            ItemFilter.Name = KpiProductGrouping_ItemFilterDTO.Name;
            ItemFilter.CategoryId = KpiProductGrouping_ItemFilterDTO.CategoryId;
            ItemFilter.ProductGroupingId = KpiProductGrouping_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductTypeId = KpiProductGrouping_ItemFilterDTO.ProductTypeId;
            ItemFilter.BrandId = KpiProductGrouping_ItemFilterDTO.BrandId;
            ItemFilter.IsNew = KpiProductGrouping_ItemFilterDTO.IsNew;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<KpiProductGrouping_ItemDTO> KpiProductGrouping_ItemDTOs = Items
                .Select(x => new KpiProductGrouping_ItemDTO(x)).ToList();
            return KpiProductGrouping_ItemDTOs;
        }

        [Route(KpiProductGroupingRoute.CountItem), HttpPost]
        public async Task<int> CountItem([FromBody] KpiProductGrouping_ItemFilterDTO KpiProductGrouping_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Id = KpiProductGrouping_ItemFilterDTO.Id;
            ItemFilter.Code = KpiProductGrouping_ItemFilterDTO.Code;
            ItemFilter.Name = KpiProductGrouping_ItemFilterDTO.Name;
            ItemFilter.CategoryId = KpiProductGrouping_ItemFilterDTO.CategoryId;
            ItemFilter.ProductGroupingId = KpiProductGrouping_ItemFilterDTO.ProductGroupingId;
            ItemFilter.ProductTypeId = KpiProductGrouping_ItemFilterDTO.ProductTypeId;
            ItemFilter.BrandId = KpiProductGrouping_ItemFilterDTO.BrandId;
            ItemFilter.IsNew = KpiProductGrouping_ItemFilterDTO.IsNew;

            int Count = await ItemService.Count(ItemFilter);
            return Count;
        }
    }
}

