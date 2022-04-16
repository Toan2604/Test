using DMS.Common;
using DMS.Entities;
using DMS.Services.MAlbum;
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

namespace DMS.Rpc.album
{
    public class AlbumController : RpcController
    {
        private IStatusService StatusService;
        private IAlbumService AlbumService;
        private ICurrentContext CurrentContext;
        public AlbumController(
            IStatusService StatusService,
            IAlbumService AlbumService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.AlbumService = AlbumService;
            this.CurrentContext = CurrentContext;
        }

        [Route(AlbumRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Album_AlbumFilterDTO Album_AlbumFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AlbumFilter AlbumFilter = ConvertFilterDTOToFilterEntity(Album_AlbumFilterDTO);
            AlbumFilter = AlbumService.ToFilter(AlbumFilter);
            int count = await AlbumService.Count(AlbumFilter);
            return count;
        }

        [Route(AlbumRoute.List), HttpPost]
        public async Task<ActionResult<List<Album_AlbumDTO>>> List([FromBody] Album_AlbumFilterDTO Album_AlbumFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AlbumFilter AlbumFilter = ConvertFilterDTOToFilterEntity(Album_AlbumFilterDTO);
            AlbumFilter = AlbumService.ToFilter(AlbumFilter);
            List<Album> Albums = await AlbumService.List(AlbumFilter);
            List<Album_AlbumDTO> Album_AlbumDTOs = Albums
                .Select(c => new Album_AlbumDTO(c)).ToList();
            return Album_AlbumDTOs;
        }

        [Route(AlbumRoute.Get), HttpPost]
        public async Task<ActionResult<Album_AlbumDTO>> Get([FromBody] Album_AlbumDTO Album_AlbumDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Album_AlbumDTO.Id))
                return Forbid();

            Album Album = await AlbumService.Get(Album_AlbumDTO.Id);
            return new Album_AlbumDTO(Album);
        }

