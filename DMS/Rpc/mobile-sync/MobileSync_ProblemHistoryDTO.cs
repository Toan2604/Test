using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_ProblemHistoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long ProblemId { get; set; }
        public DateTime Time { get; set; }
        public long ModifierId { get; set; }
        public long ProblemStatusId { get; set; }
        public MobileSync_AppUserDTO Modifier { get; set; }
        public MobileSync_ProblemStatusDTO ProblemStatus { get; set; }
        public MobileSync_ProblemHistoryDTO() { }
        public MobileSync_ProblemHistoryDTO(ProblemHistory ProblemHistory)
        {
            this.Id = ProblemHistory.Id;
            this.ProblemId = ProblemHistory.ProblemId;
            this.Time = ProblemHistory.Time;
            this.ModifierId = ProblemHistory.ModifierId;
            this.ProblemStatusId = ProblemHistory.ProblemStatusId;
            this.Modifier = ProblemHistory.Modifier == null ? null : new MobileSync_AppUserDTO(ProblemHistory.Modifier);
            this.ProblemStatus = ProblemHistory.ProblemStatus == null ? null : new MobileSync_ProblemStatusDTO(ProblemHistory.ProblemStatus);
        }
    }

    public class MobileSync_ProblemHistoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ProblemId { get; set; }
        public DateFilter Time { get; set; }
        public IdFilter ModifierId { get; set; }
        public IdFilter ProblemStatusId { get; set; }
        public ProblemHistoryOrder OrderBy { get; set; }
    }
}
