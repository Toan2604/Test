using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MBrand;
using DMS.Services.MCategory;
using DMS.Services.MKpiPeriod;
using DMS.Services.MKpiProductGrouping;
using DMS.Services.MKpiProductGroupingContent;
using DMS.Services.MKpiProductGroupingCriteria;
using DMS.Services.MKpiProductGroupingType;
using DMS.Services.MKpiYear;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public partial class KpiProductGroupingController : RpcController
    {
        private IAppUserService AppUserService;
        private IKpiPeriodService KpiPeriodService;
        private IKpiProductGroupingTypeService KpiProductGroupingTypeService;
        private IKpiProductGroupingCriteriaService KpiProductGroupingCriteriaService;
        private IStatusService StatusService;
        private IKpiProductGroupingService KpiProductGroupingService;
        private IKpiProductGroupingContentService KpiProductGroupingContentService;
        private IItemService ItemService;
        private IKpiYearService KpiYearService;
        private IOrganizationService OrganizationService;
        private ICategoryService CategoryService;
        private IProductTypeService ProductTypeService;
        private IProductGroupingService ProductGroupingService;
        private IBrandService BrandService;
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        public KpiProductGroupingController(
            IAppUserService AppUserService,
            IKpiPeriodService KpiPeriodService,
            IKpiProductGroupingTypeService KpiProductGroupingTypeService,
            IKpiProductGroupingCriteriaService KpiProductGroupingCriteriaService,
            IStatusService StatusService,
            IKpiProductGroupingService KpiProductGroupingService,
            IKpiProductGroupingContentService KpiProductGroupingContentService,
            IItemService ItemService,
            IKpiYearService KpiYearService,
            IOrganizationService OrganizationService,
            ICategoryService CategoryService,
            IProductTypeService ProductTypeService,
            IProductGroupingService ProductGroupingService,
            IBrandService BrandService,
            ICurrentContext CurrentContext,
            DataContext DataContext
        )
        {
            this.AppUserService = AppUserService;
            this.KpiPeriodService = KpiPeriodService;
            this.KpiProductGroupingTypeService = KpiProductGroupingTypeService;
            this.KpiProductGroupingCriteriaService = KpiProductGroupingCriteriaService;
            this.StatusService = StatusService;
            this.KpiProductGroupingService = KpiProductGroupingService;
            this.KpiProductGroupingContentService = KpiProductGroupingContentService;
            this.ItemService = ItemService;
            this.KpiYearService = KpiYearService;
            this.OrganizationService = OrganizationService;
            this.CategoryService = CategoryService;
            this.ProductTypeService = ProductTypeService;
            this.ProductGroupingService = ProductGroupingService;
            this.BrandService = BrandService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
        }

        [Route(KpiProductGroupingRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] KpiProductGrouping_KpiProductGroupingFilterDTO KpiProductGrouping_KpiProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiProductGroupingFilter KpiProductGroupingFilter = ConvertFilterDTOToFilterEntity(KpiProductGrouping_KpiProductGroupingFilterDTO);
            KpiProductGroupingFilter = await KpiProductGroupingService.ToFilter(KpiProductGroupingFilter);
            int count = await KpiProductGroupingService.Count(KpiProductGroupingFilter);
            return count;
        }

        [Route(KpiProductGroupingRoute.List), HttpPost]
        public async Task<List<KpiProductGrouping_KpiProductGroupingDTO>> List([FromBody] KpiProductGrouping_KpiProductGroupingFilterDTO KpiProductGrouping_KpiProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiProductGroupingFilter KpiProductGroupingFilter = ConvertFilterDTOToFilterEntity(KpiProductGrouping_KpiProductGroupingFilterDTO);
            KpiProductGroupingFilter = await KpiProductGroupingService.ToFilter(KpiProductGroupingFilter);
            List<KpiProductGrouping> KpiProductGroupings = await KpiProductGroupingService.List(KpiProductGroupingFilter);
            for (int i = 0; i < KpiProductGroupings.Count; i++)
            {
                KpiProductGroupings[i] = Utils.Clone(KpiProductGroupings[i]);
                KpiProductGroupings[i].Organization = KpiProductGroupings[i].Employee.Organization;
                KpiProductGroupings[i].OrganizationId = KpiProductGroupings[i].Employee.OrganizationId;
            } // BA y??u c???u tr??? v??? ????n v??? t??? ch???c c???a Employee, kh??ng ph???i c???a Kpi
            List<KpiProductGrouping_KpiProductGroupingDTO> KpiProductGrouping_KpiProductGroupingDTOs = KpiProductGroupings
                .Select(c => new KpiProductGrouping_KpiProductGroupingDTO(c)).ToList();
            return KpiProductGrouping_KpiProductGroupingDTOs;
        }

        [Route(KpiProductGroupingRoute.Get), HttpPost]
        public async Task<ActionResult<KpiProductGrouping_KpiProductGroupingDTO>> Get([FromBody] KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiProductGrouping_KpiProductGroupingDTO.Id))
                return Forbid();

            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL,
            });

            KpiProductGrouping KpiProductGrouping = await KpiProductGroupingService.Get(KpiProductGrouping_KpiProductGroupingDTO.Id);
            KpiProductGrouping_KpiProductGroupingDTO = new KpiProductGrouping_KpiProductGroupingDTO(KpiProductGrouping);
            KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingCriterias = KpiProductGroupingCriterias.Select(x => new KpiProductGrouping_KpiProductGroupingCriteriaDTO(x)).ToList(); // tr??? v??? c??c ch??? ti??u

            return KpiProductGrouping_KpiProductGroupingDTO;
        }

        [Route(KpiProductGroupingRoute.GetDraft), HttpPost]
        public async Task<ActionResult<KpiProductGrouping_KpiProductGroupingDTO>> GetDraft()
        {
            long KpiYearId = StaticParams.DateTimeNow.Year;
            long KpiPeriodId = StaticParams.DateTimeNow.Month + 100;
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL,
            });
            var KpiProductGrouping_KpiProductGroupingDTO = new KpiProductGrouping_KpiProductGroupingDTO();
            (KpiProductGrouping_KpiProductGroupingDTO.CurrentMonth, KpiProductGrouping_KpiProductGroupingDTO.CurrentQuarter, KpiProductGrouping_KpiProductGroupingDTO.CurrentYear) = ConvertDateTime(StaticParams.DateTimeNow);
            KpiProductGrouping_KpiProductGroupingDTO.KpiYearId = KpiYearId;
            KpiProductGrouping_KpiProductGroupingDTO.KpiYear = KpiProductGrouping_KpiProductGroupingDTO.KpiYear = Enums.KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId)
                .Select(x => new KpiProductGrouping_KpiYearDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();
            KpiProductGrouping_KpiProductGroupingDTO.KpiPeriodId = KpiPeriodId; // tr??? v??? n??m
            KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod = KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod = Enums.KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Id == KpiPeriodId)
                .Select(x => new KpiProductGrouping_KpiPeriodDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault(); //  tr??? v??? k???
            KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingCriterias = KpiProductGroupingCriterias.Select(x => new KpiProductGrouping_KpiProductGroupingCriteriaDTO(x)).ToList(); // tr??? v??? c??c ch??? ti??u
            KpiProductGrouping_KpiProductGroupingDTO.Status = new KpiProductGrouping_StatusDTO
            {
                Code = Enums.StatusEnum.ACTIVE.Code,
                Id = Enums.StatusEnum.ACTIVE.Id,
                Name = Enums.StatusEnum.ACTIVE.Name
            }; // tr??? v??? tr???ng th??i
            KpiProductGrouping_KpiProductGroupingDTO.StatusId = Enums.StatusEnum.ACTIVE.Id;
            return KpiProductGrouping_KpiProductGroupingDTO;
        }

        [Route(KpiProductGroupingRoute.Create), HttpPost]
        public async Task<ActionResult<KpiProductGrouping_KpiProductGroupingDTO>> Create([FromBody] KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiProductGrouping_KpiProductGroupingDTO.Id))
                return Forbid();

            KpiProductGrouping KpiProductGrouping = ConvertDTOToEntity(KpiProductGrouping_KpiProductGroupingDTO);
            KpiProductGrouping = await KpiProductGroupingService.Create(KpiProductGrouping);
            KpiProductGrouping_KpiProductGroupingDTO NewKpiProductGroupingDTO = new KpiProductGrouping_KpiProductGroupingDTO(KpiProductGrouping);
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL,
            });
            if (KpiProductGrouping.IsValidated)
                return NewKpiProductGroupingDTO;
            NewKpiProductGroupingDTO.Employees = KpiProductGrouping_KpiProductGroupingDTO.Employees; // map lai Employees neu xay ra loi
            NewKpiProductGroupingDTO.KpiProductGroupingCriterias = KpiProductGroupingCriterias.Select(x => new KpiProductGrouping_KpiProductGroupingCriteriaDTO(x)).ToList(); // tr??? v??? c??c ch??? ti??u
            return BadRequest(NewKpiProductGroupingDTO);
        }

        [Route(KpiProductGroupingRoute.Update), HttpPost]
        public async Task<ActionResult<KpiProductGrouping_KpiProductGroupingDTO>> Update([FromBody] KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiProductGrouping_KpiProductGroupingDTO.Id))
                return Forbid();

            KpiProductGrouping KpiProductGrouping = ConvertDTOToEntity(KpiProductGrouping_KpiProductGroupingDTO);
            KpiProductGrouping = await KpiProductGroupingService.Update(KpiProductGrouping);
            KpiProductGrouping_KpiProductGroupingDTO = new KpiProductGrouping_KpiProductGroupingDTO(KpiProductGrouping);
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL,
            });

            KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingCriterias = KpiProductGroupingCriterias.Select(x => new KpiProductGrouping_KpiProductGroupingCriteriaDTO(x)).ToList(); // tr??? v??? c??c ch??? ti??u

            if (KpiProductGrouping.IsValidated)
                return KpiProductGrouping_KpiProductGroupingDTO;
            else
                return BadRequest(KpiProductGrouping_KpiProductGroupingDTO);
        }

        [Route(KpiProductGroupingRoute.Delete), HttpPost]
        public async Task<ActionResult<KpiProductGrouping_KpiProductGroupingDTO>> Delete([FromBody] KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(KpiProductGrouping_KpiProductGroupingDTO.Id))
                return Forbid();

            KpiProductGrouping KpiProductGrouping = ConvertDTOToEntity(KpiProductGrouping_KpiProductGroupingDTO);
            KpiProductGrouping = await KpiProductGroupingService.Delete(KpiProductGrouping);
            KpiProductGrouping_KpiProductGroupingDTO = new KpiProductGrouping_KpiProductGroupingDTO(KpiProductGrouping);
            if (KpiProductGrouping.IsValidated)
                return KpiProductGrouping_KpiProductGroupingDTO;
            else
                return BadRequest(KpiProductGrouping_KpiProductGroupingDTO);
        }

        [Route(KpiProductGroupingRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter();
            KpiProductGroupingFilter = await KpiProductGroupingService.ToFilter(KpiProductGroupingFilter);
            KpiProductGroupingFilter.Id = new IdFilter { In = Ids };
            KpiProductGroupingFilter.Selects = KpiProductGroupingSelect.Id;
            KpiProductGroupingFilter.Skip = 0;
            KpiProductGroupingFilter.Take = int.MaxValue;

            List<KpiProductGrouping> KpiProductGroupings = await KpiProductGroupingService.List(KpiProductGroupingFilter);
            KpiProductGroupings = await KpiProductGroupingService.BulkDelete(KpiProductGroupings);
            if (KpiProductGroupings.Any(x => !x.IsValidated))
                return BadRequest(KpiProductGroupings.Where(x => !x.IsValidated));
            return true;
        }

        [Route(KpiProductGroupingRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL,
            });
            KpiProductGroupingCriterias = KpiProductGroupingCriterias.OrderBy(x => x.Id).ToList();

            #region validate file
            StringBuilder errorContent = new StringBuilder();
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
            {
                errorContent.AppendLine("?????nh d???ng file kh??ng h???p l???");
                return BadRequest(errorContent.ToString());
            }

            GenericEnum KpiYear;
            GenericEnum KpiPeriod;
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["KPI nh??m s???n ph???m"];
                if (worksheet == null)
                {
                    errorContent.AppendLine("File kh??ng ????ng bi???u m???u import");
                    return BadRequest(errorContent.ToString());
                }

                string KpiPeriodValue = worksheet.Cells[2, 4].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(KpiPeriodValue))
                    KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Name == KpiPeriodValue).FirstOrDefault();
                else
                {
                    errorContent.AppendLine("Ch??a ch???n k??? Kpi ho???c k??? Kpi kh??ng h???p l???");
                    return BadRequest(errorContent.ToString());
                }

                string KpiYearValue = worksheet.Cells[2, 6].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(KpiYearValue))
                    KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Name == KpiYearValue.Trim()).FirstOrDefault();
                else
                {
                    errorContent.AppendLine("Ch??a ch???n n??m Kpi ho???c n??m Kpi kh??ng h???p l???");
                    return BadRequest(errorContent.ToString());
                }

                int StartCriteriaHeaderColumn = 7;
                bool HeaderValidator = true;
                for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                {
                    string CriteriaHeader = worksheet.Cells[4, StartCriteriaHeaderColumn + i].Value?.ToString();
                    if (CriteriaHeader != KpiProductGroupingCriterias[i].Name)
                    {
                        errorContent.AppendLine($"Sai bi???u m???u: C???t {StartCriteriaHeaderColumn + i} ph???i nh???p \"{KpiProductGroupingCriterias[i].Name}\"");
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
                errorContent.AppendLine("Kh??ng th??? nh???p Kpi cho c??c k??? trong qu?? kh???");
                return BadRequest(errorContent.ToString());
            }
            #endregion

            #region filter d??? li???u
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

            List<KpiProductGrouping> KpiProductGroupings = await KpiProductGroupingService.List(new KpiProductGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                EmployeeId = new IdFilter { In = AppUserIds },
                KpiYearId = new IdFilter { Equal = KpiYear.Id },
                KpiPeriodId = new IdFilter { Equal = KpiPeriod.Id },
                Selects = KpiProductGroupingSelect.ALL
            });
            List<long> KpiProductGroupingIds = KpiProductGroupings.Select(x => x.Id).ToList();
            List<KpiProductGroupingContent> KpiProductGroupingContents = await KpiProductGroupingContentService.List(new KpiProductGroupingContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingContentSelect.ALL,
                KpiProductGroupingId = new IdFilter { In = KpiProductGroupingIds }
            });
            foreach (KpiProductGrouping KpiProductGrouping in KpiProductGroupings)
            {
                KpiProductGrouping.KpiProductGroupingContents = KpiProductGroupingContents.Where(x => x.KpiProductGroupingId == KpiProductGrouping.Id).ToList();
            }
            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(new ProductGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code | ProductGroupingSelect.Name,
            }); // lay ra tat ca productGrouping de validate

            var item_query = from i in DataContext.Item
                             join pr in DataContext.Product on i.ProductId equals pr.Id
                             join prgm in DataContext.ProductProductGroupingMapping on pr.Id equals prgm.ProductId
                             join prg in DataContext.ProductGrouping on prgm.ProductGroupingId equals prg.Id
                             select new KpiProductGrouping_ItemImportDTO
                             {
                                 Id = i.Id,
                                 Code = i.Code,
                                 Name = i.Name,
                                 IsNew = pr.IsNew,
                                 ProductGroupingId = prg.Id,
                             };
            List<KpiProductGrouping_ItemImportDTO> Items = await item_query.ToListWithNoLockAsync();
            #endregion

            #region l???y ra d??? li???u
            var AppUser = await AppUserService.Get(CurrentContext.UserId);
            List<KpiProductGrouping_ImportDTO> KpiProductGrouping_ImportDTOs = new List<KpiProductGrouping_ImportDTO>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["KPI nh??m s???n ph???m"];

                int StartColumn = 1; // cot bat dau tinh du lieu
                int StartRow = 5; // dong bat dau tinh du lieu
                int UsernameColumn = 0 + StartColumn;
                int DisplaynameColumn = 1 + StartColumn;
                int KpiProductGroupingTypeColumn = 2 + StartColumn;
                int ProductGroupingCodeColumn = 3 + StartColumn;
                int SelectAllItemColumn = 4 + StartColumn;
                int ItemCodeColumn = 5 + StartColumn;

                int CriteriaStartColumn = 6 + StartColumn;

                //int RevenueColumn = 5 + StartColumn;
                //int StoreColumn = 6 + StartColumn;
                //int DirectRevenueColumn = 7 + StartColumn;
                //int DirectStoreCounterColumn = 8 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string UsernameValue = worksheet.Cells[i, UsernameColumn].Value?.ToString();
                    string DisplaynameValue = worksheet.Cells[i, DisplaynameColumn].Value?.ToString();
                    string KpiProductGroupingTypeValue = worksheet.Cells[i, KpiProductGroupingTypeColumn].Value?.ToString();
                    string SelectAllItemValue = worksheet.Cells[i, SelectAllItemColumn].Value?.ToString();
                    string ProductGroupingCodeValue = worksheet.Cells[i, ProductGroupingCodeColumn].Value?.ToString();
                    string ItemCodeValue = worksheet.Cells[i, ItemCodeColumn].Value?.ToString();

                    KpiProductGrouping_ImportDTO KpiProductGrouping_ImportDTO = new KpiProductGrouping_ImportDTO();

                    #region validate nhan vien
                    if (UsernameValue != null && UsernameValue.ToLower() == "END".ToLower())
                        break;
                    else if (!string.IsNullOrWhiteSpace(UsernameValue) && string.IsNullOrWhiteSpace(KpiProductGroupingTypeValue)
                        && string.IsNullOrWhiteSpace(ProductGroupingCodeValue) && string.IsNullOrWhiteSpace(ItemCodeValue))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(UsernameValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"L???i d??ng th??? {i}: Ch??a nh???p m?? nh??n vi??n");
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(UsernameValue) && i == worksheet.Dimension.End.Row)
                        break;


                    AppUser Employee = Employees.Where(x => x.Username.ToLower() == UsernameValue.ToLower()).FirstOrDefault();
                    if (Employee == null)
                    {
                        errorContent.AppendLine($"L???i d??ng th??? {i}: Nh??n vi??n kh??ng t???n t???i");
                        continue;
                    }
                    else
                    {
                        KpiProductGrouping_ImportDTO.EmployeeId = Employee.Id;
                        KpiProductGrouping_ImportDTO.OrganizationId = Employee.OrganizationId;
                    }
                    #endregion

                    #region validate loai kpi
                    GenericEnum KpiProductGroupingType;
                    if (!string.IsNullOrWhiteSpace(KpiProductGroupingTypeValue))
                    {
                        KpiProductGroupingType = KpiProductGroupingTypeEnum.KpiProductGroupingTypeEnumList.Where(x => x.Name == KpiProductGroupingTypeValue.Trim()).FirstOrDefault();
                        if (KpiProductGroupingType == null)
                        {
                            errorContent.AppendLine($"L???i d??ng th??? {i + 1}: Lo???i KPI s???n ph???m kh??ng t???n t???i");
                            continue;
                        }
                        else
                        {
                            KpiProductGrouping_ImportDTO.KpiProductGroupingTypeId = KpiProductGroupingType.Id;
                        }
                    }
                    else
                    {
                        errorContent.AppendLine($"L???i d??ng th??? {i}: Ch??a ch???n lo???i KPI s???n ph???m");
                        continue;
                    }
                    #endregion

                    #region validate ch???n t???t c??? s???n ph???m
                    bool SelectAllItem;
                    if (!string.IsNullOrWhiteSpace(SelectAllItemValue))
                    {
                        if (SelectAllItemValue.ToLower() != "C??".ToLower() && SelectAllItemValue.ToLower() != "KH??NG".ToLower())
                        {
                            errorContent.AppendLine($@"L???i d??ng th??? {i + 1}: Ch???n t???t c??? s???n ph???m trong nh??m ch??? ???????c nh???p 'C??' ho???c 'Kh??ng'");
                            continue;
                        }
                        else
                        {
                            SelectAllItem = SelectAllItemValue.ToLower() == "C??".ToLower();
                        }
                    }
                    else
                    {
                        SelectAllItem = false;
                    }
                    #endregion

                    #region validate nhom san pham va item
                    ProductGrouping ProductGrouping;
                    if (!string.IsNullOrWhiteSpace(ProductGroupingCodeValue))
                    {
                        ProductGrouping = ProductGroupings.Where(x => x.Code == ProductGroupingCodeValue.Trim()).FirstOrDefault();
                        if (ProductGrouping == null)
                        {
                            errorContent.AppendLine($"L???i d??ng th??? {i}: Nh??m s???n ph???m kh??ng t???n t???i");
                            continue;
                        }
                        KpiProductGrouping_ImportDTO.ProductGroupingId = ProductGrouping.Id;
                    }
                    else
                    {
                        errorContent.AppendLine($"L???i d??ng th??? {i}: Ch??a ch???n nh??m s???n ph???m");
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(ItemCodeValue) && !SelectAllItem)
                    {
                        List<string> ItemCodes = ItemCodeValue.Trim().Split(";").Distinct().ToList();
                        ItemCodes = ItemCodes
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToList(); // b??? c??c kho???ng tr???ng ho???c null trong list item code
                        List<KpiProductGrouping_ItemImportDTO> LineItems = new List<KpiProductGrouping_ItemImportDTO>();
                        foreach (var ItemCode in ItemCodes)
                        {
                            KpiProductGrouping_ItemImportDTO Item = Items.Where(x => x.Code.ToLower() == ItemCode.ToLower().Trim()).FirstOrDefault();
                            if (Item == null)
                            {
                                errorContent.AppendLine($"L???i d??ng th??? {i}: S???n ph???m kh??ng t???n t???i");
                                continue;
                            }
                            else if (KpiProductGroupingType.Id == KpiProductGroupingTypeEnum.NEW_PRODUCT_GROUPING.Id && !Item.IsNew)
                            {
                                errorContent.AppendLine($"L???i d??ng th??? {i}: {Item.Code} - {Item.Name} Kh??ng ph???i s???n ph???m m???i");
                                continue;
                            }
                            //else if (ProductGrouping.Id != Item.ProductGroupingId)
                            //{
                            //    errorContent.AppendLine($"L???i d??ng th??? {i + 1}: S???n ph???m kh??ng thu???c nh??m s???n ph???m");
                            //    continue;
                            //} // ko c???n validate Item trong group v?? c?? th??? ch???n Item kh??c Group
                            else
                            {
                                LineItems.Add(Item);
                            }
                        }
                        KpiProductGrouping_ImportDTO.Items = LineItems; // lay ra tat ca cac item hop le
                    }
                    //if (string.IsNullOrWhiteSpace(RevenueValue) && string.IsNullOrWhiteSpace(StoreValue))
                    //{
                    //    errorContent.AppendLine($"L???i d??ng th??? {i}: Ch??a nh???p ch??? ti??u");
                    //    continue;
                    //}
                    //else
                    //{
                    //    decimal Revenue;
                    //    if (!string.IsNullOrWhiteSpace(RevenueValue) && !decimal.TryParse(RevenueValue, out Revenue))
                    //    {
                    //        errorContent.AppendLine($"L???i d??ng th??? {i}: Ch??? ti??u doanh thu kh??ng h???p l???");
                    //        continue;
                    //    }
                    //    long StoreCount;
                    //    if (!string.IsNullOrWhiteSpace(StoreValue) && !long.TryParse(StoreValue, out StoreCount))
                    //    {
                    //        errorContent.AppendLine($"L???i d??ng th??? {i}: Ch??? ti??u s??? c???a h??ng kh??ng h???p l???");
                    //        continue;
                    //    }

                    //}
                    #endregion

                    KpiProductGrouping_ImportDTO.Stt = i;
                    KpiProductGrouping_ImportDTO.Username = UsernameValue;
                    KpiProductGrouping_ImportDTO.ProductGroupingCode = ProductGroupingCodeValue;
                    KpiProductGrouping_ImportDTO.SelectAllCurrentItem = SelectAllItem;
                    KpiProductGrouping_ImportDTO.CriteriaContents = new List<KpiProductGrouping_CriteriaContent>();
                    KpiProductGrouping_ImportDTO.KpiPeriodId = KpiPeriod.Id;
                    KpiProductGrouping_ImportDTO.KpiYearId = KpiYear.Id;

                    for (int j = 0; j < KpiProductGroupingCriterias.Count; j++)
                    {
                        KpiProductGrouping_CriteriaContent KpiProductGrouping_CriteriaContent = new KpiProductGrouping_CriteriaContent();
                        KpiProductGrouping_CriteriaContent.CriteriaId = KpiProductGroupingCriterias[j].Id;
                        KpiProductGrouping_CriteriaContent.CriteriaName = KpiProductGroupingCriterias[j].Name;
                        KpiProductGrouping_CriteriaContent.Value = worksheet.Cells[i, CriteriaStartColumn + j].Value?.ToString();

                        KpiProductGrouping_ImportDTO.CriteriaContents.Add(KpiProductGrouping_CriteriaContent);
                    }

                    KpiProductGrouping_ImportDTOs.Add(KpiProductGrouping_ImportDTO);
                }
            }

            Dictionary<long, StringBuilder> Errors = new Dictionary<long, StringBuilder>();
            Dictionary<long, int> OldKpiCountDict = new Dictionary<long, int>();
            foreach (KpiProductGrouping_ImportDTO ImportDTO in KpiProductGrouping_ImportDTOs)
            {
                Errors.Add(ImportDTO.Stt, new StringBuilder(""));
                if (ImportDTO.HasValue == false)
                {
                    Errors[ImportDTO.Stt].Append($"L???i d??ng th??? {ImportDTO.Stt}: Ch??a nh???p ch??? ti??u");
                    continue;
                }
                KpiProductGrouping KpiProductGroupingInDB = KpiProductGroupings.Where(x => x.EmployeeId == ImportDTO.EmployeeId &&
                    x.KpiPeriodId == ImportDTO.KpiPeriodId &&
                    x.KpiYearId == ImportDTO.KpiYearId &&
                    x.KpiProductGroupingTypeId == ImportDTO.KpiProductGroupingTypeId)
                    .FirstOrDefault(); // tim trong 
                if (KpiProductGroupingInDB == null)
                {
                    KpiProductGroupingInDB = new KpiProductGrouping();
                    KpiProductGroupingInDB.EmployeeId = ImportDTO.EmployeeId;
                    KpiProductGroupingInDB.OrganizationId = ImportDTO.OrganizationId;
                    KpiProductGroupingInDB.KpiPeriodId = ImportDTO.KpiPeriodId;
                    KpiProductGroupingInDB.KpiYearId = ImportDTO.KpiYearId;
                    KpiProductGroupingInDB.KpiProductGroupingTypeId = ImportDTO.KpiProductGroupingTypeId;
                    KpiProductGroupingInDB.RowId = Guid.NewGuid();
                    KpiProductGroupingInDB.CreatedAt = StaticParams.DateTimeNow;
                    KpiProductGroupingInDB.KpiProductGroupingContents = new List<KpiProductGroupingContent>();
                    KpiProductGroupingInDB.KpiProductGroupingContents.Add(new KpiProductGroupingContent
                    {
                        ProductGroupingId = ImportDTO.ProductGroupingId,
                        SelectAllCurrentItem = ImportDTO.SelectAllCurrentItem,
                        RowId = Guid.NewGuid(),
                        KpiProductGroupingContentCriteriaMappings = KpiProductGroupingCriterias.Select(x => new KpiProductGroupingContentCriteriaMapping
                        {
                            KpiProductGroupingCriteriaId = x.Id,
                        }).ToList(),
                        KpiProductGroupingContentItemMappings = ImportDTO.SelectAllCurrentItem ? new List<KpiProductGroupingContentItemMapping>() :
                        ImportDTO.Items.Select(x => new KpiProductGroupingContentItemMapping
                        {
                            ItemId = x.Id
                        }).ToList()
                    });
                    KpiProductGroupings.Add(KpiProductGroupingInDB);
                    OldKpiCountDict.Add(KpiProductGroupingInDB.Id, 1);
                } // neu them moi Kpi
                else
                {
                    int OldKpiCount = default(int);
                    if (!OldKpiCountDict.TryGetValue(KpiProductGroupingInDB.Id, out OldKpiCount))
                    {
                        OldKpiCountDict.Add(KpiProductGroupingInDB.Id, 1);
                        KpiProductGroupingInDB.KpiProductGroupingContents = new List<KpiProductGroupingContent>();
                        KpiProductGroupingInDB.KpiProductGroupingContents.Add(new KpiProductGroupingContent
                        {
                            ProductGroupingId = ImportDTO.ProductGroupingId,
                            SelectAllCurrentItem = ImportDTO.SelectAllCurrentItem,
                            RowId = Guid.NewGuid(),
                            KpiProductGroupingContentCriteriaMappings = KpiProductGroupingCriterias.Select(x => new KpiProductGroupingContentCriteriaMapping
                            {
                                KpiProductGroupingCriteriaId = x.Id,
                            }).ToList(),
                            KpiProductGroupingContentItemMappings = ImportDTO.SelectAllCurrentItem ? new List<KpiProductGroupingContentItemMapping>() :
                            ImportDTO.Items.Select(x => new KpiProductGroupingContentItemMapping
                            {
                                ItemId = x.Id
                            }).ToList()
                        });
                    } // n???u l?? l???n ?????u ti??n update kpi th?? x??a h???t content c???a kpi ???? v?? th??m m???i m???t kpi content t????ng ???ng
                    else
                    {
                        var KpiProductGroupingContent = KpiProductGroupingInDB.KpiProductGroupingContents
                            .Where(x => x.KpiProductGroupingId == ImportDTO.ProductGroupingId)
                            .FirstOrDefault();
                        if (KpiProductGroupingContent == null)
                        {
                            KpiProductGroupingInDB.KpiProductGroupingContents.Add(new KpiProductGroupingContent
                            {
                                ProductGroupingId = ImportDTO.ProductGroupingId,
                                SelectAllCurrentItem = ImportDTO.SelectAllCurrentItem,
                                RowId = Guid.NewGuid(),
                                KpiProductGroupingContentCriteriaMappings = KpiProductGroupingCriterias.Select(x => new KpiProductGroupingContentCriteriaMapping
                                {
                                    KpiProductGroupingCriteriaId = x.Id,
                                }).ToList(),
                                KpiProductGroupingContentItemMappings = ImportDTO.SelectAllCurrentItem ? new List<KpiProductGroupingContentItemMapping>() :
                                ImportDTO.Items.Select(x => new KpiProductGroupingContentItemMapping
                                {
                                    ItemId = x.Id
                                }).ToList()
                            });
                        }
                    }

                }
                var Content = KpiProductGroupingInDB.KpiProductGroupingContents
                            .Where(x => x.ProductGroupingId == ImportDTO.ProductGroupingId)
                            .FirstOrDefault();
                if (Content != null)
                {
                    foreach (var KpiProductGroupingContentCriteriaMapping in Content.KpiProductGroupingContentCriteriaMappings)
                    {
                        KpiProductGrouping_CriteriaContent criteriaContent = ImportDTO.CriteriaContents.Where(x => x.CriteriaId == KpiProductGroupingContentCriteriaMapping.KpiProductGroupingCriteriaId)
                            .FirstOrDefault();
                        if (long.TryParse(criteriaContent.Value, out long value))
                        {
                            KpiProductGroupingContentCriteriaMapping.Value = value;
                        }
                    } // update list mapping content voi chi tieu
                }

                KpiProductGroupingInDB.CreatorId = AppUser.Id;
                KpiProductGroupingInDB.StatusId = StatusEnum.ACTIVE.Id;
            } // thiet lap Kpi, KpiContent tu DTOs

            #endregion
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString()); // tra ve loi khi lay du lieu
            string error = string.Join("\n", Errors.Where(x => !string.IsNullOrWhiteSpace(x.Value.ToString())).Select(x => x.Value.ToString()).ToList());
            if (!string.IsNullOrWhiteSpace(error))
                return BadRequest(error);
            KpiProductGroupings = await KpiProductGroupingService.Import(KpiProductGroupings);
            List<KpiProductGrouping_KpiProductGroupingDTO> KpiProductGrouping_KpiProductGroupingDTOs = KpiProductGroupings
                .Select(x => new KpiProductGrouping_KpiProductGroupingDTO(x))
                .ToList();
            return Ok(KpiProductGroupings);
        }

        [Route(KpiProductGroupingRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiProductGrouping_KpiProductGroupingFilterDTO KpiProductGrouping_KpiProductGroupingFilterDTO)
        {
            #region validate d??? li???u filter b???t bu???c c?? 
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (KpiProductGrouping_KpiProductGroupingFilterDTO.KpiYearId.Equal.HasValue == false)
                return BadRequest(new { message = "Ch??a ch???n n??m Kpi" });

            if (KpiProductGrouping_KpiProductGroupingFilterDTO.KpiPeriodId.Equal.HasValue == false)
                return BadRequest(new { message = "Ch??a ch???n k??? Kpi" });

            #endregion

            #region d??? li???u k??, n??m kpi
            long KpiYearId = KpiProductGrouping_KpiProductGroupingFilterDTO.KpiYearId.Equal.Value;
            var KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiYearId).FirstOrDefault();

            long KpiPeriodId = KpiProductGrouping_KpiProductGroupingFilterDTO.KpiPeriodId.Equal.Value;
            var KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Id == KpiPeriodId).FirstOrDefault();
            #endregion

            #region l???y ra d??? li???u kpiProductGrouping t??? filter
            KpiProductGrouping_KpiProductGroupingFilterDTO.Skip = 0;
            KpiProductGrouping_KpiProductGroupingFilterDTO.Take = int.MaxValue;
            KpiProductGrouping_KpiProductGroupingFilterDTO.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            KpiProductGrouping_KpiProductGroupingFilterDTO.KpiPeriodId = new IdFilter { Equal = KpiPeriodId };
            KpiProductGrouping_KpiProductGroupingFilterDTO.KpiYearId = new IdFilter { Equal = KpiYearId };

            List<KpiProductGrouping_KpiProductGroupingDTO> KpiProductGrouping_KpiProductGroupingDTOs = await List(KpiProductGrouping_KpiProductGroupingFilterDTO);
            List<long> KpiProductGroupingIds = KpiProductGrouping_KpiProductGroupingDTOs.Select(x => x.Id)
                .Distinct()
                .ToList();
            List<long> AppUserIds = KpiProductGrouping_KpiProductGroupingDTOs.Select(x => x.EmployeeId)
                .Distinct()
                .ToList();
            List<long> OrganizationIds = KpiProductGrouping_KpiProductGroupingDTOs
                .Select(x => x.OrganizationId)
                .Distinct()
                .ToList();
            List<KpiProductGroupingContent> KpiProductGroupingContents = await KpiProductGroupingContentService.List(new KpiProductGroupingContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingContentSelect.ALL,
                KpiProductGroupingId = new IdFilter { In = KpiProductGroupingIds }
            });
            List<KpiProductGroupingContentCriteriaMapping> kpiProductGroupingContentCriteriaMappings = KpiProductGroupingContents
                .SelectMany(x => x.KpiProductGroupingContentCriteriaMappings)
                .ToList(); // lay ra mapping content voi chi tieu
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Id | OrganizationSelect.Name,
                Id = new IdFilter { In = OrganizationIds }
            });
            List<AppUser> AppUsers = await AppUserService.List(new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName,
                Id = new IdFilter { In = AppUserIds }
            });

            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL
            });
            KpiProductGroupingCriterias = KpiProductGroupingCriterias.OrderBy(x => x.Id).ToList();
            #endregion

            #region t??? h???p d??? li???u
            List<KpiProductGrouping_ExportDTO> KpiProductGrouping_ExportDTOs = new List<KpiProductGrouping_ExportDTO>();
            long stt = 1;
            foreach (var Organization in Organizations)
            {
                KpiProductGrouping_ExportDTO kpiProductGrouping_ExportDTO = new KpiProductGrouping_ExportDTO();
                kpiProductGrouping_ExportDTO.OrganizationName = Organization.Name;
                kpiProductGrouping_ExportDTO.Kpis = new List<KpiProductGrouping_KpiProductGroupingExportDTO>();
                KpiProductGrouping_ExportDTOs.Add(kpiProductGrouping_ExportDTO);

                List<KpiProductGrouping_KpiProductGroupingDTO> SubKpiProductGroupings = KpiProductGrouping_KpiProductGroupingDTOs
                    .Where(x => x.OrganizationId == Organization.Id)
                    .ToList();
                List<long> SubAppUserIds = SubKpiProductGroupings
                    .Select(x => x.EmployeeId)
                    .ToList();
                List<AppUser> SubAppUsers = AppUsers
                    .Where(x => SubAppUserIds.Contains(x.Id))
                    .Distinct()
                    .ToList();
                foreach (var SubAppUser in SubAppUsers)
                {
                    List<KpiProductGrouping_KpiProductGroupingDTO> AppUserKpis = SubKpiProductGroupings
                        .Where(x => x.EmployeeId == SubAppUser.Id)
                        .ToList(); // lay tat cac kpi cua User

                    foreach (KpiProductGrouping_KpiProductGroupingDTO AppUserKpi in AppUserKpis)
                    {
                        KpiProductGrouping_KpiProductGroupingExportDTO KpiProductGrouping_KpiProductGroupingExportDTO = new KpiProductGrouping_KpiProductGroupingExportDTO();
                        KpiProductGrouping_KpiProductGroupingExportDTO.ProductGroupings = new List<KpiProductGrouping_ProductGroupingExportDTO>();
                        kpiProductGrouping_ExportDTO.Kpis.Add(KpiProductGrouping_KpiProductGroupingExportDTO);

                        List<KpiProductGroupingContent> SubContents = KpiProductGroupingContents
                            .Where(x => x.KpiProductGroupingId == AppUserKpi.Id)
                            .ToList();
                        foreach (KpiProductGroupingContent KpiProductGroupingContent in SubContents)
                        {
                            KpiProductGrouping_ProductGroupingExportDTO KpiProductGrouping_ProductGroupingExportDTO = new KpiProductGrouping_ProductGroupingExportDTO();
                            List<KpiProductGroupingContentCriteriaMapping> SubCriteriaMappings = KpiProductGroupingContent.KpiProductGroupingContentCriteriaMappings;
                            KpiProductGrouping_ProductGroupingExportDTO.Item_Lines = new List<KpiProductGrouping_ExportItemDTO>();

                            if (KpiProductGroupingContent.SelectAllCurrentItem)
                            {
                                KpiProductGrouping_ExportItemDTO line = new KpiProductGrouping_ExportItemDTO();
                                line.STT = stt;
                                line.UserName = SubAppUser.Username;
                                line.DisplayName = SubAppUser.DisplayName;
                                line.KpiProductGroupingTypeName = AppUserKpi.KpiProductGroupingType.Name;
                                line.ProductCode = KpiProductGroupingContent.ProductGrouping.Code;
                                line.ProductName = KpiProductGroupingContent.ProductGrouping.Name;
                                //line.ItemCount = KpiProductGroupingContent.KpiProductGroupingContentItemMappings.Count;
                                //line.ItemCode = "All";
                                line.ItemName = "Ch???n t???t c??? s???n ph???m trong nh??m";
                                line.CriteriaContents = new List<KpiProductGrouping_ExportCriteriaContent>();

                                for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                                {
                                    long? value = SubCriteriaMappings.Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriterias[i].Id)
                        .Select(x => x.Value).FirstOrDefault();
                                    KpiProductGrouping_ExportCriteriaContent KpiProductGrouping_ExportCriteriaContent = new KpiProductGrouping_ExportCriteriaContent
                                    {
                                        CriteriaId = KpiProductGroupingCriterias[i].Id,
                                        CriteriaName = KpiProductGroupingCriterias[i].Name,
                                        Value = value,
                                    };
                                    line.CriteriaContents.Add(KpiProductGrouping_ExportCriteriaContent);
                                }

                                // Ph???i th??m ????ng theo th??? t??? n??y ????? fill d??? li???u ????ng c???t
                                KpiProductGrouping_ProductGroupingExportDTO.Item_Lines.Add(line);
                            }
                            else
                            {
                                bool isFirstItem = true;
                                foreach (var SubItemMapping in KpiProductGroupingContent.KpiProductGroupingContentItemMappings)
                                {
                                    KpiProductGrouping_ExportItemDTO line = new KpiProductGrouping_ExportItemDTO();
                                    line.STT = stt;
                                    line.UserName = SubAppUser.Username;
                                    line.DisplayName = SubAppUser.DisplayName;
                                    line.KpiProductGroupingTypeName = AppUserKpi.KpiProductGroupingType.Name;
                                    line.ProductCode = KpiProductGroupingContent.ProductGrouping.Code;
                                    line.ProductName = KpiProductGroupingContent.ProductGrouping.Name;
                                    line.ItemCount = KpiProductGroupingContent.KpiProductGroupingContentItemMappings.Count;
                                    line.ItemCode = SubItemMapping.Item.Code;
                                    line.ItemName = SubItemMapping.Item.Name;
                                    line.CriteriaContents = new List<KpiProductGrouping_ExportCriteriaContent>();

                                    for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                                    {
                                        long? value = SubCriteriaMappings.Where(x => x.KpiProductGroupingCriteriaId == KpiProductGroupingCriterias[i].Id)
                            .Select(x => x.Value).FirstOrDefault();
                                        KpiProductGrouping_ExportCriteriaContent KpiProductGrouping_ExportCriteriaContent = new KpiProductGrouping_ExportCriteriaContent
                                        {
                                            CriteriaId = KpiProductGroupingCriterias[i].Id,
                                            CriteriaName = KpiProductGroupingCriterias[i].Name,
                                            Value = isFirstItem ? value : null, // ch??? ??i???n value v??o d??ng item ?????u ti??n c???a nh??m
                                        };
                                        line.CriteriaContents.Add(KpiProductGrouping_ExportCriteriaContent);
                                    }

                                    // Ph???i th??m ????ng theo th??? t??? n??y ????? fill d??? li???u ????ng c???t
                                    KpiProductGrouping_ProductGroupingExportDTO.Item_Lines.Add(line);
                                    isFirstItem = false;
                                }
                            }
                            KpiProductGrouping_KpiProductGroupingExportDTO.ProductGroupings.Add(KpiProductGrouping_ProductGroupingExportDTO);
                        }

                        stt++;
                    }
                } // th??m Employee
            } // th??m orgName
            KpiProductGrouping_ExportDTOs = KpiProductGrouping_ExportDTOs.Where(x => x.Kpis.Count > 0).ToList(); // ch??? l???y org n??o c?? d??? li???u
            #endregion

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("KPI nh??m s???n ph???m");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["A1"].Value = "THI???T L???P KPI NH??M S???N PH???M";

                ws.Cells["A2"].Value = $"K???: {KpiPeriod.Name} - N??m: {KpiYear.Name}";
                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "STT",
                    "M?? nh??n vi??n",
                    "T??n nh??n vi??n",
                    "Lo???i KPI nh??m s???n ph???m",
                    "M?? nh??m s???n ph???m",
                    "T??n nh??m s???n ph???m",
                    "S??? s???n ph???m",
                    "M?? s???n ph???m",
                    "T??n s???n ph???m"
                };

                for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                {
                    headers.Add(KpiProductGroupingCriterias[i].Name);
                }

                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64);

                // format l???i ti??u ????? theo s??? c???t c???a d??? li???u
                ws.Cells[$"A1:{endColumnString}1"].Merge = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Size = 14;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Bold = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells[$"A2:{endColumnString}2"].Merge = true;
                ws.Cells[$"A2:{endColumnString}2"].Style.Font.Bold = true;
                ws.Cells[$"A2:{endColumnString}2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Formart header
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
                if (KpiProductGrouping_ExportDTOs != null && KpiProductGrouping_ExportDTOs.Count > 0)
                {
                    foreach (var KpiProductGrouping_ExportDTO in KpiProductGrouping_ExportDTOs)
                    {

                        ws.Cells[$"A{startRow}"].Value = KpiProductGrouping_ExportDTO.OrganizationName;
                        ws.Cells[$"A{startRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Merge = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

                        endRow = startRow; // G??n l???i endrow = start row d??? b???t ?????u v??ng d??? li???u m???i
                        List<KpiProductGrouping_ExportItemDTO> lines = new List<KpiProductGrouping_ExportItemDTO>();
                        foreach (var kpi in KpiProductGrouping_ExportDTO.Kpis)
                        {
                            int startItemRow = startRow;
                            foreach (var productgrouping in kpi.ProductGroupings)
                            {
                                if (productgrouping.Item_Lines.Count == 0) continue;
                                int startColumn = 1; // g??n l???i c???t b???t ?????u t??? A
                                int endColumn = startColumn;

                                #region C???t STT
                                List<Object[]> SttData = new List<object[]>();
                                endRow = startItemRow;
                                foreach (var item in productgrouping.Item_Lines)
                                {
                                    SttData.Add(new object[]
                                    {
                                         item.STT,
                                    });
                                    endRow++;
                                }
                                string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                                if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                                string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                                if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                                ws.Cells[$"{startColumnString}{startItemRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(SttData);
                                // d??ng ?????u ti??n l?? d??ng org n??n startRow + 1
                                endColumn += 1; // chi???m 1 c???t
                                startColumn = endColumn; // g??n l???i ????? ti???p t???c fill d??? li???u ti???p sau
                                #endregion

                                #region C???t m?? nh??n vi??n, t??n nh??n vi??n, Lo???i KPI nh??m s???n ph???m, ...
                                List<Object[]> KpiData = new List<object[]>();
                                endRow = startItemRow; // g??n l???i d??ng b???t ?????u
                                foreach (var item in productgrouping.Item_Lines)
                                {
                                    KpiData.Add(new object[]
                                    {
                                        item.UserName,
                                        item.DisplayName,
                                        item.KpiProductGroupingTypeName,
                                        item.ProductCode,
                                        item.ProductName,
                                        item.ItemCount,
                                        item.ItemCode,
                                        item.ItemName,
                                    });
                                    endRow++;
                                }
                                startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                                if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                                currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                                if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                                ws.Cells[$"{startColumnString}{startItemRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(KpiData);
                                // d??ng ?????u ti??n l?? d??ng org n??n startRow + 1
                                endColumn += 8; // chi???m 8 c???t
                                startColumn = endColumn; // g??n l???i ????? ti???p t???c fill d??? li???u ti???p sau
                                #endregion

                                #region C??c c???t gi?? tr???
                                for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                                {
                                    List<Object[]> ValueData = new List<object[]>();
                                    endRow = startItemRow; // g??n l???i d??ng b???t ?????u
                                    foreach (var item in productgrouping.Item_Lines)
                                    {
                                        KpiProductGrouping_ExportCriteriaContent criteriaContent = item
                                            .CriteriaContents.Where(x => x.CriteriaId == KpiProductGroupingCriterias[i].Id).FirstOrDefault(); // L???y ra gi?? tr??? c???a criteria t????ng ???ng
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

                                    ws.Cells[$"{startColumnString}{startItemRow + 1}:{currentColumnString}{endRow}"].LoadFromArrays(ValueData); // fill d??? li???u

                                    endColumn += 1; // Chi???m 1 c???t cho m???i criteria
                                    startColumn = endColumn; // g??n l???i c???t b???t ?????u cho d??? li???u ti???p sau
                                }
                                #endregion

                                ws.Cells[$"E{startItemRow + 1}:E{endRow}"].Merge = true;
                                ws.Cells[$"F{startItemRow + 1}:F{endRow}"].Merge = true;
                                ws.Cells[$"G{startItemRow + 1}:G{endRow}"].Merge = true;
                                startItemRow = endRow;
                            }
                            ws.Cells[$"A{startRow + 1}:A{endRow}"].Merge = true;
                            ws.Cells[$"B{startRow + 1}:B{endRow}"].Merge = true;
                            ws.Cells[$"C{startRow + 1}:C{endRow}"].Merge = true;
                            ws.Cells[$"D{startRow + 1}:D{endRow}"].Merge = true;
                            startRow = endRow;
                        }
                        startRow = endRow + 1; // g??n d??ng b???t ?????u cho org ti???p theo
                    }


                }

                ws.Cells[$"G5:G{endRow}"].Style.Numberformat.Format = "#,##0"; // format number column so san pham
                ws.Cells[$"J5:{endColumnString}{endRow}"].Style.Numberformat.Format = "#,##0"; // format number column value
                ws.Column(9).Width = 40;
                // All borders
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{endRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "KpiProductGroupings.xlsx");
        }

        [Route(KpiProductGroupingRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            #region d??? li???u sheet ch??nh
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
            List<KpiProductGrouping_ExportTemplateDTO> KpiProductGrouping_ExportTemplateDTOs = new List<KpiProductGrouping_ExportTemplateDTO>();
            foreach (var AppUser in AppUsers)
            {
                KpiProductGrouping_ExportTemplateDTO KpiProductGrouping_ExportTemplateDTO = new KpiProductGrouping_ExportTemplateDTO();
                KpiProductGrouping_ExportTemplateDTO.Username = AppUser.Username;
                KpiProductGrouping_ExportTemplateDTO.DisplayName = AppUser.DisplayName;
                KpiProductGrouping_ExportTemplateDTOs.Add(KpiProductGrouping_ExportTemplateDTO);
            }
            #endregion


            #region d??? li???u ????? v??o c??c sheet kh??c
            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(new ProductGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductGroupingSelect.Code | ProductGroupingSelect.Name,
                OrderBy = ProductGroupingOrder.Id,
                OrderType = OrderType.ASC
            });

            List<Item> Items = await ItemService.List(new ItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                Selects = ItemSelect.Code | ItemSelect.Name | ItemSelect.Product,
                OrderBy = ItemOrder.Id,
                OrderType = OrderType.ASC
            });
            List<KpiProductGrouping_ItemExportDTO> ItemDTOs = new List<KpiProductGrouping_ItemExportDTO>();
            foreach (var item in Items)
            {
                KpiProductGrouping_ItemExportDTO KpiProductGrouping_ItemExportDTO = new KpiProductGrouping_ItemExportDTO
                {
                    ItemCode = Utils.ReplaceHexadecimalSymbols(item.Code),
                    ItemName = Utils.ReplaceHexadecimalSymbols(item.Name),
                    IsNew = item.Product.IsNew,
                };
                foreach (var product in item.Product.ProductProductGroupingMappings)
                {
                    KpiProductGrouping_ItemExportDTO.ProductGroupingCode = product == null ? "" : Utils.ReplaceHexadecimalSymbols(product.ProductGrouping.Code);
                    KpiProductGrouping_ItemExportDTO.ProductGroupingName = product == null ? "" : Utils.ReplaceHexadecimalSymbols(product.ProductGrouping.Name);
                }
                ItemDTOs.Add(KpiProductGrouping_ItemExportDTO);
            }
            ItemDTOs = ItemDTOs.OrderByDescending(x => x.ProductGroupingCode).ToList();

            List<KpiProductGrouping_ItemExportDTO> NewItems = ItemDTOs.Where(x => x.IsNew).ToList();

            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await KpiProductGroupingCriteriaService.List(new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiProductGroupingCriteriaSelect.ALL
            });
            KpiProductGroupingCriterias = KpiProductGroupingCriterias.OrderBy(x => x.Id).ToList();
            #endregion

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("KPI nh??m s???n ph???m");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["A1"].Value = "THI???T L???P KPI NH??M S???N PH???M";

                ws.Cells["C2"].Value = "K???";
                ws.Cells["D2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["D2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E7E6E6"));
                var PeriodDropdown = ws.DataValidations.AddListValidation("D2");
                for (int i = 0; i < KpiPeriodEnum.KpiPeriodEnumList.Count; i++)
                {
                    PeriodDropdown.Formula.Values.Add(KpiPeriodEnum.KpiPeriodEnumList[i].Name);
                }

                ws.Cells["E2"].Value = "N??m";
                ws.Cells["F2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["F2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E7E6E6"));
                var YearDropdown = ws.DataValidations.AddListValidation("F2");
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
                    "M?? nh??n vi??n",
                    "T??n nh??n vi??n",
                    "Lo???i KPI nh??m s???n ph???m",
                    "M?? nh??m s???n ph???m",
                    "Ch???n t???t c??? s???n ph???m trong nh??m",
                    "M?? s???n ph???m",
                };

                for (int i = 0; i < KpiProductGroupingCriterias.Count; i++)
                {
                    headers.Add(KpiProductGroupingCriterias[i].Name);
                }

                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64);

                // format l???i ti??u ????? theo s??? c???t c???a d??? li???u
                ws.Cells[$"A1:{endColumnString}1"].Merge = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Size = 14;
                ws.Cells[$"A1:{endColumnString}1"].Style.Font.Bold = true;
                ws.Cells[$"A1:{endColumnString}1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Format header
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
                foreach (var KpiProductGrouping_ExportTemplateDTO in KpiProductGrouping_ExportTemplateDTOs)
                {
                    int currentRow = startRow;
                    ws.Cells[$"A{currentRow}"].Value = $"{KpiProductGrouping_ExportTemplateDTO.Username} - {KpiProductGrouping_ExportTemplateDTO.DisplayName}";
                    ws.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                    ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Merge = true;
                    ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[$"A{currentRow}:{endColumnString}{currentRow}"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

                    currentRow++;
                    List<string[]> Contents = new List<string[]>();
                    List<string> lineData = new List<string>
                    {
                        KpiProductGrouping_ExportTemplateDTO.Username,
                        KpiProductGrouping_ExportTemplateDTO.DisplayName,
                    };
                    Contents.Add(lineData.ToArray());
                    ws.Cells[$"A{startRow + 1}:{endColumnString}{currentRow}"].LoadFromArrays(Contents);
                    startRow = currentRow + 1; // G??n l???i start row cho org ti???p theo
                }
                ws.Cells[$"A{startRow}"].Value = "END";
                ws.Cells[$"B{startRow}"].Value = "Vui l??ng ch???n lo???i KPI nh??m s???n ph???m; insert c??c m?? nh??m s???n ph???m, m?? s???n ph???m v?? ??i???n s??? ch??? ti??u KPI v??o theo t???ng nh??n vi??n";
                ws.Cells[$"A{startRow}:B{startRow}"].Style.Font.Bold = true;

                // add Drop down Kpi product grouping type
                var KpiProductGroupingTypeDropdown = ws.DataValidations.AddListValidation($"C6:C{startRow}");
                for (int i = 0; i < KpiProductGroupingTypeEnum.KpiProductGroupingTypeEnumList.Count; i++)
                {
                    KpiProductGroupingTypeDropdown.Formula.Values.Add(KpiProductGroupingTypeEnum.KpiProductGroupingTypeEnumList[i].Name);
                }

                // add Drop down ch???n t???t c??? s???n ph???m trong nh??m
                var SelectAllItemDropdown = ws.DataValidations.AddListValidation($"E6:E{startRow}");
                SelectAllItemDropdown.Formula.Values.Add("C??");
                SelectAllItemDropdown.Formula.Values.Add("Kh??ng");

                ws.Cells[$"G6:{endColumnString}{startRow}"].Style.Numberformat.Format = "#,##0"; // format number column value

                ws.Column(1).Width = 15;
                ws.Column(2).Width = 25;
                // All borders
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A4:{endColumnString}{startRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                #region Sheet nh??m s???n ph???m
                ExcelWorksheet ws2 = excel.Workbook.Worksheets.Add("Danh s??ch nh??m s???n ph???m");
                ws2.Cells["A1"].Value = "M?? nh??m s???n ph???m";
                ws2.Cells["B1"].Value = "T??n nh??m s???n ph???m";
                ws2.Cells["A1:B1"].Style.Font.Bold = true;
                ws2.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws2.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFE699"));
                if (ProductGroupings != null && ProductGroupings.Count > 0)
                {
                    List<string[]> productGroupingData = new List<string[]>();
                    foreach (var productGrouping in ProductGroupings)
                    {
                        productGroupingData.Add(new string[] { productGrouping.Code, productGrouping.Name });
                    }
                    ws2.Cells[$"A2:A{productGroupingData.Count + 1}"].LoadFromArrays(productGroupingData);
                }
                ws2.Column(1).Width = 25;
                ws2.Column(2).Width = 50;
                #endregion

                #region Sheet s???n ph???m
                ExcelWorksheet ws3 = excel.Workbook.Worksheets.Add("S???n ph???m");
                ws3.Cells["A1"].Value = "M?? s???n ph???m";
                ws3.Cells["B1"].Value = "T??n s???n ph???m";
                ws3.Cells["C1"].Value = "M?? nh??m s???n ph???m";
                ws3.Cells["D1"].Value = "T??n nh??m s???n ph???m";
                ws3.Cells["A1:D1"].Style.Font.Bold = true;
                ws3.Cells["A1:D1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws3.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFE699"));
                if (ItemDTOs != null & ItemDTOs.Count > 0)
                {
                    List<string[]> itemData = new List<string[]>();
                    foreach (var item in ItemDTOs)
                    {
                        itemData.Add(new string[] { item.ItemCode, item.ItemName, item.ProductGroupingCode, item.ProductGroupingName });
                    }
                    ws3.Cells[$"A2:A{itemData.Count + 1}"].LoadFromArrays(itemData);
                }

                ws3.Column(1).Width = 20;
                ws3.Column(2).Width = 50;
                ws3.Column(3).Width = 25;
                ws3.Column(4).Width = 50;
                #endregion

                #region Sheet s???n ph???m m???i
                ExcelWorksheet ws4 = excel.Workbook.Worksheets.Add("S???n ph???m m???i");
                ws4.Cells["A1"].Value = "M?? s???n ph???m";
                ws4.Cells["B1"].Value = "T??n s???n ph???m";
                ws4.Cells["C1"].Value = "M?? nh??m s???n ph???m";
                ws4.Cells["D1"].Value = "T??n nh??m s???n ph???m";
                ws4.Cells["A1:D1"].Style.Font.Bold = true;
                ws4.Cells["A1:D1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws4.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFE699"));
                if (NewItems != null && NewItems.Count > 0)
                {
                    List<string[]> newItemData = new List<string[]>();
                    foreach (var item in NewItems)
                    {
                        newItemData.Add(new string[] { item.ItemCode, item.ItemName, item.ProductGroupingCode, item.ProductGroupingName });
                    }
                    ws4.Cells[$"A2:A{newItemData.Count + 1}"].LoadFromArrays(newItemData);
                }
                ws4.Column(1).Width = 20;
                ws4.Column(2).Width = 50;
                ws4.Column(3).Width = 25;
                ws4.Column(4).Width = 50;
                #endregion

                #region Sheet Quy t???c import
                ExcelWorksheet ws5 = excel.Workbook.Worksheets.Add("Quy t???c import");
                ws5.Cells["A1"].Style.Font.Bold = true;
                List<string[]> ws5Text = new List<string[]>
                {
                    new string[] {"Quy t???c Import:" },
                    new string[] {"1. K??? KPI ???????c nh???p d???a theo gi?? tr??? nh???p ??? ?? C2 v?? E2, d??? li???u ???????c l???a ch???n t??? combo" },
                    new string[] {"2. Khi t???i bi???u m???u th?? h??? th???ng s??? xu???t ra file excel c?? ?????y ????? th??ng tin nh?? sau:" },
                    new string[] {" - Nh??n vi??n: hi???n th??? to??n b??? nh??n vi??n theo ph??n quy???n c???a ng?????i d??ng (orgunit, appuser, userID)" },
                    new string[] {" - V???i m???i group nh??n vi??n: xu???t to??n b??? c??c m?? s???n ph???m ???? c?? ch??? ti??u c???a nh??n vi??n + xu???t 1 d??ng tr???ng kh??ng c?? m?? s???n ph???m" },
                    new string[] {" - Gi?? tr???: xu???t gi?? tr??? t????ng ???ng v???i c??c m?? s???n ph???m ???? c?? ch??? ti??u" },
                    new string[] {" - Sheet s???n ph???m: xu???t to??n b??? s???n ph???m ???? c?? status = 1" },
                    new string[] {" - Sheet s???n ph???m m???i: Xu???t to??n b?? danh s??ch c??c s???n ph???m m???i" },
                    new string[] {"3. Khi import th?? check:" },
                    new string[] {" - N???u ???? t???n t???i nh??n vi??n + n??m + m?? s???n ph???m: update v??o b???n ghi ???? t???n t???i" },
                    new string[] {" - N???u ???? t???n t???i nh??n vi??n + n??m + m?? s???n ph???m kh??ng c??: insert b???n ghi s???n ph???m v??o KPI" },
                    new string[] {" - N???u ch??a c?? nh??n vi??n + n??m: insert KPI cho nh??n vi??n ????" },
                    new string[] {" - Khi import th?? check:" },
                    new string[] {"  + M?? nh??n vi??n: check c?? t???n t???i kh??ng, n???u kh??ng th?? hi???n th??? th??ng b??o 'M?? nh??n vi??n kh??ng t???n t???i'" },
                    new string[] {"  + M?? nh??n vi??n - T??n nh??n vi??n:  m?? v?? t??n ph???i tr??ng nhau, n???u kh??ng tr??ng th?? th??ng b??o 'M?? v?? t??n nh??n vi??n kh??ng kh???p'" },
                    new string[] {"  + M?? nh??m s???n ph???m: Check m?? nh??m s???n ph???m trung v???i m?? nh??m tr??n h??? th???ng c?? status =1. kh??ng tr??ng th?? th??ng b??o 'M?? nh??m s???n ph???m kh??ng t???n t???i'" },
                    new string[] {"  + M?? s???n ph???m: check m?? s???n ph???m tr??ng v???i m?? s???n ph???m tr??n h??? th???ng, kh??ng tr??ng th?? th??ng b??o 'M?? s???n ph???m kh??ng t???n t???i'" },
                    new string[] {"  + S???n ph???m ph???i thu???c nh??m s???n ph???m ???? ch???n, n???u kh??ng  thu???c th?? th??ng b??o 'S???n ph???m kh??ng thu???c nh??m s???n ph???m'" },
                    new string[] {"  + Gi?? tr??? c??c ch??? ti??u: n???u nh???p kh??c s??? th?? hi???n th??? th??ng b??o 'Gi?? tr??? ch??? ti??u{M?? ch??? ti??u} ??? d??ng {d??ng} sai ?????nh d???ng'" },
                    new string[] {" - K???t th??c ??? d??ng END" }
                };
                ws5.Cells["A1:A22"].LoadFromArrays(ws5Text);
                ws5.Cells["A1:A22"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws5.Cells["A1:A22"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFF2CC"));
                ws5.Column(1).Width = 105;

                #endregion
                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "Template_Kpi_ProductGrouping.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter();
            KpiProductGroupingFilter = await KpiProductGroupingService.ToFilter(KpiProductGroupingFilter);
            if (Id == 0)
            {

            }
            else
            {
                KpiProductGroupingFilter.Id = new IdFilter { Equal = Id };
                int count = await KpiProductGroupingService.Count(KpiProductGroupingFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private KpiProductGrouping ConvertDTOToEntity(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            KpiProductGrouping_KpiProductGroupingDTO.TrimString();
            KpiProductGrouping KpiProductGrouping = new KpiProductGrouping();
            KpiProductGrouping.Id = KpiProductGrouping_KpiProductGroupingDTO.Id;
            KpiProductGrouping.OrganizationId = KpiProductGrouping_KpiProductGroupingDTO.OrganizationId;
            KpiProductGrouping.KpiYearId = KpiProductGrouping_KpiProductGroupingDTO.KpiYearId;
            KpiProductGrouping.KpiPeriodId = KpiProductGrouping_KpiProductGroupingDTO.KpiPeriodId;
            KpiProductGrouping.KpiProductGroupingTypeId = KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingTypeId;
            KpiProductGrouping.StatusId = KpiProductGrouping_KpiProductGroupingDTO.StatusId;
            KpiProductGrouping.EmployeeId = KpiProductGrouping_KpiProductGroupingDTO.EmployeeId;
            KpiProductGrouping.CreatorId = KpiProductGrouping_KpiProductGroupingDTO.CreatorId;
            KpiProductGrouping.RowId = KpiProductGrouping_KpiProductGroupingDTO.RowId;
            KpiProductGrouping.Creator = KpiProductGrouping_KpiProductGroupingDTO.Creator == null ? null : new AppUser
            {
                Id = KpiProductGrouping_KpiProductGroupingDTO.Creator.Id,
                Username = KpiProductGrouping_KpiProductGroupingDTO.Creator.Username,
                DisplayName = KpiProductGrouping_KpiProductGroupingDTO.Creator.DisplayName,
                Address = KpiProductGrouping_KpiProductGroupingDTO.Creator.Address,
                Email = KpiProductGrouping_KpiProductGroupingDTO.Creator.Email,
                Phone = KpiProductGrouping_KpiProductGroupingDTO.Creator.Phone,
            };
            KpiProductGrouping.Employee = KpiProductGrouping_KpiProductGroupingDTO.Employee == null ? null : new AppUser
            {
                Id = KpiProductGrouping_KpiProductGroupingDTO.Employee.Id,
                Username = KpiProductGrouping_KpiProductGroupingDTO.Employee.Username,
                DisplayName = KpiProductGrouping_KpiProductGroupingDTO.Employee.DisplayName,
                Address = KpiProductGrouping_KpiProductGroupingDTO.Employee.Address,
                Email = KpiProductGrouping_KpiProductGroupingDTO.Employee.Email,
                Phone = KpiProductGrouping_KpiProductGroupingDTO.Employee.Phone,
            };
            KpiProductGrouping.KpiPeriod = KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod == null ? null : new KpiPeriod
            {
                Id = KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod.Id,
                Code = KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod.Code,
                Name = KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod.Name,
            };
            KpiProductGrouping.KpiProductGroupingType = KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingType == null ? null : new KpiProductGroupingType
            {
                Id = KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingType.Id,
                Code = KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingType.Code,
                Name = KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingType.Name,
            };
            KpiProductGrouping.Status = KpiProductGrouping_KpiProductGroupingDTO.Status == null ? null : new Status
            {
                Id = KpiProductGrouping_KpiProductGroupingDTO.Status.Id,
                Code = KpiProductGrouping_KpiProductGroupingDTO.Status.Code,
                Name = KpiProductGrouping_KpiProductGroupingDTO.Status.Name,
            };
            KpiProductGrouping.Employees = KpiProductGrouping_KpiProductGroupingDTO.Employees?.Select(x => new AppUser
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Username = x.Username,
                Phone = x.Phone,
                Email = x.Email,
            }).ToList();
            KpiProductGrouping.KpiProductGroupingContents = KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingContents?
                .Select(x => new KpiProductGroupingContent
                {
                    Id = x.Id,
                    ProductGroupingId = x.ProductGroupingId,
                    SelectAllCurrentItem = x.SelectAllCurrentItem,
                    ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                    {
                        Id = x.ProductGrouping.Id,
                        Code = x.ProductGrouping.Code,
                        Name = x.ProductGrouping.Name,
                        ParentId = x.ProductGrouping.ParentId,
                        Path = x.ProductGrouping.Path,
                    },
                    KpiProductGroupingContentCriteriaMappings = x.KpiProductGroupingContentCriteriaMappings.Select(p => new KpiProductGroupingContentCriteriaMapping
                    {
                        KpiProductGroupingCriteriaId = p.Key,
                        Value = p.Value,
                    }).ToList(), // map chi tieu voi gia tri
                    KpiProductGroupingContentItemMappings = x.KpiProductGroupingContentItemMappings.Select(p => new KpiProductGroupingContentItemMapping
                    {
                        ItemId = p.ItemId
                    }).ToList(), // map content voi itemId
                }).ToList();

            KpiProductGrouping.BaseLanguage = CurrentContext.Language;
            return KpiProductGrouping;
        }

        private KpiProductGroupingFilter ConvertFilterDTOToFilterEntity(KpiProductGrouping_KpiProductGroupingFilterDTO KpiProductGrouping_KpiProductGroupingFilterDTO)
        {
            KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter();
            KpiProductGroupingFilter.Selects = KpiProductGroupingSelect.ALL;
            KpiProductGroupingFilter.Skip = KpiProductGrouping_KpiProductGroupingFilterDTO.Skip;
            KpiProductGroupingFilter.Take = KpiProductGrouping_KpiProductGroupingFilterDTO.Take;
            KpiProductGroupingFilter.OrderBy = KpiProductGrouping_KpiProductGroupingFilterDTO.OrderBy;
            KpiProductGroupingFilter.OrderType = KpiProductGrouping_KpiProductGroupingFilterDTO.OrderType;

            KpiProductGroupingFilter.Id = KpiProductGrouping_KpiProductGroupingFilterDTO.Id;
            KpiProductGroupingFilter.OrganizationId = KpiProductGrouping_KpiProductGroupingFilterDTO.OrganizationId;
            KpiProductGroupingFilter.KpiYearId = KpiProductGrouping_KpiProductGroupingFilterDTO.KpiYearId;
            KpiProductGroupingFilter.KpiPeriodId = KpiProductGrouping_KpiProductGroupingFilterDTO.KpiPeriodId;
            KpiProductGroupingFilter.KpiProductGroupingTypeId = KpiProductGrouping_KpiProductGroupingFilterDTO.KpiProductGroupingTypeId;
            KpiProductGroupingFilter.StatusId = KpiProductGrouping_KpiProductGroupingFilterDTO.StatusId;
            KpiProductGroupingFilter.EmployeeId = KpiProductGrouping_KpiProductGroupingFilterDTO.EmployeeId;
            KpiProductGroupingFilter.CreatorId = KpiProductGrouping_KpiProductGroupingFilterDTO.CreatorId;
            KpiProductGroupingFilter.RowId = KpiProductGrouping_KpiProductGroupingFilterDTO.RowId;
            KpiProductGroupingFilter.CreatedAt = KpiProductGrouping_KpiProductGroupingFilterDTO.CreatedAt;
            KpiProductGroupingFilter.UpdatedAt = KpiProductGrouping_KpiProductGroupingFilterDTO.UpdatedAt;
            return KpiProductGroupingFilter;
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
    }
}

