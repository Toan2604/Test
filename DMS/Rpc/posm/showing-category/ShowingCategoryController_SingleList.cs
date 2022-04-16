using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_category
{
    public partial class ShowingCategoryController : RpcController
    {
        [Route(ShowingCategoryRoute.SingleListShowingCategory), HttpPost]
        public async Task<List<ShowingCategory_ShowingCategoryDTO>> SingleListShowingCategory([FromBody] ShowingCategory_ShowingCategoryFilterDTO ShowingCategory_ShowingCategoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter.Skip = 0;
            ShowingCategoryFilter.Take = int.MaxValue;
            ShowingCategoryFilter.OrderBy = ShowingCategoryOrder.Id;
            ShowingCategoryFilter.OrderType = OrderType.ASC;
            ShowingCategoryFilter.Selects = ShowingCategorySelect.ALL;
            ShowingCategoryFilter.Id = ShowingCategory_ShowingCategoryFilterDTO.Id;
            ShowingCategoryFilter.Code = ShowingCategory_ShowingCategoryFilterDTO.Code;
            ShowingCategoryFilter.Name = ShowingCategory_ShowingCategoryFilterDTO.Name;
            ShowingCategoryFilter.ParentId = ShowingCategory_ShowingCategoryFilterDTO.ParentId;
            ShowingCategoryFilter.Path = ShowingCategory_ShowingCategoryFilterDTO.Path;
            ShowingCategoryFilter.Level = ShowingCategory_ShowingCategoryFilterDTO.Level;
            ShowingCategoryFilter.StatusId = ShowingCategory_ShowingCategoryFilterDTO.StatusId;
            ShowingCategoryFilter.ImageId = ShowingCategory_ShowingCategoryFilterDTO.ImageId;
            ShowingCategoryFilter.RowId = ShowingCategory_ShowingCategoryFilterDTO.RowId;
            ShowingCategoryFilter.StatusId = new IdFilter { Equal = 1 };
            List<ShowingCategory> ShowingCategorys = await ShowingCategoryService.List(ShowingCategoryFilter);
            List<ShowingCategory_ShowingCategoryDTO> ShowingCategory_ShowingCategoryDTOs = ShowingCategorys
                .Select(x => new ShowingCategory_ShowingCategoryDTO(x)).ToList();
            return ShowingCategory_ShowingCategoryDTOs;
        }
        [Route(ShowingCategoryRoute.SingleListStatus), HttpPost]
        public async Task<List<ShowingCategory_StatusDTO>> SingleListStatus([FromBody] ShowingCategory_StatusFilterDTO ShowingCategory_StatusFilterDTO)
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
            List<ShowingCategory_StatusDTO> ShowingCategory_StatusDTOs = Statuses
                .Select(x => new ShowingCategory_StatusDTO(x)).ToList();
            return ShowingCategory_StatusDTOs;
        }
    }
}

