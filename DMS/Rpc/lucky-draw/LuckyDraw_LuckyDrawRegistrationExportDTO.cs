using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawRegistrationExportDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public decimal Revenue { get; set; }
        public long TurnCounter { get; set; }
        public long RemainingTurnCounter { get; set; }
        public bool IsDrawnByStore { get; set; }
        public string IsDrawnByStoreString { get; set; }
        public DateTime Time { get; set; }
        public LuckyDraw_AppUserDTO AppUser { get; set; }
        public LuckyDraw_LuckyDrawDTO LuckyDraw { get; set; }
        public LuckyDraw_StoreDTO Store { get; set; }
        public List<LuckyDraw_LuckyDrawWinnerExportDTO> LuckyDrawWinners { get; set; }
        public LuckyDraw_LuckyDrawRegistrationExportDTO() {}
        public LuckyDraw_LuckyDrawRegistrationExportDTO(LuckyDrawRegistration LuckyDrawRegistration)
        {
            this.Id = LuckyDrawRegistration.Id;
            this.LuckyDrawId = LuckyDrawRegistration.LuckyDrawId;
            this.AppUserId = LuckyDrawRegistration.AppUserId;
            this.StoreId = LuckyDrawRegistration.StoreId;
            this.Revenue = LuckyDrawRegistration.Revenue;
            this.TurnCounter = LuckyDrawRegistration.TurnCounter;
            this.RemainingTurnCounter = LuckyDrawRegistration.RemainingTurnCounter;
            this.IsDrawnByStore = LuckyDrawRegistration.IsDrawnByStore;
            this.Time = LuckyDrawRegistration.Time;
            this.AppUser = LuckyDrawRegistration.AppUser == null ? null : new LuckyDraw_AppUserDTO(LuckyDrawRegistration.AppUser);
            this.LuckyDraw = LuckyDrawRegistration.LuckyDraw == null ? null : new LuckyDraw_LuckyDrawDTO(LuckyDrawRegistration.LuckyDraw);
            this.Store = LuckyDrawRegistration.Store == null ? null : new LuckyDraw_StoreDTO(LuckyDrawRegistration.Store);
            this.Informations = LuckyDrawRegistration.Informations;
            this.Warnings = LuckyDrawRegistration.Warnings;
            this.Errors = LuckyDrawRegistration.Errors;
        }
    }
}
