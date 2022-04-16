using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreProfileExportDTO : DataDTO
    {
        public string STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public string StoreType { get; set; }
        public string ParentStore { get; set; }
        public string Description { get; set; }
        public List<StoreExport_ProductGroupingDTO> ProductGroupings { get; set; }
        public string ListProductGroupingName { get { return string.Join("; ", ProductGroupings.Where(x => x.Value == "x").Select(x => x.ProductGroupingName).ToArray()); } }
        public long ProductGroupingsCounter { get { return ProductGroupings.Where(x => x.Value == "x").Count(); } }
        public string Top1Brand { get; set; }
        public long CheckingCounterInMonth { get; set; }
        public string EstimatedRevenue { get; set; }
        public decimal? MonthlyRevenuePlanned { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal? MonthlyRevenueRatio { get { return (MonthlyRevenuePlanned == 0 || MonthlyRevenuePlanned == null) ? null : (decimal?)Math.Round((MonthlyRevenue / MonthlyRevenuePlanned.Value) * 100, 1); } }
        public decimal? QuarterlyRevenuePlanned { get; set; }
        public decimal QuarterlyRevenue { get; set; }
        public decimal? QuarterlyRevenueRatio { get { return (QuarterlyRevenuePlanned == 0 || QuarterlyRevenuePlanned == null) ? null : (decimal?)Math.Round((QuarterlyRevenue / QuarterlyRevenuePlanned.Value) * 100, 1); } }
        public string Status { get; set; }
        public Store_StoreProfileExportDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.Name = Store.Name;
            this.Address = Store.Address;
            this.Province = Store.ProvinceId == null ? "" : Store.Province.Name;
            this.District = Store.DistrictId == null ? "" : Store.District.Name;
            this.Phone = Store.Telephone;
            this.StoreType = Store.StoreType.Name;
            this.ParentStore = Store.ParentStoreId == null ? "" : Store.ParentStore.Name;
            this.Description = Store.Description == null ? "" : Store.Description;
            this.EstimatedRevenue = Store.EstimatedRevenueId == null ? "" : Store.EstimatedRevenue.Name;
            this.Status = Store.StatusId == 1 ? "Hoạt động" : "Dừng hoạt động";
        }
    }

    public class StoreExport_ProductGroupingDTO : DataDTO
    {
        public long ProductGroupingId { get; set; }
        public string ProductGroupingName { get; set; }
        public string Value { get; set; }
    }
}
