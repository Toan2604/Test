using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequest_ERouteChangeRequestContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long ERouteChangeRequestId { get; set; }
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
        public ERouteChangeRequest_StoreDTO Store { get; set; }

        public ERouteChangeRequest_ERouteChangeRequestContentDTO() { }
        public ERouteChangeRequest_ERouteChangeRequestContentDTO(ERouteChangeRequestContent ERouteChangeRequestContent)
        {
            this.Id = ERouteChangeRequestContent.Id;
            this.ERouteChangeRequestId = ERouteChangeRequestContent.ERouteChangeRequestId;
            this.StoreId = ERouteChangeRequestContent.StoreId;
            this.OrderNumber = ERouteChangeRequestContent.OrderNumber;
            this.Monday = ERouteChangeRequestContent.Monday;
            this.Tuesday = ERouteChangeRequestContent.Tuesday;
            this.Wednesday = ERouteChangeRequestContent.Wednesday;
            this.Thursday = ERouteChangeRequestContent.Thursday;
            this.Friday = ERouteChangeRequestContent.Friday;
            this.Saturday = ERouteChangeRequestContent.Saturday;
            this.Sunday = ERouteChangeRequestContent.Sunday;
            this.Week1 = ERouteChangeRequestContent.Week1;
            this.Week2 = ERouteChangeRequestContent.Week2;
            this.Week3 = ERouteChangeRequestContent.Week3;
            this.Week4 = ERouteChangeRequestContent.Week4;
            this.Store = ERouteChangeRequestContent.Store == null ? null : new ERouteChangeRequest_StoreDTO(ERouteChangeRequestContent.Store);
            this.Errors = ERouteChangeRequestContent.Errors;
        }
    }

    public class ERouteChangeRequest_ERouteChangeRequestContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter ERouteChangeRequestId { get; set; }

        public IdFilter StoreId { get; set; }

        public LongFilter OrderNumber { get; set; }

        public ERouteChangeRequestContentOrder OrderBy { get; set; }
    }
}