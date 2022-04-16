using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Services.MBanner;
using DMS.ABE.Services.MCategory;
using DMS.ABE.Services.MProduct;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc.banner
{
    public class BannerController : SimpleController
    {
        private IBannerService BannerService;
        private ICategoryService CategoryService;
        private IItemService ItemService;
        private IProductService ProductService;
        public BannerController(
            IBannerService BannerService,
            ICategoryService CategoryService,
            IItemService ItemService,
            IProductService ProductService
            )
        {
            this.BannerService = BannerService;
            this.CategoryService = CategoryService;
            this.ItemService = ItemService;
            this.ProductService = ProductService;
        }

        [Route(BannerRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter.Selects = BannerSelect.ALL;
            BannerFilter.Skip = Banner_BannerFilterDTO.Skip;
            BannerFilter.Take = Banner_BannerFilterDTO.Take;
            BannerFilter.OrderBy = Banner_BannerFilterDTO.OrderBy;
            BannerFilter.OrderType = Banner_BannerFilterDTO.OrderType;
            BannerFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            int count = await BannerService.Count(BannerFilter);
            return count;
        }

        [Route(BannerRoute.List), HttpPost]
        public async Task<ActionResult<List<Banner_BannerDTO>>> List([FromBody] Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter.Selects = BannerSelect.ALL;
            BannerFilter.Skip = Banner_BannerFilterDTO.Skip;
            BannerFilter.Take = Banner_BannerFilterDTO.Take;
            BannerFilter.OrderBy = BannerOrder.Priority;
            BannerFilter.OrderType = OrderType.ASC;
            BannerFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Banner> Banners = await BannerService.List(BannerFilter);
            List<Banner_BannerDTO> Banner_BannerDTOs = Banners
                .Select(c => new Banner_BannerDTO(c)).ToList();
            return Banner_BannerDTOs;
        }

        [Route(BannerRoute.Get), HttpPost]
        public async Task<ActionResult<Banner_BannerDTO>> Get([FromBody] Banner_BannerDTO Banner_BannerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Banner Banner = await BannerService.Get(Banner_BannerDTO.Id);
            return new Banner_BannerDTO(Banner);
        }
    }
}
