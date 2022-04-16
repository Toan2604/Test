using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Services.MCategory;
using DMS.Services.MShowingCategory;
using DMS.Services.MShowingItem;
using DMS.Services.MStatus;
using DMS.Services.MUnitOfMeasure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_item
{
    public partial class ShowingItemController : RpcController
    {
        private IShowingCategoryService ShowingCategoryService;
        private IStatusService StatusService;
        private IUnitOfMeasureService UnitOfMeasureService;
        private IShowingItemService ShowingItemService;
        private ICategoryService CategoryService;
        private ICurrentContext CurrentContext;
        public ShowingItemController(
            IShowingCategoryService ShowingCategoryService,
            IStatusService StatusService,
            IUnitOfMeasureService UnitOfMeasureService,
            IShowingItemService ShowingItemService,
            ICategoryService CategoryService,
            ICurrentContext CurrentContext
        )
        {
            this.ShowingCategoryService = ShowingCategoryService;
            this.StatusService = StatusService;
            this.UnitOfMeasureService = UnitOfMeasureService;
            this.ShowingItemService = ShowingItemService;
            this.CategoryService = CategoryService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ShowingItemRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ShowingItem_ShowingItemFilterDTO ShowingItem_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = ConvertFilterDTOToFilterEntity(ShowingItem_ShowingItemFilterDTO);
            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            int count = await ShowingItemService.Count(ShowingItemFilter);
            return count;
        }

        [Route(ShowingItemRoute.List), HttpPost]
        public async Task<ActionResult<List<ShowingItem_ShowingItemDTO>>> List([FromBody] ShowingItem_ShowingItemFilterDTO ShowingItem_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = ConvertFilterDTOToFilterEntity(ShowingItem_ShowingItemFilterDTO);
            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            List<ShowingItem> ShowingItems = await ShowingItemService.List(ShowingItemFilter);
            List<ShowingItem_ShowingItemDTO> ShowingItem_ShowingItemDTOs = ShowingItems
                .Select(c => new ShowingItem_ShowingItemDTO(c)).ToList();
            return ShowingItem_ShowingItemDTOs;
        }

        [Route(ShowingItemRoute.Get), HttpPost]
        public async Task<ActionResult<ShowingItem_ShowingItemDTO>> Get([FromBody] ShowingItem_ShowingItemDTO ShowingItem_ShowingItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingItem_ShowingItemDTO.Id))
                return Forbid();

            ShowingItem ShowingItem = await ShowingItemService.Get(ShowingItem_ShowingItemDTO.Id);
            return new ShowingItem_ShowingItemDTO(ShowingItem);
        }

        [Route(ShowingItemRoute.Create), HttpPost]
        public async Task<ActionResult<ShowingItem_ShowingItemDTO>> Create([FromBody] ShowingItem_ShowingItemDTO ShowingItem_ShowingItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingItem_ShowingItemDTO.Id))
                return Forbid();

            ShowingItem ShowingItem = ConvertDTOToEntity(ShowingItem_ShowingItemDTO);
            ShowingItem = await ShowingItemService.Create(ShowingItem);
            ShowingItem_ShowingItemDTO = new ShowingItem_ShowingItemDTO(ShowingItem);
            if (ShowingItem.IsValidated)
                return ShowingItem_ShowingItemDTO;
            else
                return BadRequest(ShowingItem_ShowingItemDTO);
        }

        [Route(ShowingItemRoute.Update), HttpPost]
        public async Task<ActionResult<ShowingItem_ShowingItemDTO>> Update([FromBody] ShowingItem_ShowingItemDTO ShowingItem_ShowingItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingItem_ShowingItemDTO.Id))
                return Forbid();

            ShowingItem ShowingItem = ConvertDTOToEntity(ShowingItem_ShowingItemDTO);
            ShowingItem = await ShowingItemService.Update(ShowingItem);
            ShowingItem_ShowingItemDTO = new ShowingItem_ShowingItemDTO(ShowingItem);
            if (ShowingItem.IsValidated)
                return ShowingItem_ShowingItemDTO;
            else
                return BadRequest(ShowingItem_ShowingItemDTO);
        }

        [Route(ShowingItemRoute.Delete), HttpPost]
        public async Task<ActionResult<ShowingItem_ShowingItemDTO>> Delete([FromBody] ShowingItem_ShowingItemDTO ShowingItem_ShowingItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ShowingItem_ShowingItemDTO.Id))
                return Forbid();

            ShowingItem ShowingItem = ConvertDTOToEntity(ShowingItem_ShowingItemDTO);
            ShowingItem = await ShowingItemService.Delete(ShowingItem);
            ShowingItem_ShowingItemDTO = new ShowingItem_ShowingItemDTO(ShowingItem);
            if (ShowingItem.IsValidated)
                return ShowingItem_ShowingItemDTO;
            else
                return BadRequest(ShowingItem_ShowingItemDTO);
        }

        [Route(ShowingItemRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            ShowingItemFilter.Id = new IdFilter { In = Ids };
            ShowingItemFilter.Selects = ShowingItemSelect.Id;
            ShowingItemFilter.Skip = 0;
            ShowingItemFilter.Take = int.MaxValue;

            List<ShowingItem> ShowingItems = await ShowingItemService.List(ShowingItemFilter);
            ShowingItems = await ShowingItemService.BulkDelete(ShowingItems);
            if (ShowingItems.Any(x => !x.IsValidated))
                return BadRequest(ShowingItems.Where(x => !x.IsValidated));
            return true;
        }

        [Route(ShowingItemRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.ALL
            };
            List<Status> Statuses = await StatusService.List(StatusFilter);
            UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureSelect.ALL
            };
            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);
            List<ShowingItem> ShowingItems = new List<ShowingItem>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(ShowingItems);
                int StartColumn = 1;
                int StartRow = 1;
                int IdColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int CategoryIdColumn = 3 + StartColumn;
                int UnitOfMeasureIdColumn = 4 + StartColumn;
                int SalePriceColumn = 5 + StartColumn;
                int DescriptionColumn = 6 + StartColumn;
                int StatusIdColumn = 7 + StartColumn;
                int UsedColumn = 11 + StartColumn;
                int RowIdColumn = 12 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i + StartRow, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i + StartRow, IdColumn].Value?.ToString();
                    string CodeValue = worksheet.Cells[i + StartRow, CodeColumn].Value?.ToString();
                    string NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
                    string CategoryIdValue = worksheet.Cells[i + StartRow, CategoryIdColumn].Value?.ToString();
                    string UnitOfMeasureIdValue = worksheet.Cells[i + StartRow, UnitOfMeasureIdColumn].Value?.ToString();
                    string SalePriceValue = worksheet.Cells[i + StartRow, SalePriceColumn].Value?.ToString();
                    string DescriptionValue = worksheet.Cells[i + StartRow, DescriptionColumn].Value?.ToString();
                    string StatusIdValue = worksheet.Cells[i + StartRow, StatusIdColumn].Value?.ToString();
                    string UsedValue = worksheet.Cells[i + StartRow, UsedColumn].Value?.ToString();
                    string RowIdValue = worksheet.Cells[i + StartRow, RowIdColumn].Value?.ToString();

                    ShowingItem ShowingItem = new ShowingItem();
                    ShowingItem.Code = CodeValue;
                    ShowingItem.Name = NameValue;
                    ShowingItem.SalePrice = decimal.TryParse(SalePriceValue, out decimal SalePrice) ? SalePrice : 0;
                    ShowingItem.Description = DescriptionValue;
                    Status Status = Statuses.Where(x => x.Id.ToString() == StatusIdValue).FirstOrDefault();
                    ShowingItem.StatusId = Status == null ? 0 : Status.Id;
                    ShowingItem.Status = Status;
                    UnitOfMeasure UnitOfMeasure = UnitOfMeasures.Where(x => x.Id.ToString() == UnitOfMeasureIdValue).FirstOrDefault();
                    ShowingItem.UnitOfMeasureId = UnitOfMeasure == null ? 0 : UnitOfMeasure.Id;
                    ShowingItem.UnitOfMeasure = UnitOfMeasure;

                    ShowingItems.Add(ShowingItem);
                }
            }
            ShowingItems = await ShowingItemService.Import(ShowingItems);
            if (ShowingItems.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < ShowingItems.Count; i++)
                {
                    ShowingItem ShowingItem = ShowingItems[i];
                    if (!ShowingItem.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.Id)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.Id)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.Code)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.Code)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.Name)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.Name)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.ShowingCategoryId)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.ShowingCategoryId)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.UnitOfMeasureId)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.UnitOfMeasureId)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.SalePrice)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.SalePrice)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.Description)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.Description)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.StatusId)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.StatusId)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.Used)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.Used)];
                        if (ShowingItem.Errors.ContainsKey(nameof(ShowingItem.RowId)))
                            Error += ShowingItem.Errors[nameof(ShowingItem.RowId)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        [Route(ShowingItemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ShowingItem_ShowingItemFilterDTO ShowingItem_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region ShowingItem
                var ShowingItemFilter = ConvertFilterDTOToFilterEntity(ShowingItem_ShowingItemFilterDTO);
                ShowingItemFilter.Skip = 0;
                ShowingItemFilter.Take = int.MaxValue;
                ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
                List<ShowingItem> ShowingItems = await ShowingItemService.List(ShowingItemFilter);

                var ShowingItemHeaders = new List<string>()
                {
                    "Id",
                    "Mã sản phẩm trưng bày",
                    "Tên sản phẩm trưng bày",
                    "Danh mục",
                    "Đơn vị tính",
                    "Giá",
                    "Mô tả",
                    "Trạng thái"
                };
                List<object[]> ShowingItemData = new List<object[]>();
                for (int i = 0; i < ShowingItems.Count; i++)
                {
                    var ShowingItem = ShowingItems[i];
                    ShowingItemData.Add(new Object[]
                    {
                        ShowingItem.Id,
                        ShowingItem.Code,
                        ShowingItem.Name,
                        ShowingItem.ShowingCategory == null ? null : ShowingItem.ShowingCategory.Name,
                        ShowingItem.UnitOfMeasure == null ? null : ShowingItem.UnitOfMeasure.Name,
                        ShowingItem.SalePrice,
                        ShowingItem.Description,
                        ShowingItem.Status == null ? null :  ShowingItem.Status.Name,
                    });
                }
                excel.GenerateWorksheet("Sản phẩm trưng bày", ShowingItemHeaders, ShowingItemData);
                #endregion

                #region ShowingCategory
                var ShowingCategoryFilter = new ShowingCategoryFilter();
                ShowingCategoryFilter.Selects = ShowingCategorySelect.ALL;
                ShowingCategoryFilter.OrderBy = ShowingCategoryOrder.Id;
                ShowingCategoryFilter.OrderType = OrderType.ASC;
                ShowingCategoryFilter.Skip = 0;
                ShowingCategoryFilter.Take = int.MaxValue;
                List<ShowingCategory> Categories = await ShowingCategoryService.List(ShowingCategoryFilter);

                var ShowingCategoryHeaders = new List<string>()
                {
                    "Id",
                    "Mã danh mục",
                    "Tên danh mục",
                    "Danh mục cha",
                };
                List<object[]> ShowingCategoryData = new List<object[]>();
                for (int i = 0; i < Categories.Count; i++)
                {
                    var ShowingCategory = Categories[i];
                    ShowingCategoryData.Add(new Object[]
                    {
                        ShowingCategory.Id,
                        ShowingCategory.Code,
                        ShowingCategory.Name,
                        ShowingCategory.Parent == null ? null : ShowingCategory.Parent.Name,
                    });
                }
                excel.GenerateWorksheet("Danh mục", ShowingCategoryHeaders, ShowingCategoryData);
                #endregion
                #region UnitOfMeasure
                var UnitOfMeasureFilter = new UnitOfMeasureFilter();
                UnitOfMeasureFilter.Selects = UnitOfMeasureSelect.ALL;
                UnitOfMeasureFilter.OrderBy = UnitOfMeasureOrder.Id;
                UnitOfMeasureFilter.OrderType = OrderType.ASC;
                UnitOfMeasureFilter.Skip = 0;
                UnitOfMeasureFilter.Take = int.MaxValue;
                List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);

                var UnitOfMeasureHeaders = new List<string>()
                {
                    "Id",
                    "Mã đơn vị tính",
                    "Tên đơn vị tính",
                    "Mô tả",
                };
                List<object[]> UnitOfMeasureData = new List<object[]>();
                for (int i = 0; i < UnitOfMeasures.Count; i++)
                {
                    var UnitOfMeasure = UnitOfMeasures[i];
                    UnitOfMeasureData.Add(new Object[]
                    {
                        UnitOfMeasure.Id,
                        UnitOfMeasure.Code,
                        UnitOfMeasure.Name,
                        UnitOfMeasure.Description,
                    });
                }
                excel.GenerateWorksheet("Đơn vị tính", UnitOfMeasureHeaders, UnitOfMeasureData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "ShowingItem.xlsx");
        }

        [Route(ShowingItemRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            #region lấy dữ liệu đổ vào các sheet
            List<ShowingCategory> ShowingCategories = await ShowingCategoryService.List(new ShowingCategoryFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ShowingCategorySelect.Code | ShowingCategorySelect.Name | ShowingCategorySelect.Path,
                OrderBy = ShowingCategoryOrder.Name,
                OrderType = OrderType.ASC
            });
            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(new UnitOfMeasureFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureSelect.Code | UnitOfMeasureSelect.Name,
                OrderBy = UnitOfMeasureOrder.Name,
                OrderType = OrderType.ASC
            });
            #endregion

            string path = "Templates/ShowingItem_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Categories = ShowingCategories;
            Data.UnitOfMeasures = UnitOfMeasures;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "ShowingItem.xlsx");
        }

        [Route(ShowingItemRoute.SaveImage), HttpPost]
        public async Task<ActionResult<ShowingItem_ImageDTO>> SaveImage(IFormFile file)
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
            Image = await ShowingItemService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            ShowingItem_ImageDTO ShowingItem_ImageDTO = new ShowingItem_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(ShowingItem_ImageDTO);
        }

        private async Task<bool> HasPermission(long Id)
        {
            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            if (Id == 0)
            {

            }
            else
            {
                ShowingItemFilter.Id = new IdFilter { Equal = Id };
                int count = await ShowingItemService.Count(ShowingItemFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ShowingItem ConvertDTOToEntity(ShowingItem_ShowingItemDTO ShowingItem_ShowingItemDTO)
        {
            ShowingItem_ShowingItemDTO.TrimString();
            ShowingItem ShowingItem = new ShowingItem();
            ShowingItem.Id = ShowingItem_ShowingItemDTO.Id;
            ShowingItem.Code = ShowingItem_ShowingItemDTO.Code;
            ShowingItem.Name = ShowingItem_ShowingItemDTO.Name;
            ShowingItem.ShowingCategoryId = ShowingItem_ShowingItemDTO.ShowingCategoryId;
            ShowingItem.UnitOfMeasureId = ShowingItem_ShowingItemDTO.UnitOfMeasureId;
            ShowingItem.SalePrice = ShowingItem_ShowingItemDTO.SalePrice;
            ShowingItem.ERPCode = ShowingItem_ShowingItemDTO.ERPCode;
            ShowingItem.Description = ShowingItem_ShowingItemDTO.Description;
            ShowingItem.StatusId = ShowingItem_ShowingItemDTO.StatusId;
            ShowingItem.Used = ShowingItem_ShowingItemDTO.Used;
            ShowingItem.RowId = ShowingItem_ShowingItemDTO.RowId;
            ShowingItem.ShowingCategory = ShowingItem_ShowingItemDTO.ShowingCategory == null ? null : new ShowingCategory
            {
                Id = ShowingItem_ShowingItemDTO.ShowingCategory.Id,
                Code = ShowingItem_ShowingItemDTO.ShowingCategory.Code,
                Name = ShowingItem_ShowingItemDTO.ShowingCategory.Name,
                ParentId = ShowingItem_ShowingItemDTO.ShowingCategory.ParentId,
                Path = ShowingItem_ShowingItemDTO.ShowingCategory.Path,
                Level = ShowingItem_ShowingItemDTO.ShowingCategory.Level,
                StatusId = ShowingItem_ShowingItemDTO.ShowingCategory.StatusId,
                ImageId = ShowingItem_ShowingItemDTO.ShowingCategory.ImageId,
                RowId = ShowingItem_ShowingItemDTO.ShowingCategory.RowId,
                Used = ShowingItem_ShowingItemDTO.ShowingCategory.Used,
            };
            ShowingItem.Status = ShowingItem_ShowingItemDTO.Status == null ? null : new Status
            {
                Id = ShowingItem_ShowingItemDTO.Status.Id,
                Code = ShowingItem_ShowingItemDTO.Status.Code,
                Name = ShowingItem_ShowingItemDTO.Status.Name,
            };
            ShowingItem.UnitOfMeasure = ShowingItem_ShowingItemDTO.UnitOfMeasure == null ? null : new UnitOfMeasure
            {
                Id = ShowingItem_ShowingItemDTO.UnitOfMeasure.Id,
                Code = ShowingItem_ShowingItemDTO.UnitOfMeasure.Code,
                Name = ShowingItem_ShowingItemDTO.UnitOfMeasure.Name,
                Description = ShowingItem_ShowingItemDTO.UnitOfMeasure.Description,
                StatusId = ShowingItem_ShowingItemDTO.UnitOfMeasure.StatusId,
                Used = ShowingItem_ShowingItemDTO.UnitOfMeasure.Used,
                RowId = ShowingItem_ShowingItemDTO.UnitOfMeasure.RowId,
            };
            ShowingItem.ShowingItemImageMappings = ShowingItem_ShowingItemDTO.ShowingItemImageMappings?
               .Select(x => new ShowingItemImageMapping
               {
                   ImageId = x.ImageId,
                   Image = new Image
                   {
                       Id = x.Image.Id,
                       Name = x.Image.Name,
                       Url = x.Image.Url,
                       ThumbnailUrl = x.Image.ThumbnailUrl,
                   },
               }).ToList();
            ShowingItem.BaseLanguage = CurrentContext.Language;
            return ShowingItem;
        }

        private ShowingItemFilter ConvertFilterDTOToFilterEntity(ShowingItem_ShowingItemFilterDTO ShowingItem_ShowingItemFilterDTO)
        {
            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Selects = ShowingItemSelect.ALL;
            ShowingItemFilter.Skip = ShowingItem_ShowingItemFilterDTO.Skip;
            ShowingItemFilter.Take = ShowingItem_ShowingItemFilterDTO.Take;
            ShowingItemFilter.OrderBy = ShowingItem_ShowingItemFilterDTO.OrderBy;
            ShowingItemFilter.OrderType = ShowingItem_ShowingItemFilterDTO.OrderType;

            ShowingItemFilter.Id = ShowingItem_ShowingItemFilterDTO.Id;
            ShowingItemFilter.Code = ShowingItem_ShowingItemFilterDTO.Code;
            ShowingItemFilter.Name = ShowingItem_ShowingItemFilterDTO.Name;
            ShowingItemFilter.ShowingCategoryId = ShowingItem_ShowingItemFilterDTO.ShowingCategoryId;
            ShowingItemFilter.UnitOfMeasureId = ShowingItem_ShowingItemFilterDTO.UnitOfMeasureId;
            ShowingItemFilter.SalePrice = ShowingItem_ShowingItemFilterDTO.SalePrice;
            ShowingItemFilter.ERPCode = ShowingItem_ShowingItemFilterDTO.ERPCode;
            ShowingItemFilter.Description = ShowingItem_ShowingItemFilterDTO.Description;
            ShowingItemFilter.StatusId = ShowingItem_ShowingItemFilterDTO.StatusId;
            ShowingItemFilter.RowId = ShowingItem_ShowingItemFilterDTO.RowId;
            ShowingItemFilter.CreatedAt = ShowingItem_ShowingItemFilterDTO.CreatedAt;
            ShowingItemFilter.UpdatedAt = ShowingItem_ShowingItemFilterDTO.UpdatedAt;
            ShowingItemFilter.Search = ShowingItem_ShowingItemFilterDTO.Search;
            return ShowingItemFilter;
        }
    }
}

