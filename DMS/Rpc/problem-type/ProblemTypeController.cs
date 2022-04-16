using DMS.Common;
using DMS.Entities;
using DMS.Services.MProblemType;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.problem_type
{
    public partial class ProblemTypeController : RpcController
    {
        private IProblemTypeService ProblemTypeService;
        private IStatusService StatusService;
        private ICurrentContext CurrentContext;
        public ProblemTypeController(
            IProblemTypeService ProblemTypeService,
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.ProblemTypeService = ProblemTypeService;
            this.StatusService = StatusService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ProblemTypeRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ProblemType_ProblemTypeFilterDTO ProblemType_ProblemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemTypeFilter ProblemTypeFilter = ConvertFilterDTOToFilterEntity(ProblemType_ProblemTypeFilterDTO);
            ProblemTypeFilter = await ProblemTypeService.ToFilter(ProblemTypeFilter);
            int count = await ProblemTypeService.Count(ProblemTypeFilter);
            return count;
        }

        [Route(ProblemTypeRoute.List), HttpPost]
        public async Task<ActionResult<List<ProblemType_ProblemTypeDTO>>> List([FromBody] ProblemType_ProblemTypeFilterDTO ProblemType_ProblemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemTypeFilter ProblemTypeFilter = ConvertFilterDTOToFilterEntity(ProblemType_ProblemTypeFilterDTO);
            ProblemTypeFilter = await ProblemTypeService.ToFilter(ProblemTypeFilter);
            List<ProblemType> ProblemTypes = await ProblemTypeService.List(ProblemTypeFilter);
            List<ProblemType_ProblemTypeDTO> ProblemType_ProblemTypeDTOs = ProblemTypes
                .Select(c => new ProblemType_ProblemTypeDTO(c)).ToList();
            return ProblemType_ProblemTypeDTOs;
        }

        [Route(ProblemTypeRoute.Get), HttpPost]
        public async Task<ActionResult<ProblemType_ProblemTypeDTO>> Get([FromBody] ProblemType_ProblemTypeDTO ProblemType_ProblemTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ProblemType_ProblemTypeDTO.Id))
                return Forbid();

            ProblemType ProblemType = await ProblemTypeService.Get(ProblemType_ProblemTypeDTO.Id);
            return new ProblemType_ProblemTypeDTO(ProblemType);
        }

        [Route(ProblemTypeRoute.Create), HttpPost]
        public async Task<ActionResult<ProblemType_ProblemTypeDTO>> Create([FromBody] ProblemType_ProblemTypeDTO ProblemType_ProblemTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ProblemType_ProblemTypeDTO.Id))
                return Forbid();

            ProblemType ProblemType = ConvertDTOToEntity(ProblemType_ProblemTypeDTO);
            ProblemType = await ProblemTypeService.Create(ProblemType);
            ProblemType_ProblemTypeDTO = new ProblemType_ProblemTypeDTO(ProblemType);
            if (ProblemType.IsValidated)
                return ProblemType_ProblemTypeDTO;
            else
                return BadRequest(ProblemType_ProblemTypeDTO);
        }

        [Route(ProblemTypeRoute.Update), HttpPost]
        public async Task<ActionResult<ProblemType_ProblemTypeDTO>> Update([FromBody] ProblemType_ProblemTypeDTO ProblemType_ProblemTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ProblemType_ProblemTypeDTO.Id))
                return Forbid();

            ProblemType ProblemType = ConvertDTOToEntity(ProblemType_ProblemTypeDTO);
            ProblemType = await ProblemTypeService.Update(ProblemType);
            ProblemType_ProblemTypeDTO = new ProblemType_ProblemTypeDTO(ProblemType);
            if (ProblemType.IsValidated)
                return ProblemType_ProblemTypeDTO;
            else
                return BadRequest(ProblemType_ProblemTypeDTO);
        }

        [Route(ProblemTypeRoute.Delete), HttpPost]
        public async Task<ActionResult<ProblemType_ProblemTypeDTO>> Delete([FromBody] ProblemType_ProblemTypeDTO ProblemType_ProblemTypeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ProblemType_ProblemTypeDTO.Id))
                return Forbid();

            ProblemType ProblemType = ConvertDTOToEntity(ProblemType_ProblemTypeDTO);
            ProblemType = await ProblemTypeService.Delete(ProblemType);
            ProblemType_ProblemTypeDTO = new ProblemType_ProblemTypeDTO(ProblemType);
            if (ProblemType.IsValidated)
                return ProblemType_ProblemTypeDTO;
            else
                return BadRequest(ProblemType_ProblemTypeDTO);
        }

        [Route(ProblemTypeRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter();
            ProblemTypeFilter = await ProblemTypeService.ToFilter(ProblemTypeFilter);
            ProblemTypeFilter.Id = new IdFilter { In = Ids };
            ProblemTypeFilter.Selects = ProblemTypeSelect.Id;
            ProblemTypeFilter.Skip = 0;
            ProblemTypeFilter.Take = int.MaxValue;

            List<ProblemType> ProblemTypes = await ProblemTypeService.List(ProblemTypeFilter);
            ProblemTypes = await ProblemTypeService.BulkDelete(ProblemTypes);
            if (ProblemTypes.Any(x => !x.IsValidated))
                return BadRequest(ProblemTypes.Where(x => !x.IsValidated));
            return true;
        }

        [Route(ProblemTypeRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            List<ProblemType> ProblemTypes = new List<ProblemType>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(ProblemTypes);
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

                    ProblemType ProblemType = new ProblemType();
                    ProblemType.Code = CodeValue;
                    ProblemType.Name = NameValue;

                    ProblemTypes.Add(ProblemType);
                }
            }
            ProblemTypes = await ProblemTypeService.Import(ProblemTypes);
            if (ProblemTypes.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < ProblemTypes.Count; i++)
                {
                    ProblemType ProblemType = ProblemTypes[i];
                    if (!ProblemType.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (ProblemType.Errors.ContainsKey(nameof(ProblemType.Id)))
                            Error += ProblemType.Errors[nameof(ProblemType.Id)];
                        if (ProblemType.Errors.ContainsKey(nameof(ProblemType.Code)))
                            Error += ProblemType.Errors[nameof(ProblemType.Code)];
                        if (ProblemType.Errors.ContainsKey(nameof(ProblemType.Name)))
                            Error += ProblemType.Errors[nameof(ProblemType.Name)];
                        if (ProblemType.Errors.ContainsKey(nameof(ProblemType.StatusId)))
                            Error += ProblemType.Errors[nameof(ProblemType.StatusId)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        [Route(ProblemTypeRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ProblemType_ProblemTypeFilterDTO ProblemType_ProblemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region ProblemType
                var ProblemTypeFilter = ConvertFilterDTOToFilterEntity(ProblemType_ProblemTypeFilterDTO);
                ProblemTypeFilter.Skip = 0;
                ProblemTypeFilter.Take = int.MaxValue;
                ProblemTypeFilter = await ProblemTypeService.ToFilter(ProblemTypeFilter);
                List<ProblemType> ProblemTypes = await ProblemTypeService.List(ProblemTypeFilter);

                var ProblemTypeHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                    "StatusId",
                };
                List<object[]> ProblemTypeData = new List<object[]>();
                for (int i = 0; i < ProblemTypes.Count; i++)
                {
                    var ProblemType = ProblemTypes[i];
                    ProblemTypeData.Add(new Object[]
                    {
                        ProblemType.Id,
                        ProblemType.Code,
                        ProblemType.Name,
                        ProblemType.StatusId,
                    });
                }
                excel.GenerateWorksheet("ProblemType", ProblemTypeHeaders, ProblemTypeData);
                #endregion

                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "ProblemType.xlsx");
        }

        [Route(ProblemTypeRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] ProblemType_ProblemTypeFilterDTO ProblemType_ProblemTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region ProblemType
                var ProblemTypeHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                    "StatusId",
                };
                List<object[]> ProblemTypeData = new List<object[]>();
                excel.GenerateWorksheet("ProblemType", ProblemTypeHeaders, ProblemTypeData);
                #endregion

                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "ProblemType.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter();
            ProblemTypeFilter = await ProblemTypeService.ToFilter(ProblemTypeFilter);
            if (Id == 0)
            {

            }
            else
            {
                ProblemTypeFilter.Id = new IdFilter { Equal = Id };
                int count = await ProblemTypeService.Count(ProblemTypeFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ProblemType ConvertDTOToEntity(ProblemType_ProblemTypeDTO ProblemType_ProblemTypeDTO)
        {
            ProblemType_ProblemTypeDTO.TrimString();
            ProblemType ProblemType = new ProblemType();
            ProblemType.Id = ProblemType_ProblemTypeDTO.Id;
            ProblemType.Code = ProblemType_ProblemTypeDTO.Code;
            ProblemType.Name = ProblemType_ProblemTypeDTO.Name;
            ProblemType.StatusId = ProblemType_ProblemTypeDTO.StatusId;
            ProblemType.BaseLanguage = CurrentContext.Language;
            return ProblemType;
        }

        private ProblemTypeFilter ConvertFilterDTOToFilterEntity(ProblemType_ProblemTypeFilterDTO ProblemType_ProblemTypeFilterDTO)
        {
            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter();
            ProblemTypeFilter.Selects = ProblemTypeSelect.ALL;
            ProblemTypeFilter.Skip = ProblemType_ProblemTypeFilterDTO.Skip;
            ProblemTypeFilter.Take = ProblemType_ProblemTypeFilterDTO.Take;
            ProblemTypeFilter.OrderBy = ProblemType_ProblemTypeFilterDTO.OrderBy;
            ProblemTypeFilter.OrderType = ProblemType_ProblemTypeFilterDTO.OrderType;

            ProblemTypeFilter.Id = ProblemType_ProblemTypeFilterDTO.Id;
            ProblemTypeFilter.Code = ProblemType_ProblemTypeFilterDTO.Code;
            ProblemTypeFilter.Name = ProblemType_ProblemTypeFilterDTO.Name;
            ProblemTypeFilter.StatusId = ProblemType_ProblemTypeFilterDTO.StatusId;
            ProblemTypeFilter.CreatedAt = ProblemType_ProblemTypeFilterDTO.CreatedAt;
            ProblemTypeFilter.UpdatedAt = ProblemType_ProblemTypeFilterDTO.UpdatedAt;
            return ProblemTypeFilter;
        }
    }
}

