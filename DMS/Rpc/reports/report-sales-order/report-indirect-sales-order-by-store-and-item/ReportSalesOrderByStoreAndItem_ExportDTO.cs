using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    public class ReportSalesOrderByStoreAndItem_ExportDTO : DataDTO
    {
        public string OrganizationName { get; set; }
        public List<ReportSalesOrderByStoreAndItem_ExportStoreDTO> Stores { get; set; }
        public ReportSalesOrderByStoreAndItem_ExportDTO(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO)
        {
            this.OrganizationName = ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO.OrganizationName;
            this.Stores = ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO.Stores?.Select(x => new ReportSalesOrderByStoreAndItem_ExportStoreDTO(x)).ToList();
        }
    }

    public class ReportSalesOrderByStoreAndItem_ExportStoreDTO : DataDTO
    {
        public long STT { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string StoreStatusName { get; set; }
        public string Address { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public List<ReportSalesOrderByStoreAndItem_ExportItemDTO> Items { get; set; }
        public ReportSalesOrderByStoreAndItem_ExportStoreDTO(ReportSalesOrderByStoreAndItem_StoreDTO ReportSalesOrderByStoreAndItem_StoreDTO)
        {
            this.Code = ReportSalesOrderByStoreAndItem_StoreDTO.Code;
            this.CodeDraft = ReportSalesOrderByStoreAndItem_StoreDTO.CodeDraft;
            this.Name = ReportSalesOrderByStoreAndItem_StoreDTO.Name;
            this.StoreStatusName = ReportSalesOrderByStoreAndItem_StoreDTO.StoreStatus.Name;
            this.Address = ReportSalesOrderByStoreAndItem_StoreDTO.Address;
            this.ProvinceName = ReportSalesOrderByStoreAndItem_StoreDTO.Province?.Name;
            this.DistrictName = ReportSalesOrderByStoreAndItem_StoreDTO.District?.Name;
            this.Items = ReportSalesOrderByStoreAndItem_StoreDTO.Items?.Select(x => new ReportSalesOrderByStoreAndItem_ExportItemDTO(x)).ToList();
        }
    }

    public class ReportSalesOrderByStoreAndItem_ExportItemDTO : DataDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal SaleStock { get; set; }
        public decimal PromotionStock { get; set; }
        public decimal SalePriceAverage { get; set; }
        public decimal Discount { get; set; }
        public decimal Revenue { get; set; }
        public long SalesOrderCounter { get; set; }
        public ReportSalesOrderByStoreAndItem_ExportItemDTO(ReportSalesOrderByStoreAndItem_ItemDTO ReportSalesOrderByStoreAndItem_ItemDTO)
        {
            this.Code = ReportSalesOrderByStoreAndItem_ItemDTO.Code;
            this.Name = ReportSalesOrderByStoreAndItem_ItemDTO.Name;
            this.UnitOfMeasureName = ReportSalesOrderByStoreAndItem_ItemDTO.UnitOfMeasureName;
            this.SaleStock = ReportSalesOrderByStoreAndItem_ItemDTO.SaleStock;
            this.PromotionStock = ReportSalesOrderByStoreAndItem_ItemDTO.PromotionStock;
            this.SalePriceAverage = ReportSalesOrderByStoreAndItem_ItemDTO.SalePriceAverage;
            this.Discount = ReportSalesOrderByStoreAndItem_ItemDTO.Discount;
            this.Revenue = ReportSalesOrderByStoreAndItem_ItemDTO.Revenue;
            this.SalesOrderCounter = ReportSalesOrderByStoreAndItem_ItemDTO.SalesOrderCounter;
        }
    }
}
