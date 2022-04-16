using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MAppUser;
using DMS.Services.MDistrict;
using DMS.Services.MOrganization;
using DMS.Services.MProvince;
using DMS.Services.MStore;
using DMS.Services.MStoreScouting;
using DMS.Services.MStoreScoutingStatus;
using DMS.Services.MStoreScoutingType;
using DMS.Services.MWard;
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

namespace DMS.Rpc.store_scouting
{
    public class StoreScoutingController : RpcController
    {
        private IAppUserService AppUserService;
        private IDistrictService DistrictService;
        private IOrganizationService OrganizationService;
        private IProvinceService ProvinceService;
        private IStoreService StoreService;
        private IStoreScoutingStatusService StoreScoutingStatusService;
        private IWardService WardService;
        private IStoreScoutingService StoreScoutingService;
        private IStoreScoutingTypeService StoreScoutingTypeService;
        private ICurrentContext CurrentContext;
        public StoreScoutingController(
            IAppUserService AppUserService,
            IDistrictService DistrictService,
            IOrganizationService OrganizationService,
            IProvinceService ProvinceService,
            IStoreService StoreService,
            IStoreScoutingStatusService StoreScoutingStatusService,
            IWardService WardService,
            IStoreScoutingService StoreScoutingService,
            IStoreScoutingTypeService StoreScoutingTypeService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.DistrictService = DistrictService;
            this.OrganizationService = OrganizationService;
            this.ProvinceService = ProvinceService;
            this.StoreService = StoreService;
            this.StoreScoutingStatusService = StoreScoutingStatusService;
            this.WardService = WardService;
            this.StoreScoutingService = StoreScoutingService;
            this.StoreScoutingTypeService = StoreScoutingTypeService;
            this.CurrentContext = CurrentContext;
        }

        [Route(StoreScoutingRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] StoreScouting_StoreScoutingFilterDTO StoreScouting_StoreScoutingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingFilter StoreScoutingFilter = ConvertFilterDTOToFilterEntity(StoreScouting_StoreScoutingFilterDTO);
            StoreScoutingFilter = await StoreScoutingService.ToFilter(StoreScoutingFilter);
            int count = await StoreScoutingService.Count(StoreScoutingFilter);
            return count;
        }

        [Route(StoreScoutingRoute.List), HttpPost]
        public async Task<List<StoreScouting_StoreScoutingDTO>> List([FromBody] StoreScouting_StoreScoutingFilterDTO StoreScouting_StoreScoutingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingFilter StoreScoutingFilter = ConvertFilterDTOToFilterEntity(StoreScouting_StoreScoutingFilterDTO);
            StoreScoutingFilter = await StoreScoutingService.ToFilter(StoreScoutingFilter);
            List<StoreScouting> StoreScoutings = await StoreScoutingService.List(StoreScoutingFilter);
            List<StoreScouting_StoreScoutingDTO> StoreScouting_StoreScoutingDTOs = StoreScoutings
                .Select(c => new StoreScouting_StoreScoutingDTO(c)).ToList();
            return StoreScouting_StoreScoutingDTOs;
        }

        [Route(StoreScoutingRoute.Get), HttpPost]
        public async Task<ActionResult<StoreScouting_StoreScoutingDTO>> Get([FromBody] StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScouting_StoreScoutingDTO.Id))
                return Forbid();

