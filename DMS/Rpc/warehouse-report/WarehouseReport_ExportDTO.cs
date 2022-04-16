using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.warehouse_report
{
    public class WarehouseReport_ExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public decimal TotalSaleStock { get; set; }
        public List<List<decimal>> TotalSaleStocks { get; set; }
        public WarehouseReport_ExportDTO( )
        { }
        public WarehouseReport_ExportDTO(WarehouseReport_WarehouseReportDTO WarehouseReport_WarehouseReportDTO)
        {
            this.ItemCode = WarehouseReport_WarehouseReportDTO.Item.Code;
            this.ItemName = WarehouseReport_WarehouseReportDTO.Item.Name;
            this.UnitOfMeasureCode = WarehouseReport_WarehouseReportDTO.UnitOfMeasure.Code;
            this.TotalSaleStock = WarehouseReport_WarehouseReportDTO.TotalSaleStock;
        }
    }
}
