using DMS.Common;
using DMS.Entities;
using DMS.Services.MStatus;
using DMS.Services.MStoreScoutingType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting_type
{
    public partial class StoreScoutingTypeController : RpcController
    {
        private IStoreScoutingTypeService StoreScoutingTypeService;
        private IStatusService StatusService;
        private ICurrentContext CurrentContext;
        public StoreScoutingTypeController(
            IStoreScoutingTypeService StoreScoutingTypeService,
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.StoreScoutingTypeService = StoreScoutingTypeService;
            this.StatusService = StatusService;
            this.CurrentContext = CurrentContext;
        }

        [Route(StoreScoutingTypeRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] StoreScoutingType_StoreScoutingTypeFilterDTO StoreScoutingType_StoreScoutingTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingTypeFilter StoreScoutingTypeFilter = ConvertFilterDTOToFilterEntity(StoreScoutingType_StoreScoutingTypeFilterDTO);
            StoreScoutingTypeFilter = await StoreScoutingTypeService.ToFilter(StoreScoutingTypeFilter);
            int count = await StoreScoutingTypeService.Count(StoreScoutingTypeFilter);
            return count;
        }

        [Route(StoreScoutingTypeRoute.List), HttpPost]
        public async Task<ActionResult<List<StoreScoutingType_StoreScoutingTypeDTO>>> List([FromBody] StoreScoutingType_StoreScoutingTypeFilterDTO StoreScoutingType_StoreScoutingTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingTypeFilter StoreScoutingTypeFilter = ConvertFilterDTOToFilterEntity(StoreScoutingType_StoreScoutingTypeFilterDTO);
            StoreScoutingTypeFilter = await StoreScoutingTypeService.ToFilter(StoreScoutingTypeFilter);
            List<StoreScoutingType> StoreScoutingTypes = await StoreScoutingTypeService.List(StoreScoutingTypeFilter);
            List<StoreScoutingType_StoreScoutingTypeDTO> StoreScoutingType_StoreScoutingTypeDTOs = StoreScoutingTypes
                .Select(c => new StoreScoutingType_StoreScoutingTypeDTO(c)).ToList();
            return StoreScoutingType_StoreScoutingTypeDTOs;
        }

        [Route(StoreScoutingTypeRoute.Get), HttpPost]
        public async Task<ActionResult<StoreScoutingType_StoreScoutingTypeDTO>> Get([FromBody] StoreScoutingType_StoreScoutingTypeDTO StoreScoutingType_StoreScoutingTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScoutingType_StoreScoutingTypeDTO.Id))
                return Forbid();

            StoreScoutingType StoreScoutingType = await StoreScoutingTypeService.Get(StoreScoutingType_StoreScoutingTypeDTO.Id);
            return new StoreScoutingType_StoreScoutingTypeDTO(StoreScoutingType);
        }

        [Route(StoreScoutingTypeRoute.Create), HttpPost]
        public async Task<ActionResult<StoreScoutingType_StoreScoutingTypeDTO>> Create([FromBody] StoreScoutingType_StoreScoutingTypeDTO StoreScoutingType_StoreScoutingTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScoutingType_StoreScoutingTypeDTO.Id))
                return Forbid();

            StoreScoutingType StoreScoutingType = ConvertDTOToEntity(StoreScoutingType_StoreScoutingTypeDTO);
            StoreScoutingType = await StoreScoutingTypeService.Create(StoreScoutingType);
            StoreScoutingType_StoreScoutingTypeDTO = new StoreScoutingType_StoreScoutingTypeDTO(StoreScoutingType);
            if (StoreScoutingType.IsValidated)
                return StoreScoutingType_StoreScoutingTypeDTO;
            else
                return BadRequest(StoreScoutingType_StoreScoutingTypeDTO);
        }

        [Route(StoreScoutingTypeRoute.Update), HttpPost]
        public async Task<ActionResult<StoreScoutingType_StoreScoutingTypeDTO>> Update([FromBody] StoreScoutingType_StoreScoutingTypeDTO StoreScoutingType_StoreScoutingTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScoutingType_StoreScoutingTypeDTO.Id))
                return Forbid();

            StoreScoutingType StoreScoutingType = ConvertDTOToEntity(StoreScoutingType_StoreScoutingTypeDTO);
            StoreScoutingType = await StoreScoutingTypeService.Update(StoreScoutingType);
            StoreScoutingType_StoreScoutingTypeDTO = new StoreScoutingType_StoreScoutingTypeDTO(StoreScoutingType);
            if (StoreScoutingType.IsValidated)
                return StoreScoutingType_StoreScoutingTypeDTO;
            else
                return BadRequest(StoreScoutingType_StoreScoutingTypeDTO);
        }

        [Route(StoreScoutingTypeRoute.Delete), HttpPost]
        public async Task<ActionResult<StoreScoutingType_StoreScoutingTypeDTO>> Delete([FromBody] StoreScoutingType_StoreScoutingTypeDTO StoreScoutingType_StoreScoutingTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScoutingType_StoreScoutingTypeDTO.Id))
                return Forbid();

            StoreScoutingType StoreScoutingType = ConvertDTOToEntity(StoreScoutingType_StoreScoutingTypeDTO);
            StoreScoutingType = await StoreScoutingTypeService.Delete(StoreScoutingType);
            StoreScoutingType_StoreScoutingTypeDTO = new StoreScoutingType_StoreScoutingTypeDTO(StoreScoutingType);
            if (StoreScoutingType.IsValidated)
                return StoreScoutingType_StoreScoutingTypeDTO;
            else
                return BadRequest(StoreScoutingType_StoreScoutingTypeDTO);
        }

        [Route(StoreScoutingTypeRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter();
            StoreScoutingTypeFilter = await StoreScoutingTypeService.ToFilter(StoreScoutingTypeFilter);
            StoreScoutingTypeFilter.Id = new IdFilter { In = Ids };
            StoreScoutingTypeFilter.Selects = StoreScoutingTypeSelect.Id;
            StoreScoutingTypeFilter.Skip = 0;
            StoreScoutingTypeFilter.Take = int.MaxValue;

            List<StoreScoutingType> StoreScoutingTypes = await StoreScoutingTypeService.List(StoreScoutingTypeFilter);
            StoreScoutingTypes = await StoreScoutingTypeService.BulkDelete(StoreScoutingTypes);
            if (StoreScoutingTypes.Any(x => !x.IsValidated))
                return BadRequest(StoreScoutingTypes.Where(x => !x.IsValidated));
            return true;
        }

        [Route(StoreScoutingTypeRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            List<StoreScoutingType> StoreScoutingTypes = new List<StoreScoutingType>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(StoreScoutingTypes);
                int StartColumn = 1;
                int StartRow = 1;
                int IdColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int StatusIdColumn = 3 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i + StartRow, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i + StartRow, IdColumn].Value?.ToString();
                    string CodeValue = worksheet.Cells[i + StartRow, CodeColumn].Value?.ToString();
                    string NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
                    string StatusIdValue = worksheet.Cells[i + StartRow, StatusIdColumn].Value?.ToString();

                    StoreScoutingType StoreScoutingType = new StoreScoutingType();
                    StoreScoutingType.Code = CodeValue;
                    StoreScoutingType.Name = NameValue;

                    StoreScoutingTypes.Add(StoreScoutingType);
                }
            }
            StoreScoutingTypes = await StoreScoutingTypeService.Import(StoreScoutingTypes);
            if (StoreScoutingTypes.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < StoreScoutingTypes.Count; i++)
                {
                    StoreScoutingType StoreScoutingType = StoreScoutingTypes[i];
                    if (!StoreScoutingType.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (StoreScoutingType.Errors.ContainsKey(nameof(StoreScoutingType.Id)))
                            Error += StoreScoutingType.Errors[nameof(StoreScoutingType.Id)];
                        if (StoreScoutingType.Errors.ContainsKey(nameof(StoreScoutingType.Code)))
                            Error += StoreScoutingType.Errors[nameof(StoreScoutingType.Code)];
                        if (StoreScoutingType.Errors.ContainsKey(nameof(StoreScoutingType.Name)))
                            Error += StoreScoutingType.Errors[nameof(StoreScoutingType.Name)];
                        if (StoreScoutingType.Errors.ContainsKey(nameof(StoreScoutingType.StatusId)))
                            Error += StoreScoutingType.Errors[nameof(StoreScoutingType.StatusId)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        [Route(StoreScoutingTypeRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] StoreScoutingType_StoreScoutingTypeFilterDTO StoreScoutingType_StoreScoutingTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region StoreScoutingType
                var StoreScoutingTypeFilter = ConvertFilterDTOToFilterEntity(StoreScoutingType_StoreScoutingTypeFilterDTO);
                StoreScoutingTypeFilter.Skip = 0;
                StoreScoutingTypeFilter.Take = int.MaxValue;
                StoreScoutingTypeFilter = await StoreScoutingTypeService.ToFilter(StoreScoutingTypeFilter);
                List<StoreScoutingType> StoreScoutingTypes = await StoreScoutingTypeService.List(StoreScoutingTypeFilter);

                var StoreScoutingTypeHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                    "StatusId",
                };
                List<object[]> StoreScoutingTypeData = new List<object[]>();
                for (int i = 0; i < StoreScoutingTypes.Count; i++)
                {
                    var StoreScoutingType = StoreScoutingTypes[i];
                    StoreScoutingTypeData.Add(new Object[]
                    {
                        StoreScoutingType.Id,
                        StoreScoutingType.Code,
                        StoreScoutingType.Name,
                        StoreScoutingType.StatusId,
                    });
                }
                excel.GenerateWorksheet("StoreScoutingType", StoreScoutingTypeHeaders, StoreScoutingTypeData);
                #endregion

                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "StoreScoutingType.xlsx");
        }

        [Route(StoreScoutingTypeRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] StoreScoutingType_StoreScoutingTypeFilterDTO StoreScoutingType_StoreScoutingTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region StoreScoutingType
                var StoreScoutingTypeHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                    "StatusId",
                };
                List<object[]> StoreScoutingTypeData = new List<object[]>();
                excel.GenerateWorksheet("StoreScoutingType", StoreScoutingTypeHeaders, StoreScoutingTypeData);
                #endregion

                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "StoreScoutingType.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter();
            StoreScoutingTypeFilter = await StoreScoutingTypeService.ToFilter(StoreScoutingTypeFilter);
            if (Id == 0)
            {

            }
            else
            {
                StoreScoutingTypeFilter.Id = new IdFilter { Equal = Id };
                int count = await StoreScoutingTypeService.Count(StoreScoutingTypeFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private StoreScoutingType ConvertDTOToEntity(StoreScoutingType_StoreScoutingTypeDTO StoreScoutingType_StoreScoutingTypeDTO)
        {
            StoreScoutingType_StoreScoutingTypeDTO.TrimString();
            StoreScoutingType StoreScoutingType = new StoreScoutingType();
            StoreScoutingType.Id = StoreScoutingType_StoreScoutingTypeDTO.Id;
            StoreScoutingType.Code = StoreScoutingType_StoreScoutingTypeDTO.Code;
            StoreScoutingType.Name = StoreScoutingType_StoreScoutingTypeDTO.Name;
            StoreScoutingType.StatusId = StoreScoutingType_StoreScoutingTypeDTO.StatusId;
            StoreScoutingType.BaseLanguage = CurrentContext.Language;
            return StoreScoutingType;
        }

        private StoreScoutingTypeFilter ConvertFilterDTOToFilterEntity(StoreScoutingType_StoreScoutingTypeFilterDTO StoreScoutingType_StoreScoutingTypeFilterDTO)
        {
            StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter();
            StoreScoutingTypeFilter.Selects = StoreScoutingTypeSelect.ALL;
            StoreScoutingTypeFilter.Skip = StoreScoutingType_StoreScoutingTypeFilterDTO.Skip;
            StoreScoutingTypeFilter.Take = StoreScoutingType_StoreScoutingTypeFilterDTO.Take;
            StoreScoutingTypeFilter.OrderBy = StoreScoutingType_StoreScoutingTypeFilterDTO.OrderBy;
            StoreScoutingTypeFilter.OrderType = StoreScoutingType_StoreScoutingTypeFilterDTO.OrderType;

            StoreScoutingTypeFilter.Id = StoreScoutingType_StoreScoutingTypeFilterDTO.Id;
            StoreScoutingTypeFilter.Code = StoreScoutingType_StoreScoutingTypeFilterDTO.Code;
            StoreScoutingTypeFilter.Name = StoreScoutingType_StoreScoutingTypeFilterDTO.Name;
            StoreScoutingTypeFilter.StatusId = StoreScoutingType_StoreScoutingTypeFilterDTO.StatusId;
            StoreScoutingTypeFilter.CreatedAt = StoreScoutingType_StoreScoutingTypeFilterDTO.CreatedAt;
            StoreScoutingTypeFilter.UpdatedAt = StoreScoutingType_StoreScoutingTypeFilterDTO.UpdatedAt;
            return StoreScoutingTypeFilter;
        }
    }
}