            StoreScouting StoreScouting = await StoreScoutingService.Get(StoreScouting_StoreScoutingDTO.Id);
            return new StoreScouting_StoreScoutingDTO(StoreScouting);
        }

        [Route(StoreScoutingRoute.Create), HttpPost]
        public async Task<ActionResult<StoreScouting_StoreScoutingDTO>> Create([FromBody] StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScouting_StoreScoutingDTO.Id))
                return Forbid();

            StoreScouting StoreScouting = ConvertDTOToEntity(StoreScouting_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Create(StoreScouting);
            StoreScouting_StoreScoutingDTO = new StoreScouting_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return StoreScouting_StoreScoutingDTO;
            else
                return BadRequest(StoreScouting_StoreScoutingDTO);
        }

        [Route(StoreScoutingRoute.Update), HttpPost]
        public async Task<ActionResult<StoreScouting_StoreScoutingDTO>> Update([FromBody] StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScouting_StoreScoutingDTO.Id))
                return Forbid();

            StoreScouting StoreScouting = ConvertDTOToEntity(StoreScouting_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Update(StoreScouting);
            StoreScouting_StoreScoutingDTO = new StoreScouting_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return StoreScouting_StoreScoutingDTO;
            else
                return BadRequest(StoreScouting_StoreScoutingDTO);
        }

        [Route(StoreScoutingRoute.Reject), HttpPost]
        public async Task<ActionResult<StoreScouting_StoreScoutingDTO>> Reject([FromBody] StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreScouting_StoreScoutingDTO.Id))
                return Forbid();

            StoreScouting StoreScouting = ConvertDTOToEntity(StoreScouting_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Reject(StoreScouting);
            StoreScouting_StoreScoutingDTO = new StoreScouting_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return StoreScouting_StoreScoutingDTO;
            else
                return BadRequest(StoreScouting_StoreScoutingDTO);
        }

        [Route(StoreScoutingRoute.Delete), HttpPost]
        public async Task<ActionResult<StoreScouting_StoreScoutingDTO>> DeleteStoreScouting([FromBody] StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScouting StoreScouting = ConvertDTOToEntity(StoreScouting_StoreScoutingDTO);
            StoreScouting = await StoreScoutingService.Delete(StoreScouting);
            StoreScouting_StoreScoutingDTO = new StoreScouting_StoreScoutingDTO(StoreScouting);
            if (StoreScouting.IsValidated)
                return StoreScouting_StoreScoutingDTO;
            else
                return BadRequest(StoreScouting_StoreScoutingDTO);
        }

        [Route(StoreScoutingRoute.Import), HttpPost]
        public async Task<ActionResult<List<StoreScouting_StoreScoutingDTO>>> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest("Định dạng file không hợp lệ");

            DataFile DataFile = new DataFile
            {
                Name = file.FileName,
                Content = file.OpenReadStream(),
            };
            #region MDM
            List<AppUser> AppUsers = await AppUserService.List(new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.Username
            });
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Id | OrganizationSelect.Code
            });

            List<StoreScoutingType> StoreScoutingTypes = await StoreScoutingTypeService.List(new StoreScoutingTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreScoutingTypeSelect.Id | StoreScoutingTypeSelect.Code
            });

            List<Province> Provinces = await ProvinceService.List(new ProvinceFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceSelect.Id | ProvinceSelect.Code
            });

            List<District> Districts = await DistrictService.List(new DistrictFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DistrictSelect.Id | DistrictSelect.Code
            });

            List<Ward> Wards = await WardService.List(new WardFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WardSelect.Id | WardSelect.Code
            });

            List<StoreScoutingStatus> StoreScoutingStatuses = await StoreScoutingStatusService.List(new StoreScoutingStatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreScoutingStatusSelect.Id | StoreScoutingStatusSelect.Code | StoreScoutingStatusSelect.Name
            });

            List<StoreScouting> All = await StoreScoutingService.List(new StoreScoutingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreScoutingSelect.Id | StoreScoutingSelect.Code | StoreScoutingSelect.Name
            });

            Dictionary<string, StoreScouting> DictionaryAll = All.ToDictionary(x => x.Code, y => y);
            #endregion
            List<StoreScouting_ImportDTO> StoreScouting_ImportDTOs = new List<StoreScouting_ImportDTO>();

            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(DataFile.Content))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Đại lý cắm cờ"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");

                #region khai báo các cột
                int StartColumn = 1;
                int StartRow = 1;
                int SttColumnn = 0 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int StoreScoutingTypeCodeColumn = 3 + StartColumn;
                int OrganizationCodeColumn = 4 + StartColumn;
                int ProvinceCodeColumn = 5 + StartColumn;
                int DistrictCodeColumn = 6 + StartColumn;
                int WardCodeColumn = 7 + StartColumn;
                int AddressColumn = 8 + StartColumn;
                int LongitudeColumn = 9 + StartColumn;
                int LatitudeColumn = 10 + StartColumn;
                int OwnerPhoneColumn = 11 + StartColumn;
                int SalesEmployeeColumn = 12 + StartColumn;
                int DescriptionColumn = 13 + StartColumn;
                int StoreScoutingStatusColumn = 14 + StartColumn;
                #endregion

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    #region Lấy thông tin từng dòng
                    string stt = worksheet.Cells[i + StartRow, SttColumnn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;
                    bool convert = long.TryParse(stt, out long Stt);
                    if (convert == false)
                        continue;
                    StoreScouting_ImportDTO StoreScouting_ImportDTO = new StoreScouting_ImportDTO();
                    StoreScouting_ImportDTOs.Add(StoreScouting_ImportDTO);
                    StoreScouting_ImportDTO.Stt = Stt;
                    StoreScouting_ImportDTO.NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
                    StoreScouting_ImportDTO.StoreScoutingTypeCodeValue = worksheet.Cells[i + StartRow, StoreScoutingTypeCodeColumn].Value?.ToString();
                    StoreScouting_ImportDTO.OrganizationCodeValue = worksheet.Cells[i + StartRow, OrganizationCodeColumn].Value?.ToString();
                    StoreScouting_ImportDTO.ProvinceCodeValue = worksheet.Cells[i + StartRow, ProvinceCodeColumn].Value?.ToString();
                    StoreScouting_ImportDTO.DistrictCodeValue = worksheet.Cells[i + StartRow, DistrictCodeColumn].Value?.ToString();
                    StoreScouting_ImportDTO.WardCodeValue = worksheet.Cells[i + StartRow, WardCodeColumn].Value?.ToString();
                    StoreScouting_ImportDTO.AddressValue = worksheet.Cells[i + StartRow, AddressColumn].Value?.ToString();
                    StoreScouting_ImportDTO.LongitudeValue = worksheet.Cells[i + StartRow, LongitudeColumn].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.LongitudeValue) && StoreScouting_ImportDTO.LongitudeValue.Contains(","))
                        StoreScouting_ImportDTO.LongitudeValue = StoreScouting_ImportDTO.LongitudeValue.Replace(",", ".");
                    StoreScouting_ImportDTO.LatitudeValue = worksheet.Cells[i + StartRow, LatitudeColumn].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.LatitudeValue) && StoreScouting_ImportDTO.LatitudeValue.Contains(","))
                        StoreScouting_ImportDTO.LatitudeValue = StoreScouting_ImportDTO.LatitudeValue.Replace(",", ".");

                    StoreScouting_ImportDTO.OwnerPhoneValue = worksheet.Cells[i + StartRow, OwnerPhoneColumn].Value?.ToString();
                    StoreScouting_ImportDTO.SalesEmployeeUsernameValue = worksheet.Cells[i + StartRow, SalesEmployeeColumn].Value?.ToString();
                    StoreScouting_ImportDTO.Description = worksheet.Cells[i + StartRow, DescriptionColumn].Value?.ToString();
                    StoreScouting_ImportDTO.StoreScoutingStatusNameValue = worksheet.Cells[i + StartRow, StoreScoutingStatusColumn].Value?.ToString();

                    #endregion
                }
            }
            Dictionary<long, StringBuilder> Errors = new Dictionary<long, StringBuilder>();
            foreach (StoreScouting_ImportDTO StoreScouting_ImportDTO in StoreScouting_ImportDTOs)
            {
                Errors.Add(StoreScouting_ImportDTO.Stt, new StringBuilder(""));
                StoreScouting_ImportDTO.IsNew = false;
            }
            Parallel.ForEach(StoreScouting_ImportDTOs, StoreScouting_ImportDTO =>
            {
                StoreScouting_ImportDTO.IsNew = true;
                StoreScouting_ImportDTO.OrganizationId = Organizations.Where(x => x.Code.Equals(StoreScouting_ImportDTO.OrganizationCodeValue)).Select(x => x.Id).FirstOrDefault();
                StoreScouting_ImportDTO.Longitude = decimal.TryParse(StoreScouting_ImportDTO.LongitudeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal Longitude) ? Longitude : 106;
                StoreScouting_ImportDTO.Latitude = decimal.TryParse(StoreScouting_ImportDTO.LatitudeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal Latitude) ? Latitude : 21;
                if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.StoreScoutingTypeCodeValue))
                    StoreScouting_ImportDTO.StoreScoutingTypeId = StoreScoutingTypes.Where(x => x.Code.Equals(StoreScouting_ImportDTO.StoreScoutingTypeCodeValue)).Select(x => x.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.ProvinceCodeValue))
                    StoreScouting_ImportDTO.ProvinceId = Provinces.Where(x => x.Code.Equals(StoreScouting_ImportDTO.ProvinceCodeValue)).Select(x => (long?)x.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.DistrictCodeValue))
                    StoreScouting_ImportDTO.DistrictId = Districts.Where(x => x.Code.Equals(StoreScouting_ImportDTO.DistrictCodeValue)).Select(x => (long?)x.Id).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.WardCodeValue))
                    StoreScouting_ImportDTO.WardId = Wards.Where(x => x.Code.Equals(StoreScouting_ImportDTO.WardCodeValue)).Select(x => (long?)x.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(StoreScouting_ImportDTO.StoreScoutingStatusNameValue))
                {
                    StoreScouting_ImportDTO.StoreScoutingStatusId = -1;
                }
                else
                {
                    string StoreScoutingStatusNameValue = StoreScouting_ImportDTO.StoreScoutingStatusNameValue;
                    StoreScouting_ImportDTO.StoreScoutingStatusId = StoreScoutingStatuses.Where(x => x.Name.ToLower().Equals(StoreScoutingStatusNameValue == null ? string.Empty : StoreScoutingStatusNameValue.Trim().ToLower())).Select(x => x.Id).FirstOrDefault();
                }
                if (!string.IsNullOrWhiteSpace(StoreScouting_ImportDTO.SalesEmployeeUsernameValue))
                    StoreScouting_ImportDTO.SalesEmployeeId = AppUsers.Where(x => x.Username.Equals(StoreScouting_ImportDTO.SalesEmployeeUsernameValue)).Select(x => x.Id).FirstOrDefault();
            });

            string error = string.Join("\n", Errors.Where(x => !string.IsNullOrWhiteSpace(x.Value.ToString())).Select(x => x.Value.ToString()).ToList());
            if (!string.IsNullOrWhiteSpace(error))
                return BadRequest(error);

            Dictionary<long, StoreScouting> DictionaryStoreScoutings = StoreScouting_ImportDTOs.ToDictionary(x => x.Stt, y => new StoreScouting());
            Parallel.ForEach(StoreScouting_ImportDTOs, StoreScouting_ImportDTO =>
            {
                StoreScouting StoreScouting = DictionaryStoreScoutings[StoreScouting_ImportDTO.Stt];
                if (StoreScouting_ImportDTO.IsNew == false)
                {
                    StoreScouting Old = DictionaryAll[StoreScouting_ImportDTO.CodeValue];
                    StoreScouting.Id = Old.Id;
                }
                StoreScouting.Code = StoreScouting_ImportDTO.CodeValue;
                StoreScouting.Name = StoreScouting_ImportDTO.NameValue;
                StoreScouting.StoreScoutingTypeId = StoreScouting_ImportDTO.StoreScoutingTypeId;
                StoreScouting.StoreScoutingType = new StoreScoutingType { Code = StoreScouting_ImportDTO.StoreScoutingTypeCodeValue };
                StoreScouting.OrganizationId = StoreScouting_ImportDTO.OrganizationId;
                StoreScouting.Organization = new Organization { Code = StoreScouting_ImportDTO.OrganizationCodeValue };
                StoreScouting.ProvinceId = StoreScouting_ImportDTO.ProvinceId;
                if (StoreScouting.ProvinceId.HasValue)
                {
                    StoreScouting.Province = new Province { Code = StoreScouting_ImportDTO.ProvinceCodeValue };
                }
                StoreScouting.DistrictId = StoreScouting_ImportDTO.DistrictId;
                if (StoreScouting.DistrictId.HasValue)
                {
                    StoreScouting.District = new District { Code = StoreScouting_ImportDTO.DistrictCodeValue };
                }
                StoreScouting.WardId = StoreScouting_ImportDTO.WardId;
                if (StoreScouting.WardId.HasValue)
                {
                    StoreScouting.Ward = new Ward { Code = StoreScouting_ImportDTO.WardCodeValue };
                }
                StoreScouting.Address = StoreScouting_ImportDTO.AddressValue;
                StoreScouting.Longitude = StoreScouting_ImportDTO.Longitude;
                StoreScouting.Latitude = StoreScouting_ImportDTO.Latitude;

                StoreScouting.OwnerPhone = StoreScouting_ImportDTO.OwnerPhoneValue;
                StoreScouting.CreatorId = StoreScouting_ImportDTO.SalesEmployeeId;
                StoreScouting.Creator = new AppUser { Username = StoreScouting_ImportDTO.SalesEmployeeUsernameValue };
                StoreScouting.StoreScoutingStatusId = StoreScouting_ImportDTO.StoreScoutingStatusId;
                StoreScouting.Description = StoreScouting_ImportDTO.Description;
                StoreScouting.BaseLanguage = CurrentContext.Language;
            });
            List<StoreScouting> StoreScoutings = DictionaryStoreScoutings.Select(x => x.Value).ToList();
            errorContent = new StringBuilder(error);
            StoreScoutings = await StoreScoutingService.Import(StoreScoutings);
            if (StoreScoutings == null)
                return Ok();
            List<StoreScouting_StoreScoutingDTO> StoreScouting_StoreScoutingDTOs = StoreScoutings
                .Select(c => new StoreScouting_StoreScoutingDTO(c)).ToList();
            for (int i = 0; i < StoreScoutings.Count; i++)
            {
                if (!StoreScoutings[i].IsValidated)
                {
                    errorContent.Append($"Lỗi dòng thứ {i + 2}:");
                    foreach (var Error in StoreScoutings[i].Errors)
                    {
                        errorContent.Append($" {Error.Value},");
                    }
                    errorContent.AppendLine("");
                }
            }
            if (StoreScoutings.Any(s => !s.IsValidated))
                return BadRequest(errorContent.ToString());
            return Ok(StoreScouting_StoreScoutingDTOs);
        }

        [Route(StoreScoutingRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] StoreScouting_StoreScoutingFilterDTO StoreScouting_StoreScoutingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScouting_StoreScoutingFilterDTO.Skip = 0;
            StoreScouting_StoreScoutingFilterDTO.Take = int.MaxValue;
            List<StoreScouting_StoreScoutingDTO> StoreScouting_StoreScoutingDTOs = await List(StoreScouting_StoreScoutingFilterDTO);
            StoreScouting_StoreScoutingDTOs = StoreScouting_StoreScoutingDTOs
                .OrderBy(x => x.Creator.Username).ThenByDescending(x => x.CreatedAt)
                .ToList();

            var OrganizationNames = StoreScouting_StoreScoutingDTOs
                .OrderBy(x => x.OrganizationId)
                .Select(x => x.Organization?.Name)
                .Distinct()
                .ToList();

            List<StoreScouting_ExportDTO> StoreScouting_ExportDTOs = new List<StoreScouting_ExportDTO>();
            foreach (var OrganizationName in OrganizationNames)
            {
                StoreScouting_ExportDTO StoreScouting_ExportDTO = new StoreScouting_ExportDTO
                {
                    OrganizationName = OrganizationName
                };
                StoreScouting_ExportDTOs.Add(StoreScouting_ExportDTO);
                StoreScouting_ExportDTO.StoreScoutings = StoreScouting_StoreScoutingDTOs.Where(x => x.Organization.Name == OrganizationName).ToList();
            }

            int stt = 1;
            foreach (var StoreScouting_ExportDTO in StoreScouting_ExportDTOs)
            {
                foreach (StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO in StoreScouting_ExportDTO.StoreScoutings)
                {
                    StoreScouting_StoreScoutingDTO.STT = stt;
                    stt++;
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            DateTime Start = StoreScouting_StoreScoutingFilterDTO.CreatedAt?.GreaterEqual == null ?
               StaticParams.DateTimeNow.Date :
               StoreScouting_StoreScoutingFilterDTO.CreatedAt.GreaterEqual.Value.Date;

            DateTime End = StoreScouting_StoreScoutingFilterDTO.CreatedAt?.LessEqual == null ?
                    StaticParams.DateTimeNow.Date.AddDays(1).AddSeconds(-1) :
                    StoreScouting_StoreScoutingFilterDTO.CreatedAt.LessEqual.Value.Date.AddDays(1).AddSeconds(-1);
            string path = "Templates/StoreScouting_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Exports = StoreScouting_ExportDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "StoreScouting_Export.xlsx");
        }

        [Route(StoreScoutingRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate()
        {
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Code | OrganizationSelect.Name | OrganizationSelect.Path,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });

            List<Province> Provinces = await ProvinceService.List(new ProvinceFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceSelect.Id | ProvinceSelect.Code | ProvinceSelect.Name,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });

            List<District> Districts = await DistrictService.List(new DistrictFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DistrictSelect.Id | DistrictSelect.Code | DistrictSelect.Name | DistrictSelect.Province,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });

            List<Ward> Wards = await WardService.List(new WardFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WardSelect.Id | WardSelect.Code | WardSelect.Name | WardSelect.District,
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            });
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MemoryStream MemoryStream = new MemoryStream();
            string tempPath = "Templates/StoreScouting_Template.xlsx";
            using (var xlPackage = new ExcelPackage(new FileInfo(tempPath)))
            {
                #region sheet Organization 
                var worksheet_Organization = xlPackage.Workbook.Worksheets["Đơn vị tổ chức"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Organization = 2;
                int numberCell_Organizations = 1;
                for (var i = 0; i < Organizations.Count; i++)
                {
                    Organization Organization = Organizations[i];
                    worksheet_Organization.Cells[startRow_Organization + i, numberCell_Organizations].Value = Organization.Code;
                    worksheet_Organization.Cells[startRow_Organization + i, numberCell_Organizations + 1].Value = Organization.Name;
                }
                #endregion

                #region sheet Province 
                var worksheet_Province = xlPackage.Workbook.Worksheets["Tỉnh, Thành phố"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Province = 2;
                int numberCell_Provinces = 1;
                for (var i = 0; i < Provinces.Count; i++)
                {
                    Province Province = Provinces[i];
                    worksheet_Province.Cells[startRow_Province + i, numberCell_Provinces].Value = Province.Code;
                    worksheet_Province.Cells[startRow_Province + i, numberCell_Provinces + 1].Value = Province.Name;
                }
                #endregion

                #region sheet District 
                var worksheet_District = xlPackage.Workbook.Worksheets["Quận, Huyện"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_District = 2;
                int numberCell_Districts = 1;
                for (var i = 0; i < Districts.Count; i++)
                {
                    District District = Districts[i];
                    worksheet_District.Cells[startRow_District + i, numberCell_Districts].Value = District.Code;
                    worksheet_District.Cells[startRow_District + i, numberCell_Districts + 1].Value = District.Name;
                    worksheet_District.Cells[startRow_District + i, numberCell_Districts + 2].Value = District.Province?.Name;
                }
                #endregion

                #region sheet Ward 
                var worksheet_Ward = xlPackage.Workbook.Worksheets["Phường, Xã"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Ward = 2;
                int numberCell_Wards = 1;
                for (var i = 0; i < Wards.Count; i++)
                {
                    Ward Ward = Wards[i];
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards].Value = Ward.Code;
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards + 1].Value = Ward.Name;
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards + 2].Value = Ward.District?.Name;
                    worksheet_Ward.Cells[startRow_Ward + i, numberCell_Wards + 3].Value = Ward.District?.Province?.Name;
                }
                #endregion
                xlPackage.SaveAs(MemoryStream);
            }

            return File(MemoryStream.ToArray(), "application/octet-stream", "Template_StoreScouting.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter();
            StoreScoutingFilter = await StoreScoutingService.ToFilter(StoreScoutingFilter);
            if (Id == 0)
            {

            }
            else
            {
                StoreScoutingFilter.Id = new IdFilter { Equal = Id };
                int count = await StoreScoutingService.Count(StoreScoutingFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private StoreScouting ConvertDTOToEntity(StoreScouting_StoreScoutingDTO StoreScouting_StoreScoutingDTO)
        {
            StoreScouting_StoreScoutingDTO.TrimString();
            StoreScouting StoreScouting = new StoreScouting();
            StoreScouting.Id = StoreScouting_StoreScoutingDTO.Id;
            StoreScouting.Code = StoreScouting_StoreScoutingDTO.Code;
            StoreScouting.Name = StoreScouting_StoreScoutingDTO.Name;
            StoreScouting.OwnerPhone = StoreScouting_StoreScoutingDTO.OwnerPhone;
            StoreScouting.ProvinceId = StoreScouting_StoreScoutingDTO.ProvinceId;
            StoreScouting.DistrictId = StoreScouting_StoreScoutingDTO.DistrictId;
            StoreScouting.WardId = StoreScouting_StoreScoutingDTO.WardId;
            StoreScouting.Address = StoreScouting_StoreScoutingDTO.Address;
            StoreScouting.Latitude = StoreScouting_StoreScoutingDTO.Latitude;
            StoreScouting.Longitude = StoreScouting_StoreScoutingDTO.Longitude;
            StoreScouting.CreatorId = StoreScouting_StoreScoutingDTO.CreatorId;
            StoreScouting.OrganizationId = StoreScouting_StoreScoutingDTO.OrganizationId;
            StoreScouting.StoreScoutingStatusId = StoreScouting_StoreScoutingDTO.StoreScoutingStatusId;
            StoreScouting.StoreScoutingTypeId = StoreScouting_StoreScoutingDTO.StoreScoutingTypeId;
            StoreScouting.Description = StoreScouting_StoreScoutingDTO.Description;
            StoreScouting.Link = StoreScouting_StoreScoutingDTO.Link;
            StoreScouting.RowId = StoreScouting_StoreScoutingDTO.RowId;
            StoreScouting.Creator = StoreScouting_StoreScoutingDTO.Creator == null ? null : new AppUser
            {
                Id = StoreScouting_StoreScoutingDTO.Creator.Id,
                Username = StoreScouting_StoreScoutingDTO.Creator.Username,
                DisplayName = StoreScouting_StoreScoutingDTO.Creator.DisplayName,
                Address = StoreScouting_StoreScoutingDTO.Creator.Address,
                Email = StoreScouting_StoreScoutingDTO.Creator.Email,
                Phone = StoreScouting_StoreScoutingDTO.Creator.Phone,
                PositionId = StoreScouting_StoreScoutingDTO.Creator.PositionId,
                Department = StoreScouting_StoreScoutingDTO.Creator.Department,
                OrganizationId = StoreScouting_StoreScoutingDTO.Creator.OrganizationId,
                StatusId = StoreScouting_StoreScoutingDTO.Creator.StatusId,
                Avatar = StoreScouting_StoreScoutingDTO.Creator.Avatar,
                ProvinceId = StoreScouting_StoreScoutingDTO.Creator.ProvinceId,
                SexId = StoreScouting_StoreScoutingDTO.Creator.SexId,
                Birthday = StoreScouting_StoreScoutingDTO.Creator.Birthday,
            };
            StoreScouting.Organization = StoreScouting_StoreScoutingDTO.Organization == null ? null : new Organization
            {
                Id = StoreScouting_StoreScoutingDTO.Organization.Id,
                Code = StoreScouting_StoreScoutingDTO.Organization.Code,
                Name = StoreScouting_StoreScoutingDTO.Organization.Name,
                ParentId = StoreScouting_StoreScoutingDTO.Organization.ParentId,
                Path = StoreScouting_StoreScoutingDTO.Organization.Path,
                Level = StoreScouting_StoreScoutingDTO.Organization.Level,
                StatusId = StoreScouting_StoreScoutingDTO.Organization.StatusId,
                Phone = StoreScouting_StoreScoutingDTO.Organization.Phone,
                Address = StoreScouting_StoreScoutingDTO.Organization.Address,
                Email = StoreScouting_StoreScoutingDTO.Organization.Email,
            };
            StoreScouting.District = StoreScouting_StoreScoutingDTO.District == null ? null : new District
            {
                Id = StoreScouting_StoreScoutingDTO.District.Id,
                Code = StoreScouting_StoreScoutingDTO.District.Code,
                Name = StoreScouting_StoreScoutingDTO.District.Name,
                Priority = StoreScouting_StoreScoutingDTO.District.Priority,
                ProvinceId = StoreScouting_StoreScoutingDTO.District.ProvinceId,
                StatusId = StoreScouting_StoreScoutingDTO.District.StatusId,
            };
            StoreScouting.Province = StoreScouting_StoreScoutingDTO.Province == null ? null : new Province
            {
                Id = StoreScouting_StoreScoutingDTO.Province.Id,
                Code = StoreScouting_StoreScoutingDTO.Province.Code,
                Name = StoreScouting_StoreScoutingDTO.Province.Name,
                Priority = StoreScouting_StoreScoutingDTO.Province.Priority,
                StatusId = StoreScouting_StoreScoutingDTO.Province.StatusId,
            };
            StoreScouting.StoreScoutingStatus = StoreScouting_StoreScoutingDTO.StoreScoutingStatus == null ? null : new StoreScoutingStatus
            {
                Id = StoreScouting_StoreScoutingDTO.StoreScoutingStatus.Id,
                Code = StoreScouting_StoreScoutingDTO.StoreScoutingStatus.Code,
                Name = StoreScouting_StoreScoutingDTO.StoreScoutingStatus.Name,
            };
            StoreScouting.StoreScoutingType = StoreScouting_StoreScoutingDTO.StoreScoutingType == null ? null : new StoreScoutingType
            {
                Id = StoreScouting_StoreScoutingDTO.StoreScoutingType.Id,
                Code = StoreScouting_StoreScoutingDTO.StoreScoutingType.Code,
                Name = StoreScouting_StoreScoutingDTO.StoreScoutingType.Name,
            };
            StoreScouting.Ward = StoreScouting_StoreScoutingDTO.Ward == null ? null : new Ward
            {
                Id = StoreScouting_StoreScoutingDTO.Ward.Id,
                Code = StoreScouting_StoreScoutingDTO.Ward.Code,
                Name = StoreScouting_StoreScoutingDTO.Ward.Name,
                Priority = StoreScouting_StoreScoutingDTO.Ward.Priority,
                DistrictId = StoreScouting_StoreScoutingDTO.Ward.DistrictId,
                StatusId = StoreScouting_StoreScoutingDTO.Ward.StatusId,
            };
            StoreScouting.StoreScoutingImageMappings = StoreScouting_StoreScoutingDTO.StoreScoutingImageMappings?
                .Select(x => new StoreScoutingImageMapping
                {
                    StoreScoutingId = x.StoreScoutingId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToList();
            StoreScouting.BaseLanguage = CurrentContext.Language;
            return StoreScouting;
        }

        private StoreScoutingFilter ConvertFilterDTOToFilterEntity(StoreScouting_StoreScoutingFilterDTO StoreScouting_StoreScoutingFilterDTO)
        {
            StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter();
            StoreScoutingFilter.Selects = StoreScoutingSelect.ALL;
            StoreScoutingFilter.Skip = StoreScouting_StoreScoutingFilterDTO.Skip;
            StoreScoutingFilter.Take = StoreScouting_StoreScoutingFilterDTO.Take;
            StoreScoutingFilter.OrderBy = StoreScouting_StoreScoutingFilterDTO.OrderBy;
            StoreScoutingFilter.OrderType = StoreScouting_StoreScoutingFilterDTO.OrderType;

            StoreScoutingFilter.Id = StoreScouting_StoreScoutingFilterDTO.Id;
            StoreScoutingFilter.Code = StoreScouting_StoreScoutingFilterDTO.Code;
            StoreScoutingFilter.Name = StoreScouting_StoreScoutingFilterDTO.Name;
            StoreScoutingFilter.OwnerPhone = StoreScouting_StoreScoutingFilterDTO.OwnerPhone;
            StoreScoutingFilter.ProvinceId = StoreScouting_StoreScoutingFilterDTO.ProvinceId;
            StoreScoutingFilter.DistrictId = StoreScouting_StoreScoutingFilterDTO.DistrictId;
            StoreScoutingFilter.WardId = StoreScouting_StoreScoutingFilterDTO.WardId;
            StoreScoutingFilter.OrganizationId = StoreScouting_StoreScoutingFilterDTO.OrganizationId;
            StoreScoutingFilter.Address = StoreScouting_StoreScoutingFilterDTO.Address;
            StoreScoutingFilter.Latitude = StoreScouting_StoreScoutingFilterDTO.Latitude;
            StoreScoutingFilter.Longitude = StoreScouting_StoreScoutingFilterDTO.Longitude;
            StoreScoutingFilter.AppUserId = StoreScouting_StoreScoutingFilterDTO.AppUserId;
            StoreScoutingFilter.StoreScoutingStatusId = StoreScouting_StoreScoutingFilterDTO.StoreScoutingStatusId;
            StoreScoutingFilter.StoreScoutingTypeId = StoreScouting_StoreScoutingFilterDTO.StoreScoutingTypeId;
            StoreScoutingFilter.CreatedAt = StoreScouting_StoreScoutingFilterDTO.CreatedAt;
            StoreScoutingFilter.UpdatedAt = StoreScouting_StoreScoutingFilterDTO.UpdatedAt;
            return StoreScoutingFilter;
        }

        [Route(StoreScoutingRoute.FilterListAppUser), HttpPost]
        public async Task<List<StoreScouting_AppUserDTO>> FilterListAppUser([FromBody] StoreScouting_AppUserFilterDTO StoreScouting_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = StoreScouting_AppUserFilterDTO.Id;
            AppUserFilter.Username = StoreScouting_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = StoreScouting_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = StoreScouting_AppUserFilterDTO.OrganizationId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<StoreScouting_AppUserDTO> StoreScouting_AppUserDTOs = AppUsers
                .Select(x => new StoreScouting_AppUserDTO(x)).ToList();
            return StoreScouting_AppUserDTOs;
        }
        [Route(StoreScoutingRoute.FilterListDistrict), HttpPost]
        public async Task<List<StoreScouting_DistrictDTO>> FilterListDistrict([FromBody] StoreScouting_DistrictFilterDTO StoreScouting_DistrictFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = StoreScouting_DistrictFilterDTO.Id;
            DistrictFilter.Code = StoreScouting_DistrictFilterDTO.Code;
            DistrictFilter.Name = StoreScouting_DistrictFilterDTO.Name;
            DistrictFilter.Priority = StoreScouting_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = StoreScouting_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = StoreScouting_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<StoreScouting_DistrictDTO> StoreScouting_DistrictDTOs = Districts
                .Select(x => new StoreScouting_DistrictDTO(x)).ToList();
            return StoreScouting_DistrictDTOs;
        }
        [Route(StoreScoutingRoute.FilterListOrganization), HttpPost]
        public async Task<List<StoreScouting_OrganizationDTO>> FilterListOrganization()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<StoreScouting_OrganizationDTO> StoreScouting_OrganizationDTOs = Organizations
                .Select(x => new StoreScouting_OrganizationDTO(x)).ToList();
            return StoreScouting_OrganizationDTOs;
        }
        [Route(StoreScoutingRoute.FilterListProvince), HttpPost]
        public async Task<List<StoreScouting_ProvinceDTO>> FilterListProvince([FromBody] StoreScouting_ProvinceFilterDTO StoreScouting_ProvinceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = StoreScouting_ProvinceFilterDTO.Id;
            ProvinceFilter.Code = StoreScouting_ProvinceFilterDTO.Code;
            ProvinceFilter.Name = StoreScouting_ProvinceFilterDTO.Name;
            ProvinceFilter.Priority = StoreScouting_ProvinceFilterDTO.Priority;
            ProvinceFilter.StatusId = StoreScouting_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<StoreScouting_ProvinceDTO> StoreScouting_ProvinceDTOs = Provinces
                .Select(x => new StoreScouting_ProvinceDTO(x)).ToList();
            return StoreScouting_ProvinceDTOs;
        }
        [Route(StoreScoutingRoute.FilterListStore), HttpPost]
        public async Task<List<StoreScouting_StoreDTO>> FilterListStore([FromBody] StoreScouting_StoreFilterDTO StoreScouting_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = StoreScouting_StoreFilterDTO.Id;
            StoreFilter.Code = StoreScouting_StoreFilterDTO.Code;
            StoreFilter.Name = StoreScouting_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = StoreScouting_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = StoreScouting_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = StoreScouting_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = StoreScouting_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = StoreScouting_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = StoreScouting_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = StoreScouting_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = StoreScouting_StoreFilterDTO.WardId;
            StoreFilter.Address = StoreScouting_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = StoreScouting_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = StoreScouting_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = StoreScouting_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = StoreScouting_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = StoreScouting_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = StoreScouting_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = StoreScouting_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = StoreScouting_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = StoreScouting_StoreFilterDTO.StatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<StoreScouting_StoreDTO> StoreScouting_StoreDTOs = Stores
                .Select(x => new StoreScouting_StoreDTO(x)).ToList();
            return StoreScouting_StoreDTOs;
        }
        [Route(StoreScoutingRoute.FilterListStoreScoutingStatus), HttpPost]
        public async Task<List<StoreScouting_StoreScoutingStatusDTO>> FilterListStoreScoutingStatus([FromBody] StoreScouting_StoreScoutingStatusFilterDTO StoreScouting_StoreScoutingStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingStatusFilter StoreScoutingStatusFilter = new StoreScoutingStatusFilter();
            StoreScoutingStatusFilter.Skip = 0;
            StoreScoutingStatusFilter.Take = 20;
            StoreScoutingStatusFilter.OrderBy = StoreScoutingStatusOrder.Id;
            StoreScoutingStatusFilter.OrderType = OrderType.ASC;
            StoreScoutingStatusFilter.Selects = StoreScoutingStatusSelect.ALL;
            StoreScoutingStatusFilter.Id = StoreScouting_StoreScoutingStatusFilterDTO.Id;
            StoreScoutingStatusFilter.Code = StoreScouting_StoreScoutingStatusFilterDTO.Code;
            StoreScoutingStatusFilter.Name = StoreScouting_StoreScoutingStatusFilterDTO.Name;

            List<StoreScoutingStatus> StoreScoutingStatuses = await StoreScoutingStatusService.List(StoreScoutingStatusFilter);
            List<StoreScouting_StoreScoutingStatusDTO> StoreScouting_StoreScoutingStatusDTOs = StoreScoutingStatuses
                .Select(x => new StoreScouting_StoreScoutingStatusDTO(x)).ToList();
            return StoreScouting_StoreScoutingStatusDTOs;
        }
        [Route(StoreScoutingRoute.FilterListWard), HttpPost]
        public async Task<List<StoreScouting_WardDTO>> FilterListWard([FromBody] StoreScouting_WardFilterDTO StoreScouting_WardFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WardFilter WardFilter = new WardFilter();
            WardFilter.Skip = 0;
            WardFilter.Take = 20;
            WardFilter.OrderBy = WardOrder.Priority;
            WardFilter.OrderType = OrderType.ASC;
            WardFilter.Selects = WardSelect.ALL;
            WardFilter.Id = StoreScouting_WardFilterDTO.Id;
            WardFilter.Code = StoreScouting_WardFilterDTO.Code;
            WardFilter.Name = StoreScouting_WardFilterDTO.Name;
            WardFilter.Priority = StoreScouting_WardFilterDTO.Priority;
            WardFilter.DistrictId = StoreScouting_WardFilterDTO.DistrictId;
            WardFilter.StatusId = StoreScouting_WardFilterDTO.StatusId;

            List<Ward> Wards = await WardService.List(WardFilter);
            List<StoreScouting_WardDTO> StoreScouting_WardDTOs = Wards
                .Select(x => new StoreScouting_WardDTO(x)).ToList();
            return StoreScouting_WardDTOs;
        }

        [Route(StoreScoutingRoute.SingleListAppUser), HttpPost]
        public async Task<List<StoreScouting_AppUserDTO>> SingleListAppUser([FromBody] StoreScouting_AppUserFilterDTO StoreScouting_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = StoreScouting_AppUserFilterDTO.Id;
            AppUserFilter.Username = StoreScouting_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = StoreScouting_AppUserFilterDTO.DisplayName;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<StoreScouting_AppUserDTO> StoreScouting_AppUserDTOs = AppUsers
                .Select(x => new StoreScouting_AppUserDTO(x)).ToList();
            return StoreScouting_AppUserDTOs;
        }
        [Route(StoreScoutingRoute.SingleListDistrict), HttpPost]
        public async Task<List<StoreScouting_DistrictDTO>> SingleListDistrict([FromBody] StoreScouting_DistrictFilterDTO StoreScouting_DistrictFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = StoreScouting_DistrictFilterDTO.Id;
            DistrictFilter.Code = StoreScouting_DistrictFilterDTO.Code;
            DistrictFilter.Name = StoreScouting_DistrictFilterDTO.Name;
            DistrictFilter.Priority = StoreScouting_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = StoreScouting_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = StoreScouting_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<StoreScouting_DistrictDTO> StoreScouting_DistrictDTOs = Districts
                .Select(x => new StoreScouting_DistrictDTO(x)).ToList();
            return StoreScouting_DistrictDTOs;
        }
        [Route(StoreScoutingRoute.SingleListOrganization), HttpPost]
        public async Task<List<StoreScouting_OrganizationDTO>> SingleListOrganization()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;
            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<StoreScouting_OrganizationDTO> StoreScouting_OrganizationDTOs = Organizations
                .Select(x => new StoreScouting_OrganizationDTO(x)).ToList();
            return StoreScouting_OrganizationDTOs;
        }
        [Route(StoreScoutingRoute.SingleListProvince), HttpPost]
        public async Task<List<StoreScouting_ProvinceDTO>> SingleListProvince([FromBody] StoreScouting_ProvinceFilterDTO StoreScouting_ProvinceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = StoreScouting_ProvinceFilterDTO.Id;
            ProvinceFilter.Code = StoreScouting_ProvinceFilterDTO.Code;
            ProvinceFilter.Name = StoreScouting_ProvinceFilterDTO.Name;
            ProvinceFilter.Priority = StoreScouting_ProvinceFilterDTO.Priority;
            ProvinceFilter.StatusId = StoreScouting_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<StoreScouting_ProvinceDTO> StoreScouting_ProvinceDTOs = Provinces
                .Select(x => new StoreScouting_ProvinceDTO(x)).ToList();
            return StoreScouting_ProvinceDTOs;
        }
        [Route(StoreScoutingRoute.SingleListStore), HttpPost]
        public async Task<List<StoreScouting_StoreDTO>> SingleListStore([FromBody] StoreScouting_StoreFilterDTO StoreScouting_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = StoreScouting_StoreFilterDTO.Id;
            StoreFilter.Code = StoreScouting_StoreFilterDTO.Code;
            StoreFilter.Name = StoreScouting_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = StoreScouting_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = StoreScouting_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = StoreScouting_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = StoreScouting_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = StoreScouting_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = StoreScouting_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = StoreScouting_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = StoreScouting_StoreFilterDTO.WardId;
            StoreFilter.Address = StoreScouting_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = StoreScouting_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = StoreScouting_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = StoreScouting_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = StoreScouting_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = StoreScouting_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = StoreScouting_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = StoreScouting_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = StoreScouting_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = StoreScouting_StoreFilterDTO.StatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<StoreScouting_StoreDTO> StoreScouting_StoreDTOs = Stores
                .Select(x => new StoreScouting_StoreDTO(x)).ToList();
            return StoreScouting_StoreDTOs;
        }
        [Route(StoreScoutingRoute.SingleListStoreScoutingStatus), HttpPost]
        public async Task<List<StoreScouting_StoreScoutingStatusDTO>> SingleListStoreScoutingStatus([FromBody] StoreScouting_StoreScoutingStatusFilterDTO StoreScouting_StoreScoutingStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingStatusFilter StoreScoutingStatusFilter = new StoreScoutingStatusFilter();
            StoreScoutingStatusFilter.Skip = 0;
            StoreScoutingStatusFilter.Take = 20;
            StoreScoutingStatusFilter.OrderBy = StoreScoutingStatusOrder.Id;
            StoreScoutingStatusFilter.OrderType = OrderType.ASC;
            StoreScoutingStatusFilter.Selects = StoreScoutingStatusSelect.ALL;
            StoreScoutingStatusFilter.Id = StoreScouting_StoreScoutingStatusFilterDTO.Id;
            StoreScoutingStatusFilter.Code = StoreScouting_StoreScoutingStatusFilterDTO.Code;
            StoreScoutingStatusFilter.Name = StoreScouting_StoreScoutingStatusFilterDTO.Name;

            List<StoreScoutingStatus> StoreScoutingStatuses = await StoreScoutingStatusService.List(StoreScoutingStatusFilter);
            List<StoreScouting_StoreScoutingStatusDTO> StoreScouting_StoreScoutingStatusDTOs = StoreScoutingStatuses
                .Select(x => new StoreScouting_StoreScoutingStatusDTO(x)).ToList();
            return StoreScouting_StoreScoutingStatusDTOs;
        }
        [Route(StoreScoutingRoute.SingleListWard), HttpPost]
        public async Task<List<StoreScouting_WardDTO>> SingleListWard([FromBody] StoreScouting_WardFilterDTO StoreScouting_WardFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WardFilter WardFilter = new WardFilter();
            WardFilter.Skip = 0;
            WardFilter.Take = 20;
            WardFilter.OrderBy = WardOrder.Priority;
            WardFilter.OrderType = OrderType.ASC;
            WardFilter.Selects = WardSelect.ALL;
            WardFilter.Id = StoreScouting_WardFilterDTO.Id;
            WardFilter.Code = StoreScouting_WardFilterDTO.Code;
            WardFilter.Name = StoreScouting_WardFilterDTO.Name;
            WardFilter.Priority = StoreScouting_WardFilterDTO.Priority;
            WardFilter.DistrictId = StoreScouting_WardFilterDTO.DistrictId;
            WardFilter.StatusId = StoreScouting_WardFilterDTO.StatusId;

            List<Ward> Wards = await WardService.List(WardFilter);
            List<StoreScouting_WardDTO> StoreScouting_WardDTOs = Wards
                .Select(x => new StoreScouting_WardDTO(x)).ToList();
            return StoreScouting_WardDTOs;
        }
        [Route(StoreScoutingRoute.FilterListStoreScoutingType), HttpPost]
        public async Task<List<StoreScouting_StoreScoutingTypeDTO>> SingleListStoreScoutingType()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter();
            StoreScoutingTypeFilter.Skip = 0;
            StoreScoutingTypeFilter.Take = 20;
            StoreScoutingTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreScoutingTypeFilter.OrderBy = StoreScoutingTypeOrder.Id;
            StoreScoutingTypeFilter.OrderType = OrderType.ASC;
            StoreScoutingTypeFilter.Selects = StoreScoutingTypeSelect.ALL;

            List<StoreScoutingType> StoreScoutingTypes = await StoreScoutingTypeService.List(StoreScoutingTypeFilter);
            List<StoreScouting_StoreScoutingTypeDTO> StoreScouting_StoreScoutingTypeDTOs = StoreScoutingTypes

                .Select(x => new StoreScouting_StoreScoutingTypeDTO(x)).ToList();
            return StoreScouting_StoreScoutingTypeDTOs;
        }
    }
}

