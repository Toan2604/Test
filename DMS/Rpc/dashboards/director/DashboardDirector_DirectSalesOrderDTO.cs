using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_DirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public long? RequestStateId { get; set; }
        public long? GeneralApprovalStateId { get; set; }
        public long SaleEmployeeId { get; set; }
        public decimal Total { get; set; }
        public DashboardDirector_AppUserDTO SaleEmployee { get; set; }
        public DashboardDirector_RequestStateDTO RequestState { get; set; }
        public DashboardDirector_GeneralApprovalStateDTO GeneralApprovalState { get; set; }

        public DashboardDirector_DirectSalesOrderDTO() { }
        public DashboardDirector_DirectSalesOrderDTO(DirectSalesOrder DirectSalesOrder)
        {
            this.Id = DirectSalesOrder.Id;
            this.Code = DirectSalesOrder.Code;
            this.OrderDate = DirectSalesOrder.OrderDate;
            this.RequestStateId = DirectSalesOrder.RequestStateId;
            this.GeneralApprovalStateId = DirectSalesOrder.GeneralApprovalStateId;
            this.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
            this.Total = DirectSalesOrder.Total;
            this.SaleEmployee = DirectSalesOrder.SaleEmployee == null ? null : new DashboardDirector_AppUserDTO(DirectSalesOrder.SaleEmployee);
            this.RequestState = DirectSalesOrder.RequestState == null ? null : new DashboardDirector_RequestStateDTO(DirectSalesOrder.RequestState);
            this.GeneralApprovalState = DirectSalesOrder.GeneralApprovalState == null ? null : new DashboardDirector_GeneralApprovalStateDTO(DirectSalesOrder.GeneralApprovalState);
        }
    }
}
