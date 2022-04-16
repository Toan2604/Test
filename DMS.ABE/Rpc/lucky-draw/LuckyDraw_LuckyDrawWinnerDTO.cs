using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawWinnerDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long? LuckyDrawStructureId { get; set; }
        public long LuckyDrawRegistrationId { get; set; }
        public long? LuckyDrawNumberId { get; set; }
        public LuckyDraw_LuckyDrawStructureDTO LuckyDrawStructure { get; set; }
        public LuckyDraw_LuckyDrawRegistrationDTO LuckyDrawRegistration { get; set; }
        public LuckyDraw_LuckyDrawNumberDTO LuckyDrawNumber { get; set; }
        public Guid RowId { get; set; }
        public DateTime Time { get; set; }
        public LuckyDraw_LuckyDrawWinnerDTO() {}
        public LuckyDraw_LuckyDrawWinnerDTO(LuckyDrawWinner LuckyDrawWinner)
        {
            this.Id = LuckyDrawWinner.Id;
            this.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
            this.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
            this.LuckyDrawRegistrationId = LuckyDrawWinner.LuckyDrawRegistrationId;
            this.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
            this.Time = LuckyDrawWinner.Time;
            this.LuckyDrawStructure = LuckyDrawWinner.LuckyDrawStructure == null ? null : new LuckyDraw_LuckyDrawStructureDTO(LuckyDrawWinner.LuckyDrawStructure);
            this.LuckyDrawRegistration = LuckyDrawWinner.LuckyDrawRegistration == null ? null : new LuckyDraw_LuckyDrawRegistrationDTO(LuckyDrawWinner.LuckyDrawRegistration);
            this.LuckyDrawNumber = LuckyDrawWinner.LuckyDrawNumber == null ? null : new LuckyDraw_LuckyDrawNumberDTO(LuckyDrawWinner.LuckyDrawNumber);
            this.RowId = LuckyDrawWinner.RowId;
            this.Errors = LuckyDrawWinner.Errors;
        }
    }

    public class LuckyDraw_LuckyDrawWinnerFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }        
        public IdFilter LuckyDrawId { get; set; }        
        public IdFilter LuckyDrawStructureId { get; set; }        
        public IdFilter LuckyDrawNumberId { get; set; }        
        public IdFilter AppUserId { get; set; }  
        public LuckyDrawWinnerOrder OrderBy { get; set; }
    }
}