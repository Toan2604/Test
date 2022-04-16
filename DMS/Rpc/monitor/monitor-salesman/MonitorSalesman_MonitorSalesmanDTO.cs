using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_salesman
{
    public class MonitorSalesman_MonitorSalesmanDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<MonitorSalesman_SaleEmployeeDTO> SaleEmployees { get; set; }
    }


    public class MonitorSalesman_SaleEmployeeDTO : DataDTO
    {
        public long STT { get; set; }
        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public long PlanCounter { get; set; }
        public long Unchecking { get; set; }
        public long InternalCounter { get { return Internal == null ? 0 : Internal.Count; } }
        public long ExternalCounter { get { return External == null ? 0 : External.Count; } }
        public long ImageCounter { get; set; }
        public long SalesOrderCounter { get; set; }
        public decimal Revenue { get; set; }
        internal HashSet<long> Internal { get; set; }
        internal HashSet<long> External { get; set; }
        public List<MonitorSalesman_StoreCheckingDTO> StoreCheckings { get; set; }
    }

    public class MonitorSalesman_StoreCheckingDTO : DataDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public MonitorSalesman_IndirectSalesOrderDTO IndirectSalesOrder { get; set; }
        public MonitorSalesman_ProblemDTO Problem { get; set; }
        public string Image { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
    public class MonitorSalesman_IndirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }

    }

    public class MonitorSalesman_ProblemDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
    }


    public class MonitorSalesman_MonitorSalesmanFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CheckIn { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public List<MonitorSalesman_MonitorSalesmanFilterDTO> OrFilters { get; set; }
    }
}
