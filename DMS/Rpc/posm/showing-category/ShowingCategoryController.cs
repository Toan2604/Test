using DMS.Common;
using DMS.Entities;
using DMS.Services.MCategory;
using DMS.Services.MImage;
using DMS.Services.MShowingCategory;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_category
{
    public partial class ShowingCategoryController : RpcController
    {
        private IImageService ImageService;
        private ICategoryService CategoryService;
        private IStatusService StatusService;
        private IShowingCategoryService ShowingCategoryService;
        private ICurrentContext CurrentContext;
        public ShowingCategoryController(
            IImageService ImageService,
            ICategoryService CategoryService,
            IStatusService StatusService,
            IShowingCategoryService ShowingCategoryService,
            ICurrentContext CurrentContext
        )
        {
            this.ImageService = ImageService;
            this.CategoryService = CategoryService;
            this.StatusService = StatusService;
            this.ShowingCategoryService = ShowingCategoryService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ShowingCategoryRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ShowingCategory_ShowingCategoryFilterDTO ShowingCategory_ShowingCategoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingCategoryFilter ShowingCategoryFilter = ConvertFilterDTOToFilterEntity(ShowingCategory_ShowingCategoryFilterDTO);
            ShowingCategoryFilter = await ShowingCategoryService.ToFilter(ShowingCategoryFilter);
            int count = await ShowingCategoryService.Count(ShowingCategoryFilter);
            return count;
        }

        [Route(ShowingCategoryRoute.List), HttpPost]
        public async Task<ActionResult<List<ShowingCategory_ShowingCategoryDTO>>> List([FromBody] ShowingCategory_ShowingCategoryFilterDTO ShowingCategory_ShowingCategoryFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingCategoryFilter ShowingCategoryFilter = ConvertFilterDTOToFilterEntity(ShowingCategory_ShowingCategoryFilterDTO);
            ShowingCategoryFilter = await ShowingCategoryService.ToFilter(ShowingCategoryFilter);
            List<ShowingCategory> ShowingCategories = await ShowingCategoryService.List(ShowingCategoryFilter);
            List<ShowingCategory_ShowingCategoryDTO> ShowingCategory_ShowingCategoryDTOs = ShowingCategories
                .Select(c => new ShowingCategory_ShowingCategoryDTO(c)).ToList();
            return ShowingCategory_ShowingCategoryDTOs;
        }

        [Route(ShowingCategoryRoute.Get), HttpPost]
        public async Task<ActionResult<ShowingCategory_ShowingCategoryDTO>> Get([FromBody] ShowingCategory_ShowingCategoryDTO ShowingCategory_ShowingCategoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingCategory_ShowingCategoryDTO.Id))
                return Forbid();

            ShowingCategory ShowingCategory = await ShowingCategoryService.Get(ShowingCategory_ShowingCategoryDTO.Id);
            return new ShowingCategory_ShowingCategoryDTO(ShowingCategory);
        }

        [Route(ShowingCategoryRoute.Create), HttpPost]
        public async Task<ActionResult<ShowingCategory_ShowingCategoryDTO>> Create([FromBody] ShowingCategory_ShowingCategoryDTO ShowingCategory_ShowingCategoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingCategory_ShowingCategoryDTO.Id))
                return Forbid();

            ShowingCategory ShowingCategory = ConvertDTOToEntity(ShowingCategory_ShowingCategoryDTO);
            ShowingCategory = await ShowingCategoryService.Create(ShowingCategory);
            ShowingCategory_ShowingCategoryDTO = new ShowingCategory_ShowingCategoryDTO(ShowingCategory);
            if (ShowingCategory.IsValidated)
                return ShowingCategory_ShowingCategoryDTO;
            else
                return BadRequest(ShowingCategory_ShowingCategoryDTO);
        }

        [Route(ShowingCategoryRoute.Update), HttpPost]
        public async Task<ActionResult<ShowingCategory_ShowingCategoryDTO>> Update([FromBody] ShowingCategory_ShowingCategoryDTO ShowingCategory_ShowingCategoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingCategory_ShowingCategoryDTO.Id))
                return Forbid();

            ShowingCategory ShowingCategory = ConvertDTOToEntity(ShowingCategory_ShowingCategoryDTO);
            ShowingCategory = await ShowingCategoryService.Update(ShowingCategory);
            ShowingCategory_ShowingCategoryDTO = new ShowingCategory_ShowingCategoryDTO(ShowingCategory);
            if (ShowingCategory.IsValidated)
                return ShowingCategory_ShowingCategoryDTO;
            else
                return BadRequest(ShowingCategory_ShowingCategoryDTO);
        }

        [Route(ShowingCategoryRoute.Delete), HttpPost]
        public async Task<ActionResult<ShowingCategory_ShowingCategoryDTO>> Delete([FromBody] ShowingCategory_ShowingCategoryDTO ShowingCategory_ShowingCategoryDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingCategory_ShowingCategoryDTO.Id))
                return Forbid();

            ShowingCategory ShowingCategory = ConvertDTOToEntity(ShowingCategory_ShowingCategoryDTO);
            ShowingCategory = await ShowingCategoryService.Delete(ShowingCategory);
            ShowingCategory_ShowingCategoryDTO = new ShowingCategory_ShowingCategoryDTO(ShowingCategory);
            if (ShowingCategory.IsValidated)
                return ShowingCategory_ShowingCategoryDTO;
            else
                return BadRequest(ShowingCategory_ShowingCategoryDTO);
        }

        [Route(ShowingCategoryRoute.SaveImage), HttpPost]
        public async Task<ActionResult<ShowingCategory_ImageDTO>> SaveImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Image Image = new Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray(),
            };
            Image = await ShowingCategoryService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            ShowingCategory_ImageDTO product_ImageDTO = new ShowingCategory_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(product_ImageDTO);
        }

        private async Task<bool> HasPermission(long Id)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter = await ShowingCategoryService.ToFilter(ShowingCategoryFilter);
            if (Id == 0)
            {

            }
            else
            {
                ShowingCategoryFilter.Id = new IdFilter { Equal = Id };
                int count = await ShowingCategoryService.Count(ShowingCategoryFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ShowingCategory ConvertDTOToEntity(ShowingCategory_ShowingCategoryDTO ShowingCategory_ShowingCategoryDTO)
        {
            ShowingCategory_ShowingCategoryDTO.TrimString();
            ShowingCategory ShowingCategory = new ShowingCategory();
            ShowingCategory.Id = ShowingCategory_ShowingCategoryDTO.Id;
            ShowingCategory.Code = ShowingCategory_ShowingCategoryDTO.Code;
            ShowingCategory.Description = ShowingCategory_ShowingCategoryDTO.Description;
            ShowingCategory.Name = ShowingCategory_ShowingCategoryDTO.Name;
            ShowingCategory.ParentId = ShowingCategory_ShowingCategoryDTO.ParentId;
            ShowingCategory.Path = ShowingCategory_ShowingCategoryDTO.Path;
            ShowingCategory.Level = ShowingCategory_ShowingCategoryDTO.Level;
            ShowingCategory.StatusId = ShowingCategory_ShowingCategoryDTO.StatusId;
            ShowingCategory.ImageId = ShowingCategory_ShowingCategoryDTO.ImageId;
            ShowingCategory.RowId = ShowingCategory_ShowingCategoryDTO.RowId;
            ShowingCategory.Used = ShowingCategory_ShowingCategoryDTO.Used;
            ShowingCategory.Image = ShowingCategory_ShowingCategoryDTO.Image == null ? null : new Image
            {
                Id = ShowingCategory_ShowingCategoryDTO.Image.Id,
                Name = ShowingCategory_ShowingCategoryDTO.Image.Name,
                Url = ShowingCategory_ShowingCategoryDTO.Image.Url,
                ThumbnailUrl = ShowingCategory_ShowingCategoryDTO.Image.ThumbnailUrl,
                RowId = ShowingCategory_ShowingCategoryDTO.Image.RowId,
            };
            ShowingCategory.Parent = ShowingCategory_ShowingCategoryDTO.Parent == null ? null : new ShowingCategory
            {
                Id = ShowingCategory_ShowingCategoryDTO.Parent.Id,
                Code = ShowingCategory_ShowingCategoryDTO.Parent.Code,
                Name = ShowingCategory_ShowingCategoryDTO.Parent.Name,
                ParentId = ShowingCategory_ShowingCategoryDTO.Parent.ParentId,
                Path = ShowingCategory_ShowingCategoryDTO.Parent.Path,
                Level = ShowingCategory_ShowingCategoryDTO.Parent.Level,
                StatusId = ShowingCategory_ShowingCategoryDTO.Parent.StatusId,
                ImageId = ShowingCategory_ShowingCategoryDTO.Parent.ImageId,
                RowId = ShowingCategory_ShowingCategoryDTO.Parent.RowId,
                Used = ShowingCategory_ShowingCategoryDTO.Parent.Used,
            };
            ShowingCategory.Status = ShowingCategory_ShowingCategoryDTO.Status == null ? null : new Status
            {
                Id = ShowingCategory_ShowingCategoryDTO.Status.Id,
                Code = ShowingCategory_ShowingCategoryDTO.Status.Code,
                Name = ShowingCategory_ShowingCategoryDTO.Status.Name,
            };
            ShowingCategory.BaseLanguage = CurrentContext.Language;
            return ShowingCategory;
        }

        private ShowingCategoryFilter ConvertFilterDTOToFilterEntity(ShowingCategory_ShowingCategoryFilterDTO ShowingCategory_ShowingCategoryFilterDTO)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter.Selects = ShowingCategorySelect.ALL;
            ShowingCategoryFilter.Skip = 0;
            ShowingCategoryFilter.Take = 99999;
            ShowingCategoryFilter.OrderBy = ShowingCategory_ShowingCategoryFilterDTO.OrderBy;
            ShowingCategoryFilter.OrderType = ShowingCategory_ShowingCategoryFilterDTO.OrderType;

            ShowingCategoryFilter.Id = ShowingCategory_ShowingCategoryFilterDTO.Id;
            ShowingCategoryFilter.Code = ShowingCategory_ShowingCategoryFilterDTO.Code;
            ShowingCategoryFilter.Name = ShowingCategory_ShowingCategoryFilterDTO.Name;
            ShowingCategoryFilter.ParentId = ShowingCategory_ShowingCategoryFilterDTO.ParentId;
            ShowingCategoryFilter.Path = ShowingCategory_ShowingCategoryFilterDTO.Path;
            ShowingCategoryFilter.Level = ShowingCategory_ShowingCategoryFilterDTO.Level;
            ShowingCategoryFilter.StatusId = ShowingCategory_ShowingCategoryFilterDTO.StatusId;
            ShowingCategoryFilter.ImageId = ShowingCategory_ShowingCategoryFilterDTO.ImageId;
            ShowingCategoryFilter.RowId = ShowingCategory_ShowingCategoryFilterDTO.RowId;
            ShowingCategoryFilter.CreatedAt = ShowingCategory_ShowingCategoryFilterDTO.CreatedAt;
            ShowingCategoryFilter.UpdatedAt = ShowingCategory_ShowingCategoryFilterDTO.UpdatedAt;
            return ShowingCategoryFilter;
        }
    }
}

