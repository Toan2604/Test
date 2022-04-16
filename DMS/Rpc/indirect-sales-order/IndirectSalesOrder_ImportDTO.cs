using DMS.Entities;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_ImportDTO : DataDTO
    {
        public int RowNumber { get; set; }
        public long Stt { get; set; }
        public long IndirectSalesOrderNumber { get; set; }
        public string SaleEmployeeCode { get; set; }
        public string SaleEmployeeName { get; set; }
        public string BuyerStoreCode { get; set; }
        public string BuyerStoreName { get; set; }
        public string SellerStoreCode { get; set; }
        public string SellerStoreName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Note { get; set; }
        public long EditedPriceStatusId { get; set; }
        public decimal GeneralDiscountPercentage { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public bool IsPromotion { get; set; }
        public long Quantity { get; set; }
        public decimal SalePrice { get; set; }
    }
}
