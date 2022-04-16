using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequest_ERouteContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long ERouteId { get; set; }
        public long StoreId { get; set; }
        public long? OrderNumber { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool Week1 { get; set; }
        public bool Week2 { get; set; }
        public bool Week3 { get; set; }
        public bool Week4 { get; set; }
        public ERouteChangeRequest_ERouteDTO ERoute { get; set; }
        public ERouteChangeRequest_StoreDTO Store { get; set; }
        public ERouteChangeRequest_ERouteContentDTO() { }
        public ERouteChangeRequest_ERouteContentDTO(ERouteContent ERouteContent)
        {
            this.Id = ERouteContent.Id;
            this.ERouteId = ERouteContent.ERouteId;
            this.StoreId = ERouteContent.StoreId;
            this.OrderNumber = ERouteContent.OrderNumber;
            this.Monday = ERouteContent.Monday;
            this.Tuesday = ERouteContent.Tuesday;
            this.Wednesday = ERouteContent.Wednesday;
            this.Thursday = ERouteContent.Thursday;
            this.Friday = ERouteContent.Friday;
            this.Saturday = ERouteContent.Saturday;
            this.Sunday = ERouteContent.Sunday;
            this.Week1 = ERouteContent.Week1;
            this.Week2 = ERouteContent.Week2;
            this.Week3 = ERouteContent.Week3;
            this.Week4 = ERouteContent.Week4;
            this.ERoute = ERouteContent.ERoute == null ? null : new ERouteChangeRequest_ERouteDTO(ERouteContent.ERoute);
            this.Store = ERouteContent.Store == null ? null : new ERouteChangeRequest_StoreDTO(ERouteContent.Store);
            this.Errors = ERouteContent.Errors;
        }
    }

    public class ERouteChangeRequest_ERouteContentFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ERouteId { get; set; }
        public IdFilter StoreId { get; set; }
        public LongFilter OrderNumber { get; set; }
        public ERouteContentOrder OrderBy { get; set; }
    }
}
