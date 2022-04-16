using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MAppUser;
using DMS.Services.MImage;
using DMS.Services.MLuckyDraw;
using DMS.Services.MLuckyDrawNumber;
using DMS.Services.MLuckyDrawRegistration;
using DMS.Services.MLuckyDrawStructure;
using DMS.Services.MLuckyDrawType;
using DMS.Services.MLuckyDrawWinner;
using DMS.Services.MOrganization;
using DMS.Services.MProvince;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.lucky_draw
{
    public partial class LuckyDrawController : RpcController
    {
        private IImageService ImageService;
        private ILuckyDrawTypeService LuckyDrawTypeService;
        private IOrganizationService OrganizationService;
        private IProvinceService ProvinceService;
        private IStatusService StatusService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreService StoreService;
        private IStoreTypeService StoreTypeService;
        private ILuckyDrawNumberService LuckyDrawNumberService;
        private ILuckyDrawStructureService LuckyDrawStructureService;
        private ILuckyDrawRegistrationService LuckyDrawRegistrationService;
        private ILuckyDrawWinnerService LuckyDrawWinnerService;
        private IAppUserService AppUserService;
        private ILuckyDrawService LuckyDrawService;
        private ICurrentContext CurrentContext;
        private IUOW UOW;
        public LuckyDrawController(
            IImageService ImageService,
            ILuckyDrawTypeService LuckyDrawTypeService,
            IOrganizationService OrganizationService,
            IProvinceService ProvinceService,
            IStatusService StatusService,
            IStoreGroupingService StoreGroupingService,
            IStoreService StoreService,
            IStoreTypeService StoreTypeService,
            ILuckyDrawNumberService LuckyDrawNumberService,
            ILuckyDrawStructureService LuckyDrawStructureService,
            ILuckyDrawRegistrationService LuckyDrawRegistrationService,
            ILuckyDrawWinnerService LuckyDrawWinnerService,
            IAppUserService AppUserService,
            ILuckyDrawService LuckyDrawService,
            ICurrentContext CurrentContext,
            IUOW UOW
        )
        {
            this.ImageService = ImageService;
            this.LuckyDrawTypeService = LuckyDrawTypeService;
            this.OrganizationService = OrganizationService;
            this.ProvinceService = ProvinceService;
            this.StatusService = StatusService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreService = StoreService;
            this.StoreTypeService = StoreTypeService;
            this.LuckyDrawNumberService = LuckyDrawNumberService;
            this.LuckyDrawStructureService = LuckyDrawStructureService;
            this.LuckyDrawRegistrationService = LuckyDrawRegistrationService;
            this.LuckyDrawWinnerService = LuckyDrawWinnerService;
            this.AppUserService = AppUserService;
            this.LuckyDrawService = LuckyDrawService;
            this.CurrentContext = CurrentContext;
            this.UOW = UOW;
        }

        [Route(LuckyDrawRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            int count = await LuckyDrawService.Count(LuckyDrawFilter);
            return count;
        }

        [Route(LuckyDrawRoute.List), HttpPost]
        public async Task<ActionResult<List<LuckyDraw_LuckyDrawDTO>>> List([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.List(LuckyDrawFilter);
            List<LuckyDraw_LuckyDrawDTO> LuckyDraw_LuckyDrawDTOs = LuckyDraws
                .Select(c => new LuckyDraw_LuckyDrawDTO(c)).ToList();
            return LuckyDraw_LuckyDrawDTOs;
        }

        [Route(LuckyDrawRoute.Get), HttpPost]
        public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> Get([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyDraw_LuckyDrawDTO.Id))
                return Forbid();

            LuckyDraw LuckyDraw = await LuckyDrawService.Get(LuckyDraw_LuckyDrawDTO.Id);
            return new LuckyDraw_LuckyDrawDTO(LuckyDraw);
        }

        [Route(LuckyDrawRoute.Create), HttpPost]
        public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> Create([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyDraw_LuckyDrawDTO.Id))
                return Forbid();

            LuckyDraw LuckyDraw = ConvertDTOToEntity(LuckyDraw_LuckyDrawDTO);
            LuckyDraw = await LuckyDrawService.Create(LuckyDraw);
            LuckyDraw_LuckyDrawDTO = new LuckyDraw_LuckyDrawDTO(LuckyDraw);
            if (LuckyDraw.IsValidated)
                return LuckyDraw_LuckyDrawDTO;
            else
                return BadRequest(LuckyDraw_LuckyDrawDTO);
        }

        [Route(LuckyDrawRoute.Update), HttpPost]
        public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> Update([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyDraw_LuckyDrawDTO.Id))
                return Forbid();

            LuckyDraw LuckyDraw = ConvertDTOToEntity(LuckyDraw_LuckyDrawDTO);
            LuckyDraw = await LuckyDrawService.Update(LuckyDraw);
            LuckyDraw_LuckyDrawDTO = new LuckyDraw_LuckyDrawDTO(LuckyDraw);
            if (LuckyDraw.IsValidated)
                return LuckyDraw_LuckyDrawDTO;
            else
                return BadRequest(LuckyDraw_LuckyDrawDTO);
        }

        [Route(LuckyDrawRoute.Delete), HttpPost]
        public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> Delete([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(LuckyDraw_LuckyDrawDTO.Id))
                return Forbid();

            LuckyDraw LuckyDraw = ConvertDTOToEntity(LuckyDraw_LuckyDrawDTO);
            LuckyDraw = await LuckyDrawService.Delete(LuckyDraw);
            LuckyDraw_LuckyDrawDTO = new LuckyDraw_LuckyDrawDTO(LuckyDraw);
            if (LuckyDraw.IsValidated)
                return LuckyDraw_LuckyDrawDTO;
            else
                return BadRequest(LuckyDraw_LuckyDrawDTO);
        }

        [Route(LuckyDrawRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter();
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.Id = new IdFilter { In = Ids };
            LuckyDrawFilter.Selects = LuckyDrawSelect.Id;
            LuckyDrawFilter.Skip = 0;
            LuckyDrawFilter.Take = int.MaxValue;

            List<LuckyDraw> LuckyDraws = await LuckyDrawService.List(LuckyDrawFilter);
            LuckyDraws = await LuckyDrawService.BulkDelete(LuckyDraws);
            if (LuckyDraws.Any(x => !x.IsValidated))
                return BadRequest(LuckyDraws.Where(x => !x.IsValidated));
            return true;
        }
        [Route(LuckyDrawRoute.LoadImage), HttpPost]
        public async Task<ActionResult<bool>> LoadImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Image Image = new Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray()
            };
            Image = await LuckyDrawService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            LuckyDraw_ImageDTO LuckyDraw_ImageDTO = new LuckyDraw_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
                ThumbnailUrl = Image.ThumbnailUrl,
            };
            return Ok(LuckyDraw_ImageDTO);
        }

        [Route(LuckyDrawRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            ImageFilter ImageFilter = new ImageFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ImageSelect.ALL
            };
            List<Image> Images = await ImageService.List(ImageFilter);
            LuckyDrawTypeFilter LuckyDrawTypeFilter = new LuckyDrawTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = LuckyDrawTypeSelect.ALL
            };
            List<LuckyDrawType> LuckyDrawTypes = await LuckyDrawTypeService.List(LuckyDrawTypeFilter);
            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.ALL
            };
            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<LuckyDraw> LuckyDraws = new List<LuckyDraw>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(LuckyDraws);
                int StartColumn = 1;
                int StartRow = 1;
                int IdColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int LuckyDrawTypeIdColumn = 3 + StartColumn;
                int OrganizationIdColumn = 4 + StartColumn;
                int RevenuePerTurnColumn = 5 + StartColumn;
                int StartAtColumn = 6 + StartColumn;
                int EndAtColumn = 7 + StartColumn;
                int ImageIdColumn = 8 + StartColumn;
                int StatusIdColumn = 9 + StartColumn;
                int UsedColumn = 10 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i, IdColumn].Value?.ToString();
                    string CodeValue = worksheet.Cells[i, CodeColumn].Value?.ToString();
                    string NameValue = worksheet.Cells[i, NameColumn].Value?.ToString();
                    string LuckyDrawTypeIdValue = worksheet.Cells[i, LuckyDrawTypeIdColumn].Value?.ToString();
                    string OrganizationIdValue = worksheet.Cells[i, OrganizationIdColumn].Value?.ToString();
                    string RevenuePerTurnValue = worksheet.Cells[i, RevenuePerTurnColumn].Value?.ToString();
                    string StartAtValue = worksheet.Cells[i, StartAtColumn].Value?.ToString();
                    string EndAtValue = worksheet.Cells[i, EndAtColumn].Value?.ToString();
                    string ImageIdValue = worksheet.Cells[i, ImageIdColumn].Value?.ToString();
                    string StatusIdValue = worksheet.Cells[i, StatusIdColumn].Value?.ToString();
                    string UsedValue = worksheet.Cells[i, UsedColumn].Value?.ToString();

                    LuckyDraw LuckyDraw = new LuckyDraw();
                    LuckyDraw.Code = CodeValue;
                    LuckyDraw.Name = NameValue;
                    LuckyDraw.RevenuePerTurn = decimal.TryParse(RevenuePerTurnValue, out decimal RevenuePerTurn) ? RevenuePerTurn : 0;
                    LuckyDraw.StartAt = DateTime.TryParse(StartAtValue, out DateTime StartAt) ? StartAt : DateTime.Now;
                    LuckyDraw.EndAt = DateTime.TryParse(EndAtValue, out DateTime EndAt) ? EndAt : DateTime.Now;
                    LuckyDraw.Used = bool.TryParse(UsedValue, out bool Used) ? Used : false;
                    Image Image = Images.Where(x => x.Id.ToString() == ImageIdValue).FirstOrDefault();
                    LuckyDraw.ImageId = Image == null ? 0 : Image.Id;
                    LuckyDraw.Image = Image;
                    LuckyDrawType LuckyDrawType = LuckyDrawTypes.Where(x => x.Id.ToString() == LuckyDrawTypeIdValue).FirstOrDefault();
                    LuckyDraw.LuckyDrawTypeId = LuckyDrawType == null ? 0 : LuckyDrawType.Id;
                    LuckyDraw.LuckyDrawType = LuckyDrawType;
                    Status Status = Statuses.Where(x => x.Id.ToString() == StatusIdValue).FirstOrDefault();
                    LuckyDraw.StatusId = Status == null ? 0 : Status.Id;
                    LuckyDraw.Status = Status;

                    LuckyDraws.Add(LuckyDraw);
                }
            }
            LuckyDraws = await LuckyDrawService.Import(LuckyDraws);
            if (LuckyDraws.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < LuckyDraws.Count; i++)
                {
                    LuckyDraw LuckyDraw = LuckyDraws[i];
                    if (!LuckyDraw.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.Id)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.Id)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.Code)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.Code)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.Name)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.Name)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.LuckyDrawTypeId)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.LuckyDrawTypeId)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.OrganizationId)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.OrganizationId)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.RevenuePerTurn)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.RevenuePerTurn)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.StartAt)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.StartAt)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.EndAt)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.EndAt)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.ImageId)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.ImageId)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.StatusId)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.StatusId)];
                        if (LuckyDraw.Errors.ContainsKey(nameof(LuckyDraw.Used)))
                            Error += LuckyDraw.Errors[nameof(LuckyDraw.Used)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        //xuất excel báo cáo dung lượng giải thưởng
        [Route(LuckyDrawRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.Skip = 0;
            LuckyDrawFilter.Take = int.MaxValue;
            LuckyDrawFilter.Selects = LuckyDrawSelect.Id;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.Export(LuckyDrawFilter);
            List<long> LuckyDrawIds = LuckyDraws.Select(x => x.Id).ToList();
            LuckyDrawStructureFilter LuckyDrawStructureFilter = new LuckyDrawStructureFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawStructureSelect.ALL,
                LuckyDrawId = new IdFilter { In = LuckyDrawIds },
                OrderBy = LuckyDrawStructureOrder.LuckyDraw | LuckyDrawStructureOrder.Id,
                OrderType = OrderType.ASC
            };
            List<LuckyDrawStructure> LuckyDrawStructures = await LuckyDrawStructureService.List(LuckyDrawStructureFilter);
            List<long> LuckyDrawStructureIds = LuckyDrawStructures.Select(x => x.Id).ToList();
            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawWinnerSelect.Id | LuckyDrawWinnerSelect.LuckyDrawRegistration | LuckyDrawWinnerSelect.LuckyDrawStructure | LuckyDrawWinnerSelect.Time | LuckyDrawWinnerSelect.LuckyDrawNumber,
                LuckyDrawId = new IdFilter { In = LuckyDrawIds },
                Used = true
            };
            List<LuckyDrawWinner> LuckyDrawWinners = await LuckyDrawWinnerService.List(LuckyDrawWinnerFilter);
            List<LuckyDraw_LuckyDrawStructureExportDTO> LuckyDraw_LuckyDrawStructureExportDTOs = LuckyDrawStructures.Select(x => new LuckyDraw_LuckyDrawStructureExportDTO(x)).ToList();
            LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter = new LuckyDrawRegistrationFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawRegistrationSelect.Id | LuckyDrawRegistrationSelect.TurnCounter | LuckyDrawRegistrationSelect.Store | LuckyDrawRegistrationSelect.LuckyDraw,
                OrderBy = LuckyDrawRegistrationOrder.Store,
                OrderType = OrderType.ASC,
                LuckyDrawId = new IdFilter { In = LuckyDrawIds },
            };
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await LuckyDrawRegistrationService.List(LuckyDrawRegistrationFilter);
            List<LuckyDraw_LuckyDrawRegistrationExportDTO> LuckyDraw_LuckyDrawRegistrationExportDTOs = LuckyDrawRegistrations.Select(x => new LuckyDraw_LuckyDrawRegistrationExportDTO(x)).ToList();

            LuckyDrawNumberFilter LuckydrawNumberFilter = new LuckyDrawNumberFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawNumberSelect.Id,
                LuckyDrawStructureId = new IdFilter { In = LuckyDrawStructureIds }
            };
            List<LuckyDrawNumber> LuckyDrawNumbers = await LuckyDrawNumberService.List(LuckydrawNumberFilter);
            List<long> LuckyDrawNumberIds = LuckyDrawNumbers.Select(x => x.Id).ToList();
            LuckyDrawNumbers = await LuckyDrawNumberService.Export(LuckyDrawNumberIds);
            List<LuckyDraw_LuckyDrawNumberExportDTO> LuckyDraw_LuckyDrawNumberExportDTOs = LuckyDrawNumbers.Select(x => new LuckyDraw_LuckyDrawNumberExportDTO(x)).ToList();
            LuckyDraw_LuckyDrawNumberExportDTOs = LuckyDraw_LuckyDrawNumberExportDTOs.OrderBy(x => x.Id).ToList();
            long stt = 1;

            foreach (LuckyDraw_LuckyDrawStructureExportDTO LuckyDraw_LuckyDrawStructureExportDTO in LuckyDraw_LuckyDrawStructureExportDTOs)
            {
                LuckyDraw_LuckyDrawStructureExportDTO.STT = stt++;
                var GivenPrizeCounter = LuckyDrawWinners.Count(x => x.LuckyDrawStructureId == LuckyDraw_LuckyDrawStructureExportDTO.Id);
                LuckyDraw_LuckyDrawStructureExportDTO.GivenPrizeCounter = GivenPrizeCounter;
                LuckyDraw_LuckyDrawStructureExportDTO.RemainingPrizeCounter = LuckyDraw_LuckyDrawStructureExportDTO.Quantity - GivenPrizeCounter;
            }
            long stt1 = 1;
            foreach (var LuckyDraw_LuckyDrawRegistrationExportDTO in LuckyDraw_LuckyDrawRegistrationExportDTOs)
            {
                var GivenPrizeCounter = LuckyDrawWinners.Count(x => x.LuckyDrawRegistrationId == LuckyDraw_LuckyDrawRegistrationExportDTO.Id);
                LuckyDraw_LuckyDrawRegistrationExportDTO.RemainingTurnCounter = LuckyDraw_LuckyDrawRegistrationExportDTO.TurnCounter - GivenPrizeCounter;
            }
            LuckyDraw_LuckyDrawRegistrationExportDTOs = LuckyDraw_LuckyDrawRegistrationExportDTOs.Where(x => x.RemainingTurnCounter > 0).GroupBy(x => new { x.LuckyDrawId, x.StoreId })
            .Select(x => new LuckyDraw_LuckyDrawRegistrationExportDTO
            {
                LuckyDrawId = x.Key.LuckyDrawId,
                StoreId = x.Key.StoreId,
                TurnCounter = x.Sum(x => x.TurnCounter),
                RemainingTurnCounter = x.Sum(x => x.RemainingTurnCounter),
                Store = x.Select(x => x.Store).FirstOrDefault(),
                LuckyDraw = x.Select(x => x.LuckyDraw).FirstOrDefault()
            }).ToList();
            foreach (var LuckyDraw_LuckyDrawRegistrationExportDTO in LuckyDraw_LuckyDrawRegistrationExportDTOs)
            {
                LuckyDraw_LuckyDrawRegistrationExportDTO.STT = stt1++;
            }
            long stt2 = 1;
            foreach (LuckyDraw_LuckyDrawNumberExportDTO LuckyDraw_LuckyDrawNumberExportDTO in LuckyDraw_LuckyDrawNumberExportDTOs)
            {
                LuckyDraw_LuckyDrawNumberExportDTO.STT = stt2++;
                LuckyDraw_LuckyDrawNumberExportDTO.UsedString = LuckyDraw_LuckyDrawNumberExportDTO.Used ? "Đã quay" : "Chưa quay";
                if (LuckyDraw_LuckyDrawNumberExportDTO.Used)
                {
                    LuckyDraw_LuckyDrawNumberExportDTO.TimeString = LuckyDrawWinners.Where(x => x.LuckyDrawNumberId == LuckyDraw_LuckyDrawNumberExportDTO.Id).FirstOrDefault()?.Time.ToString("dd-MM-yyyy");
                }
            }
            string path = "Templates/LuckyDraw_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Data = LuckyDraw_LuckyDrawStructureExportDTOs;
            Data.Data1 = LuckyDraw_LuckyDrawRegistrationExportDTOs;
            Data.Data2 = LuckyDraw_LuckyDrawNumberExportDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "LuckyDraw_Export.xlsx");
        }

        [Route(LuckyDrawRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            string path = "Templates/LuckyDraw_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "LuckyDraw.xlsx");
        }

        [Route(LuckyDrawRoute.ImportStore), HttpPost]
        public async Task<ActionResult<List<LuckyDraw_LuckyDrawStoreMappingDTO>>> ImportStore([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest(new { message = "Định dạng file không hợp lệ" });
            LuckyDraw LuckyDraw = new LuckyDraw();
            List<Store> Stores = await StoreService.List(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.CodeDraft | StoreSelect.Name,
                OrderBy = StoreOrder.Id,
                OrderType = OrderType.ASC,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });
            StringBuilder errorContent = new StringBuilder();
            LuckyDraw.LuckyDrawStoreMappings = new List<LuckyDrawStoreMapping>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Daily_Apdung"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 1;
                int SttColumnn = 0 + StartColumn;
                int StoreCodeColumn = 1 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string stt = worksheet.Cells[i + StartRow, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;

                    string StoreCodeValue = worksheet.Cells[i + StartRow, StoreCodeColumn].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(StoreCodeValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Chưa nhập mã đại lý");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(StoreCodeValue) && i == worksheet.Dimension.End.Row)
                        break;

                    var Store = Stores.Where(x => x.Code == StoreCodeValue).FirstOrDefault();
                    if (Store == null)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Mã đại lý không tồn tại");
                        continue;
                    }
                    LuckyDrawStoreMapping LuckyDrawStoreMapping = LuckyDraw.LuckyDrawStoreMappings.Where(x => x.StoreId == Store.Id).FirstOrDefault();
                    if (LuckyDrawStoreMapping == null)
                    {
                        LuckyDrawStoreMapping = new LuckyDrawStoreMapping();
                        LuckyDrawStoreMapping.StoreId = Store.Id;
                        LuckyDrawStoreMapping.Store = Store;
                        LuckyDraw.LuckyDrawStoreMappings.Add(LuckyDrawStoreMapping);
                    }
                }
                if (errorContent.Length > 0)
                    return BadRequest(errorContent.ToString());
            }

            List<LuckyDraw_LuckyDrawStoreMappingDTO> LuckyDraw_LuckyDrawStoreMappingDTOs = LuckyDraw.LuckyDrawStoreMappings
                 .Select(c => new LuckyDraw_LuckyDrawStoreMappingDTO(c)).ToList();
            return LuckyDraw_LuckyDrawStoreMappingDTOs;
        }

        [Route(LuckyDrawRoute.ExportStore), HttpPost]
        public async Task<ActionResult> ExportStore([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long LuckyDrawId = LuckyDraw_LuckyDrawDTO?.Id ?? 0;
            LuckyDraw LuckyDraw = await LuckyDrawService.Get(LuckyDrawId);
            if (LuckyDraw == null)
                return null;

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                var LuckyDrawStoreMappingHeader = new List<string>()
                {
                    "STT",
                    "Mã đại lý",
                    "Tên đại lý"
                };
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < LuckyDraw.LuckyDrawStoreMappings.Count; i++)
                {
                    var LuckyDrawStoreMapping = LuckyDraw.LuckyDrawStoreMappings[i];
                    data.Add(new Object[]
                    {
                        i + 1,
                        LuckyDrawStoreMapping.Store?.Code,
                        LuckyDrawStoreMapping.Store?.Name,
                    });
                }
                excel.GenerateWorksheet("Daily_QuayThuong", LuckyDrawStoreMappingHeader, data);

                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", $"{LuckyDraw.Code}_Store.xlsx");
        }

        [Route(LuckyDrawRoute.ExportTemplateStore), HttpPost]
        public async Task<ActionResult> ExportTemplateStore()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var appUser = await AppUserService.Get(CurrentContext.UserId);
            var StoreFilter = new StoreFilter
            {
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name,
                Skip = 0,
                Take = int.MaxValue,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                OrganizationId = new IdFilter { Equal = appUser.OrganizationId }
            };
            var Stores = await StoreService.List(StoreFilter);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string tempPath = "Templates/LuckyDraw_Store.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(tempPath);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            using (var xlPackage = new ExcelPackage(input))
            {
                var worksheet = xlPackage.Workbook.Worksheets["Daily"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow = 2;
                int numberCell = 1;
                for (var i = 0; i < Stores.Count; i++)
                {
                    Store Store = Stores[i];
                    worksheet.Cells[startRow + i, numberCell].Value = Store.Code;
                    worksheet.Cells[startRow + i, numberCell + 1].Value = Store.Name;
                }
                xlPackage.SaveAs(output);
            }
            return File(output.ToArray(), "application/octet-stream", "Template_LuckyDraw_Store.xlsx");
        }
        [Route(LuckyDrawRoute.ExportWinnerStore), HttpPost]
        public async Task<ActionResult> ExportWinnerStore([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.Skip = 0;
            LuckyDrawFilter.Take = int.MaxValue;
            LuckyDrawFilter.Selects = LuckyDrawSelect.Id;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.Export(LuckyDrawFilter);
            List<long> LuckyDrawIds = LuckyDraws.Select(x => x.Id).ToList();
            LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter = new LuckyDrawRegistrationFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawRegistrationSelect.ALL,
                OrderBy = LuckyDrawRegistrationOrder.Store,
                OrderType = OrderType.ASC,
                LuckyDrawId = new IdFilter { In = LuckyDrawIds },
            };
            List<long> LuckyDrawRegistrationIds = (await LuckyDrawRegistrationService.List(LuckyDrawRegistrationFilter)).Select(x => x.Id).ToList();
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(LuckyDrawRegistrationIds);

            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawWinnerSelect.Id,
                LuckyDrawId = new IdFilter { In = LuckyDrawIds },
                Used = true,
            };
            List<long> LuckyDrawWinnerIds = (await LuckyDrawWinnerService.List(LuckyDrawWinnerFilter)).Select(x => x.Id).ToList();
            List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(LuckyDrawWinnerIds);
            List<LuckyDraw_LuckyDrawRegistrationExportDTO> LuckyDraw_LuckyDrawRegistrationExportDTOs = LuckyDrawRegistrations?
                .Select(x => new LuckyDraw_LuckyDrawRegistrationExportDTO(x))
                .OrderBy(x => x.Store.Code)
                .ToList();
            long stt = 1;
            foreach (var LuckyDraw_LuckyDrawRegistrationExportDTO in LuckyDraw_LuckyDrawRegistrationExportDTOs)
            {
                LuckyDraw_LuckyDrawRegistrationExportDTO.LuckyDrawWinners = LuckyDrawWinners
                    .Where(x => x.LuckyDrawRegistrationId == LuckyDraw_LuckyDrawRegistrationExportDTO.Id)?
                    .Select(x => new LuckyDraw_LuckyDrawWinnerExportDTO(x))
                    .ToList();
            }
            LuckyDraw_LuckyDrawRegistrationExportDTOs = LuckyDraw_LuckyDrawRegistrationExportDTOs.Where(x => x.LuckyDrawWinners != null && x.LuckyDrawWinners.Any()).ToList();
            foreach (var LuckyDraw_LuckyDrawRegistrationExportDTO in LuckyDraw_LuckyDrawRegistrationExportDTOs)
            {
                LuckyDraw_LuckyDrawRegistrationExportDTO.STT = stt;
                stt++;
                LuckyDraw_LuckyDrawRegistrationExportDTO.LuckyDrawWinners.ForEach(x =>
                {
                    x.IsDrawnByStoreString = LuckyDraw_LuckyDrawRegistrationExportDTO.IsDrawnByStore ? "x" : "";
                    x.TimeString = x.Time.Date.ToString("dd-MM-yyyy");
                });
            }
            string path = "Templates/LuckyDrawWinner_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Data = LuckyDraw_LuckyDrawRegistrationExportDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "LuckyDrawWinner_Export.xlsx");
        }
        private async Task<bool> HasPermission(long Id)
        {
            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter();
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            if (Id == 0)
            {

            }
            else
            {
                LuckyDrawFilter.Id = new IdFilter { Equal = Id };
                int count = await LuckyDrawService.Count(LuckyDrawFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }
        private LuckyDraw ConvertDTOToEntity(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            LuckyDraw_LuckyDrawDTO.TrimString();
            LuckyDraw LuckyDraw = new LuckyDraw();
            LuckyDraw.Id = LuckyDraw_LuckyDrawDTO.Id;
            LuckyDraw.Code = LuckyDraw_LuckyDrawDTO.Code;
            LuckyDraw.Name = LuckyDraw_LuckyDrawDTO.Name;
            LuckyDraw.LuckyDrawTypeId = LuckyDraw_LuckyDrawDTO.LuckyDrawTypeId;
            LuckyDraw.OrganizationId = LuckyDraw_LuckyDrawDTO.OrganizationId;
            LuckyDraw.RevenuePerTurn = LuckyDraw_LuckyDrawDTO.RevenuePerTurn;
            LuckyDraw.StartAt = LuckyDraw_LuckyDrawDTO.StartAt ?? default(DateTime);
            LuckyDraw.EndAt = LuckyDraw_LuckyDrawDTO.EndAt ?? default(DateTime);
            LuckyDraw.ImageId = LuckyDraw_LuckyDrawDTO.ImageId;
            LuckyDraw.AvatarImageId = LuckyDraw_LuckyDrawDTO.AvatarImageId;
            LuckyDraw.Description = LuckyDraw_LuckyDrawDTO.Description;
            LuckyDraw.StatusId = LuckyDraw_LuckyDrawDTO.StatusId;
            LuckyDraw.Used = LuckyDraw_LuckyDrawDTO.Used;
            LuckyDraw.Image = LuckyDraw_LuckyDrawDTO.Image == null ? null : new Image
            {
                Id = LuckyDraw_LuckyDrawDTO.Image.Id,
                Name = LuckyDraw_LuckyDrawDTO.Image.Name,
                Url = LuckyDraw_LuckyDrawDTO.Image.Url,
                ThumbnailUrl = LuckyDraw_LuckyDrawDTO.Image.ThumbnailUrl,
            };
            LuckyDraw.AvatarImage = LuckyDraw_LuckyDrawDTO.AvatarImage == null ? null : new Image
            {
                Id = LuckyDraw_LuckyDrawDTO.AvatarImage.Id,
                Name = LuckyDraw_LuckyDrawDTO.AvatarImage.Name,
                Url = LuckyDraw_LuckyDrawDTO.AvatarImage.Url,
                ThumbnailUrl = LuckyDraw_LuckyDrawDTO.AvatarImage.ThumbnailUrl,
            };
            LuckyDraw.LuckyDrawType = LuckyDraw_LuckyDrawDTO.LuckyDrawType == null ? null : new LuckyDrawType
            {
                Id = LuckyDraw_LuckyDrawDTO.LuckyDrawType.Id,
                Code = LuckyDraw_LuckyDrawDTO.LuckyDrawType.Code,
                Name = LuckyDraw_LuckyDrawDTO.LuckyDrawType.Name,
            };
            LuckyDraw.Organization = LuckyDraw_LuckyDrawDTO.Organization == null ? null : new Organization
            {
                Id = LuckyDraw_LuckyDrawDTO.Organization.Id,
                Code = LuckyDraw_LuckyDrawDTO.Organization.Code,
                Name = LuckyDraw_LuckyDrawDTO.Organization.Name,
                ParentId = LuckyDraw_LuckyDrawDTO.Organization.ParentId,
                Path = LuckyDraw_LuckyDrawDTO.Organization.Path,
                Level = LuckyDraw_LuckyDrawDTO.Organization.Level,
                StatusId = LuckyDraw_LuckyDrawDTO.Organization.StatusId,
                Phone = LuckyDraw_LuckyDrawDTO.Organization.Phone,
                Email = LuckyDraw_LuckyDrawDTO.Organization.Email,
                Address = LuckyDraw_LuckyDrawDTO.Organization.Address,
                IsDisplay = LuckyDraw_LuckyDrawDTO.Organization.IsDisplay,
            };
            LuckyDraw.Status = LuckyDraw_LuckyDrawDTO.Status == null ? null : new Status
            {
                Id = LuckyDraw_LuckyDrawDTO.Status.Id,
                Code = LuckyDraw_LuckyDrawDTO.Status.Code,
                Name = LuckyDraw_LuckyDrawDTO.Status.Name,
            };
            LuckyDraw.LuckyDrawStoreGroupingMappings = LuckyDraw_LuckyDrawDTO.LuckyDrawStoreGroupingMappings?
                .Select(x => new LuckyDrawStoreGroupingMapping
                {
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                        Used = x.StoreGrouping.Used,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStoreMappings = LuckyDraw_LuckyDrawDTO.LuckyDrawStoreMappings?
                .Select(x => new LuckyDrawStoreMapping
                {
                    StoreId = x.StoreId,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        UnsignName = x.Store.UnsignName,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        UnsignAddress = x.Store.UnsignAddress,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        TaxCode = x.Store.TaxCode,
                        LegalEntity = x.Store.LegalEntity,
                        CreatorId = x.Store.CreatorId,
                        AppUserId = x.Store.AppUserId,
                        StatusId = x.Store.StatusId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                        IsStoreApprovalDirectSalesOrder = x.Store.IsStoreApprovalDirectSalesOrder,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStoreTypeMappings = LuckyDraw_LuckyDrawDTO.LuckyDrawStoreTypeMappings?
                .Select(x => new LuckyDrawStoreTypeMapping
                {
                    StoreTypeId = x.StoreTypeId,
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        Used = x.StoreType.Used,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStructures = LuckyDraw_LuckyDrawDTO.LuckyDrawStructures?
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity,
                }).ToList();
            LuckyDraw.LuckyDrawWinners = LuckyDraw_LuckyDrawDTO.LuckyDrawWinners?
                .Select(x => new LuckyDrawWinner
                {
                    Id = x.Id,
                    LuckyDrawStructureId = x.LuckyDrawStructureId,
                    LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                    Time = x.Time,
                    LuckyDrawRegistration = x.LuckyDrawRegistration == null ? null : new LuckyDrawRegistration
                    {
                        Id = x.LuckyDrawRegistration.Id,
                        LuckyDrawId = x.LuckyDrawRegistration.LuckyDrawId,
                        AppUserId = x.LuckyDrawRegistration.AppUserId,
                        StoreId = x.LuckyDrawRegistration.StoreId,
                        Revenue = x.LuckyDrawRegistration.Revenue,
                        TurnCounter = x.LuckyDrawRegistration.TurnCounter,
                        IsDrawnByStore = x.LuckyDrawRegistration.IsDrawnByStore,
                        Time = x.LuckyDrawRegistration.Time,
                    },
                    LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                    {
                        Id = x.LuckyDrawStructure.Id,
                        LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                        Name = x.LuckyDrawStructure.Name,
                        Value = x.LuckyDrawStructure.Value,
                        Quantity = x.LuckyDrawStructure.Quantity,
                    },
                }).ToList();
            LuckyDraw.BaseLanguage = CurrentContext.Language;
            return LuckyDraw;
        }
        private LuckyDrawFilter ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter();
            LuckyDrawFilter.Selects = LuckyDrawSelect.ALL;
            LuckyDrawFilter.Skip = LuckyDraw_LuckyDrawFilterDTO.Skip;
            LuckyDrawFilter.Take = LuckyDraw_LuckyDrawFilterDTO.Take;
            LuckyDrawFilter.OrderBy = LuckyDraw_LuckyDrawFilterDTO.OrderBy;
            LuckyDrawFilter.OrderType = LuckyDraw_LuckyDrawFilterDTO.OrderType;

            LuckyDrawFilter.Id = LuckyDraw_LuckyDrawFilterDTO.Id;
            LuckyDrawFilter.Code = LuckyDraw_LuckyDrawFilterDTO.Code;
            LuckyDrawFilter.Name = LuckyDraw_LuckyDrawFilterDTO.Name;
            LuckyDrawFilter.LuckyDrawTypeId = LuckyDraw_LuckyDrawFilterDTO.LuckyDrawTypeId;
            LuckyDrawFilter.OrganizationId = LuckyDraw_LuckyDrawFilterDTO.OrganizationId;
            LuckyDrawFilter.RevenuePerTurn = LuckyDraw_LuckyDrawFilterDTO.RevenuePerTurn;
            LuckyDrawFilter.StartAt = LuckyDraw_LuckyDrawFilterDTO.StartAt;
            LuckyDrawFilter.EndAt = LuckyDraw_LuckyDrawFilterDTO.EndAt;
            LuckyDrawFilter.ImageId = LuckyDraw_LuckyDrawFilterDTO.ImageId;
            LuckyDrawFilter.AvatarImageId = LuckyDraw_LuckyDrawFilterDTO.AvatarImageId;
            LuckyDrawFilter.StatusId = LuckyDraw_LuckyDrawFilterDTO.StatusId;
            LuckyDrawFilter.CreatedAt = LuckyDraw_LuckyDrawFilterDTO.CreatedAt;
            LuckyDrawFilter.UpdatedAt = LuckyDraw_LuckyDrawFilterDTO.UpdatedAt;
            return LuckyDrawFilter;
        }
    }
}

