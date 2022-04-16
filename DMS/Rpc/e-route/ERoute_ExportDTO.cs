using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_ExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string StoreCode { get; set; }
        public string StoreCodeDraft { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreProvince { get; set; }
        public string StoreDistrict { get; set; }
        public string StoreWard { get; set; }
        public string StoreStatus { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public string Week1 { get; set; }
        public string Week2 { get; set; }
        public string Week3 { get; set; }
        public string Week4 { get; set; }
        public ERoute_ExportDTO() { }
        public ERoute_ExportDTO(ERouteContent ERouteContent)
        {
            this.StoreCode = ERouteContent.Store?.Code;
            this.StoreCodeDraft = ERouteContent.Store?.CodeDraft;
            this.StoreName = ERouteContent.Store?.Name;
            this.StoreAddress = ERouteContent.Store?.Address;
            this.StoreProvince = ERouteContent.Store?.Province?.Name;
            this.StoreDistrict = ERouteContent.Store?.District?.Name;
            this.StoreWard = ERouteContent.Store?.Ward?.Name;
            this.StoreStatus = ERouteContent.Store?.StoreStatus.Name;
            this.Monday = ERouteContent.Monday ? "x" : "";
            this.Tuesday = ERouteContent.Tuesday ? "x" : "";
            this.Wednesday = ERouteContent.Wednesday ? "x" : "";
            this.Thursday = ERouteContent.Thursday ? "x" : "";
            this.Friday = ERouteContent.Friday ? "x" : "";
            this.Saturday = ERouteContent.Saturday ? "x" : "";
            this.Sunday = ERouteContent.Sunday ? "x" : "";
            this.Week1 = ERouteContent.Week1 ? "x" : "";
            this.Week2 = ERouteContent.Week2 ? "x" : "";
            this.Week3 = ERouteContent.Week3 ? "x" : "";
            this.Week4 = ERouteContent.Week4 ? "x" : "";
            this.Errors = ERouteContent.Errors;
        }
    }
}
