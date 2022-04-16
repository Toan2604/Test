using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.warehouse_report
{
    public class WarehouseReport_WarehouseReportDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal TotalSaleStock { get; set; }

        public WarehouseReport_ItemDTO Item { get; set; }
        public WarehouseReport_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public List<WarehouseReport_WarehouseReportWarehouseDTO> Warehouses { get; set; }

    }

    public class WarehouseReport_WarehouseReportWarehouseDTO : DataDTO
    {
        public long WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public decimal TotalSaleStock { get; set; }

    }

    public class WarehouseReport_WarehouseReportFilterDTO : FilterDTO
    {
        public StringFilter ItemCode { get; set; }
        public StringFilter ItemName { get; set; }
        public StringFilter ERPCode { get; set; }
        public IdFilter ItemStatusId { get; set; }
        public IdFilter WarehouseId { get; set; }
        public IdFilter OrganizationId { get; set; }
    }
}
