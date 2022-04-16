using System;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_statistic_store_scouting
{
    public class ReportStatisticStoreScouting_TotalDTO : DataDTO
    {
        public string OfficalName { get; set; }
        public long StoreScoutingCounter { get; set; }
        public long StoreOpennedCounter { get; set; }
        public long StoreScoutingUnOpen => StoreScoutingCounter - StoreOpennedCounter;
        public long StoreCounter { get; set; }
        public decimal StoreCoutingOpennedRate => StoreScoutingCounter == 0 ? 0 : Math.Round((((decimal)StoreOpennedCounter / StoreScoutingCounter) * 100), 2);
        public string eStoreCoutingOpennedRate => $"{Math.Round(StoreCoutingOpennedRate, 0)}%";
        public decimal StoreCoutingRate => (StoreScoutingUnOpen + StoreCounter) == 0 ? 0 : Math.Round((((decimal)StoreScoutingUnOpen / (StoreScoutingUnOpen + StoreCounter)) * 100), 2);
        public string eStoreCoutingRate => $"{Math.Round(StoreCoutingRate, 0)}%";
        public decimal StoreRate => 100 - StoreCoutingRate;
        public string eStoreRate => $"{Math.Round(StoreRate, 0)}%";
    }

}
