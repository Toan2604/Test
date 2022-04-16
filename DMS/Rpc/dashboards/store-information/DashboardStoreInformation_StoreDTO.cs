using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    public class DashboardStoreInformation_StoreDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Top1BrandName { get; set; }
        public string Telephone { get; set; }
        public bool IsScouting { get; set; }
        public long StoreStatusId { get; set; }
        public long OrganizationId { get; set; }
        public long ProductGroupingCounter { get; set; }
        public string Rate8 { get { return ProductGroupingCounter >= 8 ? "x" : ""; } }
        public string Rate7 { get { return ProductGroupingCounter >= 7 ? "x" : ""; } }
        public string Rate6 { get { return ProductGroupingCounter >= 6 ? "x" : ""; } }
        public string Rate5 { get { return ProductGroupingCounter >= 5 ? "x" : ""; } }
        public string Rate4 { get { return ProductGroupingCounter >= 4 ? "x" : ""; } }
        public string Rate3 { get { return ProductGroupingCounter >= 3 ? "x" : ""; } }
        public Dictionary<long, string> ProductGroupings { get; set; }
        public DashboardStoreInformation_StoreDTO() { }
        public DashboardStoreInformation_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Longitude = Store.Longitude;
            this.Latitude = Store.Latitude;
            this.Name = Store.Name;
            this.Address = Store.Address;
            this.StoreStatusId = Store.StoreStatusId;
            this.OrganizationId = Store.OrganizationId;
            this.Telephone = Store.Telephone;
        }
    }

    public class DashboardStoreInformation_StoreExport_ProductGroupingDTO : DataDTO
    {
        public long ProductGroupingId { get; set; }
        public string InStore { get; set; }
    }

    public class DashboardStoreInformation_StoreExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<DashboardStoreInformation_StoreDTO> Stores { get; set; }
    }

    public class DashboardStoreInformation_StoreFilterDTO : DashboardStoreInformation_FilterDTO
    {
    }
}
