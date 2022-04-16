using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDraw_LuckyDrawWinnerDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long? LuckyDrawStructureId { get; set; }
        public long LuckyDrawRegistrationId { get; set; }
        public long? LuckyDrawNumberId { get; set; }
        public WebLuckyDraw_LuckyDrawStructureDTO LuckyDrawStructure { get; set; }
        public WebLuckyDraw_LuckyDrawRegistrationDTO LuckyDrawRegistration { get; set; }
        public WebLuckyDraw_LuckyDrawNumberDTO LuckyDrawNumber { get; set; }
        public Guid RowId { get; set; }
        public DateTime Time { get; set; }
        public WebLuckyDraw_LuckyDrawWinnerDTO() {}
        public WebLuckyDraw_LuckyDrawWinnerDTO(LuckyDrawWinner LuckyDrawWinner)
        {
            this.Id = LuckyDrawWinner.Id;
            this.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
            this.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
            this.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
            this.Time = LuckyDrawWinner.Time;
            this.LuckyDrawStructure = LuckyDrawWinner.LuckyDrawStructure == null ? null : new WebLuckyDraw_LuckyDrawStructureDTO(LuckyDrawWinner.LuckyDrawStructure);
            this.LuckyDrawRegistration = LuckyDrawWinner.LuckyDrawRegistration == null ? null : new WebLuckyDraw_LuckyDrawRegistrationDTO(LuckyDrawWinner.LuckyDrawRegistration);
            this.LuckyDrawNumber = LuckyDrawWinner.LuckyDrawNumber == null ? null : new WebLuckyDraw_LuckyDrawNumberDTO(LuckyDrawWinner.LuckyDrawNumber);
            this.RowId = LuckyDrawWinner.RowId;
            this.Errors = LuckyDrawWinner.Errors;
        }
    }

    public class WebLuckyDraw_LuckyDrawWinnerFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }        
        public IdFilter LuckyDrawId { get; set; }        
        public IdFilter LuckyDrawStructureId { get; set; }        
        public IdFilter AppUserId { get; set; }        
        public LuckyDrawWinnerOrder OrderBy { get; set; }
    }
}