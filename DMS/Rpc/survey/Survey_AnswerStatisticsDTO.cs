using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.survey
{
    public class Survey_AnswerStatisticsDTO : DataDTO
    {
        public long TotalCounter { get; set; }
        public long StoreCounter { get; set; }
        public long StoreScoutingCounter { get; set; }
        public long OtherCounter { get; set; }
        public List<Survey_StoreResultStatisticsDTO> StoreResults { get; set; }
        public List<Survey_StoreScoutingResultStatisticsDTO> StoreScoutingResults { get; set; }
        public List<Survey_OtherStatisticsDTO> OtherResults { get; set; }

    }

    public class Survey_StoreResultStatisticsDTO : DataDTO
    {
        public long StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }

    public class Survey_StoreScoutingResultStatisticsDTO : DataDTO
    {
        public long StoreScoutingId { get; set; }
        public string StoreScoutingCode { get; set; }
        public string StoreScoutingName { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }

    public class Survey_OtherStatisticsDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
