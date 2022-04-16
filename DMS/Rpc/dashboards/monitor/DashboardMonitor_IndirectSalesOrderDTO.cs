using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitor_IndirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public long? RequestStateId { get; set; }
        public long SaleEmployeeId { get; set; }
        public decimal Total { get; set; }
        public DashboardMonitor_AppUserDTO SaleEmployee { get; set; }
        public DashboardMonitor_RequestStateDTO RequestState { get; set; }

        public DashboardMonitor_IndirectSalesOrderDTO() { }
        public DashboardMonitor_IndirectSalesOrderDTO(IndirectSalesOrder IndirectSalesOrder)
        {
            this.Id = IndirectSalesOrder.Id;
            this.Code = IndirectSalesOrder.Code;
            this.OrderDate = IndirectSalesOrder.OrderDate;
            this.RequestStateId = IndirectSalesOrder.RequestStateId;
            this.SaleEmployeeId = IndirectSalesOrder.SaleEmployeeId;
            this.Total = IndirectSalesOrder.Total;
            this.SaleEmployee = IndirectSalesOrder.SaleEmployee == null ? null : new DashboardMonitor_AppUserDTO(IndirectSalesOrder.SaleEmployee);
            this.RequestState = IndirectSalesOrder.RequestState == null ? null : new DashboardMonitor_RequestStateDTO(IndirectSalesOrder.RequestState);
        }
    }
}
