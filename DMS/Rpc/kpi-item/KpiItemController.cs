using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MAppUser;
using DMS.Services.MKpiCriteriaItem;
using DMS.Services.MKpiItem;
using DMS.Services.MKpiItemContent;
using DMS.Services.MKpiItemType;
using DMS.Services.MKpiPeriod;
using DMS.Services.MKpiYear;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using DMS.Services.MStatus;
using DMS.Services.MSupplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public partial class KpiItemController : RpcController
    {
        private IAppUserService AppUserService;
        private IKpiPeriodService KpiPeriodService;
        private IKpiYearService KpiYearService;
        private IOrganizationService OrganizationService;
        private IKpiCriteriaItemService KpiCriteriaItemService;
        private IStatusService StatusService;
        private IItemService ItemService;
        private IKpiItemContentService KpiItemContentService;
        private IKpiItemService KpiItemService;
        private IKpiItemTypeService KpiItemTypeService;
        private ISupplierService SupplierService;
        private IProductTypeService ProductTypeService;
        private IProductGroupingService ProductGroupingService;
        private ICurrentContext CurrentContext;
        public KpiItemController(
            IAppUserService AppUserService,
            IKpiPeriodService KpiPeriodService,
            IKpiYearService KpiYearService,
            IOrganizationService OrganizationService,
            IKpiCriteriaItemService KpiCriteriaItemService,
            IStatusService StatusService,
            IItemService ItemService,
            IKpiItemContentService KpiItemContentService,
            IKpiItemService KpiItemService,
            IKpiItemTypeService KpiItemTypeService,
            ISupplierService SupplierService,
            IProductTypeService ProductTypeService,
            IProductGroupingService ProductGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.KpiPeriodService = KpiPeriodService;
            this.KpiYearService = KpiYearService;
            this.OrganizationService = OrganizationService;
            this.KpiCriteriaItemService = KpiCriteriaItemService;
            this.StatusService = StatusService;
            this.ItemService = ItemService;
            this.KpiItemContentService = KpiItemContentService;
            this.KpiItemService = KpiItemService;
            this.KpiItemTypeService = KpiItemTypeService;
            this.SupplierService = SupplierService;
            this.ProductTypeService = ProductTypeService;
            this.ProductGroupingService = ProductGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(KpiItemRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] KpiItem_KpiItemFilterDTO KpiItem_KpiItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiItemFilter KpiItemFilter = ConvertFilterDTOToFilterEntity(KpiItem_KpiItemFilterDTO);
            KpiItemFilter = await KpiItemService.ToFilter(KpiItemFilter);
            int count = await KpiItemService.Count(KpiItemFilter);
            return count;
        }

        [Route(KpiItemRoute.List), HttpPost]
        public async Task<List<KpiItem_KpiItemDTO>> List([FromBody] KpiItem_KpiItemFilterDTO KpiItem_KpiItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiItemFilter KpiItemFilter = ConvertFilterDTOToFilterEntity(KpiItem_KpiItemFilterDTO);
            KpiItemFilter = await KpiItemService.ToFilter(KpiItemFilter);
            List<KpiItem> KpiItems = await KpiItemService.List(KpiItemFilter);
            List<KpiItem_KpiItemDTO> KpiItem_KpiItemDTOs = KpiItems
                .Select(c => new KpiItem_KpiItemDTO(c)).ToList();
            return KpiItem_KpiItemDTOs;
        }

        [Route(KpiItemRoute.Get), HttpPost]
        public async Task<ActionResult<KpiItem_KpiItemDTO>> Get([FromBody] KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiItem_KpiItemDTO.Id))
                return Forbid();

            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL,
            });

            KpiItem KpiItem = await KpiItemService.Get(KpiItem_KpiItemDTO.Id);
            KpiItem_KpiItemDTO = new KpiItem_KpiItemDTO(KpiItem);
            KpiItem_KpiItemDTO.KpiCriteriaItems = KpiCriteriaItems.Select(x => new KpiItem_KpiCriteriaItemDTO(x)).ToList();
            return KpiItem_KpiItemDTO;
        }

        [Route(KpiItemRoute.GetDraft), HttpPost]
        public async Task<ActionResult<KpiItem_KpiItemDTO>> GetDraft()
        {
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL,
            });
            var KpiItem_KpiItemDTO = new KpiItem_KpiItemDTO();
            (KpiItem_KpiItemDTO.CurrentMonth, KpiItem_KpiItemDTO.CurrentQuarter, KpiItem_KpiItemDTO.CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            KpiItem_KpiItemDTO.KpiYearId = KpiYearId;
            KpiItem_KpiItemDTO.KpiYear = KpiItem_KpiItemDTO.KpiYear = Enums.KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId)
                .Select(x => new KpiItem_KpiYearDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();
            KpiItem_KpiItemDTO.KpiPeriodId = KpiPeriodId;
            KpiItem_KpiItemDTO.KpiPeriod = KpiItem_KpiItemDTO.KpiPeriod = Enums.KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Id == KpiPeriodId)
                .Select(x => new KpiItem_KpiPeriodDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();
            KpiItem_KpiItemDTO.KpiCriteriaItems = KpiCriteriaItems.Select(x => new KpiItem_KpiCriteriaItemDTO(x)).ToList();
            KpiItem_KpiItemDTO.Status = new KpiItem_StatusDTO
            {
                Code = Enums.StatusEnum.ACTIVE.Code,
                Id = Enums.StatusEnum.ACTIVE.Id,
                Name = Enums.StatusEnum.ACTIVE.Name
            };
            KpiItem_KpiItemDTO.StatusId = Enums.StatusEnum.ACTIVE.Id;
            return KpiItem_KpiItemDTO;
        }

        [Route(KpiItemRoute.Create), HttpPost]
        public async Task<ActionResult<KpiItem_KpiItemDTO>> Create([FromBody] KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiItem_KpiItemDTO.Id))
                return Forbid();

            KpiItem KpiItem = ConvertDTOToEntity(KpiItem_KpiItemDTO);
            KpiItem = await KpiItemService.Create(KpiItem);
            KpiItem_KpiItemDTO = new KpiItem_KpiItemDTO(KpiItem);
            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL,
            });
            KpiItem_KpiItemDTO.KpiCriteriaItems = KpiCriteriaItems.Select(x => new KpiItem_KpiCriteriaItemDTO(x)).ToList();
            if (KpiItem.IsValidated)
                return KpiItem_KpiItemDTO;
            else
                return BadRequest(KpiItem_KpiItemDTO);
        }

        [Route(KpiItemRoute.Update), HttpPost]
        public async Task<ActionResult<KpiItem_KpiItemDTO>> Update([FromBody] KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiItem_KpiItemDTO.Id))
                return Forbid();

            KpiItem KpiItem = ConvertDTOToEntity(KpiItem_KpiItemDTO);
            KpiItem = await KpiItemService.Update(KpiItem);
            KpiItem_KpiItemDTO = new KpiItem_KpiItemDTO(KpiItem);
            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL,
            });
            KpiItem_KpiItemDTO.KpiCriteriaItems = KpiCriteriaItems.Select(x => new KpiItem_KpiCriteriaItemDTO(x)).ToList();
            if (KpiItem.IsValidated)
                return KpiItem_KpiItemDTO;
            else
                return BadRequest(KpiItem_KpiItemDTO);
        }

        [Route(KpiItemRoute.Delete), HttpPost]
        public async Task<ActionResult<KpiItem_KpiItemDTO>> Delete([FromBody] KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiItem_KpiItemDTO.Id))
                return Forbid();

            KpiItem KpiItem = ConvertDTOToEntity(KpiItem_KpiItemDTO);
            KpiItem = await KpiItemService.Delete(KpiItem);
            KpiItem_KpiItemDTO = new KpiItem_KpiItemDTO(KpiItem);
            if (KpiItem.IsValidated)
                return KpiItem_KpiItemDTO;
            else
                return BadRequest(KpiItem_KpiItemDTO);
        }

        [Route(KpiItemRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiItemFilter KpiItemFilter = new KpiItemFilter();
            KpiItemFilter = await KpiItemService.ToFilter(KpiItemFilter);
            KpiItemFilter.Id = new IdFilter { In = Ids };
            KpiItemFilter.Selects = KpiItemSelect.Id;
            KpiItemFilter.Skip = 0;
            KpiItemFilter.Take = int.MaxValue;

            List<KpiItem> KpiItems = await KpiItemService.List(KpiItemFilter);
            KpiItems = await KpiItemService.BulkDelete(KpiItems);
            return true;
        }

        [Route(KpiItemRoute.Import), HttpPost]
        public async Task<ActionResult> Import([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL,
            });
            KpiCriteriaItems = KpiCriteriaItems.OrderBy(x => x.Id).ToList();

            StringBuilder errorContent = new StringBuilder();
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
            {
                errorContent.AppendLine("Định dạng file không hợp lệ");
                return BadRequest(errorContent.ToString());
            }

            GenericEnum KpiYear;
            GenericEnum KpiPeriod;
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["KPI sản phẩm"];
                if (worksheet == null)
                {
                    errorContent.AppendLine("File không đúng biểu mẫu import");
                    return BadRequest(errorContent.ToString());
                }

                string KpiPeriodValue = worksheet.Cells[2, 3].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(KpiPeriodValue))
                    KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Name == KpiPeriodValue).FirstOrDefault();
                else
                {
                    errorContent.AppendLine("Chưa chọn kỳ Kpi hoặc kỳ Kpi không hợp lệ");
                    return BadRequest(errorContent.ToString());
                }

                string KpiYearValue = worksheet.Cells[2, 5].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(KpiYearValue))
                    KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Name == KpiYearValue.Trim()).FirstOrDefault();
                else
                {
                    errorContent.AppendLine("Chưa chọn năm Kpi hoặc năm Kpi không hợp lệ");
                    return BadRequest(errorContent.ToString());
                }

                int StartCriteriaHeaderColumn = 5;
                bool HeaderValidator = true;
                for (int i = 0; i < KpiCriteriaItems.Count; i++)
                {
                    string CriteriaHeader = worksheet.Cells[4, StartCriteriaHeaderColumn + i].Value?.ToString();
                    if (CriteriaHeader != KpiCriteriaItems[i].Name)
                    {
                        errorContent.AppendLine($"Sai biểu mẫu: Cột {StartCriteriaHeaderColumn + i} phải nhập \"{KpiCriteriaItems[i].Name}\"");
                        HeaderValidator = false;
                    }
                }
                if (!HeaderValidator)
                    return BadRequest(errorContent.ToString());
            }

            HashSet<long> KpiPeriodIds = new HashSet<long>();
            long CurrentMonth = 100 + StaticParams.DateTimeNow.Month;
            long CurrentQuater = 0;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH01.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH03.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER01.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH04.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH06.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER02.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH07.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH09.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER03.Id;
            if (Enums.KpiPeriodEnum.PERIOD_MONTH10.Id <= CurrentMonth && CurrentMonth <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
                CurrentQuater = Enums.KpiPeriodEnum.PERIOD_QUATER04.Id;
            if (KpiYear.Id >= StaticParams.DateTimeNow.Year)
            {
                KpiPeriodIds.Add(KpiYear.Id);
            }
            foreach (var kpiPeriod in KpiPeriodEnum.KpiPeriodEnumList)
            {
                if (CurrentMonth <= kpiPeriod.Id && kpiPeriod.Id <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
                    KpiPeriodIds.Add(kpiPeriod.Id);
                if (CurrentQuater <= kpiPeriod.Id && kpiPeriod.Id <= Enums.KpiPeriodEnum.PERIOD_QUATER04.Id)
                    KpiPeriodIds.Add(kpiPeriod.Id);
            }

            if (!KpiPeriodIds.Contains(KpiPeriod.Id))
            {
                errorContent.AppendLine("Không thể nhập Kpi cho các kỳ trong quá khứ");
                return BadRequest(errorContent.ToString());
            }

            var AppUser = await AppUserService.Get(CurrentContext.UserId);
            AppUserFilter EmployeeFilter = new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName | AppUserSelect.Organization,
                Id = new IdFilter { }
            };
            EmployeeFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<AppUser> Employees = await AppUserService.List(EmployeeFilter);
            var AppUserIds = Employees.Select(x => x.Id).ToList();
            List<KpiItem> KpiItems = await KpiItemService.List(new KpiItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                AppUserId = new IdFilter { In = AppUserIds },
                KpiYearId = new IdFilter { Equal = KpiYear.Id },
                KpiPeriodId = new IdFilter { Equal = KpiPeriod.Id },
                Selects = KpiItemSelect.ALL
            });
            var KpiItemIds = KpiItems.Select(x => x.Id).ToList();
            List<KpiItemContent> KpiItemContents = await KpiItemContentService.List(new KpiItemContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiItemContentSelect.ALL,
                KpiItemId = new IdFilter { In = KpiItemIds },
            });
            foreach (KpiItem KpiItem in KpiItems)
            {
                KpiItem.KpiItemContents = KpiItemContents.Where(x => x.KpiItemId == KpiItem.Id).ToList();
            }

            List<Item> Items = await ItemService.List(new ItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ItemSelect.Id | ItemSelect.Code | ItemSelect.Name,
                OrderBy = ItemOrder.Id,
                OrderType = OrderType.ASC
            });

            List<KpiItem_ImportDTO> KpiItem_ImportDTOs = new List<KpiItem_ImportDTO>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["KPI sản phẩm"];

                int StartColumn = 1;
                int StartRow = 5;
                int UserNameColumn = 0 + StartColumn;
                int DisplayNameColumn = 1 + StartColumn;
                int KpiItemTypeColumn = 2 + StartColumn;
                int ItemCodeColumn = 3 + StartColumn;

                int RevenueColumn = 4 + StartColumn;
                //int StoreColumn = 5 + StartColumn;

                int CriteriaStartColumn = 4 + StartColumn;

                //int DirectRevenueColumn = 6 + StartColumn;
                //int DirectStoreColumn = 7 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string UserNameValue = worksheet.Cells[i, UserNameColumn].Value?.ToString();
                    string DisplayNameValue = worksheet.Cells[i, DisplayNameColumn].Value?.ToString();
                    string KpiItemTypeValue = worksheet.Cells[i, KpiItemTypeColumn].Value?.ToString();
                    string ItemCodeValue = worksheet.Cells[i, ItemCodeColumn].Value?.ToString();
                    if (UserNameValue != null && UserNameValue.ToLower() == "END".ToLower())
                        break;
                    else if (!string.IsNullOrWhiteSpace(UserNameValue) && string.IsNullOrWhiteSpace(DisplayNameValue) && string.IsNullOrWhiteSpace(ItemCodeValue))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(UserNameValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa nhập mã nhân viên");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(UserNameValue) && i == worksheet.Dimension.End.Row)
                        break;

                    KpiItem_ImportDTO KpiItem_ImportDTO = new KpiItem_ImportDTO();
                    AppUser Employee = Employees.Where(x => x.Username.ToLower() == UserNameValue.ToLower()).FirstOrDefault();
                    if (Employee == null)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Nhân viên không tồn tại");
                        continue;
                    }
                    else
                    {
                        KpiItem_ImportDTO.EmployeeId = Employee.Id;
                    }
                    GenericEnum KpiItemType;
                    if (!string.IsNullOrWhiteSpace(KpiItemTypeValue))
                    {
                        KpiItemType = KpiItemTypeEnum.KpiItemTypeEnumList.Where(x => x.Name == KpiItemTypeValue.Trim()).FirstOrDefault();
                        if (KpiItemType == null)
                        {
                            errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Loại KPI sản phẩm không tồn tại");
                            continue;
                        }
                        else
                        {
                            KpiItem_ImportDTO.KpiItemTypeId = KpiItemType.Id;
                        }
                    }
                    else
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i}: Chưa chọn loại KPI sản phẩm");
                        continue;
                    }
                    Item Item;
                    if (!string.IsNullOrWhiteSpace(ItemCodeValue))
                    {
                        Item = Items.Where(x => x.Code.ToLower() == ItemCodeValue.ToLower().Trim()).FirstOrDefault();
                        if (Item == null)
                        {
                            errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Sản phẩm không tồn tại");
                            continue;
                        }
                        else
                        {
                            KpiItem_ImportDTO.ItemId = Item.Id;
                        }
                    }

                    KpiItem_ImportDTO.Stt = i;
                    KpiItem_ImportDTO.UsernameValue = UserNameValue;
                    KpiItem_ImportDTO.ItemCodeValue = ItemCodeValue;
                    KpiItem_ImportDTO.KpiPeriodId = KpiPeriod.Id;
                    KpiItem_ImportDTO.KpiYearId = KpiYear.Id;
                    KpiItem_ImportDTO.CriteriaContents = new List<KpiItem_CriteriaContentDTO>();

                    for (int j = 0; j < KpiCriteriaItems.Count; j++)
                    {
                        KpiItem_CriteriaContentDTO KpiItem_CriteriaContentDTO = new KpiItem_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaItems[j].Id,
                            CriteriaName = KpiCriteriaItems[j].Name,
                            Value = worksheet.Cells[i, CriteriaStartColumn + j].Value?.ToString()
                        };
                        KpiItem_ImportDTO.CriteriaContents.Add(KpiItem_CriteriaContentDTO);
                    }
                    KpiItem_ImportDTOs.Add(KpiItem_ImportDTO);
                }
            }

            Dictionary<long, StringBuilder> Errors = new Dictionary<long, StringBuilder>();
            HashSet<KpiItem_RowDTO> KpiItem_RowDTOs = new HashSet<KpiItem_RowDTO>(KpiItems.Select(x => new KpiItem_RowDTO
            {
                AppUserId = x.EmployeeId,
                KpiPeriodId = x.KpiPeriodId,
                KpiYearId = x.KpiYearId,
                KpiItemTypeId = x.KpiItemTypeId
            }).ToList());
            foreach (KpiItem_ImportDTO KpiItem_ImportDTO in KpiItem_ImportDTOs)
            {
                Errors.Add(KpiItem_ImportDTO.Stt, new StringBuilder(""));
                KpiItem_ImportDTO.IsNew = false;
                if (!KpiItem_RowDTOs.Contains(new KpiItem_RowDTO
                {
                    AppUserId = KpiItem_ImportDTO.EmployeeId,
                    KpiPeriodId = KpiItem_ImportDTO.KpiPeriodId,
                    KpiYearId = KpiItem_ImportDTO.KpiYearId,
                    KpiItemTypeId = KpiItem_ImportDTO.KpiItemTypeId
                }
                ))
                {
                    KpiItem_RowDTOs.Add(new KpiItem_RowDTO
                    {
                        AppUserId = KpiItem_ImportDTO.EmployeeId,
                        KpiPeriodId = KpiItem_ImportDTO.KpiPeriodId,
                        KpiYearId = KpiItem_ImportDTO.KpiYearId,
                        KpiItemTypeId = KpiItem_ImportDTO.KpiItemTypeId
                    });
                    KpiItem_ImportDTO.IsNew = true;

                    var Employee = Employees.Where(x => x.Username.ToLower() == KpiItem_ImportDTO.UsernameValue.ToLower()).FirstOrDefault();
                    KpiItem_ImportDTO.OrganizationId = Employee.OrganizationId;
                    KpiItem_ImportDTO.EmployeeId = Employee.Id;
                }
            }

            foreach (var KpiItem_ImportDTO in KpiItem_ImportDTOs)
            {
                if (KpiItem_ImportDTO.HasValue == false)
                {
                    Errors[KpiItem_ImportDTO.Stt].Append($"Lỗi dòng thứ {KpiItem_ImportDTO.Stt}: Chưa nhập chỉ tiêu");
                    continue;
                }
                KpiItem KpiItem;
                if (KpiItem_ImportDTO.IsNew)
                {
                    KpiItem = new KpiItem();
                    KpiItems.Add(KpiItem);
                    KpiItem.EmployeeId = KpiItem_ImportDTO.EmployeeId;
                    KpiItem.OrganizationId = KpiItem_ImportDTO.OrganizationId;
                    KpiItem.KpiPeriodId = KpiItem_ImportDTO.KpiPeriodId;
                    KpiItem.KpiYearId = KpiItem_ImportDTO.KpiYearId;
                    KpiItem.KpiItemTypeId = KpiItem_ImportDTO.KpiItemTypeId;
                    KpiItem.RowId = Guid.NewGuid();
                    KpiItem.CreatedAt = StaticParams.DateTimeNow;
                    KpiItem.KpiItemContents = new List<KpiItemContent>();
                    KpiItem.KpiItemContents.Add(new KpiItemContent
                    {
                        ItemId = KpiItem_ImportDTO.ItemId,
                        RowId = Guid.NewGuid(),
                        KpiItemContentKpiCriteriaItemMappings = KpiCriteriaItems.Select(x => new KpiItemContentKpiCriteriaItemMapping
                        {
                            KpiCriteriaItemId = x.Id,
                        }).ToList()
                    });
                }
                else
                {
                    KpiItem = KpiItems.Where(x => x.EmployeeId == KpiItem_ImportDTO.EmployeeId &&
                    x.KpiPeriodId == KpiItem_ImportDTO.KpiPeriodId &&
                    x.KpiYearId == KpiItem_ImportDTO.KpiYearId &&
                    x.KpiItemTypeId == KpiItem_ImportDTO.KpiItemTypeId)
                        .FirstOrDefault();
                    var content = KpiItem.KpiItemContents.Where(x => x.ItemId == KpiItem_ImportDTO.ItemId).FirstOrDefault();
                    if (content == null)
                    {
                        KpiItem.KpiItemContents.Add(new KpiItemContent
                        {
                            ItemId = KpiItem_ImportDTO.ItemId,
                            RowId = Guid.NewGuid(),
                            KpiItemContentKpiCriteriaItemMappings = KpiCriteriaItems.Select(x => new KpiItemContentKpiCriteriaItemMapping
                            {
                                KpiCriteriaItemId = x.Id,
                            }).ToList()
                        });
                    }
                }

                KpiItemContent KpiItemContent = KpiItem.KpiItemContents.Where(x => x.ItemId == KpiItem_ImportDTO.ItemId).FirstOrDefault();
                if (KpiItemContent != null)
                {
                    foreach (var KpiItemContentKpiCriteriaItemMapping in KpiItemContent.KpiItemContentKpiCriteriaItemMappings)
                    {
                        var CriteriaContent = KpiItem_ImportDTO.CriteriaContents.Where(x => x.CriteriaId == KpiItemContentKpiCriteriaItemMapping.KpiCriteriaItemId).FirstOrDefault();

                        if (long.TryParse(CriteriaContent.Value, out long value))
                        {
                            KpiItemContentKpiCriteriaItemMapping.Value = value;
                        }
                    }
                }

                KpiItem.CreatorId = AppUser.Id;
                KpiItem.StatusId = StatusEnum.ACTIVE.Id;
            }
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());
            string error = string.Join("\n", Errors.Where(x => !string.IsNullOrWhiteSpace(x.Value.ToString())).Select(x => x.Value.ToString()).ToList());
            if (!string.IsNullOrWhiteSpace(error))
                return BadRequest(error);

            KpiItems = await KpiItemService.Import(KpiItems);
            List<KpiItem_KpiItemDTO> KpiItem_KpiItemDTOs = KpiItems
                .Select(c => new KpiItem_KpiItemDTO(c)).ToList();
            return Ok(KpiItem_KpiItemDTOs);
        }

        [Route(KpiItemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiItem_KpiItemFilterDTO KpiItem_KpiItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (KpiItem_KpiItemFilterDTO.KpiYearId.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn năm Kpi" });

            if (KpiItem_KpiItemFilterDTO.KpiPeriodId.Equal.HasValue == false)
                return BadRequest(new { message = "Chưa chọn kỳ Kpi" });

            long KpiYearId = KpiItem_KpiItemFilterDTO.KpiYearId.Equal.Value;
            var KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId).FirstOrDefault();

            long KpiPeriodId = KpiItem_KpiItemFilterDTO.KpiPeriodId.Equal.Value;
            var KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Id == KpiPeriodId).FirstOrDefault();

            KpiItem_KpiItemFilterDTO.Skip = 0;
            KpiItem_KpiItemFilterDTO.Take = int.MaxValue;
            KpiItem_KpiItemFilterDTO.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            List<KpiItem_KpiItemDTO> KpiItem_KpiItemDTOs = await List(KpiItem_KpiItemFilterDTO);
            var KpiItemIds = KpiItem_KpiItemDTOs.Select(x => x.Id).ToList();

            KpiItemContentFilter KpiItemContentFilter = new KpiItemContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiItemContentSelect.ALL,
                KpiItemId = new IdFilter { In = KpiItemIds }
            };
            List<KpiItemContent> KpiItemContents = await KpiItemContentService.List(KpiItemContentFilter);

            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL
            });
            KpiCriteriaItems = KpiCriteriaItems.OrderBy(x => x.Id).ToList();

            List<KpiItem_ExportDTO> KpiItem_ExportDTOs = new List<KpiItem_ExportDTO>();
            foreach (var KpiItem in KpiItem_KpiItemDTOs)
            {
                KpiItem_ExportDTO KpiItem_ExportDTO = new KpiItem_ExportDTO();
                KpiItem_ExportDTO.Username = KpiItem.Employee.Username;
                KpiItem_ExportDTO.DisplayName = KpiItem.Employee.DisplayName;
                KpiItem_ExportDTO.KpiItemTypeName = KpiItem.KpiItemType.Name;
                KpiItem_ExportDTO.Contents = new List<KpiItem_ExportContentDTO>();

                var subContents = KpiItemContents.Where(x => x.KpiItemId == KpiItem.Id);

                foreach (var content in subContents)
                {
                    KpiItem_ExportContentDTO KpiItem_ExportContentDTO = new KpiItem_ExportContentDTO();
                    KpiItem_ExportContentDTO.UserName = KpiItem.Employee.Username;
                    KpiItem_ExportContentDTO.DisplayName = KpiItem.Employee.DisplayName;
                    KpiItem_ExportContentDTO.KpiItemTypeName = KpiItem.KpiItemType.Name;
                    KpiItem_ExportContentDTO.ItemCode = content.Item.Code;
                    KpiItem_ExportContentDTO.ItemName = content.Item.Name;
                    KpiItem_ExportContentDTO.CriteriaContents = new List<KpiItem_ExportCriteriaContent>();
                    for (int i = 0; i < KpiCriteriaItems.Count; i++)
                    {
                        long? value = content.KpiItemContentKpiCriteriaItemMappings.Where(x => x.KpiCriteriaItemId == KpiCriteriaItems[i].Id)
                            .Select(x => x.Value).FirstOrDefault();
                        KpiItem_ExportCriteriaContent KpiItem_ExportCriteriaContent = new KpiItem_ExportCriteriaContent
                        {
                            CriteriaId = KpiCriteriaItems[i].Id,
                            CriteriaName = KpiCriteriaItems[i].Name,
                            Value = value,
                        };
                        KpiItem_ExportContentDTO.CriteriaContents.Add(KpiItem_ExportCriteriaContent);
                    }
                    KpiItem_ExportDTO.Contents.Add(KpiItem_ExportContentDTO);
                }
                KpiItem_ExportDTOs.Add(KpiItem_ExportDTO);
            }

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("KPI sản phẩm");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = "THIẾT LẬP KPI SẢN PHẨM TRỌNG TÂM";

                ws.Cells["A2"].Value = $"Kỳ: {KpiPeriod.Name} - Năm: {KpiYear.Name}";
                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "Mã nhân viên",
                    "Tên nhân viên",
                    "Loại KPI sản phẩm",
                    "Mã sản phẩm",
                    "Tên sản phẩm"
                };

                for (int i = 0; i < KpiCriteriaItems.Count; i++)
                {
                    headers.Add(KpiCriteriaItems[i].Name);
                }

                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64);

                // format lại tiêu đề theo số cột của dữ liệu
                ws.Cells[$"A1:{endColumnString}1"].Merge = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Size = 14;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Bold = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells[$"A2:{endColumnString}2"].Merge = true;
                ws.Cells[$"A2:{endColumnString}2"].Style.Font.Bold = true;
                ws.Cells[$"A2:{endColumnString}2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                string headerRange = $"A4:" + $"{endColumnString}4";
                List<string[]> Header = new List<string[]> { headers.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#9BC2E6"));

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells[headerRange].AutoFitColumns();
                #endregion

                #region Du lieu bao cao
                int startRow = 5;
                int endRow = startRow;
                if (KpiItem_ExportDTOs != null && KpiItem_ExportDTOs.Count > 0)
                {
                    foreach (var KpiItem_ExportDTO in KpiItem_ExportDTOs)
                    {
                        int startColumn = 1; // gán lại cột bắt đầu từ A
                        int endColumn = startColumn;

                        ws.Cells[$"A{startRow}"].Value = $"{KpiItem_ExportDTO.Username} - {KpiItem_ExportDTO.DisplayName}";
                        ws.Cells[$"A{startRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Merge = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

                        endRow = startRow; // Gán lại endrow = start row dể bắt đầu vùng dữ liệu mới
                        #region Cột mã nhân viên, tên nhân viên, mã nhóm sản phẩm, tên nhóm sản phẩm
                        List<Object[]> KpiData = new List<object[]>();
                        foreach (var content in KpiItem_ExportDTO.Contents)
                        {
                            KpiData.Add(new object[]
                            {
                                content.UserName,
                                content.DisplayName,
                                content.KpiItemTypeName,
                                content.ItemCode,
                                content.ItemName,
                            });
                            endRow++;
                        }
                        string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{startColumnString}{startRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(KpiData);
                        // dòng đầu tiên là dòng tên nhân viên nên startRow + 1
                        endColumn += 5; // chiếm 5 cột
                        startColumn = endColumn;
                        #endregion

                        #region Các cột giá trị
                        for (int i = 0; i < KpiCriteriaItems.Count; i++)
                        {
                            List<Object[]> ValueData = new List<object[]>();
                            endRow = startRow; // gán lại dòng bắt đầu
                            foreach (var content in KpiItem_ExportDTO.Contents)
                            {
                                KpiItem_ExportCriteriaContent criteriaContent = content
                                    .CriteriaContents.Where(x => x.CriteriaId == KpiCriteriaItems[i].Id).FirstOrDefault(); // Lấy ra giá trị của criteria tương ứng
                                ValueData.Add(new object[]
                                {
                                criteriaContent.Value,
                                });
                                endRow++;
                            }
                            startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                            if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                            currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                            if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);


                            ws.Cells[$"{startColumnString}{startRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(ValueData); // fill dữ liệu

                            endColumn += 1; // Chiếm 1 cột cho mỗi criteria
                            startColumn = endColumn; // gán lại cột bắt đầu cho dữ liệu tiếp sau
                        }
                        #endregion

                        ws.Cells[$"A{startRow + 1}:A{endRow}"].Merge = true; // merge cột mã nhân viên và tên nhân viên
                        ws.Cells[$"B{startRow + 1}:B{endRow}"].Merge = true; // dòng đầu tiên là dòng tên nhân viên nên startRow + 1

                        startRow = endRow + 1; // gán dòng bắt đầu cho org tiếp theo
                    }
                }

                ws.Cells[$"F5:{endColumnString}{endRow}"].Style.Numberformat.Format = "#,##0"; // format number column value
                ws.Column(2).Width = 20;
                ws.Column(5).Width = 40;
                // All Borders                  
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "KpiItems.xlsx");
        }

        [Route(KpiItemRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var appUser = await AppUserService.Get(CurrentContext.UserId);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.OrganizationId = new IdFilter { Equal = appUser.OrganizationId };
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            {
                if (AppUserFilter.Id.In == null) AppUserFilter.Id.In = new List<long>();
                AppUserFilter.Id.In.AddRange(await FilterAppUser(AppUserService, OrganizationService, CurrentContext));
            }
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);

            List<Item> Items = await ItemService.List(new ItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                Selects = ItemSelect.Code | ItemSelect.Name | ItemSelect.Product,
                OrderBy = ItemOrder.Id,
                OrderType = OrderType.ASC
            });

            List<Item> NewItems = Items.Where(x => x.Product.IsNew).ToList();

            List<KpiItem_ExportDTO> KpiItem_ExportDTOs = new List<KpiItem_ExportDTO>();
            foreach (var AppUser in AppUsers)
            {
                KpiItem_ExportDTO KpiItem_ExportDTO = new KpiItem_ExportDTO();
                KpiItem_ExportDTO.Username = AppUser.Username;
                KpiItem_ExportDTO.DisplayName = AppUser.DisplayName;
                KpiItem_ExportDTOs.Add(KpiItem_ExportDTO);
            }

            List<KpiCriteriaItem> KpiCriteriaItems = await KpiCriteriaItemService.List(new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaItemSelect.ALL
            });
            KpiCriteriaItems = KpiCriteriaItems.OrderBy(x => x.Id).ToList();

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("KPI sản phẩm");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["A1"].Value = "THIẾT LẬP KPI SẢN PHẨM TRỌNG TÂM";

                ws.Cells["B2"].Value = "Kỳ";
                ws.Cells["C2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["C2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E7E6E6"));
                var PeriodDropdown = ws.DataValidations.AddListValidation("C2");
                for (int i = 0; i < KpiPeriodEnum.KpiPeriodEnumList.Count; i++)
                {
                    PeriodDropdown.Formula.Values.Add(KpiPeriodEnum.KpiPeriodEnumList[i].Name);
                }

                ws.Cells["D2"].Value = "Năm";
                ws.Cells["E2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["E2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E7E6E6"));
                var YearDropdown = ws.DataValidations.AddListValidation("E2");
                for (int i = 0; i < KpiYearEnum.KpiYearEnumList.Count; i++)
                {
                    YearDropdown.Formula.Values.Add(KpiYearEnum.KpiYearEnumList[i].Name);
                }

                ws.Cells["A2:F2"].Style.Font.Bold = true;
                ws.Cells["A2:F2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "Mã nhân viên",
                    "Tên nhân viên",
                    "Loại KPI sản phẩm",
                    "Mã sản phẩm",
                };

                for (int i = 0; i < KpiCriteriaItems.Count; i++)
                {
                    headers.Add(KpiCriteriaItems[i].Name);
                }

                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64); ;

                // format lại tiêu đề theo số cột của dữ liệu
                ws.Cells[$"A1:{endColumnString}1"].Merge = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Size = 14;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Bold = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                string headerRange = $"A4:" + $"{endColumnString}4";
                List<string[]> Header = new List<string[]> { headers.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#9BC2E6"));

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells[headerRange].AutoFitColumns();
                #endregion

                #region Du lieu bao cao
                int startRow = 5;
                if (KpiItem_ExportDTOs != null && KpiItem_ExportDTOs.Count > 0)
                {
                    foreach (var KpiItem_ExportDTO in KpiItem_ExportDTOs)
                    {
                        int currentRow = startRow;
                        ws.Cells[$"A{currentRow}"].Value = $"{KpiItem_ExportDTO.Username} - {KpiItem_ExportDTO.DisplayName}";
                        ws.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Merge = true;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

                        List<string[]> Contents = new List<string[]>();
                        currentRow++;
                        List<string> lineData = new List<string>
                        {
                           KpiItem_ExportDTO.Username,
                           KpiItem_ExportDTO.DisplayName,
                        };
                        Contents.Add(lineData.ToArray());

                        ws.Cells[$"A{startRow + 1}:{endColumnString}{currentRow}"].LoadFromArrays(Contents);
                        ws.Cells[$"A{startRow + 1}:A{currentRow}"].Merge = true;
                        ws.Cells[$"B{startRow + 1}:B{currentRow}"].Merge = true;
                        startRow = currentRow + 1; // Gán lại start row cho employee tiếp theo
                    }
                }

                ws.Cells[$"A{startRow}"].Value = "END";
                ws.Cells[$"B{startRow}"].Value = "Vui lòng insert các mã sản phẩm vào theo từng nhân viên";
                ws.Cells[$"A{startRow}:B{startRow}"].Style.Font.Bold = true;

                // add Drop down Kpi item type
                var KpiItemTypeDropdown = ws.DataValidations.AddListValidation($"C6:C{startRow}");
                for (int i = 0; i < KpiItemTypeEnum.KpiItemTypeEnumList.Count; i++)
                {
                    KpiItemTypeDropdown.Formula.Values.Add(KpiItemTypeEnum.KpiItemTypeEnumList[i].Name);
                }

                ws.Cells[$"E6:{endColumnString}{startRow}"].Style.Numberformat.Format = "#,##0"; // format number column value

                ws.Column(2).Width = 20;
                // All borders
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                #region Sheet sản phẩm
                ExcelWorksheet ws2 = excel.Workbook.Worksheets.Add("Sản phẩm");
                ws2.Cells["A1"].Value = "Mã sản phẩm";
                ws2.Cells["B1"].Value = "Tên sản phẩm";
                ws2.Cells["A1:B1"].Style.Font.Bold = true;
                ws2.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws2.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFE699"));
                if (Items != null && Items.Count > 0)
                {
                    List<string[]> itemData = new List<string[]>();
                    foreach (var item in Items)
                    {
                        itemData.Add(new string[] { item.Code, item.Name });
                    }
                    ws2.Cells[$"A2:A{itemData.Count + 1}"].LoadFromArrays(itemData);
                }

                ws2.Column(1).Width = 20;
                ws2.Column(2).Width = 50;
                #endregion

                #region Sheet sản phẩm mới
                ExcelWorksheet ws3 = excel.Workbook.Worksheets.Add("Sản phẩm mới");
                ws3.Cells["A1"].Value = "Mã sản phẩm";
                ws3.Cells["B1"].Value = "Tên sản phẩm";
                ws3.Cells["A1:B1"].Style.Font.Bold = true;
                ws3.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws3.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFE699"));
                if (NewItems != null && NewItems.Count > 0)
                {
                    List<string[]> newItemData = new List<string[]>();
                    foreach (var item in NewItems)
                    {
                        newItemData.Add(new string[] { item.Code, item.Name });
                    }
                    ws3.Cells[$"A2:A{newItemData.Count + 1}"].LoadFromArrays(newItemData);
                }

                ws3.Column(1).Width = 20;
                ws3.Column(2).Width = 50;
                #endregion

                #region Sheet Quy tắc import
                ExcelWorksheet ws4 = excel.Workbook.Worksheets.Add("Quy tắc import");
                ws4.Cells["A1"].Style.Font.Bold = true;
                List<string[]> ws4Text = new List<string[]>
                {
                    new string[] {"Quy tắc Import:" },
                    new string[] {"1. Kỳ KPI được nhập dựa theo giá trị nhập ở ô C2 và E2, dữ liệu được lựa chọn từ combo" },
                    new string[] {"2. Khi tải biểu mẫu thì hệ thống sẽ xuất ra file excel có đầy đủ thông tin như sau:" },
                    new string[] {" - Nhân viên: hiển thị toàn bộ nhân viên theo phân quyền của người dùng (orgunit, appuser, userID)" },
                    new string[] {" - Với mỗi group nhân viên: xuất toàn bộ các mã sản phẩm đã có chỉ tiêu của nhân viên + xuất 1 dòng trắng không có mã sản phẩm" },
                    new string[] {" - Giá trị: xuất giá trị tương ứng với các mã sản phẩm đã có chỉ tiêu" },
                    new string[] {" - Sheet sản phẩm: xuất toàn bộ sản phẩm đã có status = 1" },
                    new string[] {" - Sheet sản phẩm mới: Xuất toàn bô danh sách các sản phẩm mới" },
                    new string[] {"3. Khi import thì check:" },
                    new string[] {" - Nếu đã tồn tại nhân viên + năm + mã sản phẩm: update vào bản ghi đã tồn tại" },
                    new string[] {" - Nếu đã tồn tại nhân viên + năm + mã sản phẩm không có: insert bản ghi sản phẩm vào KPI" },
                    new string[] {" - Nếu chưa có nhân viên + năm: insert KPI cho nhân viên đó" },
                    new string[] {" - Khi import thì check:" },
                    new string[] {"  + Mã nhân viên: check có tồn tại không, nếu không thì hiển thị thông báo 'Mã nhân viên không tồn tại'" },
                    new string[] {"  + Mã nhân viên - Tên nhân viên:  mã và tên phải trùng nhau, nếu không trùng thì thông báo 'Mã và tên nhân viên không khớp'" },
                    new string[] {"  + Giá trị các chỉ tiêu: nếu nhập khác số thì hiển thị thông báo 'Giá trị chỉ tiêu{Mã chỉ tiêu} ở dòng {dòng} sai định dạng'" },
                    new string[] {" - Kết thúc ở dòng END" }
                };
                ws4.Cells["A1:A17"].LoadFromArrays(ws4Text);
                ws4.Cells["A1:A17"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws4.Cells["A1:A17"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF2CC"));
                ws4.Column(1).Width = 105;

                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "Template_Kpi_Item.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            KpiItemFilter KpiItemFilter = new KpiItemFilter();
            KpiItemFilter = await KpiItemService.ToFilter(KpiItemFilter);
            if (Id == 0)
            {

            }
            else
            {
                KpiItemFilter.Id = new IdFilter { Equal = Id };
                int count = await KpiItemService.Count(KpiItemFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }
        private Tuple<GenericEnum, GenericEnum, GenericEnum> ConvertDateTime(DateTime date)
        {
            GenericEnum monthName = Enums.KpiPeriodEnum.PERIOD_MONTH01;
            GenericEnum quarterName = Enums.KpiPeriodEnum.PERIOD_MONTH01;
            GenericEnum yearName = Enums.KpiYearEnum.KpiYearEnumList.Where(x => x.Id == StaticParams.DateTimeNow.Year).FirstOrDefault();
            switch (date.Month)
            {
                case 1:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH01;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER01;
                    break;
                case 2:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH02;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER01;
                    break;
                case 3:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH03;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER01;
                    break;
                case 4:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH04;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER02;
                    break;
                case 5:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH05;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER02;
                    break;
                case 6:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH06;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER02;
                    break;
                case 7:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH07;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER03;
                    break;
                case 8:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH08;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER03;
                    break;
                case 9:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH09;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER03;
                    break;
                case 10:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH10;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER04;
                    break;
                case 11:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH11;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER04;
                    break;
                case 12:
                    monthName = Enums.KpiPeriodEnum.PERIOD_MONTH12;
                    quarterName = Enums.KpiPeriodEnum.PERIOD_QUATER04;
                    break;
            }
            return Tuple.Create(monthName, quarterName, yearName);
        }
        private KpiItem ConvertDTOToEntity(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            KpiItem_KpiItemDTO.TrimString();
            KpiItem KpiItem = new KpiItem();
            KpiItem.Id = KpiItem_KpiItemDTO.Id;
            KpiItem.OrganizationId = KpiItem_KpiItemDTO.OrganizationId;
            KpiItem.KpiYearId = KpiItem_KpiItemDTO.KpiYearId;
            KpiItem.KpiPeriodId = KpiItem_KpiItemDTO.KpiPeriodId;
            KpiItem.KpiItemTypeId = KpiItem_KpiItemDTO.KpiItemTypeId;
            KpiItem.StatusId = KpiItem_KpiItemDTO.StatusId;
            KpiItem.EmployeeId = KpiItem_KpiItemDTO.EmployeeId;
            KpiItem.CreatorId = KpiItem_KpiItemDTO.CreatorId;
            KpiItem.Creator = KpiItem_KpiItemDTO.Creator == null ? null : new AppUser
            {
                Id = KpiItem_KpiItemDTO.Creator.Id,
                Username = KpiItem_KpiItemDTO.Creator.Username,
                DisplayName = KpiItem_KpiItemDTO.Creator.DisplayName,
                Address = KpiItem_KpiItemDTO.Creator.Address,
                Email = KpiItem_KpiItemDTO.Creator.Email,
                Phone = KpiItem_KpiItemDTO.Creator.Phone,
            };
            KpiItem.Employee = KpiItem_KpiItemDTO.Employee == null ? null : new AppUser
            {
                Id = KpiItem_KpiItemDTO.Employee.Id,
                Username = KpiItem_KpiItemDTO.Employee.Username,
                DisplayName = KpiItem_KpiItemDTO.Employee.DisplayName,
                Address = KpiItem_KpiItemDTO.Employee.Address,
                Email = KpiItem_KpiItemDTO.Employee.Email,
                Phone = KpiItem_KpiItemDTO.Employee.Phone,
            };
            KpiItem.KpiYear = KpiItem_KpiItemDTO.KpiYear == null ? null : new KpiYear
            {
                Id = KpiItem_KpiItemDTO.KpiYear.Id,
                Code = KpiItem_KpiItemDTO.KpiYear.Code,
                Name = KpiItem_KpiItemDTO.KpiYear.Name,
            };
            KpiItem.KpiPeriod = KpiItem_KpiItemDTO.KpiPeriod == null ? null : new KpiPeriod
            {
                Id = KpiItem_KpiItemDTO.KpiPeriod.Id,
                Code = KpiItem_KpiItemDTO.KpiPeriod.Code,
                Name = KpiItem_KpiItemDTO.KpiPeriod.Name,
            };
            KpiItem.KpiItemType = KpiItem_KpiItemDTO.KpiItemType == null ? null : new KpiItemType
            {
                Id = KpiItem_KpiItemDTO.KpiItemType.Id,
                Code = KpiItem_KpiItemDTO.KpiItemType.Code,
                Name = KpiItem_KpiItemDTO.KpiItemType.Name,
            };
            KpiItem.Organization = KpiItem_KpiItemDTO.Organization == null ? null : new Organization
            {
                Id = KpiItem_KpiItemDTO.Organization.Id,
                Code = KpiItem_KpiItemDTO.Organization.Code,
                Name = KpiItem_KpiItemDTO.Organization.Name,
                ParentId = KpiItem_KpiItemDTO.Organization.ParentId,
            };
            KpiItem.Status = KpiItem_KpiItemDTO.Status == null ? null : new Status
            {
                Id = KpiItem_KpiItemDTO.Status.Id,
                Code = KpiItem_KpiItemDTO.Status.Code,
                Name = KpiItem_KpiItemDTO.Status.Name,
            };
            KpiItem.Employees = KpiItem_KpiItemDTO.Employees?.Select(x => new AppUser
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Username = x.Username,
                Phone = x.Phone,
                Email = x.Email,
            }).ToList();
            KpiItem.KpiItemContents = KpiItem_KpiItemDTO.KpiItemContents?
                .Select(x => new KpiItemContent
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    Item = x.Item == null ? null : new Item
                    {
                        Id = x.Item.Id,
                        ProductId = x.Item.ProductId,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ScanCode = x.Item.ScanCode,
                        StatusId = x.Item.StatusId,
                    },
                    KpiItemContentKpiCriteriaItemMappings = x.KpiItemContentKpiCriteriaItemMappings.Select(p => new KpiItemContentKpiCriteriaItemMapping
                    {
                        KpiCriteriaItemId = p.Key,
                        Value = p.Value,
                    }).ToList(),
                }).ToList();
            KpiItem.BaseLanguage = CurrentContext.Language;
            return KpiItem;
        }

        private KpiItemFilter ConvertFilterDTOToFilterEntity(KpiItem_KpiItemFilterDTO KpiItem_KpiItemFilterDTO)
        {
            KpiItemFilter KpiItemFilter = new KpiItemFilter();
            KpiItemFilter.Selects = KpiItemSelect.ALL;
            KpiItemFilter.Skip = KpiItem_KpiItemFilterDTO.Skip;
            KpiItemFilter.Take = KpiItem_KpiItemFilterDTO.Take;
            KpiItemFilter.OrderBy = KpiItem_KpiItemFilterDTO.OrderBy;
            KpiItemFilter.OrderType = KpiItem_KpiItemFilterDTO.OrderType;

            KpiItemFilter.Id = KpiItem_KpiItemFilterDTO.Id;
            KpiItemFilter.OrganizationId = KpiItem_KpiItemFilterDTO.OrganizationId;
            KpiItemFilter.KpiYearId = KpiItem_KpiItemFilterDTO.KpiYearId;
            KpiItemFilter.KpiPeriodId = KpiItem_KpiItemFilterDTO.KpiPeriodId;
            KpiItemFilter.KpiItemTypeId = KpiItem_KpiItemFilterDTO.KpiItemTypeId;
            KpiItemFilter.StatusId = KpiItem_KpiItemFilterDTO.StatusId;
            KpiItemFilter.AppUserId = KpiItem_KpiItemFilterDTO.AppUserId;
            KpiItemFilter.CreatorId = KpiItem_KpiItemFilterDTO.CreatorId;
            KpiItemFilter.CreatedAt = KpiItem_KpiItemFilterDTO.CreatedAt;
            KpiItemFilter.UpdatedAt = KpiItem_KpiItemFilterDTO.UpdatedAt;
            return KpiItemFilter;
        }
    }
}

