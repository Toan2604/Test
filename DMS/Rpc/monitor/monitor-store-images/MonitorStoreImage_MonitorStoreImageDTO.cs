using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_images
{
    public class MonitorStoreImage_MonitorStoreImageDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<MonitorStoreImage_SaleEmployeeDTO> SaleEmployees { get; set; }
    }

    public class MonitorStoreImage_SaleEmployeeDTO : DataDTO
    {

        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<MonitorStoreImage_DetailDTO> StoreCheckings { get; set; }
    }

    public class MonitorStoreImage_DetailDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string DateDisplay => Date.ToString("dd-MM-yyyy");
        public long StoreId { get; set; }
        public string StoreName { get; set; }
        public long ImageCounter { get; set; }
        public long SaleEmployeeId { get; set; }
    }

    public class MonitorStoreImage_MonitorStoreImageFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CheckIn { get; set; }
        public IdFilter StoreId { get; set; }
        public List<MonitorStoreImage_MonitorStoreImageFilterDTO> OrFilters { get; set; }
    }
}
