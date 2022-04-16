using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_statistic_store_scouting
{
    public class ReportStatisticStoreScouting_ReportStatisticStoreScoutingDTO : DataDTO
    {
        public long STT { get; set; }
        public string OfficalName { get; set; }
        internal long? ProvinceId { get; set; }
        internal long? DistrictId { get; set; }
        internal long? WardId { get; set; }
        public long StoreScoutingCounter => StoreScoutingIds == null ? 0 : StoreScoutingIds.Count();
        internal List<long> StoreScoutingIds { get; set; }
        public long StoreOpennedCounter => StoreOpennedIds == null ? 0 : StoreOpennedIds.Count();
        internal List<long> StoreOpennedIds { get; set; }
        public long StoreScoutingUnOpen => StoreScoutingCounter - StoreOpennedCounter;
        public long StoreCounter => StoreIds == null ? 0 : StoreIds.Count();
        internal List<long> StoreIds { get; set; }
        public decimal StoreCoutingOpennedRate => StoreScoutingCounter == 0 ? 0 : Math.Round((((decimal)StoreOpennedCounter / StoreScoutingCounter) * 100), 2);
        public string eStoreCoutingOpennedRate => $"{Math.Round(StoreCoutingOpennedRate, 0)}%";
        public decimal StoreCoutingRate => (StoreScoutingUnOpen + StoreCounter) == 0 ? 0 : Math.Round((((decimal)StoreScoutingUnOpen / (StoreScoutingUnOpen + StoreCounter)) * 100), 2);
        public string eStoreCoutingRate => $"{Math.Round(StoreCoutingRate, 0)}%";
        public decimal StoreRate => 100 - StoreCoutingRate;
        public string eStoreRate => $"{Math.Round(StoreRate, 0)}%";
    }

    public class ReportStatisticStoreScouting_ReportStatisticStoreScoutingFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public DateFilter Date { get; set; }
        internal bool HasValue => (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue) ||
            (WardId != null && WardId.HasValue);
    }
}
