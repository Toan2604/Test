using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public class DashboardUser_DirectSalesOrderDTO : DataDTO
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
        //public DashboardUser_StoreDTO SellerStore { get; set; }

        public DashboardUser_DirectSalesOrderDTO() { }
        public DashboardUser_DirectSalesOrderDTO(DirectSalesOrder DirectSalesOrder)
        {
            this.Id = DirectSalesOrder.Id;
            this.Code = DirectSalesOrder.Code;
            this.OrderDate = DirectSalesOrder.OrderDate;
            this.RequestStateId = DirectSalesOrder.RequestStateId;
            this.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
            this.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
            //this.SellerStoreId = DirectSalesOrder.SellerStoreId;
            this.Total = DirectSalesOrder.Total;
            this.SaleEmployee = DirectSalesOrder.SaleEmployee == null ? null : new DashboardUser_AppUserDTO(DirectSalesOrder.SaleEmployee);
            this.RequestState = DirectSalesOrder.RequestState == null ? null : new DashboardUser_RequestStateDTO(DirectSalesOrder.RequestState);
            this.BuyerStore = DirectSalesOrder.BuyerStore == null ? null : new DashboardUser_StoreDTO(DirectSalesOrder.BuyerStore);
            // this.SellerStore = DirectSalesOrder.SellerStore == null ? null : new DashboardUser_StoreDTO(DirectSalesOrder.SellerStore);
        }
    }

    //public class DashboardUser_DashboardUserFilterDTO : FilterDTO
    //{
    //    public IdFilter Time { get; set; }
    //}
}
