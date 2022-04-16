using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_salesman
{
    public class MonitorSalesman_MonitorSalesmanDetailDTO : DataDTO
    {
        public long StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public long ImageCounter { get; set; }
        public List<MonitorSalesman_MonitorSalesmanDetailInfoDTO> Infoes { get; set; }
    }
    public class MonitorSalesman_MonitorSalesmanDetailInfoDTO : DataDTO
    {
        public string IndirectSalesOrderCode { get; set; }
        public decimal Sales { get; set; }
        public string ImagePath { get; set; }
        public string ProblemCode { get; set; }
        public long ProblemId { get; set; }
    }


    public class MonitorSalesman_MonitorSalesmanDetailFilterDTO : FilterDTO
    {
        public long SaleEmployeeId { get; set; }
        public long StoreId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public DateTime Date { get; set; }
    }
}