        [Route(AlbumRoute.Create), HttpPost]
        public async Task<ActionResult<Album_AlbumDTO>> Create([FromBody] Album_AlbumDTO Album_AlbumDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Album_AlbumDTO.Id))
                return Forbid();

            Album Album = ConvertDTOToEntity(Album_AlbumDTO);
            Album = await AlbumService.Create(Album);
            Album_AlbumDTO = new Album_AlbumDTO(Album);
            if (Album.IsValidated)
                return Album_AlbumDTO;
            else
                return BadRequest(Album_AlbumDTO);
        }

        [Route(AlbumRoute.Update), HttpPost]
        public async Task<ActionResult<Album_AlbumDTO>> Update([FromBody] Album_AlbumDTO Album_AlbumDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Album_AlbumDTO.Id))
                return Forbid();

            Album Album = ConvertDTOToEntity(Album_AlbumDTO);
            Album = await AlbumService.Update(Album);
            Album_AlbumDTO = new Album_AlbumDTO(Album);
            if (Album.IsValidated)
                return Album_AlbumDTO;
            else
                return BadRequest(Album_AlbumDTO);
        }

        [Route(AlbumRoute.Delete), HttpPost]
        public async Task<ActionResult<Album_AlbumDTO>> Delete([FromBody] Album_AlbumDTO Album_AlbumDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Album_AlbumDTO.Id))
                return Forbid();

            Album Album = ConvertDTOToEntity(Album_AlbumDTO);
            Album = await AlbumService.Delete(Album);
            Album_AlbumDTO = new Album_AlbumDTO(Album);
            if (Album.IsValidated)
                return Album_AlbumDTO;
            else
                return BadRequest(Album_AlbumDTO);
        }

        [Route(AlbumRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AlbumFilter AlbumFilter = new AlbumFilter();
            AlbumFilter = AlbumService.ToFilter(AlbumFilter);
            AlbumFilter.Id = new IdFilter { In = Ids };
            AlbumFilter.Selects = AlbumSelect.Id;
            AlbumFilter.Skip = 0;
            AlbumFilter.Take = int.MaxValue;

            List<Album> Albums = await AlbumService.List(AlbumFilter);
            Albums = await AlbumService.BulkDelete(Albums);
            if (Albums.Any(x => !x.IsValidated))
                return BadRequest(Albums.Where(x => !x.IsValidated));
            return true;
        }

        [Route(AlbumRoute.Import), HttpPost]
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
            List<Album> Albums = new List<Album>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(Albums);
                int StartColumn = 1;
                int StartRow = 1;
                int IdColumn = 0 + StartColumn;
                int NameColumn = 1 + StartColumn;
                int StatusIdColumn = 2 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i + StartRow, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i + StartRow, IdColumn].Value?.ToString();
                    string NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
                    string StatusIdValue = worksheet.Cells[i + StartRow, StatusIdColumn].Value?.ToString();

                    Album Album = new Album();
                    Album.Name = NameValue;
                    Status Status = Statuses.Where(x => x.Id.ToString() == StatusIdValue).FirstOrDefault();
                    Album.StatusId = Status == null ? 0 : Status.Id;
                    Album.Status = Status;

                    Albums.Add(Album);
                }
            }
            Albums = await AlbumService.Import(Albums);
            if (Albums.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < Albums.Count; i++)
                {
                    Album Album = Albums[i];
                    if (!Album.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (Album.Errors.ContainsKey(nameof(Album.Id)))
                            Error += Album.Errors[nameof(Album.Id)];
                        if (Album.Errors.ContainsKey(nameof(Album.Name)))
                            Error += Album.Errors[nameof(Album.Name)];
                        if (Album.Errors.ContainsKey(nameof(Album.StatusId)))
                            Error += Album.Errors[nameof(Album.StatusId)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        [Route(AlbumRoute.Export), HttpPost]
        public async Task<FileResult> Export([FromBody] Album_AlbumFilterDTO Album_AlbumFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region Album
                var AlbumFilter = ConvertFilterDTOToFilterEntity(Album_AlbumFilterDTO);
                AlbumFilter.Skip = 0;
                AlbumFilter.Take = int.MaxValue;
                AlbumFilter = AlbumService.ToFilter(AlbumFilter);
                List<Album> Albums = await AlbumService.List(AlbumFilter);

                var AlbumHeaders = new List<string>()
                {
                    "Id",
                    "Name",
                    "StatusId",
                };
                List<object[]> AlbumData = new List<object[]>();
                for (int i = 0; i < Albums.Count; i++)
                {
                    var Album = Albums[i];
                    AlbumData.Add(new Object[]
                    {
                        Album.Id,
                        Album.Name,
                        Album.StatusId,
                    });
                }
                excel.GenerateWorksheet("Album", AlbumHeaders, AlbumData);
                #endregion

                #region Status
                var StatusFilter = new StatusFilter();
                StatusFilter.Selects = StatusSelect.ALL;
                StatusFilter.OrderBy = StatusOrder.Id;
                StatusFilter.OrderType = OrderType.ASC;
                StatusFilter.Skip = 0;
                StatusFilter.Take = int.MaxValue;
                List<Status> Statuses = await StatusService.List(StatusFilter);

                var StatusHeaders = new List<string>()
                {
                        "Id",
                        "Code",
                        "Name",
                };
                List<object[]> StatusData = new List<object[]>();
                for (int i = 0; i < Statuses.Count; i++)
                {
                    var Status = Statuses[i];
                    StatusData.Add(new Object[]
                    {
                        Status.Id,
                        Status.Code,
                        Status.Name,
                    });
                }
                excel.GenerateWorksheet("Status", StatusHeaders, StatusData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "Album.xlsx");
        }

        [Route(AlbumRoute.ExportTemplate), HttpPost]
        public async Task<FileResult> ExportTemplate([FromBody] Album_AlbumFilterDTO Album_AlbumFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region Album
                var AlbumHeaders = new List<string>()
                {
                        "Id",
                        "Name",
                        "StatusId",
                };
                List<object[]> AlbumData = new List<object[]>();
                excel.GenerateWorksheet("Album", AlbumHeaders, AlbumData);
                #endregion

                #region Status
                var StatusFilter = new StatusFilter();
                StatusFilter.Selects = StatusSelect.ALL;
                StatusFilter.OrderBy = StatusOrder.Id;
                StatusFilter.OrderType = OrderType.ASC;
                StatusFilter.Skip = 0;
                StatusFilter.Take = int.MaxValue;
                List<Status> Statuses = await StatusService.List(StatusFilter);

                var StatusHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                };
                List<object[]> StatusData = new List<object[]>();
                for (int i = 0; i < Statuses.Count; i++)
                {
                    var Status = Statuses[i];
                    StatusData.Add(new Object[]
                    {
                        Status.Id,
                        Status.Code,
                        Status.Name,
                    });
                }
                excel.GenerateWorksheet("Status", StatusHeaders, StatusData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "Album.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            AlbumFilter AlbumFilter = new AlbumFilter();
            AlbumFilter = AlbumService.ToFilter(AlbumFilter);
            if (Id == 0)
            {

            }
            else
            {
                AlbumFilter.Id = new IdFilter { Equal = Id };
                int count = await AlbumService.Count(AlbumFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Album ConvertDTOToEntity(Album_AlbumDTO Album_AlbumDTO)
        {
            Album_AlbumDTO.TrimString();
            Album Album = new Album();
            Album.Id = Album_AlbumDTO.Id;
            Album.Name = Album_AlbumDTO.Name;
            Album.StatusId = Album_AlbumDTO.StatusId;
            Album.Status = Album_AlbumDTO.Status == null ? null : new Status
            {
                Id = Album_AlbumDTO.Status.Id,
                Code = Album_AlbumDTO.Status.Code,
                Name = Album_AlbumDTO.Status.Name,
            };
            Album.BaseLanguage = CurrentContext.Language;
            return Album;
        }

        private AlbumFilter ConvertFilterDTOToFilterEntity(Album_AlbumFilterDTO Album_AlbumFilterDTO)
        {
            AlbumFilter AlbumFilter = new AlbumFilter();
            AlbumFilter.Selects = AlbumSelect.ALL;
            AlbumFilter.Skip = Album_AlbumFilterDTO.Skip;
            AlbumFilter.Take = Album_AlbumFilterDTO.Take;
            AlbumFilter.OrderBy = Album_AlbumFilterDTO.OrderBy;
            AlbumFilter.OrderType = Album_AlbumFilterDTO.OrderType;

            AlbumFilter.Id = Album_AlbumFilterDTO.Id;
            AlbumFilter.Name = Album_AlbumFilterDTO.Name;
            AlbumFilter.StatusId = Album_AlbumFilterDTO.StatusId;
            AlbumFilter.CreatedAt = Album_AlbumFilterDTO.CreatedAt;
            AlbumFilter.UpdatedAt = Album_AlbumFilterDTO.UpdatedAt;
            return AlbumFilter;
        }

        [Route(AlbumRoute.FilterListStatus), HttpPost]
        public async Task<List<Album_StatusDTO>> FilterListStatus([FromBody] Album_StatusFilterDTO Album_StatusFilterDTO)
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
            List<Album_StatusDTO> Album_StatusDTOs = Statuses
                .Select(x => new Album_StatusDTO(x)).ToList();
            return Album_StatusDTOs;
        }

        [Route(AlbumRoute.SingleListStatus), HttpPost]
        public async Task<List<Album_StatusDTO>> SingleListStatus([FromBody] Album_StatusFilterDTO Album_StatusFilterDTO)
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
            List<Album_StatusDTO> Album_StatusDTOs = Statuses
                .Select(x => new Album_StatusDTO(x)).ToList();
            return Album_StatusDTOs;
        }

    }
}

