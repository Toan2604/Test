using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MDistrict;
using DMS.Services.MInventory;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProvince;
using DMS.Services.MStatus;
using DMS.Services.MUnitOfMeasure;
using DMS.Services.MWard;
using DMS.Services.MWarehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

namespace DMS.Rpc.warehouse_report
{
    public class WarehouseReportController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IStatusService StatusService;
        private IInventoryService InventoryService;
        private IWarehouseService WarehouseService;
        private IItemService ItemService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;

        public WarehouseReportController(
            DataContext DataContext,
            DWContext DWContext,
            IStatusService StatusService,
            IInventoryService InventoryService,
            IWarehouseService WarehouseService,
            IItemService ItemService,
            IOrganizationService OrganizationService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.StatusService = StatusService;
            this.InventoryService = InventoryService;
            this.WarehouseService = WarehouseService;
            this.ItemService = ItemService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WarehouseReportRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WarehouseReport_WarehouseReportFilterDTO WarehouseReport_WarehouseReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? OrganizationId = WarehouseReport_WarehouseReportFilterDTO.OrganizationId?.Equal;
            long? ItemStatusId = WarehouseReport_WarehouseReportFilterDTO.ItemStatusId?.Equal;
            long? WarehouseId = WarehouseReport_WarehouseReportFilterDTO.WarehouseId?.Equal;

            List<UnitOfMeasure> UnitOfMeasures = await DWContext.Dim_UnitOfMeasure.Select(x => new UnitOfMeasure
            {
                Id = x.UnitOfMeasureId,
                Code = x.Code,
                Name = x.Name
            }).ToListWithNoLockAsync();

            var Item_query = DWContext.Dim_Item.AsNoTracking();
            Item_query = Item_query.Where(x => x.Code, WarehouseReport_WarehouseReportFilterDTO.ItemCode);
            Item_query = Item_query.Where(x => x.Name, WarehouseReport_WarehouseReportFilterDTO.ItemName);
            Item_query = Item_query.Where(x => x.ERPCode, WarehouseReport_WarehouseReportFilterDTO.ERPCode);
            Item_query = Item_query.Where(x => x.StatusId, WarehouseReport_WarehouseReportFilterDTO.ItemStatusId);

            List<Item> Items = await Item_query.Select(x => new Item
            {
                Id = x.ItemId,
                Code = x.Code,
                Name = x.Name,
                Product = new Product
                {
                    Id = x.ProductId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                }
            }).ToListWithNoLockAsync();
            foreach (var item in Items)
            {
                item.Product.UnitOfMeasure = UnitOfMeasures.Where(x => x.Id == item.Product.UnitOfMeasureId).FirstOrDefault();
            }
            List<long> ItemIds = Items.Select(x => x.Id).ToList();

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            if (OrganizationId.HasValue)
                OrganizationIds = OrganizationIds.Intersect(new List<long> { WarehouseReport_WarehouseReportFilterDTO.OrganizationId.Equal.Value }).ToList();

            List<long> WarehouseIds = await DataContext.WarehouseOrganizationMapping.AsNoTracking()
                .Where(x => x.WarehouseId, new IdFilter { Equal = WarehouseId })
                .Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds })
                .Select(x => x.WarehouseId).ToListWithNoLockAsync();

            var Warehouse_query = DWContext.Dim_Warehouse.AsNoTracking();
            Warehouse_query = Warehouse_query.Where(x => x.DeletedAt == null);
            Warehouse_query = Warehouse_query.Where(x => x.WarehouseId, new IdFilter { In = WarehouseIds });
            Warehouse_query = Warehouse_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });

            var Warehouses = await Warehouse_query.OrderBy(x => x.CreatedAt).ToListWithNoLockAsync();
            WarehouseIds = Warehouses.Select(x => x.WarehouseId).ToList();

            var Inventory_query = DWContext.Fact_Inventory.AsNoTracking();
            Inventory_query = Inventory_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            Inventory_query = Inventory_query.Where(x => x.WarehouseId, new IdFilter { In = WarehouseIds });

            var Inventories = await Inventory_query.ToListWithNoLockAsync();

            ItemIds = Inventories.Select(x => x.ItemId).Distinct().ToList();
            Items = Items.Where(x => ItemIds.Contains(x.Id)).Distinct().ToList();
            return Items.Count();
        }

        [Route(WarehouseReportRoute.List), HttpPost]
        public async Task<List<WarehouseReport_WarehouseReportDTO>> List([FromBody] WarehouseReport_WarehouseReportFilterDTO WarehouseReport_WarehouseReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? OrganizationId = WarehouseReport_WarehouseReportFilterDTO.OrganizationId?.Equal;
            long? ItemStatusId = WarehouseReport_WarehouseReportFilterDTO.ItemStatusId?.Equal;
            long? WarehouseId = WarehouseReport_WarehouseReportFilterDTO.WarehouseId?.Equal;

            List<UnitOfMeasure> UnitOfMeasures = await DWContext.Dim_UnitOfMeasure.Select(x => new UnitOfMeasure
            {
                Id = x.UnitOfMeasureId,
                Code = x.Code,
                Name = x.Name
            }).ToListWithNoLockAsync();

            var Item_query = DWContext.Dim_Item.AsNoTracking();
            Item_query = Item_query.Where(x => x.Code, WarehouseReport_WarehouseReportFilterDTO.ItemCode);
            Item_query = Item_query.Where(x => x.Name, WarehouseReport_WarehouseReportFilterDTO.ItemName);
            Item_query = Item_query.Where(x => x.ERPCode, WarehouseReport_WarehouseReportFilterDTO.ERPCode);
            Item_query = Item_query.Where(x => x.StatusId, WarehouseReport_WarehouseReportFilterDTO.ItemStatusId);

            List<Item> Items = await Item_query.Select(x => new Item
            {
                Id = x.ItemId,
                Code = x.Code,
                Name = x.Name,
                Product = new Product
                {
                    Id = x.ProductId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                }
            }).ToListWithNoLockAsync();
            foreach (var item in Items)
            {
                item.Product.UnitOfMeasure = UnitOfMeasures.Where(x => x.Id == item.Product.UnitOfMeasureId).FirstOrDefault();
            }
            List<long> ItemIds = Items.Select(x => x.Id).ToList();

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            if (OrganizationId.HasValue)
                OrganizationIds = OrganizationIds.Intersect(new List<long> { WarehouseReport_WarehouseReportFilterDTO.OrganizationId.Equal.Value }).ToList();

            List<long> WarehouseIds = await DataContext.WarehouseOrganizationMapping.AsNoTracking()
                .Where(x => x.WarehouseId, new IdFilter { Equal = WarehouseId })
                .Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds })
                .Select(x => x.WarehouseId).ToListWithNoLockAsync();

            var Warehouse_query = DWContext.Dim_Warehouse.AsNoTracking();
            Warehouse_query = Warehouse_query.Where(x => x.DeletedAt == null);
            Warehouse_query = Warehouse_query.Where(x => x.WarehouseId, new IdFilter { In = WarehouseIds });
            Warehouse_query = Warehouse_query.Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id });

            var Warehouses = await Warehouse_query.OrderBy(x => x.CreatedAt).ToListWithNoLockAsync();
            WarehouseIds = Warehouses.Select(x => x.WarehouseId).ToList();

            var Inventory_query = DWContext.Fact_Inventory.AsNoTracking();
            Inventory_query = Inventory_query.Where(x => x.ItemId, new IdFilter { In = ItemIds});
            Inventory_query = Inventory_query.Where(x => x.WarehouseId, new IdFilter { In = WarehouseIds});

            var Inventories = await Inventory_query.ToListWithNoLockAsync();

            ItemIds = Inventories.Select(x => x.ItemId).Distinct().ToList();
            Items = Items.Where(x => ItemIds.Contains(x.Id)).Distinct().ToList();
            Items = Items.OrderBy(x => x.Id).ToList()
                .Skip(WarehouseReport_WarehouseReportFilterDTO.Skip).Take(WarehouseReport_WarehouseReportFilterDTO.Take).ToList();

            List<WarehouseReport_WarehouseReportDTO> WarehouseReport_WarehouseReportDTOs = new List<WarehouseReport_WarehouseReportDTO>();

            foreach (var item in Items)
            {
                WarehouseReport_WarehouseReportDTO WarehouseReport_WarehouseReportDTO = new WarehouseReport_WarehouseReportDTO();
                WarehouseReport_WarehouseReportDTOs.Add(WarehouseReport_WarehouseReportDTO);
                var SubInventory = Inventories.Where(x => x.ItemId == item.Id).ToList();

                WarehouseReport_WarehouseReportDTO.ItemId = item.Id;
                WarehouseReport_WarehouseReportDTO.Item = new WarehouseReport_ItemDTO() { Name = item.Name, Code = item.Code };
                WarehouseReport_WarehouseReportDTO.UnitOfMeasureId = item.Product.UnitOfMeasureId;
                WarehouseReport_WarehouseReportDTO.TotalSaleStock = SubInventory.Sum(x => x.SaleStock);
                WarehouseReport_WarehouseReportDTO.UnitOfMeasure = new WarehouseReport_UnitOfMeasureDTO(item.Product.UnitOfMeasure);

                WarehouseReport_WarehouseReportDTO.Warehouses = new List<WarehouseReport_WarehouseReportWarehouseDTO>();
                foreach (var warehouse in Warehouses)
                {
                    WarehouseReport_WarehouseReportWarehouseDTO WarehouseReport_WarehouseReportWarehouseDTO = new WarehouseReport_WarehouseReportWarehouseDTO();
                    WarehouseReport_WarehouseReportDTO.Warehouses.Add(WarehouseReport_WarehouseReportWarehouseDTO);

                    WarehouseReport_WarehouseReportWarehouseDTO.WarehouseId = warehouse.WarehouseId;
                    WarehouseReport_WarehouseReportWarehouseDTO.WarehouseName = warehouse.Name;
                    WarehouseReport_WarehouseReportWarehouseDTO.TotalSaleStock = SubInventory
                        .Where(x => x.WarehouseId == warehouse.WarehouseId).Sum(x => x.SaleStock);
                }

            }

            return WarehouseReport_WarehouseReportDTOs;
        }

        [Route(WarehouseReportRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] WarehouseReport_WarehouseReportFilterDTO WarehouseReport_WarehouseReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            WarehouseReport_WarehouseReportFilterDTO.Skip = 0;
            WarehouseReport_WarehouseReportFilterDTO.Take = int.MaxValue;
            List<WarehouseReport_WarehouseReportDTO> WarehouseReport_WarehouseReportDTOs = await List(WarehouseReport_WarehouseReportFilterDTO);
            //List<WarehouseReport_ExportDTO> WarehouseReport_ExportDTOs = WarehouseReport_WarehouseReportDTOs.Select(x => new WarehouseReport_ExportDTO(x)).ToList();
            List<WarehouseReport_ExportDTO> WarehouseReport_ExportDTOs = new List<WarehouseReport_ExportDTO>();

            long stt = 1;
            foreach (var report in WarehouseReport_WarehouseReportDTOs)
            {
                WarehouseReport_ExportDTO WarehouseReport_ExportDTO = new WarehouseReport_ExportDTO(report);
                WarehouseReport_ExportDTO.STT = stt++;
                WarehouseReport_ExportDTOs.Add(WarehouseReport_ExportDTO);
                var Warehouse = report.Warehouses.OrderBy(x => x.WarehouseId).ToList();
                WarehouseReport_ExportDTO.TotalSaleStocks = new List<List<decimal>>() { Warehouse.Select(x => x.TotalSaleStock).ToList() };
            }

            List<List<string>> Header = new List<List<string>>();
            Header.Add(WarehouseReport_WarehouseReportDTOs[0].Warehouses.OrderBy(x => x.WarehouseId)
                .Select(x => x.WarehouseName).ToList());

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region header
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Báo cáo tồn kho có thể bán");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = OrgRoot.Name.ToUpper();
                ws.Cells["A1"].Style.Font.Size = 11;
                ws.Cells["A1"].Style.Font.Bold = true;

                ws.Cells["A3"].Value = "BÁO CÁO TỒN KHO CÓ THỂ BÁN";
                ws.Cells["A3:E3"].Merge = true;
                ws.Cells["A3:E3"].Style.Font.Size = 20;
                ws.Cells["A3:E3"].Style.Font.Bold = true;
                ws.Cells["A3:E3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["A4"].Value = $"Thời gian báo cáo: {StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).ToString("dd/MM/yyyy, HH:mm:ss")}";
                ws.Cells["A4:E4"].Merge = true;
                ws.Cells["A4:E4"].Style.Font.Bold = true;
                ws.Cells["A4:E4"].Style.Font.Italic = true;
                ws.Cells["A4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var header = new List<string>
                {
                    "STT",
                    "Mã sản phẩm",
                    "Tên sản phẩm",
                    "Đơn vị tính",
                    "Tổng tồn"
                };
                header.AddRange(Header[0]);
                string endColumn = ConvertColumnNameExcel(header.Count);
                string headerRange = $"A7:" + $"{endColumn}7";

                ws.Cells[headerRange].LoadFromArrays(new List<string[]> { header.ToArray() });
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));

                #endregion

                #region Fill data
                int StartRow = 8;
                int EndRow = StartRow - 1;
                int StartColumn = 1;
                int EndColumn = 0;
                if (WarehouseReport_ExportDTOs != null && WarehouseReport_ExportDTOs.Count > 0)
                {
                    List<object[]> Data = new List<object[]>();
                    foreach (var row in WarehouseReport_ExportDTOs)
                    {
                        Data.Add(new object[]
                        {
                            row.STT,
                            row.ItemCode,
                            row.ItemName,
                            row.UnitOfMeasureCode,
                            row.TotalSaleStock,
                        });
                        EndRow++;
                    }
                    EndColumn += 5;
                    ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(Data);
                    StartColumn = EndColumn + 1;

                    // Total sale stock
                    for (int i = 0; i < Header[0].Count; i++)
                    {
                        EndRow = StartRow;
                        Data = new List<object[]>();
                        foreach (var row in WarehouseReport_ExportDTOs)
                        {
                            Data.Add(new object[]
                            {
                                row.TotalSaleStocks[0][i]
                            });
                            EndRow++;
                        };

                        EndColumn += 1;
                        ws.Cells[$"{ConvertColumnNameExcel(StartColumn)}{StartRow}:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].LoadFromArrays(Data);
                        StartColumn = EndColumn + 1;
                    }
                }
                #endregion
                // All borders
                ws.Cells[$"A7:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                for (int i = 2; i <= header.Count; i++)
                {
                ws.Column(i).Width = 30;
                }

                ws.Cells[$"E8:{ConvertColumnNameExcel(EndColumn)}{EndRow}"].Style.Numberformat.Format = "#,##0"; // format number column value

                excel.Save();
            }
            return File(output.ToArray(), "application/octet-stream", "Warehouse_Report.xlsx");

        }
        public async Task<ActionResult> Export1([FromBody] WarehouseReport_WarehouseReportFilterDTO WarehouseReport_WarehouseReportFilterDTO)
        {// Phương pháp này bị giới hạn 12 cột * 1490 dòng. nếu hơn thì bị lỗi file
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            WarehouseReport_WarehouseReportFilterDTO.Skip = 0;
            WarehouseReport_WarehouseReportFilterDTO.Take = int.MaxValue;
            List<WarehouseReport_WarehouseReportDTO> WarehouseReport_WarehouseReportDTOs = await List(WarehouseReport_WarehouseReportFilterDTO);
            //List<WarehouseReport_ExportDTO> WarehouseReport_ExportDTOs = WarehouseReport_WarehouseReportDTOs.Select(x => new WarehouseReport_ExportDTO(x)).ToList();
            List<WarehouseReport_ExportDTO> WarehouseReport_ExportDTOs = new List<WarehouseReport_ExportDTO>();

            long stt = 1;
            foreach (var report in WarehouseReport_WarehouseReportDTOs)
            {
                WarehouseReport_ExportDTO WarehouseReport_ExportDTO = new WarehouseReport_ExportDTO(report);
                WarehouseReport_ExportDTO.STT = stt++;

                WarehouseReport_ExportDTOs.Add(WarehouseReport_ExportDTO);
                var Warehouse = report.Warehouses.OrderBy(x => x.WarehouseId).ToList();
                WarehouseReport_ExportDTO.TotalSaleStocks = new List<List<decimal>>() { Warehouse.Select(x => x.TotalSaleStock).ToList() };
                if (stt == 1489)
                    break;
            }

            List<List<string>> Header = new List<List<string>>();
            Header.Add(WarehouseReport_WarehouseReportDTOs[0].Warehouses.OrderBy(x => x.WarehouseId)
                .Select(x => x.WarehouseName).ToList());

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/Warehouse_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Data = WarehouseReport_ExportDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            Data.Time = StaticParams.DateTimeNow.ToString("dd/MM/yyyy, HH:mm:ss");
            Data.Header = Header;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "Warehouse_Report.xlsx");

        }

        [Route(WarehouseReportRoute.FilterListWarehouse), HttpPost]
        public async Task<List<WarehouseReport_WarehouseDTO>> FilterListWarehouse([FromBody] WarehouseReport_WarehouseFilterDTO Warehouse_WarehouseFilterDTO)
        {
            WarehouseFilter WarehouseFilter = new WarehouseFilter();
            WarehouseFilter.Skip = 0;
            WarehouseFilter.Take = int.MaxValue;
            WarehouseFilter.OrderBy = WarehouseOrder.Id;
            WarehouseFilter.OrderType = OrderType.ASC;
            WarehouseFilter.Selects = WarehouseSelect.ALL;
            WarehouseFilter.Id = Warehouse_WarehouseFilterDTO.Id;
            WarehouseFilter.Code = Warehouse_WarehouseFilterDTO.Code;
            WarehouseFilter.Name = Warehouse_WarehouseFilterDTO.Name;
            WarehouseFilter.ProvinceId = Warehouse_WarehouseFilterDTO.ProvinceId;
            WarehouseFilter.DistrictId = Warehouse_WarehouseFilterDTO.DistrictId;
            WarehouseFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            if (Warehouse_WarehouseFilterDTO.OrganizationId != null && Warehouse_WarehouseFilterDTO.OrganizationId.HasValue)
                OrganizationIds = OrganizationIds.Intersect(new List<long> { Warehouse_WarehouseFilterDTO.OrganizationId.Equal.Value }).ToList();

            WarehouseFilter.OrganizationId = new IdFilter { In = OrganizationIds };

            List<Warehouse> Warehouses = await WarehouseService.List(WarehouseFilter);
            List<WarehouseReport_WarehouseDTO> Warehouse_WarehouseDTOs = Warehouses
                .Select(x => new WarehouseReport_WarehouseDTO(x)).ToList();
            return Warehouse_WarehouseDTOs;
        }

        [Route(WarehouseReportRoute.FilterListOrganization), HttpPost]
        public async Task<List<WarehouseReport_OrganizationDTO>> FilterListOrganization([FromBody] WarehouseReport_OrganizationFilterDTO Warehouse_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Code = Warehouse_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Warehouse_OrganizationFilterDTO.Name;
            OrganizationFilter.Path = Warehouse_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Warehouse_OrganizationFilterDTO.Level;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<WarehouseReport_OrganizationDTO> Warehouse_OrganizationDTOs = Organizations
                .Select(x => new WarehouseReport_OrganizationDTO(x)).ToList();
            return Warehouse_OrganizationDTOs;
        }

        [Route(WarehouseReportRoute.FilterListStatus), HttpPost]
        public async Task<List<WarehouseReport_StatusDTO>> FilterListStatus([FromBody] WarehouseReport_StatusFilterDTO WarehouseReport_StatusFilterDTO)
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
            List<WarehouseReport_StatusDTO> WarehouseReport_StatusDTOs = Statuses
                .Select(x => new WarehouseReport_StatusDTO(x)).ToList();
            return WarehouseReport_StatusDTOs;
        }

        private string ConvertColumnNameExcel(int ColumnNumber)
        {
            string ColumnString = Char.ConvertFromUtf32(ColumnNumber + 64);
            if (ColumnNumber > 26) ColumnString = Char.ConvertFromUtf32(ColumnNumber / 26 + 64) + Char.ConvertFromUtf32(ColumnNumber % 26 + 64);
            return ColumnString;
        }
    }
}
