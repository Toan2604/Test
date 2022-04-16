using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public class DashboardUser_IndirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public long BuyerStoreId { get; set; }
        public long SellerStoreId { get; set; }
        public long? RequestStateId { get; set; }
        public long SaleEmployeeId { get; set; }
        public decimal Total { get; set; }
        public DashboardUser_AppUserDTO SaleEmployee { get; set; }
        public DashboardUser_RequestStateDTO RequestState { get; set; }
        public DashboardUser_StoreDTO BuyerStore { get; set; }
        public DashboardUser_StoreDTO SellerStore { get; set; }

        public DashboardUser_IndirectSalesOrderDTO() { }
        public DashboardUser_IndirectSalesOrderDTO(IndirectSalesOrder IndirectSalesOrder)
        {
            this.Id = IndirectSalesOrder.Id;
            this.Code = IndirectSalesOrder.Code;
            this.OrderDate = IndirectSalesOrder.OrderDate;
            this.RequestStateId = IndirectSalesOrder.RequestStateId;
            this.SaleEmployeeId = IndirectSalesOrder.SaleEmployeeId;
            this.BuyerStoreId = IndirectSalesOrder.BuyerStoreId;
            this.SellerStoreId = IndirectSalesOrder.SellerStoreId;
            this.Total = IndirectSalesOrder.Total;
            this.SaleEmployee = IndirectSalesOrder.SaleEmployee == null ? null : new DashboardUser_AppUserDTO(IndirectSalesOrder.SaleEmployee);
            this.RequestState = IndirectSalesOrder.RequestState == null ? null : new DashboardUser_RequestStateDTO(IndirectSalesOrder.RequestState);
            this.BuyerStore = IndirectSalesOrder.BuyerStore == null ? null : new DashboardUser_StoreDTO(IndirectSalesOrder.BuyerStore);
            this.SellerStore = IndirectSalesOrder.SellerStore == null ? null : new DashboardUser_StoreDTO(IndirectSalesOrder.SellerStore);
        }
    }

    public class DashboardUser_DashboardUserFilterDTO : FilterDTO
    {
        public IdFilter Time { get; set; }
    }
}
