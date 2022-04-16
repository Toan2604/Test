using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawWinnerExportDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long? LuckyDrawNumberId { get; set; }
        public long? LuckyDrawStructureId { get; set; }
        public long LuckyDrawRegistrationId { get; set; }
        public string IsDrawnByStoreString { get; set; }
        public decimal Revenue { get; set; }
        public LuckyDraw_AppUserDTO AppUser { get; set; }   
        public LuckyDraw_LuckyDrawDTO LuckyDraw { get; set; }   
        public LuckyDraw_StoreDTO Store { get; set; }   
        public LuckyDraw_LuckyDrawStructureDTO LuckyDrawStructure { get; set; }   
        public LuckyDraw_LuckyDrawNumberDTO LuckyDrawNumber { get; set; }   
        public LuckyDraw_LuckyDrawRegistrationDTO LuckyDrawRegistration { get; set; }   
        public Guid RowId { get; set; }
        public DateTime Time { get; set; }
        public string TimeString { get; set; }
        public LuckyDraw_LuckyDrawWinnerExportDTO() {}
        public LuckyDraw_LuckyDrawWinnerExportDTO(LuckyDrawWinner LuckyDrawWinner)
        {
            this.Id = LuckyDrawWinner.Id;
            this.LuckyDrawId = LuckyDrawWinner.LuckyDrawId;
            this.LuckyDrawStructureId = LuckyDrawWinner.LuckyDrawStructureId;
            this.LuckyDrawRegistrationId = LuckyDrawWinner.LuckyDrawRegistrationId;
            this.LuckyDrawNumberId = LuckyDrawWinner.LuckyDrawNumberId;
            this.LuckyDraw = LuckyDrawWinner.LuckyDraw == null ? null : new LuckyDraw_LuckyDrawDTO(LuckyDrawWinner.LuckyDraw);
            this.LuckyDrawStructure = LuckyDrawWinner.LuckyDrawStructure == null ? null : new LuckyDraw_LuckyDrawStructureDTO(LuckyDrawWinner.LuckyDrawStructure);
            this.LuckyDrawRegistration = LuckyDrawWinner.LuckyDrawRegistration == null ? null : new LuckyDraw_LuckyDrawRegistrationDTO(LuckyDrawWinner.LuckyDrawRegistration);
            this.LuckyDrawNumber = LuckyDrawWinner.LuckyDrawNumber == null ? null : new LuckyDraw_LuckyDrawNumberDTO(LuckyDrawWinner.LuckyDrawNumber);
            this.RowId = LuckyDrawWinner.RowId;
            this.Errors = LuckyDrawWinner.Errors;
            this.Time = LuckyDrawWinner.Time;
        }
    }
}