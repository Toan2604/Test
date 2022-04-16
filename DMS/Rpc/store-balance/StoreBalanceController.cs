using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MColor;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreBalance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store_balance
{
    public partial class StoreBalanceController : RpcController
    {
        private IStoreBalanceService StoreBalanceService;
        private IStoreService StoreService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        public StoreBalanceController(
            IStoreBalanceService StoreBalanceService,
            IStoreService StoreService,
            IOrganizationService OrganizationService,
            ICurrentContext CurrentContext
        )
        {
            this.StoreBalanceService = StoreBalanceService;
            this.StoreService = StoreService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
        }

        [Route(StoreBalanceRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] StoreBalance_StoreBalanceFilterDTO StoreBalance_StoreBalanceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreBalanceFilter StoreBalanceFilter = ConvertFilterDTOToFilterEntity(StoreBalance_StoreBalanceFilterDTO);
            StoreBalanceFilter = await StoreBalanceService.ToFilter(StoreBalanceFilter);
            int count = await StoreBalanceService.Count(StoreBalanceFilter);
            return count;
        }

        [Route(StoreBalanceRoute.List), HttpPost]
        public async Task<ActionResult<List<StoreBalance_StoreBalanceDTO>>> List([FromBody] StoreBalance_StoreBalanceFilterDTO StoreBalance_StoreBalanceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreBalanceFilter StoreBalanceFilter = ConvertFilterDTOToFilterEntity(StoreBalance_StoreBalanceFilterDTO);
            StoreBalanceFilter = await StoreBalanceService.ToFilter(StoreBalanceFilter);
            List<StoreBalance> StoreBalances = await StoreBalanceService.List(StoreBalanceFilter);
            List<StoreBalance_StoreBalanceDTO> StoreBalance_StoreBalanceDTOs = StoreBalances
                .Select(c => new StoreBalance_StoreBalanceDTO(c)).ToList();
            return StoreBalance_StoreBalanceDTOs;
        }

        [Route(StoreBalanceRoute.Get), HttpPost]
        public async Task<ActionResult<StoreBalance_StoreBalanceDTO>> Get([FromBody] StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreBalance_StoreBalanceDTO.Id))
                return Forbid();

            StoreBalance StoreBalance = await StoreBalanceService.Get(StoreBalance_StoreBalanceDTO.Id);
            return new StoreBalance_StoreBalanceDTO(StoreBalance);
        }

        [Route(StoreBalanceRoute.Create), HttpPost]
        public async Task<ActionResult<StoreBalance_StoreBalanceDTO>> Create([FromBody] StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreBalance_StoreBalanceDTO.Id))
                return Forbid();

            StoreBalance StoreBalance = ConvertDTOToEntity(StoreBalance_StoreBalanceDTO);
            StoreBalance = await StoreBalanceService.Create(StoreBalance);
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO(StoreBalance);
            if (StoreBalance.IsValidated)
                return StoreBalance_StoreBalanceDTO;
            else
                return BadRequest(StoreBalance_StoreBalanceDTO);
        }

        [Route(StoreBalanceRoute.Update), HttpPost]
        public async Task<ActionResult<StoreBalance_StoreBalanceDTO>> Update([FromBody] StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreBalance_StoreBalanceDTO.Id))
                return Forbid();

            StoreBalance StoreBalance = ConvertDTOToEntity(StoreBalance_StoreBalanceDTO);
            StoreBalance = await StoreBalanceService.Update(StoreBalance);
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO(StoreBalance);
            if (StoreBalance.IsValidated)
                return StoreBalance_StoreBalanceDTO;
            else
                return BadRequest(StoreBalance_StoreBalanceDTO);
        }

        [Route(StoreBalanceRoute.Delete), HttpPost]
        public async Task<ActionResult<StoreBalance_StoreBalanceDTO>> Delete([FromBody] StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreBalance_StoreBalanceDTO.Id))
                return Forbid();

            StoreBalance StoreBalance = ConvertDTOToEntity(StoreBalance_StoreBalanceDTO);
            StoreBalance = await StoreBalanceService.Delete(StoreBalance);
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO(StoreBalance);
            if (StoreBalance.IsValidated)
                return StoreBalance_StoreBalanceDTO;
            else
                return BadRequest(StoreBalance_StoreBalanceDTO);
        }

        [Route(StoreBalanceRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreBalanceFilter StoreBalanceFilter = new StoreBalanceFilter();
            StoreBalanceFilter.Id = new IdFilter { In = Ids };
            StoreBalanceFilter.Selects = StoreBalanceSelect.Id;
            StoreBalanceFilter.Skip = 0;
            StoreBalanceFilter.Take = int.MaxValue;

            List<StoreBalance> StoreBalances = await StoreBalanceService.List(StoreBalanceFilter);
            StoreBalances = await StoreBalanceService.BulkDelete(StoreBalances);
            if (StoreBalances.Any(x => !x.IsValidated))
                return BadRequest(StoreBalances.Where(x => !x.IsValidated));
            return true;
        }
        [Route(StoreBalanceRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest("Định dạng file không hợp lệ");
            StoreFilter StoreFilter = new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name
            };
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL
            };
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<StoreBalance> StoreBalances = new List<StoreBalance>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Công nợ"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 2;
                int IdColumn = 0 + StartColumn;
                int StoreCodeColumn = 1 + StartColumn;
                int StoreNameColumn = 2 + StartColumn;
                int OrganizationCodeColumn = 3 + StartColumn;
                int DebitAmountColumn = 4 + StartColumn;
                int CreditAmountColumn = 5 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i, IdColumn].Value?.ToString();
                    string StoreCodeValue = worksheet.Cells[i, StoreCodeColumn].Value?.ToString();
                    string StoreNameValue = worksheet.Cells[i, StoreNameColumn].Value?.ToString();
                    string OrganizationCodeValue = worksheet.Cells[i, OrganizationCodeColumn].Value?.ToString();
                    string DebitAmountValue = worksheet.Cells[i, DebitAmountColumn].Value?.ToString();
                    string CreditAmountValue = worksheet.Cells[i, CreditAmountColumn].Value?.ToString();

                    StoreBalance StoreBalance = new StoreBalance();
                    StoreBalance.CreditAmount = decimal.TryParse(CreditAmountValue, out decimal CreditAmount) ? (decimal?) CreditAmount : null;
                    StoreBalance.DebitAmount = decimal.TryParse(DebitAmountValue, out decimal DebitAmount) ? (decimal?) DebitAmount : null;
                    Store Store = Stores.Where(x => x.Code == StoreCodeValue).FirstOrDefault();
                    StoreBalance.StoreId = Store == null ? 0 : Store.Id;
                    Organization Organization = Organizations.Where(x => x.Code == OrganizationCodeValue).FirstOrDefault();
                    StoreBalance.OrganizationId = Organization == null ? 0 : Organization.Id;

                    StoreBalances.Add(StoreBalance);
                }
            }
            StoreBalances = await StoreBalanceService.Import(StoreBalances);
            if (StoreBalances.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                StringBuilder Errors = new StringBuilder();
                string space = " ";
                for (int i = 0; i < StoreBalances.Count; i++)
                {
                    StoreBalance StoreBalance = StoreBalances[i];
                    if (!StoreBalance.IsValidated)
                    {
                        Errors.Append($"Dòng {i + 2} có lỗi: ");
                        if (StoreBalance.Errors.ContainsKey(nameof(StoreBalance.Id).Camelize()))
                            Errors.Append($" {StoreBalance.Errors[nameof(StoreBalance.Id).Camelize()]}");
                        if (StoreBalance.Errors.ContainsKey(nameof(StoreBalance.OrganizationId).Camelize()))
                            Errors.Append($" {StoreBalance.Errors[nameof(StoreBalance.OrganizationId).Camelize()]}");
                        if (StoreBalance.Errors.ContainsKey(nameof(StoreBalance.Organization).Camelize()))
                            Errors.Append($" {StoreBalance.Errors[nameof(StoreBalance.Organization).Camelize()]}");
                        if (StoreBalance.Errors.ContainsKey(nameof(StoreBalance.Store).Camelize()))
                            Errors.Append($" {StoreBalance.Errors[nameof(StoreBalance.Store).Camelize()]}");
                        if (StoreBalance.Errors.ContainsKey(nameof(StoreBalance.CreditAmount).Camelize()))
                            Errors.Append($" {StoreBalance.Errors[nameof(StoreBalance.CreditAmount).Camelize()]}");
                        if (StoreBalance.Errors.ContainsKey(nameof(StoreBalance.DebitAmount).Camelize()))
                            Errors.Append($" {StoreBalance.Errors[nameof(StoreBalance.DebitAmount).Camelize()]}");
                        Errors.AppendLine("");
                    }
                }
                return BadRequest(Errors.ToString());
            }
        }

        [Route(StoreBalanceRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] StoreBalance_StoreBalanceFilterDTO StoreBalance_StoreBalanceFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                var StoreBalanceFilter = ConvertFilterDTOToFilterEntity(StoreBalance_StoreBalanceFilterDTO);
                StoreBalanceFilter.Skip = 0;
                StoreBalanceFilter.Take = int.MaxValue;
                StoreBalanceFilter = await StoreBalanceService.ToFilter(StoreBalanceFilter);
                List<StoreBalance> StoreBalances = await StoreBalanceService.List(StoreBalanceFilter);

                List<string> StoreBalanceHeaders = new List<string>()
                {
                    "STT",
                    "Mã đại lý tự sinh",
                    "Mã đại lý tự nhập",
                    "Tên đại lý",
                    "Tên đơn vị quản lý",
                    "Dư nợ",
                    "Dư có",
                };
                List<object[]> StoreBalanceData = new List<object[]>();
                for (int i = 0; i < StoreBalances.Count; i++)
                {
                    var StoreBalance = StoreBalances[i];
                    StoreBalanceData.Add(new Object[]
                    {
                        i + 1,
                        StoreBalance.Store.Code,
                        StoreBalance.Store.CodeDraft,
                        StoreBalance.Store.Name,
                        StoreBalance.Organization.Name,
                        StoreBalance.DebitAmount,
                        StoreBalance.CreditAmount,
                    });
                }
                excel.GenerateWorksheet("Công nợ", StoreBalanceHeaders, StoreBalanceData);
                var Worksheet = excel.Workbook.Worksheets[0];
                Worksheet.Column(6).Style.Numberformat.Format = "#,##0.00";
                Worksheet.Column(6).AutoFit();
                Worksheet.Column(7).Style.Numberformat.Format = "#,##0.00";
                Worksheet.Column(7).AutoFit();
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "StoreBalance.xlsx");
        }

        [Route(StoreBalanceRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<Store> Stores = await StoreService.Export(new StoreFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = StoreSelect.Code | StoreSelect.Name | StoreSelect.Address | StoreSelect.Organization,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
            });
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                Selects = OrganizationSelect.Code | OrganizationSelect.Name
            });
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string tempPath = "Templates/StoreBalance_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(tempPath);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            using (var xlPackage = new ExcelPackage(input))
            {
                var worksheet_Store = xlPackage.Workbook.Worksheets["Đại lý"];
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                int startRow_Store = 2;
                int numberCell_Store = 1;
                for (var i = 0; i < Stores.Count; i++)
                {
                    Store Store = Stores[i];
                    worksheet_Store.Cells[startRow_Store + i, numberCell_Store].Value = Store.Code;
                    worksheet_Store.Cells[startRow_Store + i, numberCell_Store + 1].Value = Store.Name;
                    worksheet_Store.Cells[startRow_Store + i, numberCell_Store + 2].Value = Store.Organization.Code;
                    worksheet_Store.Cells[startRow_Store + i, numberCell_Store + 3].Value = Store.Organization.Name;
                    worksheet_Store.Cells[startRow_Store + i, numberCell_Store + 4].Value = Store.Address;
                }
                var worksheet_Organization = xlPackage.Workbook.Worksheets["Đơn vị"];
                int startRow_Organization = 2;
                int numberCell_Organization = 1;
                for (var i = 0; i < Organizations.Count; i++)
                {
                    Organization Organization = Organizations[i];
                    worksheet_Organization.Cells[startRow_Organization + i, numberCell_Organization].Value = Organization.Code;
                    worksheet_Organization.Cells[startRow_Organization + i, numberCell_Organization + 1].Value = Organization.Name;
                }

                xlPackage.SaveAs(output);
            }
            return File(output.ToArray(), "application/octet-stream", "Template_StoreBalance.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            StoreBalanceFilter StoreBalanceFilter = new StoreBalanceFilter();
            StoreBalanceFilter = await StoreBalanceService.ToFilter(StoreBalanceFilter);
            if (Id == 0)
            {

            }
            else
            {
                StoreBalanceFilter.Id = new IdFilter { Equal = Id };
                int count = await StoreBalanceService.Count(StoreBalanceFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private StoreBalance ConvertDTOToEntity(StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO)
        {
            StoreBalance_StoreBalanceDTO.TrimString();
            StoreBalance StoreBalance = new StoreBalance();
            StoreBalance.Id = StoreBalance_StoreBalanceDTO.Id;
            StoreBalance.OrganizationId = StoreBalance_StoreBalanceDTO.OrganizationId;
            StoreBalance.StoreId = StoreBalance_StoreBalanceDTO.StoreId;
            StoreBalance.CreditAmount = StoreBalance_StoreBalanceDTO.CreditAmount;
            StoreBalance.DebitAmount = StoreBalance_StoreBalanceDTO.DebitAmount;
            StoreBalance.Organization = StoreBalance_StoreBalanceDTO.Organization == null ? null : new Organization
            {
                Id = StoreBalance_StoreBalanceDTO.Organization.Id,
                Code = StoreBalance_StoreBalanceDTO.Organization.Code,
                Name = StoreBalance_StoreBalanceDTO.Organization.Name,
                ParentId = StoreBalance_StoreBalanceDTO.Organization.ParentId,
                Path = StoreBalance_StoreBalanceDTO.Organization.Path,
                Level = StoreBalance_StoreBalanceDTO.Organization.Level,
                StatusId = StoreBalance_StoreBalanceDTO.Organization.StatusId,
                Phone = StoreBalance_StoreBalanceDTO.Organization.Phone,
                Email = StoreBalance_StoreBalanceDTO.Organization.Email,
                Address = StoreBalance_StoreBalanceDTO.Organization.Address,
                IsDisplay = StoreBalance_StoreBalanceDTO.Organization.IsDisplay,
            };
            StoreBalance.Store = StoreBalance_StoreBalanceDTO.Store == null ? null : new Store
            {
                Id = StoreBalance_StoreBalanceDTO.Store.Id,
                Code = StoreBalance_StoreBalanceDTO.Store.Code,
                CodeDraft = StoreBalance_StoreBalanceDTO.Store.CodeDraft,
                Name = StoreBalance_StoreBalanceDTO.Store.Name,
                UnsignName = StoreBalance_StoreBalanceDTO.Store.UnsignName,
                ParentStoreId = StoreBalance_StoreBalanceDTO.Store.ParentStoreId,
                OrganizationId = StoreBalance_StoreBalanceDTO.Store.OrganizationId,
                StoreTypeId = StoreBalance_StoreBalanceDTO.Store.StoreTypeId,
                Telephone = StoreBalance_StoreBalanceDTO.Store.Telephone,
                ProvinceId = StoreBalance_StoreBalanceDTO.Store.ProvinceId,
                DistrictId = StoreBalance_StoreBalanceDTO.Store.DistrictId,
                WardId = StoreBalance_StoreBalanceDTO.Store.WardId,
                Address = StoreBalance_StoreBalanceDTO.Store.Address,
                UnsignAddress = StoreBalance_StoreBalanceDTO.Store.UnsignAddress,
                DeliveryAddress = StoreBalance_StoreBalanceDTO.Store.DeliveryAddress,
                Latitude = StoreBalance_StoreBalanceDTO.Store.Latitude,
                Longitude = StoreBalance_StoreBalanceDTO.Store.Longitude,
                DeliveryLatitude = StoreBalance_StoreBalanceDTO.Store.DeliveryLatitude,
                DeliveryLongitude = StoreBalance_StoreBalanceDTO.Store.DeliveryLongitude,
                OwnerName = StoreBalance_StoreBalanceDTO.Store.OwnerName,
                OwnerPhone = StoreBalance_StoreBalanceDTO.Store.OwnerPhone,
                OwnerEmail = StoreBalance_StoreBalanceDTO.Store.OwnerEmail,
                TaxCode = StoreBalance_StoreBalanceDTO.Store.TaxCode,
                LegalEntity = StoreBalance_StoreBalanceDTO.Store.LegalEntity,
                CreatorId = StoreBalance_StoreBalanceDTO.Store.CreatorId,
                AppUserId = StoreBalance_StoreBalanceDTO.Store.AppUserId,
                StatusId = StoreBalance_StoreBalanceDTO.Store.StatusId,
                Used = StoreBalance_StoreBalanceDTO.Store.Used,
                StoreScoutingId = StoreBalance_StoreBalanceDTO.Store.StoreScoutingId,
                StoreStatusId = StoreBalance_StoreBalanceDTO.Store.StoreStatusId,
                IsStoreApprovalDirectSalesOrder = StoreBalance_StoreBalanceDTO.Store.IsStoreApprovalDirectSalesOrder,
            };
            StoreBalance.BaseLanguage = CurrentContext.Language;
            return StoreBalance;
        }

        private StoreBalanceFilter ConvertFilterDTOToFilterEntity(StoreBalance_StoreBalanceFilterDTO StoreBalance_StoreBalanceFilterDTO)
        {
            StoreBalanceFilter StoreBalanceFilter = new StoreBalanceFilter();
            StoreBalanceFilter.Selects = StoreBalanceSelect.ALL;
            StoreBalanceFilter.Skip = StoreBalance_StoreBalanceFilterDTO.Skip;
            StoreBalanceFilter.Take = StoreBalance_StoreBalanceFilterDTO.Take;
            StoreBalanceFilter.OrderBy = StoreBalance_StoreBalanceFilterDTO.OrderBy;
            StoreBalanceFilter.OrderType = StoreBalance_StoreBalanceFilterDTO.OrderType;

            StoreBalanceFilter.Id = StoreBalance_StoreBalanceFilterDTO.Id;
            StoreBalanceFilter.OrganizationId = StoreBalance_StoreBalanceFilterDTO.OrganizationId;
            StoreBalanceFilter.StoreId = StoreBalance_StoreBalanceFilterDTO.StoreId;
            StoreBalanceFilter.Code = StoreBalance_StoreBalanceFilterDTO.Code;
            StoreBalanceFilter.Name = StoreBalance_StoreBalanceFilterDTO.Name;
            StoreBalanceFilter.CodeDraft = StoreBalance_StoreBalanceFilterDTO.CodeDraft;
            StoreBalanceFilter.CreditAmount = StoreBalance_StoreBalanceFilterDTO.CreditAmount;
            StoreBalanceFilter.DebitAmount = StoreBalance_StoreBalanceFilterDTO.DebitAmount;
            StoreBalanceFilter.CreatedAt = StoreBalance_StoreBalanceFilterDTO.CreatedAt;
            StoreBalanceFilter.UpdatedAt = StoreBalance_StoreBalanceFilterDTO.UpdatedAt;
            return StoreBalanceFilter;
        }
    }
}

