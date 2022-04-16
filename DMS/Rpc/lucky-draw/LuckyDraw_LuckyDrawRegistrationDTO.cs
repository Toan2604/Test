using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawRegistrationDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public decimal Revenue { get; set; }
        public long TurnCounter { get; set; }
        public long UsedTurnCounter { get; set; }
        public bool IsDrawnByStore { get; set; }
        public DateTime Time { get; set; }
        public LuckyDraw_AppUserDTO AppUser { get; set; }
        public LuckyDraw_LuckyDrawDTO LuckyDraw { get; set; }
        public LuckyDraw_StoreDTO Store { get; set; }
        public LuckyDraw_LuckyDrawRegistrationDTO() {}
        public LuckyDraw_LuckyDrawRegistrationDTO(LuckyDrawRegistration LuckyDrawRegistration)
        {
            this.Id = LuckyDrawRegistration.Id;
            this.LuckyDrawId = LuckyDrawRegistration.LuckyDrawId;
            this.AppUserId = LuckyDrawRegistration.AppUserId;
            this.StoreId = LuckyDrawRegistration.StoreId;
            this.Revenue = LuckyDrawRegistration.Revenue;
            this.TurnCounter = LuckyDrawRegistration.TurnCounter;
            this.UsedTurnCounter = LuckyDrawRegistration.UsedTurnCounter;
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

    public class LuckyDraw_LuckyDrawRegistrationfilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public DecimalFilter Revenue { get; set; }
        public LongFilter TurnCounter { get; set; }
        public DateFilter Time { get; set; }
        public LuckyDrawRegistrationOrder OrderBy { get; set; }
    }
}
