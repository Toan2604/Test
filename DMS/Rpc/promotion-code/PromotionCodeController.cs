using DMS.Common;
using DMS.Entities;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MPromotionCode;
using DMS.Services.MPromotionDiscountType;
using DMS.Services.MPromotionProductAppliedType;
using DMS.Services.MPromotionType;
using DMS.Services.MSalesOrderType;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using DMS.Services.MSupplier;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public partial class PromotionCodeController : RpcController
    {
        private IOrganizationService OrganizationService;
        private IPromotionDiscountTypeService PromotionDiscountTypeService;
        private IPromotionProductAppliedTypeService PromotionProductAppliedTypeService;
        private IPromotionTypeService PromotionTypeService;
        private IStatusService StatusService;
        private ISalesOrderTypeService SalesOrderTypeService;
        private IProductService ProductService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreService StoreService;
        private IStoreTypeService StoreTypeService;
        private ISupplierService SupplierService;
        private IPromotionCodeService PromotionCodeService;
        private ICurrentContext CurrentContext;
        public PromotionCodeController(
            IOrganizationService OrganizationService,
            IPromotionDiscountTypeService PromotionDiscountTypeService,
            IPromotionProductAppliedTypeService PromotionProductAppliedTypeService,
            IPromotionTypeService PromotionTypeService,
            IStatusService StatusService,
            ISalesOrderTypeService SalesOrderTypeService,
            IProductService ProductService,
            IStoreGroupingService StoreGroupingService,
            IStoreService StoreService,
            IStoreTypeService StoreTypeService,
            ISupplierService SupplierService,
            IPromotionCodeService PromotionCodeService,
            ICurrentContext CurrentContext
        )
        {
            this.OrganizationService = OrganizationService;
            this.PromotionDiscountTypeService = PromotionDiscountTypeService;
            this.PromotionProductAppliedTypeService = PromotionProductAppliedTypeService;
            this.PromotionTypeService = PromotionTypeService;
            this.StatusService = StatusService;
            this.SalesOrderTypeService = SalesOrderTypeService;
            this.ProductService = ProductService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreService = StoreService;
            this.StoreTypeService = StoreTypeService;
            this.SupplierService = SupplierService;
            this.PromotionCodeService = PromotionCodeService;
            this.CurrentContext = CurrentContext;
        }

        [Route(PromotionCodeRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] PromotionCode_PromotionCodeFilterDTO PromotionCode_PromotionCodeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionCodeFilter PromotionCodeFilter = ConvertFilterDTOToFilterEntity(PromotionCode_PromotionCodeFilterDTO);
            PromotionCodeFilter = await PromotionCodeService.ToFilter(PromotionCodeFilter);
            int count = await PromotionCodeService.Count(PromotionCodeFilter);
            return count;
        }

        [Route(PromotionCodeRoute.List), HttpPost]
        public async Task<ActionResult<List<PromotionCode_PromotionCodeDTO>>> List([FromBody] PromotionCode_PromotionCodeFilterDTO PromotionCode_PromotionCodeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionCodeFilter PromotionCodeFilter = ConvertFilterDTOToFilterEntity(PromotionCode_PromotionCodeFilterDTO);
            PromotionCodeFilter = await PromotionCodeService.ToFilter(PromotionCodeFilter);
            List<PromotionCode> PromotionCodes = await PromotionCodeService.List(PromotionCodeFilter);
            List<PromotionCode_PromotionCodeDTO> PromotionCode_PromotionCodeDTOs = PromotionCodes
                .Select(c => new PromotionCode_PromotionCodeDTO(c)).ToList();
            return PromotionCode_PromotionCodeDTOs;
        }

        [Route(PromotionCodeRoute.Get), HttpPost]
        public async Task<ActionResult<PromotionCode_PromotionCodeDTO>> Get([FromBody] PromotionCode_PromotionCodeDTO PromotionCode_PromotionCodeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(PromotionCode_PromotionCodeDTO.Id))
                return Forbid();

            PromotionCode PromotionCode = await PromotionCodeService.Get(PromotionCode_PromotionCodeDTO.Id);
            return new PromotionCode_PromotionCodeDTO(PromotionCode);
        }

        [Route(PromotionCodeRoute.Create), HttpPost]
        public async Task<ActionResult<PromotionCode_PromotionCodeDTO>> Create([FromBody] PromotionCode_PromotionCodeDTO PromotionCode_PromotionCodeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(PromotionCode_PromotionCodeDTO.Id))
                return Forbid();

            PromotionCode PromotionCode = ConvertDTOToEntity(PromotionCode_PromotionCodeDTO);
            PromotionCode = await PromotionCodeService.Create(PromotionCode);
            PromotionCode_PromotionCodeDTO = new PromotionCode_PromotionCodeDTO(PromotionCode);
            if (PromotionCode.IsValidated)
                return PromotionCode_PromotionCodeDTO;
            else
                return BadRequest(PromotionCode_PromotionCodeDTO);
        }

        [Route(PromotionCodeRoute.Update), HttpPost]
        public async Task<ActionResult<PromotionCode_PromotionCodeDTO>> Update([FromBody] PromotionCode_PromotionCodeDTO PromotionCode_PromotionCodeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(PromotionCode_PromotionCodeDTO.Id))
                return Forbid();

            PromotionCode PromotionCode = ConvertDTOToEntity(PromotionCode_PromotionCodeDTO);
            PromotionCode = await PromotionCodeService.Update(PromotionCode);
            PromotionCode_PromotionCodeDTO = new PromotionCode_PromotionCodeDTO(PromotionCode);
            if (PromotionCode.IsValidated)
                return PromotionCode_PromotionCodeDTO;
            else
                return BadRequest(PromotionCode_PromotionCodeDTO);
        }

        [Route(PromotionCodeRoute.Delete), HttpPost]
        public async Task<ActionResult<PromotionCode_PromotionCodeDTO>> Delete([FromBody] PromotionCode_PromotionCodeDTO PromotionCode_PromotionCodeDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(PromotionCode_PromotionCodeDTO.Id))
                return Forbid();

            PromotionCode PromotionCode = ConvertDTOToEntity(PromotionCode_PromotionCodeDTO);
            PromotionCode = await PromotionCodeService.Delete(PromotionCode);
            PromotionCode_PromotionCodeDTO = new PromotionCode_PromotionCodeDTO(PromotionCode);
            if (PromotionCode.IsValidated)
                return PromotionCode_PromotionCodeDTO;
            else
                return BadRequest(PromotionCode_PromotionCodeDTO);
        }

        [Route(PromotionCodeRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter();
            PromotionCodeFilter = await PromotionCodeService.ToFilter(PromotionCodeFilter);
            PromotionCodeFilter.Id = new IdFilter { In = Ids };
            PromotionCodeFilter.Selects = PromotionCodeSelect.Id;
            PromotionCodeFilter.Skip = 0;
            PromotionCodeFilter.Take = int.MaxValue;

            List<PromotionCode> PromotionCodes = await PromotionCodeService.List(PromotionCodeFilter);
            PromotionCodes = await PromotionCodeService.BulkDelete(PromotionCodes);
            if (PromotionCodes.Any(x => !x.IsValidated))
                return BadRequest(PromotionCodes.Where(x => !x.IsValidated));
            return true;
        }

        #region import export
        //[Route(PromotionCodeRoute.Import), HttpPost]
        //public async Task<ActionResult> Import(IFormFile file)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);
        //    PromotionDiscountTypeFilter PromotionDiscountTypeFilter = new PromotionDiscountTypeFilter
        //    {
        //        Skip = 0,
        //        Take = int.MaxValue,
        //        Selects = PromotionDiscountTypeSelect.ALL
        //    };
        //    List<PromotionDiscountType> PromotionDiscountTypes = await PromotionDiscountTypeService.List(PromotionDiscountTypeFilter);
        //    PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter = new PromotionProductAppliedTypeFilter
        //    {
        //        Skip = 0,
        //        Take = int.MaxValue,
        //        Selects = PromotionProductAppliedTypeSelect.ALL
        //    };
        //    List<PromotionProductAppliedType> PromotionProductAppliedTypes = await PromotionProductAppliedTypeService.List(PromotionProductAppliedTypeFilter);
        //    PromotionTypeFilter PromotionTypeFilter = new PromotionTypeFilter
        //    {
        //        Skip = 0,
        //        Take = int.MaxValue,
        //        Selects = PromotionTypeSelect.ALL
        //    };
        //    List<PromotionType> PromotionTypes = await PromotionTypeService.List(PromotionTypeFilter);
        //    StatusFilter StatusFilter = new StatusFilter
        //    {
        //        Skip = 0,
        //        Take = int.MaxValue,
        //        Selects = StatusSelect.ALL
        //    };
        //    List<Status> Statuses = await StatusService.List(StatusFilter);
        //    List<PromotionCode> PromotionCodes = new List<PromotionCode>();
        //    using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
        //    {
        //        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
        //        if (worksheet == null)
        //            return Ok(PromotionCodes);
        //        int StartColumn = 1;
        //        int StartRow = 1;
        //        int IdColumn = 0 + StartColumn;
        //        int CodeColumn = 1 + StartColumn;
        //        int NameColumn = 2 + StartColumn;
        //        int QuantityColumn = 3 + StartColumn;
        //        int PromotionDiscountTypeIdColumn = 4 + StartColumn;
        //        int ValueColumn = 5 + StartColumn;
        //        int MaxValueColumn = 6 + StartColumn;
        //        int PromotionTypeIdColumn = 7 + StartColumn;
        //        int PromotionProductAppliedTypeIdColumn = 8 + StartColumn;
        //        int OrganizationIdColumn = 9 + StartColumn;
        //        int StartDateColumn = 10 + StartColumn;
        //        int EndDateColumn = 11 + StartColumn;
        //        int StatusIdColumn = 12 + StartColumn;

        //        for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
        //        {
        //            if (string.IsNullOrEmpty(worksheet.Cells[i + StartRow, StartColumn].Value?.ToString()))
        //                break;
        //            string IdValue = worksheet.Cells[i + StartRow, IdColumn].Value?.ToString();
        //            string CodeValue = worksheet.Cells[i + StartRow, CodeColumn].Value?.ToString();
        //            string NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
        //            string QuantityValue = worksheet.Cells[i + StartRow, QuantityColumn].Value?.ToString();
        //            string PromotionDiscountTypeIdValue = worksheet.Cells[i + StartRow, PromotionDiscountTypeIdColumn].Value?.ToString();
        //            string ValueValue = worksheet.Cells[i + StartRow, ValueColumn].Value?.ToString();
        //            string MaxValueValue = worksheet.Cells[i + StartRow, MaxValueColumn].Value?.ToString();
        //            string PromotionTypeIdValue = worksheet.Cells[i + StartRow, PromotionTypeIdColumn].Value?.ToString();
        //            string PromotionProductAppliedTypeIdValue = worksheet.Cells[i + StartRow, PromotionProductAppliedTypeIdColumn].Value?.ToString();
        //            string OrganizationIdValue = worksheet.Cells[i + StartRow, OrganizationIdColumn].Value?.ToString();
        //            string StartDateValue = worksheet.Cells[i + StartRow, StartDateColumn].Value?.ToString();
        //            string EndDateValue = worksheet.Cells[i + StartRow, EndDateColumn].Value?.ToString();
        //            string StatusIdValue = worksheet.Cells[i + StartRow, StatusIdColumn].Value?.ToString();

        //            PromotionCode PromotionCode = new PromotionCode();
        //            PromotionCode.Code = CodeValue;
        //            PromotionCode.Name = NameValue;
        //            PromotionCode.Quantity = long.TryParse(QuantityValue, out long Quantity) ? Quantity : 0;
        //            PromotionCode.Value = decimal.TryParse(ValueValue, out decimal Value) ? Value : 0;
        //            PromotionCode.MaxValue = decimal.TryParse(MaxValueValue, out decimal MaxValue) ? MaxValue : 0;
        //            PromotionCode.StartDate = DateTime.TryParse(StartDateValue, out DateTime StartDate) ? StartDate : DateTime.Now;
        //            PromotionCode.EndDate = DateTime.TryParse(EndDateValue, out DateTime EndDate) ? EndDate : DateTime.Now;
        //            PromotionDiscountType PromotionDiscountType = PromotionDiscountTypes.Where(x => x.Id.ToString() == PromotionDiscountTypeIdValue).FirstOrDefault();
        //            PromotionCode.PromotionDiscountTypeId = PromotionDiscountType == null ? 0 : PromotionDiscountType.Id;
        //            PromotionCode.PromotionDiscountType = PromotionDiscountType;
        //            PromotionProductAppliedType PromotionProductAppliedType = PromotionProductAppliedTypes.Where(x => x.Id.ToString() == PromotionProductAppliedTypeIdValue).FirstOrDefault();
        //            PromotionCode.PromotionProductAppliedTypeId = PromotionProductAppliedType == null ? 0 : PromotionProductAppliedType.Id;
        //            PromotionCode.PromotionProductAppliedType = PromotionProductAppliedType;
        //            PromotionType PromotionType = PromotionTypes.Where(x => x.Id.ToString() == PromotionTypeIdValue).FirstOrDefault();
        //            PromotionCode.PromotionTypeId = PromotionType == null ? 0 : PromotionType.Id;
        //            PromotionCode.PromotionType = PromotionType;
        //            Status Status = Statuses.Where(x => x.Id.ToString() == StatusIdValue).FirstOrDefault();
        //            PromotionCode.StatusId = Status == null ? 0 : Status.Id;
        //            PromotionCode.Status = Status;

        //            PromotionCodes.Add(PromotionCode);
        //        }
        //    }
        //    PromotionCodes = await PromotionCodeService.Import(PromotionCodes);
        //    if (PromotionCodes.All(x => x.IsValidated))
        //        return Ok(true);
        //    else
        //    {
        //        List<string> Errors = new List<string>();
        //        for (int i = 0; i < PromotionCodes.Count; i++)
        //        {
        //            PromotionCode PromotionCode = PromotionCodes[i];
        //            if (!PromotionCode.IsValidated)
        //            {
        //                string Error = $"Dòng {i + 2} có lỗi:";
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.Id)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.Id)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.Code)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.Code)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.Name)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.Name)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.Quantity)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.Quantity)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.PromotionDiscountTypeId)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.PromotionDiscountTypeId)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.Value)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.Value)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.MaxValue)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.MaxValue)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.PromotionTypeId)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.PromotionTypeId)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.PromotionProductAppliedTypeId)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.PromotionProductAppliedTypeId)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.OrganizationId)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.OrganizationId)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.StartDate)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.StartDate)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.EndDate)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.EndDate)];
        //                if (PromotionCode.Errors.ContainsKey(nameof(PromotionCode.StatusId)))
        //                    Error += PromotionCode.Errors[nameof(PromotionCode.StatusId)];
        //                Errors.Add(Error);
        //            }
        //        }
        //        return BadRequest(Errors);
        //    }
        //}

        //[Route(PromotionCodeRoute.Export), HttpPost]
        //public async Task<FileResult> Export([FromBody] PromotionCode_PromotionCodeFilterDTO PromotionCode_PromotionCodeFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    MemoryStream memoryStream = new MemoryStream();
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {
        //        #region PromotionCode
        //        var PromotionCodeFilter = ConvertFilterDTOToFilterEntity(PromotionCode_PromotionCodeFilterDTO);
        //        PromotionCodeFilter.Skip = 0;
        //        PromotionCodeFilter.Take = int.MaxValue;
        //        PromotionCodeFilter = await PromotionCodeService.ToFilter(PromotionCodeFilter);
        //        List<PromotionCode> PromotionCodes = await PromotionCodeService.List(PromotionCodeFilter);

        //        var PromotionCodeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //                "Quantity",
        //                "PromotionDiscountTypeId",
        //                "Value",
        //                "MaxValue",
        //                "PromotionTypeId",
        //                "PromotionProductAppliedTypeId",
        //                "OrganizationId",
        //                "StartDate",
        //                "EndDate",
        //                "StatusId",
        //            }
        //        };
        //        List<object[]> PromotionCodeData = new List<object[]>();
        //        for (int i = 0; i < PromotionCodes.Count; i++)
        //        {
        //            var PromotionCode = PromotionCodes[i];
        //            PromotionCodeData.Add(new Object[]
        //            {
        //                PromotionCode.Id,
        //                PromotionCode.Code,
        //                PromotionCode.Name,
        //                PromotionCode.Quantity,
        //                PromotionCode.PromotionDiscountTypeId,
        //                PromotionCode.Value,
        //                PromotionCode.MaxValue,
        //                PromotionCode.PromotionTypeId,
        //                PromotionCode.PromotionProductAppliedTypeId,
        //                PromotionCode.OrganizationId,
        //                PromotionCode.StartDate,
        //                PromotionCode.EndDate,
        //                PromotionCode.StatusId,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionCode", PromotionCodeHeaders, PromotionCodeData);
        //        #endregion

        //        #region Organization
        //        var OrganizationFilter = new OrganizationFilter();
        //        OrganizationFilter.Selects = OrganizationSelect.ALL;
        //        OrganizationFilter.OrderBy = OrganizationOrder.Id;
        //        OrganizationFilter.OrderType = OrderType.ASC;
        //        OrganizationFilter.Skip = 0;
        //        OrganizationFilter.Take = int.MaxValue;
        //        List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);

        //        var OrganizationHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //                "ParentId",
        //                "Path",
        //                "Level",
        //                "StatusId",
        //                "Phone",
        //                "Email",
        //                "Address",
        //                "RowId",
        //            }
        //        };
        //        List<object[]> OrganizationData = new List<object[]>();
        //        for (int i = 0; i < Organizations.Count; i++)
        //        {
        //            var Organization = Organizations[i];
        //            OrganizationData.Add(new Object[]
        //            {
        //                Organization.Id,
        //                Organization.Code,
        //                Organization.Name,
        //                Organization.ParentId,
        //                Organization.Path,
        //                Organization.Level,
        //                Organization.StatusId,
        //                Organization.Phone,
        //                Organization.Email,
        //                Organization.Address,
        //                Organization.RowId,
        //            });
        //        }
        //        excel.GenerateWorksheet("Organization", OrganizationHeaders, OrganizationData);
        //        #endregion
        //        #region PromotionDiscountType
        //        var PromotionDiscountTypeFilter = new PromotionDiscountTypeFilter();
        //        PromotionDiscountTypeFilter.Selects = PromotionDiscountTypeSelect.ALL;
        //        PromotionDiscountTypeFilter.OrderBy = PromotionDiscountTypeOrder.Id;
        //        PromotionDiscountTypeFilter.OrderType = OrderType.ASC;
        //        PromotionDiscountTypeFilter.Skip = 0;
        //        PromotionDiscountTypeFilter.Take = int.MaxValue;
        //        List<PromotionDiscountType> PromotionDiscountTypes = await PromotionDiscountTypeService.List(PromotionDiscountTypeFilter);

        //        var PromotionDiscountTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> PromotionDiscountTypeData = new List<object[]>();
        //        for (int i = 0; i < PromotionDiscountTypes.Count; i++)
        //        {
        //            var PromotionDiscountType = PromotionDiscountTypes[i];
        //            PromotionDiscountTypeData.Add(new Object[]
        //            {
        //                PromotionDiscountType.Id,
        //                PromotionDiscountType.Code,
        //                PromotionDiscountType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionDiscountType", PromotionDiscountTypeHeaders, PromotionDiscountTypeData);
        //        #endregion
        //        #region PromotionProductAppliedType
        //        var PromotionProductAppliedTypeFilter = new PromotionProductAppliedTypeFilter();
        //        PromotionProductAppliedTypeFilter.Selects = PromotionProductAppliedTypeSelect.ALL;
        //        PromotionProductAppliedTypeFilter.OrderBy = PromotionProductAppliedTypeOrder.Id;
        //        PromotionProductAppliedTypeFilter.OrderType = OrderType.ASC;
        //        PromotionProductAppliedTypeFilter.Skip = 0;
        //        PromotionProductAppliedTypeFilter.Take = int.MaxValue;
        //        List<PromotionProductAppliedType> PromotionProductAppliedTypes = await PromotionProductAppliedTypeService.List(PromotionProductAppliedTypeFilter);

        //        var PromotionProductAppliedTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> PromotionProductAppliedTypeData = new List<object[]>();
        //        for (int i = 0; i < PromotionProductAppliedTypes.Count; i++)
        //        {
        //            var PromotionProductAppliedType = PromotionProductAppliedTypes[i];
        //            PromotionProductAppliedTypeData.Add(new Object[]
        //            {
        //                PromotionProductAppliedType.Id,
        //                PromotionProductAppliedType.Code,
        //                PromotionProductAppliedType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionProductAppliedType", PromotionProductAppliedTypeHeaders, PromotionProductAppliedTypeData);
        //        #endregion
        //        #region PromotionType
        //        var PromotionTypeFilter = new PromotionTypeFilter();
        //        PromotionTypeFilter.Selects = PromotionTypeSelect.ALL;
        //        PromotionTypeFilter.OrderBy = PromotionTypeOrder.Id;
        //        PromotionTypeFilter.OrderType = OrderType.ASC;
        //        PromotionTypeFilter.Skip = 0;
        //        PromotionTypeFilter.Take = int.MaxValue;
        //        List<PromotionType> PromotionTypes = await PromotionTypeService.List(PromotionTypeFilter);

        //        var PromotionTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> PromotionTypeData = new List<object[]>();
        //        for (int i = 0; i < PromotionTypes.Count; i++)
        //        {
        //            var PromotionType = PromotionTypes[i];
        //            PromotionTypeData.Add(new Object[]
        //            {
        //                PromotionType.Id,
        //                PromotionType.Code,
        //                PromotionType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionType", PromotionTypeHeaders, PromotionTypeData);
        //        #endregion
        //        #region Status
        //        var StatusFilter = new StatusFilter();
        //        StatusFilter.Selects = StatusSelect.ALL;
        //        StatusFilter.OrderBy = StatusOrder.Id;
        //        StatusFilter.OrderType = OrderType.ASC;
        //        StatusFilter.Skip = 0;
        //        StatusFilter.Take = int.MaxValue;
        //        List<Status> Statuses = await StatusService.List(StatusFilter);

        //        var StatusHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> StatusData = new List<object[]>();
        //        for (int i = 0; i < Statuses.Count; i++)
        //        {
        //            var Status = Statuses[i];
        //            StatusData.Add(new Object[]
        //            {
        //                Status.Id,
        //                Status.Code,
        //                Status.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("Status", StatusHeaders, StatusData);
        //        #endregion
        //        #region PromotionCodeHistory
        //        var PromotionCodeHistoryFilter = new PromotionCodeHistoryFilter();
        //        PromotionCodeHistoryFilter.Selects = PromotionCodeHistorySelect.ALL;
        //        PromotionCodeHistoryFilter.OrderBy = PromotionCodeHistoryOrder.Id;
        //        PromotionCodeHistoryFilter.OrderType = OrderType.ASC;
        //        PromotionCodeHistoryFilter.Skip = 0;
        //        PromotionCodeHistoryFilter.Take = int.MaxValue;
        //        List<PromotionCodeHistory> PromotionCodeHistories = await PromotionCodeHistoryService.List(PromotionCodeHistoryFilter);

        //        var PromotionCodeHistoryHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "PromotionCodeId",
        //                "AppliedAt",
        //                "SalesOrderTypeId",
        //                "RowId",
        //            }
        //        };
        //        List<object[]> PromotionCodeHistoryData = new List<object[]>();
        //        for (int i = 0; i < PromotionCodeHistories.Count; i++)
        //        {
        //            var PromotionCodeHistory = PromotionCodeHistories[i];
        //            PromotionCodeHistoryData.Add(new Object[]
        //            {
        //                PromotionCodeHistory.Id,
        //                PromotionCodeHistory.PromotionCodeId,
        //                PromotionCodeHistory.AppliedAt,
        //                PromotionCodeHistory.SalesOrderTypeId,
        //                PromotionCodeHistory.RowId,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionCodeHistory", PromotionCodeHistoryHeaders, PromotionCodeHistoryData);
        //        #endregion
        //        #region SalesOrderType
        //        var SalesOrderTypeFilter = new SalesOrderTypeFilter();
        //        SalesOrderTypeFilter.Selects = SalesOrderTypeSelect.ALL;
        //        SalesOrderTypeFilter.OrderBy = SalesOrderTypeOrder.Id;
        //        SalesOrderTypeFilter.OrderType = OrderType.ASC;
        //        SalesOrderTypeFilter.Skip = 0;
        //        SalesOrderTypeFilter.Take = int.MaxValue;
        //        List<SalesOrderType> SalesOrderTypes = await SalesOrderTypeService.List(SalesOrderTypeFilter);

        //        var SalesOrderTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> SalesOrderTypeData = new List<object[]>();
        //        for (int i = 0; i < SalesOrderTypes.Count; i++)
        //        {
        //            var SalesOrderType = SalesOrderTypes[i];
        //            SalesOrderTypeData.Add(new Object[]
        //            {
        //                SalesOrderType.Id,
        //                SalesOrderType.Code,
        //                SalesOrderType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("SalesOrderType", SalesOrderTypeHeaders, SalesOrderTypeData);
        //        #endregion
        //        #region Product
        //        var ProductFilter = new ProductFilter();
        //        ProductFilter.Selects = ProductSelect.ALL;
        //        ProductFilter.OrderBy = ProductOrder.Id;
        //        ProductFilter.OrderType = OrderType.ASC;
        //        ProductFilter.Skip = 0;
        //        ProductFilter.Take = int.MaxValue;
        //        List<Product> Products = await ProductService.List(ProductFilter);

        //        var ProductHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "SupplierCode",
        //                "Name",
        //                "Description",
        //                "ScanCode",
        //                "ERPCode",
        //                "ProductTypeId",
        //                "SupplierId",
        //                "BrandId",
        //                "UnitOfMeasureId",
        //                "UnitOfMeasureGroupingId",
        //                "SalePrice",
        //                "RetailPrice",
        //                "TaxTypeId",
        //                "StatusId",
        //                "OtherName",
        //                "TechnicalName",
        //                "Note",
        //                "IsNew",
        //                "UsedVariationId",
        //                "Used",
        //                "RowId",
        //            }
        //        };
        //        List<object[]> ProductData = new List<object[]>();
        //        for (int i = 0; i < Products.Count; i++)
        //        {
        //            var Product = Products[i];
        //            ProductData.Add(new Object[]
        //            {
        //                Product.Id,
        //                Product.Code,
        //                Product.SupplierCode,
        //                Product.Name,
        //                Product.Description,
        //                Product.ScanCode,
        //                Product.ERPCode,
        //                Product.ProductTypeId,
        //                Product.SupplierId,
        //                Product.BrandId,
        //                Product.UnitOfMeasureId,
        //                Product.UnitOfMeasureGroupingId,
        //                Product.SalePrice,
        //                Product.RetailPrice,
        //                Product.TaxTypeId,
        //                Product.StatusId,
        //                Product.OtherName,
        //                Product.TechnicalName,
        //                Product.Note,
        //                Product.IsNew,
        //                Product.UsedVariationId,
        //                Product.Used,
        //                Product.RowId,
        //            });
        //        }
        //        excel.GenerateWorksheet("Product", ProductHeaders, ProductData);
        //        #endregion
        //        #region Store
        //        var StoreFilter = new StoreFilter();
        //        StoreFilter.Selects = StoreSelect.ALL;
        //        StoreFilter.OrderBy = StoreOrder.Id;
        //        StoreFilter.OrderType = OrderType.ASC;
        //        StoreFilter.Skip = 0;
        //        StoreFilter.Take = int.MaxValue;
        //        List<Store> Stores = await StoreService.List(StoreFilter);

        //        var StoreHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "CodeDraft",
        //                "Name",
        //                "UnsignName",
        //                "ParentStoreId",
        //                "OrganizationId",
        //                "StoreTypeId",
        //                "StoreGroupingId",
        //                "ResellerId",
        //                "Telephone",
        //                "ProvinceId",
        //                "DistrictId",
        //                "WardId",
        //                "Address",
        //                "UnsignAddress",
        //                "DeliveryAddress",
        //                "Latitude",
        //                "Longitude",
        //                "DeliveryLatitude",
        //                "DeliveryLongitude",
        //                "OwnerName",
        //                "OwnerPhone",
        //                "OwnerEmail",
        //                "TaxCode",
        //                "LegalEntity",
        //                "AppUserId",
        //                "StatusId",
        //                "RowId",
        //                "Used",
        //                "StoreScoutingId",
        //                "StoreStatusId",
        //            }
        //        };
        //        List<object[]> StoreData = new List<object[]>();
        //        for (int i = 0; i < Stores.Count; i++)
        //        {
        //            var Store = Stores[i];
        //            StoreData.Add(new Object[]
        //            {
        //                Store.Id,
        //                Store.Code,
        //                Store.CodeDraft,
        //                Store.Name,
        //                Store.UnsignName,
        //                Store.ParentStoreId,
        //                Store.OrganizationId,
        //                Store.StoreTypeId,
        //                Store.StoreGroupingId,
        //                Store.ResellerId,
        //                Store.Telephone,
        //                Store.ProvinceId,
        //                Store.DistrictId,
        //                Store.WardId,
        //                Store.Address,
        //                Store.UnsignAddress,
        //                Store.DeliveryAddress,
        //                Store.Latitude,
        //                Store.Longitude,
        //                Store.DeliveryLatitude,
        //                Store.DeliveryLongitude,
        //                Store.OwnerName,
        //                Store.OwnerPhone,
        //                Store.OwnerEmail,
        //                Store.TaxCode,
        //                Store.LegalEntity,
        //                Store.AppUserId,
        //                Store.StatusId,
        //                Store.RowId,
        //                Store.Used,
        //                Store.StoreScoutingId,
        //                Store.StoreStatusId,
        //            });
        //        }
        //        excel.GenerateWorksheet("Store", StoreHeaders, StoreData);
        //        #endregion
        //        excel.Save();
        //    }
        //    return File(memoryStream.ToArray(), "application/octet-stream", "PromotionCode.xlsx");
        //}

        //[Route(PromotionCodeRoute.ExportTemplate), HttpPost]
        //public async Task<FileResult> ExportTemplate([FromBody] PromotionCode_PromotionCodeFilterDTO PromotionCode_PromotionCodeFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    MemoryStream memoryStream = new MemoryStream();
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {
        //        #region PromotionCode
        //        var PromotionCodeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //                "Quantity",
        //                "PromotionDiscountTypeId",
        //                "Value",
        //                "MaxValue",
        //                "PromotionTypeId",
        //                "PromotionProductAppliedTypeId",
        //                "OrganizationId",
        //                "StartDate",
        //                "EndDate",
        //                "StatusId",
        //            }
        //        };
        //        List<object[]> PromotionCodeData = new List<object[]>();
        //        excel.GenerateWorksheet("PromotionCode", PromotionCodeHeaders, PromotionCodeData);
        //        #endregion

        //        #region Organization
        //        var OrganizationFilter = new OrganizationFilter();
        //        OrganizationFilter.Selects = OrganizationSelect.ALL;
        //        OrganizationFilter.OrderBy = OrganizationOrder.Id;
        //        OrganizationFilter.OrderType = OrderType.ASC;
        //        OrganizationFilter.Skip = 0;
        //        OrganizationFilter.Take = int.MaxValue;
        //        List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);

        //        var OrganizationHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //                "ParentId",
        //                "Path",
        //                "Level",
        //                "StatusId",
        //                "Phone",
        //                "Email",
        //                "Address",
        //                "RowId",
        //            }
        //        };
        //        List<object[]> OrganizationData = new List<object[]>();
        //        for (int i = 0; i < Organizations.Count; i++)
        //        {
        //            var Organization = Organizations[i];
        //            OrganizationData.Add(new Object[]
        //            {
        //                Organization.Id,
        //                Organization.Code,
        //                Organization.Name,
        //                Organization.ParentId,
        //                Organization.Path,
        //                Organization.Level,
        //                Organization.StatusId,
        //                Organization.Phone,
        //                Organization.Email,
        //                Organization.Address,
        //                Organization.RowId,
        //            });
        //        }
        //        excel.GenerateWorksheet("Organization", OrganizationHeaders, OrganizationData);
        //        #endregion
        //        #region PromotionDiscountType
        //        var PromotionDiscountTypeFilter = new PromotionDiscountTypeFilter();
        //        PromotionDiscountTypeFilter.Selects = PromotionDiscountTypeSelect.ALL;
        //        PromotionDiscountTypeFilter.OrderBy = PromotionDiscountTypeOrder.Id;
        //        PromotionDiscountTypeFilter.OrderType = OrderType.ASC;
        //        PromotionDiscountTypeFilter.Skip = 0;
        //        PromotionDiscountTypeFilter.Take = int.MaxValue;
        //        List<PromotionDiscountType> PromotionDiscountTypes = await PromotionDiscountTypeService.List(PromotionDiscountTypeFilter);

        //        var PromotionDiscountTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> PromotionDiscountTypeData = new List<object[]>();
        //        for (int i = 0; i < PromotionDiscountTypes.Count; i++)
        //        {
        //            var PromotionDiscountType = PromotionDiscountTypes[i];
        //            PromotionDiscountTypeData.Add(new Object[]
        //            {
        //                PromotionDiscountType.Id,
        //                PromotionDiscountType.Code,
        //                PromotionDiscountType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionDiscountType", PromotionDiscountTypeHeaders, PromotionDiscountTypeData);
        //        #endregion
        //        #region PromotionProductAppliedType
        //        var PromotionProductAppliedTypeFilter = new PromotionProductAppliedTypeFilter();
        //        PromotionProductAppliedTypeFilter.Selects = PromotionProductAppliedTypeSelect.ALL;
        //        PromotionProductAppliedTypeFilter.OrderBy = PromotionProductAppliedTypeOrder.Id;
        //        PromotionProductAppliedTypeFilter.OrderType = OrderType.ASC;
        //        PromotionProductAppliedTypeFilter.Skip = 0;
        //        PromotionProductAppliedTypeFilter.Take = int.MaxValue;
        //        List<PromotionProductAppliedType> PromotionProductAppliedTypes = await PromotionProductAppliedTypeService.List(PromotionProductAppliedTypeFilter);

        //        var PromotionProductAppliedTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> PromotionProductAppliedTypeData = new List<object[]>();
        //        for (int i = 0; i < PromotionProductAppliedTypes.Count; i++)
        //        {
        //            var PromotionProductAppliedType = PromotionProductAppliedTypes[i];
        //            PromotionProductAppliedTypeData.Add(new Object[]
        //            {
        //                PromotionProductAppliedType.Id,
        //                PromotionProductAppliedType.Code,
        //                PromotionProductAppliedType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionProductAppliedType", PromotionProductAppliedTypeHeaders, PromotionProductAppliedTypeData);
        //        #endregion
        //        #region PromotionType
        //        var PromotionTypeFilter = new PromotionTypeFilter();
        //        PromotionTypeFilter.Selects = PromotionTypeSelect.ALL;
        //        PromotionTypeFilter.OrderBy = PromotionTypeOrder.Id;
        //        PromotionTypeFilter.OrderType = OrderType.ASC;
        //        PromotionTypeFilter.Skip = 0;
        //        PromotionTypeFilter.Take = int.MaxValue;
        //        List<PromotionType> PromotionTypes = await PromotionTypeService.List(PromotionTypeFilter);

        //        var PromotionTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> PromotionTypeData = new List<object[]>();
        //        for (int i = 0; i < PromotionTypes.Count; i++)
        //        {
        //            var PromotionType = PromotionTypes[i];
        //            PromotionTypeData.Add(new Object[]
        //            {
        //                PromotionType.Id,
        //                PromotionType.Code,
        //                PromotionType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionType", PromotionTypeHeaders, PromotionTypeData);
        //        #endregion
        //        #region Status
        //        var StatusFilter = new StatusFilter();
        //        StatusFilter.Selects = StatusSelect.ALL;
        //        StatusFilter.OrderBy = StatusOrder.Id;
        //        StatusFilter.OrderType = OrderType.ASC;
        //        StatusFilter.Skip = 0;
        //        StatusFilter.Take = int.MaxValue;
        //        List<Status> Statuses = await StatusService.List(StatusFilter);

        //        var StatusHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> StatusData = new List<object[]>();
        //        for (int i = 0; i < Statuses.Count; i++)
        //        {
        //            var Status = Statuses[i];
        //            StatusData.Add(new Object[]
        //            {
        //                Status.Id,
        //                Status.Code,
        //                Status.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("Status", StatusHeaders, StatusData);
        //        #endregion
        //        #region PromotionCodeHistory
        //        var PromotionCodeHistoryFilter = new PromotionCodeHistoryFilter();
        //        PromotionCodeHistoryFilter.Selects = PromotionCodeHistorySelect.ALL;
        //        PromotionCodeHistoryFilter.OrderBy = PromotionCodeHistoryOrder.Id;
        //        PromotionCodeHistoryFilter.OrderType = OrderType.ASC;
        //        PromotionCodeHistoryFilter.Skip = 0;
        //        PromotionCodeHistoryFilter.Take = int.MaxValue;
        //        List<PromotionCodeHistory> PromotionCodeHistories = await PromotionCodeHistoryService.List(PromotionCodeHistoryFilter);

        //        var PromotionCodeHistoryHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "PromotionCodeId",
        //                "AppliedAt",
        //                "SalesOrderTypeId",
        //                "RowId",
        //            }
        //        };
        //        List<object[]> PromotionCodeHistoryData = new List<object[]>();
        //        for (int i = 0; i < PromotionCodeHistories.Count; i++)
        //        {
        //            var PromotionCodeHistory = PromotionCodeHistories[i];
        //            PromotionCodeHistoryData.Add(new Object[]
        //            {
        //                PromotionCodeHistory.Id,
        //                PromotionCodeHistory.PromotionCodeId,
        //                PromotionCodeHistory.AppliedAt,
        //                PromotionCodeHistory.SalesOrderTypeId,
        //                PromotionCodeHistory.RowId,
        //            });
        //        }
        //        excel.GenerateWorksheet("PromotionCodeHistory", PromotionCodeHistoryHeaders, PromotionCodeHistoryData);
        //        #endregion
        //        #region SalesOrderType
        //        var SalesOrderTypeFilter = new SalesOrderTypeFilter();
        //        SalesOrderTypeFilter.Selects = SalesOrderTypeSelect.ALL;
        //        SalesOrderTypeFilter.OrderBy = SalesOrderTypeOrder.Id;
        //        SalesOrderTypeFilter.OrderType = OrderType.ASC;
        //        SalesOrderTypeFilter.Skip = 0;
        //        SalesOrderTypeFilter.Take = int.MaxValue;
        //        List<SalesOrderType> SalesOrderTypes = await SalesOrderTypeService.List(SalesOrderTypeFilter);

        //        var SalesOrderTypeHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "Name",
        //            }
        //        };
        //        List<object[]> SalesOrderTypeData = new List<object[]>();
        //        for (int i = 0; i < SalesOrderTypes.Count; i++)
        //        {
        //            var SalesOrderType = SalesOrderTypes[i];
        //            SalesOrderTypeData.Add(new Object[]
        //            {
        //                SalesOrderType.Id,
        //                SalesOrderType.Code,
        //                SalesOrderType.Name,
        //            });
        //        }
        //        excel.GenerateWorksheet("SalesOrderType", SalesOrderTypeHeaders, SalesOrderTypeData);
        //        #endregion
        //        #region Product
        //        var ProductFilter = new ProductFilter();
        //        ProductFilter.Selects = ProductSelect.ALL;
        //        ProductFilter.OrderBy = ProductOrder.Id;
        //        ProductFilter.OrderType = OrderType.ASC;
        //        ProductFilter.Skip = 0;
        //        ProductFilter.Take = int.MaxValue;
        //        List<Product> Products = await ProductService.List(ProductFilter);

        //        var ProductHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "SupplierCode",
        //                "Name",
        //                "Description",
        //                "ScanCode",
        //                "ERPCode",
        //                "ProductTypeId",
        //                "SupplierId",
        //                "BrandId",
        //                "UnitOfMeasureId",
        //                "UnitOfMeasureGroupingId",
        //                "SalePrice",
        //                "RetailPrice",
        //                "TaxTypeId",
        //                "StatusId",
        //                "OtherName",
        //                "TechnicalName",
        //                "Note",
        //                "IsNew",
        //                "UsedVariationId",
        //                "Used",
        //                "RowId",
        //            }
        //        };
        //        List<object[]> ProductData = new List<object[]>();
        //        for (int i = 0; i < Products.Count; i++)
        //        {
        //            var Product = Products[i];
        //            ProductData.Add(new Object[]
        //            {
        //                Product.Id,
        //                Product.Code,
        //                Product.SupplierCode,
        //                Product.Name,
        //                Product.Description,
        //                Product.ScanCode,
        //                Product.ERPCode,
        //                Product.ProductTypeId,
        //                Product.SupplierId,
        //                Product.BrandId,
        //                Product.UnitOfMeasureId,
        //                Product.UnitOfMeasureGroupingId,
        //                Product.SalePrice,
        //                Product.RetailPrice,
        //                Product.TaxTypeId,
        //                Product.StatusId,
        //                Product.OtherName,
        //                Product.TechnicalName,
        //                Product.Note,
        //                Product.IsNew,
        //                Product.UsedVariationId,
        //                Product.Used,
        //                Product.RowId,
        //            });
        //        }
        //        excel.GenerateWorksheet("Product", ProductHeaders, ProductData);
        //        #endregion
        //        #region Store
        //        var StoreFilter = new StoreFilter();
        //        StoreFilter.Selects = StoreSelect.ALL;
        //        StoreFilter.OrderBy = StoreOrder.Id;
        //        StoreFilter.OrderType = OrderType.ASC;
        //        StoreFilter.Skip = 0;
        //        StoreFilter.Take = int.MaxValue;
        //        List<Store> Stores = await StoreService.List(StoreFilter);

        //        var StoreHeaders = new List<string[]>()
        //        {
        //            new string[] { 
        //                "Id",
        //                "Code",
        //                "CodeDraft",
        //                "Name",
        //                "UnsignName",
        //                "ParentStoreId",
        //                "OrganizationId",
        //                "StoreTypeId",
        //                "StoreGroupingId",
        //                "ResellerId",
        //                "Telephone",
        //                "ProvinceId",
        //                "DistrictId",
        //                "WardId",
        //                "Address",
        //                "UnsignAddress",
        //                "DeliveryAddress",
        //                "Latitude",
        //                "Longitude",
        //                "DeliveryLatitude",
        //                "DeliveryLongitude",
        //                "OwnerName",
        //                "OwnerPhone",
        //                "OwnerEmail",
        //                "TaxCode",
        //                "LegalEntity",
        //                "AppUserId",
        //                "StatusId",
        //                "RowId",
        //                "Used",
        //                "StoreScoutingId",
        //                "StoreStatusId",
        //            }
        //        };
        //        List<object[]> StoreData = new List<object[]>();
        //        for (int i = 0; i < Stores.Count; i++)
        //        {
        //            var Store = Stores[i];
        //            StoreData.Add(new Object[]
        //            {
        //                Store.Id,
        //                Store.Code,
        //                Store.CodeDraft,
        //                Store.Name,
        //                Store.UnsignName,
        //                Store.ParentStoreId,
        //                Store.OrganizationId,
        //                Store.StoreTypeId,
        //                Store.StoreGroupingId,
        //                Store.ResellerId,
        //                Store.Telephone,
        //                Store.ProvinceId,
        //                Store.DistrictId,
        //                Store.WardId,
        //                Store.Address,
        //                Store.UnsignAddress,
        //                Store.DeliveryAddress,
        //                Store.Latitude,
        //                Store.Longitude,
        //                Store.DeliveryLatitude,
        //                Store.DeliveryLongitude,
        //                Store.OwnerName,
        //                Store.OwnerPhone,
        //                Store.OwnerEmail,
        //                Store.TaxCode,
        //                Store.LegalEntity,
        //                Store.AppUserId,
        //                Store.StatusId,
        //                Store.RowId,
        //                Store.Used,
        //                Store.StoreScoutingId,
        //                Store.StoreStatusId,
        //            });
        //        }
        //        excel.GenerateWorksheet("Store", StoreHeaders, StoreData);
        //        #endregion
        //        excel.Save();
        //    }
        //    return File(memoryStream.ToArray(), "application/octet-stream", "PromotionCode.xlsx");
        //}

        #endregion

        private async Task<bool> HasPermission(long Id)
        {
            PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter();
            PromotionCodeFilter = await PromotionCodeService.ToFilter(PromotionCodeFilter);
            if (Id == 0)
            {

            }
            else
            {
                PromotionCodeFilter.Id = new IdFilter { Equal = Id };
                int count = await PromotionCodeService.Count(PromotionCodeFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private PromotionCode ConvertDTOToEntity(PromotionCode_PromotionCodeDTO PromotionCode_PromotionCodeDTO)
        {
            PromotionCode_PromotionCodeDTO.TrimString();
            PromotionCode PromotionCode = new PromotionCode();
            PromotionCode.Id = PromotionCode_PromotionCodeDTO.Id;
            PromotionCode.Code = PromotionCode_PromotionCodeDTO.Code;
            PromotionCode.Name = PromotionCode_PromotionCodeDTO.Name;
            PromotionCode.Quantity = PromotionCode_PromotionCodeDTO.Quantity;
            PromotionCode.PromotionDiscountTypeId = PromotionCode_PromotionCodeDTO.PromotionDiscountTypeId;
            PromotionCode.Value = PromotionCode_PromotionCodeDTO.Value;
            PromotionCode.MaxValue = PromotionCode_PromotionCodeDTO.MaxValue;
            PromotionCode.PromotionTypeId = PromotionCode_PromotionCodeDTO.PromotionTypeId;
            PromotionCode.PromotionProductAppliedTypeId = PromotionCode_PromotionCodeDTO.PromotionProductAppliedTypeId;
            PromotionCode.OrganizationId = PromotionCode_PromotionCodeDTO.OrganizationId;
            PromotionCode.StartDate = PromotionCode_PromotionCodeDTO.StartDate;
            PromotionCode.EndDate = PromotionCode_PromotionCodeDTO.EndDate;
            PromotionCode.StatusId = PromotionCode_PromotionCodeDTO.StatusId;
            PromotionCode.Organization = PromotionCode_PromotionCodeDTO.Organization == null ? null : new Organization
            {
                Id = PromotionCode_PromotionCodeDTO.Organization.Id,
                Code = PromotionCode_PromotionCodeDTO.Organization.Code,
                Name = PromotionCode_PromotionCodeDTO.Organization.Name,
                ParentId = PromotionCode_PromotionCodeDTO.Organization.ParentId,
                Path = PromotionCode_PromotionCodeDTO.Organization.Path,
                Level = PromotionCode_PromotionCodeDTO.Organization.Level,
                StatusId = PromotionCode_PromotionCodeDTO.Organization.StatusId,
                Phone = PromotionCode_PromotionCodeDTO.Organization.Phone,
                Email = PromotionCode_PromotionCodeDTO.Organization.Email,
                Address = PromotionCode_PromotionCodeDTO.Organization.Address,
                RowId = PromotionCode_PromotionCodeDTO.Organization.RowId,
            };
            PromotionCode.PromotionDiscountType = PromotionCode_PromotionCodeDTO.PromotionDiscountType == null ? null : new PromotionDiscountType
            {
                Id = PromotionCode_PromotionCodeDTO.PromotionDiscountType.Id,
                Code = PromotionCode_PromotionCodeDTO.PromotionDiscountType.Code,
                Name = PromotionCode_PromotionCodeDTO.PromotionDiscountType.Name,
            };
            PromotionCode.PromotionProductAppliedType = PromotionCode_PromotionCodeDTO.PromotionProductAppliedType == null ? null : new PromotionProductAppliedType
            {
                Id = PromotionCode_PromotionCodeDTO.PromotionProductAppliedType.Id,
                Code = PromotionCode_PromotionCodeDTO.PromotionProductAppliedType.Code,
                Name = PromotionCode_PromotionCodeDTO.PromotionProductAppliedType.Name,
            };
            PromotionCode.PromotionType = PromotionCode_PromotionCodeDTO.PromotionType == null ? null : new PromotionType
            {
                Id = PromotionCode_PromotionCodeDTO.PromotionType.Id,
                Code = PromotionCode_PromotionCodeDTO.PromotionType.Code,
                Name = PromotionCode_PromotionCodeDTO.PromotionType.Name,
            };
            PromotionCode.Status = PromotionCode_PromotionCodeDTO.Status == null ? null : new Status
            {
                Id = PromotionCode_PromotionCodeDTO.Status.Id,
                Code = PromotionCode_PromotionCodeDTO.Status.Code,
                Name = PromotionCode_PromotionCodeDTO.Status.Name,
            };
            PromotionCode.PromotionCodeHistories = PromotionCode_PromotionCodeDTO.PromotionCodeHistories?
                .Select(x => new PromotionCodeHistory
                {
                    Id = x.Id,
                    AppliedAt = x.AppliedAt,
                    RowId = x.RowId,
                }).ToList();
            PromotionCode.PromotionCodeOrganizationMappings = PromotionCode_PromotionCodeDTO.PromotionCodeOrganizationMappings?
                .Select(x => new PromotionCodeOrganizationMapping
                {
                    OrganizationId = x.OrganizationId,
                    Organization = x.Organization == null ? null : new Organization
                    {
                        Id = x.Organization.Id,
                        Code = x.Organization.Code,
                        Name = x.Organization.Name,
                        ParentId = x.Organization.ParentId,
                        Path = x.Organization.Path,
                        Level = x.Organization.Level,
                        StatusId = x.Organization.StatusId,
                        Phone = x.Organization.Phone,
                        Email = x.Organization.Email,
                        Address = x.Organization.Address,
                        RowId = x.Organization.RowId,
                    },
                }).ToList();
            PromotionCode.PromotionCodeProductMappings = PromotionCode_PromotionCodeDTO.PromotionCodeProductMappings?
                .Select(x => new PromotionCodeProductMapping
                {
                    ProductId = x.ProductId,
                    Product = x.Product == null ? null : new Product
                    {
                        Id = x.Product.Id,
                        Code = x.Product.Code,
                        SupplierCode = x.Product.SupplierCode,
                        Name = x.Product.Name,
                        Description = x.Product.Description,
                        ScanCode = x.Product.ScanCode,
                        ERPCode = x.Product.ERPCode,
                        ProductTypeId = x.Product.ProductTypeId,
                        SupplierId = x.Product.SupplierId,
                        BrandId = x.Product.BrandId,
                        UnitOfMeasureId = x.Product.UnitOfMeasureId,
                        UnitOfMeasureGroupingId = x.Product.UnitOfMeasureGroupingId,
                        SalePrice = x.Product.SalePrice,
                        RetailPrice = x.Product.RetailPrice,
                        TaxTypeId = x.Product.TaxTypeId,
                        StatusId = x.Product.StatusId,
                        OtherName = x.Product.OtherName,
                        TechnicalName = x.Product.TechnicalName,
                        Note = x.Product.Note,
                        IsNew = x.Product.IsNew,
                        UsedVariationId = x.Product.UsedVariationId,
                        Used = x.Product.Used,
                        RowId = x.Product.RowId,
                    },
                }).ToList();
            PromotionCode.PromotionCodeStoreMappings = PromotionCode_PromotionCodeDTO.PromotionCodeStoreMappings?
                .Select(x => new PromotionCodeStoreMapping
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
                        AppUserId = x.Store.AppUserId,
                        StatusId = x.Store.StatusId,
                        RowId = x.Store.RowId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                    },
                }).ToList();
            PromotionCode.BaseLanguage = CurrentContext.Language;
            return PromotionCode;
        }

        private PromotionCodeFilter ConvertFilterDTOToFilterEntity(PromotionCode_PromotionCodeFilterDTO PromotionCode_PromotionCodeFilterDTO)
        {
            PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter();
            PromotionCodeFilter.Selects = PromotionCodeSelect.ALL;
            PromotionCodeFilter.Skip = PromotionCode_PromotionCodeFilterDTO.Skip;
            PromotionCodeFilter.Take = PromotionCode_PromotionCodeFilterDTO.Take;
            PromotionCodeFilter.OrderBy = PromotionCode_PromotionCodeFilterDTO.OrderBy;
            PromotionCodeFilter.OrderType = PromotionCode_PromotionCodeFilterDTO.OrderType;

            PromotionCodeFilter.Id = PromotionCode_PromotionCodeFilterDTO.Id;
            PromotionCodeFilter.Code = PromotionCode_PromotionCodeFilterDTO.Code;
            PromotionCodeFilter.Name = PromotionCode_PromotionCodeFilterDTO.Name;
            PromotionCodeFilter.Quantity = PromotionCode_PromotionCodeFilterDTO.Quantity;
            PromotionCodeFilter.PromotionDiscountTypeId = PromotionCode_PromotionCodeFilterDTO.PromotionDiscountTypeId;
            PromotionCodeFilter.Value = PromotionCode_PromotionCodeFilterDTO.Value;
            PromotionCodeFilter.MaxValue = PromotionCode_PromotionCodeFilterDTO.MaxValue;
            PromotionCodeFilter.PromotionTypeId = PromotionCode_PromotionCodeFilterDTO.PromotionTypeId;
            PromotionCodeFilter.PromotionProductAppliedTypeId = PromotionCode_PromotionCodeFilterDTO.PromotionProductAppliedTypeId;
            PromotionCodeFilter.OrganizationId = PromotionCode_PromotionCodeFilterDTO.OrganizationId;
            PromotionCodeFilter.StartDate = PromotionCode_PromotionCodeFilterDTO.StartDate;
            PromotionCodeFilter.EndDate = PromotionCode_PromotionCodeFilterDTO.EndDate;
            PromotionCodeFilter.StatusId = PromotionCode_PromotionCodeFilterDTO.StatusId;
            PromotionCodeFilter.CreatedAt = PromotionCode_PromotionCodeFilterDTO.CreatedAt;
            PromotionCodeFilter.UpdatedAt = PromotionCode_PromotionCodeFilterDTO.UpdatedAt;
            return PromotionCodeFilter;
        }
    }
}

