using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawWinnerDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long? LuckyDrawStructureId { get; set; }
        public long LuckyDrawRegistrationId { get; set; }
        public long? LuckyDrawNumberId { get; set; }
        public DateTime Time { get; set; }
        public GeneralMobile_LuckyDrawRegistrationDTO LuckyDrawRegistration { get; set; }
        public GeneralMobile_LuckyDrawStructureDTO LuckyDrawStructure { get; set; }
        public GeneralMobile_LuckyDrawNumberDTO LuckyDrawNumber { get; set; }
        public Guid RowId { get; set; }
        public GeneralMobile_LuckyDrawWinnerDTO() { }
        public GeneralMobile_LuckyDrawWinnerDTO(LuckyDrawWinner LuckyDrawWinner)
        {
            this.Id = LuckyDrawWinner.Id;
            this.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
            this.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
            this.LuckyDrawRegistrationId = LuckyDrawWinner.LuckyDrawRegistrationId;
            this.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
            this.Time = LuckyDrawWinner.Time;
            this.LuckyDrawRegistration = LuckyDrawWinner.LuckyDrawRegistration == null ? null : new GeneralMobile_LuckyDrawRegistrationDTO(LuckyDrawWinner.LuckyDrawRegistration);
            this.LuckyDrawStructure = LuckyDrawWinner.LuckyDrawStructure == null ? null : new GeneralMobile_LuckyDrawStructureDTO(LuckyDrawWinner.LuckyDrawStructure);
            this.LuckyDrawNumber = LuckyDrawWinner.LuckyDrawNumber == null ? null : new GeneralMobile_LuckyDrawNumberDTO(LuckyDrawWinner.LuckyDrawNumber);
            this.RowId = LuckyDrawWinner.RowId;
            this.Errors = LuckyDrawWinner.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawWinnerFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter LuckyDrawStructureId { get; set; }
        public IdFilter LuckyDrawRegistrationId { get; set; }
        public DateFilter Time { get; set; }
        public LuckyDrawWinnerOrder OrderBy { get; set; }
    }
}